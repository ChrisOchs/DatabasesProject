<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddPatient.aspx.cs" Inherits="WebApplication1.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Size="X-Large" 
        Text="Add New Patient"></asp:Label>

    <p />
        <asp:Label ID="lblResult" runat="server"></asp:Label>
    <p />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="First Name: "></asp:Label>
&nbsp;<asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
    <p />
        <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Last Name: "></asp:Label>
&nbsp;<asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
    <p />
        <asp:Label ID="Label4" runat="server" Font-Bold="True" 
            Text="Social Security #:"></asp:Label>
&nbsp;<asp:TextBox ID="txtSSN1" runat="server" Width="50px"></asp:TextBox>-
        <asp:TextBox ID="txtSSN2" runat="server" Width="39px"></asp:TextBox>-
        <asp:TextBox ID="txtSSN3" runat="server" Width="52px"></asp:TextBox>
    <p />
        <asp:Label ID="lblAddress" runat="server" Font-Bold="True" Text="Address: "></asp:Label>
&nbsp;<asp:TextBox ID="txtAddress" runat="server" Width="318px"></asp:TextBox>
    <p />
        <asp:Label ID="lblAddress0" runat="server" Font-Bold="True" Text="Phone #:"></asp:Label>
&nbsp;<asp:TextBox ID="txtPhone" runat="server"
            Width="160px"></asp:TextBox>
    <p />
        <asp:Label ID="lblAddress1" runat="server" Font-Bold="True" 
            Text="Date Of Birth: "></asp:Label>
        <asp:TextBox ID="txtDOBMonth" runat="server" Width="33px"></asp:TextBox>/<asp:TextBox 
            ID="txtDay" runat="server" Width="39px"></asp:TextBox>/<asp:TextBox 
            ID="txtDOBYear" runat="server" Width="52px"></asp:TextBox>
    <p />
        <asp:Label ID="lblAddress2" runat="server" Font-Bold="True" Text="Gender:"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlGender" runat="server">
            <asp:ListItem Value="male">Male</asp:ListItem>
            <asp:ListItem Value="female">Female</asp:ListItem>
        </asp:DropDownList>
    <p />
        <asp:Label ID="lblAddress3" runat="server" Font-Bold="True" Text="Blood Type:"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlBloodType1" runat="server">
            <asp:ListItem>A</asp:ListItem>
            <asp:ListItem>B</asp:ListItem>
            <asp:ListItem>O</asp:ListItem>
            <asp:ListItem>AB</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="ddlBloodType2" runat="server">
            <asp:ListItem>+</asp:ListItem>
            <asp:ListItem>-</asp:ListItem>
        </asp:DropDownList>
    <p />
        <asp:Label ID="lblAddress4" runat="server" Font-Bold="True" 
            Text="Primary Physician:"></asp:Label>
        <asp:DropDownList ID="ddlPhysician" runat="server">
        </asp:DropDownList>
    <p class="style1" />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Patient" 
            onclick="btnSubmit_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" 
            Text="Clear" />
</asp:Content>
