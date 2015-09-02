<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Builder_UnitDetails.aspx.cs"
    Title="Builder Portal - Property Details" Inherits="HomeOwner.app.Builder_UnitDetails"
    Theme="BuilderDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(jsFunctions);
        }
        function jsFunctions() {
            $(document).ready(function () {
                $("#triangleDetails").on('click', function () {
                    $("#pnlHomeDetails").slideToggle("slow")
                });
                $('#triangleDetails').on('click', function () {
                    $(this).toggleClass('triangle-down triangle-right')
                });
                $("#triangleDocuments").on('click', function () {
                    $("#pnlDocuments").slideToggle("slow")
                });
                $("#triangleDocuments").on('click', function () {
                    $(this).toggleClass('triangle-down triangle-right')
                });
            });
        }

        $(document).ready(function () {
            $("#triangleDetails").on('click', function () {
                $("#pnlHomeDetails").slideToggle("slow")
            });
            $('#triangleDetails').on('click', function () {
                $(this).toggleClass('triangle-down triangle-right')
            });
            $("#triangleDocuments").on('click', function () {
                $("#pnlDocuments").slideToggle("slow")
            });
            $("#triangleDocuments").on('click', function () {
                $(this).toggleClass('triangle-down triangle-right')
            });            
        });

        //function to show confirmation box when document delete is clicked.
        function deleteDocument() {            
            var choice = confirm('Are you sure you want to delete this document?');
            return choice;
            //$("#hdnChoice").val(choice)
        }
    </script>
