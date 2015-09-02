using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRObjectModel;

namespace HomeOwner.app.Builder
{
  public partial class DeliveryReviewTerms : WRWebControls.BasePage
  {
    public static string GetUrl(int deliveryTermsID)
    {
      return "~/app/builder/DeliveryReviewTerms.aspx?tid=" + deliveryTermsID.ToString();
    }

    private int GetDetailID()
    {
      return int.Parse(Request.QueryString["tid"]);
    }

    private ProjectDeliveryTerm GetDetails()
    {      
      return ProjectDeliveryTerm.Get(GetDetailID());
    }

    protected void LoadDetails(object sender, EventArgs e)
    {
      ProjectDeliveryTerm t = GetDetails();
      litTerms.Text = t.Terms.Replace(System.Environment.NewLine, "<br />");
      litTermsDate.Text = t.EffectiveDate.ToShortDateString();      
    }
  }
}
