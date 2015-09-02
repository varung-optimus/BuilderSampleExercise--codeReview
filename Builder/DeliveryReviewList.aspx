<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/HIP.Master" CodeBehind="DeliveryReviewList.aspx.cs" 
    Inherits="HomeOwner.app.Builder.DeliveryReviewList" Title="Builder Portal - Delivery Reviews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
<h2>What's this?</h2>
<p>
Delivery reviews are conducted for every project to ensure that the information we've collected is complete, accurate, and ready for the publication both online and in printed form.  
</p>

<h2>Pending Delivery Reviews</h2>

<div>
        
        <wr:BaseGridView ID="gvPendingReviews" runat="server" SkinID="grid"
                DataKeyNames="ProjectDeliveryReviewID" 
                OnRowDataBound="SetClientOnClicks"  
                EnableMouseHover="false"
                EnableViewState="false" AllowSorting="false" Width="800px">               
                <Columns>               
                    <asp:TemplateField HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkView" runat="server" Text="click to view" Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <asp:TemplateField HeaderText="Delivery Review #" HeaderStyle-Width="80px" Visible="true" SortExpression="ProjectDeliveryReviewID">
                        <ItemTemplate>
                            <%# Eval("ProjectDeliveryReviewID") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Order #" HeaderStyle-Width="100px" Visible="true" SortExpression="Project.Order.OrderNumber">
                        <ItemTemplate>
                            <%# Eval("Project.Order.OrderNumber") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Project Name" HeaderStyle-Width="120px" SortExpression="Project.ProjectName">
                        <ItemTemplate>
                            <%# Eval("Project.ProjectName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sent To" HeaderStyle-Width="145px" SortExpression="IssuedToName">
                        <ItemTemplate>
                            <asp:Literal ID="litReviewIssuedTo" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sent On" HeaderStyle-Width="100px" SortExpression="IssuedDate" >
                        <ItemTemplate>
                            <%# String.Format("{0:dd-MMM-yyyy}", Eval("IssuedDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
        </wr:BaseGridView>    
      
</div>

<div class="wr-clearHack" style="margin-top: 5px">.</div>  
</asp:Content>
