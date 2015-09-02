using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using System.Web.Security;

namespace HomeOwner.app.Builder 
{
  public partial class Lobby : HIP_MasterBasePage 
  {
    protected void Page_Load(object sender, EventArgs e) 
    {
      if(!Page.IsPostBack) 
      {
        LoadAlertsGrid();
        LoadProjectGrid();
      }
    }
    
    public static string GetUrl()
    {
      return GetBaseURL();
    }
    
    private static string GetBaseURL() 
    {
      return "/app/Builder/Lobby.aspx";
    }
    
    protected void LoadAlertsGrid()
    {
      List<int> statusForServiceRequests = new List<int>();
      statusForServiceRequests.Add(WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID);
      this.gridAlerts.DataSource = WRObjectModel.BuilderPortalAccount.GetBuilderAlerts
          (UserInfo.ByID, this.Page.Theme, DeliveryReview.GetUrl(0), DeliveryReviewList.GetUrl(), statusForServiceRequests,
           Builder_ServiceRequestMain.GetUrl(0), Builder_ServiceRequestList.GetUrl());
      this.gridAlerts.DataBind();


      if(this.gridAlerts.Rows.Count == 0) 
      {
        this.panelAlerts.Visible = false;
        this.gridAlerts.Visible = false;
        this.imgAlert.Visible = false;
        this.imgExcl.Visible = false;
      }
    }
    
    protected void gridAlerts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if(e.Row.RowType == DataControlRowType.DataRow) 
      {
        var x = e.Row.DataItem;
        string alertName = (string)DataBinder.Eval(x, "AlertName");
        string title = (string)DataBinder.Eval(x, "DisplayTitle");
        string linkURL = (string)DataBinder.Eval(x, "LinkURL");
        int count = (int)DataBinder.Eval(x, "AlertCount");
        
        //tools                  
        HyperLink lnkAlert = ((HyperLink)e.Row.FindControl("lnkDetails"));

        //message
        Literal litMsg = (Literal)e.Row.FindControl("litMsg");
        if(count > 1) 
        {
          litMsg.Text = "You have " + count.ToString() + " " + title +
            " that require your attention.<br><a  href='" +
            linkURL + " '>Click here to view them all</a>";
        }
        else 
        {
          string s = title.EndsWith("s") ? title.Remove(title.Length-1, 1) : title;              
          litMsg.Text = "You have one " + s +" that requires your attention.<br><a target='_blank' href='" +
            linkURL + " '>Click here to go directly there</a>";
        }
      }
    }

    protected void LoadProjectGrid() 
    {
      this.gridProjects.DataSource = BuilderPortal.DataSources.GetProjectList(this.UserInfo.ByID);
      this.gridProjects.DataBind();
    }
    
