<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cholesterol.aspx.cs" Inherits="WebApplication1.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" 
        Text="Cholesterol Research"></asp:Label>

    <p />
                        
        <asp:Table ID="researchResultsTbl" runat="server" CellPadding="2" 
            CellSpacing="2" Width="913px">
            <asp:TableRow ID="titleRow" runat="server">
                <asp:TableCell runat="server" Font-Bold="True">Patient Name</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Test Date</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">HDL</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">LDL</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Triglycerides</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Total Cholesterol</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Total Cholesterol / LDL</asp:TableCell>
                <asp:TableCell runat="server" Font-Bold="True">Risk Factor</asp:TableCell>
            </asp:TableRow>
        </asp:Table>

                        
</asp:Content>
