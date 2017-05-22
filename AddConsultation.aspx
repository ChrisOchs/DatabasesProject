<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddConsultation.aspx.cs" Inherits="WebApplication1.AddConsultation" %>
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
        Text="Add New Consultation Results"></asp:Label>

    <p />
        <strong>Patient Name:</strong>
        <asp:Label ID="lblPatientName" runat="server"></asp:Label>
    <p />
        <strong>Consultation With:</strong>
        <asp:DropDownList ID="ddlPhysicians" runat="server" Height="19px" Width="128px">
        </asp:DropDownList>
    <p />
        <strong>Consultation Date/Time:</strong>
        <asp:Label ID="lblDate" runat="server"></asp:Label>
    <p />
        <strong>Consultation Type:</strong>
        <asp:DropDownList ID="ddlType" runat="server">
            <asp:ListItem>First Visit</asp:ListItem>
            <asp:ListItem>Follow Up</asp:ListItem>
        </asp:DropDownList>
    <p />
        <strong>Subjective, Objective, Assessment and Plan (SOAP) Note</strong><br />
        <asp:TextBox ID="txtSOAP" runat="server" Height="112px" TextMode="MultiLine" 
            Width="520px"></asp:TextBox>
    <p />
        <strong>Illness Diagnosis<asp:Panel ID="pnlIllnessDiagnosis" runat="server">
    </asp:Panel>
    <p>
        Allergy Diagnosis<strong><asp:Panel ID="pnlAllergyDiagnosis" runat="server">
    </asp:Panel>
    <p>
        Prescriptions</strong><asp:Panel ID="pnlPrescriptions" runat="server">
    </asp:Panel>
        </strong>
    <p class="style1" />
        &nbsp;
        <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" 
            Text="Submit" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
            Text="Cancel" />
</asp:Content>