    protected void gridProjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if(e.Row.RowType == DataControlRowType.DataRow) 
      {
        WRObjectModel.Project p = (WRObjectModel.Project)e.Row.DataItem;

        //******* Set the grid tools links 

        #region self-serve configuration
        //NOTE: this is to be restored after the portal revamp (with new configuration screen)
        ((Panel)e.Row.FindControl("pnlConfiguration")).Visible = false; 
        //((HyperLink)e.Row.FindControl("lnkConfigurationHeaderText")).NavigateUrl = ProjectDetails.GetUrl(p.ProjectID);
        ((HyperLink)e.Row.FindControl("lnkConfigurationHeaderText")).NavigateUrl = "#";
        ((HyperLink)e.Row.FindControl("lnkConfigurationHeaderText")).Text = p.ProjectName;
        //((HyperLink)e.Row.FindControl("lnkConfigurationIcon")).NavigateUrl = ProjectDetails.GetUrl(p.ProjectID);
        //((HyperLink)e.Row.FindControl("lnkConfigurationText")).NavigateUrl = ProjectDetails.GetUrl(p.ProjectID);
        #endregion

        //Delivery Reviews - go directly if only one or list if there are more
        HyperLink lnkDelivery = ((HyperLink)e.Row.FindControl("lnkDelReviews"));
        HyperLink lnkDelivery2 = ((HyperLink)e.Row.FindControl("lnkDelReviews2"));
        Literal litDR = (Literal)e.Row.FindControl("litDelReview");
        int reviewCount = WRObjectModel.ProjectDeliveryReview.GetPendingDeliveryReviewCountsByProject(p.ProjectID);

        //Project Banner
        Image img = ((Image)e.Row.FindControl("imgProjectBanner"));
        try
        {
          WRObjectModel.Document banner = p.GetBanner();
          img.Height = Unit.Pixel(110);
          img.ImageUrl = banner.GetUrl();
          if (!img.ImageUrl.ToLower().EndsWith(".jpg"))
          {
            throw new Exception("Banner must be a JPEG (.jpg)");
          }
        }
        catch
        {
          img.CssClass = "banner";
        }

        //Project Address
        ((Literal)e.Row.FindControl("litAddress")).Text =
          "<h3 style='padding: 0px; margin: 0px' class='dark'>" + Utilities.GetMultiLineProjectAddress(p, false) + "</h3>";

        if (reviewCount == 0)
        {
          Panel panel = ((Panel)e.Row.FindControl("pnlDeliveryReviews"));
          panel.Visible = false;
        }
        else if (reviewCount == 1)
        {          
          //if there is only one review we must get the link to the active review
          WRObjectModel.ProjectDeliveryReview review = new WRObjectModel.ProjectDeliveryReview();
          IQueryable pendingReviews = WRObjectModel.ProjectDeliveryReview.GetPendingDeliveryReviews(UserInfo.ByID, p.ProjectID);
          foreach (WRObjectModel.ProjectDeliveryReview pendingReview in pendingReviews)
          {
            review = pendingReview;
            break;
          }
          lnkDelivery.NavigateUrl = DeliveryReview.GetUrl(review.ProjectDeliveryReviewID);
          lnkDelivery2.NavigateUrl = DeliveryReview.GetUrl(review.ProjectDeliveryReviewID);
        }
        else if (p.ProjectDeliveryReviews.Count > 1)
        {
          lnkDelivery.NavigateUrl = DeliveryReviewList.GetUrl(p.ProjectID);
          lnkDelivery2.NavigateUrl = DeliveryReviewList.GetUrl(p.ProjectID);
        }

        #region Determine Accessible Resources
                
        bool wr = p.ProductListEnabled;
        bool ma = p.MaintenanceChecklistEnabled;
        bool sc = p.ServiceRequestEnabled;               

        #endregion

        HyperLink lnkConfigurationHeaderText = ((HyperLink)e.Row.FindControl("lnkConfigurationHeaderText"));
        HyperLink lnkHomes = ((HyperLink)e.Row.FindControl("lnkHomes"));
        HyperLink lnkHomes2 = ((HyperLink)e.Row.FindControl("lnkHomes2"));
        HyperLink lnkProducts = ((HyperLink)e.Row.FindControl("lnkProducts"));
        HyperLink lnkProducts2 = ((HyperLink)e.Row.FindControl("lnkProducts2"));

        HyperLink lnkMaintPart = ((HyperLink)e.Row.FindControl("lnkMaintPart"));
        HyperLink lnkMaintPart2 = ((HyperLink)e.Row.FindControl("lnkMaintPart2"));

        HyperLink lnkServiceRequest = ((HyperLink)e.Row.FindControl("lnkServiceRequest"));
        HyperLink lnkServiceRequest2 = ((HyperLink)e.Row.FindControl("lnkServiceRequest2"));
        Literal litSR = (Literal)e.Row.FindControl("litRequest");

        Panel pnlDeliveryReviews = ((Panel)e.Row.FindControl("pnlDeliveryReviews"));
        Panel pnlWR_Homes = ((Panel)e.Row.FindControl("pnlWR_Homes"));
        Panel pnlWR_Products = ((Panel)e.Row.FindControl("pnlWR_Products"));
        Panel pnlSR = ((Panel)e.Row.FindControl("pnlSR_RequestList"));

        pnlWR_Homes.Visible = false;
        pnlWR_Products.Visible = false;
        pnlSR.Visible = false;

        if (wr || ma)
        {
          //Homes          
          if (p.GetAllUnits().Count > 0)
          {
            pnlWR_Homes.Visible = true;

            lnkHomes.NavigateUrl = Builder_Homes.GetUrl(p.ProjectID);
            lnkConfigurationHeaderText.NavigateUrl = Builder_Homes.GetUrl(p.ProjectID);
            lnkHomes2.NavigateUrl = Builder_Homes.GetUrl(p.ProjectID);
          }

          if (wr)
          {
            //Products       
            if (WRObjectModel.Item.GetProductCount(p.ProjectID) > 0)
            {
              pnlWR_Products.Visible = true;

              lnkProducts.NavigateUrl = Builder_Products.GetProjectUrl(p.ProjectID);
              lnkProducts2.NavigateUrl = Builder_Products.GetProjectUrl(p.ProjectID);
            }
          }
        }        
                
        if (sc)
        {
          //Service Requests - go directly if only one or list if there are more
          //hide if not subscribed         

          List<int> defaultStatus = new List<int>
                                        {
                                            WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID,
                                            WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID
                                        };

            //get all Requests for this Project
          IQueryable<WRObjectModel.UnitServiceRequest> sr =
            (IQueryable<WRObjectModel.UnitServiceRequest>)WRObjectModel.ServiceResource.GetServiceRequestsByBuilder(
              this.UserInfo.ByID, 0, p.ProjectID, "", "", "", "", defaultStatus, "", "", null, null, "UnitServiceRequestID", SortDirection.Ascending,null,0,null);

          if (sr.Count() > 0)
          {
            pnlSR.Visible = true;
          }

          if (sr.Count() == 1)
          {
            lnkServiceRequest.NavigateUrl = Builder_ServiceRequestMain.GetUrl(sr.First().UnitServiceRequestID);
            lnkServiceRequest2.NavigateUrl = Builder_ServiceRequestMain.GetUrl(sr.First().UnitServiceRequestID);
          }
          else if (sr.Count() > 1)
          {
            lnkServiceRequest.NavigateUrl = Builder_ServiceRequestList.GetUrl(p.ProjectID);
            lnkServiceRequest2.NavigateUrl = Builder_ServiceRequestList.GetUrl(p.ProjectID);
          }
        }

        if (!pnlDeliveryReviews.Visible)
        {
          pnlDeliveryReviews.Width = 1;
        }

        if (!pnlWR_Homes.Visible)
        {
          pnlWR_Homes.Width = 1;
        }

        if (!pnlWR_Products.Visible)
        {
          pnlWR_Products.Width = 1;
        }

        if (!pnlSR.Visible)
        {
          pnlSR.Width = 1;
        }
      }
    }
  }
}
