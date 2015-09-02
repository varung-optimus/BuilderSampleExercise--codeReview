using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRObjectModel;
using HomeOwner.app.Builder;

namespace HomeOwner.app
{
    public partial class Builder_ServiceRequestMain : HIP_BuilderServiceBasePage
    {
        #region Properties
        private static int? _requestID;
        public int requestID
        {
            set
            {
                _requestID = value;
            }
            get
            {
                if (_requestID == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(_requestID);
                }
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

        #region Public Method
        public static string GetUrl(int requestId)
        {
            return "/app/builder/Builder_ServiceRequestMain.aspx?rid=" + requestId;
        }
        #endregion

        #region Private Methods
        private void FillOwnerDetails(UnitServiceRequest usr)
        {
            var usrName = String.Format("{0} {1}", usr.ContactFirstName, usr.ContactLastName);
            litUsrName.Text = usrName;

            var usrRemark = usr.UnitServiceRequestRemarks;
            string remark = usrRemark.FirstOrDefault().Remarks;

            if (!string.IsNullOrWhiteSpace(usr.InitialOwnerComment))
            {
                litOwnerComment.Text = usr.InitialOwnerComment;
            }
            else
            {
                litOwnerComment.Text = remark;
            }
            litDate.Text = String.Format("{0:MMMM d, yyyy H:mm}", usrRemark.FirstOrDefault().CreatedOn);

            if (!string.IsNullOrWhiteSpace(usr.InitialLocation))
            {
                if (!usr.InitialLocation.ToLower().Equals("select location") && !usr.InitialLocation.ToLower().Equals("select a location first"))
                {
                    litLocation.Text = String.Format("<b>Location: </b>{0}", usr.InitialLocation);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(usr.Location))
                {
                    if (!usr.Location.ToLower().Equals("select location") && !usr.Location.ToLower().Equals("select a location first"))
                    {
                        litLocation.Text = String.Format("<b>Location: </b>{0}", usr.Location);
                    }
                }
            }

            // Product details
            if (usr.InitialProductId != -99)
            {
                if (usr.InitialProductId != 0 && usr.InitialProductId != -1)
                {
                    Product product = Product.GetProductById(usr.InitialProductId);

                    if (product != null)
                    {
                        litProduct.Text = String.Format("<b>Product: </b>{0} - {1} - {2}", product.ProductCategory.Name, product.ProductType.Name, product.Model);
                    }

                    else
                    {
                        Component c = Component.Get(usr.InitialProductId);
                        litProduct.Text = String.Format("<b>Product: </b>{0} - {1}", c.ComponentType.ParentComponentType.Name,
                                                        c.ComponentType.Name);
                    }
                }
                else if (usr.InitialProductId == -1)
                {
                    litProduct.Text = @"<b>Product: </b>Miscellaneous";
                }
            }
            else
            {
                if (usr.ProductId != 0 && usr.ProductId != -1)
                {
                    Product product = Product.GetProductById(usr.ProductId);

                    if (product != null)
                    {
                        litProduct.Text = String.Format("<b>Product: </b>{0} - {1} - {2}", product.ProductCategory.Name, product.ProductType.Name, product.Model);
                    }

                    else
                    {
                        Component c = Component.Get(usr.ProductId);
                        litProduct.Text = String.Format("<b>Product: </b>{0} - {1}", c.ComponentType.ParentComponentType.Name,
                                                        c.ComponentType.Name);
                    }
                }
                else if (usr.ProductId == -1)
                {
                    litProduct.Text = @"<b>Product: </b>Miscellaneous";
                }
            }
        }

        private string RemoveBadText(string s)
        {
            s = s.Replace("'", "");
            //s = s.Replace("\"", "");
            s = s.Replace(";", " ");
            s = s.Replace("%", "");
            s = s.Replace("*", "");
            s = s.Replace("delete", "");
            //s = s.Replace("""", "");
            s = s.Replace("<", "(");
            s = s.Replace(">", ")");
            return s;
        }

        //this event is fired from the MasterPage when the user updates any Request data (like Status or Manager)
        private void Master_ServiceRequestUpdated(object sender, EventArgs e)
        {

            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
            //HomeOwner.master.HIP_BuilderService builderMaster = (HomeOwner.master.HIP_BuilderService)Page.Master;
            //builderMaster.SetMasterPageValues(usr, "main");

            //refresh data
            this.repeatRemarks.DataBind();
            //this.repeatActionItems.DataBind();
            //this.UpdatePanel1.Update();

            if (CheckForLockedStatus(usr) == true)
            {
                this.btnAdd.Enabled = false;
                //this.btnNewActionItem.Enabled = false;
                // this.btnNotifyOwner.Enabled = false;
                this.txtBuilderComment.Enabled = false;

                this.lblDisabledMsg.Text = "Note: Requests with the status of " + usr.ServiceRequestStatus.Name + " can not be modified. ";
                this.lblDisabledMsg.Visible = true;

            }
        }
        
        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["rid"] != null)
                {
                    requestID = Convert.ToInt32(Request.QueryString["rid"]);
                    unitID = WRObjectModel.UnitServiceRequest.Get(requestID).UnitID;
                }

                WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
                HomeOwner.master.HIP_BuilderService builderMaster = (HomeOwner.master.HIP_BuilderService)Page.Master;
                int userId = UserInfo.ByID;
                builderMaster.SetMasterPageValues(usr, "main", userId);
                LoadForm();

                if (CheckForLockedStatus(usr) == true)
                {
                    this.btnAdd.Enabled = false;
                    //this.btnNewActionItem.Enabled = false;
                    //this.btnNotifyOwner.Enabled = false;
                    this.txtBuilderComment.Enabled = false;


                    this.lblDisabledMsg.Text = "Note: Requests with the status of " + usr.ServiceRequestStatus.Name + " can not be modified. ";
                    this.lblDisabledMsg.Visible = true;

                }
                FillOwnerDetails(usr);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Create an event handler for the master page event when User updates Request details
            Master.ServiceRequestUpdated += new EventHandler(Master_ServiceRequestUpdated);
        }

