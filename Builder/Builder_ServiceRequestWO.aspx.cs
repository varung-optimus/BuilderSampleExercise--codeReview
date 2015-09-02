using Conasys.HIP.BusinessManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRObjectModel;

namespace HomeOwner.app
{
    public partial class Builder_ServiceRequestWO : HIP_BuilderServiceBasePage
    {
        public static string GetUrl(int requestId)
        {
            return "~/app/builder/Builder_ServiceRequestWO.aspx?rid=" + requestId;
        }
        private bool IsPageRefresh = false;//To check whether the page is refreshed or not.   

        #region Properties
        private int _requestID;

        public int RequestID
        {
            set
            {
                _requestID = value;
            }
            get
            {
                if (Request.QueryString["rid"] != null)
                {
                    _requestID = Convert.ToInt32(Request.QueryString["rid"]);
                }
                return _requestID;
            }
        }

        private static int? _unitID;

        public int unitID
        {
            set
            {
                _unitID = value;
            }
            get
            {
                if (_unitID == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(_unitID);
                }
            }
        }
        #endregion

        #region Methods
        void LoadWorkOrderGrid()
        {
            //get the Work Orders for the current Request.

            //this.gridWorkOrders.DataSource = WRObjectModel.ServiceResource.GetWorkOrdersByRequestID(requestID);
            this.gridWorkOrders.DataBind();
        }
        #endregion

        #region Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();

                if (Request.QueryString["rid"] != null)
                {
                    RequestID = Convert.ToInt32(Request.QueryString["rid"]);
                    unitID = WRObjectModel.UnitServiceRequest.Get(RequestID).UnitID;
                }

                WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(RequestID);
                master.HIP_BuilderService builderMaster = (master.HIP_BuilderService)Page.Master;
                int userId = UserInfo.ByID;
                builderMaster.SetMasterPageValues(usr, "workorders", userId);
                LoadForm();

                CheckForLockedStatus(usr);
                LoadCheckBoxList();
                txtInstructionToVendor.Text = GetDescriptionText();

