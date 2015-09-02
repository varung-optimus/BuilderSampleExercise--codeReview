<%@ Page Title="Builder Portal - Products" Language="C#" MasterPageFile="~/master/HIP.Master"
    AutoEventWireup="True" CodeBehind="Builder_Products.aspx.cs" Inherits="HomeOwner.app.Builder_Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

    <h2>
        Products</h2>
               
    <asp:UpdatePanel ID="upProducts" runat="server">   
    <ContentTemplate>     
          
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProducts" DisplayAfter="250">
        <ProgressTemplate>
              <div id="divProgressPopup" class="progressIndicator">                           
               <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" /> Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    
    <div class="left medtext" style="width: 550px; padding-bottom: 10px" align="left">
         <div class="left dark bold">Showing: </div>
         <div class="right" style="padding-left: 8px; width: 485px;"><asp:Label ID="lblFilterDisplay" runat="server" Text=""></asp:Label>   </div>         
    </div>   
         
    <div align="right" class="right pad" style="padding-top: 3px">
        <asp:LinkButton ID="btnShowSearch" runat="server" BorderStyle="None" 
            onclick="btnShowSearch_Click" Font-Size="1.15em" Font-Bold="True" CssClass="altColor">Search</asp:LinkButton>   
    </div>
    <div align="right" class="right">
        <asp:ImageButton ID="btnShowSearch2" runat="server" SkinID="search" 
            onclick="btnShowSearch_Click" />          
    </div>    
   
    <div class="wr-clearHack topDblSpc">.</div> 
                                  
                                                             
   <asp:Panel ID="pnlFilter" class="box2" runat="server" HorizontalAlign="Center" Visible="False" DefaultButton="btnFilter"> 
    <div class="left" style="width: 60px; padding-left: 8px;" align="left">             
        <asp:Button ID="btnClearCriteria" runat="server" Text="Clear" onclick="btnClearCriteria_Click" Font-Size="8pt" TabIndex="2" />
    </div>
    <h3 style="padding: 0px; margin: 0px; padding-left: 275px;" align="left">Search for Products</h3>   
    <div class="wr-clearHack">.</div>
    
    <div align="center">   
        <div style="width: 90%"> 
            <div class="left" style="width: 250px; padding-bottom: 10px" align="left">            
                <h4 style="margin-bottom: 15px">By Project</h4>                   
                <asp:DropDownList ID="ddlProject" runat="server" Width="250px" AppendDataBoundItems="True">
                </asp:DropDownList>
            </div>         
            <div class="left" style="width: 250px;margin-left:90px;" align="left"> 
                <h4 style="margin-bottom: 15px">By Unit</h4>
                <label style="width: 50px; display:inline-table; display:inline-block;">Unit #: </label>
                <asp:TextBox ID="txtUnitNumber" runat="server" Width="85px"></asp:TextBox><div style="height: 6px;"></div>
                <label style="width: 50px; display:inline-table; display:inline-block;">Address: </label>
                <asp:TextBox ID="txtStreetAddress" runat="server" Width="185px"></asp:TextBox><div style="height: 6px;"></div>
                <asp:HiddenField ID="hfUnitID" runat="server" />
            </div>
            
            <div class="left" style="width:250px;" align="left"> 
                <h4 style="margin-bottom: 15px">By Products Used</h4>
                    <div class="left" style="width: 275px;" align="left">
                        <label style="width: 90px; display:inline-table; display:inline-block;">Category: </label>
                        <asp:DropDownList ID="drpCategory" runat="server" Width="157px" AppendDataBoundItems="True">
                        </asp:DropDownList><div style="height: 6px;"></div>
                        <label style="width: 90px; display:inline-table; display:inline-block;">Model: </label>
                        <asp:TextBox ID="txtProductName" runat="server" Width="150px"></asp:TextBox><div style="height: 6px;"></div>
                        <label style="width: 90px; display:inline-table; display:inline-block;">Manufacturer: </label>
                        <asp:TextBox ID="txtManufacturer" runat="server" Width="150px"></asp:TextBox>
                        <div style="height: 6px;"></div>
                    </div>
            </div>
            
            <div class="left" style="width: 250px; margin-left:90px;" align="left"> 
                <h4 style="margin-bottom: 15px">By Contacts Involved</h4> 
                    <label style="width: 50px; display:inline-table; display:inline-block;">Name: </label>
                    <asp:TextBox ID="txtContactName" runat="server" Width="185px"></asp:TextBox>  
                    <div class="wr-clearHack">.</div> 
            </div>           
            
            <div class="right" style="width: 300px; padding-top: 35px;" align="right">             
                <asp:Button ID="btnFilter" runat="server" Text="Search" onclick="btnFilter_Click" TabIndex="1" />
            </div>
            
        </div>
        <div class="wr-clearHack">.</div>  
    </div>
    </asp:Panel>  
    <div class="wr-clearHack" style="padding-top: 8px;">.</div> 
      
      
    <act:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="pnlFilter" SkinID="roundCorners">
    </act:RoundedCornersExtender>     
       
    <div class="left" style="width: 250px;padding-top:25px;">  
      <h3>Search Results</h3>
    </div>
   
    <div class="right" style="width: 305px; font-size: .8em;">  
        <asp:Panel ID="pnlLegend" runat="server" HorizontalAlign="Center" Width="280px" Height="45px" BorderStyle="Dashed" BorderWidth="1" BorderColor="Silver"> 
        <h6 style="padding: 0px; margin: 0px">Tools Legend</h6>   
        <div align="center">   
            <div style="width: 99%"> 
                <asp:HyperLink ID="lnkLegendDetails" runat="server" SkinID="detailsIcon"></asp:HyperLink> = Product Details &nbsp;&nbsp;            
                <asp:HyperLink ID="lnkLegendHome" runat="server" SkinID="buildingIcon"></asp:HyperLink> = Properties Using Product &nbsp;
            </div>
        </div>
        </asp:Panel>
    </div>
   
    
    <div class="wr-clearHack" style="padding-top:6px;">.</div> 
    
    <asp:GridView ID="gridProducts" runat="server" SkinID="grid"
         Width="97%"
         DataKeyNames="ItemID" 
         RowStyle-Height="20px"
         OnRowDataBound="gridProducts_RowDataBound" 
         AllowSorting="True"
         onsorting="gridProducts_Sorting"
         EmptyDataText="No Products found that match your criteria." >   
        <RowStyle Height="20px" />
        <Columns>
                      
            <asp:TemplateField HeaderText="Category" SortExpression="ComponentType.ParentComponentType.Name" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                   <%-- <%# Eval("ComponentType.FullName") %>--%>
                   <%#Eval("ComponentType.ParentComponentType.Name")%>
                </ItemTemplate>               
            </asp:TemplateField>             

            <asp:TemplateField HeaderText="Sub-Category" SortExpression="ComponentType.Name" HeaderStyle-Width="210px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>                
                    <%-- <asp:Literal ID="litGridModel" runat="server" />--%>
                    <%#Eval("ComponentType.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Model/Type" SortExpression="ItemName" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate> 
                    <%-- <%# Eval("Manufacturer.CompanyLegalName") %>  --%>   
                    <%# Eval("ItemName")%>        
                </ItemTemplate>               
            </asp:TemplateField> 
            
            <asp:TemplateField HeaderText="Tools" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>                    
                    <asp:HyperLink ID="lnkDetails" runat="server" SkinID="detailsIcon"  ToolTip="Show Product Details" ></asp:HyperLink> 
                    <asp:HyperLink ID="lnkHome" runat="server" SkinID="buildingIcon"  ToolTip="Go to Properties that use this Product"></asp:HyperLink>
                </ItemTemplate>               
            </asp:TemplateField> 
            
        </Columns>
    </asp:GridView>
                           
            
   <div class="wr-clearHack" style="margin-top: 9px">.</div> 
   
        
   </ContentTemplate>       
   </asp:UpdatePanel>   
</asp:Content>
