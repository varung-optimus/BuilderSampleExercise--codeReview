<%@ Page Language="C#" MasterPageFile="~/master/HIP.Master" AutoEventWireup="true"
    CodeBehind="Contacts.aspx.cs" Inherits="HomeOwner.app.Builder.Contacts" Title="Builder Portal - Contacts" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="contentMain" runat="server">
    <asp:HiddenField ID="hfBuilderPortalID" runat="server" OnPreRender="LoadPortalID" />
    <div>
        
        <h2>
        Contacts</h2>
                
        
        <asp:UpdatePanel ID="upContacts" runat="server">
            <ContentTemplate>
            
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upContacts"
                DisplayAfter="250">
                <ProgressTemplate>
                    <div id="divProgressPopup" class="progressIndicator">
                        <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                        Please Wait...
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
                    <asp:Button ID="btnClearCriteria" runat="server" Text="Clear" onclick="btnClearCriteria_Click" Font-Size="8pt" TabIndex="1" />
                </div>
                <h3 style="padding: 0px; margin: 0px; padding-left: 275px;" align="left">Search for Contacts</h3>   
                <div class="wr-clearHack">.</div>        
                 
                <div align="center">   
                    <div style="width: 90%"> 
                    
                        <div class="left" style="width: 250px; padding-bottom: 10px" align="left">            
                            <h4 style="margin-bottom: 15px">By Project</h4>                   
                            <asp:DropDownList ID="drpProject" runat="server" Width="250px" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </div>  
                               
                        <div class="left" style="width: 250px;margin-left:90px;" align="left"> 
                            <h4 style="margin-bottom: 15px">By Role</h4>
                            <asp:DropDownList ID="drpRole" runat="server" Width="200px">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Builder" Value="1" />
                                <asp:ListItem Text="Installer" Value="6" />
                                <asp:ListItem Text="Manufacturer" Value="7" />
                                <asp:ListItem Text="Supplier" Value="11" />
                                <asp:ListItem Text="Warranty Provider" Value="10" />                            
                            </asp:DropDownList>                            
                        </div>                                          
                        <div class="wr-clearHack">.</div> 
                        
                        <div class="left" style="width: 250px;" align="left"> 
                            <h4 style="margin-bottom: 15px">By Company Name</h4>                         
                            <asp:TextBox ID="txtName" runat="server" Width="185px"></asp:TextBox><div style="height: 6px;"></div>
                        </div>                                           
                        
                        <div class="right" style="width: 600px;" align="right">             
                            <asp:Button ID="btnFilter" runat="server" Text="Search" onclick="btnFilter_Click" />
                        </div>
                        
                    </div>
                    <div class="wr-clearHack">.</div>  
                </div>
            </asp:Panel>  
            <div class="wr-clearHack" style="padding-top: 8px;">.</div> 
          
            <act:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" TargetControlID="pnlFilter" SkinID="roundCorners">
            </act:RoundedCornersExtender>   
             
            <div class="left" style="width: 250px;padding-top:25px;">  
              <h3>Search Results</h3>
            </div>
           
            <div class="right" style="width: 195px; font-size: .8em;">  
                <asp:Panel ID="pnlLegend" runat="server" HorizontalAlign="Center" Width="175px" Height="45px" BorderStyle="Dashed" BorderWidth="1" BorderColor="Silver"> 
                <h6 style="padding: 0px; margin: 0px">Tools Legend</h6>   
                <div align="center">   
                    <div style="width: 99%"> 
                        <asp:HyperLink ID="lnkLegendDetails" runat="server" SkinID="BuildingProductsIcon"></asp:HyperLink> = Product List &nbsp;&nbsp;                        
                    </div>
                </div>
                </asp:Panel>
            </div>
            <div class="wr-clearHack" style="padding-top:6px;">.</div> 

                 <asp:GridView ID="gridContacts" runat="server" SkinID="grid"
                     Width="97%"
                     DataKeyNames="CompanyID" 
                     RowStyle-Height="20px"
                     OnRowDataBound="gridContacts_RowDataBound" 
                     AllowSorting="True"
                     EmptyDataText="No Companies found that match your criteria."
                     DataSourceID="dsContacts" >             
                 <Columns>
                              
                    <asp:TemplateField HeaderText="Project" SortExpression="Project.ProjectName" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>                       
                           <%#Eval("Project.ProjectName")%>
                        </ItemTemplate>               
                    </asp:TemplateField>         
                    
                    <asp:TemplateField HeaderText="Role" SortExpression="CompanyType.Name" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>                       
                           <%#Eval("CompanyType.Name")%>
                        </ItemTemplate>               
                    </asp:TemplateField>             

                    <asp:TemplateField HeaderText="Company" SortExpression="Company.CompanyLegalName" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>                          
                            <%#Eval("Company.CompanyLegalName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Address" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>                         
                            <asp:Literal ID="litContactAddress" runat="server" />                      
                        </ItemTemplate>               
                    </asp:TemplateField> 
                    
                    <asp:TemplateField HeaderText="Contact Details" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>                       
                            <%--<asp:Literal ID="litGridContact" runat="server" />--%>
                             Phone: <%# Eval("Company.MainBusinessPhone")%> <br />
                             Web Site: <asp:Literal id="litContactWeb" runat="server" />     
                        </ItemTemplate>               
                    </asp:TemplateField>                        
                    
                    <asp:TemplateField HeaderText="Tools" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>                    
                            <asp:HyperLink ID="lnkDetails" runat="server" SkinID="BuildingProductsIcon" ToolTip="Show Product List"  ></asp:HyperLink>                            
                        </ItemTemplate>               
                    </asp:TemplateField> 
                    
                </Columns>
                </asp:GridView>
                           
        
        
            </ContentTemplate>
        </asp:UpdatePanel>       
        
    <div class="wr-clearHack" style="margin-top: 9px">.</div>
    </div>   
    
    <asp:ObjectDataSource ID="dsContacts" runat="server" 
        SelectMethod="SearchBuilderContacts" TypeName="HomeOwner.app.Builder.Contacts" SortParameterName="sortExpression">
        <SelectParameters>
            <asp:ControlParameter ControlID="hfBuilderPortalID" DefaultValue="0" Name="builderPortalId" 
                PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="drpProject" DefaultValue="0" Name="projectId" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="drpRole" DefaultValue="0" Name="companyTypeId" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="txtName" DefaultValue="" Name="companyName" 
                PropertyName="Text" Type="String" />
            <asp:Parameter Name="sortExpression" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
 
</asp:Content>
