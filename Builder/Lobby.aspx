<%@ Page Title="Builder Portal - Lobby" Language="C#" MasterPageFile="~/master/HIP.Master"
    AutoEventWireup="True" CodeBehind="Lobby.aspx.cs" Inherits="HomeOwner.app.Builder.Lobby" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
                     
    <asp:UpdatePanel ID="upHomeDetails" runat="server">   
    <ContentTemplate>     
          
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upHomeDetails" DisplayAfter="250">
        <ProgressTemplate>
              <div id="divProgressPopup" class="progressIndicator">                           
                <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" /> Please Wait...
              </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <div>
        <h1><span class="fade">Welcome to the</span>
        <a href="Lobby.aspx">Builder Portal</a></h1>                
    </div>   
    <div class="wr-clearHack">.</div>              
 
    <div style="line-height: 1.1em;">        
                                                 
        <div class="left" style="margin-left: 27px;">
        
          
                      
                                                         
            <asp:Panel ID="panelAlerts" runat="server" Width="640px" CssClass="box2">
            
            <div>
                <asp:Image ID="imgAlert" runat="server" SkinID="alerts" />
                <div style="margin-top: -42px; margin-left: 15px;"><asp:Image ID="imgExcl" runat="server" SkinID="exclamation" Height="30" Width="30" /></div>
            </div>
                
                <asp:GridView ID="gridAlerts" runat="server"
                 Width="99%"
                 DataKeyNames="AlertName"         
                 OnRowDataBound="gridAlerts_RowDataBound"
                 ShowHeader="false"
                 AutoGenerateColumns="false" 
                 CellPadding="2"
                 CellSpacing="12"             
                 BorderColor="#CCCCCC"
                 BorderStyle="None" 
                 BorderWidth="0px"
                 GridLines="None">         
                <Columns>            
                    <asp:TemplateField>
                        <ItemTemplate>                
                            <div style="border-bottom: dashed 0px #D8D8D8;">  
                            
                                <div style="width: 420px; margin-left: 75px; background-color: #FFFFFF; padding: 12px;" class="border medtext"> 
                                
                                    <div class="left" style="margin-top:-3px;">
                                        <asp:HyperLink ID="lnkAlert" runat="server" ImageUrl='<%# Eval("IconImageURL") %>'
                                            Target="_blank" ToolTip='<%# Eval("IconToolTip") %>' NavigateUrl='<%# Eval("LinkURL") %>'>
                                        </asp:HyperLink>
                                    </div> 
                                    <div style="width: 95%;padding-bottom:3px;" class="left">
                                        <div style="padding-left: 3px; margin: 0px; margin-top: -5px;" class="important">
                                        <%# Eval("DisplayTitle") %>
                                        </div>
                                        <asp:Literal ID="litMsg" runat="server"  />
                                    </div>                                                                                                           
                                    <div class="wr-clearHack">.</div>                               
                               </div>                        
                                                                              
                            <div class="wr-clearHack" style="margin-bottom: 5px">.</div> 
                            </div>                    
                            <div class="wr-clearHack">.</div>                 
                        </ItemTemplate>
                    </asp:TemplateField>
                                           
                </Columns>
            </asp:GridView>
            
            <act:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" TargetControlID="panelAlerts" SkinID="roundCorners">
            </act:RoundedCornersExtender>          
                                    
          </asp:Panel>
          
        </div> 
        <div class="wr-clearHack">.</div>     
    </div>
 
                    
    <div class="wr-clearHack" style="padding-top: 23px;">.</div> 
    <h2>
        Projects</h2>                                          
    <div class="wr-clearHack" style="padding-top: 5px;">.</div> 
    
    <asp:GridView ID="gridProjects" runat="server"
         Width="99%" 
         DataKeyNames="ProjectID"         
         OnRowDataBound="gridProjects_RowDataBound"
         ShowHeader="false"
         AutoGenerateColumns="false" 
         CellPadding="2"
         CellSpacing="12"
         BackColor="#ffffff" 
         RowStyle-BackColor="#ffffff"  
         BorderColor="#CCCCCC"
         BorderStyle="None" 
         BorderWidth="0px"
         GridLines="None">         
        <Columns>            
            <asp:TemplateField>
                <ItemTemplate>
                
                <div class="border3">             
                    <div class="box2 pad border2" style="border-top: none; border-left: none; border-right: none;">
                        <div class="left medtext" style="width: 68%">
                            <h3 style="padding: 0px; margin: 0px" class="dark">
                                <asp:HyperLink ID="lnkConfigurationHeaderText" runat="server" />                                
                            </h3>
                        </div>
                        <div class="left smltext" style="width: 32%"align="right">  <asp:Literal ID="litAddress" runat="server" />  </div>  
                        <div class="wr-clearHack">.</div>                                       
                    </div>
                     
                    <div class="wr-clearHack">.</div>               
                    <div class="pad"> 
                    
                        <div align="center">  
                            <div style="width: 640px;" align="right">                                                   
                                <asp:Image ID="imgProjectBanner" runat="server" 
                                    AlternateText="Header" alt="Project Banner" SkinID="projectLogo" ImageAlign="Right" />                            
                            </div>
                        </div> 
                        
                        <asp:Panel ID="pnlConfiguration" runat="server" Width="120px" CssClass="left">
                            <div class="pad">  
                                <div class="left"> 
                                    <asp:HyperLink ID="lnkConfigurationIcon" runat="server" SkinID="delReviewIcon" Target="_blank" ToolTip="Project Configuration"></asp:HyperLink> 
                                </div> 
                                <div style="width: 70%;padding-top:4px;padding-left: 3px;" class="left">
                                    <asp:HyperLink ID="lnkConfigurationText" runat="server" Target="_blank" ToolTip="Go to Project Configuration" Text="Configuration" Font-Size="1.2em"></asp:HyperLink> 
                                </div>                                    
                            </div>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlDeliveryReviews" runat="server" Width="100px" CssClass="left">
                            <div class="pad">  
                                <div class="left"> <asp:HyperLink ID="lnkDelReviews" runat="server" SkinID="delReviewIcon" Target="_blank" ToolTip="Go to Delivery Review(s)"></asp:HyperLink> </div> 
                                 <div style="width: 70%;padding-top:4px;padding-left: 3px;" class="left">
                                    <asp:HyperLink ID="lnkDelReviews2" runat="server" Target="_blank" ToolTip="Go to Delivery Reviews" Text="Delivery Reviews" Font-Size="1.2em"></asp:HyperLink> 
                                 </div>    
                                <asp:Literal ID="litDelReview" runat="server" />                            
                            </div>
                        </asp:Panel>
                         
                        <asp:Panel ID="pnlWR_Homes" runat="server" Width="100px" CssClass="left">
                            <div class="pad">  
                                <div class="left">
                                <%-- <asp:HyperLink ID="lnkHomes" runat="server" SkinID="homeIcon" Target="_blank" ToolTip="Go to Homes" Text="Homes"></asp:HyperLink> --%> 
                                   <asp:HyperLink ID="lnkHomes" runat="server" SkinID="buildingIcon"  ToolTip="Go to Properties" Text="Properties"></asp:HyperLink>  </div>                                 
                                 <div style="width: 70%;padding-top:4px;padding-left: 3px;" class="left">
                                    <asp:HyperLink ID="lnkHomes2" runat="server"  ToolTip="Go to Properties" Text="Properties" Font-Size="1.2em"></asp:HyperLink> 
                                 </div>  
                                <asp:Literal ID="litHomes" runat="server" />                     
                            </div>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlWR_Products" runat="server" Width="100px" CssClass="left">
                            <div class="pad">  
                                <div class="left">
                              <%--  <asp:HyperLink ID="lnkProducts" runat="server" SkinID="productsIcon" Target="_blank" ToolTip="Go to Products"></asp:HyperLink> --%>
                                  <asp:HyperLink ID="lnkProducts" runat="server" SkinID="BuildingProductsIcon" ToolTip="Go to Products"></asp:HyperLink> 
                                </div> 
                                  <div style="width: 70%;padding-top:4px;padding-left: 3px;" class="left">
                                    <asp:HyperLink ID="lnkProducts2" runat="server"  ToolTip="Go to Products" Text="Products" Font-Size="1.2em"></asp:HyperLink> 
                                 </div>  
                                <asp:Literal ID="litProducts" runat="server" />      
                            </div>
                        </asp:Panel>                        
                        
                        <asp:Panel ID="pnlSR_RequestList" runat="server" Width="110px" CssClass="left">
                            <div class="pad">  
                                <div class="left"> <asp:HyperLink ID="lnkServiceRequest" runat="server" SkinID="requestIcon"  ToolTip="Go to Service Requests"></asp:HyperLink> </div> 
                                <div style="width: 70%;padding-top:4px;padding-left: 3px;" class="left">
                                    <asp:HyperLink ID="lnkServiceRequest2" runat="server"  ToolTip="Go to Service Requests" Text="Service Requests" Font-Size="1.2em"></asp:HyperLink> 
                                 </div> 
                                   <asp:Literal ID="litRequest" runat="server" />                             
                            </div>
                        </asp:Panel>
                                                
                    </div>            
                                                                   
                   
                    <div class="wr-clearHack" style="margin-bottom: 5px">.</div> 
                </div>
                <div class="wr-clearHack">.</div>                 
                </ItemTemplate>
            </asp:TemplateField>
                                   
        </Columns>
    </asp:GridView>
                           
            
   <div class="wr-clearHack" style="margin-top: 2px">.</div> 
   
   </ContentTemplate>       
   </asp:UpdatePanel>   
</asp:Content>
