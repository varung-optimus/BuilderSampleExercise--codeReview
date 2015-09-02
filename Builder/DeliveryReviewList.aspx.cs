using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRObjectModel;

namespace HomeOwner.app.Builder
{
  public partial class DeliveryReviewList : HIP_MasterBasePage  
  {
    public static string GetUrl()
    {
      return "/app/Builder/DeliveryReviewList.aspx";
    }
    public static string GetUrl(int projectID)
    {
        return "/app/Builder/DeliveryReviewList.aspx" + "?projectID=" + projectID.ToString();
    }

    public int _projectID
    {
        get
        {
            string s = Request.QueryString["projectID"] ?? "0";
            return Convert.ToInt32(s);
        }
    }

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      SetGridDataSource();
    }

    private void SetGridDataSource()
    {

      gvPendingReviews.DataSource = ProjectDeliveryReview.GetPendingDeliveryReviews(this.UserInfo.ByID, _projectID);
      gvPendingReviews.DataBind();
    }

    protected void SetClientOnClicks(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        ProjectDeliveryReview dr = (ProjectDeliveryReview)e.Row.DataItem;

        ((Literal)e.Row.FindControl("litReviewIssuedTo")).Text = dr.IssuedToName + "<br /><a href='mailto:" + dr.IssuedToEmail + "'>" + dr.IssuedToEmail + "</a>";
        ((HyperLink)e.Row.FindControl("lnkView")).NavigateUrl = DeliveryReview.GetUrl(dr.ProjectDeliveryReviewID);

        e.Row.Attributes.Add("onmouseover", "this.className='gridrowover'");
        e.Row.Attributes.Add("onmouseout", "this.className=''");        
      }
    }
  }
}
