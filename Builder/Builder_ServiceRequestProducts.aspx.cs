using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeOwner.app {
    public partial class Builder_ServiceRequestProducts : HIP_BuilderServiceBasePage 
    {
      public static string GetUrl(int requestId)
      {
        return "~/app/builder/Builder_ServiceRequestProducts.aspx?rid=" + requestId;
      }

        private static int? _requestID;
        public int requestID {
            set {
                _requestID = value;
            }
            get {
                if (_requestID == null) {
                    return 0;
                }
                else {
                    return Convert.ToInt32(_requestID);
                }
            }
        }

        private static int? _unitID;
        public int unitID {
            set {
                _unitID = value;
            }
            get {
                if (_unitID == null) {
                    return 0;
                }
                else {
                    return Convert.ToInt32(_unitID);
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                              
                if (Request.QueryString["rid"] != null) {
                    requestID = Convert.ToInt32(Request.QueryString["rid"]);
                    unitID = WRObjectModel.UnitServiceRequest.Get(requestID).UnitID;
                }

                CheckForLockedStatus();             
                LoadForm();

            }
        }


        protected void CheckForLockedStatus()
        {

            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
            HomeOwner.master.HIP_BuilderService builderMaster = (HomeOwner.master.HIP_BuilderService)Page.Master;
            int userId = UserInfo.ByID;
            builderMaster.SetMasterPageValues(usr, "products", userId);

            //changes are NOT allowed for Merged, Denied or Complete Requests...will disable 'add' btn in ItemCreated event.
            WRObjectModel.ServiceRequestStatus complete = WRObjectModel.ServiceRequestStatus.Get("complete");
            WRObjectModel.ServiceRequestStatus merged = WRObjectModel.ServiceRequestStatus.Get("merged");
            WRObjectModel.ServiceRequestStatus denied = WRObjectModel.ServiceRequestStatus.Get("non-warrantable");
            if(usr.ServiceRequestStatusID == complete.ServiceRequestStatusID ||
                usr.ServiceRequestStatusID == merged.ServiceRequestStatusID ||
                usr.ServiceRequestStatusID == denied.ServiceRequestStatusID) {

                this.lblDisabledMsg.Text = "Note: Requests with the status of " + usr.ServiceRequestStatus.Name + " can not be modified. ";
                this.lblDisabledMsg.Visible = true;

            }  
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            // Create an event handler for the master page event when User updates Request details
            Master.ServiceRequestUpdated += new EventHandler(Master_ServiceRequestUpdated);
        }

        //protected void SetMasterPageValues() 
        //{
        //    WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
        //    ((Label)this.Master.FindControl("lblRequestID")).Text = requestID.ToString();
        //    ((Label)this.Master.FindControl("lblUnitAddress")).Text = Utilities.GetSingleLineAddress(usr.Unit, true);
        //    Label l = (Label)this.Master.FindControl("lblRequestor");
        //    l.Text = Utilities.GetServiceRequestContactDetails(usr, true);
        //    l.ToolTip = l.Text;            
        //}

        protected void LoadForm() {
            if (requestID != 0) {

                LoadProductCategoryDrop();
                LoadRelatedGrid();
                LoadUnrelatedGrid();     
            }
        }

        private void LoadProductCategoryDrop() {
            Category.DataSource = WRObjectModel.ComponentType.GetUnitLookupList(unitID);
            Category.DataTextField = "Name";
            Category.DataValueField = "ComponentTypeID";
            Category.DataBind();

            Category.Items.Insert(0, new ListItem("All", "0"));
        }


        void LoadRelatedGrid()
        {
          //grab all the related Products (Components) for the current Request 
          this.listRelatedComponents.DataSource = WRObjectModel.UnitServiceRequest.Get(requestID).GetRelatedComponents(
            new List<WRObjectModel.ServiceRequestProductRelationMethod>() { WRObjectModel.ServiceRequestProductRelationMethod.Get("builder") });
          
          this.listRelatedComponents.DataBind();
        }

        private WRObjectModel.ComponentFilterCriteria getFilterCriteria()
        {
          WRObjectModel.ComponentFilterCriteria crit = new WRObjectModel.ComponentFilterCriteria();
          crit.UnitID = unitID;

          //first, before retrieving the components, include any filters
          crit.KeyWord = Keyword.Text;
          if (!String.IsNullOrEmpty(Category.SelectedValue))
          {
            if (Category.SelectedValue != "0")
            {
              crit.ComponentTypeID = Convert.ToInt32(Category.SelectedValue);
            }
          }

          return crit;
        }

        void LoadUnrelatedGrid()      
        {
            //grab all the Products (Components) for the current unit that arent already related to Request.
            //...this list can be filtered by Category and keyword.
            this.listHomeProducts.DataSource = WRObjectModel.UnitServiceRequest.Get(requestID).GetUnrelatedComponents(
              getFilterCriteria(), 
              WRObjectModel.ServiceRequestProductRelationMethod.Get("builder"));

            this.listHomeProducts.DataBind();

        }

        protected void btnFilter_Click(object sender, EventArgs e) {
            LoadUnrelatedGrid();
        }


        protected void listRelatedComponents_ItemDataBound(object sender, DataListItemEventArgs e) {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                //any changes needed to row data, do it here
                //WRObjectModel.Component c = (WRObjectModel.MaintenanceChecklist)e.Item.DataItem;


                ////set URL for each componenet
                //HyperLink h = (HyperLink)e.Item.FindControl("lnkcomp");               
                //h.NavigateUrl = AppendUnit("Maintenance_Actions.aspx?ListID=" + cl.MaintenanceChecklistID.ToString());                             
            }
        }

        protected void list_ItemCreated(object sender, DataListItemEventArgs e)
        {
           
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {

                //attach popup behavior to each row
                AjaxControlToolkit.PopupControlExtender pce = (AjaxControlToolkit.PopupControlExtender)e.Item.FindControl("PopupControlExtender1");
                string id = "pce_" + e.Item.ItemIndex;
                if(pce == null) {
                    pce = (AjaxControlToolkit.PopupControlExtender)e.Item.FindControl("PopupControlExtender2");
                    id = "pce2_"+ e.Item.ItemIndex;
                }
                
                pce.BehaviorID = id;

                Image img = (Image)e.Item.FindControl("imgDetails");
                if(img == null) { img = (Image)e.Item.FindControl("imgDetails2"); };
                string mouseOver = string.Format("$find('{0}').showPopup();", id);
                string mouseOut = string.Format("$find('{0}').hidePopup();", id);

                img.Attributes.Add("onmouseover", mouseOver);
                img.Attributes.Add("onmouseout", mouseOut);


                //if the current request is a locked Status, disable add button for each row  
                LinkButton addBtn = (LinkButton)e.Item.FindControl("btnAddComponent");
                if(addBtn != null) {

                    if(lblDisabledMsg.Visible == true) {
                        //addBtn.Visible = false;
                        addBtn.Enabled = false;
                        addBtn.ForeColor = System.Drawing.Color.Gray;
                        addBtn.Style.Add("text-decoration", "none");

                    }
                
                }

                //if the current request is a locked Status, disable remove button for each row  
                LinkButton removeBtn = (LinkButton)e.Item.FindControl("btnRemoveComponent");
                if(removeBtn != null) {

                    if(lblDisabledMsg.Visible == true) {
                        //removeBtn.Visible = false;
                        removeBtn.Enabled = false;
                        removeBtn.ForeColor = System.Drawing.Color.Gray;
                        removeBtn.Style.Add("text-decoration", "none");
                    }
                 
                }

            }
                        
        }

        protected void listHomeProducts_ItemCommand(object source, DataListCommandEventArgs e) {

            if (e.CommandName == "select") {
                //save selected item to RequestComponent and RelationSource tables (checking for dups first) and rebind both datalists               

                int compID = Convert.ToInt32(((LinkButton)e.CommandSource).CommandArgument);
                WRObjectModel.UnitServiceRequest request = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
                WRObjectModel.ServiceResource.InsertRequestComponent(request, compID, "builder");

                LoadRelatedGrid();
                LoadUnrelatedGrid();
            }
        }
        

        protected void listRelatedComponent_ItemCommand(object source, DataListCommandEventArgs e) {

            if (e.CommandName == "remove") {
                //delete selected item from RelationSource table and possibly RequestComponent then rebind both datalists

                WRObjectModel.UnitServiceRequest request = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
                int componentID = Convert.ToInt32(((LinkButton)e.CommandSource).CommandArgument);
                WRObjectModel.ServiceResource.DeleteRequestComponent(request, componentID, WRObjectModel.ServiceRequestProductRelationMethod.Get("builder").Name);

                LoadRelatedGrid();
                LoadUnrelatedGrid();
            }
        }

        //this event is fired from the MasterPage when the user updates any Request data (like Status or Manager)
        private void Master_ServiceRequestUpdated(object sender, EventArgs e)
        {

            WRObjectModel.UnitServiceRequest usr = WRObjectModel.ServiceResource.GetServiceRequest(requestID);
 
            CheckForLockedStatus();
            LoadForm();
        
            this.UpdatePanel1.Update();                       
        }
              



        [System.Web.Services.WebMethodAttribute(),
        System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetPopupContent(string contextKey)
        {                     
            StringBuilder sb = new StringBuilder();

            //get Component object and build html output
            WRObjectModel.Component c = WRObjectModel.Component.Get(Convert.ToInt32(contextKey));
            if(c == null) {
                sb.Append("<div class='important topDblSpc' style='color: red'>No information found for this Product</div>");
            }
            else {                
                sb.Append("<h2 style='margin-bottom: 20px;'>Product Details</h2>"); 
                sb.Append("<div style='font-size: 1.1em; line-height: 1.4em;' class='bold'>");
                sb.Append("<div align='left'><span class='light'>Component Type: </span>" + c.ComponentType.ParentComponentType.Name + "</div>");
                sb.Append("<div align='left'><span class='light'>Item Type: </span>" + c.ComponentType.Name + "</div>");
                sb.Append("<div align='left'><span class='light'>Description: </span>" + c.ComponentDescription + "</div><br />");       
                sb.Append("<div align='left'><span class='light'>Manufacturer: </span>" + c.Manufacturer.CompanyLegalName + "</div>");
                sb.Append("<div align='left'><span class='light'>Supplier: </span>" + c.Supplier.CompanyLegalName + "</div>");
                sb.Append("<div align='left'><span class='light'>Installer: </span>" + c.Installer.CompanyLegalName + "</div><br />");
                sb.Append("<div align='left'><span class='light'>Location: </span>" + c.LocationType.Name + "</div>");
                sb.Append("<div align='left'><span class='light'>Location Description: </span>" + c.LocationDescription + "</div><br />");               
                sb.Append("</div>");
                sb.Append("<br /><div align='right' class='smltext dark'>Component ID: " + contextKey + "</div>");             

            }


            return sb.ToString();
        }

    }
}
