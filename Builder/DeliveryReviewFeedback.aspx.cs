using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HomeOwner.Properties;
using WRObjectModel;
using System.Web.Security;

namespace HomeOwner.app.Builder
{
    public partial class DeliveryReviewFeedback : WRWebControls.BasePage
    {
        public static string GetUrl(int deliveryReviewID)
        {
            return "~/app/builder/deliveryreviewfeedback.aspx?prid=" + deliveryReviewID.ToString();
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

            litTextHeading.Text = "Please provide your feedback regarding this delivery";
            txtTerms.Text = "";
            txtTerms.ReadOnly = false;
        }

        protected void Commit(object sender, EventArgs e)
        {
            ProjectDeliveryReview r = GetDetails();

            if (txtTerms.Text.Length > 0)
            {
                //comments are being added
                r.ResponseComments = txtTerms.Text;

                DataContextFactory.GetWarranyDataContext().SubmitChanges();
            }

            try
            {
                //Notify the CS rep, Print rep and Operations Manager that the client has approved/denied the review
                // try {
                string csEmail = "";
                string printEmail = "";
                string uniquePhases = string.Empty;

                //get email addresses for all required folks
                foreach (ProjectDeliveryReviewUnit u in r.RelatedUnits)
                {
                    Phase h = Phase.Get(u.Unit.Phase.PhaseID);

                    //Check if the phase is already been covered in earlier iterations.
                    if(!uniquePhases.Contains(u.Unit.Phase.PhaseID.ToString()))
                    {
                        uniquePhases = uniquePhases + u.Unit.Phase.PhaseID.ToString();

                        foreach (ProjectActivity a in h.ProjectActivities)
                        {
                            if (a.UserAssignedTo != null)
                            {
                                switch (a.ProjectRoleID)
                                {
                                    case 1: case 3:
                                        //Case 1: Client Services
                                        //Case 3: Research
                                        MembershipUser csUser = Membership.GetUser(a.UserAssignedTo.UserName);
                                        if (!csEmail.Contains(csUser.Email))
                                        {
                                            csEmail = csEmail + csUser.Email + ",";
                                        }
                                        break;
                                    case 4:
                                        //Print
                                        MembershipUser prUser = Membership.GetUser(a.UserAssignedTo.UserName);
                                        if (!printEmail.Contains(prUser.Email))
                                        {
                                            printEmail = printEmail + prUser.Email + ",";
                                        }
                                        break;
                                }
                            }
                        }
                        }   
                }

                string ourEmail = "info@conasysinc.com";
                string opsManEmail = Settings.Default.OperationManagerEmail;
                string subject = r.Project.ProjectName + " Has Been Reviewed and " + r.ProjectDeliveryReviewResponseType.Name + "!";

                string htmlMessageBody = File.ReadAllText(Path.Combine(Settings.Default.EmailTemplatePath,
                                                                                   "MessageTemplates\\OnlineDeliveryReviewResponse_HTML.txt"));
                string txtMessageBody = File.ReadAllText(Path.Combine(Settings.Default.EmailTemplatePath,
                                                                                  "MessageTemplates\\OnlineDeliveryReviewResponse_PlainText.txt"));
                htmlMessageBody = ProjectDeliveryReview.GetOnlineDeliveryResponse(htmlMessageBody, r);
                txtMessageBody = ProjectDeliveryReview.GetOnlineDeliveryResponse(txtMessageBody, r);
                EmailTemplate emailTemplate = new EmailTemplate();
                List<string> to = new List<string> { csEmail, printEmail, opsManEmail };
                foreach (string recipient in to)
                {
                    if (!string.IsNullOrEmpty(recipient))
                    {
                        string htmlEmailMessageBody = emailTemplate.GetConasysHtmlMessageTemplate(htmlMessageBody,
                                                                                                  subject, r.UserMap1.UserName,
                                                                                                  recipient);
                        string txtEmailMessageBody = emailTemplate.GetConasysTxtMessageTemplate(txtMessageBody,
                                                                                                 r.UserMap1.UserName, recipient);


                        //Send an email to client services
                        MailHelper.SendMailMessage(
                            ourEmail,
                            recipient,
                            "",
                            "",
                            subject,
                            txtEmailMessageBody,
                            htmlEmailMessageBody,
                            System.Net.Mail.MailPriority.Normal);
                    }
                }

            }
            catch
            {
            }

            Response.Redirect(DeliveryConfirmed.GetUrl(r.ProjectDeliveryReviewID));
        }
    }
}
