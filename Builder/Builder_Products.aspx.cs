using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using System.Web.Security;

namespace HomeOwner.app 
{    
  public partial class Builder_Products : HIP_MasterBasePage     
  {
    protected override string GetSelectedResource()
    {
      return "Warranty";
    }

    protected override string GetSelectedResourceLink()
    {
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

    public static string GetProjectUrl(int projectID)
    {
      return GetBaseURL() + "?projectID=" + projectID.ToString();
    }

    private static string GetBaseURL()
    {
      return "/app/Builder/Builder_Products.aspx";
    }

    public static string GetUrlByCompany(int companyID, int companyTypeID)
    {
      if (companyTypeID == 6 || companyTypeID == 7 || companyTypeID == 11)
      {
        return "/app/Builder/Builder_Products.aspx?cmpID=" + companyID.ToString() + "&ctID=" + companyTypeID.ToString();
      }
      else
      {
        return "#";
      }
    }

    public int _unitID
    {
      get
      {
        string s = Request.QueryString["unitID"] ?? "0";
        return Convert.ToInt32(s);
      }
    }

    public string GridViewSortExpression
    {
      get
      {
        if (ViewState["sortExp"] == null)
          ViewState["sortExp"] = "ComponentType.ParentComponentType.Name";
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
          ViewState["sortDirection"] = SortDirection.Ascending;
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
    
    protected void Page_Load(object sender, EventArgs e) 
    {
      if(!Page.IsPostBack) 
      {
        FillProjectListDropDown();
        FillCategoryDropDown();

        //fill any pre-selected filters
        if (_unitID != 0)
        {
          WRObjectModel.Unit u = WRObjectModel.Unit.Get(_unitID);
          ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(u.Phase.ProjectID.ToString()));
          this.txtUnitNumber.Text = u.UnitDescription;
          this.txtStreetAddress.Text = u.UnitStreet;
          hfUnitID.Value = _unitID.ToString();
        }
        else
        {
          hfUnitID.Value = "0";
        }

        if(!String.IsNullOrEmpty(Request.QueryString["projectID"])) 
        {
           ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(Request.QueryString["projectID"]));
        }

        if(!String.IsNullOrEmpty(Request.QueryString["cmpID"])) 
        {
          WRObjectModel.Company c = WRObjectModel.Company.Get(Convert.ToInt32(Request.QueryString["cmpID"]));

          //check CompanyType to see which textbox to populate
          if(!String.IsNullOrEmpty(Request.QueryString["ctID"])) 
          {
            if (Request.QueryString["ctID"] == "7")
            {
              this.txtManufacturer.Text = c.CompanyLegalName;
            }
            else
            {
              this.txtContactName.Text = c.CompanyLegalName;
            }
          }                                                       
        }

        LoadProductsGrid("ComponentType.ParentComponentType.Name", SortDirection.Ascending);                                        
      }
    }
        
    protected void LoadProductsGrid(string sortExpr, SortDirection sortDir) 
    {
      int projectID = Convert.ToInt32(this.ddlProject.SelectedValue);
      List<int> listProjectIDs = new List<int>();
      if(projectID == 0) 
      {
        foreach(ListItem i in this.ddlProject.Items) 
        {
          if(i.Value != "0") 
          {
              listProjectIDs.Add(Convert.ToInt32(i.Value));
          }
        }                
      }
      else 
      {
        listProjectIDs.Add(projectID);
      }

      string unitNum = this.txtUnitNumber.Text;
      string address = this.txtStreetAddress.Text;
      int category = Convert.ToInt32(this.drpCategory.SelectedValue);
      string product = this.txtProductName.Text;
      string manuf = this.txtManufacturer.Text;        
      string contact = this.txtContactName.Text;
      int companyID = Convert.ToInt32(Request.QueryString["cmpID"] ?? "0");
      
      //Show any fiters 
      //Build filter string to display to user
      System.Text.StringBuilder s = new System.Text.StringBuilder();
      if(projectID == 0 && string.IsNullOrEmpty(address) && string.IsNullOrEmpty(unitNum) && category == 0
         && string.IsNullOrEmpty(product) && string.IsNullOrEmpty(manuf) && string.IsNullOrEmpty(contact)) 
      {
        s.Append(" All Products ");
      }
      else 
      {
        string projectFilter = "";
        if(projectID > 0) projectFilter = " Within " + this.ddlProject.SelectedItem.Text + " Project";
        //if(companyID > 0) projectFilter += " and CompanyID = " + companyID.ToString();
        s.Append(projectFilter);
       
        string unitFilter = "";
        if(!String.IsNullOrEmpty(unitNum)) 
        {
          if(!string.IsNullOrEmpty(address)) 
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
          if(!string.IsNullOrEmpty(address)) 
          {
            unitFilter = " Units with the Address like " + address;
          }
        }

        //todo: check if the unit filters result in a unique unit
        //if so, add the unit id to the hfUnitID field and use it in the search criteria
        try
        {
          int swap = int.Parse(hfUnitID.Value);

          if (swap == 0)
          {
            throw new Exception("No unit specified");
          }

          WRObjectModel.Unit home = WRObjectModel.Unit.Get(swap);

          if (!(home.UnitDescription.Equals(txtUnitNumber.Text) && home.UnitStreet.Equals(home.UnitStreet)))
          {
            throw new Exception("Unit has changed");
          }          
        }
        catch
        {
          hfUnitID.Value = "0";
        }

        s.Append(unitFilter);

        string productFilter = "";
        if(category != 0) productFilter = " and Product Category equals " + this.drpCategory.SelectedItem.Text;
        if(!string.IsNullOrEmpty(product)) productFilter += " and Model contains " + product;
        if(!string.IsNullOrEmpty(manuf)) productFilter += " and Product Manufacturer is " + manuf;
        s.Append(productFilter);

        string contactFilter = "";
        if(!string.IsNullOrEmpty(contact)) contactFilter = " and Involved Contact contains " + contact;
        s.Append(contactFilter);
      }

      //take away leading 'and'
      if(s.ToString().StartsWith(" and")) s.Remove(0, 4);

      //finally, display entire filter string to user
      this.lblFilterDisplay.Text = s.ToString();                            

      this.gridProducts.DataSource = WRObjectModel.Item.GetItemsUsingFilters
          (listProjectIDs, int.Parse(hfUnitID.Value), category, unitNum, address, product, manuf, contact, companyID, sortExpr, sortDir);
      this.gridProducts.DataBind();
    }

    protected void gridProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if(e.Row.RowType == DataControlRowType.DataRow) 
      {
        WRObjectModel.Item i = (WRObjectModel.Item)e.Row.DataItem;

        //set the Details link                   
        HyperLink lnkDetails = ((HyperLink)e.Row.FindControl("lnkDetails"));
        lnkDetails.NavigateUrl = Builder_ProductDetails.GetUrl(i.ItemID);

        //link to Units on Homes page that use this Item
        HyperLink lnkHome = ((HyperLink)e.Row.FindControl("lnkHome"));
        lnkHome.NavigateUrl = Builder_Homes.GetItemUrl(i.ItemID);
        
        //set up row click formatting and client event
        e.Row.Attributes["ondblclick"] = "javascipt: window.open('" + Builder_ProductDetails.GetUrl(i.ItemID) + "')";           
      }
    }
    
    protected void gridProducts_Sorting(object sender, GridViewSortEventArgs e)
    {
      //need to store sortDirection and Expression in ViewState 
      if(GridViewSortDirection == SortDirection.Ascending) 
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

      LoadProductsGrid(e.SortExpression, e.SortDirection);
    }

    protected void btnFilter_Click(object sender, EventArgs e) 
    {

      LoadProductsGrid(GridViewSortExpression, GridViewSortDirection);
    }
            
    protected void btnShowSearch_Click(object sender, EventArgs e)
    {
      ShowHideSearch();  
    }
    
    protected void btnClearCriteria_Click(object sender, EventArgs e)
    {
      this.ddlProject.SelectedIndex = 0;
      this.txtUnitNumber.Text = "";
      this.txtStreetAddress.Text = "";
      this.drpCategory.SelectedIndex = 0;
      this.txtProductName.Text = "";
      this.txtManufacturer.Text = "";
    }
  }
}
