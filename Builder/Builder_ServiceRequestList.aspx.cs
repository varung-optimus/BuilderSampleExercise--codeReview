using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using WRObjectModel;
using System.Text.RegularExpressions;
using BuilderPortal;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using Conasys.HIP.BusinessManager.Model;
using System.Web.Services;

namespace HomeOwner.app
{
    public partial class Builder_ServiceRequestList : HIP_MasterBasePage
    {
        #region Static Methods

        public static string GetUrl()
        {
            return GetBaseURL();
        }

        public static string GetUrl(int projectID)
        {
            return GetBaseURL() + "?projectID=" + projectID.ToString();
        }

        public static string GetUrl(int projectId, int unitID)
        {
            return GetBaseURL() + "?projectID=" + projectId.ToString() + "&unitID=" + unitID.ToString();
        }

        public static string GetBaseURL()
        {
            return "/app/Builder/Builder_ServiceRequestList.aspx";
        }

        [System.Web.Services.WebMethodAttribute(),
        System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetPopupContent(string contextKey)
        {
            StringBuilder sb = new StringBuilder();

            //first, verify that all passed-in requestID's are from the same unit
            string[] theSplit = contextKey.ToString().Split(',');
            bool b = WRObjectModel.ServiceResource.IsSameUnit(Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); })); ;
            if (b == false)
            {
                sb.Append("<div class='important' style='color: red'>Not all Requests are from the same Home!");
                sb.Append("<div>Can not proceed with Merge.</div></div");
            }
            else
            {

                sb.Append("<div>The above requests will be combined into one.</div>");
                sb.Append("<div>Enter a comment (optional) and press Merge button.</div></div>");
            }

