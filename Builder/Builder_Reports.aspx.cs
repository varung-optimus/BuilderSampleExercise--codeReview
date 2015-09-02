using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HomeOwner.app.Reports;
using WRObjectModel;

namespace HomeOwner.app 
{
    
  public partial class Builder_Reports : HIP_MasterBasePage  
  {
        public static string GetUrl()
        {
            return "/app/Builder/Builder_Reports.aspx";
        }

        protected override string GetSelectedResource()
        {
            return "Service";
        }
        protected override string GetSelectedResourceLink()
        {
            return GetUrl();
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
            BuilderPortalAccount builderPortalAccount = BuilderPortalAccount.GetByUserID(UserInfo.ByID);
            if (builderPortalAccount.IsHomeownerUsage)
            {
                linkHomeownerUsage.Visible = true;
                int companyId = builderPortalAccount.CompanyBuilderPortalAccounts.FirstOrDefault().CompanyID;
                linkHomeownerUsage.NavigateUrl = GenerateReport.GetHomeownerUsageReportUrl(companyId);
            }
            bool serviceResource = false;
            foreach (BuilderPortalAccountResource par in builderPortalAccount.GetResources())
            {
                if (par.Resource.Name == "Service")
                {
                    serviceResource=true;
                }
            }
            linkSummary.Visible = linkWorkOrderSummary.Visible = serviceResource;
            this.linkSummary.NavigateUrl = Report_RequestSummary.GetUrl(); 
          this.linkDetails.NavigateUrl = Report_RequestDetails.GetUrl(); 
          this.linkTradePerformance.NavigateUrl = Report_TradePerformance.GetUrl(); 
          this.linkProductIssues.NavigateUrl = Report_ProductIssues.GetUrl();

            linkWorkOrderSummary.NavigateUrl = Report_WorkOrderSummary.GetUrl();
        }
    }
}