                FillToVendorDropDown();//Fills the To Vendor Drop Down with list  of specific companies types.
                FillContactNameEmailDropDown();//Fills the To Contact Name & Email Drop Down with respect to selected company.
                FillServiceRequestStatusDropDown();//Fills the Reequest Status Drop Down in the Modal Popup.
                ddlServiceRequestStatus.SelectedValue = "3";//By default selected to In progress.
            }
            else
            {
                if ((ViewState["postids"] != null) && (Session["postid"] != null))
                {
                    if (ViewState["postids"].ToString() != Session["postid"].ToString())
                    {
                        IsPageRefresh = true;
                    }
                    Session["postid"] = System.Guid.NewGuid().ToString();
                    ViewState["postids"] = Session["postid"];
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Create an event handler for the master page event when User updates Request details
            Master.ServiceRequestUpdated += new EventHandler(Master_ServiceRequestUpdated);
        }

        protected void LoadForm()
        {
            if (RequestID != 0)
            {

                LoadWorkOrderGrid();

            }
        }

        protected void CheckForLockedStatus(WRObjectModel.UnitServiceRequest usr)
        {
            //changes are NOT allowed for Merged, Denied or Complete Requests...disable controls!
            WRObjectModel.ServiceRequestStatus complete = WRObjectModel.ServiceRequestStatus.Get("complete");
            WRObjectModel.ServiceRequestStatus merged = WRObjectModel.ServiceRequestStatus.Get("merged");
            WRObjectModel.ServiceRequestStatus denied = WRObjectModel.ServiceRequestStatus.Get("non-warrantable");
            if (usr.ServiceRequestStatusID == complete.ServiceRequestStatusID ||
                usr.ServiceRequestStatusID == merged.ServiceRequestStatusID ||
                usr.ServiceRequestStatusID == denied.ServiceRequestStatusID)
            {

                //this.txtAppointmentDate.Enabled = false;
                //this.txtAssignedEmail.Enabled = false;
                //this.txtAssignedTo.Enabled = false;
                //this.txtDescription.Enabled = false;
                //this.txtNotifyText.Enabled = false;
                //this.timeAppointment.Enabled = false;
                //this.btnSaveWO.Enabled = false;

                this.lblDisabledMsg.Text = "Note: Requests with the status of " + usr.ServiceRequestStatus.Name + " can not be modified. ";
                this.lblDisabledMsg.Visible = true;

            }

        }

        protected void btnSaveWO_Click(object sender, EventArgs e)
        {
            if (!IsPageRefresh)
            {              

                string poJobId = string.Empty, toVendor = string.Empty, vendorContactEmail = string.Empty, instructionToVendor = string.Empty, ownerEmailText = string.Empty;
                int requestStatus = 0;
                DateTime appointmentDateTime = DateTime.MinValue;
                //bool attachExistingFiles = false;
                if (this.txtInstructionToVendor.Text.Length >= 3)
                {
                    HttpFileCollection fileCollection = HttpContext.Current.Request.Files;

                    // Create Document
                    if (fileCollection != null)
                    {
                        for (int i = 0; i < fileCollection.Count; i++)
                        {
                            HttpPostedFile uploadfile = fileCollection[i];

                            if (uploadfile.ContentLength != 0)
                            {
                                int fileSize = 0;
                                fileSize = fileSize + uploadfile.ContentLength;
                                if (fileSize > 10485760)
                                {
                                    string message = @"Files larger then 10 MB can not be processed. Please upload smaller files and try again";
                                    return;
                                }
                                string fileName = uploadfile.FileName;
                                // Get file name if it contains full path in case of IE browser
                                if (fileName.Contains(@"\"))
                                {
                                    string[] pathArr = fileName.Split('\\');
                                    fileName = pathArr.Last();
                                }
                                if (fileName.Length > 64)
                                {
                                    return;
                                }
                            }
                        }
                    }

                    IEnumerable<int> requestDocumentIds = chkRequestServiceFiles.Items.Cast<ListItem>()
                    .Where(i => i.Selected)
                    .Select(i => (Convert.ToInt32(i.Value)));

                    if (!string.IsNullOrEmpty(this.txtPOId.Text))
                    {
                        poJobId = this.txtPOId.Text;
                    }

                    var lstContactIds = new List<int>();
                    var lstEmailIds = new StringBuilder();
                    var lstContactNames = new StringBuilder();
                    string contactIds = Request.Form["ContactId"];
                    string contactNames = Request.Form["ContactName"];
                    if (contactIds != null)
                    {
                        lstContactIds = contactIds.Split(',').Select(int.Parse).ToList();
                    }
                    //  prepare list of emails
                    // prepare list of contactids
                    List<string> contacts = new List<string>();
                    if (contactNames != null)
                    {
                        contacts = contactNames.Split(',').ToList<string>();
                    }
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        var split = contacts[i].Split('(');
                        lstEmailIds.Append(String.Format(", {0}", split[1].TrimEnd(')')));
                        lstContactNames.Append(String.Format(", {0}", split[0]));
                    }

                    if (lstEmailIds.Length > 0)
                    {
                        lstEmailIds.Remove(0, 2);//Take Out the First Comma.
                    }
                    if (lstContactNames.Length > 0)
                    {
                        lstContactNames.Remove(0, 2);//Take Out the First Comma.
                    }
                    if (!string.IsNullOrEmpty(this.txtInstructionToVendor.Text))
                    {
                        instructionToVendor = this.txtInstructionToVendor.Text;
                    }
                    if (!string.IsNullOrEmpty(this.txtOwnerEmailText.Text))
                    {
                        ownerEmailText = this.txtOwnerEmailText.Text;
                    }
                    if (!string.IsNullOrEmpty(ddlServiceRequestStatus.SelectedValue))
                    {
                        requestStatus = Convert.ToInt32(ddlServiceRequestStatus.SelectedValue);
                    }
                    if (!string.IsNullOrWhiteSpace(txtAppointmentDate.Text))
                    {
                        appointmentDateTime = Convert.ToDateTime(Convert.ToDateTime(this.txtAppointmentDate.Text).ToLongDateString() + "," +
                                 this.timeAppointment.Date.ToLongTimeString());
                    }
                    var addEmails = txtAdditionalEmailAddress.Text;
                    var addtionalEmailAddresses = string.Empty;
                    if (!string.IsNullOrWhiteSpace(addEmails))
                    {
                        if (addEmails.Contains(','))
                        {
                            var emails = addEmails.Trim().Replace(",", ", ");
                            addtionalEmailAddresses = emails;
                        }
                        else if (addEmails.Contains(';'))
                        {
                            var emails = addEmails.Trim().Replace(";", ", ");
                            addtionalEmailAddresses = emails;
                        }
                        else
                        {
                            addtionalEmailAddresses = addEmails;
                        }
                        if (lstEmailIds.Length > 0)
                        {
                            lstEmailIds.Append(", ");
                        }
                        lstEmailIds.Append(addtionalEmailAddresses);
                    }

                    int companyId = 0;
                    int.TryParse(ddlToVendor.SelectedValue, out companyId);
                    if (Request.QueryString["rid"] != null)
                    {
                        RequestID = Convert.ToInt32(Request.QueryString["rid"]);
                    }
                    WRObjectModel.ServiceResource.InsertWorkOrder(RequestID, instructionToVendor, vendorContactEmail, ownerEmailText, poJobId, requestStatus, appointmentDateTime, addtionalEmailAddresses, companyId, lstContactIds, lstEmailIds.ToString(), fileCollection, requestDocumentIds, lstContactNames.ToString());
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), "clearInputs", "clearInputs();", true);
            var usr = WRObjectModel.UnitServiceRequest.Get(RequestID);
            Master.SetMasterPageValues(usr, "workorder", UserInfo.ByID); //Calling the Master Page function to set the default values in the dropdowns and text boxes.            
            gridWorkOrders.DataBind();
            txtInstructionToVendor.Text = GetDescriptionText();
        }

        protected void gridWorkOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UnitServiceRequestWorkOrder wo = (WRObjectModel.UnitServiceRequestWorkOrder)e.Row.DataItem;               
                string attachedFiles = string.Empty;

                List<Document> documents = ServiceResource.GetWorkOrderDocuments(wo.UnitServiceRequestWorkOrderID);
                if (documents.Count > 0)
                {

                    attachedFiles = string.Empty;
                    foreach (Document attachedFile in documents)
                    {
                        string file = "<a target='_blank' href=" + Document.GetUrl(attachedFile.DocumentID) + ">" +
                        attachedFile.Name + "." + attachedFile.FileExtension + "</a>";
                        attachedFiles = attachedFiles + file + " , ";
                    }
                    int index = attachedFiles.LastIndexOf(", ");
                    attachedFiles = attachedFiles.Remove(index);
                    Literal litFiles = (Literal)e.Row.FindControl("litFiles");
                    if (litFiles != null)
                    {
                        litFiles.Text = attachedFiles;
                    }
                }
                if (wo.CompanyID != null)
                {
                    var lblToVendor = (Label)e.Row.FindControl("lblToVendor");
                    if (lblToVendor != null)
                    {
                        var companyName = WRObjectModel.Company.GetCompanyNameByID(wo.CompanyID.Value);
                        if (!String.IsNullOrWhiteSpace(companyName))
                        {
                            lblToVendor.Text = companyName;
                        }
                    }
                }
                var woContacts = WRObjectModel.UnitServiceRequestWorkOrderContact.GetContactsByWorkOrderId(wo.UnitServiceRequestWorkOrderID);
                var lblAssignedToNameEmail = (Label)e.Row.FindControl("lblAssignedToNameEmail");
                if (woContacts.Count > 0)
                {
                    var assignedTo = new StringBuilder();
                    foreach (var c in woContacts)
                    {
                        if (!String.IsNullOrWhiteSpace(c.CompanyContact.Name))
                        {
                            assignedTo.Append(String.Format(",<br/>{0}", c.CompanyContact.Name));
                        }
                        if (!String.IsNullOrWhiteSpace(c.CompanyContact.Email))
                        {
                            assignedTo.Append(String.Format("<br/><span style='font-weight: 500;color: #6F6F6F;'>{0}</span>", c.CompanyContact.Email));
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(wo.AdditionalEmailAddress))
                    {
                        assignedTo.Append(String.Format(",<br/><span style='font-weight: 500;color: #6F6F6F;'>{0}</span>", wo.AdditionalEmailAddress));
                    }
                    if (assignedTo.Length > 0)
                    {
                        assignedTo.Remove(0, 6);//for Removing the first comma and <br/>
                    }
                    if (lblAssignedToNameEmail != null)
                    {
                        lblAssignedToNameEmail.Text = assignedTo.ToString();
                    }
                }
                else
                {
                    var nameEmail = new StringBuilder();
                    if (!String.IsNullOrWhiteSpace(wo.AssignedTo))
                    {
                        nameEmail.Append(wo.AssignedTo);
                    }
                    if (!String.IsNullOrWhiteSpace(wo.AssignedToEmail))
                    {
                        nameEmail.Append(String.Format("<br/><span style='font-weight: 500;color: #6F6F6F;'>{0}</span>", wo.AssignedToEmail));
                    }

                    if (!String.IsNullOrWhiteSpace(wo.AdditionalEmailAddress))
                    {
                        var additionalEmails = wo.AdditionalEmailAddress;
                        var formattedAdditionalEmails = additionalEmails;
                        if (additionalEmails.Contains(","))
                        {
                            formattedAdditionalEmails = additionalEmails.Replace(",", ",<br/>");
                        }
                        if (nameEmail.Length > 0)
                        {
                            nameEmail.Append(String.Format(",<br/><span style='font-weight: 500;color: #6F6F6F;'>{0}</span>", formattedAdditionalEmails));
                        }
                        else
                        {
                            nameEmail.Append(String.Format("<span style='font-weight: 500;color: #6F6F6F;'>{0}</span>", formattedAdditionalEmails));
                        }                      
                    }
                    lblAssignedToNameEmail.Text = nameEmail.ToString();
                }
            }
        }

        protected void gridWorkOrders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            //update Work Order...linqdatasource auto handles save but we can handle the funky stuff here

            //if status closed...then include ClosedDate
            if (Convert.ToInt32(e.NewValues["ServiceRequestWorkOrderStatusID"]) ==
                WRObjectModel.ServiceRequestWorkOrderStatus.Get("complete").ServiceRequestWorkOrderStatusID)
            {
                if (e.NewValues["CompletedOn"] == null)
                {
                    e.NewValues["CompletedOn"] = DateTime.Now;
                }
            }
            else
            {
                e.NewValues["CompletedOn"] = null;
            }

            if (e.NewValues["AppointmentOn"] == null)
            {
                e.NewValues["AppointmentOn"] = null;

            }
            else
            {
                //need to include the time with the AppointmentDate cause it cant be Bound (in current version)
                MKB.TimePicker.TimeSelector tp = (MKB.TimePicker.TimeSelector)this.gridWorkOrders.Rows[e.RowIndex].FindControl("timeAppointmentEdit");
                if (tp != null)
                {

                    e.NewValues["AppointmentOn"] = Convert.ToDateTime(e.NewValues["AppointmentOn"]).ToLongDateString() + "," + tp.Date.ToLongTimeString();

                }
            }


            this.gridWorkOrders.DataBind();

        }

