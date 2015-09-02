<%@ Page Title="Builder Portal - Reports" Language="C#" MasterPageFile="~/master/HIP.Master" AutoEventWireup="true" 
    CodeBehind="Builder_Reports.aspx.cs" Inherits="HomeOwner.app.Builder_Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<h2>Reports</h2>

<div id="reportLinks" class="pad left indent" >

    <asp:HyperLink ID="linkSummary" runat="server">Service Request Summary</asp:HyperLink>
    <br />
     <asp:HyperLink ID="linkWorkOrderSummary" runat="server">Work Order Summary</asp:HyperLink>
    <br />
     <asp:HyperLink ID="linkHomeownerUsage" Visible="false" runat="server">Homeowner Usage Report</asp:HyperLink>
    <br />

    <asp:HyperLink ID="linkDetails" runat="server" Target="_blank" Visible="false" >Request Details</asp:HyperLink>
    <br />
    <asp:HyperLink ID="linkTradePerformance" runat="server" Target="_blank" Visible="false">Trade Performance</asp:HyperLink>
    <br />
    <asp:HyperLink ID="linkProductIssues" runat="server" Target="_blank" Visible="false">Issues By Product</asp:HyperLink>
    <br />
 
    
</div>

<div class="wr-clearHack" style="margin-top: 5px">.</div>  
</asp:Content>
