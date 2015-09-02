<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ProjectDetails.aspx.cs" Title="Builder Portal - Project Details"  
    Inherits="HomeOwner.ProjectDetails" Theme="BuilderDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
</head>
<body>

    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <div id="wrap" style="width: 770px">
     <div id="wrap_border">    
     
        <div id="divHipLogo" align="left">
            <asp:HyperLink ID="lnkLobby" runat="server">
                <asp:Image ID="imgHipLogo" runat="server" SkinID="hipLogoNew" />
            </asp:HyperLink>
        </div>
     
        <asp:UpdatePanel ID="upHomeDetails" runat="server">
            <ContentTemplate>
            
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upHomeDetails"
                DisplayAfter="250">
                <ProgressTemplate>
                    <div id="divProgressPopup" class="progressIndicator">
                        <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                        Please Wait...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                
      
            <span id="spanHomesLink" runat="server" class="right" style="width: 222px;">
                 <asp:HyperLink ID="lnkViewHomes" runat="server" Target="_blank" SkinID="building"></asp:HyperLink>
                 View all Properties in this Project             
            </span><br />    <div class="wr-clearHack">.</div> 
            <span id="span1" runat="server" class="right" style="width: 222px;">
                <asp:HyperLink ID="lnkViewProducts" runat="server" Target="_blank" SkinID="BuildingProductsIcon"></asp:HyperLink>
                View all Products in this Project
            </span>  <br />    <div class="wr-clearHack">.</div> 
            <span id="spanlnkRequests" runat="server" class="right" style="width: 222px;">
               <asp:HyperLink ID="lnkViewRequests" runat="server" Target="_blank" SkinID="requestIcon"></asp:HyperLink>
               View any Requests for this Project
            </span> 
             
           <h2>
            Project Details</h2> 
                                
             
                <div class="wr-clearHack">.</div> 
                              
                <asp:Panel ID="pnlDetails" runat="server">
                            <br />             
                            <asp:Panel ID="pnlProjectInformation" runat="server">
                                <div class="box2 pad" align="center">
                                    <div class="important bold left">
                                        <asp:Literal ID="litProjectName" runat="server" /> 
                                    </div>                                   
                                    <div class="wr-clearHack">.</div>
                                </div>
                                <div class="wr-clearHack">.</div>
                                
                                <div align="center" class="pad">
                                    <div style="width: 640px;display: table-cell;">
                                        <asp:Image ID="imgProjectBanner" runat="server" SkinID="projectLogo" AlternateText="Header"
                                            alt="Project Banner" />
                                    </div>
                                </div>
                                
                                <div class="topDblSpc"> 
                                    <ul class="ulHorz">
                                        <li>
                                            <h4>Province, City</h4>
                                            <asp:Literal ID="litProvince" runat="server" />
                                        </li>
                                        <li>
                                            <h4>Address</h4>
                                            <asp:Literal ID="litAddress" runat="server" />
                                        </li>
                                        <li>
                                            <h4>Legal Description</h4>
                                            <asp:Literal ID="litLegalDescription" runat="server" />
                                        </li>                                        
                                    </ul>
                                </div>                                                                                            
                                                                                               
                                <div class="wr-clearHack" style="margin-top: 15px">.</div>
                                <div style="padding-left: 40px;" align="left">
                                    <h5>
                                        Configuration</h5>
                                    <table style="width: 700px;">
                                        <tr style="width: 700px;">
                                            <td style="width: 200px;">
                                                Property Warranty
                                            </td>
                                            <td style="width: 500px;">
                                                <asp:DropDownList ID="ddlHomeWarrantyPlan" runat="server" Width="360px" AppendDataBoundItems="true"  
                                                    DataSourceID="dsHomeWarranties" DataTextField="Name" DataValueField="HomeWarrantyID">
                                                    <asp:ListItem Text="<< Not specified >>" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Maintenance Guide
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMaintenancePlan" runat="server" Width="360px" AppendDataBoundItems="true"
                                                    DataSourceID="dsMaintenancePlans" DataTextField="Name" DataValueField="MaintenancePlanID">
                                                    <asp:ListItem Text="<< Not specified >>" Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Email Reminders <br />(for Registered Owners)
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkMaintenanceReminders" runat="server" Text="Maintenance Due" /> <br />
                                                <asp:CheckBox ID="chkHomeWarrantyReminders" runat="server" Text="Property Warranty Milestones" /> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="cmdSaveConfiguration" runat="server" Text="Save" OnClick="SaveConfiguration" />
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </div>
                                                               
                                
                                <div class="wr-clearHack">.</div>
                            </asp:Panel>
                                          
                </asp:Panel>
                
                <div class="wr-clearHack" style="margin-top: 9px">.</div>                    
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </div>
    
    <asp:LinqDataSource ID="dsHomeWarranties" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext" 
        EnableDelete="false" EnableInsert="false" EnableUpdate="false" TableName="HomeWarranties" OrderBy="Name"></asp:LinqDataSource>
        
    <asp:LinqDataSource ID="dsMaintenancePlans" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext" 
        EnableDelete="false" EnableInsert="false" EnableUpdate="false" TableName="MaintenancePlans" OrderBy="Name"></asp:LinqDataSource>
    
    </form>
    
</body>
</html>
