<%@ Page Title="Builder Portal - Maintenance Alert History" Language="C#" MasterPageFile="~/master/HIP.Master"
    AutoEventWireup="True" CodeBehind="Builder_MaintenanceAlerts.aspx.cs" Inherits="HomeOwner.app.Builder_MaintenanceAlerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

    <h2>
        Maintenance Advisor Email History
    </h2>

        
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
    <ContentTemplate>
           
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="250">                           
        <ProgressTemplate>
            <div id="divProgressPopup" class="progressIndicator">                          
                <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" /> Please Wait...
            </div>
        </ProgressTemplate>
        </asp:UpdateProgress>       
        
        
        <div class="left medtext" style="width: 550px; padding-bottom: 10px" align="left">
           <div class="left dark bold">Showing: </div>
           <div class="right" style="padding-left: 8px; width: 485px;"><asp:Label ID="lblFilterDisplay" runat="server" Text=""></asp:Label>   </div> 
        <div class="wr-clearHack">.</div>    
              
        </div>     
        <div align="right" class="right pad" style="padding-top: 3px">
            <asp:LinkButton ID="btnShowSearch" runat="server" BorderStyle="None" TabIndex="3"
                onclick="btnShowSearch_Click" Font-Size="1.15em" Font-Bold="True" CssClass="altColor">Search</asp:LinkButton>   
        </div>
        <div align="right" class="right">
            <asp:ImageButton ID="btnShowSearch2" runat="server" SkinID="search" 
                onclick="btnShowSearch_Click"  TabIndex="3"/>          
        </div>    
       
        <div class="wr-clearHack topDblSpc">.</div> 
    
        
        <asp:Panel ID="pnlFilter" class="box2" runat="server" HorizontalAlign="Center" Visible="False" DefaultButton="btnFilter"> 
        <div class="left" style="width: 60px; padding-left: 8px;" align="left">             
            <asp:Button ID="btnClearCriteria" runat="server" Text="Clear" onclick="btnClearCriteria_Click" CausesValidation="false" Font-Size="8pt" TabIndex="2" />
        </div>
        <h3 style="padding: 0px; margin: 0px; padding-left: 235px;" align="left">Search for Building sent Alerts</h3>   
        <div class="wr-clearHack">.</div>
        
        <div align="center">   
            <div style="width: 90%"> 
            
                <div class="left" style="width: 250px; padding-bottom: 10px" align="left">            
                    <h4 style="margin-bottom: 15px">By Project</h4>                   
                    <asp:DropDownList ID="ddlProject" runat="server" Width="250px" AppendDataBoundItems="True">
                    </asp:DropDownList>
                </div>  
                <div class="left" style="width: 250px; padding-bottom: 10px; margin-left: 90px;" align="left">            
                    <h4 style="margin-bottom: 15px">By Unit</h4>  
                    <label style="width: 60px; display:inline-table; display:inline-block;">Unit #: </label>
                    <asp:TextBox ID="txtUnitNumber" runat="server" Width="85px"></asp:TextBox><div style="height: 6px;"></div>                         
                    <label style="width: 60px; display:inline-table; display:inline-block;">Address: </label>
                    <asp:TextBox ID="txtAddress" runat="server" Width="175px" /><div style="height: 6px;"></div>
                </div>  
                <div class="wr-clearHack">.</div> 
                
                <div class="left" style="width: 250px;" align="left"> 
                    <h4 style="margin-bottom: 15px">By User</h4>                                                    
                   
                    <label style="width: 50px; display:inline-table; display:inline-block;">Name: </label>
                    <asp:TextBox ID="txtUserName" runat="server" Width="170px" /><div style="height: 6px;"></div>
                                        
                    <label style="width: 50px; display:inline-table; display:inline-block;">Email: </label>
                    <asp:TextBox ID="txtUserEmail" runat="server" Width="170px" /> <div style="height: 6px;"></div>                        
                </div>              
                   
               
                <div class="left" style="width: 250px;margin-left: 90px;" align="left"> 
                    <h4 style="margin-bottom: 15px">By Sent Date</h4>                       
                  
                    <label style="width: 40px; display:inline-table; display:inline-block;">From: </label>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="80px" ValidationGroup="date">
                    </asp:TextBox>&nbsp;
               
                 
                    <asp:ImageButton ID="btnFromDate" runat="server" SkinID="cal" />
                    <asp:RegularExpressionValidator
                     ID="dateFromValidation" runat="server" ErrorMessage="Not a valid date!"  Display="Dynamic"
                     ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$" 
                     ControlToValidate="txtFromDate" ValidationGroup="date">
                    </asp:RegularExpressionValidator>       
                    
                    <act:CalendarExtender runat="server" 
                        ID="calFromDate"                       
                        TargetControlID="txtFromDate"
                        PopupButtonID="btnFromDate" Format="dd-MMM-yyyy" />  
                    <div style="height: 6px;"></div>    
                    <div class="wr-clearHack">.</div>                                                       
                
                    <label style="width: 40px; display:inline-table; display:inline-block;">To: </label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="80px">
                    </asp:TextBox>&nbsp;
                  
                    <asp:ImageButton ID="btnToDate" runat="server" SkinID="cal" />
                    <asp:RegularExpressionValidator
                         ID="dateToValidation" runat="server" ErrorMessage="Not a valid date!"  Display="Dynamic"
                         ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$" 
                         ControlToValidate="txtToDate" ValidationGroup="date">
                    </asp:RegularExpressionValidator> 
                    <act:CalendarExtender runat="server" 
                        ID="calToDate"                       
                        TargetControlID="txtToDate"
                        PopupButtonID="btnToDate" Format="dd-MMM-yyyy" />  
                        
                </div>
                <div class="wr-clearHack">.</div>
            </div>                
                             
            <div class="wr-clearHack">.</div>
            <div class="right" style="width: 600px;padding-right:30px;" align="right">             
                <asp:Button ID="btnFilter" runat="server" Text="Search" onclick="btnFilter_Click" TabIndex="1" ValidationGroup="date" />
                <div class="wr-clearHack">.</div>
            </div>
      
            
        </div>        
        <div class="wr-clearHack">.</div>  
        </div>
        
        </asp:Panel>  
        <div class="wr-clearHack" style="padding-top: 8px;">.</div> 
                                
        <act:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" TargetControlID="pnlFilter" SkinID="roundCorners">
        </act:RoundedCornersExtender>     
    
               
        <div class="wr-clearHack">.</div> 
        
        <h3>Search Results</h3>
        <asp:GridView ID="gridAlerts" runat="server" SkinID="grid"
            DataKeyNames="UnitMaintenanceAlertID" 
            AllowSorting="true"
            HeaderStyle-HorizontalAlign="Left"
            EmptyDataText="No Alerts found with your criteria" 
            OnRowDataBound="gridAlerts_RowDataBound" 
            onsorting="gridAlerts_Sorting"
            RowStyle-CssClass="line1" >
            <Columns>  
            
                <asp:TemplateField HeaderText="Project/Address" SortExpression="Address" HeaderStyle-Width="225px">
                    <ItemTemplate>
                        <%# Eval("Unit.Phase.Project.ProjectName")%> <br />
                        <div class="topSpc">
                            <asp:Literal ID="litGridRowAddress" runat="server" />
                        </div>
                    </ItemTemplate>               
                </asp:TemplateField>      
            
                <asp:TemplateField HeaderText="User" SortExpression="UserName" HeaderStyle-Width="200px">
                    <ItemTemplate>
                        <asp:Literal ID="litGridRowUser" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Plan Name" SortExpression="PlanName" HeaderStyle-Width="125px">
                    <ItemTemplate>
                         <%# Eval("MaintenancePlanChecklist.Plan.Name")%>
                    </ItemTemplate>               
                </asp:TemplateField>    
                  
               <asp:TemplateField HeaderText="Checklist" SortExpression="Checklist" HeaderStyle-Width="75px">
                    <ItemTemplate>
                         <%# Eval("MaintenancePlanChecklist.Checklist.Name")%>
                    </ItemTemplate>               
                </asp:TemplateField> 
                
                <asp:TemplateField HeaderText="Date Sent" SortExpression="CreatedOn" HeaderStyle-Width="90px" >
                    <ItemTemplate>
                         <%#Eval("CreatedOn", "{0:dd-MMM-yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:TemplateField HeaderText="Viewed" SortExpression="AlertAcknowledged" HeaderStyle-Width="55px" 
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>               
                       <%# Convert.ToBoolean(Eval("AlertAcknowledged")) == true ? "Yes" : "No"%>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
      
      
    
    <div class="wr-clearHack" style="margin-top: 9px">.</div> 
      
    </ContentTemplate>   
    </asp:UpdatePanel>
</asp:Content>
