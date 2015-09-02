using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using System.Web.Security;

namespace HomeOwner.app {
    public partial class Builder_MaintenanceAlerts : HIP_MasterBasePage 
    {
          
        protected override string GetSelectedResource() {
            return "Maintenance";
        }
        protected override string GetSelectedResourceLink() {
            return GetBaseURL();
        }
        public static string GetUrl()
        {
            return GetBaseURL();
        }
        public static string GetUrl(int unitID)
        {
            return GetBaseURL() + "?unitID=" + unitID.ToString();
        }
        public static string GetBaseURL() 
        {
            return "/app/Builder/Builder_MaintenanceAlerts.aspx";
        }



        protected void Page_Load(object sender, EventArgs e)
        {            
        
            if(!Page.IsPostBack) {

                FillProjectListDropDown();

                //fill any pre-selected filters
                //Project
                if(!String.IsNullOrEmpty(Request.QueryString["projectID"])) {
                    ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(Request.QueryString["projectID"]));
                }
                //Unit Address
                //if(!String.IsNullOrEmpty(Request.QueryString["addr"])) {
                //    this.txtAddress.Text = Request.QueryString["addr"];
                //} 
                if(!String.IsNullOrEmpty(Request.QueryString["unitID"])) {
                    WRObjectModel.Unit u = WRObjectModel.Unit.Get(Convert.ToInt32(Request.QueryString["unitID"]));
                    this.txtUnitNumber.Text = u.UnitDescription;
                    this.txtAddress.Text = u.UnitStreet;                   
                }


                
                LoadGrid("Project", SortDirection.Ascending);

            }
        }


        protected void FillProjectListDropDown()
        {
            ListItem li = new ListItem(" (All) ", "0");
            this.ddlProject.Items.Add(li);

            ddlProject.DataSource = BuilderPortal.DataSources.GetProjectList(this.UserInfo.ByID);
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
        }

        //private void LoadUnitDrop()
        //{
        //    this.drpUnits.DataSource = WRObjectModel.MaintenanceResource.GetUnitsWithAlerts(UserInfo.ByID, 0);
        //    this.drpUnits.DataTextField = "UnitStreet";
        //    this.drpUnits.DataValueField = "UnitID";
        //    this.drpUnits.DataBind();

        //    this.drpUnits.Items.Insert(0, new ListItem("All", "0"));
             
        //}


       
        protected void LoadGrid(string sortExpr, SortDirection sortDir)
        {
            //gather filters
            int projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            string address = this.txtAddress.Text;
            string unitNum = this.txtUnitNumber.Text;
            string userName = this.txtUserName.Text;
            string userEmail = this.txtUserEmail.Text;

            //dates
            string date1 = this.txtFromDate.Text ?? null;
            string date2 = this.txtToDate.Text ?? null;
            DateTime? sdate = null;
            DateTime? edate = null;
            if(!string.IsNullOrEmpty(date1)) sdate = Convert.ToDateTime(date1);
            if(!string.IsNullOrEmpty(date2)) edate = Convert.ToDateTime(date2);


            //Build filter string to display to user
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if(projectID == 0 && string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(address) && string.IsNullOrEmpty(userEmail)
                    && string.IsNullOrEmpty(unitNum) && sdate == null && edate == null) {
                s.Append(" All Alerts ");
            }
            else {
                string projectFilter = "";
                if(projectID > 0) projectFilter = " Within " + this.ddlProject.SelectedItem.Text + " Project";
                s.Append(projectFilter);

                string unitFilter = "";
                if(!String.IsNullOrEmpty(unitNum)) {
                    if(!string.IsNullOrEmpty(address)) {
                        unitFilter = " Units with the Address like " + unitNum + " - " + address;
                    }
                    else {
                        unitFilter = " Units with the Address like " + unitNum;
                    }
                }
                else {
                    if(!string.IsNullOrEmpty(address)) {
                        unitFilter = " Units with the Address like " + address;
                    }
                }
                s.Append(unitFilter);

                string userFilter = "";
                if(!string.IsNullOrEmpty(userName)) userFilter = " and User Name like " + userName;
                if(!string.IsNullOrEmpty(userEmail)) userFilter += " and User Email contains " + userEmail;
                s.Append(userFilter);

                string datesFilter = "";
                if(sdate != null) datesFilter = " and Alert was sent on or after " + sdate.Value.ToLongDateString();
                if(edate != null) datesFilter += " and Alert was sent before " + edate.Value.ToLongDateString();
                s.Append(datesFilter);

            }
            
            //take away leading 'and'
            if(s.ToString().StartsWith(" and")) s.Remove(0, 4);

            //finally, display entire filter string to user
            this.lblFilterDisplay.Text = s.ToString();


            //Load grid from LINQ Object Model using any criteria
            this.gridAlerts.DataSource = WRObjectModel.MaintenanceResource.GetSentAlerts
                (projectID, UserInfo.ByID, unitNum, address, userName, userEmail, sdate, edate, sortExpr, sortDir);            
            this.gridAlerts.DataBind();                        


        }
              

