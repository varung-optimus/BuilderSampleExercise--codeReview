<%@ Page Title="Builder Portal - Request List" Language="C#" MasterPageFile="~/master/HIP.Master"
    AutoEventWireup="true" CodeBehind="Builder_ServiceRequestList.aspx.cs" Inherits="HomeOwner.app.Builder_ServiceRequestList" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../App_Themes/BuilderDefault/Stylesheet1.css" rel="stylesheet" />
    <script src="../../js/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.MultiFile.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_contentMain_dvCompletionDate").addClass("noDisplay");
            $("#ctl00_contentMain_ddlRequestStatus").change(function () {
                $("#ctl00_contentMain_dvCompletionDate").addClass("noDisplay");
                if ($("#ctl00_contentMain_ddlRequestStatus").val() == "4") {
                    $("#ctl00_contentMain_dvCompletionDate").removeClass("noDisplay");
                }

            });

            $("#ctl00_contentMain_divWOCompletionDate").addClass("noDisplay");
            $("#ctl00_contentMain_ddlWorkOrderStatus").change(function () {
                $("#ctl00_contentMain_divWOCompletionDate").addClass("noDisplay");
                if ($("#ctl00_contentMain_ddlWorkOrderStatus").val() == "3") {
                    $("#ctl00_contentMain_dvCompletionDate").removeClass("noDisplay");
                }
            });
            PrepareContactList();
        });

        function toggleCheckBox(thisCheckBox, multiSelectID, checkBoxIDs) {

            var arrayIDs;
            var arraySelected = [];
            var arraySelectIds = [];

            if (checkBoxIDs == null) {
                checkBoxIDs = $get("<%=hidCheckBoxIDs.ClientID%>");
                arrayIDs = checkBoxIDs.value.split(',');
            }
            else {
                arrayIDs = document.getElementById(checkBoxIDs).value.split(',');
            }

            var cbxMultiSelect = document.getElementById(multiSelectID);
            if (cbxMultiSelect == null) {
                cbxMultiSelect = multiSelectID;
            }



            if (thisCheckBox == cbxMultiSelect) {
                var cbx;
                for (var i = 0; i < arrayIDs.length; i++) {
                    cbx = document.getElementById(arrayIDs[i]);
                    if (cbx) {
                        if (!cbx.disabled) {
                            cbx.checked = thisCheckBox.checked;
                            if (cbx.checked) {
                                var eTableRow = cbx.parentNode.parentNode;
                                var eTableCells = eTableRow.getElementsByTagName("td");
                                arraySelected.push(' ' + eTableCells[1].innerHTML);
                                var requestId = (eTableCells[1].getElementsByTagName("a")[0].innerHTML);
                                arraySelectIds.push(requestId);

                            } else {
                                arraySelected = [];
                                arraySelectIds = []

                            }
                        }
                    }
                }
            } else {

                var cbx;
                for (var i = 0; i < arrayIDs.length; i++) {
                    cbx = document.getElementById(arrayIDs[i]);
                    if (cbx) {
                        if (!cbx.disabled) {
                            if (cbx.checked) {
                                var eTableRow = cbx.parentNode.parentNode;
                                var eTableCells = eTableRow.getElementsByTagName("td");
                                arraySelected.push(' ' + eTableCells[1].innerHTML);
                                var requestId = (eTableCells[1].getElementsByTagName("a")[0].innerHTML);
                                arraySelectIds.push(requestId);
                            }
                        }
                    }
                }

                if (thisCheckBox.checked) {
                    if (cbxMultiSelect) {
                        var allChecked = true;
                        for (var i = 0; i < arrayIDs.length; i++) {
                            allChecked = document.getElementById(arrayIDs[i]).checked;
                            if (!(allChecked))
                                break;
                        }
                        if (allChecked) {
                            cbxMultiSelect.checked = true;
                        }

                    }
                } else {
                    if (cbxMultiSelect) {
                        cbxMultiSelect.checked = false;
                    }
                }
            }




            //display list of selected Requests and show button if more than 1 selected
            var displayDiv = $get("<%=divMultiSelectIDs.ClientID%>");
            if (displayDiv) {
                if (arraySelected.length == 0) {
                    displayDiv.innerHTML = "<div style='font-size: 0.74em;' class='light'>(none selected)</div>";
                }
                else {
                    displayDiv.innerHTML = arraySelected;
                }
            }
            var lblReqIDs = $get("<%=lblRequestIDs.ClientID%>");
            if (lblReqIDs) {
                lblReqIDs.innerHTML = arraySelected;

                //set popup method parameter value
                var popext = $find("<%=ModalPopupExtender1.ClientID%>");
                if (popext) {
                    popext.set_dynamicContextKey(arraySelectIds.toString());
                }


                //set popup create workorder method parameter value
                var popext1 = $find("<%=ModalPopupExtenderCreateWO.ClientID%>");
                if (popext1) {
                    popext1.set_dynamicContextKey(arraySelectIds.toString());
                }
            }


            var lblCompletedReqIDs = $get("<%=lblCompleteRequestIDs.ClientID%>");
            if (lblCompletedReqIDs) {
                lblCompletedReqIDs.innerHTML = "Update Service Request ID’s</br>" + arraySelected;

            }

            var lblDeleteReqIDs = $get("<%=lblDeleteRequestIDs.ClientID %>");
            if (lblDeleteReqIDs) {
                lblDeleteReqIDs.innerHTML = "The following Request ID’s: " + arraySelected + " and its related work orders will be deleted.";
            }

            var lblExportTo = $get("<%=lblExportTo.ClientID%>");
            if (lblExportTo) {
                lblExportTo.innerHTML = "Select type of Export:";
            }

            var lblCreateWOIds = $get("<%=lblCreateWorkOrderIds.ClientID%>");
            var lblReqIdsOnly = $get("<%=lblReqIdsOnly.ClientID%>");
            if (lblCreateWOIds) {
                lblCreateWOIds.innerHTML = "Create Work Order for Service Request ID's " + arraySelected;
                $("[id*=ctl00_contentMain_chkNameEmail] tr").remove();
            }

            btn1 = $get("<%=btnMerge.ClientID%>");
            btn2 = $get("<%=btnReport.ClientID%>");
            btn3 = $get("<%=btnComplete.ClientID%>");
            btn4 = $get("<%=btnDelete.ClientID%>");
            btn5 = $get("<%=btnCreateWO.ClientID%>");

            if (btn1) {
                if (arraySelected.length > 1) {
                    btn1.disabled = false;
                }
                else {
                    btn1.disabled = true;
                }
            }
            if (btn2) {
                if (arraySelected.length > 0) {
                    btn2.disabled = false;
                }
                else {
                    btn2.disabled = true;
                }
            }
            if (btn3) {
                if (arraySelected.length > 0) {
                    btn3.disabled = false;
                }
                else {
                    btn3.disabled = true;
                }
            }
            if (btn4) {
                if (arraySelected.length > 0) {
                    btn4.disabled = false;
                }
                else {
                    btn4.disabled = true;
                }
            }
            if (btn4) {
                if (arraySelected.length > 0) {
                    btn5.disabled = false;
                }
                else {
                    btn5.disabled = true;
                }
            }
        }



        function disableBtn(btnID, newText) {
            var btn = document.getElementById(btnID);
            //setTimeout("setImage('" + btnID + "')", 10);
            //var btn = $get("<%=btnMerge.ClientID%>");
            btn.disabled = true;
            btn.value = newText;
        }

        function disableBtn(btnID) {
            var btn = document.getElementById(btnID);
            btn.disabled = true;
        }

        function toggleCheckBoxWO(thisCheckBox, multiSelectID, checkBoxIDs) {

            var arrayIDs;
            var arraySelected = [];
            var arraySelectIds = [];

            if (checkBoxIDs == null) {
                checkBoxIDs = $get("<%=hidWOChkBoxIDs.ClientID%>");
                arrayIDs = checkBoxIDs.value.split(',');
            }
            else {
                arrayIDs = document.getElementById(checkBoxIDs).value.split(',');
            }

            var cbxMultiSelect = document.getElementById(multiSelectID);
            if (cbxMultiSelect == null) {
                cbxMultiSelect = multiSelectID;
            }



            if (thisCheckBox == cbxMultiSelect) {
                var cbx;
                for (var i = 0; i < arrayIDs.length; i++) {
                    cbx = document.getElementById(arrayIDs[i]);
                    if (cbx) {
                        if (!cbx.disabled) {
                            cbx.checked = thisCheckBox.checked;
                            if (cbx.checked) {
                                var eTableRow = cbx.parentNode.parentNode;
                                var eTableCells = eTableRow.getElementsByTagName("td");
                                arraySelected.push(' ' + eTableCells[1].innerHTML);
                                var requestId = (eTableCells[1].getElementsByTagName("a")[0].innerHTML);
                                arraySelectIds.push(requestId);

                            } else {

                                arraySelected = [];
                                arraySelectIds = []

                            }
                        }
                    }
                }
            } else {

                var cbx;
                for (var i = 0; i < arrayIDs.length; i++) {
                    cbx = document.getElementById(arrayIDs[i]);
                    if (cbx) {
                        if (!cbx.disabled) {
                            if (cbx.checked) {
                                var eTableRow = cbx.parentNode.parentNode;
                                var eTableCells = eTableRow.getElementsByTagName("td");
                                arraySelected.push(' ' + eTableCells[1].innerHTML);
                                var requestId = (eTableCells[1].getElementsByTagName("a")[0].innerHTML);
                                arraySelectIds.push(requestId);
                            }
                        }
                    }
                }

                if (thisCheckBox.checked) {
                    if (cbxMultiSelect) {
                        var allChecked = true;
                        for (var i = 0; i < arrayIDs.length; i++) {
                            allChecked = document.getElementById(arrayIDs[i]).checked;
                            if (!(allChecked))
                                break;
                        }
                        if (allChecked) {
                            cbxMultiSelect.checked = true;
                        }

                    }
                } else {
                    if (cbxMultiSelect) {
                        cbxMultiSelect.checked = false;
                    }
                }
            }




            //display list of selected Requests and show button if any 1 is selected
            var displayDiv = $get("<%=divMultiSelectWOIds.ClientID%>");
            if (displayDiv) {
                if (arraySelected.length == 0) {
                    displayDiv.innerHTML = "<div style='font-size: 0.74em;' class='light'>(none selected)</div>";
                }
                else {
                    displayDiv.innerHTML = arraySelected;
                }
            }
            var lblUpdateWO = $get("<%=lblUpdateWOIDs.ClientID%>");
            if (lblUpdateWO) {
                lblUpdateWO.innerHTML = "Update Work Orders ID’s</br>" + arraySelected;

            }

            var lblDeleteWO = $get("<%=lblDeleteWOIDs.ClientID %>");
            if (lblDeleteWO) {
                lblDeleteWO.innerHTML = "The following Work Order ID’s: " + arraySelected + "will be deleted.";
            }

            var lblExportToWO = $get("<%=lblWOExportTo.ClientID%>");
            if (lblExportToWO) {
                lblExportToWO.innerHTML = "Select type of Export:";
            }

            btn1 = $get("<%=btnUpdateWO.ClientID%>");
            btn2 = $get("<%=btnDeleteWO.ClientID%>");
            btn3 = $get("<%=btnExportWO.ClientID%>");

            if (btn1) {
                if (arraySelected.length > 0) {
                    btn1.disabled = false;
                }
                else {
                    btn1.disabled = true;
                }
            }
            if (btn2) {
                if (arraySelected.length > 0) {
                    btn2.disabled = false;
                }
                else {
                    btn2.disabled = true;
                }
            }
            if (btn3) {
                if (arraySelected.length > 0) {
                    btn3.disabled = false;
                }
                else {
                    btn3.disabled = true;
                }
            }
        }

        if (!Array.indexOf) {
            Array.prototype.indexOf = function (obj) {
                for (var i = 0; i < this.length; i++) {
                    if (this[i] == obj) {
                        return i;
                    }
                }
                return -1;
            }
        }
        function PrepareContactList() {
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
                var contacts = r.d;
                var repeatColumns = parseInt("<%=chkNameEmail.RepeatColumns%>");
                if (repeatColumns == 0) {
                    repeatColumns = 1;
                }
                var cell = $("[id*=ctl00_contentMain_chkNameEmail] td").eq(0).clone(true);
                $("[id*=ctl00_contentMain_chkNameEmail] tr").remove();
                $.each(contacts, function (i) {
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
            };


        }
        window.onload = function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(jsFunctions);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PrepareContactList);
        }

        function jsFunctions() {
            $(document).ready(function () {
                $("#triangleLocation").on('click', function () {
                    $("#pnlLocation").slideToggle("slow")
                });
                $('#triangleLocation').on('click', function () {
                    $(this).toggleClass('triangle-down-request triangle-right-request')
                });
                $("#triangleProducts").on('click', function () {
                    $("#pnlProduct").slideToggle("slow")
                });
                $("#triangleProducts").on('click', function () {
                    $(this).toggleClass('triangle-down-request triangle-right-request')
                });
                $("#triangleService").on('click', function () {
                    $("#pnlService").slideToggle("slow")
                });
                $("#triangleService").on('click', function () {
                    $(this).toggleClass('triangle-down-request triangle-right-request')
                });
            });
        }

        $(document).ready(function () {
            $("#triangleLocation").on('click', function () {
                $("#pnlLocation").slideToggle("slow")
            });
            $('#triangleLocation').on('click', function () {
                $(this).toggleClass('triangle-down-request triangle-right-request')
            });
            $("#triangleProducts").on('click', function () {
                $("#pnlProduct").slideToggle("slow")
            });
            $("#triangleProducts").on('click', function () {
                $(this).toggleClass('triangle-down-request triangle-right-request')
            });
            $("#triangleService").on('click', function () {
                $("#pnlService").slideToggle("slow")
            });
            $("#triangleService").on('click', function () {
                $(this).toggleClass('triangle-down-request triangle-right-request')
            });
        });

        function clearInputs() {

            $("[id*=ctl00_contentMain_chkNameEmail] tr").remove();
            $("#ctl00_contentMain_txtPOJobIdCreateWO").val('');
            $("#ctl00_contentMain_txtInstructionToVendor").val('');
            $("#ctl00_contentMain_txtOwnerEmailText").val('');
            $("#ctl00_contentMain_cbAttachFiles").prop('checked', false);
            $("#ctl00_contentMain_ddlServiceRequestStatus").val(3);
            $("#ctl00_contentMain_ddlToVendor").val(0);
            $("#ctl00_contentMain_txtAppointmentDate").val('');
            $("#ctl00_contentMain_txtAdditionalEmailAddress").val('');
            var clearChecked = $(".FormText input");
            clearChecked.attr('checked', false);

        }

        function WorkOrderCreate() {
            $("#ctl00_contentMain_hdnFiles").val("");

            $("#ctl00_contentMain_chkRequestServiceFiles input[type=checkbox]").each(function (i, e) {
                if (this.checked) {
                    var hdnRequestDoc = $("#ctl00_contentMain_hdnFiles").val();
                    var newVal = hdnRequestDoc + $(e).prev().val() + ",";
                    $("#ctl00_contentMain_hdnFiles").val(newVal)

                }
            });
            return true;

        }

        function hideDate() {
            if ($("#ctl00_contentMain_ddlRequestStatus").val() == "4") {
                $("#ctl00_contentMain_dvCompletionDate").removeClass("noDisplay");
            }
            else {
                $("#ctl00_contentMain_dvCompletionDate").addClass("noDisplay");
            }
            if ($("#ctl00_contentMain_ddlWorkOrderStatus").val() == "3") {
                $("#ctl00_contentMain_divWOCompletionDate").removeClass("noDisplay");
            }
            else {
                $("#ctl00_contentMain_divWOCompletionDate").addClass("noDisplay");
            }
        }

        function hidePleaseWaitDiv() {
            $("#divProgressPopup").css("display", "none");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentMain" runat="server">
    <h2>Request List
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                DisplayAfter="240">
                <ProgressTemplate>
                    <div id="divProgressPopup" class="progressIndicator" style="margin-top: 35%; margin-left: -30px; left: 55%;">
                        <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" />
                        Please Wait...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel ID="pnlFilter" class="box2" runat="server" HorizontalAlign="Center" Visible="true"
                DefaultButton="btnFilter">
                <div class="left" style="width: 60px; padding-left: 8px;" align="left">
                    <asp:Button ID="btnClearCriteria" runat="server" Text="Clear" OnClick="btnClearCriteria_Click"
                        Font-Size="8pt" TabIndex="2" CausesValidation="false" />
                </div>
                <div>
                    <br></br>
                    <div style="padding-left: 5px;">
                        <div id="triangleLocation" class="triangle-down-request">
                        </div>
                    </div>
                    <asp:Panel runat="server">
                        <div class="left" style="width: 650px; padding-bottom: 5px;" align="left">
                            <div class="searchFilterSections">
                                &nbsp;Location
                            </div>
                        </div>
                        <div class="wr-clearHack">
                            .
                        </div>
                        <div style="border-bottom: 2px solid; border-bottom-color: darkgrey; width: 705px; margin-bottom: 3px;"></div>
                        <div id="pnlLocation" style="padding-left: 10px;">
                            <table>
                                <tr>
                                    <td align="left" style="width: 78px;">Project:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProject" AutoPostBack="true" runat="server" Width="204px"
                                            AppendDataBoundItems="True" OnSelectedIndexChanged="onProjectChange">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td align="left" style="width: 100px;">Address:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" Width="200px" Style="width: 210px; margin-left: 10px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Unit:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnitNumber" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td align="left">Requestor:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRequestor" runat="server" Width="200px" Style="width: 210px; margin-left: 10px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Strata Lot #
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStrataLot" runat="server" Width="200px" />
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td align="left">Possession Date:
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>From
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromPossession" runat="server" Width="80px" ValidationGroup="date" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnFromPsnDate" runat="server" SkinID="cal" />
                                                    <asp:RegularExpressionValidator ID="dateFromPsnValidation" runat="server" ErrorMessage="Not a valid date!"
                                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$"
                                                        ControlToValidate="txtFromPossession" ValidationGroup="date">
                                                    </asp:RegularExpressionValidator>
                                                    <act:CalendarExtender runat="server" ID="calFromPsnDate" TargetControlID="txtFromPossession"
                                                        PopupButtonID="btnFromPsnDate" Format="dd-MMM-yyyy" />
                                                </td>
                                                <td>To
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToPossession" runat="server" Width="80px" ValidationGroup="date" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnToPsnDate" runat="server" SkinID="cal" />
                                                    <asp:RegularExpressionValidator ID="dateToPsnValidation" runat="server" ErrorMessage="Not a valid date!"
                                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$"
                                                        ControlToValidate="txtToPossession" ValidationGroup="date">
                                                    </asp:RegularExpressionValidator>
                                                    <act:CalendarExtender runat="server" ID="calToPsnDate" TargetControlID="txtToPossession"
                                                        PopupButtonID="btnToPsnDate" Format="dd-MMM-yyyy" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
                <div>
                    <div style="padding-left: 5px;">
                        <div id="triangleProducts" class="triangle-down-request">
                        </div>
                    </div>
                    <asp:Panel runat="server">
                        <div class="left" style="width: 650px; padding-bottom: 5px" align="left">
                            <div class="searchFilterSections">&nbsp;Product</div>
                        </div>
                        <div class="wr-clearHack">
                            .
                        </div>
                        <div style="border-bottom: 2px solid; border-bottom-color: darkgrey; width: 705px; margin-bottom: 3px;"></div>
                        <div id="pnlProduct" style="padding-left: 10px;">
                            <table>
                                <tr>
                                    <td align="left" style="width: 67px;">Category:
                                    </td>
                                    <td align="left">&nbsp;&nbsp;<asp:DropDownList ID="ddlCategory" runat="server" AppendDataBoundItems="True" AutoPostBack="true" Width="140px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
                                    </td>
                                    <td style="width: 25px;"></td>
                                    <td align="left">Type:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProductType" runat="server" AppendDataBoundItems="True" AutoPostBack="true" Width="140px" OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged" />
                                    </td>
                                    <td style="width: 45px;"></td>
                                    <td align="left">Model:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlModel" runat="server" AppendDataBoundItems="True" Width="140px" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="left" style="width: 73px;">Company:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlCompany" runat="server" AppendDataBoundItems="True" Width="210px" />
                                    </td>
                                    <td style="width: 60px;"></td>
                                    <td align="left">Assigned To:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAssignedTo" runat="server" Width="230px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>

                <div>
                    <div style="padding-left: 5px;">
                        <div id="triangleService" class="triangle-down-request">
                        </div>
                    </div>
                    <asp:Panel runat="server">
                        <div class="left" style="width: 650px; padding-bottom: 5px" align="left">
                            <div class="searchFilterSections">&nbsp;Service</div>
                        </div>
                        <div class="wr-clearHack">
                            .
                        </div>
                        <div style="border-bottom: 2px solid; border-bottom-color: darkgrey; width: 705px; margin-bottom: 3px;"></div>
                        <div id="pnlService" style="padding-left: 10px;">
                            <table>
                                <tr>
                                    <td align="left" style="width: 75px;">Request ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRequestID" runat="server" Width="70px" />
                                    </td>
                                    <td style="width: 3px;"></td>
                                    <td align="left" style="width: 85px;">PO/WO/Job ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPOWOJobId" runat="server" Width="65px" />
                                    </td>
                                    <td style="width: 1px;"></td>
                                    <td align="left" style="width: 80px;">Submit Date:
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>From
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" Width="80px" ValidationGroup="date" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnFromDate" runat="server" SkinID="cal" />
                                                    <asp:RegularExpressionValidator ID="dateFromValidation" runat="server" ErrorMessage="Not a valid date!"
                                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$"
                                                        ControlToValidate="txtFromDate" ValidationGroup="date">
                                                    </asp:RegularExpressionValidator>
                                                    <act:CalendarExtender runat="server" ID="calFromDate" TargetControlID="txtFromDate"
                                                        PopupButtonID="btnFromDate" Format="dd-MMM-yyyy" />
                                                </td>
                                                <td>To
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server" Width="80px" ValidationGroup="date" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnToDate" runat="server" SkinID="cal" />
                                                    <asp:RegularExpressionValidator ID="dateToValidation" runat="server" ErrorMessage="Not a valid date!"
                                                        Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$"
                                                        ControlToValidate="txtToDate" ValidationGroup="date">
                                                    </asp:RegularExpressionValidator>
                                                    <act:CalendarExtender runat="server" ID="calToDate" TargetControlID="txtToDate" PopupButtonID="btnToDate"
                                                        Format="dd-MMM-yyyy" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <table>
                                    <tr>
                                        <td width="150px;">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<br>Invalid ID.Numbers Only."
                                                ControlToValidate="txtRequestID" ValidationExpression="^\d{1,10}$" Display="Dynamic"
                                                ValidationGroup="date">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </table>
                            <table style="column-width: 80px;">
                                <div class="left topSpc" style="width: 590px;" align="left" visible="false" id="stateFilter"
                                    runat="server">
                                    <tr>
                                        <td colspan="5">
                                            <div style="border-bottom: 1px solid #CECECE;">
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">Service Type:
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="chkReviewState" runat="server" RepeatDirection="Horizontal"
                                                RepeatColumns="4" CellPadding="5" CssClass="filterCss">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <div style="border-bottom: 1px solid #CECECE;">
                                            </div>

                                        </td>
                                    </tr>
                                </div>
                                <tr>
                                    <td align="left">Request Status:
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkStatus" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                                            CellPadding="5" CssClass="filterCss">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div style="border-bottom: 1px solid #CECECE;">
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Priority:
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkPriority" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                                            CellPadding="5" CssClass="filterCss">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div style="border-bottom: 1px solid #CECECE;">
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">WO Status:
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkWorkOrder" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                                            CellPadding="5" CssClass="filterCss" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
                <div class="right" style="width: 600px; padding-right: 34px;" align="right">
                    <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click"
                        TabIndex="1" ValidationGroup="date" />
                </div>
                <div class="wr-clearHack">
                    .
                </div>
                <div class="wr-clearHack">
                    .
                </div>
            </asp:Panel>
            <act:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" TargetControlID="pnlFilter"
                SkinID="roundCorners">
            </act:RoundedCornersExtender>
            <asp:HiddenField ID="hidCheckBoxIDs" runat="server" />
            <asp:HiddenField ID="hidWOChkBoxIDs" runat="server" />
            <br />
            Display records:
            <asp:DropDownList AutoPostBack="true" ID="ddlViewAll" runat="server" OnSelectedIndexChanged="ddlViewAll_SelectedIndexChanged">
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="0">All</asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;<asp:Literal ID="litCount" runat="server"></asp:Literal>
            <br />
            <br />

            <h2 style="border-bottom: 0px; -webkit-margin-before: 0; -webkit-margin-after: 0; padding: 0px; border-collapse: collapse; border-spacing: 0px; margin-bottom: 0px; margin-top: 0px;">
                <asp:Menu ID="Menu1" Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab"
                    StaticSelectedStyle-CssClass="selectedtab" runat="server" OnMenuItemClick="Menu1_MenuItemClick" MaximumDynamicDisplayLevels="1">
                    <Items>
                        <asp:MenuItem Text="Service Requests" Value="0" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Work Orders" Value="1"></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </h2>
            <div style="border: 3px solid #297FD5; border-radius: 6px; max-height: 530px;">
                <asp:MultiView ID="mvSearchFilter" ActiveViewIndex="0" runat="server">
                    <div id="divMain" class="bigscrollBox" style="max-height: 390px; margin-bottom: 4px; overflow: auto;">
                        <asp:View ID="vwServiceRequests" runat="server">
                            <div style="max-height: 430px; overflow: auto;" id="dvRequestService">
                                <asp:GridView ID="gridRequests" runat="server" SkinID="gridWorkOrder" DataKeyNames="UnitServiceRequestID"
                                    AllowSorting="true" EmptyDataText="No Requests Found" OnSorting="gridRequests_Sorting" CssClass="wrap" AllowPaging="true" PageSize="50" OnPageIndexChanging="gridRequests_PageIndexChanging" PagerStyle-HorizontalAlign="Center" OnRowDataBound="gridRequests_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<input type='checkbox' id='gridRequests_chkSelectAll' name='gridRequests_chkSelectAll' onclick='toggleCheckBox(this,gridRequests_chkSelectAll,null)'"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20px">
                                            <ItemStyle Width="20px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SR ID" SortExpression="UnitServiceRequestID" HeaderStyle-Width="40px"
                                            ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkIDLink" runat="server" Text='<%# Eval("UnitServiceRequestID")%>'
                                                    NavigateUrl='<%# String.Format("~/app/builder/builder_servicerequestmain.aspx?rid={0}", Eval("UnitServiceRequestID")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO/WO ID" HeaderStyle-ForeColor="#0093d0" HeaderStyle-Width="50px"
                                            ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#Eval("WorkoredrIds") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address" SortExpression="Address" HeaderStyle-Width="100px"
                                            ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkSRViewHome" runat="server" Text='<%# Eval("UnitAddress")%>'
                                                    NavigateUrl='<%# String.Format("~/app/Welcome.aspx?uid={0}", Eval("UnitID")) %>' />
                                                <br />
                                                <asp:Label Text='<%#Eval("RequestorDisplayName") %>' ID="lblRequestorName" runat="server"> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Strata Lot #" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="Unit.LegalDescription">
                                            <ItemTemplate>
                                                <%# Eval("LegalDescription") == null ? String.Empty : Eval("LegalDescription").ToString().Length > 60 ? string.Concat(Eval("LegalDescription").ToString().Substring(0, 60), "...") : Eval("LegalDescription")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Days Open" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="70px" ItemStyle-Width="70px" HeaderStyle-CssClass="padding" ItemStyle-CssClass="padding" HeaderStyle-ForeColor="#0093d0">
                                            <ItemTemplate>
                                                <%--<%# String.Format("{0:d-MMM-yyyy}", Eval("SubmittedOn"))%>--%>
                                                <asp:Literal ID="litDaysOpen" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Type" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ServiceRequestPriorityID" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <%# Eval("ProjectReviewState") == null ? "Service Call" : Eval("ProjectReviewState") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ServiceRequestStatusID" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <%# Eval("ServiceRequestStatus")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product" HeaderStyle-ForeColor="#0093d0"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px"
                                            ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <%# Eval("ProductCategory")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-ForeColor="#0093d0" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <%# Eval("ServiceRemarksType").ToString().Length > 60 ? string.Concat(Eval("ServiceRemarksType").ToString().Substring(0, 60), "...") : Eval("ServiceRemarksType")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:View>
                        <asp:View ID="vwWorkOrders" runat="server">
                            <div style="max-height: 430px; overflow: auto;" id="dvWorkOrder">
                                <asp:GridView ID="gridWorkOrder" runat="server" SkinID="gridWorkOrder" DataKeyNames="UnitServiceRequestWorkOrderID"
                                    AllowSorting="true" EmptyDataText="No Work Orders Found" OnSorting="gridWorkOrder_Sorting" CssClass="wrap" AllowPaging="true" OnPageIndexChanging="gridWorkOrder_PageIndexChanging" PageSize="50" PagerStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<input type='checkbox' id='gridWorkOrder_chkSelectAllWO' name='gridWorkOrder_chkSelectAllWO' onclick='toggleCheckBoxWO(this,gridWorkOrder_chkSelectAllWO,null)'"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20px">
                                            <ItemStyle Width="20px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectWO" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WO ID" HeaderStyle-ForeColor="#0093d0" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px" HeaderStyle-Width="30px" SortExpression="UnitServiceRequestWorkOrderID">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkIDLink" runat="server" Text='<%# Eval("UnitServiceRequestWorkOrderID")%>'
                                                    Target="_blank" NavigateUrl='<%# String.Format("~/app/builder/Builder_ServiceRequestWO.aspx?rid={0}", Eval("UnitServiceRequestID")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="PO/Job ID" HeaderStyle-ForeColor="#0093d0" ItemStyle-Width="50px"
                                            HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <%# Eval("POJobID")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address" HeaderStyle-ForeColor="#0093d0" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="UnitServiceRequest.Unit.Address">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkSRViewHome" runat="server" Text='<%# Eval("UnitAddress")%>'
                                                    Target="_blank" NavigateUrl='<%# String.Format("~/app/Welcome.aspx?uid={0}", Eval("UnitID")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Strata Lot #" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="UnitServiceRequest.Unit.LegalDescription">
                                            <ItemTemplate>
                                                <%# Eval("LegalDescription") == null ? String.Empty : Eval("LegalDescription").ToString().Length > 60 ? string.Concat(Eval("LegalDescription").ToString().Substring(0, 60), "...") : Eval("LegalDescription")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assigned Date" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" SortExpression="AssignedOn"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-ForeColor="#0093d0" HeaderStyle-CssClass="padding" ItemStyle-CssClass="padding">
                                            <ItemTemplate>
                                                <%# String.Format("{0:d-MMM-yyyy}", Eval("AssignedOn"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request Priority" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                            HeaderStyle-ForeColor="#0093d0">
                                            <ItemTemplate>
                                                <%# Eval("ServiceRequestPriority")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="ServiceRequestWorkOrderStatus.Name"
                                            ItemStyle-Width="80px" HeaderStyle-ForeColor="#0093d0" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <%# Eval("ServiceRequestWorkOrderStatus")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assigned to" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="AssignedTo"
                                            ItemStyle-Width="70px" HeaderStyle-Width="70px" HeaderStyle-ForeColor="#0093d0">
                                            <ItemTemplate>
                                                <%# Eval("AssignedTo")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="StatementOfWork"
                                            ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderStyle-ForeColor="#0093d0">
                                            <ItemTemplate>
                                                <%# Eval("StatementOfWork").ToString().Length > 50 ? string.Concat(Eval("StatementOfWork").ToString().Substring(0, 50), "...") : Eval("StatementOfWork")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:View>
                    </div>
                </asp:MultiView>
                <div id="dvMerge" runat="server">
                    <asp:Panel ID="pnlMultiSelect" class="box2" runat="server">
                        <div style="width: 95%" class="indent">
                            <div class="indent">
                                <div class="medtext" style="padding-top: 8px;">
                                    Selected Request ID's
                                </div>
                                <div class="indent">
                                    <div id="divMultiSelectIDs" runat="server" class="important" style="overflow: auto; height: 40px;">
                                        <div style='font-size: 0.74em;' class='light'>
                                            (none selected)
                                        </div>
                                    </div>
                                    <div align="right" style="padding-bottom: 8px;">
                                        <asp:Button ID="btnCreateWO" runat="server" Text="Create WO" Enabled="false" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnComplete" runat="server" Text="Update" Enabled="false" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnReport" runat="server" Text="Export" Enabled="False" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Enabled="false" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnMerge" runat="server" Text="Merge" Enabled="False" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                    </div>
                                </div>
                            </div>
                            <div class="wr-clearHack">
                                .
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div id="dvWorkOrderSelectedRequests" runat="server">
                    <asp:Panel ID="pnlWOSelected" class="box2" runat="server">
                        <div style="width: 95%" class="indent">
                            <div class="indent">
                                <div class="medtext" style="padding-top: 8px;">
                                    Selected Work Order ID's
                                </div>
                                <div class="indent">
                                    <div id="divMultiSelectWOIds" runat="server" class="important" style="overflow: auto; height: 40px;">
                                        <div style='font-size: 0.74em;' class='light'>
                                            (none selected)
                                        </div>
                                    </div>
                                    <div align="right" style="padding-bottom: 8px;">
                                        <asp:Button ID="btnUpdateWO" runat="server" Enabled="false" Text="Update" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnExportWO" runat="server" Enabled="false" Text="Export" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnDeleteWO" runat="server" Enabled="false" Text="Delete" OnClientClick="disableBtn(this.id, 'Submitting...')" />
                                    </div>
                                </div>
                            </div>
                            <div class="wr-clearHack">
                                .
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div id="divCreateWorkOrder" runat="server" style="display: none; width: 930px; height: 530px; overflow-y: auto;" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left"><span style="margin-left: 10px;">Service Request - Work Order</span></h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    <div id="lblCreateWorkOrderIds" runat="server" class="light" style="height: 60px; overflow-y: auto;">
                    </div>
                    <div id="lblReqIdsOnly" runat="server" style="display: none"></div>
                    <div style="padding-left: 20px;">
                        <table>
                            <tr>
                                <td valign="top" style="width: 90px;" align="left">PO/Job ID
                                </td>
                                <td style="width: 63px;"></td>
                                <td valign="top" style="width: 200px;" align="left">To: Vendor
                                </td>
                                <td style="width: 120px;"></td>
                                <td valign="top" style="width: 251px;" align="left">To: Contact Name & Email
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtPOJobIdCreateWO" runat="server" Width="90px" MaxLength="10" />
                                </td>
                                <td style="width: 60px;">
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlToVendor" runat="server" Width="250px" />
                                    </td>
                                    <td style="width: 70px;"></td>
                                    <td valign="top">
                                        <div class="scroll_items" style="width: 350px;">
                                            <asp:CheckBoxList ID="chkNameEmail" runat="server" RepeatDirection="Vertical" RepeatColumns="1" BorderWidth="0"
                                                Width="300px" AppendDataBoundItems="true" CssClass="FormText">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td valign="top" style="width: 200px;" align="left">Service Request Status
                                </td>
                                <td style="width: 282px;"></td>
                                <td>Additional Email Addresses</td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:DropDownList ID="ddlServiceRequestStatus" runat="server" Width="200px" />
                                </td>
                                <td style="width: 144px;"></td>
                                <td>
                                    <asp:TextBox ID="txtAdditionalEmailAddress" runat="server" Width="358px" />
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
                                    <asp:TextBox ID="txtInstructionToVendor" runat="server" Width="569px" Height="60px" TextMode="MultiLine" Font-Names="'Trebuchet MS', tahoma"
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
                                <asp:HiddenField ID="hdnFiles" runat="server" />
                                <td colspan="5" valign="top">
                                    <div id="panelFiles" align="left" class="indent" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div align="right" style="position: relative; top: 20px; margin-bottom: 15px; right: 10px">
                        <asp:Button ID="btnOkCreateWO" runat="server" ValidationGroup="newWO" Text="Create" OnClick="btnOkCreateWO_Click"
                            Width="60px" UseSubmitBehavior="False" OnClientClick="if ( ! WorkOrderCreate()) return false;" />&nbsp;
                    <asp:Button ID="btnCancelCreateWO" runat="server" Text="Cancel" OnClientClick="clearInputs()" />
                    </div>
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderCreateWO" runat="server" TargetControlID="btnCreateWO"
                PopupControlID="divCreateWorkOrder" BackgroundCssClass="AJAX_ModalPopup_Background" DynamicControlID="panelFiles"
                CancelControlID="btnCancelCreateWO" DynamicContextKey="setinjavascript" DynamicServiceMethod="GetPopupWorkOrderContent">
            </act:ModalPopupExtender>
            <div id="divPopupConfirmMerge" style="display: none;" runat="server" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Confirm Merge Requests</h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    Request ID's:
                    <div id="lblRequestIDs" runat="server" class="light">
                    </div>
                </div>
                <div id="panelMerge" align="left" class="indent" runat="server">
                </div>
                <div>
                    <h6>Merge reason or comment</h6>
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" Width="250px"></asp:TextBox>
                </div>
                <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                    <asp:Button ID="btnProceedMerge" runat="server" Text="Merge" OnClick="btnProceedMerge_Click"
                        Width="135" UseSubmitBehavior="False" OnClientClick="disableBtn(this.id, 'Submitting...')" />&nbsp;
                    <asp:Button ID="btnCancelMerge" runat="server" Text="Cancel" />
                </div>
            </div>

            <act:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnMerge"
                PopupControlID="divPopupConfirmMerge" DynamicControlID="panelMerge" DynamicContextKey="setinjavascript"
                DynamicServiceMethod="GetPopupContent" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelMerge">
            </act:ModalPopupExtender>
            <div id="divPopupConfirmComplete" style="display: none; height: 610px;" runat="server"
                class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Update Requests</h2>
                <div class="important indentdbl" style="padding-bottom: 5px;" align="left">
                    <div id="lblCompleteRequestIDs" runat="server" class="light" style="height: 150px; overflow-y: auto;">
                    </div>
                </div>
                <div align="left" class="indent">
                    <label></label>
                </div>
                <div>
                    <table>
                        <tr>
                            <td style="width: 80px;" align="left">
                                <h6>Service Type</h6>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlServiceType" runat="server" Width="200px" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <h6>Request Priority</h6>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRequestPriority" runat="server" AppendDataBoundItems="True" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <h6>Request Status</h6>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRequestStatus" runat="server" AppendDataBoundItems="True" Width="200px" />
                            </td>
                        </tr>
                    </table>
                    <div id="dvCompletionDate" runat="server">
                        <div style="float: left; width: 95px; padding-left: 18px; text-align: left;">
                            Please specify a Completion Date
                        </div>
                        <div style="margin: 0 auto;">
                            <asp:TextBox ID="txtCompleteDate" runat="server" Width="150px" ValidationGroup="date"></asp:TextBox>
                            <asp:ImageButton ID="btnCompleteDate" runat="server" SkinID="cal" />
                            <div>
                                <asp:RegularExpressionValidator ID="completeDateValidation" runat="server" ErrorMessage="Not a valid date!"
                                    Display="Dynamic" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$"
                                    ControlToValidate="txtCompleteDate" ValidationGroup="date">
                                </asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <act:CalendarExtender runat="server" ID="calCompleteDate" TargetControlID="txtCompleteDate"
                            PopupButtonID="btnCompleteDate" Format="dd-MMM-yyyy" />
                    </div>
                </div>
                <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                    <asp:Button ID="btnOkUpdate" runat="server" Text="Ok" OnClick="btnOkUpdate_Click"
                        Width="60px" UseSubmitBehavior="False" OnClientClick="disableBtn(this.id)" />&nbsp;
                    <asp:Button ID="btnCancelUpdate" runat="server" Text="Cancel" />
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderComplete" runat="server" TargetControlID="btnComplete"
                PopupControlID="divPopupConfirmComplete" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelUpdate">
            </act:ModalPopupExtender>
            <div id="divPopupConfirmDelete" style="display: none;" runat="server" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Confirm Deletion of Requests</h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    <div id="lblDeleteRequestIDs" runat="server" class="light">
                    </div>
                </div>
                <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                    <asp:Button ID="btnOkDelete" runat="server" Text="Ok" OnClick="btnOkDelete_Click"
                        Width="60px" UseSubmitBehavior="False" OnClientClick="disableBtn(this.id)" />&nbsp;
                    <asp:Button ID="btnCancelDelete" runat="server" Text="Cancel" />
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" TargetControlID="btnDelete"
                PopupControlID="divPopupConfirmDelete" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelDelete">
            </act:ModalPopupExtender>
            <div id="divPopupExportTo" style="display: none;" runat="server" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Export To:</h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    <div id="lblExportTo" runat="server" class="light">
                    </div>
                </div>
                <div>
                    <table>
                        <tr>
                            <td align="left" style="padding-left: 10px;">
                                <asp:RadioButton ID="btnExportAllFields" GroupName="exportTo" runat="server" />
                            </td>
                            <td align="left">
                                <h6 style="margin-top: 3px;">Export All Fields</h6>
                            </td>
                            <td width="100px"></td>
                            <td>
                                <input type="image" src="../../App_Themes/globalImages/excel.png" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 10px;">
                                <asp:RadioButton ID="btnExportRecords" GroupName="exportTo" runat="server" />
                            </td>
                            <td align="left">
                                <h6 style="margin-top: 3px;">Export Records</h6>
                            </td>
                            <td width="100px"></td>
                            <td>
                                <input type="image" src="../../App_Themes/globalImages/pdf.png" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                    <asp:Button ID="btnOkExport" runat="server" Text="Ok" Width="60px" UseSubmitBehavior="False" OnClick="btnOkExport_Click" />&nbsp;
                    <asp:Button ID="btnCancelExport" runat="server" Text="Close" OnClientClick="hidePleaseWaitDiv()" />
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderExportTo" runat="server" TargetControlID="btnReport"
                PopupControlID="divPopupExportTo" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelExport">
            </act:ModalPopupExtender>
            <div id="divPopupUpdateWO" style="display: none; height: 550px;" runat="server" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Update Work Orders</h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    <div id="lblUpdateWOIDs" runat="server" class="light" style="height: 150px; overflow-y: auto;">
                    </div>
                </div>
                <div id="panelUpdateWO" align="left" class="indent" runat="server">
                </div>
                <div>
                    <table>
                        <tr>
                            <td width="100px" align="left">
                                <h6>Work Order Status</h6>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWorkOrderStatus" runat="server" Width="200px" DataSourceID="StatusDataSource"
                                    DataTextField="Name" DataValueField="ServiceRequestWorkOrderStatusID" />
                            </td>
                        </tr>
                    </table>
                    </br>
                    <div id="divWOCompletionDate" runat="server">
                        <div style="float: left; width: 95px; padding-left: 18px; text-align: left;">
                            Please specify a Completion Date
                        </div>
                        <div style="margin: 0 auto;">
                            <asp:TextBox ID="txtWOComletionDate" runat="server" ValidationGroup="date" Width="150px"></asp:TextBox>
                            <asp:ImageButton ID="btnWOCompletionDate" runat="server" SkinID="cal" />
                            <div>
                                <asp:RegularExpressionValidator ID="WOCompletionDateValidation" runat="server" ControlToValidate="txtWOComletionDate" Display="Dynamic" ErrorMessage="Not a valid date!" ValidationExpression="^(3[0-1]|2[0-9]|1[0-9]|0[1-9])[\s{1}|\/|-](Jan|JAN|jan|Feb|FEB|feb|Mar|MAR|mar|Apr|APR|apr|May|MAY|may|Jun|JUN|jun|Jul|JUL|jul|Aug|AUG|aug|Sep|SEP|sep|Oct|OCT|oct|Nov|NOV|nov|Dec|DEC|dec)[\s{1}|\/|-]\d{4}$" ValidationGroup="date">
                                </asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <act:CalendarExtender ID="calWOCompleteDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="btnWOCompletionDate" TargetControlID="txtWOComletionDate" />
                    </div>
                    <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                        <asp:Button ID="btnOkWOUpdate" runat="server" OnClick="btnOkWOUpdate_Click" OnClientClick="disableBtn(this.id)" Text="Ok" UseSubmitBehavior="False" Width="60px" />
                        &nbsp;
                        <asp:Button ID="btnCancelWOUpdate" runat="server" Text="Cancel" />
                    </div>
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderUpdateWO" runat="server" TargetControlID="btnUpdateWO"
                PopupControlID="divPopupUpdateWO" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelWOUpdate" />

            <div id="divPopupExportToWO" style="display: none;" runat="server" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Export To:</h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    <div id="lblWOExportTo" runat="server" class="light">
                    </div>
                </div>
                <div>
                    <table>
                        <tr>
                            <td align="left" style="padding-left: 10px;">
                                <asp:RadioButton ID="btnExportAllFieldsWO" GroupName="exportTo" runat="server" />
                            </td>
                            <td align="left">
                                <h6 style="margin-top: 3px;">Export All Fields</h6>
                            </td>
                            <td width="100px"></td>
                            <td>
                                <input type="image" src="../../App_Themes/globalImages/excel.png" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 10px;">
                                <asp:RadioButton ID="btnExportRecordsWO" GroupName="exportTo" runat="server" />
                            </td>
                            <td align="left">
                                <h6 style="margin-top: 3px;">Export Records</h6>
                            </td>
                            <td width="100px"></td>
                            <td>
                                <input type="image" src="../../App_Themes/globalImages/pdf.png" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                    <asp:Button ID="btnOKExportToWo" runat="server" Text="Ok" Width="60px" OnClick="btnOKExportToWo_Click" UseSubmitBehavior="False" />&nbsp;
                    <asp:Button ID="btnCancelExportToWO" runat="server" Text="Close" OnClientClick="hidePleaseWaitDiv()" />
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderExportToWO" runat="server" TargetControlID="btnExportWO"
                PopupControlID="divPopupExportToWO" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelExportToWO">
            </act:ModalPopupExtender>
            <div id="divPopupDeleteWO" style="display: none;" runat="server" class="AJAX_ModalPopup">
                <h2 class="topDblSpc" align="left">Confirm Deletion of Requests</h2>
                <div class="important indentdbl" style="padding-bottom: 30px;" align="left">
                    <div id="lblDeleteWOIDs" runat="server" class="light">
                    </div>
                </div>
                <div align="right" style="position: absolute; bottom: 10px; right: 10px">
                    <asp:Button ID="btnOkDeleteWO" runat="server" Text="Ok" OnClick="btnOkDeleteWO_Click"
                        Width="60px" UseSubmitBehavior="False" OnClientClick="disableBtn(this.id)" />&nbsp;
                    <asp:Button ID="btnCancelDeleteWO" runat="server" Text="Cancel" />
                </div>
            </div>
            <act:ModalPopupExtender ID="ModalPopupExtenderDeleteWO" runat="server" TargetControlID="btnDeleteWO"
                PopupControlID="divPopupDeleteWO" BackgroundCssClass="AJAX_ModalPopup_Background"
                CancelControlID="btnCancelDeleteWO">
            </act:ModalPopupExtender>
            <div class="wr-clearHack">
                .
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinqDataSource ID="StatusDataSource" runat="server" ContextTypeName="WRObjectModel.WarrantyDataContext"
        Select="new (ServiceRequestWorkOrderStatusID, Name)" TableName="ServiceRequestWorkOrderStatus">
    </asp:LinqDataSource>
</asp:Content>
