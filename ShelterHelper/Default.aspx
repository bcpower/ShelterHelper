<%@ Page Title="Adopt an Animal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ShelterHelper._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Louisville : <%: Title %></h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server" ID="UpdateFilters">
        <ContentTemplate>
            <asp:DropDownList ID="ColorSelector" AutoPostBack="true" OnSelectedIndexChanged="FilterAnimalsNow" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="GenderDropdown" AutoPostBack="true" OnSelectedIndexChanged="FilterAnimalsNow" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="AnimalBreed" AutoPostBack="true" OnSelectedIndexChanged="FilterAnimalsNow" runat="server"></asp:DropDownList>
            <asp:CheckBox AutoPostBack="true" OnCheckedChanged="FilterAnimalsNow" ID="AnimalFixed" runat="server" Text="Already Fixed" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="UpdateContent">
        <ContentTemplate>
            <asp:Label ID="ResultText" Text="Showing All Pets" runat="server" Font-Bold="true"></asp:Label>
            <asp:Repeater ID="RepeatAnimals" OnItemDataBound="RepeatAnimals_ItemDataBound" runat="server">
                <HeaderTemplate>
                    <table class="table">
                        <tr>
                            <th>Admitted Date</th>
                            <th>Type</th>
                            <th>Sex</th>
                            <th>Size</th>
                            <th>Color</th>
                            <th>Breed</th>
                            <th>Name</th>
                            <th>Age</th>
                            <th>Adopt Me</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("Intake_Date") %>
                        </td>
                        <td>
                            <%# Eval("Animal_Type") %>
                        </td>
                        <td>
                            <%# Eval("sex") %>
                        </td>
                        <td>
                            <%# Eval("size") %>
                        </td>
                        <td>
                            <%# Eval("primary_color") %>
                        </td>
                        <td>
                            <%# Eval("breed") %>
                        </td>
                        <td>
                            <%# Eval("name") %>
                        </td>
                        <td>
                            <%# Eval("estimated_age") %>
                        </td>
                        <td>
                            <asp:LinkButton ID="AdoptMeButton" Text="Adopt Me" runat="server" OnClick="AdoptMeButton_Click" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdateScripts" runat="server">
        <ContentTemplate>
            <div class="modal fade" id="animalModal" tabindex="-1" role="dialog" aria-labelledby="adoptionLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="adoptionLabel">Adopt
                                <asp:Label ID="AnimalName" runat="server"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            You are the human! Here is all the information you need to adopt this pet:
                            <br />
                            <br />
                            <h4>Text Me The Address:</h4>
                            <div class="row">
                                <div class="col-xs-8">
                                    <asp:TextBox ID="PhoneNumber" runat="server" placeholder="502-xxx-xxxx"></asp:TextBox>
                                </div>
                                <div class="col-xs-4">
                                    <asp:Button ID="SendAddressButton" OnClick="SendAddressButton_Click" runat="server" Text="Send Address" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-4">
                                    <strong>Address:</strong>
                                </div>
                                <div class="col-xs-8">
                                    <asp:Label ID="AnimalAddress" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <iframe width="425" height="350" frameborder="0" id="embeddedMap" scrolling="no" marginheight="0" marginwidth="0" src=""></iframe>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
