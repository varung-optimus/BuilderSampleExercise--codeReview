using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using System.Web.Security;
using WRObjectModel;

namespace HomeOwner
{
  public partial class ProjectDetails : HomeOwner.code.HipBaseClass 
  {
    public static string GetUrl(int projectID)
    {
      return GetBaseURL() + "?pID=" + projectID.ToString();
    }

    private static string GetBaseURL()
    {
      return "/app/Builder/ProjectDetails.aspx";
    }

    public int ProjectID
    {
      get
      {
        string s = Request.QueryString["pID"] ?? "0";
        return Convert.ToInt32(s);
      }
    }

    protected Project CurrentProject()
    {
      WRObjectModel.Project result = null;

      if (!String.IsNullOrEmpty(Request.QueryString["pID"]))
      {
        result = WRObjectModel.Project.Get(int.Parse(Request.QueryString["pID"]));
      }

      return result;
    }

    //need to override baseclass that sets this page's theme on the UnitID
    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e);
      this.Theme = "BuilderDefault";
    }

    protected void Page_Load(object sender, EventArgs e) 
    {
      if (!Page.IsPostBack) 
      {
        LoadForm();
      }
    }
    
    protected void LoadForm()
    {
      //bool success = false;

      //try
      //{
      //  try
      //  {
      //    if (CurrentProject().GetBanner() != null)
      //    {
      //      imgProjectBanner.ImageUrl = CurrentProject().GetBanner().Url;
      //    }
      //  }
      //  catch
      //  {

      //  }

      //  lnkViewHomes.NavigateUrl = HomeOwner.app.Builder_Homes.GetUrl(CurrentProject().ProjectID);
      //  lnkViewProducts.NavigateUrl = HomeOwner.app.Builder_Products.GetUrl(CurrentProject().ProjectID);

      //  //if(CurrentProject()
      //  bool sa = false;

      //  foreach (ProjectResource r in CurrentProject().ProjectResources)
      //  {
      //    if (r.ResourceID == 2)
      //    {
      //      sa = true;
      //      break;
      //    }
      //  }

      //  if (sa)
      //  {
      //    lnkViewRequests.NavigateUrl = HomeOwner.app.Builder_ServiceRequestList.GetUrl(CurrentProject().ProjectID);
      //  }
      //  else
      //  {
      //    lnkViewRequests.NavigateUrl = "#";
      //    spanlnkRequests.Visible = false;
      //  }

      //  litProjectName.Text = CurrentProject().ProjectName;
      //  litAddress.Text = CurrentProject().Address1;
      //  if (CurrentProject().City != null)
      //  {
      //    litProvince.Text = CurrentProject().City.Name + ", " + CurrentProject().City.Province.ShortName;
      //  }
      //  else
      //  {
      //    litProvince.Text = "";
      //  }

      //  litLegalDescription.Text = CurrentProject().LegalDescription;

      //  //load configuration
      //  try
      //  {
      //    int homeWarrantyId = -1;
      //    if (CurrentProject().GetHomeWarrantyID() != 0)
      //    {
      //      homeWarrantyId = CurrentProject().GetHomeWarrantyID();
      //    }
          
      //    ddlHomeWarrantyPlan.SelectedValue = homeWarrantyId.ToString();
      //  }
      //  catch
      //  {
      //    ddlHomeWarrantyPlan.SelectedIndex = 0;
      //  }
        
      //  try
      //  {
      //    chkHomeWarrantyReminders.Checked = CurrentProject().GetHomeWarrantyRemindersEnabled();
      //  }
      //  catch
      //  {
      //    chkHomeWarrantyReminders.Checked = false;
      //  }

      //  try
      //  {
      //    int maintenancePlanId = -1;
      //    if (CurrentProject().GetMaintenancePlanID() != 0)
      //    {
      //      maintenancePlanId = CurrentProject().GetMaintenancePlanID();
      //    }

      //    ddlMaintenancePlan.SelectedValue = maintenancePlanId.ToString();
      //  }
      //  catch
      //  {
      //    ddlMaintenancePlan.SelectedIndex = 0;
      //  }

      //  try
      //  {
      //    chkMaintenanceReminders.Checked = CurrentProject().GetMaintenanceRemindersEnabled();
      //  }
      //  catch
      //  {
      //    chkMaintenanceReminders.Checked = false;
      //  }

      //  success = true;
      //}
      //catch
      //{
      //  success = false;
      //}

      //TODO: hide the configuration panel if it fails to load.
    }

    protected void SaveConfiguration(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}
