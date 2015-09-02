<%@ Page Title="Builder Portal - Service Request Related Requests" Language="C#" MasterPageFile="~/master/HIP_BuilderService.Master" AutoEventWireup="true" CodeBehind="Builder_ServiceRequestRelated.aspx.cs" 
Inherits="HomeOwner.app.Builder_ServiceRequestRelated" %>
<%@ MasterType VirtualPath="~/master/HIP_BuilderService.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">

<div id="divMain">
    <h2>Service Request - Related Requests</h2>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    <div id="divTop" class="box pad" style="margin-bottom: 23px; margin-left: 18px; width: 95%;" align="center">
        <div align="left">
            <asp:Label ID="Label1" runat="server" Text="This a list of any Related Requests that this Request may have. You can Relate other Requests to this one, only if you know the Request ID.">
            </asp:Label>
        </div>        
    </div>
    
    <asp:Label ID="lblDisabledMsg" runat="server" Text="" ForeColor="#D50000" CssClass="medtext" Width="99%"></asp:Label>
          
    <div id="divWOGrid" class="scrollBox topDblSpc">
    
        <asp:GridView ID="gridRelated" runat="server" SkinID="grid"         
            DataSourceID="LinqDataSource1"
            DataKeyNames="UnitServiceRequestRelatedRequestID"       
            EnableViewState="true"       
            EmptyDataText="No Related Requests found for this Request" 
            HeaderStyle-HorizontalAlign="Left"
            OnRowDataBound="gridRelated_RowDataBound">             
            <Columns>           
                <asp:BoundField DataField="UnitServiceRequestRelatedRequestID" Visible="False"  />
                                           
                <asp:TemplateField HeaderText="Request ID" >
                    <ItemTemplate>                        
                        <%#Eval("ChildRequestID")%>               
                    </ItemTemplate>     
                    <ItemStyle Width="90px" />              
                </asp:TemplateField> 
                
                <asp:TemplateField HeaderText="Comment">
                    <ItemTemplate> 
                        <%#Eval("Comments")%>                     
                    </ItemTemplate>                     
                </asp:TemplateField>    
                
                <asp:TemplateField HeaderText="Relation Method">
                    <ItemTemplate> 
                        <%#Eval("ServiceRequestRelationMethod.Name")%>                     
                    </ItemTemplate>                  
                    <ItemStyle Width="125px" />
                </asp:TemplateField>    
                
                
                <asp:TemplateField HeaderText="Created On">
                    <ItemStyle Width="90px" />
                    <ItemTemplate> 
                        <%#Eval("CreatedOn", "{0:dd-MMM-yyyy}")%> 
                    </ItemTemplate>  
                </asp:TemplateField> 
                
                  <asp:TemplateField HeaderText="View/Edit" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" SkinID="detailsIcon" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                   
            </Columns>
        </asp:GridView>
        
    </div>  
    
<br />  
<div class="pad indentdbl" style="padding-left: 150px;" > 
<h3 align="left">Relate a Request</h3>

    <div class="pad" >
        <div>
            Request ID:&nbsp;
            <asp:TextBox ID="txtRequestID" runat="server" MaxLength="6" Width="70px"></asp:TextBox>
               <asp:RequiredFieldValidator ID="IDRequired" runat="server" ControlToValidate="txtRequestID" Display="Dynamic" 
                        ErrorMessage="Request ID is required" 
                        ToolTip="Request ID is required" ValidationGroup="relate">
               </asp:RequiredFieldValidator>
               <asp:RegularExpressionValidator ID="RequestIDRegExpr" runat="server" ErrorMessage="Invalid ID. Numbers Only."
                        ControlToValidate="txtRequestID" ValidationExpression="^\d{1,10}$" Display="Dynamic" ValidationGroup="relate">
               </asp:RegularExpressionValidator>
            
        </div>
        <div class="pad">
            Comment:&nbsp;
            <asp:TextBox ID="txtComment" runat="server" Font-Names="'Trebuchet MS', tahoma" Height="44px"
                MaxLength="2000" TextMode="MultiLine" Width="500px">
            </asp:TextBox>          
        </div>
        
        <div align="right" class="pad" style="padding-right: 210px;">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Add Request"
            ValidationGroup="relate" />
        </div>
  
    </div>
    </div>
    
    <div class="important topDblSpc" style="color: red;">
        <asp:Literal ID="litMsg" runat="server"></asp:Literal>
    </div>
    
        
    <div class="wr-clearHack">.</div>
    
        
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="250">
        <ProgressTemplate>
              <div id="divProgressPopup" class="progressIndicator">                           
               <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" /> Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    
    </ContentTemplate>
    </asp:UpdatePanel>   
       
 
    <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
        ContextTypeName="WRObjectModel.WarrantyDataContext" EnableUpdate="False" 
        OrderBy="ChildRequestID" TableName="UnitServiceRequestRelatedRequests" 
        Where="ParentRequestID == @RequestID">
        <WhereParameters>
            <asp:QueryStringParameter Name="RequestID" QueryStringField="rid" Type="Int32" />
        </WhereParameters>    
    </asp:LinqDataSource>
   
     
</div>
 <div class="wr-clearHack" style="margin-top: 5px">.</div>  
</asp:Content>