        protected void LoadForm()
        {
            if (requestID != 0)
            {

                //LoadRepeaters();

                //LoadComments();
                //LoadActionItemGrid();                
                //this.repeatActionItems_Incomplete.DataBind();
            }
        }

        protected bool CheckForLockedStatus(WRObjectModel.UnitServiceRequest usr)
        {
            bool locked = false;
            //changes are NOT allowed for Merged, Denied or Complete Requests...disable controls!
            WRObjectModel.ServiceRequestStatus complete = WRObjectModel.ServiceRequestStatus.Get("complete");
            WRObjectModel.ServiceRequestStatus merged = WRObjectModel.ServiceRequestStatus.Get("merged");
            WRObjectModel.ServiceRequestStatus denied = WRObjectModel.ServiceRequestStatus.Get("non-warrantable");
            if (usr.ServiceRequestStatusID == complete.ServiceRequestStatusID ||
                usr.ServiceRequestStatusID == merged.ServiceRequestStatusID ||
                usr.ServiceRequestStatusID == denied.ServiceRequestStatusID)
            {

                locked = true;
            }

            return locked;
        }

        protected void repeatRemarks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                WRObjectModel.UnitServiceRequestRemark r = (WRObjectModel.UnitServiceRequestRemark)e.Item.DataItem;
                System.Web.UI.HtmlControls.HtmlContainerControl tr =
                    (System.Web.UI.HtmlControls.HtmlContainerControl)e.Item.FindControl("divRepeaterRow");

                if (r.ServiceRequestRemarkType.ServiceRequestRemarkTypeID == WRObjectModel.ServiceRequestRemarkType.Get("owner").ServiceRequestRemarkTypeID)
                {
                    tr.Attributes.Add("class", "box2");
                }
                else if (r.ServiceRequestRemarkType.ServiceRequestRemarkTypeID == WRObjectModel.ServiceRequestRemarkType.Get("builder").ServiceRequestRemarkTypeID)
                {
                    tr.Attributes.Add("class", "box3");
                }
                else if (r.ServiceRequestRemarkType.ServiceRequestRemarkTypeID == WRObjectModel.ServiceRequestRemarkType.Get("status").ServiceRequestRemarkTypeID)
                {
                    tr.Attributes.Add("class", "box");
                }

                Literal builderName = (Literal)e.Item.FindControl("litBuilderName");
                if (builderName != null)
                {
                    if (r.CreatedBy != -1)
                    {
                        builderName.Text = WRObjectModel.BuilderPortalAccount.GetByUserID(r.CreatedBy).UserName;
                    }
                    else
                    {
                        builderName.Text = CurrentUnit.Phase.Project.BuilderPortalAccountProjects.FirstOrDefault().BuilderPortalAccount.UserName;
                    }
                }

                Literal remarks = (Literal)e.Item.FindControl("litRemarks");
                if (remarks != null)
                {
                    remarks.Text = r.Remarks;
                }

