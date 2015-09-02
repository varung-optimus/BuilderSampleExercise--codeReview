using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WRObjectModel;
using WRWebControls;
using System.Web.Security;
using HomeOwner.Properties;
using Conasys.HIP.BusinessManager.Model;

namespace HomeOwner.app
{
    public partial class Builder_Homes : HIP_MasterBasePage
    {
        public static string GetUrl()
        {
            return GetBaseURL();
        }

        public static string GetUrl(int projectId)
        {
            return GetBaseURL() + "?projectID=" + projectId.ToString();
        }

        public static string GetUrl(int projectId, int unitID)
        {
            return GetBaseURL() + "?projectID=" + projectId.ToString() + "&unitID=" + unitID.ToString();
        }

        public static string GetItemUrl(int itemID)
        {
            return GetBaseURL() + "?itemID=" + itemID.ToString();
        }

        private static string GetBaseURL()
        {
            return "/app/Builder/Builder_Homes.aspx";
        }

        //TODO: We need to remove the reliance on ViewState
        public string GridViewSortExpression
        {
            get
            {
                if (ViewState["sortExp"] == null)
                    ViewState["sortExp"] = "Project";
                return (string)ViewState["sortExp"];
            }
            set
            {
                ViewState["sortExp"] = value;
            }
        }

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

        private void ShowHideSearch()
        {
            if (this.pnlFilter.Visible == false)
            {
                this.btnShowSearch.Text = "Hide Search";
                this.pnlFilter.Visible = true;
            }
            else
            {
                this.btnShowSearch.Text = "Search";
                this.pnlFilter.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillProjectListDropDown();
                FillCategoryDropDown();

                //fill any pre-selected filters
                if (!String.IsNullOrEmpty(Request.QueryString["projectID"]))
                {
                    ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(Request.QueryString["projectID"]));
                }

                if (!String.IsNullOrEmpty(Request.QueryString["unitID"]))
                {
                    WRObjectModel.Unit u = WRObjectModel.Unit.Get(Convert.ToInt32(Request.QueryString["unitID"]));
                    this.txtUnitNumber.Text = u.UnitDescription;
                    this.txtStreetAddress.Text = u.UnitStreet;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["itemID"]))
                {
                    Item i = Item.Get(Convert.ToInt32(Request.QueryString["itemID"]));
                    this.drpCategory.SelectedIndex = drpCategory.Items.IndexOf(
                      drpCategory.Items.FindByValue(i.ComponentType.RootComponentTypeID.ToString()));

                    if (i != null) this.txtProductName.Text = i.ItemName;
                }

                LoadUnitGrid("Project", SortDirection.Ascending);
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

        protected void FillCategoryDropDown()
        {
            ListItem li = new ListItem(" (All) ", "0");
            this.drpCategory.Items.Add(li);

            drpCategory.DataSource = WRObjectModel.ComponentType.GetLookupList();
            drpCategory.DataTextField = "Name";
            drpCategory.DataValueField = "ComponentTypeID";
            drpCategory.DataBind();
        }

        protected void LoadUnitGrid(string sortExpr, SortDirection sortDir)
        {
            int projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
            string unitNum = this.txtUnitNumber.Text;
            string address = this.txtStreetAddress.Text;
            int category = Convert.ToInt32(this.drpCategory.SelectedValue);
            string product = this.txtProductName.Text;
            string manuf = this.txtManufacturer.Text;
            string contact = this.txtContactName.Text;

            //Show any fiters 
            //Build filter string to display to user
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (projectID == 0 && string.IsNullOrEmpty(address) && string.IsNullOrEmpty(unitNum) && category == 0 &&
              string.IsNullOrEmpty(product) && string.IsNullOrEmpty(manuf) && string.IsNullOrEmpty(contact))
            {
                s.Append(" All Homes ");
            }
            else
            {
                string projectFilter = "";
                if (projectID > 0) projectFilter = " Within " + this.ddlProject.SelectedItem.Text + " Project";
                s.Append(projectFilter);

                string unitFilter = "";
                if (!String.IsNullOrEmpty(unitNum))
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        unitFilter = " Units with the Address like " + unitNum + " - " + address;
                    }
                    else
                    {
                        unitFilter = " Units with the Address like " + unitNum;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        unitFilter = " Units with the Address like " + address;
                    }
                }

                s.Append(unitFilter);

                string productFilter = "";
                if (category != 0) productFilter = " and Product Category equals " + this.drpCategory.SelectedItem.Text;
                if (!string.IsNullOrEmpty(product)) productFilter += " and Model contains " + product;
                if (!string.IsNullOrEmpty(manuf)) productFilter += " and Product Manufacturer is " + manuf;
                s.Append(productFilter);