        protected void SetNotifications(object sender, EventArgs e)
        {
            string msg = "<ul class='ulSmlBullet'>";

            if (WRObjectModel.ServiceResource.NotifyHomeownerOfWorkOrderAppointment(unitID))
            {

                msg = msg + "<li>An Appointment is scheduled</li>";
            }
            if (WRObjectModel.ServiceResource.NotifyHomeownerOfWorkOrderCreate(unitID))
            {

                msg = msg + "<li>A Work Order is created</li>";
            }
            if (WRObjectModel.ServiceResource.NotifyHomeownerOfWorkOrderClose(unitID))
            {

                msg = msg + "<li>A Work Order is marked complete</li>";
            }

            msg = msg + "</ul>";


            if (msg.Length > 0)
            {
                this.litNotifications.Text = msg;
            }
        }

        protected void gridWorkOrders_DataBound(object sender, EventArgs e)
        {
            //hide email column in edit mode...need the space!
            if (this.gridWorkOrders.EditIndex > -1)
            {
                this.gridWorkOrders.Columns[2].Visible = false;
            }
            else
            {
                this.gridWorkOrders.Columns[2].Visible = true;
            }

        }

        protected void ddlToVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkNameEmail.Items.Clear();
            FillContactNameEmailDropDown();
        }
        #endregion

        #region Private Methods
        private void LoadCheckBoxList()
        {
            List<Document> documents = ServiceResource.GetServiceDocumnets(RequestID);

            var documentQuery =
                  documents.Select(
                      d =>
                      new
                      {
                          DocumentID = d.DocumentID,
                          DisplayText = d.Name + "." + d.FileExtension
                      });
            chkRequestServiceFiles.DataSource = documentQuery;
            chkRequestServiceFiles.DataTextField = "DisplayText";
            chkRequestServiceFiles.DataValueField = "DocumentID";
            chkRequestServiceFiles.DataBind();
        }

        //this event is fired from the MasterPage when the user updates any Request data (like Status or Manager)
        private void Master_ServiceRequestUpdated(object sender, EventArgs e)
        {

            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(RequestID);

            CheckForLockedStatus(usr);
            //this.UpdatePanel1.Update();

        }

        private string GetDescriptionText()
        {
            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(RequestID);
            ServiceRequestRemarkType t = ServiceRequestRemarkType.Get("owner");
            UnitServiceRequestRemark r = ServiceResource.GetRemark(usr.UnitServiceRequestID, t);
            if (r != null)
            {
                string desciption = r.Remarks;

                return desciption;
            }
            return string.Empty;
        }

        /// <summary>
        /// Method to fill the To Company DropDown
        /// </summary>
        private void FillToVendorDropDown()
        {
            if (Request.QueryString["rid"] != null)
            {
                RequestID = Convert.ToInt32(Request.QueryString["rid"]);
            }

            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(RequestID);

            int projectId = usr.Unit.Phase.Project.ProjectID;
            List<int> listProjectIds = new List<int>();
            listProjectIds.Add(projectId);

            var lstCompanyTypeIds = new List<int>();
            lstCompanyTypeIds.Add(1);//Builder Company
            lstCompanyTypeIds.Add(3);//Contractor Company
            lstCompanyTypeIds.Add(6);//Installer Company
            lstCompanyTypeIds.Add(10);//Warranty Provider Company
            lstCompanyTypeIds.Add(11);//Supplier Company
            lstCompanyTypeIds.Add(15);//Service & Repair(business hours)
            lstCompanyTypeIds.Add(16);//Service & Repair(after hours)

            var lstCompany = ComponentInfo.GetCompanyByCompanyTypeIdAndProjectId(lstCompanyTypeIds, listProjectIds);
            var sortedList = lstCompany.OrderBy(o => o.Name).ToList();

            ddlToVendor.DataSource = sortedList;
            ddlToVendor.DataTextField = "Name";
            ddlToVendor.DataValueField = "ID";
            ddlToVendor.DataBind();

            ListItem li = new ListItem("None", "0");
            this.ddlToVendor.Items.Insert(0, li);
        }

        /// <summary>
        /// Method to fill Multiselect Name and Email Dropdown.
        /// </summary>
        private void FillContactNameEmailDropDown()
        {
            int companyId = int.Parse(ddlToVendor.SelectedValue);
            var lstContactEmail = CompanyContactsNameEmail.GetCompanyContactsNameEmail(companyId);
            chkNameEmail.DataSource = lstContactEmail;
            chkNameEmail.DataValueField = "ContactId";
            chkNameEmail.DataTextField = "ContactNameEmail";
            chkNameEmail.DataBind();
        }

        private void FillServiceRequestStatusDropDown()
        {
            var lstRequestStatus = ComponentInfo.GetServiceRequestStatus();//list of Service Request Status.

            ddlServiceRequestStatus.DataSource = lstRequestStatus;
            ddlServiceRequestStatus.DataTextField = "Name";
            ddlServiceRequestStatus.DataValueField = "ID";
            ddlServiceRequestStatus.DataBind();

            //disable Merged
            ListItem listItem = ddlServiceRequestStatus.Items.FindByText("Closed - Merged");
            ddlServiceRequestStatus.Items.Remove(listItem);
        }

        [System.Web.Services.WebMethod]
        public static List<ListItem> GetContacts(string name)
        {
            List<ListItem> constacts = new List<ListItem>();
            int companyId;
            int.TryParse(name, out companyId);
            var lstContactEmail = CompanyContactsNameEmail.GetCompanyContactsNameEmail(companyId);
            foreach (var contact in lstContactEmail)
            {
                constacts.Add(new ListItem
                {
                    Text = contact.ContactNameEmail.ToString(),
                    Value = contact.ContactId.ToString()
                });
            }

            return constacts;
        }
        #endregion

    }
}
