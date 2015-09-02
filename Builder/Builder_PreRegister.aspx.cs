using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HomeOwner.Properties;
using WRObjectModel;
using WRWebControls;

namespace HomeOwner.app
{
    public partial class Builder_PreRegister : WRWebControls.BasePage
    {
        public static string GetUrl(int unitID)
        {
            return "~/app/Builder/Builder_PreRegister.aspx?uid=" + unitID.ToString();
        }

        protected void CreateUserWizard1_CreatingUser(object sender, EventArgs e)
        {

        }

        //called after Membership User has been created, now add any additional profile info needed
        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {

            try
            {
                int unitID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["uid"]))
                {
                    unitID = int.Parse(Request.QueryString["uid"]);
                }

                MembershipUser user = Membership.GetUser(this.CreateUserWizard1.UserName);
                bool success = WRObjectModel.Users.User.RegisterHomeOwnerUser(user, unitID);

                //create new profile on the new user and save extra info
                if (success)
                {
                    System.Web.Profile.ProfileBase profile =
                        System.Web.Profile.ProfileBase.Create(CreateUserWizard1.UserName, true);
                    profile.SetPropertyValue("FirstName",
                                             ((TextBox)
                                              this.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl
                                                  (
                                                      "FirstName")).Text);
                    profile.SetPropertyValue("LastName",
                                             ((TextBox)
                                              this.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl
                                                  (
                                                      "LastName")).Text);
                    profile.SetPropertyValue("PhoneNumber",
                                             ((TextBox)
                                              this.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl
                                                  (
                                                      "Phone")).Text);

                    //Save profile since we created it manually
                    profile.Save();

                    WRObjectModel.Users.Membership.UpdateUserWarrantyReminder(user.UserName, true, true);

                    TextReader htmlReader =
                        new StreamReader(
                            this.OpenFile(Path.Combine(Settings.Default.EmailTemplatePath,
                                                       @"MessageTemplates\BuilderPortalPreRegister_HTML.txt")));
                    TextReader txtReader =
                        new StreamReader(
                            this.OpenFile(Path.Combine(Settings.Default.EmailTemplatePath,
                                                       @"MessageTemplates\BuilderPortalPreRegister_PlainText .txt")));
                    string htmlEmailBody;
                    string txtEmailBody;
                    try
                    {
                        htmlEmailBody = htmlReader.ReadToEnd();
                        txtEmailBody = txtReader.ReadToEnd();
                    }
                    finally
                    {
                        htmlReader.Close();
                        txtReader.Close();
                    }

                    EmailTemplate emailTemplate = new EmailTemplate();

                    //Get the UserId of the just-added user
                    Guid newUserId = (Guid)user.ProviderUserKey;

                    //get the full URL
                    string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
                    string verifyUrl = BuilderRegistration_Verification.GetUrl(newUserId, user.UserName);
                    string fullUrl = urlBase + verifyUrl;
                    const string subject = "Your Online Homeowner Portal";
                    const string from = "accountinformation@homeinformationpackages.com";
                    //Replace <%VerificationUrl%> with the appropriate URL and querystring
                    htmlEmailBody = htmlEmailBody.Replace("<%VerificationUrl%>", fullUrl);
                    txtEmailBody = txtEmailBody.Replace("<%VerificationUrl%>", fullUrl);
                    //Enter home info into email
                    if (unitID > 0)
                    {
                        WRObjectModel.Unit u = WRObjectModel.Unit.Get(unitID);
                        HomeInformationPackage hip = HomeInformationPackage.Get(unitID);
                        htmlEmailBody = htmlEmailBody.Replace("<%HomeID%>", u.Purchaser.UserName);
                        htmlEmailBody = htmlEmailBody.Replace("<%Address%>", u.GetSingleLineAddress());
                        htmlEmailBody = emailTemplate.GetMatchPortalHtmlMessageTemplate(htmlEmailBody, hip, subject,
                                                                                        user.UserName,
                                                                                        user.Email);
                        txtEmailBody = txtEmailBody.Replace("<%HomeID%>", u.Purchaser.UserName);
                        txtEmailBody = txtEmailBody.Replace("<%Address%>", u.GetSingleLineAddress());
                        txtEmailBody = emailTemplate.GetMatchPortalTxtMessageTemplate(txtEmailBody, user.UserName,
                                                                                      user.Email);
                    }
                    else
                    {
                        htmlEmailBody = htmlEmailBody.Replace("<%HomeID%>", "(not available)");
                        htmlEmailBody = htmlEmailBody.Replace("<%Address%>", "(not available)");
                        txtEmailBody = txtEmailBody.Replace("<%HomeID%>", "(not available)");
                        txtEmailBody = txtEmailBody.Replace("<%Address%>", "(not available)");
                    }
                    MailHelper.SendMailMessage(from, user.Email, "", "", subject, txtEmailBody, htmlEmailBody,
                                               MailPriority.Normal);

                }
            }
            catch
            {
                Literal msg = (Literal)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorMessage");
                msg.Text = "Error creating user. Likely cause is an invalid email.";
            }
        }

        //protected void CreateUserWizard_SendingEmail(object sender, MailMessageEventArgs e)
        //{
        //    try
        //    {
        //        EmailTemplate emailTemplate = new EmailTemplate();

        //        //Get the UserId of the just-added user
        //        MembershipUser newUser = Membership.GetUser(this.CreateUserWizard1.UserName);
        //        Guid newUserId = (Guid)newUser.ProviderUserKey;

        //        //get the full URL
        //        string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
        //        string verifyUrl = BuilderRegistration_Verification.GetUrl(newUserId, newUser.UserName);
        //        string fullUrl = urlBase + verifyUrl;
        //        const string subject = "Your Online Home Information Package";
        //        const string from = "accountinformation@homeinformationpackages.com";
        //        //Replace <%VerificationUrl%> with the appropriate URL and querystring
        //        e.Message.Body = e.Message.Body.Replace("<%VerificationUrl%>", fullUrl);

        //        //Enter home info into email
        //        if (!String.IsNullOrEmpty(Request.QueryString["uid"]))
        //        {
        //            int unitID = int.Parse(Request.QueryString["uid"]);
        //            WRObjectModel.Unit u = WRObjectModel.Unit.Get(unitID);
        //            HomeInformationPackage hip = HomeInformationPackage.Get(unitID);
        //            e.Message.Body = e.Message.Body.Replace("<%HomeID%>", u.Purchaser.UserName);
        //            e.Message.Body = e.Message.Body.Replace("<%Address%>", u.GetSingleLineAddress());
        //            e.Message.Body = emailTemplate.GetMatchPortalHtmlMessageTemplate(e.Message.Body, hip, subject, newUser.UserName,
        //                                                                  newUser.Email);
        //        }
        //        else
        //        {
        //            e.Message.Body = e.Message.Body.Replace("<%HomeID%>", "(not available)");
        //            e.Message.Body = e.Message.Body.Replace("<%Address%>", "(not available)");
        //        }
        //        MailHelper.SendMailMessage(from, newUser.Email, "", "", subject, e.Message.Body, e.Message.Body, MailPriority.Normal);

        //    }
        //    catch
        //    {
        //        e.Cancel = true;
        //        Literal msg = (Literal)this.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorMessage");
        //        msg.Text = "Error creating user. Likely cause is an invalid email.";
        //    }
        //}

        protected void CreateUserWizard1_ContinueButtonClick(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //footer links
            lnkPrivacy.NavigateUrl = HIPLinkManager.LinkManager.GetUrl("privacyUrl");
            lnkTerms.NavigateUrl = HIPLinkManager.LinkManager.GetUrl("termsUrl");
            lnkFooterLobby.NavigateUrl = HomeOwner.app.Builder.Lobby.GetUrl();

            CreateUserWizard1.ContinueDestinationPageUrl = GetUrl(int.Parse(Request.QueryString["uid"]));

            WRObjectModel.Unit u = WRObjectModel.Unit.Get(int.Parse(Request.QueryString["uid"]));
            litUnitInfo.Text = "<div class='bigtext altColor line1'>";
            litUnitInfo.Text += "<div class='bigtext'>" + u.Phase.Project.ProjectName + "</div>";
            litUnitInfo.Text += "<div class='medtext'>" + u.GetHtmlMultiLineAddress() + "</div>";
            litUnitInfo.Text += "</div>";
        }

        protected void CreateUserWizard1_CreateUserError(object sender, CreateUserErrorEventArgs e)
        {
            Literal msg = (Literal)this.CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("ErrorMessage");

            switch (e.CreateUserError)
            {
                case MembershipCreateStatus.InvalidPassword:
                    {
                        break;
                    }
                case MembershipCreateStatus.InvalidQuestion:
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    msg.Text = "The Email you entered appears to be invalid. Please try another.";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    break;
                default:
                    break;
            }
        }
    }
}
