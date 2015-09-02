using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeOwner.app
{
    public partial class Builder_ServiceRequestRelated : HIP_BuilderServiceBasePage
    {
        public static string GetUrl(int requestId)
        {
            return "~/app/builder/Builder_ServiceRequestRelated.aspx?rid=" + requestId;
        }

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
                builderMaster.SetMasterPageValues(usr, "related", userId);
                LoadForm();

                CheckForLockedStatus(usr);

            }
        }


        protected void LoadForm()
        {
            if (requestID != 0)
            {

                LoadGrid();
            }
        }


        void LoadGrid()
        {
            //get the Related Requests for the current Request. Using a LinqDataSource.
            this.gridRelated.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Create an event handler for the master page event when User updates Request details
            Master.ServiceRequestUpdated += new EventHandler(Master_ServiceRequestUpdated);
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

                this.btnSave.Enabled = false;
                this.txtComment.Enabled = false;
                this.txtRequestID.Enabled = false;

                this.lblDisabledMsg.Text = "Note: Requests with the status of " + usr.ServiceRequestStatus.Name + " can not be modified. ";
                this.lblDisabledMsg.Visible = true;
            }

        }



        protected void btnSave_Click(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(this.txtRequestID.Text);
            bool success = false;

            //Validate! Check for legit requestid and same unit and duplicates          
            WRObjectModel.UnitServiceRequest usr = WRObjectModel.UnitServiceRequest.Get(id);
            if (usr == null)
            {

                this.litMsg.Text = "Invalid Request ID";
                success = false;
            }
            else
            {

                int[] ary = { requestID, Convert.ToInt32(id) };
                if (WRObjectModel.ServiceResource.IsSameUnit(ary))
                {
                    success = true;
                }
                else
                {
                    this.litMsg.Text = "That Request ID is assigned to a different home.";
                    success = false;
                }

                if (success)
                {
                    if (WRObjectModel.UnitServiceRequestRelatedRequest.IsDuplicate(this.requestID, id))
                    {
                        success = false;
                        this.litMsg.Text = "The specified Request is already related.";
                    }
                    else
                    {
                        success = true;
                    }
                }


                if (success)
                {
                    success = WRObjectModel.ServiceResource.RelateRequest(this.requestID, id, this.txtComment.Text);

                    if (success == true)
                    {
                        this.litMsg.Text = "<div style='color: green;'>Relation successful!</div>";
                        this.txtComment.Text = "";
                        this.txtRequestID.Text = "";
                    }
                    else
                    {
                        this.litMsg.Text = "<div>Failed! Was not able to relate the Request.</div>";
                    }

                    LoadGrid();
                }

            }
            var request = WRObjectModel.UnitServiceRequest.Get(this.requestID);
            //Calling the Master Page function to set the default values in the dropdowns and text boxes.
            Master.SetMasterPageValues(request, "related", UserInfo.ByID);
        }


        //this event is fired from the MasterPage when the user updates any Request data (like Status or Manager)
        private void Master_ServiceRequestUpdated(object sender, EventArgs e)
        {
            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(requestID);

            //refresh the page
            this.gridRelated.DataBind();
            //this.UpdatePanel1.Update();
            CheckForLockedStatus(usr);

        }


        protected void gridRelated_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                WRObjectModel.UnitServiceRequestRelatedRequest r = (WRObjectModel.UnitServiceRequestRelatedRequest)e.Row.DataItem;

                //set Details image link           
                HyperLink hl = ((HyperLink)e.Row.FindControl("lnkEdit"));
                hl.NavigateUrl = app.Builder_ServiceRequestMain.GetUrl(r.ChildRequestID);

            }
        }


    }
}
