using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HomeOwner.Code;
using WRObjectModel;
using WRWebControls;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeOwner.app.Builder
{
  public partial class Contacts : HIP_MasterBasePage    
  {
    protected override string GetSelectedResource()
    {
      return "Warranty";
    }
    
    protected override string GetSelectedResourceLink()
    {
      return GetBaseUrl();
    }
    
    public static string GetUrl()
    {
      return AppendUnit(VirtualPathUtility.ToAbsolute("~" + GetBaseUrl()));        
    }

    private static string GetBaseUrl()
    {
      return "/app/Builder/Contacts.aspx";
    }
    
    public static string GetUrl(int unitID)
    {
      return GetBaseUrl() + "?uid=" + unitID.ToString();
    }

    public static string GetUrl(string companyName, int roleID)
    {
      return "/app/Builder/Contacts.aspx?name=" + companyName + "&roleID=" + roleID.ToString();
    }

    protected override void OnLoad(EventArgs e)        
    {
      base.OnLoad(e);
      
      if(!IsPostBack) 
      {
        FillProjectListDropDown();
        FillCompanyTypeDropDown(Request.QueryString["roleID"]);
        
        if(!String.IsNullOrEmpty(Request.QueryString["name"])) 
        {
          this.txtName.Text = Request.QueryString["name"];
        }
        
        LoadContactGrid();
      }
    }
    
    protected void FillProjectListDropDown()
    {
      ListItem li = new ListItem(" (All) ", "0");      
      this.drpProject.Items.Add(li);

      this.drpProject.DataSource = BuilderPortal.DataSources.GetProjectList(this.UserInfo.ByID);      
      this.drpProject.DataTextField = "ProjectName";
      this.drpProject.DataValueField = "ProjectID";
      this.drpProject.DataBind();
    }

    protected void FillCompanyTypeDropDown(string roleID)        
    { 
      if(roleID != null) 
      {
        this.drpRole.SelectedIndex = drpRole.Items.IndexOf(drpRole.Items.FindByValue(roleID.ToString()));
      }
    }

    protected void LoadContactGrid()        
    {
            //create list of wanted RoleIDs (CompanyTypes)
            int roleID = Convert.ToInt32(this.drpRole.SelectedValue);
            List<int> roleList = new List<int>();
            if(roleID == 0) {
                foreach(ListItem i in this.drpRole.Items) {
                    if(i.Value != "0") {
                        roleList.Add(Convert.ToInt32(i.Value));
                    }
                }
            }
            else {
                //just one requested
                roleList.Add(roleID);
            }
                                                                             
            //build Project List if 'All' is selected
            int projectID = Convert.ToInt32(this.drpProject.SelectedValue);
            List<int> projectList = new List<int>();
            if(projectID == 0) {
                foreach(ListItem i in this.drpProject.Items) {
                    if(i.Value != "0") {
                        projectList.Add(Convert.ToInt32(i.Value));
                    }
                }
            }
            else {
                //just one requested
                projectList.Add(projectID);
            }

            string companyName = this.txtName.Text;

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if(projectID == 0 && roleID == 0 && string.IsNullOrEmpty(companyName)) {
                s.Append(" All Contacts ");
            }
            else {

                string projectFilter = "";
                if(projectID > 0) projectFilter = " Within " + this.drpProject.SelectedItem.Text + " Project";             
                s.Append(projectFilter);

                string roleFilter = "";
                if(roleID != 0) roleFilter = " and Contact Role is " + this.drpRole.SelectedItem.Text;
                s.Append(roleFilter);

                string contactFilter = "";
                if(!string.IsNullOrEmpty(companyName)) contactFilter = " and Company Name contains " + companyName;
                s.Append(contactFilter);

            }

            //take away leading 'and'
            if(s.ToString().StartsWith(" and")) s.Remove(0, 4);
            //finally, display entire filter string to user
            this.lblFilterDisplay.Text = s.ToString();


            //this.gridContacts.DataSource = WRObjectModel.Company.GetContacts(projectList, 0, roleList, companyName, sortExpr, sortDir);
            //this.gridContacts.DataBind();            

        }

    protected void gridContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if(e.Row.RowType == DataControlRowType.DataRow) 
      {
        ProjectContactCompany cc = (ProjectContactCompany)e.Row.DataItem;
        int companyID = cc.CompanyID;
        int companyTypeID = cc.CompanyTypeID;

        HyperLink lnkDetails = ((HyperLink)e.Row.FindControl("lnkDetails"));
        if (Builder_Products.GetUrlByCompany(companyID, companyTypeID) == "#")
        {
          lnkDetails.Visible = false;
        }
        else
        {
          lnkDetails.Visible = true;
          lnkDetails.NavigateUrl = Builder_Products.GetUrlByCompany(companyID, companyTypeID);
        }
        
        if (!String.IsNullOrEmpty(cc.Company.WebSite))
        {
          if (cc.Company.WebSite.Contains("http:"))
          {
            ((Literal)e.Row.FindControl("litContactWeb")).Text = "<a href='" + cc.Company.WebSite + "' target='_blank'>" + cc.Company.WebSite + "</a>";
          }
          else
          {
            ((Literal)e.Row.FindControl("litContactWeb")).Text = "<a href='http://" + cc.Company.WebSite + "' target='_blank'>" + cc.Company.WebSite + "</a>";
          }
        }
        else
        {
          ((Literal)e.Row.FindControl("litContactWeb")).Text = "";
        }
        
        ((Literal)e.Row.FindControl("litContactAddress")).Text = cc.Company.GetMultiLineAddress();
      }
    }

    protected void btnFilter_Click(object sender, EventArgs e)    
    {
      LoadContactGrid();
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
        this.drpProject.SelectedIndex = 0;
        this.drpRole.SelectedIndex = 0;
        this.txtName.Text = "";      
    }

    public static IQueryable SearchBuilderContacts(int builderPortalId, int projectId, int companyTypeId, string companyName, string sortExpression)
    {
      List<int> projectList = new List<int>();

      var projects = BuilderPortalAccount.GetAccessibleProjects(UserMap.Get(builderPortalId).UserName);

      if (projectId != 0)
      {
        bool allowed = false;

        foreach (Project p in projects)
        {
          if (p.ProjectID == projectId)
          {
            allowed = true;
            break;
          }
        }

        if (allowed)
        {
          projectList.Add(projectId);
        }
        else
        {
          //if attempting to access an invalid project return everything.
          foreach (Project p in projects)
          {
            projectList.Add(p.ProjectID);
          }
        }
      }
      else
      {
        foreach (Project p in projects)
        {
          projectList.Add(p.ProjectID);
        }
      }      
      
      bool descSort = false;
      string sortExp = sortExpression;
      if (sortExp.Contains("DESC"))
      {
        descSort = true;
        sortExp = sortExp.Replace("DESC", "").Trim();
      }

      switch (sortExp)
      {
        case "Project.ProjectName":
          if (descSort)
          {
            return ProjectContactCompany.Search(projectList, companyTypeId, companyName).OrderByDescending(cc => cc.Project.ProjectName);
          }
          else
          {
            return ProjectContactCompany.Search(projectList, companyTypeId, companyName).OrderBy(cc => cc.Project.ProjectName);
          }

          break;

        case "CompanyType.Name":
          if (descSort)
          {
            return ProjectContactCompany.Search(projectList, companyTypeId, companyName).OrderByDescending(cc => cc.CompanyType.Name);
          }
          else
          {
            return ProjectContactCompany.Search(projectList, companyTypeId, companyName).OrderBy(cc => cc.CompanyType.Name);
          }

          break;

        case "Company.CompanyLegalName":
          if (descSort)
          {
            return ProjectContactCompany.Search(projectList, companyTypeId, companyName).OrderByDescending(cc => cc.Company.CompanyName);
          }
          else
          {
            return ProjectContactCompany.Search(projectList, companyTypeId, companyName).OrderBy(cc => cc.Company.CompanyName);
          }

          break;

        default:
          return ProjectContactCompany.Search(projectList, companyTypeId, companyName);

          break;
      }      
    }
    protected void LoadPortalID(object sender, EventArgs e)
    {
      hfBuilderPortalID.Value = UserInfo.ByID.ToString();
    }
  }
}
