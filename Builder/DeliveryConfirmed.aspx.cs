using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRObjectModel;

namespace HomeOwner.app.Builder
{
  public partial class DeliveryConfirmed : WRWebControls.BasePage
  {
    public static string GetUrl(int deliveryReviewID)
    {
      return "~/app/builder/deliveryconfirmed.aspx?prid=" + deliveryReviewID.ToString();
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
        catch (Exception ex)
        {
          throw new Exception("Project Delivery Review data not found", ex);
        }
      }
      catch (Exception ex2)
      {
        throw ex2;
      }

      return p;
    }

    protected void LoadReviewConfirmationDetails(object sender, EventArgs e)
    { 
      ProjectDeliveryReview dr = GetDetails();

      if (dr.IssuedToUserID != UserInfo.ByID)
      {
        throw new Exception("Attempting to access an invalid delivery review");
      }

      //populate the header
      litSalesOrder.Text = dr.Project.Order.OrderNumber;
      litProjectDetails.Text = dr.Project.ProjectName; //Project name + Multi-line project address              
    }    
  }
}
