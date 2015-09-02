using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using System.Web.Security;
using WRObjectModel;


namespace HomeOwner.app
{
    public partial class Builder_UnitDetails : HomeOwner.code.HipBaseClass
    {
        public int _unitID
        {
            get
            {
                string s = Request.QueryString["uid"] ?? "0";
                return Convert.ToInt32(s);
            }
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
                //footer links
                lnkPrivacy.NavigateUrl = HIPLinkManager.LinkManager.GetUrl("privacyUrl");
                lnkTerms.NavigateUrl = HIPLinkManager.LinkManager.GetUrl("termsUrl");
                lnkFooterLobby.NavigateUrl = Builder.Lobby.GetUrl();
                LoadForm();
                LoadContactGrid();
                LoadDocumentGrid();
            }
        }

        public static string GetUrl()
        {
            return GetBaseURL();
        }

        public static string GetUrl(int unitID)
        {
            return GetBaseURL() + "?uid=" + unitID.ToString();
        }

        private static string GetBaseURL()
        {
            return "/app/Builder/Builder_UnitDetails.aspx";
        }

        protected void LoadForm()
        {
            if (_unitID != 0)
            {
                WRObjectModel.Unit u = WRObjectModel.Unit.Get(_unitID);

                if (u != null)
                {
                    lnkPreRegister1.NavigateUrl = Builder_PreRegister.GetUrl(_unitID);
                    lnkPreRegister2.NavigateUrl = Builder_PreRegister.GetUrl(_unitID);

                    litHomeID.Text = "Home ID not found";
                    if (u.Purchaser != null)
                    {
                        if (!String.IsNullOrEmpty(u.Purchaser.UserName))
                        {
                            litHomeID.Text = u.Purchaser.UserName;
                        }
                    }
                    litUnitAddress.Text = Utilities.GetHtmlMultiLineAddress(u);

                    litUnitLegalDescription.Text = "";
                    if (!String.IsNullOrEmpty(u.LegalDescription))
                    {
                        litUnitLegalDescription.Text = u.LegalDescription;
                    }

                    litUnitHomeWarrantyProvider.Text = "";
                    if (u.Phase.Project.WarrantyProvider != null)
                    {
                        if (!String.IsNullOrEmpty(u.Phase.Project.WarrantyProvider.CompanyLegalName))
                        {
                            litUnitHomeWarrantyProvider.Text = u.Phase.Project.WarrantyProvider.CompanyLegalName;
                        }
                    }

                    this.txtPosssessionDate.Text = "";
                    if (!String.IsNullOrEmpty(u.PossessionDate.ToString()))
                    {
                        this.txtPosssessionDate.Text = String.Format("{0:dd-MMM-yyyy}", u.PossessionDate);
                    }

                    try
                    {
                        WRObjectModel.Document banner = u.Phase.Project.GetBanner();
                        imgProjectBanner.Height = System.Web.UI.WebControls.Unit.Pixel(110);
                        imgProjectBanner.ImageUrl = banner.GetUrl();
                        if (!imgProjectBanner.ImageUrl.ToLower().EndsWith(".jpg"))
                        {
                            throw new Exception("Banner must be a JPEG (.jpg)");
                        }
                    }
                    catch
                    {
                        imgProjectBanner.CssClass = "banner";
                    }

                    this.lnkViewHome.NavigateUrl = Welcome.GetUrl(_unitID);
                    this.lnkProducts.NavigateUrl = Builder_Products.GetUrl(_unitID);

                    //get all Request for this Unit...need counts
                    //get StatusIds for 'under-review' and 'In progress'
                    int[] defaultStatusIDs = { WRObjectModel.ServiceRequestStatus.Get("review").ServiceRequestStatusID,
                                     WRObjectModel.ServiceRequestStatus.Get("progress").ServiceRequestStatusID };
                    List<WRObjectModel.UnitServiceRequest> usrs =
                      WRObjectModel.ServiceResource.GetServiceRequestsByUnit(_unitID, defaultStatusIDs, "SubmittedOn", SortDirection.Ascending).ToList();

                    if (usrs.Count() > 0)
                    {
                        this.lnkRequests.NavigateUrl = Builder_ServiceRequestList.GetUrl(u.Phase.ProjectID, u.UnitID);
                        this.lnkRequests.Text = "View Requests for this Home (" + usrs.Count().ToString() + ")";
                    }
                    else
                    {
                        this.lnkRequests.Visible = false;
                        this.spanlnkRequests.Visible = false;
                    }
                }
            }
            else
            {
                //unitID missing or invalid! Oh oh...should display error msg here.
            }
        }

