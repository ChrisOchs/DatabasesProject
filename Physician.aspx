<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Physician.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <div class="physicianHeading">
        Physician
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <div class="physicianConten">
        <asp:TabContainer ID="TabContainer1" runat="server">
            <asp:TabPanel runat="server" HeaderText="Search" ID="Search">
                <ContentTemplate>
                    <div>
                        <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
                        <asp:TextBox ID="Fname" runat="server" ToolTip="Enter Fist Name" AutoPostBack="true" OnTextChanged="txt_search"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" Text="Last Name"></asp:Label>
                        <asp:TextBox ID="Lname" runat="server" ToolTip="Enter Last Name" AutoPostBack="true" OnTextChanged="txt_search"></asp:TextBox>
                    </div>
                    <div>
                        
                        <asp:Label ID="lblError" runat="server" Visible="false" Text=""></asp:Label>
                        <div> <</div>
                        <asp:GridView ID="PhysEntries" DataKeyNames="employeeid" runat="server"
                        ShowHeaderWhenEmpty="True" AllowPaging="true" AutoGenerateColumns="False" Font-Bold="False"
                        Style="margin-left: 15px; width: 800px" OnPageIndexChanging="Physician_PageIndexChanging"
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
                        PageSize="50" Font-Underline="False" CellPadding="4" ForeColor="White" BackColor="#4b6c9e" ShowFooter="false"
                         OnRowCommand="LoadPhysician">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField HeaderText="First Name">
                                <ItemTemplate>
                                    <asp:Label ID="first_name_col" runat="server" Text='<%# Eval("firstname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <asp:Label ID="last_name_col" runat="server" Text='<%# Eval("lastname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Specialty">
                                <ItemTemplate>
                                    <asp:Label ID="specialty_col" runat="server" Text='<%# Eval("specialty") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button ID="Select_button" runat="server" Text="Select" OnClick="select_Physician"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        
                        </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>

            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="Physician" ID="Physician_tap">
                <ContentTemplate>
                    <asp:Label ID="test" runat="server"></asp:Label>
                
                </ContentTemplate>       
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
</asp:Content>
