<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Surgery.aspx.cs" Inherits="WebApplication1.Surgery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 394px;
            font-weight: 700;
        }
        .style2
        {
            width: 394px;
            font-weight: 700;
            height: 39px;
        }
        .style3
        {
            height: 39px;
        }
        .style4
        {
            width: 394px;
            font-weight: 700;
            height: 77px;
        }
        .style5
        {
            height: 77px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" 
        Text="Surgery Details"></asp:Label>

    <p />
        <table style="width: 100%; margin-top: 63px;">
            <tr>
                <td class="style2">
                    Patient Name:&nbsp;
                    <asp:HyperLink ID="patientLnk" runat="server" Font-Size="Large">[patientLnk]</asp:HyperLink>
                </td>
                <td class="style3">
                    <strong>Surgery Type:&nbsp;
                    </strong>
                    <asp:Label ID="typeLabel" runat="server" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style4" valign="top">
                    Peforming Surgeon(s)<asp:Panel ID="surgeonPanel" runat="server">
                    </asp:Panel>
                </td>
                <td valign="top" class="style5">
                    <strong>Surgery Date:&nbsp;
                    </strong>
                    <asp:Label ID="dateLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style1" valign = "top">
                    Attening Nurse(s)<asp:Panel ID="nursePanel" runat="server">
                    </asp:Panel>
                </td>
                <td valign="top">
                    <strong>Operating Theater:&nbsp;
                    </strong>
                    <asp:Label ID="theaterLabel" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

</asp:Content>