</head>
<body onload="load()">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnChoice" runat="server" />
        <div id="wrap" style="width: 770px">
            <div id="wrap_border">
                <div id="divHipLogo" align="left">
                    <asp:HyperLink ID="lnkLobby" runat="server">
                        <asp:Image ID="imgHipLogo" runat="server" SkinID="hipLogoNew" />
                    </asp:HyperLink>
                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
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
                            <asp:HyperLink ID="lnkViewHome" runat="server" SkinID="homeOwner"></asp:HyperLink>
                            View Property in Online Portal </span>
                        <br />
                        <div class="wr-clearHack">
                            .
                        </div>
                        <span id="span1" runat="server" class="right" style="width: 222px;">
                            <asp:HyperLink ID="lnkProducts" runat="server" SkinID="BuildingProductsIcon"></asp:HyperLink>
                            View All Products for this Property </span>
                        <br />
                        <div class="wr-clearHack">
                            .
                        </div>
                        <span id="spanlnkRequests" runat="server" class="right" style="width: 222px;">
                            <asp:HyperLink ID="lnkRequests" runat="server" Target="_blank" SkinID="requestIcon"></asp:HyperLink>
                            View any Requests for this Property </span>
                        <br />
                        <div class="wr-clearHack">
                            .
                        </div>
                        <span class="right" style="width: 222px;">
                            <asp:HyperLink ID="lnkBackToProperties" Style="text-decoration: none" runat="server"
                                NavigateUrl="~/app/Builder/Builder_Homes.aspx">
           Return to Properties
                            </asp:HyperLink>
                        </span>
                        <div id="triangleDetails" class="triangle-down">
                        </div>
                        <h2>&nbsp;&nbsp;&nbsp;&nbsp;Property Details</h2>
                        <div class="wr-clearHack">
                            .
                        </div>
                        <asp:Panel ID="pnlHomeDetails" runat="server">
                            <br />
                            <asp:Panel ID="pnlHomeInformation" runat="server">
                                <div align="center" class="box2 pad">
                                    <div class="important bold left">
                                        <asp:Literal ID="litHomeID" runat="server" />
                                    </div>
                                    <div class="wr-clearHack">
                                        .
                                    </div>
                                </div>
                                <div class="wr-clearHack">
                                    .
                                </div>
                                <div align="center" class="pad">
                                    <div style="width: 640px; display: table-cell;">
                                        <asp:Image ID="imgProjectBanner" runat="server" alt="Project Banner"
                                            AlternateText="Header" SkinID="projectLogo" />
                                    </div>
                                </div>
                                <div class="topDblSpc">
                                    <div align="right" class="right">
                                        Possession Date:
                                    <asp:TextBox ID="txtPosssessionDate" runat="server" Width="90px"> </asp:TextBox>
                                        <asp:Button ID="btnUpdatePossessionDate" runat="server"
                                            OnClick="btnUpdatePossessionDate_Click" Text="Update" />
                                        <asp:RegularExpressionValidator ID="dateValidation" runat="server"
                                            ControlToValidate="txtPosssessionDate" Display="Dynamic"
                                            ErrorMessage="&lt;br /&gt;Not a valid date!"
                                            ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|Feb|FEB|Mar|MAR|Apr|APR|May|MAY|Jun|JUN|Jul|JUL|Aug|AUG|Sep|SEP|Oct|OCT|Nov|NOV|Dec|DEC)[\s{1}|\/|-]\d{4}$">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                    <act:CalendarExtender ID="calExtPossessionDate" runat="server"
                                        CssClass="ajaxCalendar" Format="dd-MMM-yyyy" PopupPosition="BottomRight"
                                        TargetControlID="txtPosssessionDate">
                                    </act:CalendarExtender>
                                    <div class="wr-clearHack">
                                        .
                                    </div>
                                    <br />
                                    <ul class="ulHorz">
                                        <li>
                                            <h4>Address</h4>
                                            <asp:Literal ID="litUnitAddress" runat="server" />
                                        </li>
                                        <li>
                                            <h4>Legal Description</h4>
                                            <asp:Literal ID="litUnitLegalDescription" runat="server" />
                                        </li>
                                        <li>
                                            <h4>Warranty Provider</h4>
                                            <asp:Literal ID="litUnitHomeWarrantyProvider" runat="server" />
                                        </li>
                                    </ul>
                                </div>
                                <div class="wr-clearHack" style="margin-top: 15px">
                                    .
                                </div>
                                <div align="left" style="padding-left: 40px;">
                                    <div style="width: 100%;">
                                        <div style="width: 50%; float: left;">
                                            <h5>Contacts</h5>
                                        </div>
                                        <div style="width: 50%; float: right; text-align: right;">
                                            <asp:HyperLink ID="lnkPreRegister1" runat="server" SkinID="preRegisterIcon"></asp:HyperLink>
                                            <asp:HyperLink ID="lnkPreRegister2" runat="server">Add owner (Pre-Register)</asp:HyperLink>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <asp:GridView ID="gridContacts" runat="server" DataKeyNames="UnitContactID"
                                        EmptyDataText="no contacts found" OnRowDataBound="gridContacts_RowDataBound"
                                        RowStyle-Height="20px" SkinID="grid" Width="90%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUserName" runat="server" />
                                                    &nbsp;
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUserEmail" runat="server" />
                                                    &nbsp;
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUserPhone" runat="server" />
                                                    &nbsp;
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                                HeaderText="Registration Status" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRegStatus" runat="server" />
                                                    &nbsp;
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkResetPassword" runat="server" Text="Reset Password" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="wr-clearHack">
                                    .
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        <br />
                        <div id="triangleDocuments" class="triangle-down">
                        </div>
                        <h2>&nbsp;&nbsp;&nbsp;&nbsp;Documents</h2>
                        <asp:Panel ID="pnlDocuments" runat="server">
                            <div align="left" style="padding-left: 40px;">
                                <asp:GridView ID="gridDocuments" runat="server" RowStyle-Height="20px" SkinID="grid" Width="90%" OnRowDataBound="gridDocuments_RowDataBound" OnRowCommand="btDelete_Command">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Name" ControlStyle-Width="270px">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkDocumentName" runat="server"
                                                    NavigateUrl='<%#Eval("DocUrl") %>' Style="text-decoration: underline;"
                                                    Target="_blank" Text=' <%#Eval("DocName")%>' />
                                                &nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litLocation" runat="server" Text='<%#Eval("DocLocation") %>' />
                                                &nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="Date Loaded" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDateLoaded" runat="server"
                                                    Text='<%#Eval("DateLoaded") %>' />
                                                &nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteDocument" CommandArgument='<%#Eval("DocId")+ ";" +Eval("DocType") %>' OnClientClick="return deleteDocument();"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <div id="footer">
                            <asp:HyperLink ID="lnkFooterLobby" runat="server" Text="Lobby" />
                            &nbsp;|
                        <asp:HyperLink ID="lnkTerms" runat="server" Text="Terms of Use" />
                            &nbsp;|
                        <asp:HyperLink ID="lnkPrivacy" runat="server" Text="Privacy Policy" />
                            <div class="alt" style="padding-top: 5px;">
                                Copyright ©2013 <a href="http://www.conasysinc.com">CONASYS Inc.</a>
                            </div>
                        </div>
                        <div class="wr-clearHack" style="margin-top: 9px">
                            .
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
