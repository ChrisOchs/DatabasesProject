<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Medication.aspx.cs" Inherits="WebApplication1.Medication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" 
        Text="Clinic Pharmacy Medications"></asp:Label>

    <br />
    <br />
    <asp:Label ID="medListLbl" runat="server" Font-Size="Large" 
        style="text-align: center" Text="Medication List"></asp:Label>
    <br />
    <asp:Table ID="medicationTbl" runat="server" CellPadding="2" CellSpacing="2" 
        Width="913px">
        <asp:TableRow ID="titleRow" runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Medication Name</asp:TableCell>
            <asp:TableCell runat="server" Font-Bold="True">Unit Cost</asp:TableCell>
            <asp:TableCell runat="server" Font-Bold="True">Year To Date Usage</asp:TableCell>
            <asp:TableCell runat="server" Font-Bold="True">Amount Ordered</asp:TableCell>
            <asp:TableCell runat="server" Font-Bold="True">Manufactuer</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <asp:Label ID="medInfoLbl" runat="server" Font-Size="Large" 
        style="text-align: center" Text="Medication Info" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Label ID="medNameTitleLbl" runat="server" Font-Bold="True" Visible="False">Medication Name: </asp:Label>
    <asp:Label ID="medNameLbl" runat="server" Visible="False" Font-Size="Large"></asp:Label>
    <br />
    <asp:Label ID="medCostTitleLbl" runat="server" Font-Bold="True" Visible="False">Medication Cost: $</asp:Label>
    <asp:Label ID="medCostLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <asp:Label ID="medUsageTitleLbl" runat="server" Font-Bold="True" 
        Visible="False">Year to Date Usage: </asp:Label>
    <asp:Label ID="medUsageLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <asp:Label ID="medOrderQtyTitleLbl" runat="server" Font-Bold="True" 
        Visible="False">Quantity Ordered: </asp:Label>
    <asp:Label ID="medOrderLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <asp:Label ID="medMfgTitleLbl" runat="server" Font-Bold="True" Visible="False">Manufacturer: </asp:Label>
    <asp:Label ID="medMfgLbl" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Label ID="medReactionLbl" runat="server" Font-Size="Large" 
        style="text-align: center" Text="Medication Reactions" Visible="False"></asp:Label>
    <br />
    <asp:Table ID="reactionTbl" runat="server" CellPadding="2" CellSpacing="2" 
        Width="593px" Visible="False">
        <asp:TableRow ID="titleRow0" runat="server">
            <asp:TableCell runat="server" Font-Bold="True">Medication Name</asp:TableCell>
            <asp:TableCell runat="server" Font-Bold="True">Severity</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <br />

</asp:Content>
