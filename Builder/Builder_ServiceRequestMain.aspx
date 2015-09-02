<%@ Page Title="Builder Portal - Service Request Details" Language="C#" MasterPageFile="~/master/HIP_BuilderService.Master"
    AutoEventWireup="true" CodeBehind="Builder_ServiceRequestMain.aspx.cs" Inherits="HomeOwner.app.Builder_ServiceRequestMain" %>

<%@ MasterType VirtualPath="~/master/HIP_BuilderService.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="JavaScript" type="text/javascript">
        <%--  var text = "";
        function getActiveText(e) {
            //sets text in ActionItem textbox to selectedtext    
            text = (document.all) ? document.selection.createRange().text : document.getSelection();
         //   var tb = $get("-<%=txtNewActionItem.ClientID%>");
       /    tb.value = tb.value + text;
            return true;
        }--%>

        var list = $get("<%=repeatRemarks.ClientID%>");
        list.onmouseup = getActiveText(this);

        document.onmouseup = getActiveText;
        if (!document.all) document.captureEvents(Event.MOUSEUP);

        function AddNewCommentToRequestorMessage() {
            return confirm('This comment will be sent by email to the requestor. This comment can also be viewed in the Homeowner Portal - Service Record page (if the option is turned on). Click "Ok" to proceed.');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
    <div id="divMain">
        <div style="border-bottom: 3px solid #dadada; margin: -12px 0 2px 0; padding: 5px 2px 2px 3px;"></div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="left selection" style="width: 47.7%; min-height: 200px;" onmouseup="getActiveText(this)">
                    <asp:Label ID="lblDisabledMsg" runat="server" Text="" ForeColor="#D50000" CssClass="medtext"
                        Width="99%"></asp:Label>
                    <h2 style="margin-top: 0px; border-bottom: 0px;">Internal Notes</h2>
                    <div class="wr-clearHack">
                        .
                    </div>
                    <div class="pad topSpc indent" style="padding-left: 0px; margin-left: 6px;">
                        <h5 align="left" class="light" style="margin-left: 6px;">New Note</h5>
                        <asp:TextBox ID="txtBuilderComment" runat="server" TextMode="MultiLine" Height="60px"
                            Width="96%" MaxLength="4000" Font-Size="0.9em" Font-Names="'Trebuchet MS', tahoma"
                            CssClass="line1">
                        </asp:TextBox>
                        <div class="right pad">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnNewRemark_Click" />
                        </div>
                        <div class="wr-clearHack">
                            .
                        </div>
                    </div>
                    <div class="line1" style="margin-bottom: 0px;" align="left">
                        <asp:Repeater ID="repeatRemarks" runat="server" OnItemDataBound="repeatRemarks_ItemDataBound"
                            DataSourceID="LinqDataSourceRemarks" OnItemCommand="repeatRemarks_ItemCommand">
                            <ItemTemplate>
                                <div class="pad" id="divRepeaterRow" runat="server" style="background-color: #DAE5F0; border: 1px solid #BDD6F0; width: 436px; margin-left: 6px; word-break: break-all; word-wrap: break-word;">
                                    <div class="pad">
                                        <div class="left bold" style="width: 150px">
                                            <asp:Literal ID="litBuilderName" runat="server"></asp:Literal>
                                        </div>
                                        <div class="right indent" style="margin-right: 5px;">
                                            <b><%#Eval("CreatedOn", "{0:MMMM d, yyyy H:mm}")%></b>
                                        </div>
                                        <br />
                                        <div class="wr-clearHack">
                                            .
                                        </div>
                                        <div class="text">
                                            <div style="float: left; margin-bottom: 8px;">
                                                <asp:Literal ID="litRemarks" runat="server" />
                                            </div>
                                            <div style="float: right;">
                                                <asp:LinkButton ID="lbDeleteRemark" CommandName="deleteRemark" CommandArgument='<%#Eval("UnitServiceRequestRemarkID") %>' runat="server" Style="float: right; margin-right: 5px; margin-bottom: 5px; font-size: 12pt; color: #929496" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="wr-clearHack">
                                        .
                                    </div>
                                </div>
                                <div style="border-bottom: hidden 1px silver; margin-bottom: 4px; margin-top: 4px;">
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="wr-clearHack">
                        .
                    </div>
                </div>
                <div class="left" style="width: 50%; padding-right: 0px; margin-left: 15px;" align="right">
                    <div align="left" class="indentdbl">
                        <div style="width: 215px" class="left" id="dvReqComm">
                            <h2 style="margin-top: 0px; border-bottom: 0px; width: 215px;">Requestor Communication</h2>
                        </div>
                        <div class="wr-clearHack">
                            .
                        </div>
                        <div class="pad topSpc indent" style="margin-left: 0px;">
                            <h5 align="left" class="light">New Comment to Requestor</h5>
                            <asp:TextBox ID="txtNewCommenttoRequestor" runat="server" TextMode="MultiLine" Height="60px"
                                Width="96%" MaxLength="4000" Font-Size="0.9em" Font-Names="'Trebuchet MS', tahoma"
                                CssClass="line1">
                            </asp:TextBox>
                            <div class="right pad">
                                <asp:Button ID="btnAddNewCommentToRequestor" runat="server" Text="Add" OnClick="btnAddNewCommenttoRequestor_Click" OnClientClick="return AddNewCommentToRequestorMessage()" />
                            </div>
                            <div class="wr-clearHack">
                                .
                            </div>
                        </div>
                        <div>
                            <asp:Repeater ID="rptCommentToRequestor" runat="server" DataSourceID="LinqDataSourceAI" OnItemDataBound="rptCommentToRequestor_ItemDataBound">
                                <ItemTemplate>
                                    <div class="pad" id="divRepeaterRow" runat="server" style="background-color: #F2F2F2; width: 430px; border: 1px solid #B9B9B9; min-height: 40px!important; margin-left: 6px; word-break: break-all; word-wrap: break-word;">
                                        <div class="pad" style="margin-left: -4px;">
                                            <div>
                                                <div class="left bold" style="width: 150px">
                                                    <asp:Literal ID="litCommentToRequestorBuilderName" runat="server"></asp:Literal>
                                                </div>
                                                <div class="right indent" style="margin-right: -1px;">
                                                    <b><%#Eval("CreatedOn", "{0:MMMM d, yyyy H:mm}")%></b>
                                                </div>
                                            </div>
                                            <div class="wr-clearHack">
                                                .
                                            </div>
                                            <div>
                                                <asp:Literal ID="litCommentToRequestor" runat="server" />
                                            </div>
                                            <br />
                                        </div>
                                    </div>
                                    <div class="wr-clearHack">
                                        .
                                    </div>
                                    <div style="border-bottom: hidden 1px silver; margin-bottom: 4px; margin-top: 4px;">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div>
                            <div id="userDeatils" style="min-height: 85px!important; width: 442px; background-color: #F2F2F2; border: 1px solid #B9B9B9; margin-left: 6px;">
                                <b>
                                    <div style="float: left; margin-top: 5px; margin-left: 9px;">
                                        <asp:Literal ID="litUsrName" runat="server" />
                                    </div>
                                </b>
                                <b>
                                    <div style="float: right; margin-right: 11px; margin-top: 5px;">
                                        <asp:Literal ID="litDate" runat="server" />
                                    </div>
                                </b>
                                <br />
                                <div class="wr-clearHack">
                                    .
                                </div>
                                <div style="margin-left: 9px;">
                                    <div style="word-break: break-all; word-wrap: break-word; margin-right: 6px;">
                                        <asp:Literal ID="litOwnerComment" runat="server"/>                                       
                                    </div>
                                    <asp:Literal ID="litLocation" runat="server" />
                                    <br />
                                    <asp:Literal ID="litProduct" runat="server" />
                                    <br />
                                </div>
                                <div class="wr-clearHack">
                                    .
                                </div>
                            </div>
                        </div>
                        <div style="border-bottom: hidden 1px silver; margin-bottom: 4px; margin-top: 4px;">
                        </div>
                        <div>
                            <asp:Repeater ID="rptRelatedRequests" runat="server" DataSourceID="LinqDataSourceDescription" OnItemDataBound="rptRelatedRequests_ItemDataBound">
                                <ItemTemplate>
                                    <div>
                                        <div id="userDeatils" style="min-height: 85px!important; width: 442px; background-color: #F2F2F2; border: 1px solid #B9B9B9; margin-left: 6px;">
                                            <b>
                                                <div style="float: left; margin-top: 5px; margin-left: 9px;">
                                                    <asp:Literal ID="litUsrName" runat="server" />
                                                </div>
                                            </b>
                                            <b>
                                                <div style="float: right; margin-right: 11px; margin-top: 5px;">
                                                    <asp:Literal ID="litDate" runat="server" />
                                                </div>
                                            </b>
                                            <br />
                                            <div class="wr-clearHack">
                                                .
                                            </div>
                                            <div style="margin-left: 9px;">
                                                <div style="word-break: break-all; word-wrap: break-word; margin-right: 6px;">
                                                    <asp:Literal ID="litOwnerComment" runat="server"/>
                                                </div>
                                                <asp:Literal ID="litLocation" runat="server" />
                                                <br />
                                                <asp:Literal ID="litProduct" runat="server" />
                                                <br />
                                            </div>
                                            <div class="wr-clearHack">
                                                .
                                            </div>
                                        </div>
                                    </div>
                                    <div style="border-bottom: hidden 1px silver; margin-bottom: 4px; margin-top: 4px;">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="wr-clearHack">
                    .
                </div>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="270">
                    <ProgressTemplate>
                        <div id="divProgressPopup" class="progressIndicator">
                            <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                            Please Wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:LinqDataSource ID="LinqDataSourceAI" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext"
            EnableUpdate="True" OrderBy="CreatedOn Desc" TableName="UnitServiceRequestActionItems"
            Where="UnitServiceRequestID == @UnitServiceRequestID" EnableDelete="True">
            <WhereParameters>
                <asp:QueryStringParameter Name="UnitServiceRequestID" QueryStringField="rid" Type="Int32" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="LinqDataSourceDescription" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext"
            EnableUpdate="True" OrderBy="CreatedOn Desc" TableName="UnitServiceRequestRelatedRequests"
            Where="ParentRequestID == @UnitServiceRequestID" EnableDelete="True">
            <WhereParameters>
                <asp:QueryStringParameter Name="UnitServiceRequestID" QueryStringField="rid" Type="Int32" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="LinqDataSourceRemarks" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext"
            TableName="UnitServiceRequestRemarks" Where="UnitServiceRequestID == @UnitServiceRequestID && ServiceRequestRemarkTypeID != 1"
            OrderBy="CreatedOn Desc">
            <WhereParameters>
                <asp:QueryStringParameter Name="UnitServiceRequestID" QueryStringField="rid" Type="Int32" />
            </WhereParameters>
        </asp:LinqDataSource>
    </div>
    <div class="wr-clearHack" style="margin-top: 5px">
        .
    </div>
</asp:Content>