                if (r.ServiceRequestRemarkTypeID != 2)
                {
                    LinkButton lbDeleteRemark = (LinkButton)e.Item.FindControl("lbDeleteRemark");
                    if (lbDeleteRemark != null)
                    {
                        lbDeleteRemark.Text = "X";
                    }

                }

            }
        }

        protected void btnNewRemark_Click(object sender, EventArgs e)
        {
            //save builder Remark into this Request
            if (this.txtBuilderComment.Text.Length > 2)
            {
                string comment = this.txtBuilderComment.Text.Trim();
                var requestId = Convert.ToInt32(Request.QueryString["rid"]);
                WRObjectModel.ServiceResource.InsertRemark(requestId, RemoveBadText(comment), "builder");
                //WRObjectModel.ServiceResource.InsertRemark(requestID, HttpUtility.HtmlEncode(comment), "builder");
                this.txtBuilderComment.Text = "";

                //LoadRepeaters();
                this.repeatRemarks.DataBind();
            }
        }

        //handles both Repeater controls ItemDataBound event
        protected void repeatActionItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                WRObjectModel.UnitServiceRequestActionItem ai = (WRObjectModel.UnitServiceRequestActionItem)e.Item.DataItem;

                System.Web.UI.HtmlControls.HtmlContainerControl tr =
                    (System.Web.UI.HtmlControls.HtmlContainerControl)e.Item.FindControl("divRepeaterRow");

                if (CheckForLockedStatus(ai.UnitServiceRequest) == true)
                {
                    //delete btn
                    ImageButton del = e.Item.FindControl("btnDeleteItem") as ImageButton;
                    del.Visible = false;

                    //toggle link
                    LinkButton tog = e.Item.FindControl("btnCompleteToggle") as LinkButton;
                    tog.Visible = false;
                }


                Label l = (Label)e.Item.FindControl("lblComplete");
                if (ai.ActionItemComplete)
                {
                    l.ForeColor = System.Drawing.Color.Green;
                    l.Text = "Yes";
                    //tr.Attributes.Add("class", "box2 pad");
                }
                else
                {
                    l.ForeColor = System.Drawing.Color.Red;
                    l.Text = "No";
                    tr.Attributes.Add("class", "box pad");
                }


            }
        }

        protected void Complete_Toggle(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            Int32 itemId = Int32.Parse(chk.InputAttributes["actionID"].ToString());
            WRObjectModel.UnitServiceRequestActionItem.ToggleActionComplete(itemId);

            //LoadRepeaters();
        }

        //Send an Email to the Owner with Request Details
        protected void btnNotifyOwner_Click(object sender, EventArgs e)
        {
            //publish the request to the home service record. Email is handled by the request handler.
            WRObjectModel.ServiceResource.UpdateIsPublished(requestID, true);
        }

        protected void repeatActionItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "delete")
            {

                WRObjectModel.UnitServiceRequestActionItem.Delete(id);
            }

            if (e.CommandName == "complete")
            {
                //change complete flag
                WRObjectModel.UnitServiceRequestActionItem.ToggleActionComplete(id);
                //LoadRepeaters();
            }


            //LoadRepeaters();

        }

        protected void repeatRemarks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "deleteRemark")
            {

                WRObjectModel.UnitServiceRequestRemark.Delete(id);
            }

            this.repeatRemarks.DataBind();

        }

        protected void btnAddNewCommenttoRequestor_Click(object sender, EventArgs e)
        {
            /*To write code for saving the comment
             *  and sending the Email to Homeowner
             *  and to Display it in records page as it is shown earlier.
             */
            if (!string.IsNullOrWhiteSpace(txtNewCommenttoRequestor.Text))
            {
                //WRObjectModel.UnitServiceRequestRemark.InsertCommentToRequestor(CurrentRequest.UnitServiceRequestID, txtNewCommenttoRequestor.Text, UserInfo.ByID);
                var requestId = Convert.ToInt32(Request.QueryString["rid"]);
                string comment = RemoveBadText(txtNewCommenttoRequestor.Text);
                WRObjectModel.UnitServiceRequestActionItem.InsertActionItem(requestId, comment, false);
                WRObjectModel.ServiceResource.SendServiceRequestNewCommentToRequestorEmailToHomeOwner(requestId, comment);
            }

            this.rptCommentToRequestor.DataBind();
            txtNewCommenttoRequestor.Text = "";
        }

        protected void rptCommentToRequestor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var commentToRequestor = (UnitServiceRequestActionItem)e.Item.DataItem;

            Literal litCommentToRequestorBuilderName = (Literal)e.Item.FindControl("litCommentToRequestorBuilderName");
            if (litCommentToRequestorBuilderName != null)
            {
                if (commentToRequestor.CreatedBy != -1)
                {
                    litCommentToRequestorBuilderName.Text = WRObjectModel.BuilderPortalAccount.GetByUserID(commentToRequestor.CreatedBy).UserName;
                }
                else
                {
                    litCommentToRequestorBuilderName.Text = CurrentUnit.Phase.Project.BuilderPortalAccountProjects.FirstOrDefault().BuilderPortalAccount.UserName;
                }
            }

            Literal litCommentToRequestor = (Literal)e.Item.FindControl("litCommentToRequestor");
            if (litCommentToRequestor != null)
            {
                litCommentToRequestor.Text = commentToRequestor.ActionItemDescription;
            }
        }

        /// <summary>
        /// Method to Load the Homeowner Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptRelatedRequests_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UnitServiceRequestRelatedRequest item = (UnitServiceRequestRelatedRequest)e.Item.DataItem;

                WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(item.ChildRequestID);

                var usrName = String.Format("{0} {1}", usr.ContactFirstName, usr.ContactLastName);
                var litUsrName = (Literal)e.Item.FindControl("litUsrName");

                if (litUsrName != null)
                {
                    litUsrName.Text = usrName;
                }

                var usrRemark = usr.UnitServiceRequestRemarks;
                string remark = usrRemark.FirstOrDefault().Remarks;

                var litOwnerComment = (Literal)e.Item.FindControl("litOwnerComment");
                var litLocation = (Literal)e.Item.FindControl("litLocation");
                var litProduct = (Literal)e.Item.FindControl("litProduct");

                string ownerComment = string.Empty;
                string location = string.Empty;
                string productDescription = string.Empty;

                if (!string.IsNullOrWhiteSpace(usr.InitialOwnerComment))
                {
                    litOwnerComment.Text = usr.InitialOwnerComment;
                    ownerComment = usr.InitialOwnerComment;
                }
                else
                {
                    litOwnerComment.Text = remark;
                    ownerComment = remark;
                }

                if (litOwnerComment != null)
                {
                    litOwnerComment.Text = ownerComment;
                }

                var litDate = (Literal)e.Item.FindControl("litDate");
                if (litDate != null)
                {
                    litDate.Text = String.Format("{0:MMMM d, yyyy H:mm}", usrRemark.FirstOrDefault().CreatedOn);
                }

                if (!string.IsNullOrWhiteSpace(usr.InitialLocation))
                {
                    if (!usr.InitialLocation.ToLower().Equals("select location") && !usr.InitialLocation.ToLower().Equals("select a location first"))
                    {
                        location = String.Format("<b>Location: </b>{0}", usr.InitialLocation);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(usr.Location))
                    {
                        if (!usr.Location.ToLower().Equals("select location") && !usr.Location.ToLower().Equals("select a location first"))
                        {
                            location = String.Format("<b>Location: </b>{0}", usr.Location);
                        }
                    }
                }

                if (litLocation != null)
                {
                    litLocation.Text = location;
                }

                // Product details
                if (usr.InitialProductId != -99)
                {
                    if (usr.InitialProductId != 0 && usr.InitialProductId != -1)
                    {
                        Product product = Product.GetProductById(usr.InitialProductId);

                        if (product != null)
                        {
                            productDescription = String.Format("<b>Product: </b>{0} - {1} - {2}", product.ProductCategory.Name, product.ProductType.Name, product.Model);
                        }

                        else
                        {
                            Component c = Component.Get(usr.InitialProductId);
                            productDescription = String.Format("<b>Product: </b>{0} - {1}", c.ComponentType.ParentComponentType.Name,
                                                            c.ComponentType.Name);
                        }
                    }
                    else if (usr.InitialProductId == -1)
                    {
                        productDescription = @"<b>Product: </b>Miscellaneous";
                    }
                }
                else
                {
                    if (usr.ProductId != 0 && usr.ProductId != -1)
                    {
                        Product product = Product.GetProductById(usr.ProductId);

                        if (product != null)
                        {
                            productDescription = String.Format("<b>Product: </b>{0} - {1} - {2}", product.ProductCategory.Name, product.ProductType.Name, product.Model);
                        }

                        else
                        {
                            Component c = Component.Get(usr.ProductId);
                            productDescription = String.Format("<b>Product: </b>{0} - {1}", c.ComponentType.ParentComponentType.Name,
                                                            c.ComponentType.Name);
                        }
                    }
                    else if (usr.ProductId == -1)
                    {
                        productDescription = @"<b>Product: </b>Miscellaneous";
                    }
                }
                if (litProduct != null)
                {
                    litProduct.Text = productDescription;
                }
            }
        } 
        #endregion
    }
}
