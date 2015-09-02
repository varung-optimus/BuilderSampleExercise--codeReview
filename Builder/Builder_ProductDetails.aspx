<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Builder_ProductDetails.aspx.cs"
    Inherits="HomeOwner.app.Builder_ProductDetails" Theme="BuilderDefault" Title="Builder Portal - Product Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Warranty Resource Builder Portal - Product Details</title>
</head>

<body>
    <form id="form1" runat="server">
    <div id="wrap" style="width: 770px">
     <div id="wrap_border">    
     
        <div id="divHipLogo" align="left">
            <asp:HyperLink ID="lnkLobby" runat="server">
                <asp:Image ID="imgHipLogo" runat="server" SkinID="hipLogoNew" />
            </asp:HyperLink>
        </div>
                
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="upProductDetails" runat="server">
            <ContentTemplate>
            
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProductDetails"
                    DisplayAfter="270">
                    <ProgressTemplate>
                        <div id="divProgressPopup" class="progressIndicator">
                            <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                            Please Wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
                 <span id="spanHomesLink" runat="server" class="right" style="width: 200px;">                
                    <asp:HyperLink ID="lnkViewHomes" runat="server" SkinID="buildingIcon" Text="View Properties using Product"> </asp:HyperLink> 
                    View Properties Using Product 
                </span>
                 <div class="wr-clearHack">.</div> 
            <span class="right" style="width: 198px;">
           <asp:HyperLink ID="lnkBackToProducts" style="text-decoration:none" runat="server" NavigateUrl="~/app/Builder/Builder_Products.aspx">
          Return to Products
           </asp:HyperLink>            
            </span>
                <h2>
                    Product Details</h2>
              
                <div class="wr-clearHack">.</div>
                
                
                <asp:Panel ID="pnlProductDetails" runat="server">
                <br />
                    <div class="box2 pad border" align="center">
                        <div class="important bold left">                         
                            <asp:Label ID="lblComponentType" runat="server" Text=""></asp:Label>                            
                        </div> 
                        <div class="wr-clearHack">.</div>                     
                    </div>
                    
                    <div class="box pad right border2" align="center" style="width: 320px;margin-top: -20px; margin-right: 15px; ">
                        <div class="important bold">                         
                            <asp:Label ID="lblComponent" runat="server" Text=""></asp:Label>                            
                        </div> 
                        <div class="wr-clearHack">.</div>
                    </div>                    
                    <div class="wr-clearHack">.</div>
                    
                    <div class="left" style="margin-right: 55px; margin-top: 8px; margin-left: 15px;line-height: 1.1em">
                        <asp:Panel ID="Panel1" runat="server" CssClass="left" Width="255px">
                            <div class="pad">
                                <h5 align="center" style="margin-top: -8px;">Manufacturer</h5>
                                <asp:Literal ID="litManufacturer" runat="server" />
                            </div>       
                        </asp:Panel>
                    </div>
                    <act:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="Panel1" SkinID="roundCorners"></act:RoundedCornersExtender>  
                
                    <div style="height: 25px;"></div>
                    <asp:Panel ID="pnlDocs" runat="server" CssClass="left" Width="395px">
                      <div class="pad" style="line-height: 1.3em">
                        <div align="center"><h5 style="margin-top: -8px;">Documents</h5></div>
                            <wr:LinksList ID="litDocuments" runat="server" CssClass="topDblSpc" Font-Size="10pt" />
                                <br />
                                <div class="smltext pad line1" style="border-top: dashed 1px silver; margin-left: 30px;" align="right">                                        
                                Can't open the documents? Download the latest version
                                of the <a href="http://get.adobe.com/reader/" target="_blank">
                                Acrobat Reader</a> free.</div>   
                      </div>                                    
                    </asp:Panel>
                    <act:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" TargetControlID="pnlDocs" SkinID="roundCorners"></act:RoundedCornersExtender>  
                                     
                    <div class="wr-clearHack">.</div><br /> 
                    <div style="width: 700px;">
                       <%--All Other Info--%>
                        <asp:Literal ID="litOther" runat="server" />                    
                    </div>    
                                                                       
                                
                    <div class="wr-clearHack">.</div> 
                    <br />   
                    <div style="width: 340px;" class="left">
                        <h5>Suppliers</h5>
                        
                        <asp:GridView ID="gridSuppliers" runat="server" 
                            DataKeyNames="CompanyID"      
                            GridLines="None"     
                            AutoGenerateColumns="false"          
                            OnRowDataBound="grids_RowDataBound" 
                            ShowHeader="false"                                  
                            EmptyDataText="No Suppliers found for this Product">        
                            <Columns>                                  
                                <asp:TemplateField HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div style="border-top: dashed 1px silver" class="pad">
                                            <div class="bigtext pad">                                         
                                                <asp:HyperLink ID="lnkContact" runat="server" ><%# Eval("CompanyLegalName") %></asp:HyperLink><br />
                                                <div class="smltext"><asp:Literal ID="litGridRowAddress" runat="server" /> </div>
                                            </div>                                                                                           
                                        </div>                        
                                    </ItemTemplate>
                                </asp:TemplateField>                             
                            </Columns>                            
                        </asp:GridView>
                      
                    </div>
                                      
                    <div style="width: 340px; margin-left: 20px;" class="left">
                        <h5>Installers</h5>
                         <asp:GridView ID="gridInstallers" runat="server" 
                            DataKeyNames="CompanyID"          
                            GridLines="None"    
                            AutoGenerateColumns="false"      
                            OnRowDataBound="grids_RowDataBound" 
                            ShowHeader="false"                                  
                            EmptyDataText="No Installers found for this Product">        
                             <Columns>                                  
                                <asp:TemplateField>
                                    <ItemTemplate>
                                         <div style="border-top: dashed 1px silver" class="pad">
                                            <div class="bigtext pad">                                                                          
                                                <asp:HyperLink ID="lnkContact" runat="server"><%# Eval("CompanyLegalName") %></asp:HyperLink><br />
                                                <div class="smltext"><asp:Literal ID="litGridRowAddress" runat="server" /> </div>
                                            </div>                                                                                           
                                        </div>                        
                                    </ItemTemplate>
                                </asp:TemplateField>                            
                                
                            </Columns>  
                        </asp:GridView>            
                    </div>                    
                    
                <div class="wr-clearHack">.</div>               
                </asp:Panel>    
                <div id="footer">        
          <asp:HyperLink ID="lnkFooterLobby" runat="server" Text="Lobby" /> 
            &nbsp;|
        <asp:HyperLink ID="lnkTerms" runat="server" Text="Terms of Use" /> 
            &nbsp;|
        <asp:HyperLink ID="lnkPrivacy" runat="server" Text="Privacy Policy"  />
             
        <div style="padding-top: 5px;" class="alt">
            Copyright &copy;2013 <a href="http://www.conasysinc.com">CONASYS Inc.</a>    
        </div>
    </div>            
                <div class="wr-clearHack" style="margin-top: 9px">.</div>                    
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    </div>
    </form>
</body>
</html>