                string contactFilter = "";
                if (!string.IsNullOrEmpty(contact)) contactFilter = " and Involved Contact is " + contact;
                s.Append(contactFilter);
            }

            //take away leading 'and'
            if (s.ToString().StartsWith(" and")) s.Remove(0, 4);

            //finally, display entire filter string to user

            this.lblFilterDisplay.Text = s.ToString();
            // Building portal change for homes
            if (s.ToString().Equals(" All Homes "))
            {
                this.lblFilterDisplay.Text = @"All Properties";
            }

            //if needed, add manufacturer to CompanyType list          
            List<int> compTypes = new List<int>();
            if (!String.IsNullOrEmpty(manuf))
            {
                compTypes.Add(7);
            }

            //this.gridHomes.DataSource = WRObjectModel.Unit.GetUnits(
            //  projectID,
            //  this.UserInfo.ByID,
            //  unitNum,
            //  address,
            //  category,
            //  product,
            //  compTypes,
            //  manuf,
            //  contact,
            //  true,
            //  sortExpr,
            //  sortDir);

            this.gridHomes.DataSource = UnitModel.GetUnitModelList(projectID,
              this.UserInfo.ByID,
              unitNum,
              address,
              category,
              product,
              compTypes,
              manuf,
              contact,
              sortExpr,
              sortDir);

            this.gridHomes.DataBind();
        }

        protected void gridHomes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    UnitModel u = (UnitModel)e.Row.DataItem;

                    //set PossDate textbox backcolor to color of row so it appears transparent
                    if (e.Row.RowState == DataControlRowState.Alternate)
                    {
                        ((TextBox)e.Row.FindControl("txtCalDate")).BackColor = this.gridHomes.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        ((TextBox)e.Row.FindControl("txtCalDate")).BackColor = this.gridHomes.RowStyle.BackColor;
                    }

                    //set up row click formatting and client event
                    e.Row.Attributes["ondblclick"] = "javascipt: window.open('" + Welcome.GetUrl(u.UnitID) + "')";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gridHomes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //need to store sortDirection and Expression in ViewState 
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

            GridViewSortExpression = e.SortExpression;
            LoadUnitGrid(e.SortExpression, e.SortDirection);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadUnitGrid(GridViewSortExpression, GridViewSortDirection);
        }

        protected void btnShowSearch_Click(object sender, EventArgs e)
        {
            ShowHideSearch();
        }

        protected void PossDateChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = tb.NamingContainer as GridViewRow;
            HiddenField hiddenUnitID = gvr.FindControl("hiddenUnitID") as HiddenField;

            int unitID = 0;
            if (!string.IsNullOrEmpty(hiddenUnitID.Value)) unitID = Convert.ToInt32(hiddenUnitID.Value);

            if (unitID != 0)
            {
                WRObjectModel.Unit.UpdatePossessionDate(unitID, tb.Text);
                //LoadUnitGrid(GridViewSortExpression, GridViewSortDirection);
            }
        }

        protected void btnClearCriteria_Click(object sender, EventArgs e)
        {
            this.ddlProject.SelectedIndex = 0;
            this.txtUnitNumber.Text = "";
            this.txtStreetAddress.Text = "";
            this.drpCategory.SelectedIndex = 0;
            this.txtProductName.Text = "";
            this.txtManufacturer.Text = "";
            this.txtContactName.Text = "";
        }

        protected void btReview_Command(Object sender, GridViewCommandEventArgs e)
        {
            // If multiple buttons are used in a GridView control, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "DeficiencyStart")
            {
                BasePage bp = new BasePage();
                int userId = bp.UserInfo.ByID;
                int unitId = Convert.ToInt32(e.CommandArgument.ToString());
                BuilderPortalAccount builderPortalAccount = BuilderPortalAccount.GetByUserID(userId);
                string token = builderPortalAccount.Token;
                if(builderPortalAccount.Token==null)
                {
                    Guid tokenId = Guid.NewGuid();
                    token = tokenId.ToString();
                    BuilderPortalAccount.UpdateBuilderToken(userId, token);
                }
              
                token = MakeToken(token);
                string redirectUrl = Settings.Default.PDIRootUrl + "/?unitid=" + unitId+"&userId="+userId+"&token="+token;
                Response.Redirect(redirectUrl);
            }
        }
        private static string MakeToken(string token)
        {
            SimpleAES aes = new SimpleAES();

            XDocument x = new XDocument(new XElement("T", token));

            string ret = aes.EncryptToString(x.ToString());

            return ret;
        }
    }
}
