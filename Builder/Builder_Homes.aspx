<%@ Page Title="Builder Portal - Properties" Language="C#" MasterPageFile="~/master/HIP.Master"
    AutoEventWireup="True" CodeBehind="Builder_Homes.aspx.cs" Inherits="HomeOwner.app.Builder_Homes" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
    <h2>Properties</h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                DisplayAfter="250">
                <ProgressTemplate>
                    <div id="divProgressPopup" class="progressIndicator">
                        <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                        Please Wait...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <script type="text/javascript" language="javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                function EndRequestHandler(sender, args) {
                    if (args.get_error() != undefined) {
                        args.set_errorHandled(true);
                    }
                }
            </script>
            <div class="left medtext" style="width: 550px; padding-bottom: 10px" align="left">
                <div class="left dark bold">
                    Showing:
                </div>
                <div class="right" style="padding-left: 8px; width: 485px;">
                    <asp:Label ID="lblFilterDisplay" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div align="right" class="right pad" style="padding-top: 3px">
                <asp:LinkButton ID="btnShowSearch" runat="server" BorderStyle="None" TabIndex="3"
                    OnClick="btnShowSearch_Click" Font-Size="1.15em" Font-Bold="True" CssClass="altColor">Search</asp:LinkButton>
            </div>
            <div align="right" class="right">
                <asp:ImageButton ID="btnShowSearch2" runat="server" SkinID="search" OnClick="btnShowSearch_Click"
                    TabIndex="3" />
            </div>
            <div class="wr-clearHack topDblSpc">
                .
            </div>
            <asp:Panel ID="pnlFilter" class="box2" runat="server" HorizontalAlign="Center" Visible="false"
                DefaultButton="btnFilter">
                <div class="left" style="width: 60px; padding-left: 8px;" align="left">
                    <asp:Button ID="btnClearCriteria" runat="server" Text="Clear" OnClick="btnClearCriteria_Click"
                        Font-Size="8pt" TabIndex="2" />
                </div>
                <h3 style="padding: 0px; margin: 0px; padding-left: 275px;" align="left">Search for Properties</h3>
                <div class="wr-clearHack">
                    .
                </div>
                <div align="center">
                    <div style="width: 90%">
                        <div class="left" style="width: 250px; padding-bottom: 10px" align="left">
                            <h4 style="margin-bottom: 15px">By Project</h4>
                            <asp:DropDownList ID="ddlProject" runat="server" Width="250px" AppendDataBoundItems="True">
                            </asp:DropDownList>
                        </div>
                        <div class="left" style="width: 250px; margin-left: 90px;" align="left">
                            <h4 style="margin-bottom: 15px">By Unit</h4>
                            <label style="width: 50px; display: inline-table; display: inline-block;">
                                Unit #:
                            </label>
                            <asp:TextBox ID="txtUnitNumber" runat="server" Width="85px"></asp:TextBox><div style="height: 6px;">
                            </div>
                            <label style="width: 50px; display: inline-table; display: inline-block;">
                                Address:
                            </label>
                            <asp:TextBox ID="txtStreetAddress" runat="server" Width="185px"></asp:TextBox><div
                                style="height: 6px;">
                            </div>
                        </div>
                        <div class="left" style="width: 250px;" align="left">
                            <h4 style="margin-bottom: 15px">By Products Used</h4>
                            <div class="left" style="width: 275px;" align="left">
                                <label style="width: 90px; display: inline-table; display: inline-block;">
                                    Category:
                                </label>
                                <asp:DropDownList ID="drpCategory" runat="server" Width="157px" AppendDataBoundItems="True">
                                </asp:DropDownList>
                                <div style="height: 6px;">
                                </div>
                                <label style="width: 90px; display: inline-table; display: inline-block;">
                                    Model:
                                </label>
                                <asp:TextBox ID="txtProductName" runat="server" Width="150px"></asp:TextBox><div
                                    style="height: 6px;">
                                </div>
                                <label style="width: 90px; display: inline-table; display: inline-block;">
                                    Manufacturer:
                                </label>
                                <asp:TextBox ID="txtManufacturer" runat="server" Width="150px"></asp:TextBox>
                                <div style="height: 6px;">
                                </div>
                            </div>
                        </div>
                        <div class="left" style="width: 250px; margin-left: 90px;" align="left">
                            <h4 style="margin-bottom: 15px">By Contacts Involved</h4>
                            <label style="width: 50px; display: inline-table; display: inline-block;">
                                Name:
                            </label>
                            <asp:TextBox ID="txtContactName" runat="server" Width="185px"></asp:TextBox>
                            <div class="wr-clearHack">
                                .
                            </div>
                        </div>
                        <div class="right" style="width: 300px; padding-top: 35px;" align="right">
                            <asp:Button ID="btnFilter" runat="server" Text="Search" OnClick="btnFilter_Click"
                                TabIndex="1" />
                        </div>
                    </div>
                    <div class="wr-clearHack">
                        .
                    </div>
                </div>
            </asp:Panel>
            <div class="wr-clearHack" style="padding-top: 8px;">
                .
            </div>
            <act:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="pnlFilter"
                SkinID="roundCorners">
            </act:RoundedCornersExtender>
            <div class="left" style="width: 120px; padding-top: 20px;">
                <h3>Search Results</h3>
            </div>
            <div class="right" style="width: 555px; font-size: .8em;">
                <asp:Panel class="pnlLegend" ID="pnlLegend" runat="server" HorizontalAlign="Center" Width="560px" Height="55px"
                    BorderStyle="Dashed" BorderWidth="1" BorderColor="Silver">
                    <h6 style="padding: 0px; margin: 0px">Tools Legend</h6>
                    <div align="center" class="legendsContainer">
                        <div class="legends" style="width: 99%">
                            <asp:HyperLink ID="lnkLegendDeficiencyReviewForm" runat="server" SkinID="reviewIcon"></asp:HyperLink>
                            <label>= Deficiency Review Form</label>
                            <asp:HyperLink ID="lnkLegendDetails" runat="server" SkinID="detailsIcon"></asp:HyperLink>
                            <label>= Details</label>
                            <asp:HyperLink ID="lnkLegendViewHome" runat="server" SkinID="building"></asp:HyperLink>
                            <label>= View Property</label>
                            <asp:HyperLink ID="lnkLegendProducts" runat="server" SkinID="BuildingProductsIcon"></asp:HyperLink>
                            <label>= Products</label>
                            <asp:HyperLink ID="lnkLegendPreRegister" runat="server" SkinID="preRegisterIcon"></asp:HyperLink>
                            <label>= Register User</label>
                            <asp:HyperLink ID="lnkLegendAlerts" runat="server" SkinID="maintParticpationIcon"></asp:HyperLink>
                            <label>= Maintenance Alerts</label>
                        </div>
                    </div>
                </asp:Panel>
                <div class="wr-clearHack">
                    .
                </div>
            </div>
            <div class="wr-clearHack" style="padding-top: 5px;">
                .
            </div>
            <div style="height: 400px; overflow: auto">
                <asp:GridView ID="gridHomes" runat="server" SkinID="grid" Width="97%" DataKeyNames="UnitID"
                    RowStyle-Height="20px" OnRowDataBound="gridHomes_RowDataBound" AllowSorting="True"
                    OnSorting="gridHomes_Sorting" EmptyDataText="No Properties found that match your criteria." OnRowCommand="btReview_Command">
                    <RowStyle Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Project" HeaderStyle-Width="185px" SortExpression="Project">
                            <ItemTemplate>
                                <%# Eval("ProjectName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" SortExpression="Address">
                            <ItemTemplate>
                                <%# Eval("Address") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Strata Lot #" HeaderStyle-Width="75px" ItemStyle-Width="75px"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">    
                            <ItemTemplate>
                                <%# Eval("LegalDescription") == null ? String.Empty : Eval("LegalDescription").ToString().Length > 60 ? string.Concat(Eval("LegalDescription").ToString().Substring(0, 60), "...") : Eval("LegalDescription")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Possession Date" HeaderStyle-Width="100px" SortExpression="PossDate"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div align="center">
                                    <asp:HiddenField ID="hiddenUnitID" runat="server" Value=' <%# Eval("UnitID") %>' />
                                    <act:CalendarExtender runat="server" ID="calPosDate" TargetControlID="txtCalDate"
                                        PopupButtonID="btnEditPossDate" Format="MMM-dd-yyyy" />
                                    <div class="left">
                                        <asp:TextBox ID="txtCalDate" runat="server" Width="77px" BorderStyle="None" AutoPostBack="True"
                                            Text=' <%# String.Format("{0:MMM-dd-yyyy}", Eval("PossessionDate"))%>' OnTextChanged="PossDateChanged">
                                        </asp:TextBox>
                                    </div>
                                    <div class="left">
                                        <asp:ImageButton ID="btnEditPossDate" runat="server" SkinID="cal" />
                                    </div>
                                </div>
                                <div class="wr-clearHack">
                                    .
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Registered Owners" HeaderStyle-Width="80px" SortExpression=""
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Eval("RegisteredOwners") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tools" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDefeciencyReviewForm" runat="server" CommandArgument='<%# Eval("UnitID") %>' CommandName="DeficiencyStart" ToolTip="View Deficiency Review Form" Visible='<%# Eval("DisplayPDILink") %>'>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="../../images/review_tablet_icon.png" Style="border-width: 0px;" />
                                </asp:LinkButton>
                                <asp:HyperLink ID="lnkDetails" NavigateUrl='<%#Eval("UnitID","~/app/Builder/Builder_UnitDetails.aspx?uid={0}")%>' runat="server" SkinID="detailsIcon" ToolTip="Show Property Details"></asp:HyperLink>
                                <asp:HyperLink ID="lnkViewHome" NavigateUrl='<%# Eval("UnitID","~/app/Welcome.aspx?uid={0}") %>' runat="server" SkinID="building" ToolTip="View Property in Online Portal"></asp:HyperLink>
                                <asp:HyperLink ID="lnkProducts" NavigateUrl='<%# Eval("UnitID","~/app/Builder/Builder_Products.aspx?unitID={0}") %>' runat="server" SkinID="BuildingProductsIcon" ToolTip="Go to Products"></asp:HyperLink>
                                <asp:HyperLink ID="lnkPreRegister" NavigateUrl='<%# Eval("UnitID","~/app/Builder/Builder_PreRegister.aspx?uid={0}") %>' runat="server" SkinID="preRegisterIcon" ToolTip="Pre-Register Users"></asp:HyperLink>
                                <asp:HyperLink ID="lnkAlerts" runat="server" SkinID="maintParticpationIcon" Target="_blank"
                                    ToolTip="Maintenance Alerts" Visible='<%# Eval("UnitMaintenanceRecordsVisible") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="wr-clearHack" style="margin-top: 9px">
                .
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