        void LoadContactGrid()
        {
            this.gridContacts.DataSource = BuilderPortal.DataSources.GetUnitContacts(_unitID);
            this.gridContacts.DataBind();
        }

        void LoadDocumentGrid()
        {
            BasePage bp = new BasePage();
            int userId = bp.UserInfo.ByID;
            List<BuilderPropertyDocuments> lstBuilderPropertyDocuments = (BuilderPropertyDocuments.GetBuilderPortalDocuments(_unitID, userId)).ToList();
            gridDocuments.DataSource = lstBuilderPropertyDocuments;
            gridDocuments.DataBind();
        }

        protected void gridContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //gather and display User Contact info      
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                WRObjectModel.UnitContact uc = (WRObjectModel.UnitContact)e.Row.DataItem;

                MembershipUser user = Membership.GetUser(uc.ByIDUserID.UserName);
                if (user != null)
                {
                    Literal litEmail = (Literal)e.Row.FindControl("litUserEmail");
                    litEmail.Text = "<a style='background: none;' href='mailto:" + user.Email + "'>" + user.Email + "</a>";

                    System.Web.Profile.ProfileBase profile = WRObjectModel.Users.User.GetUserProfile(user.UserName);

                    Literal litName = (Literal)e.Row.FindControl("litUserName");
                    litName.Text = profile.GetPropertyValue("FirstName").ToString() + " " + profile.GetPropertyValue("LastName").ToString();
                    Literal litPhone = (Literal)e.Row.FindControl("litUserPhone");
                    litPhone.Text = profile.GetPropertyValue("PhoneNumber").ToString();

                    //Registartion Status from IsApproved...false means User hasnt clicked on confirmation link in email to change password
                    Literal litStatus = (Literal)e.Row.FindControl("litRegStatus");
                    litStatus.Text = user.IsApproved ? "Confirmed!" : "Pending Confirmation";

                    HyperLink lnkResetPassword = (HyperLink)e.Row.FindControl("lnkResetPassword");
                    lnkResetPassword.NavigateUrl = PasswordUsernameRecovery.GetUrl(user.Email);
                }
            }
        }
        protected void btnUpdatePossessionDate_Click(object sender, EventArgs e)
        {
            if (_unitID != 0)
            {
                WRObjectModel.Unit.UpdatePossessionDate(Convert.ToInt32(_unitID), this.txtPosssessionDate.Text);
            }
        }
        protected void gridDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WRObjectModel.BuilderPropertyDocuments bpd = (WRObjectModel.BuilderPropertyDocuments)e.Row.DataItem;
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            if (lnkDelete != null)
            {
                if (bpd.DocType == 0)
                {
                    lnkDelete.Visible = false;
                }
            }
        }

        protected void btDelete_Command(Object sender, GridViewCommandEventArgs e)
        {
            //var choice = hdnChoice.Value;//Getting the value of the choice selected in the confirmation box for document deletion.
            //if (choice == "true")
            //{
            int documentId, documentType;
                // If multiple buttons are used in a GridView control, use the
                // CommandName property to determine which button was clicked.
                if (e.CommandName == "DeleteDocument")
                {
                    string[] arg = new string[2];
                    arg = e.CommandArgument.ToString().Split(';');
                    int.TryParse(arg[0], out documentId);
                    int.TryParse(arg[1], out documentType);
                    BuilderPropertyDocuments.DeleteDocument(documentId, documentType);
                }
                LoadDocumentGrid();
            //}
        }
    }
}
