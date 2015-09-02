<%@ Page Title="Builder Portal - Service Request Related Products" Language="C#" MasterPageFile="~/master/HIP_BuilderService.Master" AutoEventWireup="true" CodeBehind="Builder_ServiceRequestProducts.aspx.cs" 
Inherits="HomeOwner.app.Builder_ServiceRequestProducts" %>
<%@ MasterType VirtualPath="~/master/HIP_BuilderService.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
       
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 920px; height: auto; margin-left: auto; margin-right: auto; ">
                <div style="width: 920px; margin-left: auto; margin-right: auto; height: auto;" class="pad indentdbl">
                    <!-- Start header content -->
                    <div style="width: 920px; height: auto; margin-left: auto; margin-right: auto; ">
                        <h2>Service Request - Related Products</h2>
                        <h3 style="margin-top: 1px" class="indent">Product Selection</h3>
                        <div id="divTop" class="box pad" style="margin-bottom: 11px; margin-left: 18px; width: 92%;" align="center">
                            <div align="left">
                                <asp:Label ID="Label2" runat="server" 
                                    Text="Select a Product from the left to add to the list of related Products.  This list contains the Products that you believe are related to the request."> 
                                </asp:Label>            
                            </div> 
                        </div>
                        
                        <asp:Label ID="lblDisabledMsg" runat="server" Text="" ForeColor="#D50000" CssClass="medtext" Width="99%" Visible="False"></asp:Label>
                    </div>
                    <!-- End header content -->
                    <!-- Start page content -->
                    <div style="width: 705px; height: auto; margin-left: auto; margin-right: auto; ">
                        <!-- Start left frame content -->
                        <div style="width: 340px; height: 520px; float: left; text-align: left;">
                            <h6>Your Property Products - Not Related</h6>
                            <div class="box border pad">
                                <div id="divFilter" class="pad smltext">
                                    <div style="float: left;">                    
                                        <div align="center">Category</div>                  
                                        <asp:DropDownList ID="Category" runat="server" Width="140px" />
                                    </div>
                                    <div style="float: left; padding-left: 7px;">
                                        <div align="left" style="margin-bottom: -2px;padding-left: 45px;">Keyword</div>                   
                                        <asp:PlaceHolder ID="phSearchKeyword" runat="server">
                                            <asp:TextBox ID="Keyword" runat="server" Width="120px"/>
                                        </asp:PlaceHolder>&nbsp;              
                                       <asp:Button ID="btnFilter" runat="server" Text="Go" Width="32px"
                                            CausesValidation="False" onclick="btnFilter_Click" />         
                                    </div>
                                    <div class="wr-clearHack">.</div>
                                </div>
                                <div class="wr-clearHack">.</div>
                                <div id="divProductList" class="scrollBox topDblSpc" style="max-height: 395px; height: 395px;"> 
                 
                                     <asp:DataList ID="listHomeProducts" runat="server"
                                        DataKeyField="ComponentID"                                    
                                        ShowHeader="false"
                                        Showfooter="false"
                                        GridLines="None"
                                        BorderStyle="None"
                                        CellSpacing="0"
                                        CellPadding="0"
                                        RepeatLayout="Table"               
                                        RepeatDirection="Vertical"                     
                                        onitemcommand="listHomeProducts_ItemCommand" 
                                        onitemcreated="list_ItemCreated" Width="340px">                                            
                                           <ItemTemplate>                        
                                             <div id="divProductItems">                               
                                                <div style="float: left; width: 28px; min-height: 30px; padding-top: 8px;">
                                                    <asp:LinkButton ID="btnAddComponent" runat="server" CommandName="select" CommandArgument='<%#Eval("ComponentID")%>'>Add</asp:LinkButton>
                                                </div>                                
                                                <div style="float: left; width: 292px;">
                                                    <div style="text-align: left; float: left; padding-right: 3px;"> 
                                                        <%#Eval("ComponentType.ParentComponentType.Name")%> >
                                                    </div>
                                                 
                                                    <div style="text-align: left; float: left;">
                                                         <%#Eval("ComponentType.Name")%> 
                                                     </div>
                                                    <div style="clear: both; width: 99%;" class="medtext">  
                                                         <div class="right" style="margin-right: 17px; width: 45px;" align="right">
                                                            <asp:Image ID="imgDetails" runat="server" SkinID="moreInfo"/>
                                                        <act:PopupControlExtender ID="PopupControlExtender1" runat="server"
                                                            TargetControlID="imgDetails"
                                                            PopupControlID="popupComponentDetails"
                                                            DynamicContextKey='<%#Eval("ComponentID")%>' 
                                                            DynamicControlID="popupComponentDetails"
                                                            DynamicServiceMethod="GetPopupContent" Position="Bottom" OffsetX="-20"> 
                                                        </act:PopupControlExtender>
                                                        </div> 
                                                        <%#Eval("ComponentDescription")%>  
                                                     </div>  
                                                                                      
                                                </div>
                                                <div class="wr-clearHack">.</div>                                                                                                           
                                            </div>        
                                     
                                        </ItemTemplate>
                                        
                                        <FooterTemplate>
                                            <div style="padding-top: 15px">
                                            <asp:Label ID="lblEmpty" Text="No related Products can be found for this home" runat="server" 
                                                Visible='<%#bool.Parse((listHomeProducts.Items.Count==0).ToString())%>' Font-Size="0.9em" ForeColor="#333333"> </asp:Label>
                                            </div>
                                        </FooterTemplate>

                                        <ItemStyle HorizontalAlign="Left" />                                                             
                                    </asp:DataList>
               
                                </div>            
                            </div>               
                        </div>
                        <!-- End left frame content -->
                        <!-- Start center frame content -->
                        <div style="height: 300px; width: 30px; padding-top: 200px; float: left;">       
                            <asp:Image ID="imgArw" runat="server" SkinID="rightArrow_large" />
                        </div>
                        <!-- End center frame content -->
                        <!-- Start right frame content -->
                        <div style="width: 335px; float: right;">
                            <h6>Products - Related to Request</h6>
                                
                            <div class="scrollBox" id="divRelatedProductList" style="max-height: 435px; height: 435px;">                                        
                                <asp:DataList ID="listRelatedComponents" runat="server"
                                        DataKeyField="ComponentID"
                                        onitemcommand="listRelatedComponent_ItemCommand"        
                                        ShowHeader="false"
                                        Showfooter="false"
                                        GridLines="None"
                                        BorderStyle="None"
                                        CellSpacing="0" 
                                        CellPadding="0"
                                        RepeatLayout="Table"               
                                        RepeatDirection="Vertical"
                                        onitemcreated="list_ItemCreated" Width="335">                                              
                                               <ItemTemplate>
                                             
                                                  <div id="divProductItems">                                   
                                                     <div style="float: left; width: 47px; min-height: 30px; padding-top: 8px;">
                                                         <asp:LinkButton ID="btnRemoveComponent" runat="server" CommandName="remove" 
                                                                CommandArgument='<%#Eval("ComponentID")%>'>Remove</asp:LinkButton>
                                                     </div>
                                                     <div style="float: left; width: 273px;">
                                                         <div style="text-align: left; float: left; padding-right: 3px;">
                                                             <%#Eval("Component.ComponentType.ParentComponentType.Name")%> >
                                                         </div>
                                                         <div style="text-align: left; float: left;">
                                                             <%#Eval("Component.ComponentType.Name")%>
                                                         </div>
                                                         <div style="clear: both; width: 100%;" class="medtext">  
                                                             <div class="right" style="margin-right: 8px; width: 45px;" align="right">
                                                                <asp:Image ID="imgDetails2" runat="server" SkinID="moreInfo"/>
                                                                <act:PopupControlExtender ID="PopupControlExtender2" runat="server"
                                                                    TargetControlID="imgDetails2"
                                                                    PopupControlID="popupComponentDetails"
                                                                    DynamicContextKey='<%#Eval("ComponentID")%>' 
                                                                    DynamicControlID="popupComponentDetails"
                                                                    DynamicServiceMethod="GetPopupContent" Position="Bottom" OffsetX="-305"> 
                                                                </act:PopupControlExtender>
                                                             </div> 
                                                                <%#Eval("Component.ComponentDescription")%>  
                                                         </div>  
                                                     </div>
                                                     <div class="wr-clearHack">.</div>
                                                 </div> 
                                                       
                                            </ItemTemplate> 
                                            <FooterTemplate>
                                                <div style="padding-top: 15px">
                                                </div>
                                            </FooterTemplate>                            
                                            <ItemStyle HorizontalAlign="Left" />
                                </asp:DataList> 
                                            
                            </div>        
                        </div>                              
                        <!-- End right frame content -->
                        <div class="wr-clearHack">.</div>
                    </div> 
                    <!-- End page content -->
                    <div class="wr-clearHack">.</div>       
                    <!-- Start Popup Content -->
                    <div id="popupComponentDetails" style="display:none;" runat="server" class="AJAX_ModalPopup"></div>
                    <!-- End Popup Content -->
                </div>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>   
 


    <div class="wr-clearHack" style="margin-top: 2px">.</div>
</asp:Content>