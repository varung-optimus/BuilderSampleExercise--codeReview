using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using WRObjectModel;

namespace HomeOwner.app.Builder
{
  public partial class DeliveryReview : WRWebControls.BasePage
  {
    public static string GetUrl(int projectDeliveryReviewId)
    {
      return "/app/Builder/DeliveryReview.aspx?prid=" + projectDeliveryReviewId.ToString();
    }

    private ProjectDeliveryReview GetDetails()
    {
      int id = -1;
      ProjectDeliveryReview p = null;

      try
      {
        if (String.IsNullOrEmpty(Request.QueryString["prID"]))
        {
          throw new Exception("Project Delivery Review could not be loaded - no project delivery review specified");
        }
        else
        {
          id = Convert.ToInt32(Request.QueryString["prID"]);
        }

        try
        {
          p = ProjectDeliveryReview.Get(id);
        }
        catch(Exception ex)
        {
          throw new Exception("Project Delivery Review data not found", ex);
        }
      }
      catch(Exception ex2)
      {
        throw ex2;
      }

      return p;
    }

    private string GetFormattedUnitList(ProjectDeliveryReviewComponent component)
    {
      string result = "";

      System.Text.StringBuilder condos = new System.Text.StringBuilder();      
      System.Text.StringBuilder homes = new System.Text.StringBuilder();

      foreach (ProjectDeliveryReviewComponentUnit rcu in component.ProjectDeliveryReviewComponentUnits)
      {
        if (!String.IsNullOrEmpty(rcu.ProjectDeliveryReviewUnit.Unit.UnitDescription))
        {
          condos.Append(rcu.ProjectDeliveryReviewUnit.Unit.UnitDescription + ", ");
        }
        else
        {
          homes.Append(rcu.ProjectDeliveryReviewUnit.Unit.GetShortAddress() + ", ");
        }        
      }

      if (condos.Length > 0)
      {
        condos.Remove(condos.Length - 1, 1);
        condos.Insert(0, "<p><strong>Units: </strong>");
        condos.Append("</p>");

        result += condos.ToString();
      }

      if (homes.Length > 0)
      {
        homes.Remove(homes.Length - 1, 1);
        homes.Insert(0, "<p><strong>Homes/Townhomes: </strong>");
        homes.Append("</p>");

        result += homes.ToString();
      }

      return result;
    }

    private string GetFormattedCompanyLineItemDetails(Company c)
    {
      string result = "";

      if (c != null)
      {
        result = c.CompanyLegalName + "<br />" +
          "Phone: " + c.MainBusinessPhone;
      }

      return result;
    }

    protected void LoadReviewDetails(object sender, EventArgs e)
    { 
      ProjectDeliveryReview dr = GetDetails();

      if (dr.IssuedToUserID != UserInfo.ByID)
      {
        throw new Exception("Attempting to access an invalid delivery review");
      }

      if (dr.ResponseTypeID.HasValue)
      {
        throw new Exception("Delivery Review is already complete");
      }

      //populate the header
      litSalesOrder.Text = dr.Project.Order.OrderNumber;
      litProjectDetails.Text = dr.Project.ProjectName; //Project name + Multi-line project address

      //lnkPrintView.NavigateUrl = DeliveryReviewPrintView.GetUrl(dr.ProjectDeliveryReviewID);

      txtTerms1.Text = dr.ProjectDeliveryTerm.Terms;
      lnkTerms.NavigateUrl = HomeOwner.app.Builder.DeliveryReviewTerms.GetUrl(dr.DeliveryTermsID);
      lnkTerms2.NavigateUrl = HomeOwner.app.Builder.DeliveryReviewTerms.GetUrl(dr.DeliveryTermsID);
      lnkTerms3.NavigateUrl = HomeOwner.app.Builder.DeliveryReviewTerms.GetUrl(dr.DeliveryTermsID);

      //Gather the list of components
      grdProductIndex.DataSource = dr.RelatedComponents.OrderBy(drc => drc.Component.ComponentType.FullName).ThenBy(drc => drc.Component.Origin);
      grdProductIndex.DataBind();        
    }

    protected void SetProductIndexRowDetails(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        ProjectDeliveryReviewComponent di = (ProjectDeliveryReviewComponent)e.Row.DataItem;
        Component dataItem = di.Component;

        ((Literal)e.Row.FindControl("litSchemeDetails")).Text = dataItem.Origin;
        ((Literal)e.Row.FindControl("litComponentTypeDetails")).Text = dataItem.ComponentType.FullName;

        ((Literal)e.Row.FindControl("litManufacturerWarranty")).Text = dataItem.ManufacturerWarranty;
        //((Literal)e.Row.FindControl("litInstallerWarranty")).Text = dataItem.InstallerWarranty;
        //((Literal)e.Row.FindControl("litSupplierWarranty")).Text = dataItem.SupplierWarranty;

        ((Literal)e.Row.FindControl("litItemDetails")).Text = dataItem.ComponentDescription;

        if (dataItem.Item != null)
        {          
          ((Literal)e.Row.FindControl("litManufacturerInfo")).Text = GetFormattedCompanyLineItemDetails(dataItem.Item.Manufacturer);

          if (dataItem.Item.WarrantyDescription.Length > 0)
          {
            ((Literal)e.Row.FindControl("litManufacturerWarranty")).Text = dataItem.Item.WarrantyDescription + " " + dataItem.ManufacturerWarranty;
          }
        }
        else
        {          
          //((Literal)e.Row.FindControl("litModelDetails")).Text = "";
          ((Literal)e.Row.FindControl("litManufacturerInfo")).Text = GetFormattedCompanyLineItemDetails(dataItem.Manufacturer);
        }

        ((Literal)e.Row.FindControl("litSupplierInfo")).Text = GetFormattedCompanyLineItemDetails(dataItem.Supplier);
        ((Literal)e.Row.FindControl("litInstallerInfo")).Text = GetFormattedCompanyLineItemDetails(dataItem.Installer);
        
        ((Literal)e.Row.FindControl("litComponentLocations")).Text = dataItem.LocationDescription;
          if(string.IsNullOrEmpty(dataItem.LocationDescription))
          {
              ((Literal)e.Row.FindControl("litComponentLocations")).Text = dataItem.ComponentLocations;
          }

          ((Literal)e.Row.FindControl("litApplicableUnits")).Text = GetFormattedUnitList(di);
        ((Literal)e.Row.FindControl("litComments")).Text = dataItem.Comments;
      }
    }

    protected void Accept(object sender, EventArgs e)
    {     
      ProjectDeliveryReview dr = GetDetails();

      WRObjectModel.ProjectDeliveryReview.ApproveDeliveryReview(dr.ProjectDeliveryReviewID);
            
      Response.Redirect(DeliveryReviewFeedback.GetUrl(dr.ProjectDeliveryReviewID));      
    }
    
    protected void Reject(object sender, EventArgs e)
    {      
     
      ProjectDeliveryReview dr = GetDetails();

      WRObjectModel.ProjectDeliveryReview.RejectDeliveryReview(dr.ProjectDeliveryReviewID);
        
      Response.Redirect(DeliveryReviewFeedback.GetUrl(dr.ProjectDeliveryReviewID));      
    }


    protected void btnPrintReport_Click(object sender, ImageClickEventArgs e)
    {
        //send current Delivery Review to ReportGenerator
        if(!String.IsNullOrEmpty(Request.QueryString["prID"])) {
            Response.Redirect(app.Reports.GenerateReport.GetDeliveryReviewReportUrl(Request.QueryString["prID"]));
        }  

    }


  }
}
