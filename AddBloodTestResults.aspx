<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddBloodTestResults.aspx.cs" Inherits="WebApplication1.AddBloodTestResults" %>
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
        Text="Add Blood Test Result For:"></asp:Label>
&nbsp;
    <asp:Label ID="lblPatientName" runat="server" Font-Size="X-Large"></asp:Label>

    <p />

        <asp:Label ID="lblResult" runat="server"></asp:Label>
    <p />

        <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Blood Sugar: "></asp:Label>
        <asp:TextBox ID="txtBloodSugar" runat="server" Width="64px"></asp:TextBox>
    <p />

        <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="HDL: "></asp:Label>
        <asp:TextBox ID="txtHDL" runat="server" Width="64px"></asp:TextBox>
    <p />

        <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="LDL: "></asp:Label>
        <asp:TextBox ID="txtLDL" runat="server" Width="64px"></asp:TextBox>
    <p />

        <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Triglycerides: "></asp:Label>
        <asp:TextBox ID="txtTriglycerides" runat="server" Width="64px"></asp:TextBox>
    <p class="style1" />

        <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" 
            Text="Submit" />
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
            Text="Cancel" />


</asp:Content>