        protected void gridAlerts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //gather and display Alerts info

            if(e.Row.RowType == DataControlRowType.DataRow) {

                WRObjectModel.UnitMaintenanceAlert uma = (WRObjectModel.UnitMaintenanceAlert)e.Row.DataItem;

                ((Literal)e.Row.FindControl("litGridRowAddress")).Text = uma.Unit.GetHtmlMultiLineAddress();

                if(uma.Recipient != null) {
                    MembershipUser user = Membership.GetUser(uma.Recipient.UserName);
                    if(user != null) {
                        if(!string.IsNullOrEmpty(user.UserName)) {
                            System.Web.Profile.ProfileBase profile = WRObjectModel.Users.User.GetUserProfile(user.UserName);

                            Literal litUser = (Literal)e.Row.FindControl("litGridRowUser");
                            litUser.Text = profile.GetPropertyValue("FirstName").ToString() + " " + profile.GetPropertyValue("LastName").ToString();
                            if(!string.IsNullOrEmpty(user.Email)) litUser.Text += 
                                "<br><a style='background: none;' href='mailto:" + user.Email + "'>" + user.Email + "</a>";
                            if(!string.IsNullOrEmpty(profile.GetPropertyValue("PhoneNumber").ToString())) litUser.Text +=
                                "<br>" + profile.GetPropertyValue("PhoneNumber").ToString();
                          
                        }
                    }
                }
            }

        }
        
     

        protected void btnFilter_Click(object sender, EventArgs e)
        {
           
            LoadGrid("Project", SortDirection.Ascending);
                                    
        }
        

        protected void btnShowSearch_Click(object sender, EventArgs e)
        {
            ShowHideSearch();
        }

        private void ShowHideSearch()
        {
            if(this.pnlFilter.Visible == false) {
                this.btnShowSearch.Text = "Hide Search";
                this.pnlFilter.Visible = true;
            }
            else {
                this.btnShowSearch.Text = "Search";
                this.pnlFilter.Visible = false;
            }

        }
        

        protected void btnClearCriteria_Click(object sender, EventArgs e)
        {
            this.ddlProject.SelectedIndex = 0;
            this.txtAddress.Text = "";
            this.txtUnitNumber.Text = "";
            this.txtUserEmail.Text = "";
            this.txtUserName.Text = "";        
            this.txtToDate.Text = "";
            this.txtFromDate.Text = "";
                       
        }

        protected void gridAlerts_Sorting(object sender, GridViewSortEventArgs e)
        {
            //need to store sortDirection in ViewState 
            if(GridViewSortDirection == SortDirection.Ascending) {

                GridViewSortDirection = SortDirection.Descending;
                e.SortDirection = SortDirection.Descending;
            }
            else {
                GridViewSortDirection = SortDirection.Ascending;
                e.SortDirection = SortDirection.Ascending;
            }
                     
            LoadGrid(e.SortExpression, e.SortDirection);

        }


        public SortDirection GridViewSortDirection
        {
            get
            {
                if(ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];

            }
            set { ViewState["sortDirection"] = value; }
        }



    }
}