            return sb.ToString();
        }


        [System.Web.Services.WebMethodAttribute(),
     System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetPopupWorkOrderContent(string contextKey)
        {
            //first, verify that all passed-in requestID's are from the same unit



            string[] requestIds = contextKey.ToString().Split(',');


            var requestList = new List<int>();
            foreach (var requestId in requestIds)
            {
                int id;
                int.TryParse(requestId, out id);
                requestList.Add(id);
            }

            List<Document> documents = WRObjectModel.ServiceDocument.GetListOfServiceDocByRequestIds(requestList);
            var documentQuery =
                  documents.Select(
                      d =>
                      new
                      {
                          DocumentID = d.DocumentID,
                          DisplayText = d.Name + "." + d.FileExtension
                      });

            StringBuilder sb1 = new StringBuilder();
            Dictionary<int, string> dctFiles = new Dictionary<int, string>();
            // chkRequestServiceFiles.DataSource = dctFiles;
            // chkRequestServiceFiles.DataBind();

            sb1.Append("<table id='ctl00_contentMain_chkRequestServiceFiles' class='requestFiles' border='0'>");

            sb1.Append("<tbody>");


            //sb1.Append("<tr>");
            //sb1.Append("<td><input id='ctl00_contentMain_chkRequestServiceFiles_1' type='checkbox' name='ctl00$contentMain$chkRequestServiceFiles$1'><label for='ctl00_contentMain_chkRequestServiceFiles_1'>Desert.jpg</label></td>");
            //sb1.Append("</tr>");

            int i = 0;
            foreach (var document in documents)
            {
                sb1.Append("<tr>");
                sb1.Append("<td> <input type='hidden' value='" + document.DocumentID + "' name='fileVal" + i + "' id='fileVal" + i + "'><input id='ctl00_contentMain_chkRequestServiceFiles_" + i + "' type='checkbox' name='ctl00$contentMain$chkRequestServiceFiles$" + i + "'><label for='ctl00_contentMain_chkRequestServiceFiles_" + i + "'>" + string.Format("{0}.{1}", document.Name, document.FileExtension) + "</label></td>");
                sb1.Append("</tr>");
                i++;
            }
            sb1.Append("</tbody></table>");


            //string[] theSplit = contextKey.ToString().Split(',');
            //bool b = WRObjectModel.ServiceResource.IsSameUnit(Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); })); ;
            //if (b == false)
            //{
            //    sb.Append("<div class='important' style='color: red'>Not all Requests are from the same Home!");
            //    sb.Append("<div>Can not proceed with Merge.</div></div");
            //}
            //else
            //{

            //    sb.Append("<div>The above requests will be combined into one.</div>");
            //    sb.Append("<div>Enter a comment (optional) and press Merge button.</div></div>");
            //}

            return sb1.ToString();
        }

        #endregion

        private static int _serviceRequestCount;
        private static int _workOrderCount;

        #region Properties

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                {
                    ViewState["sortDirection"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }
        }

        /// <summary>
        /// Property to get the Service Request Count.
        /// </summary>
        public int ServiceRequestCount
        {
            set
            {
                _serviceRequestCount = value;
            }
            get
            {
                if (_serviceRequestCount == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(_serviceRequestCount);
                }
            }
        }

        /// <summary>
        /// Property to get the Work Order Count.
        /// </summary>
        public int WorkOrderCount
        {
            set
            {
                _workOrderCount = value;
            }
            get
            {
                if (_workOrderCount == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(_workOrderCount);
                }
            }
        }

        public DataKeyArray SelectedDataKeys
        {
            get { return GetSelectedDataKeys(); }
        }

        public DataKeyArray SelectedDataKeysWO
        {
            get { return GetSelectedDataKeysWO(); }
        }

        #endregion

        #region Methods

        protected override string GetSelectedResource()
        {
            return "Service";
        }

        protected override string GetSelectedResourceLink()
        {
            return GetBaseURL();
        }

        /// <summary>
        /// Method to Load the Service Requests Grid.
        /// </summary>
        /// <param name="sortExpr">Sorting Expression</param>
        /// <param name="sortDir">Sorting Direction</param>
        /// <param name="requestStatusIDs">List of Request Status Ids</param>
        /// <param name="wrkOrderStatusIds">List of Work Order Status Ids</param>
        /// <param name="priorityList">List of Priority Ids</param>
        /// <param name="reviewStates">List of Review States</param>
        /// <returns>Count of Service Requests</returns>
        protected int LoadServiceRequestGrid(string sortExpr, SortDirection sortDir, List<int> requestStatusIDs, List<int> wrkOrderStatusIds, List<int> priorityList, List<int> reviewStates)
        {
            //gather filters
            int reqID = 0;
            if (!string.IsNullOrEmpty(this.txtRequestID.Text)) reqID = Convert.ToInt32(this.txtRequestID.Text);
            string person = this.txtRequestor.Text;
            string address = this.txtAddress.Text;
            string unitNum = this.txtUnitNumber.Text;
            int projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            int category = Convert.ToInt32(this.ddlCategory.SelectedValue);
            string model = string.Empty;
            string productType = string.Empty;
            int companyId = 0;

            if (ddlModel.Items.Count > 0 && !(ddlModel.Text.Equals(" (ALL) ")))
            {
                model = ddlModel.SelectedValue;
            }

            if (ddlCompany.Items.Count > 0 && !(ddlCompany.Text.Equals(" (ALL) ")))
            {
                companyId = Convert.ToInt32(this.ddlCompany.SelectedValue);
            }

            if (!string.IsNullOrEmpty(ddlProductType.SelectedValue))
            {
                productType = this.ddlProductType.SelectedValue;
            }

            string assignedTo = this.txtAssignedTo.Text;
            string poWoJobId = this.txtPOWOJobId.Text;
            string strataLot = this.txtStrataLot.Text;


            //dates
            string date1 = this.txtFromDate.Text ?? null;
            string date2 = this.txtToDate.Text ?? null;
            string fromPsnDate = this.txtFromPossession.Text ?? null;
            string toPsnDate = this.txtToPossession.Text ?? null;


            DateTime? sdate = null;
            DateTime? edate = null;
            DateTime? sPsnDate = null;
            DateTime? ePsnDate = null;

            if (!string.IsNullOrEmpty(date1))
            {
                sdate = Convert.ToDateTime(date1);
            }
            if (!string.IsNullOrEmpty(date2))
            {
                edate = Convert.ToDateTime(date2);
            }
            if (!string.IsNullOrEmpty(fromPsnDate))
            {
                sPsnDate = Convert.ToDateTime(fromPsnDate);
            }
            if (!string.IsNullOrEmpty(toPsnDate))
            {
                ePsnDate = Convert.ToDateTime(toPsnDate);
            }

            //Display Priority filter separately
            string priority = " and with the priority of (";
            foreach (ListItem cb in this.chkPriority.Items)
            {
                if (cb.Selected == true)
                {
                    priority += cb.Text + ", ";
                    priorityList.Add(Convert.ToInt32(cb.Value)); //also add to list that is sent to LINQ
                }
            }
            if (priority.EndsWith("("))
            {
                priority = " with every Priority ";
                priorityList.Clear(); //so LINQ returns all status
            }

            if (reviewStates != null)
            {
                foreach (ListItem cb in chkReviewState.Items)
                {
                    if (cb.Selected)
                    {
                        var valuesServiceType = cb.Value.Split(',');
                        foreach (var value in valuesServiceType)
                        {
                            reviewStates.Add(Convert.ToInt32(value));
                        }
                    }
                }
            }

            //Display status filter serperately
            string status = " and with the Status of (";
            foreach (ListItem cb in this.chkStatus.Items)
            {
                if (cb.Selected == true)
                {
                    status += cb.Text + ", ";
                    requestStatusIDs.Add(Convert.ToInt32(cb.Value)); //also add to list that is sent to LINQ
                }
            }
            if (status.EndsWith("("))
            {
                status = " with every Status ";
                requestStatusIDs.Clear(); //so LINQ returns all status
            }


            var lstServiceRequest = ServiceRequestModel.GetProductList(UserInfo.ByID, reqID, projectID, "", person, unitNum, address, requestStatusIDs, "", "", sdate, edate,
                                                                                                sortExpr, sortDir, priorityList, category, reviewStates, productType, model, sPsnDate, ePsnDate,
                                                                                                strataLot, companyId, assignedTo, poWoJobId);

            gridRequests.DataSource = lstServiceRequest;
            gridRequests.DataBind();

            //to set all the drop downs to the default value.
            ddlServiceRequestStatus.SelectedValue = "3";
            ddlRequestStatus.SelectedIndex = 0;
            ddlRequestPriority.SelectedIndex = 0;
            ddlServiceType.SelectedIndex = 0;
            ddlWorkOrderStatus.SelectedIndex = 0;

            ServiceRequestCount = lstServiceRequest.Count;
            return ServiceRequestCount;
        }

        /// <summary>
        /// Method to Load the Work Orders Grid.
        /// </summary>
        /// <param name="sortExpr">Sorting Expression</param>
        /// <param name="sortDir">Sorting Direction</param>
        /// <param name="requestStatusIDs">List of Request Status Ids</param>
        /// <param name="wrkOrderStatusIds">List of Work Order Status Ids</param>
        /// <param name="priorityList">List of Priority Ids</param>
        /// <param name="reviewStates">List of Review States</param>
        /// <returns>Count of Service Requests</returns>
        protected int LoadWorkOrdersGrid(string sortExpr, SortDirection sortDir, List<int> requestStatusIDs, List<int> wrkOrderStatusIds, List<int> priorityList, List<int> reviewStates)
        {
            //gather filters
            int reqID = 0;
            if (!string.IsNullOrEmpty(this.txtRequestID.Text)) reqID = Convert.ToInt32(this.txtRequestID.Text);
            string person = this.txtRequestor.Text;
            string address = this.txtAddress.Text;
            string unitNum = this.txtUnitNumber.Text;
            int projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            int category = Convert.ToInt32(this.ddlCategory.SelectedValue);
            string model = string.Empty;
            string productType = string.Empty;
            int companyId = 0;

            if (ddlModel.Items.Count > 0 && !(ddlModel.Text.Equals(" (ALL) ")))
            {
                model = ddlModel.SelectedValue;
            }

            if (ddlCompany.Items.Count > 0 && !(ddlCompany.Text.Equals(" (ALL) ")))
            {
                companyId = Convert.ToInt32(this.ddlCompany.SelectedValue);
            }

            if (!string.IsNullOrEmpty(ddlProductType.SelectedValue))
            {
                productType = this.ddlProductType.SelectedValue;
            }

            string assignedTo = this.txtAssignedTo.Text;
            string poWoJobId = this.txtPOWOJobId.Text;
            string strataLot = this.txtStrataLot.Text;


            //dates
            string date1 = this.txtFromDate.Text ?? null;
            string date2 = this.txtToDate.Text ?? null;
            string fromPsnDate = this.txtFromPossession.Text ?? null;
            string toPsnDate = this.txtToPossession.Text ?? null;


            DateTime? sdate = null;
            DateTime? edate = null;
            DateTime? sPsnDate = null;
            DateTime? ePsnDate = null;

            if (!string.IsNullOrEmpty(date1))
            {
                sdate = Convert.ToDateTime(date1);
            }
            if (!string.IsNullOrEmpty(date2))
            {
                edate = Convert.ToDateTime(date2);
            }
            if (!string.IsNullOrEmpty(fromPsnDate))
            {
                sPsnDate = Convert.ToDateTime(fromPsnDate);
            }
            if (!string.IsNullOrEmpty(toPsnDate))
            {
                ePsnDate = Convert.ToDateTime(toPsnDate);
            }

            //Display Priority filter separately
            string priority = " and with the priority of (";
            foreach (ListItem cb in this.chkPriority.Items)
            {
                if (cb.Selected == true)
                {
                    priority += cb.Text + ", ";
                    priorityList.Add(Convert.ToInt32(cb.Value)); //also add to list that is sent to LINQ
                }
            }
            if (priority.EndsWith("("))
            {
                priority = " with every Priority ";
                priorityList.Clear(); //so LINQ returns all status
            }

            if (reviewStates != null)
            {
                foreach (ListItem cb in chkReviewState.Items)
                {
                    if (cb.Selected)
                    {
                        var valuesServiceType = cb.Value.Split(',');
                        foreach (var value in valuesServiceType)
                        {
                            reviewStates.Add(Convert.ToInt32(value));
                        }
                    }
                }
            }

            //Display status filter serperately
            string status = " and with the Status of (";
            foreach (ListItem cb in this.chkStatus.Items)
            {
                if (cb.Selected == true)
                {
                    status += cb.Text + ", ";
                    requestStatusIDs.Add(Convert.ToInt32(cb.Value)); //also add to list that is sent to LINQ
                }
            }
            if (status.EndsWith("("))
            {
                status = " with every Status ";
                requestStatusIDs.Clear(); //so LINQ returns all status
            }

            //Display status filter separately
            foreach (ListItem cb in this.chkWorkOrder.Items)
            {
                if (cb.Selected == true)
                {
                    wrkOrderStatusIds.Add(Convert.ToInt32(cb.Value)); //also add to list that is sent to LINQ
                }
            }
            if (status.EndsWith("("))
            {
                status = " with every Status ";
                wrkOrderStatusIds.Clear(); //so LINQ returns all status
            }

            var lstWorkOrders = WorkOrdersModel.GetWorkordersByBuilder(UserInfo.ByID, reqID, projectID, "", person, unitNum, address, requestStatusIDs, wrkOrderStatusIds, "", sdate, edate,
                                                                                            sortExpr, sortDir, priorityList, assignedTo, poWoJobId, sPsnDate, ePsnDate, category, productType, model,
                                                                                            strataLot, reviewStates, companyId);
            gridWorkOrder.DataSource = lstWorkOrders;
            gridWorkOrder.DataBind();

            //to set all the drop downs to the default value.
            ddlServiceRequestStatus.SelectedValue = "3";
            ddlRequestStatus.SelectedIndex = 0;
            ddlRequestPriority.SelectedIndex = 0;
            ddlServiceType.SelectedIndex = 0;
            ddlWorkOrderStatus.SelectedIndex = 0;

            WorkOrderCount = lstWorkOrders.Count;
            return WorkOrderCount;
        }

        #endregion

        #region Private Methods

        private DataKeyArray GetSelectedDataKeys()
        {
            System.Collections.ArrayList keys = new System.Collections.ArrayList();

            CheckBox cbxMultiSelect = null;

            if (this.gridRequests.DataKeys.Count > 0)
            {
                foreach (GridViewRow gvr in this.gridRequests.Rows)
                {
                    cbxMultiSelect = (CheckBox)gvr.FindControl("chkSelect");

                    if ((cbxMultiSelect != null) && cbxMultiSelect.Checked)
                    {
                        keys.Add(this.gridRequests.DataKeys[gvr.RowIndex]);
                    }
                }
            }

            return new DataKeyArray(keys);
        }

        private DataKeyArray GetSelectedDataKeysWO()
        {
            System.Collections.ArrayList keys = new System.Collections.ArrayList();

            CheckBox cbxMultiSelect = null;

            if (this.gridWorkOrder.DataKeys.Count > 0)
            {
                foreach (GridViewRow gvr in this.gridWorkOrder.Rows)
                {
                    cbxMultiSelect = (CheckBox)gvr.FindControl("chkSelectWO");

                    if ((cbxMultiSelect != null) && cbxMultiSelect.Checked)
                    {
                        keys.Add(this.gridWorkOrder.DataKeys[gvr.RowIndex]);
                    }
                }
            }

            return new DataKeyArray(keys);
        }

        private void LoadStatusChkList()
        {
            var lstRequestStatus = ComponentInfo.GetServiceRequestStatus();//list of Service Request Status.

            this.chkStatus.DataSource = lstRequestStatus;
            this.chkStatus.DataTextField = "Name";
            this.chkStatus.DataValueField = "ID";
            this.chkStatus.DataBind();

            this.chkStatus.Items.FindByText("In-progress").Selected = true;
            this.chkStatus.Items.FindByText("Under-review").Selected = true;

            ListItem li = new ListItem(" (no change) ", "0");
            this.ddlRequestStatus.Items.Add(li);
            this.ddlRequestStatus.SelectedIndex = 0;

            ddlRequestStatus.DataSource = lstRequestStatus;
            ddlRequestStatus.DataTextField = "Name";
            ddlRequestStatus.DataValueField = "ID";
            ddlRequestStatus.DataBind();

            //disable Merged
            ListItem i = ddlRequestStatus.Items.FindByText("Closed - Merged");
            i.Attributes.Add("style", "color:silver;");
            i.Attributes.Add("style", "background-color:whitesmoke;");
            i.Attributes.Add("disabled", "true");

            li.Attributes.Add("style", "color:silver;");
            li.Attributes.Add("style", "background-color:whitesmoke;");

            ddlServiceRequestStatus.DataSource = lstRequestStatus;
            ddlServiceRequestStatus.DataTextField = "Name";
            ddlServiceRequestStatus.DataValueField = "ID";
            ddlServiceRequestStatus.DataBind();

            //disable Merged
            ListItem listItem = ddlServiceRequestStatus.Items.FindByText("Closed - Merged");
            ddlServiceRequestStatus.Items.Remove(listItem);
        }

        private void FillProjectListDropDown()
        {
            ListItem li = new ListItem(" (All) ", "0");
            this.ddlProject.Items.Add(li);

            ddlProject.DataSource = BuilderPortal.DataSources.GetProjectList(this.UserInfo.ByID);
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
        }

        /// <summary>
        /// Method to populate priority checklist
        /// </summary>
        private void LoadPriorityChkList()
        {
            var lstPriority = ComponentInfo.GetServiceRequestPriority();//List of Request Priority

            chkPriority.DataSource = lstPriority;
            chkPriority.DataTextField = "Name";
            chkPriority.DataValueField = "ID";
            chkPriority.DataBind();
            foreach (ListItem item in chkPriority.Items)
            {
                item.Selected = true;
            }

            ListItem li = new ListItem(" (no change) ", "0");
            this.ddlRequestPriority.Items.Add(li);
            this.ddlRequestPriority.SelectedIndex = 0;

            ddlRequestPriority.DataSource = lstPriority;
            ddlRequestPriority.DataTextField = "Name";
            ddlRequestPriority.DataValueField = "ID";
            ddlRequestPriority.DataBind();

            li.Attributes.Add("style", "color:silver;");
            li.Attributes.Add("style", "background-color:whitesmoke;");
        }

        /// <summary>
        /// Method to populate Work order status checklist
        /// </summary>
        private void LoadWorkOrderStatusChkList()
        {

            var lstWOStatus = ComponentInfo.GetServiceRequestWorkOrderStatus();

            chkWorkOrder.DataSource = lstWOStatus;
            chkWorkOrder.DataTextField = "Name";
            chkWorkOrder.DataValueField = "ID";
            chkWorkOrder.DataBind();
            chkWorkOrder.Items.FindByText("Pending").Selected = true;
            chkWorkOrder.Items.FindByText("In-progress").Selected = true;
        }

        /// <summary>
        /// Method to populate review state checklist
        /// </summary>
        private void LoadProjectReviewStateChklist()
        {
            ListItem li = new ListItem(" (no change) ", "0");
            this.ddlServiceType.Items.Add(li);
            this.ddlServiceType.SelectedIndex = 0;

            int projectId;
            int.TryParse(this.ddlProject.SelectedValue, out projectId);

            Dictionary<string, string> lstReviewStates = DataSources.GetProjectReviewStatesWithSameName(UserInfo.ByID);
            var result = WRObjectModel.ServiceResource.GetUnitServiceRequestsWithNullReviewStateByProjectIdAndUserId(projectId, UserInfo.ByID);

            if (result == true)
            {
                var serviceType = WRObjectModel.ServiceResource.GetHomeownerServiceTypeName();
                lstReviewStates.Add(serviceType, "-1");
            }

            if (lstReviewStates.Count > 0)
            {
                stateFilter.Visible = true;
                chkReviewState.DataSource = lstReviewStates;
                chkReviewState.DataTextField = "Key";
                chkReviewState.DataValueField = "Value";
                chkReviewState.DataBind();
                foreach (ListItem item in chkReviewState.Items)
                {
                    item.Selected = true;
                }

                this.ddlServiceType.DataSource = lstReviewStates;
                ddlServiceType.DataTextField = "Key";
                ddlServiceType.DataValueField = "Value";
                this.ddlServiceType.DataBind();
            }
            li.Attributes.Add("style", "color:silver;");
            li.Attributes.Add("style", "background-color:whitesmoke;");
        }

        /// <summary>
        /// Method to fill the Product Category Dropdown
        /// </summary>
        private void FillCategoryDropDown()
        {
            ListItem li = new ListItem(" (All) ", "0");
            ListItem liMisc = new ListItem("Miscellaneous", "-1");

            this.ddlCategory.Items.Add(li);
            this.ddlCategory.Items.Add(liMisc);
            ddlCategory.DataSource = ComponentInfo.GetCategoriesList();
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "ID";
            ddlCategory.DataBind();
        }

        /// <summary>
        /// Method to fill the Product Type Dropdown
        /// </summary>
        private void FillProductTypeDropDown()
        {
            int projectId;
            int.TryParse(this.ddlProject.SelectedValue, out projectId);
            List<int> listProjectIds = new List<int>();
            int categoryId;
            int.TryParse(this.ddlCategory.SelectedValue, out categoryId);

            if (projectId == 0)
            {
                foreach (ListItem i in this.ddlProject.Items)
                {
                    if (i.Value != "0")
                    {
                        listProjectIds.Add(Convert.ToInt32(i.Value));
                    }
                }
            }
            else
            {
                listProjectIds.Add(projectId);
            }

            ListItem li = new ListItem(" (All) ", "0");
            this.ddlProductType.Items.Add(li);

            Dictionary<string, string> lstcomponentTypes;
            lstcomponentTypes = WRObjectModel.ComponentType.GetProductsByProjectIdAndComponentId(listProjectIds, categoryId);
            if (lstcomponentTypes.Count > 0)
            {
                ddlProductType.DataSource = lstcomponentTypes;
                ddlProductType.DataTextField = "Key";
                ddlProductType.DataValueField = "Value";
                ddlProductType.DataBind();
            }
        }

        /// <summary>
        /// Method to fill the Product Model Dropdown
        /// </summary>
        private void FillModelDropDown()
        {
            int projectId;
            int.TryParse(this.ddlProject.SelectedValue, out projectId);
            List<int> listProjectIds = new List<int>();

            int parentCategoryId;
            int.TryParse(this.ddlCategory.SelectedValue, out parentCategoryId);

            int productTypeid;
            int.TryParse(this.ddlProductType.SelectedValue, out productTypeid);

            List<int> lstCategoryId = new List<int>();

            if (ddlCategory.SelectedValue != "0")
            {
                if (ddlProductType.SelectedValue == "0")
                {
                    foreach (ListItem item in ddlProductType.Items)
                    {
                        if (item.Value != "0")
                        {
                            int id = 0;
                            int.TryParse(item.Value, out id);
                            lstCategoryId.Add(id);
                        }
                    }
                }

                else
                {
                    lstCategoryId.Add(productTypeid);
                }
            }

            if (projectId == 0)
            {
                foreach (ListItem i in this.ddlProject.Items)
                {
                    if (i.Value != "0")
                    {
                        listProjectIds.Add(Convert.ToInt32(i.Value));
                    }
                }
            }
            else
            {
                listProjectIds.Add(projectId);
            }


            ListItem li = new ListItem(" (All) ", "0");
            this.ddlModel.Items.Add(li);

            List<string> lstModel = new List<string>();

            lstModel = WRObjectModel.Component.GetComponentsByProjectAndComponentId(listProjectIds, lstCategoryId);
            ddlModel.DataSource = lstModel;
            ddlModel.DataBind();
        }

        /// <summary>
        /// Method to fill the Product Company Dropdown
        /// </summary>
        private void FillCompanyDropDown()
        {
            int projectId;
            int.TryParse(this.ddlProject.SelectedValue, out projectId);
            List<int> lstProjectIds = new List<int>();
            if (projectId == 0)
            {
                foreach (ListItem i in this.ddlProject.Items)
                {
                    if (i.Value != "0")
                    {
                        lstProjectIds.Add(Convert.ToInt32(i.Value));
                    }
                }
            }
            else
            {
                lstProjectIds.Add(projectId);
            }

            var lstCompanyTypeIds = new List<int>();
            lstCompanyTypeIds.Add(1);//Builder Company
            lstCompanyTypeIds.Add(3);//Contractor Company
            lstCompanyTypeIds.Add(6);//Installer Company
            lstCompanyTypeIds.Add(10);//Warranty Provider Company
            lstCompanyTypeIds.Add(11); // Supplier Company
            lstCompanyTypeIds.Add(15);//Service & Repair(business hours)
            lstCompanyTypeIds.Add(16);//Service & Repair(after hours)

            var lstCompany = ComponentInfo.GetCompanyByCompanyTypeIdAndProjectId(lstCompanyTypeIds, lstProjectIds);
            var sortedList = lstCompany.OrderBy(o => o.Name).ToList();

            if (lstCompany.ToList().Count > 0)
            {
                ListItem li = new ListItem(" (All) ", "0");
                this.ddlCompany.Items.Add(li);

                ddlCompany.DataSource = sortedList;
                ddlCompany.DataTextField = "Name";
                ddlCompany.DataValueField = "ID";
                ddlCompany.DataBind();
            }

        }

        /// <summary>
        /// Method to perform some task when the view Index of Multi is changed.
        /// </summary>
        /// <param name="index"></param>
        private void mvSearchFilter_ActiveViewChange(int index)
        {
            ViewState["CallPrerenderCode"] = false;
            CheckBox cbxMultiSelect = null;
            if (index == 0)
            {
                mvSearchFilter.ActiveViewIndex = 0;
                gridRequests.Visible = true;
                gridWorkOrder.Visible = false;
                dvWorkOrderSelectedRequests.Visible = false;
                dvMerge.Visible = true;
                this.divMultiSelectIDs.InnerHtml = "<div style='font-size: 0.74em;' class='light'>(none selected)</div>";

                if (this.gridRequests.DataKeys.Count > 0)
                {
                    foreach (GridViewRow gvr in this.gridRequests.Rows)
                    {
                        cbxMultiSelect = (CheckBox)gvr.FindControl("chkSelect");

                        if ((cbxMultiSelect != null) && cbxMultiSelect.Checked)
                        {
                            cbxMultiSelect.Checked = false;
                        }
                    }
                }

                ddlViewAll.SelectedValue = "0";
                if (gridRequests.AllowPaging)
                {
                    ddlViewAll.SelectedValue = gridRequests.PageSize.ToString();
                }
                litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Displaying the total number of service requests. 
            }
            else
            {
                mvSearchFilter.ActiveViewIndex = 1;
                gridRequests.Visible = false;
                gridWorkOrder.Visible = true;
                dvWorkOrderSelectedRequests.Visible = true;
                dvMerge.Visible = false;
                this.divMultiSelectWOIds.InnerHtml = "<div style='font-size: 0.74em;' class='light'>(none selected)</div>";

                if (this.gridWorkOrder.DataKeys.Count > 0)
                {
                    foreach (GridViewRow gvr in this.gridWorkOrder.Rows)
                    {
                        cbxMultiSelect = (CheckBox)gvr.FindControl("chkSelectWO");

                        if ((cbxMultiSelect != null) && cbxMultiSelect.Checked)
                        {
                            cbxMultiSelect.Checked = false;
                        }
                    }
                }

                ddlViewAll.SelectedValue = "0";
                if (gridWorkOrder.AllowPaging)
                {
                    ddlViewAll.SelectedValue = gridWorkOrder.PageSize.ToString();
                }
                litCount.Text = String.Format("Total # of records: {0}", WorkOrderCount);//Displaying the total number of Work Orders.
            }
        }

        /// <summary>
        /// Method to fill the To Company DropDown
        /// </summary>
        private void FillToVendorDropDown()
        {
            int projectId;
            int.TryParse(this.ddlProject.SelectedValue, out projectId);
            List<int> listProjectIds = new List<int>();

            if (projectId == 0)
            {
                foreach (ListItem i in this.ddlProject.Items)
                {
                    if (i.Value != "0")
                    {
                        listProjectIds.Add(Convert.ToInt32(i.Value));
                    }
                }
            }
            else
            {
                listProjectIds.Add(projectId);
            }

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

        /// <summary>
        /// Function to fill the Attached files section.
        /// </summary>
        private void LoadCheckBoxList()
        {
            // List<Document> documents = ServiceResource.GetServiceDocumnets(RequestID);
            //get all selected requestIds and send to ReportGenerator
            StringBuilder selectedKeys = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeys())
            {
                selectedKeys.Append(", " + key[0].ToString());
            }

            if (selectedKeys.Length > 0)
            {
                selectedKeys.Remove(0, 2);  //take out first commma
                string[] theSplit = selectedKeys.ToString().Split(',');
                int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
                List<Document> documents = WRObjectModel.ServiceDocument.GetListOfServiceDocByRequestIds(arrayIDs.ToList());
                var documentQuery =
                      documents.Select(
                          d =>
                          new
                          {
                              DocumentID = d.DocumentID,
                              DisplayText = d.Name + "." + d.FileExtension
                          });
                //chkRequestServiceFiles.DataSource = documentQuery;
                //chkRequestServiceFiles.DataTextField = "DisplayText";
                //chkRequestServiceFiles.DataValueField = "DocumentID";
                //chkRequestServiceFiles.DataBind();
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var check = true;
            if (ViewState["CallPrerenderCode"] != null)
            {
                check = (bool)ViewState["CallPrerenderCode"];
            }

            if (check)
            {
                System.Collections.ArrayList arrayChkboxes = new System.Collections.ArrayList();
                System.Collections.ArrayList arrayWOChkboxes = new System.Collections.ArrayList();

                foreach (GridViewRow row in this.gridRequests.Rows)
                {
                    // Retrieve the reference to the checkbox 
                    CheckBox cb = (CheckBox)row.FindControl("chkSelect");

                    // Add script code to enable selection 
                    cb.Attributes.Add("onClick",
                      "toggleCheckBox(this,'gridRequests_chkSelectAll','" + this.hidCheckBoxIDs.ClientID + "');");

                    arrayChkboxes.Add(cb.ClientID);
                }

                foreach (GridViewRow row in this.gridWorkOrder.Rows)
                {
                    // Retrieve the reference to the checkbox 
                    CheckBox cbWO = (CheckBox)row.FindControl("chkSelectWO");

                    // Add script code to enable selection 
                    cbWO.Attributes.Add("onClick",
                      "toggleCheckBoxWO(this,'gridWorkOrder_chkSelectAllWO','" + this.hidWOChkBoxIDs.ClientID + "');");

                    arrayWOChkboxes.Add(cbWO.ClientID);
                }
                //set hidden field value with all checkbox client ids in grid

                if ((hidCheckBoxIDs != null))
                {
                    hidCheckBoxIDs.Value = String.Join(",", (string[])arrayChkboxes.ToArray(Type.GetType("System.String")));
                }
                if ((hidWOChkBoxIDs != null))
                {
                    hidWOChkBoxIDs.Value = String.Join(",", (string[])arrayWOChkboxes.ToArray(Type.GetType("System.String")));
                }

            }
            ViewState["CallPrerenderCode"] = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadStatusChkList();//Fill Request Status Check List.
                FillProjectListDropDown();//Fill Project Drop Down
                FillCategoryDropDown();//Fill Product Categry Drop Down
                FillProductTypeDropDown();//Fill Product Type Drop Down                
                FillModelDropDown();//Fill Product Model Drop Down.
                FillCompanyDropDown();//Fills Company Drop Down
                FillToVendorDropDown();//Fills the To Vendor Drop Down with list  of specific companies types.
                FillContactNameEmailDropDown();//Fills the To Contact Name & Email Drop Down with respect to selected company.               

                txtCompleteDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtWOComletionDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                dvMerge.Visible = true;
                dvWorkOrderSelectedRequests.Visible = false;
                ddlServiceRequestStatus.SelectedValue = "3";//By default selected to In progress.

                // Fill priority checklist dynamically
                LoadPriorityChkList();

                // Fill work order status checklist dynamically
                LoadWorkOrderStatusChkList();

                // Fill review state checklist
                LoadProjectReviewStateChklist();

                //To call the javascript function that hides the date inside update requests & update work orders modal popup
                ddlRequestStatus.Attributes.Add("onchange", "hideDate();");
                ddlWorkOrderStatus.Attributes.Add("onchange", "hideDate();");

                //fill any pre-selected filters
                //Project
                if (!String.IsNullOrEmpty(Request.QueryString["projectID"]))
                {
                    ddlProject.SelectedIndex =
                      ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(Request.QueryString["projectID"]));
                }

                //Unit Address
                if (!String.IsNullOrEmpty(Request.QueryString["unitID"]))
                {
                    WRObjectModel.Unit u = WRObjectModel.Unit.Get(Convert.ToInt32(Request.QueryString["unitID"]));
                    this.txtUnitNumber.Text = u.UnitDescription;
                    this.txtAddress.Text = u.UnitStreet;
                }

                //get StatusIds for 'under-review' and 'In progress'           
                List<int> defaultStatus = new List<int>();
                defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
                defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

                List<int> defaultWorkorderStatus = new List<int>();
                defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
                defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
                List<int> defaultPriority = new List<int>();
                defaultPriority.AddRange(
                    WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

                List<int> defaultReviewStates = new List<int> { 0 };
                //LoadGrid("SubmittedOn", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);

                var serviceRequestCount = LoadServiceRequestGrid("SubmittedOn", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
                LoadWorkOrdersGrid("SubmittedOn", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
                ServiceRequestCount = serviceRequestCount;
                litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Displaying the total number of service requests. 
            }
            else
            {
                //To show/hide date only on completed selected in update button.
                ScriptManager.RegisterStartupScript(this, typeof(Page), "HideDate", "hideDate();", true);
            }

            string shouldCheck = string.Empty;
            string checkBoxID = "gridRequests_chkSelectAll";
            string checkBoxWOID = "gridWorkOrder_chkSelectAllWO";
            if (!DesignMode)
            {
                object o = Page.Request[checkBoxID];
                if (o != null) shouldCheck = "checked='checked'";

                object p = Page.Request[checkBoxWOID];
                if (p != null) shouldCheck = "checked='checked'";
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            List<int> i = new List<int>();
            List<int> j = new List<int>();
            List<int> k = new List<int>();
            List<int> l = new List<int>();
            i.Add(0);
            j.Add(0);
            k.Add(0);
            l.Add(0);
            var serviceRequestCount = LoadServiceRequestGrid("SubmittedOn", SortDirection.Descending, i, j, k, l);
            var workOrderCount = LoadWorkOrdersGrid("SubmittedOn", SortDirection.Descending, i, j, k, l);
            this.divMultiSelectIDs.InnerHtml = "<div style='font-size: 0.74em;' class='light'>(none selected)</div>";

            if (Menu1.SelectedValue == "0")
            {
                ServiceRequestCount = serviceRequestCount;
                litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the Total Number of Service Requests.
            }
            else
            {
                WorkOrderCount = workOrderCount;
                litCount.Text = String.Format("Total # of records: {0}", WorkOrderCount);//Display the total number od Work Orders.
            }
        }

        protected void gridRequests_Sorting(object sender, GridViewSortEventArgs e)
        {
            //need to store sortDirection in ViewState 
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                e.SortDirection = SortDirection.Descending;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                e.SortDirection = SortDirection.Ascending;
            }

            List<int> i = new List<int>();
            i.Add(0);
            LoadServiceRequestGrid(e.SortExpression, e.SortDirection, i, i, i, i);
        }

        protected void gridWorkOrder_Sorting(object sender, GridViewSortEventArgs e)
        {
            //need to store sortDirection in ViewState 
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                e.SortDirection = SortDirection.Descending;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                e.SortDirection = SortDirection.Ascending;
            }

            List<int> i = new List<int>();
            i.Add(0);
            LoadWorkOrdersGrid(e.SortExpression, e.SortDirection, i, i, i, i);
        }

        protected void btnProceedMerge_Click(object sender, EventArgs e)
        {
            StringBuilder selectedKeys = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeys())
            {
                selectedKeys.Append(", " + key[0].ToString());
            }

            if (selectedKeys.Length > 0)
            {
                selectedKeys.Remove(0, 2);  //take out first commma
            }

            string[] theSplit = selectedKeys.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });

            bool success = false;

            //verify that all passed-in requestID's are from the same unit
            if (WRObjectModel.ServiceResource.IsSameUnit(arrayIDs))
            {
                //send all RequestIDs to method that will Merge them into one                              
                success = WRObjectModel.ServiceResource.MergeRequests(arrayIDs, this.txtComments.Text);
            }

            if (success == true)
            {
                this.divMultiSelectIDs.InnerHtml = "Merge Successful!";
            }
            else
            {
                this.divMultiSelectIDs.InnerHtml = "<div class='error'>Merge failed!</div>";
            }

            //get StatusIds for 'under-review' and 'In progress'           
            List<int> defaultStatus = new List<int>();
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

            List<int> defaultWorkorderStatus = new List<int>();
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
            List<int> defaultPriority = new List<int>();
            defaultPriority.AddRange(
                WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

            List<int> defaultReviewStates = new List<int> { 0 };
            var serviceRequestCount = LoadServiceRequestGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            this.txtComments.Text = "";
            ServiceRequestCount = serviceRequestCount;
            litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the total number of Service Requests.
        }

        //Supposedly, override needed to get get past event validation check on Excel export     
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        /// <summary>
        /// Downloads the Service Request Report according to the input selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOkExport_Click(object sender, EventArgs e)
        {

            //get all selected requestIds and send to ReportGenerator
            StringBuilder selectedKeys = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeys())
            {
                selectedKeys.Append(", " + key[0].ToString());
            }

            if (selectedKeys.Length > 0)
            {
                selectedKeys.Remove(0, 2);  //take out first commma
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Popup", "alert('This is alert Message from C#')", true);
            string[] theSplit = selectedKeys.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
            this.ModalPopupExtenderExportTo.Hide();
            if (btnExportRecords.Checked == true)
            {
                Response.Redirect(app.Reports.GenerateReport.GetServiceRequestReportUrl(selectedKeys.ToString()));
            }

            if (btnExportAllFields.Checked == true)
            {
                Session["exportAllRequests"] = arrayIDs;
                Response.Redirect("~/app/Reports/Export_ServiceRequests.aspx");
            }
        }

        /// <summary>
        /// Clears the Filters selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClearCriteria_Click(object sender, EventArgs e)
        {
            this.ddlProject.SelectedIndex = 0;
            this.txtAddress.Text = "";
            this.txtUnitNumber.Text = "";
            this.txtRequestID.Text = "";
            this.txtRequestor.Text = "";
            this.txtToDate.Text = "";
            this.txtFromDate.Text = "";
            this.ddlProductType.SelectedIndex = 0;
            this.ddlModel.Items.Clear();
            this.txtAssignedTo.Text = "";
            this.txtPOWOJobId.Text = "";
            this.ddlCategory.SelectedIndex = 0;
            this.txtToPossession.Text = "";
            this.txtFromPossession.Text = "";
            this.ddlCompany.SelectedIndex = 0;
            this.txtStrataLot.Text = string.Empty;

            this.chkStatus.ClearSelection();
            this.chkStatus.Items.FindByText("In-progress").Selected = true;
            this.chkStatus.Items.FindByText("Under-review").Selected = true;

            chkPriority.ClearSelection();
            foreach (ListItem item in chkPriority.Items)
            {
                item.Selected = true;
            }

            chkWorkOrder.ClearSelection();
            chkWorkOrder.Items.FindByText("In-progress").Selected = true;
            chkWorkOrder.Items.FindByText("Pending").Selected = true;

            chkReviewState.ClearSelection();
            foreach (ListItem item in chkReviewState.Items)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// Event fired when request type will change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void onProjectChange(object sender, EventArgs e)
        {
            var projectId = 0;
            int.TryParse(ddlProject.SelectedValue, out projectId);
            Dictionary<string, string> lstReviewStates;

            if (projectId != 0)
            {
                lstReviewStates = DataSources.ProjectReviewStatesByProjectId(projectId);

            }
            else
            {
                lstReviewStates = DataSources.GetProjectReviewStatesWithSameName(UserInfo.ByID);
            }

            var result = WRObjectModel.ServiceResource.GetUnitServiceRequestsWithNullReviewStateByProjectIdAndUserId(projectId, UserInfo.ByID);

            if (result == true)
            {
                var serviceType = WRObjectModel.ServiceResource.GetHomeownerServiceTypeName();
                lstReviewStates.Add(serviceType, "-1");
            }

            if (lstReviewStates.Count > 0)
            {
                chkReviewState.DataSource = lstReviewStates;
                chkReviewState.DataBind();
            }
            foreach (ListItem item in chkReviewState.Items)
            {
                item.Selected = true;
            }
            ddlCompany.Items.Clear();
            FillCompanyDropDown();

            ddlToVendor.Items.Clear();
            FillToVendorDropDown();
        }

        /// <summary>
        /// Updates the Service Request when Update button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOkUpdate_Click(object sender, EventArgs e)
        {
            StringBuilder selectedKeys = new StringBuilder();
            int requestPriority = 0;
            int requestStatus = 0;
            string serviceType = string.Empty;
            string completionDate = "";

            foreach (DataKey key in GetSelectedDataKeys())
            {
                selectedKeys.Append(", " + key[0].ToString());
            }

            if (selectedKeys.Length > 0)
            {
                selectedKeys.Remove(0, 2);  //take out first commma
            }

            string[] theSplit = selectedKeys.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
            bool success = false;

            if (!string.IsNullOrEmpty(ddlServiceType.SelectedValue))
            {
                if (ddlServiceType.SelectedValue == "-1")
                {
                    serviceType = null;
                }
                else
                {
                    serviceType = ddlServiceType.SelectedValue;
                }
            }

            if (!string.IsNullOrEmpty(ddlRequestPriority.SelectedValue) || Convert.ToInt32(ddlRequestPriority.SelectedValue) != 0)
            {
                requestPriority = Convert.ToInt32(ddlRequestPriority.SelectedValue);
            }

            if (!string.IsNullOrEmpty(ddlRequestStatus.SelectedValue) || Convert.ToInt32(ddlRequestStatus.SelectedValue) != 0)
            {
                requestStatus = Convert.ToInt32(ddlRequestStatus.SelectedValue);
            }

            if (!string.IsNullOrEmpty(this.txtCompleteDate.Text))
            {
                completionDate = this.txtCompleteDate.Text;
            }
            success = WRObjectModel.ServiceResource.UpdateRequest(arrayIDs, completionDate, serviceType, requestPriority, requestStatus);

            this.divMultiSelectIDs.InnerHtml = "Updating of Requests is Successful!";

            if (!success)
            {
                this.divMultiSelectIDs.InnerHtml = "<div class='error'>Update Failed!</div>";
            }

            //get StatusIds for 'under-review' and 'In progress'           
            List<int> defaultStatus = new List<int>();
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

            List<int> defaultWorkorderStatus = new List<int>();
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
            List<int> defaultPriority = new List<int>();
            defaultPriority.AddRange(
                WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

            List<int> defaultReviewStates = new List<int> { 0 };
            var serviceRequestCount = LoadServiceRequestGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            LoadWorkOrdersGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            ServiceRequestCount = serviceRequestCount;
            litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the total number of Service Request.
        }

        /// <summary>
        /// Deletes the Service Request when Delete button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOkDelete_Click(object sender, EventArgs e)
        {
            StringBuilder selectedKeys = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeys())
            {
                selectedKeys.Append(", " + key[0].ToString());
            }

            if (selectedKeys.Length > 0)
            {
                selectedKeys.Remove(0, 2);  //take out first commma
            }

            string[] theSplit = selectedKeys.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
            bool success = false;

            success = WRObjectModel.ServiceResource.DeleteServiceRequest(arrayIDs);
            this.divMultiSelectIDs.InnerHtml = "Deletion of Requests is Successful!";
            if (!success)
            {
                this.divMultiSelectIDs.InnerHtml = "<div class='error'>Deletion Failed!</div>";
            }


            //get StatusIds for 'under-review' and 'In progress'           
            List<int> defaultStatus = new List<int>();
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

            List<int> defaultWorkorderStatus = new List<int>();
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
            List<int> defaultPriority = new List<int>();
            defaultPriority.AddRange(
                WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

            List<int> defaultReviewStates = new List<int> { 0 };
            // LoadGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);

            var serviceRequestCount = LoadServiceRequestGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            LoadWorkOrdersGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            ServiceRequestCount = serviceRequestCount;
            litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the Total Number of Service Request
        }

        /// <summary>
        /// Changes the view when the selected tab is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {

            int index = Int32.Parse(e.Item.Value);
            mvSearchFilter_ActiveViewChange(index);
        }

        /// <summary>
        /// Updates the Work Order when Update button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOkWOUpdate_Click(object sender, EventArgs e)
        {
            StringBuilder selectedKeysWO = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeysWO())
            {
                selectedKeysWO.Append(", " + key[0].ToString());
            }

            if (selectedKeysWO.Length > 0)
            {
                selectedKeysWO.Remove(0, 2);  //take out first commma
            }

            string[] theSplit = selectedKeysWO.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
            bool success = false;
            string completionDate = "";
            int woStatus = 0;

            if (!string.IsNullOrEmpty(this.txtWOComletionDate.Text))
            {
                completionDate = this.txtWOComletionDate.Text;
            }

            if (!string.IsNullOrEmpty(ddlWorkOrderStatus.SelectedValue))
            {
                woStatus = Convert.ToInt32(ddlWorkOrderStatus.SelectedValue);
            }
            success = WRObjectModel.ServiceResource.UpdateWorkOrders(arrayIDs, woStatus, completionDate);

            this.divMultiSelectWOIds.InnerHtml = "Updating of Work Orders is Successful!";

            if (!success)
            {
                this.divMultiSelectWOIds.InnerHtml = "<div class='error'>Updation Failed!</div>";
            }
            //get StatusIds for 'under-review' and 'In progress'           
            List<int> defaultStatus = new List<int>();
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

            List<int> defaultWorkorderStatus = new List<int>();
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
            List<int> defaultPriority = new List<int>();
            defaultPriority.AddRange(
                WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

            List<int> defaultReviewStates = new List<int> { 0 };
            var workOrderCount = LoadWorkOrdersGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            WorkOrderCount = workOrderCount;
            litCount.Text = String.Format("Total # of records: {0}", WorkOrderCount);//Display the Total Number of Work Orders
        }

        /// <summary>
        /// Deletes the Work Oder when Delete button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOkDeleteWO_Click(object sender, EventArgs e)
        {
            StringBuilder selectedKeysWO = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeysWO())
            {
                selectedKeysWO.Append(", " + key[0].ToString());
            }

            if (selectedKeysWO.Length > 0)
            {
                selectedKeysWO.Remove(0, 2);  //take out first commma
            }

            string[] theSplit = selectedKeysWO.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
            bool success = false;

            success = WRObjectModel.ServiceResource.DeleteWorkOrders(arrayIDs);

            this.divMultiSelectWOIds.InnerHtml = "Deletion of Work Orders is Successful!";

            if (!success)
            {
                this.divMultiSelectWOIds.InnerHtml = "<div class='error'>Deletion Failed!</div>";
            }
            //get StatusIds for 'under-review' and 'In progress'           
            List<int> defaultStatus = new List<int>();
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
            defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

            List<int> defaultWorkorderStatus = new List<int>();
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
            defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
            List<int> defaultPriority = new List<int>();
            defaultPriority.AddRange(
                WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

            List<int> defaultReviewStates = new List<int> { 0 };
            LoadServiceRequestGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            var workOrderCount = LoadWorkOrdersGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
            WorkOrderCount = workOrderCount;
            litCount.Text = String.Format("Total # of records: {0}", WorkOrderCount);//Display the Total Number of Work Orders
        }

        /// <summary>
        /// Downloads the Work Order Report according to the input Selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOKExportToWo_Click(object sender, EventArgs e)
        {
            //get all selected requestIds and send to ReportGenerator            
            StringBuilder selectedKeysWO = new StringBuilder();

            foreach (DataKey key in GetSelectedDataKeysWO())
            {
                selectedKeysWO.Append(", " + key[0].ToString());
            }

            if (selectedKeysWO.Length > 0)
            {
                selectedKeysWO.Remove(0, 2);  //take out first commma
            }

            if (btnExportRecordsWO.Checked == true)
            {
                Response.Redirect(app.Reports.GenerateReport.GetWorkOrderReportUrl(selectedKeysWO.ToString()));
            }

            string[] theSplit = selectedKeysWO.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });

            if (btnExportAllFieldsWO.Checked == true)
            {
                Session["exportAllWorkOrders"] = arrayIDs;

                Response.Redirect("~/app/Reports/Export_WorkOrders.aspx");
            }
        }

        /// <summary>
        /// Creates the Work Order for selected Service Requests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOkCreateWO_Click(object sender, EventArgs e)
        {

            //var count = chkRequestServiceFiles.Items.Count;
            string poJobId = string.Empty, toVendor = string.Empty, vendorContactEmail = string.Empty, instructionToVendor = string.Empty, ownerEmailText = string.Empty;
            int requestStatus = 0;
            //bool attachExistingFiles = false;
            DateTime appointmentDateTime = DateTime.MinValue;
            //get all selected requestIds and send to ReportGenerator
            StringBuilder selectedKeys = new StringBuilder();
            foreach (DataKey key in GetSelectedDataKeys())
            {
                selectedKeys.Append(", " + key[0].ToString());
            }

            if (selectedKeys.Length > 0)
            {
                selectedKeys.Remove(0, 2);  //take out first commma
            }

            string[] theSplit = selectedKeys.ToString().Split(',');
            int[] arrayIDs = Array.ConvertAll<string, int>(theSplit, delegate(string s) { return int.Parse(s); });
            bool success = false;

            if (this.txtInstructionToVendor.Text.Length >= 3)
            {
                if (!string.IsNullOrEmpty(this.txtPOJobIdCreateWO.Text))
                {
                    poJobId = this.txtPOJobIdCreateWO.Text;
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
                if (contactNames != null)
                {
                    List<string> contacts = contactNames.Split(',').ToList<string>();
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        var split = contacts[i].Split('(');
                        lstEmailIds.Append(String.Format(", {0}", split[1].TrimEnd(')')));
                        lstContactNames.Append(String.Format(", {0}", split[0]));
                    }
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

                HttpFileCollection fileCollection = Request.Files;
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
                                //lblerror.Text = message;
                                //lblerror.Visible = true;
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
                                //lblerror.Text = @"File with names beyond 64 characters are not supported. Please rename and try again";
                                //lblerror.Visible = true;
                                return;
                            }
                        }
                    }
                }

                // attach existing files
                var requestDocIds = hdnFiles.Value;
                if (!string.IsNullOrWhiteSpace(requestDocIds))
                {
                    requestDocIds = requestDocIds.Trim().Remove(requestDocIds.LastIndexOf(","));
                }

                var requestDocuments = requestDocIds.Split(',').ToList();
                List<int> requestDocumentIds = new List<int>();
                foreach (var requestDoc in requestDocuments)
                {
                    int docId = 0;
                    int.TryParse(requestDoc, out docId);
                    requestDocumentIds.Add(docId);
                }

                success = WRObjectModel.UnitServiceRequestWorkOrder.InsertWorkOrder(arrayIDs, instructionToVendor, vendorContactEmail, ownerEmailText, poJobId, requestStatus, addtionalEmailAddresses, companyId, lstContactIds, lstEmailIds.ToString(), fileCollection, requestDocumentIds, lstContactNames.ToString());

                this.divMultiSelectIDs.InnerHtml = "Work Orders are Created Successfully!";

                if (!success)
                {
                    this.divMultiSelectIDs.InnerHtml = "<div class='error'>Work Order Creation Failed!</div>";
                }

                //get StatusIds for 'under-review' and 'In progress'           
                List<int> defaultStatus = new List<int>();
                defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
                defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

                List<int> defaultWorkorderStatus = new List<int>();
                //defaultWirkorderStatus.AddRange(WRObjectModel.ServiceRequestWorkOrderStatus.GetLookupList().Select(m => m.ServiceRequestWorkOrderStatusID));
                defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
                defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
                List<int> defaultPriority = new List<int>();
                defaultPriority.AddRange(
                    WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

                List<int> defaultReviewStates = new List<int> { 0 };
                var serviceRequestCount = LoadServiceRequestGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
                LoadWorkOrdersGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
                ServiceRequestCount = serviceRequestCount;
                litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the Total Number of Service Request
                ScriptManager.RegisterStartupScript(this, typeof(Page), "clearInputs", "clearInputs();", true);
            }
            else
            {
                //get StatusIds for 'under-review' and 'In progress'           
                List<int> defaultStatus = new List<int>();
                defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
                defaultStatus.Add(WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID);

                List<int> defaultWorkorderStatus = new List<int>();
                //defaultWirkorderStatus.AddRange(WRObjectModel.ServiceRequestWorkOrderStatus.GetLookupList().Select(m => m.ServiceRequestWorkOrderStatusID));
                defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("pending").ServiceRequestWorkOrderStatusID);
                defaultWorkorderStatus.Add(WRObjectModel.ServiceRequestWorkOrderStatus.Get("In-progress").ServiceRequestWorkOrderStatusID);
                List<int> defaultPriority = new List<int>();
                defaultPriority.AddRange(
                    WRObjectModel.ServiceRequestPriority.GetLookupList().Select(m => m.ServiceRequestPriorityID));

                List<int> defaultReviewStates = new List<int> { 0 };
                var serviceRequestCount = LoadServiceRequestGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
                LoadWorkOrdersGrid("UnitServiceRequestID", SortDirection.Descending, defaultStatus, defaultWorkorderStatus, defaultPriority, defaultReviewStates);
                ServiceRequestCount = serviceRequestCount;
                litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the Total Number of Service Request
                ScriptManager.RegisterStartupScript(this, typeof(Page), "clearInputs", "clearInputs();", true);
            }
        }

        /// <summary>
        /// Method called when Product Category selected index changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CallPrerenderCode"] = false;
            ddlModel.Items.Clear();
            ddlProductType.Items.Clear();

            if (!(ddlCategory.SelectedValue.Equals("-1")))
            {
                FillProductTypeDropDown();
                FillModelDropDown();
            }
        }

        /// <summary>
        /// Method called when Product Type selected index changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CallPrerenderCode"] = false;
            ddlModel.Items.Clear();
            if (!(ddlProductType.Text.Equals(" (ALL) ")))
            {
                FillModelDropDown();
            }
        }

        /// <summary>
        /// Method called when the page index is changes in the requests grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridRequests.PageIndex = e.NewPageIndex;

            List<int> i = new List<int>();
            List<int> j = new List<int>();
            List<int> k = new List<int>();
            List<int> l = new List<int>();
            i.Add(0);
            j.Add(0);
            k.Add(0);
            l.Add(0);
            LoadServiceRequestGrid("SubmittedOn", SortDirection.Descending, i, j, k, l);
        }

        /// <summary>
        /// Method called when the page index is changes in the work order grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridWorkOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridWorkOrder.PageIndex = e.NewPageIndex;

            List<int> i = new List<int>();
            List<int> j = new List<int>();
            List<int> k = new List<int>();
            List<int> l = new List<int>();
            i.Add(0);
            j.Add(0);
            k.Add(0);
            l.Add(0);
            LoadWorkOrdersGrid("SubmittedOn", SortDirection.Descending, i, j, k, l);
        }

        protected void ddlViewAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<int> i = new List<int>();
            List<int> j = new List<int>();
            List<int> k = new List<int>();
            List<int> l = new List<int>();
            i.Add(0);
            j.Add(0);
            k.Add(0);
            l.Add(0);

            int pageCount;
            int.TryParse(ddlViewAll.SelectedValue, out pageCount);

            if (Menu1.SelectedValue == "0")
            {
                if (pageCount != 0)
                {
                    gridRequests.AllowPaging = true;
                    gridRequests.PageSize = pageCount;
                }
                else
                {
                    gridRequests.AllowPaging = false;
                }
                var serviceRequestCount = LoadServiceRequestGrid("SubmittedOn", SortDirection.Descending, i, j, k, l);
                gridRequests.DataBind();
                ServiceRequestCount = serviceRequestCount;
                litCount.Text = String.Format("Total # of records: {0}", ServiceRequestCount);//Display the total number of Service Request.
            }
            else
            {
                if (pageCount != 0)
                {
                    gridWorkOrder.AllowPaging = true;
                    gridWorkOrder.PageSize = pageCount;
                }
                else
                {
                    gridWorkOrder.AllowPaging = false;
                }
                var workOrderCount = LoadWorkOrdersGrid("SubmittedOn", SortDirection.Descending, i, j, k, l);
                gridWorkOrder.DataBind();
                WorkOrderCount = workOrderCount;
                litCount.Text = String.Format("Total # of records: {0}", WorkOrderCount);//Display the total number of Work Orders.
            }
        }

        protected void ddlToVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkNameEmail.Items.Clear();
            FillContactNameEmailDropDown();
            ModalPopupExtenderCreateWO.Show();
        }

        /// <summary>
        /// Service Requests Grid Row Data Bound.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.RowIndex >= 0)
                {
                    var usr = (Conasys.HIP.BusinessManager.Model.ServiceRequestModel)e.Row.DataItem;
                    var serviceRequest = WRObjectModel.UnitServiceRequest.Get(usr.UnitServiceRequestID);

                    //Days Open
                    Literal litDaysOpen = (Literal)e.Row.FindControl("litDaysOpen");
                    if (litDaysOpen != null)
                    {
                        TimeSpan daysOpen = new TimeSpan();
                        if (serviceRequest.ServiceRequestStatusID == 1 || serviceRequest.ServiceRequestStatusID == 2 || serviceRequest.ServiceRequestStatusID == 3 || serviceRequest.ServiceRequestStatusID == 7)//1 for pending,2 for Under-Review,3 for In-Progress and 7 for Courtesy
                        {
                            daysOpen = DateTime.Now.Date - serviceRequest.CreatedOn.Date;
                        }
                        else
                        {
                            if (serviceRequest.ClosedOn.HasValue)
                            {
                                daysOpen = serviceRequest.ClosedOn.Value.Date - serviceRequest.CreatedOn.Date;
                            }
                        }
                        litDaysOpen.Text = daysOpen.Days.ToString();
                    }
                }
            }
        }
        #endregion

        #region Public Methods
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
