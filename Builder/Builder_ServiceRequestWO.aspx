<%@ Page Title="Builder Portal - Service Request Work Orders" Language="C#" MasterPageFile="~/master/HIP_BuilderService.Master"
    AutoEventWireup="true" CodeBehind="Builder_ServiceRequestWO.aspx.cs" Inherits="HomeOwner.app.Builder_ServiceRequestWO" %>

<%@ MasterType VirtualPath="~/master/HIP_BuilderService.Master" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
    <script src="../../js/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.MultiFile.js" type="text/javascript"></script>
    <script type="text/javascript">
        function clearInputs() {
            $("[id*=ctl00_contentMain_chkNameEmail] tr").remove();
            $("#ctl00_contentMain_txtPOId").val('');
            $("#ctl00_contentMain_txtOwnerEmailText").val('');
            $("#ctl00_contentMain_cbAttachFiles").prop('checked', false);
            $("#ctl00_contentMain_ddlServiceRequestStatus").val(3);
            $("#ctl00_contentMain_ddlToVendor").val(0);
            $("#ctl00_contentMain_txtAppointmentDate").val('');
            $("#ctl00_contentMain_txtAdditionalEmailAddress").val('');
            var clearChecked = $(".FormText input");
            clearChecked.attr('checked', false);
        }
        $(document).ready(function () {
            $("[id*=ctl00_contentMain_chkNameEmail] tr").remove();           
            $("[id*=ctl00_contentMain_chkRequestServiceFiles] input[type=checkbox]").each(function (i, e) {
                $(this).prop('checked', false);
            });
            $("#ctl00_contentMain_ddlToVendor").change(function () {
                var str = "";
                str = $(this).find('option:selected').val();
                if ($("#ctl00_contentMain_chkNameEmail > tbody tr").length == 0) {
                    var row = "<tr><td><input id='ctl00_contentMain_chkNameEmail_0' type='checkbox' name='ctl00$contentMain$chkNameEmail$0'></td></tr>";
                    $("[id*=ctl00_contentMain_chkNameEmail] tbody").append(row);
                }

                $.ajax({
                    type: "POST",
                    url: "Builder_ServiceRequestList.aspx/GetContacts",
                    data: '{name: "' + str + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert("error");
                    }
                });


            });

            function OnSuccess(r) {
                var fruits = r.d;
                var repeatColumns = parseInt("<%=chkNameEmail.RepeatColumns%>");
                if (repeatColumns == 0) {
                    repeatColumns = 1;
                }
                var cell = $("[id*=ctl00_contentMain_chkNameEmail] td").eq(0).clone(true);
                $("[id*=ctl00_contentMain_chkNameEmail] tr").remove();
                $.each(fruits, function (i) {
                    var row;
                    if (i % repeatColumns == 0) {
                        row = $("<tr />");
                        $("[id*=ctl00_contentMain_chkNameEmail] tbody").append(row);
                    } else {
                        row = $("[id*=ctl00_contentMain_chkNameEmail] tr:last-child");
                    }

                    var checkbox = $("input[type=checkbox]", cell);

                    //Set Unique Id to each CheckBox.
                    checkbox[0].id = checkbox[0].id.replace("0", i);

                    //Give common name to each CheckBox.
                    checkbox[0].name = "ContactId";

                    //Set the CheckBox value.
                    checkbox.val(this.Value);

                    var label = cell.find("label");
                    if (label.length == 0) {
                        label = $("<label class='lblEmailNames' />");
                    }

                    //Set the 'for' attribute of Label.
                    label.attr("for", checkbox[0].id);

                    //Set the text in Label.
                    label.html(this.Text);

                    //Append the Label to the cell.
                    cell.append(label);

                    //Append the cell to the Table Row.
                    row.append(cell);
                    cell = $("[id*=ctl00_contentMain_chkNameEmail] td").eq(0).clone(true);
                });

                $("[id*=ctl00_contentMain_chkNameEmail] input[type=checkbox]").click(function () {
                    var cell = $(this).parent();
                    var hidden = cell.find("input[type=hidden]");
                    var label = cell.find("label");
                    if ($(this).is(":checked")) {
                        //Add Hidden field if not present.
                        if (hidden.length == 0) {
                            hidden = $("<input type = 'hidden' />");
                            cell.append(hidden);
                        }
                        hidden[0].name = "ContactName";

                        //Set the Hidden Field value.
                        hidden.val(label.text());
                        cell.append(hidden);
                    } else {
                        $(cell).find(hidden).remove();
                    }
                });
            }
        });
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divMain">
                <h2>Service Request - Work Orders</h2>
                <div id="divTop" class="box pad" style="margin-bottom: 15px; margin-left: 18px; width: 95%; margin-bottom: 20px;"
                    align="center">
                    <div align="left">
                        <asp:Label ID="Label1" runat="server" Text="Work Orders are used to assign and schedule work.">
                        </asp:Label>
                    </div>
                </div>
                <asp:Label ID="lblDisabledMsg" runat="server" Text="" ForeColor="#D50000" CssClass="medtext"
                    Width="99%"></asp:Label>
                <div class="scrollBox topDblSpc">
                    <asp:GridView ID="gridWorkOrders" runat="server" SkinID="grid" DataSourceID="LinqDataSource1"
                        DataKeyNames="UnitServiceRequestWorkOrderID" EmptyDataText="No Work Orders exist for this Request"
                        OnRowDataBound="gridWorkOrders_RowDataBound" OnRowUpdating="gridWorkOrders_RowUpdating"
                        OnDataBound="gridWorkOrders_DataBound" CssClass="wrap">
                        <Columns>
                            <asp:BoundField DataField="UnitServiceRequestWorkOrderID" Visible="False" />
                            <asp:CommandField ShowEditButton="True" ValidationGroup="grid" />
                            <asp:TemplateField HeaderText="Work Order ID" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("UnitServiceRequestWorkOrderID")%>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="115px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PO/Job ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("POJobID")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned To" ItemStyle-Width="115px">
                                <ItemTemplate>
                                    <div>
                                        <asp:Label ID="lblToVendor" runat="server" />
                                    </div>
                                    <div>
                                        <asp:Label ID="lblAssignedToNameEmail" runat="server" />
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="115px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="On" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <%#Eval("AssignedOn", "{0:d}")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGridAssignedDate" runat="server" Width="80px" Text='<%#Bind("AssignedOn", "{0:dd-MMM-yyyy}")%>'>
                                    </asp:TextBox>
                                    <act:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtGridAssignedDate"
                                        Format="dd-MMM-yyyy" CssClass="ajaxCalendar" PopupPosition="TopLeft">
                                    </act:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="dateGridAssignValidation" runat="server" ErrorMessage="<br />Not a valid date!"
                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|Feb|FEB|Mar|MAR|Apr|APR|May|MAY|Jun|JUN|Jul|JUL|Aug|AUG|Sep|SEP|Oct|OCT|Nov|NOV|Dec|DEC)[\s{1}|\/|-]\d{4}$"
                                        ControlToValidate="txtGridAssignedDate" ValidationGroup="grid">
                                    </asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <ItemStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <%# Eval("StatementOfWork").ToString().Length > 50 ? string.Concat(Eval("StatementOfWork").ToString().Substring(0, 50), "...") : Eval("StatementOfWork")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtStatementOfWork" runat="server" Text='<%# Bind("StatementOfWork") %>'
                                        TextMode="MultiLine" Height="75px" Width="172px" Font-Names="tahoma" Font-Size="9pt"
                                        MaxLength="4000"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="gridDescriptionRequired" runat="server" ToolTip="Description is required."
                                        Display="Dynamic" ErrorMessage="<br />Description is required." ControlToValidate="txtStatementOfWork"
                                        ValidationGroup="grid">
                                    </asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Attachments" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:Literal ID="litFiles" runat="server"></asp:Literal>
                                </ItemTemplate>

                                <ItemStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Appointment Date" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("AppointmentOn", "{0:dd-MMM-yyyy  hh:mm tt}")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGridAppointmentDate" runat="server" Width="80px" Text='<%#Bind("AppointmentOn", "{0:dd-MMM-yyyy}")%>'>
                                    </asp:TextBox>
                                    <act:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtGridAppointmentDate"
                                        Format="dd-MMM-yyyy" CssClass="ajaxCalendar" PopupPosition="TopLeft">
                                    </act:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="dateGridAppointValidation" runat="server" ErrorMessage="<br />Not a valid date!"
                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|Feb|FEB|Mar|MAR|Apr|APR|May|MAY|Jun|JUN|Jul|JUL|Aug|AUG|Sep|SEP|Oct|OCT|Nov|NOV|Dec|DEC)[\s{1}|\/|-]\d{4}$"
                                        ControlToValidate="txtGridAppointmentDate" ValidationGroup="grid">
                                    </asp:RegularExpressionValidator>
                                    <MKB:TimeSelector ID="timeAppointmentEdit" runat="server" DisplaySeconds="False"
                                        Date='<%# Convert.ToDateTime(Eval("AppointmentOn"))%>'>
                                    </MKB:TimeSelector>
                                </EditItemTemplate>
                                <ItemStyle Width="90px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("ServiceRequestWorkOrderStatus.Name")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="StatusDataSource"
                                        DataTextField="Name" DataValueField="ServiceRequestWorkOrderStatusID" SelectedValue='<%# Bind("ServiceRequestWorkOrderStatusID") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemStyle Width="65px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completed" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle Width="85px" />
                                <ItemTemplate>
                                    <%#Eval("CompletedOn", "{0:dd-MMM-yyyy}")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGridCompleteDate" runat="server" Width="75px" Text='<%#Bind("CompletedOn", "{0:dd-MMM-yyyy}")%>'>
                                    </asp:TextBox>
                                    <act:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtGridCompleteDate"
                                        Format="dd-MMM-yyyy" CssClass="ajaxCalendar" PopupPosition="TopLeft">
                                    </act:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="dateGridCompleteValidation" runat="server" ErrorMessage="<br />Not a valid date!"
                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|Feb|FEB|Mar|MAR|Apr|APR|May|MAY|Jun|JUN|Jul|JUL|Aug|AUG|Sep|SEP|Oct|OCT|Nov|NOV|Dec|DEC)[\s{1}|\/|-]\d{4}$"
                                        ControlToValidate="txtGridCompleteDate" ValidationGroup="grid">
                                    </asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Trade Confirmed" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("TradeConfirmed").ToString().Equals ( "True") ?"Yes":"No"%>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="115px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Owner Confirmed" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("HOConfirmed").ToString().Equals("True") ? "Yes" : "No"%>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="115px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div>
                    <h3 align="left">Add a new Work Order</h3>
                    <div>
                        <div id="divNewWO" class="pad ">
                            <div style="padding-left: 20px;">
                                <table>
                                    <tr>
                                        <td valign="top" style="width: 90px;" align="left">PO/Job ID
                                        </td>
                                        <td style="width: 23px;"></td>
                                        <td valign="top" style="width: 200px;" align="left">To: Vendor
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td valign="top" style="width: 251px;" align="left">To: Contact Name & Email
                                        </td>
                                        <td style="width: 71px;"></td>
                                        <td valign="top" style="width: 140px;" align="left">Appointment Date
                                        </td>
                                        <td style="width: 20px;"></td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:TextBox ID="txtPOId" runat="server" Width="90px" MaxLength="10" />
                                        </td>
                                        <td style="width: 20px;">
                                            <td valign="top">
                                                <asp:DropDownList ID="ddlToVendor" runat="server" Width="200px" />
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td valign="top">
                                                <div class="scroll_items" style="width: 289px;">
                                                    <asp:CheckBoxList ID="chkNameEmail" runat="server" RepeatDirection="Vertical" RepeatColumns="1" BorderWidth="0"
                                                        Width="300px" AppendDataBoundItems="true" CssClass="FormText">
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtAppointmentDate" runat="server" Width="140px">
                                                </asp:TextBox>
                                                <asp:RegularExpressionValidator ID="dateValidation" runat="server" ControlToValidate="txtAppointmentDate"
                                                    Display="Dynamic" ErrorMessage="&lt;br /&gt;Not a valid date!" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|Feb|FEB|Mar|MAR|Apr|APR|May|MAY|Jun|JUN|Jul|JUL|Aug|AUG|Sep|SEP|Oct|OCT|Nov|NOV|Dec|DEC)[\s{1}|\/|-]\d{4}$"
                                                    ValidationGroup="newWO">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td valign="top" align="left">
                                                <MKB:TimeSelector ID="timeAppointment" runat="server" DisplaySeconds="False" Hour="12"
                                                    Minute="00">
                                                </MKB:TimeSelector>
                                            </td>
                                    </tr>
                                </table>
                                <br />
                                <table>
                                    <tr>
                                        <td valign="top" style="width: 200px;" align="left">Service Request Status
                                        </td>
                                        <td style="width: 54px;"></td>
                                        <td>Additional Email Addresses</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlServiceRequestStatus" runat="server" Width="200px" />
                                        </td>
                                        <td style="width: 144px;"></td>
                                        <td>
                                            <asp:TextBox ID="txtAdditionalEmailAddress" runat="server" Width="295px" />
                                            <asp:RegularExpressionValidator ID="AdditionalEmailRegExp" runat="server" ErrorMessage="&lt;br /&gt;Invalid Email"
                                                ValidationGroup="newWO" ControlToValidate="txtAdditionalEmailAddress" ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,; ])*)*$">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 551px;" valign="top">Description
                                        </td>
                                        <td style="width: 20px;" valign="top"></td>
                                        <td>Owner Email Text
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 551px;">
                                            <asp:TextBox ID="txtInstructionToVendor" runat="server" Width="645px" Height="60px" TextMode="MultiLine" Font-Names="'Trebuchet MS', tahoma"
                                                MaxLength="4000" />
                                            <asp:RequiredFieldValidator ID="DescriptionRequired" runat="server" ControlToValidate="txtInstructionToVendor"
                                                Display="Static" ErrorMessage="&lt;br /&gt;Description is required." ToolTip="Description is required."
                                                ValidationGroup="newWO">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtOwnerEmailText" runat="server" Width="243px" Height="60px" TextMode="MultiLine" Font-Names="'Trebuchet MS', tahoma"
                                                MaxLength="4000" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td valign="top">Attach Existing Files to Work Order
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" valign="top">
                                            <asp:CheckBoxList ID="chkRequestServiceFiles" runat="server" CssClass="requestFiles"></asp:CheckBoxList>
                                            <br />
                                            Attach Additional Files to Work Order
                                            <br />
                                            <asp:FileUpload ID="file_upload" class="multi" runat="server" Style="width: 90px; color: #fff;" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div align="right" style="position: relative; top: 10px; right: 10px">
                                <asp:Button ID="btnSaveWO" runat="server" OnClick="btnSaveWO_Click" Text="Create Work Order"
                                    ValidationGroup="newWO" />
                            </div>
                            <div class="wr-clearHack">
                                .
                            </div>
                        </div>
                    </div>                    
                </div>
                <div class="wr-clearHack">
                    .
                </div>
                <div align="left" style="width: 80%">
                    <div class="indentdbl hint" align="left" style="float: left;">
                        <asp:Label ID="Label2" runat="server" Text="The owner is automatically notified when: ">
                        </asp:Label>
                        <div>
                            <asp:Literal ID="litNotifications" runat="server" OnPreRender="SetNotifications" />
                        </div>
                    </div>
                </div>
                <div class="wr-clearHack" style="margin-top: 5px">
                    .
                </div>
                <br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DisplayAfter="250">
                    <ProgressTemplate>
                        <div id="divProgressPopup" class="progressIndicator">
                            <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                            Please Wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <act:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtAppointmentDate"
                    Format="dd-MMM-yyyy" CssClass="ajaxCalendar" PopupPosition="BottomRight">
                </act:CalendarExtender>
        </ContentTemplate>
        <Triggers>

            <asp:PostBackTrigger ControlID="btnSaveWO" />

        </Triggers>
    </asp:UpdatePanel>
    <asp:LinqDataSource ID="StatusDataSource" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext"
        Select="new (ServiceRequestWorkOrderStatusID, Name)" TableName="ServiceRequestWorkOrderStatus">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext"
        EnableUpdate="True" OrderBy="AssignedOn" TableName="UnitServiceRequestWorkOrders"
        Where="UnitServiceRequestID == @UnitServiceRequestID1">
        <WhereParameters>
            <asp:QueryStringParameter Name="UnitServiceRequestID1" QueryStringField="rid" Type="Int32" />
        </WhereParameters>
    </asp:LinqDataSource>
    <div class="wr-clearHack" style="margin-top: 5px">
        .
    </div>
</asp:Content>
