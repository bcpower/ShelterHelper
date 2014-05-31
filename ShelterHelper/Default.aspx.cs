using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio;

namespace ShelterHelper
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var inventory = GetInventory();

                RepeatAnimals.DataSource = inventory;
                RepeatAnimals.DataBind();

                ColorSelector.DataSource = GetColors(inventory);
                ColorSelector.DataBind();

                List<string> gender = new List<string>();
                gender.Add("Any");
                gender.Add("Female");
                gender.Add("Male");

                GenderDropdown.DataSource = gender;
                GenderDropdown.DataBind();

                AnimalBreed.DataSource = GetBreed(inventory);
                AnimalBreed.DataBind();
            }
        }

        protected void SendAddressButton_Click(object sender, EventArgs e)
        {
            var address = ((Button)sender).CommandArgument;
            var convertedAddress = "http://maps.google.com/?q=" + address.Replace(" ", "+");
            var client = new TwilioRestClient("YOURS", "YOURS");
        
            client.SendSmsMessage("YOURS", PhoneNumber.Text.Replace("(", "").Replace(")", "").Replace("-", ""), "ShelterHelper: " + convertedAddress);
            ScriptManager.RegisterClientScriptBlock(UpdateScripts, typeof(string), "showModal", "$('#animalModal').modal('hide'); alert('You are awesome, thank you!'); window.location=window.location;", true);
        }

        protected void FilterAnimalsNow(object sender, EventArgs e)
        {
            var inventory = GetInventory();
            var filteredInventory = new List<AnimalInventory>();

            foreach (var item in inventory)
                filteredInventory.Add(item);

            // breed filter
            if (AnimalBreed.SelectedItem.Text != "Any")
            {
                foreach (var animal in inventory)
                {
                    if (animal.Breed.ToLower() != AnimalBreed.SelectedItem.Text.ToLower())
                    {
                        filteredInventory.RemoveAll(p => p.AnimalID == animal.AnimalID);
                    }
                }
            }

            // color filters
            if (ColorSelector.SelectedItem.Text != "Any")
            {
                foreach (var animal in inventory)
                {
                    if (!animal.Primary_Color.ToLower().Contains(ColorSelector.SelectedItem.Text.ToLower())) {
                        filteredInventory.RemoveAll(p => p.AnimalID == animal.AnimalID);
                    }
                }
            }

            // gender filter
            if (GenderDropdown.SelectedItem.Text != "Any")
            {
                foreach (var animal in inventory)
                {
                    if (!animal.Sex.ToLower().Contains(GenderDropdown.SelectedItem.Text.ToLower())) {
                        filteredInventory.RemoveAll(p => p.AnimalID == animal.AnimalID);
                    }
                }
            }

            if (AnimalFixed.Checked)
            {
                foreach (var animal in inventory)
                {
                    if (animal.Sex.ToLower().Contains("spayed") || animal.Sex.ToLower().Contains("neutered"))
                    {
                        // do nothing
                    }
                    else
                    {
                        filteredInventory.RemoveAll(p => p.AnimalID == animal.AnimalID);
                    }
                }
            }

            RepeatAnimals.DataSource = filteredInventory;
            RepeatAnimals.DataBind();

            ResultText.Text = "Showing " + filteredInventory.Count.ToString() + " Pet(s).";
        }

        private List<AnimalInventory> GetInventory()
        {
            List<AnimalInventory> inventory = new List<AnimalInventory>();

            using (var db = new AwesomePetsDataContext())
            {
                inventory = db.AnimalInventories.Where(p => p.Kennel_Status == "available").OrderBy(p => p.Intake_Date).ToList();
            }

            return inventory;
        }

        private List<string> GetColors(List<AnimalInventory> inventory)
        {
            List<string> result = new List<string>();

            result.Add("Any");

            foreach (var animal in inventory.OrderBy(p => p.Primary_Color))
            {
                if (!string.IsNullOrEmpty(animal.Primary_Color.Trim()) && !result.Contains(animal.Primary_Color.ToLower()))
                {
                    result.Add(animal.Primary_Color.ToLower());
                }
            }

            return result;
        }

        private List<string> GetBreed(List<AnimalInventory> inventory)
        {
            List<string> result = new List<string>();

            result.Add("Any");

            foreach (var animal in inventory.OrderBy(p => p.Breed))
            {
                if (!string.IsNullOrEmpty(animal.Breed.Trim()) && !result.Contains(animal.Breed.ToLower()))
                {
                    result.Add(animal.Breed.ToLower());
                }
            }

            return result;
        }

        protected void AdoptMeButton_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;
            var animal = GetInventory().Where(p => p.AnimalID == id).FirstOrDefault();

            AnimalName.Text = animal.Name;
            AnimalAddress.Text = animal.Kennel_Number;
            SendAddressButton.CommandArgument = animal.Kennel_Number + " Louisville, KY";

            string addressUrl = "https://www.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=" + animal.Kennel_Number.Replace(" ", "+") + "+louisville+ky&output=embed";
            
            ScriptManager.RegisterClientScriptBlock(UpdateScripts, typeof(string), "showModal", "$('#animalModal').modal('show'); $('#embeddedMap').attr('src', '" + addressUrl + "');", true);
            // adopt the mofo
        }

        protected void RepeatAnimals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dataSource = e.Item.DataItem as AnimalInventory;

                LinkButton adoptMeButton = e.Item.FindControl("AdoptMeButton") as LinkButton;

                adoptMeButton.CommandArgument = dataSource.AnimalID;
            }
        }
    }
}