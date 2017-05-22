<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Patient.aspx.cs" Inherits="WebApplication1.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            height: 73px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:Table ID="tblContent" runat="server" Width="100%">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">
                <asp:Label runat="server" Font-Size="X-Large" Text="PATIENT MANAGEMENT"/>
                <p />

                <asp:HyperLink runat="server" Text="Add New Patient" NavigateUrl="/AddPatient.aspx"/>
            </asp:TableCell></asp:TableRow></asp:Table></asp:Content>