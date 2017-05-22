<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNurseToSchedule.aspx.cs" Inherits="WebApplication1.AddNurseToSchedule" %>
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
        Text="Add Nurse To Schedule"></asp:Label>
    <p />
    
        <asp:Label ID="Label2" runat="server" Text="Add Nurse For:"></asp:Label>
&nbsp;&nbsp;
        <asp:Label ID="lblPatientName" runat="server"></asp:Label>
        <br />
        <asp:Label ID="Label3" runat="server" Text="On Date:"></asp:Label>
&nbsp;&nbsp;
        <asp:Label ID="lblDate" runat="server"></asp:Label>
    <p />
    
        <asp:Label ID="Label4" runat="server" Text="Nurse:"></asp:Label>
&nbsp;&nbsp;
        <asp:DropDownList ID="ddlNurseList" runat="server" Height="34px" Width="208px">
        </asp:DropDownList>
    <p />
    
        <asp:Label ID="Label5" runat="server" Text="Shift: "></asp:Label>
        <asp:DropDownList ID="ddlShift" runat="server">
            <asp:ListItem Value="day">Day</asp:ListItem>
            <asp:ListItem Value="night">Night</asp:ListItem>
        </asp:DropDownList>
    <p class="style1" />
    
        <asp:Button ID="btnSubmit" runat="server" 
            Text="Submit" onclick="btnSubmit_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
            Text="Cancel" />

    
</asp:Content>
