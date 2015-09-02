using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WRWebControls;
using System.Web.Security;
using HomeOwner.Code;
using WRObjectModel;

namespace HomeOwner.app 
{    
  public partial class Builder_ProductDetails : HomeOwner.code.HipBaseClass     
  {
    public int _itemID
    {
      get
      {
        string s = Request.QueryString["itemID"] ?? "0";
        return Convert.ToInt32(s);
      }
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
      }
    }
    
    public static string GetUrl(int itemID)        
    {
      return GetBaseURL() + "?itemID=" + itemID.ToString();
    }
    
    private static string GetBaseURL() 
    {
      return "/app/Builder/Builder_ProductDetails.aspx";
    }
    
    protected void LoadForm()
    {      
      WRObjectModel.Item i = WRObjectModel.Item.Get(_itemID);
      
      this.lblComponentType.Text = i.ComponentType != null ? i.ComponentType.FullName : "<unknown type>";
      this.lblComponent.Text = i.ItemName;
      this.litManufacturer.Text = i.Manufacturer != null ? CompanyDetailFormatter.GetCompanyDetailsHtml(i.Manufacturer,true) : "<unknown Manufacturer>";
      this.lnkViewHomes.NavigateUrl = Builder_Homes.GetItemUrl(i.ItemID);

      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      sb.Append("<div class='bigtext bold line1'>");
      sb.Append(!String.IsNullOrEmpty(i.Description) ? "<div><label class='light dark'>Description: </label>" + i.Description + "</div>" : "");
      sb.Append(!String.IsNullOrEmpty(i.Color) ? "<div class='topSpc'><label class='light dark'>Colour: </label>" + i.Color + "</div>" : "");      
      sb.Append(!String.IsNullOrEmpty(i.WarrantyDescription) ? "<div class='topSpc'><label class='light dark'>Warranty: </label>" + i.WarrantyDescription + "</div>" : "");
      sb.Append("</div><br>");
      this.litOther.Text = sb.ToString();

      if(i.Documents.Count > 0) 
      {
        foreach(var document in i.Documents) 
        {
          AddDocToLinksList(document, this.litDocuments, UserInfo.UserName);
        }
      }

      //Suppliers and Installers                           
      IEnumerable<Project> projects = BuilderPortal.DataSources.GetProjectList(this.UserInfo.ByID);
      List<int> projectList = BuilderPortal.DataSources.GetProjectList(this.UserInfo.ByID).Select(p => p.ProjectID).ToList<int>();
      
      this.gridSuppliers.DataSource = (IQueryable<Company>)WRObjectModel.Company.GetInstallerSupplierCompanies(i.ItemID, projectList, true, false);
      this.gridSuppliers.DataBind();

      this.gridInstallers.DataSource = (IQueryable<Company>)WRObjectModel.Company.GetInstallerSupplierCompanies(i.ItemID, projectList,false, true);
      this.gridInstallers.DataBind();        
    }

    protected void AddDocToLinksList(Document doc, LinksList ll, string UserName)
    {
      HyperLink hl;
      try 
      {
        hl = Utils.newTargetBlankHyperlink(Document.GetUrl(doc.DocumentID), DocumentFormatter.GetDocumentContentsString(doc));            
      }
      catch(Exception e) 
      {
        ErrorLogger.LogException(e);

        string url = "#";
        hl = Utils.newTargetBlankHyperlink(url, DocumentFormatter.GetDocumentContentsString(doc));
      }
      
      ll.Controls.Add(hl);
    }
    
    protected void grids_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if(e.Row.RowType == DataControlRowType.DataRow) 
      {
        WRObjectModel.Company c = (WRObjectModel.Company)e.Row.DataItem;
        if(c != null) 
        {
          ((Literal)e.Row.FindControl("litGridRowAddress")).Text = CompanyDetailFormatter.GetCompanyDetailsHtml(c, false); 
          HyperLink lnkContact = ((HyperLink)e.Row.FindControl("lnkContact"));
          if(c.CompanyLegalName != null) 
          {
            if(((GridView)sender).ID == "gridSuppliers") 
            {
              lnkContact.NavigateUrl = HomeOwner.app.Builder.Contacts.GetUrl(c.CompanyLegalName, 11);
            }
            else 
            {
              lnkContact.NavigateUrl = HomeOwner.app.Builder.Contacts.GetUrl(c.CompanyLegalName, 6);
            }
          }
        }
      }
    }
  }
}
