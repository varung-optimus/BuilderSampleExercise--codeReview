<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Builder_PreRegister.aspx.cs" 
    Inherits="HomeOwner.app.Builder_PreRegister"  Theme="BuilderDefault" Title="Builder Portal - Online Registration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Builder Pre-Register Online</title>  
</head>

<body>  

    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
    <div id="wrap">
     <div id="wrap_border">    
     
        <div id="divHipLogo" align="left" class="left">
            <asp:HyperLink ID="lnkLobby" runat="server">
                <asp:Image ID="imgHipLogo" runat="server" SkinID="hipLogoNew" />
            </asp:HyperLink>
        </div>  
        
       <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="250">        
            <ProgressTemplate>
                <div id="divProgressPopup" class="progressIndicator">                           
               <asp:Image ID="Image1" runat="server" SkinID="wait" AlternateText="" /> Please Wait...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <div class="topSpc pad left" style="padding-left: 25px; padding-top: 20px;">
            <asp:Literal ID="litUnitInfo" runat="server" />
            <br />
            
        </div> 
            
           
        <div align="center" class="left" style="padding-left: 25px; padding-top:35px; padding-bottom: 20px;">
          <asp:HyperLink ID="lnkBackToProperties" style="text-decoration:none;margin-left: 500px!important" runat="server" NavigateUrl="~/app/Builder/Builder_Homes.aspx">
           Return to Properties
           </asp:HyperLink>
            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" MembershipProvider="WRProvider"               
                Font-Size="1.1em" 
                LoginCreatedUser="False"  
                ContinueDestinationPageUrl="~/Login.aspx" 
                ContinueButtonText="Add Another User"                                        
                UnknownErrorMessage="The registration could not be completed. Please try again."
                CompleteSuccessText="<br><b>Pre-Registration Complete!</b> <br><br> The User you have Pre-Registered has been sent a confirmation email. This email contains a link that will take the User to a website that will allow them to complete their registration.<br><br><b>NOTE: <b> The account will not be ready for use until they complete this step.<br><br>"
                OnCreatedUser="CreateUserWizard1_CreatedUser" 
                OnCreatingUser="CreateUserWizard1_CreatingUser"
                DisableCreatedUser="True"
                AutoGeneratePassword="true"
                Width="655px"   
                DuplicateUserNameErrorMessage="That UserName is already in use. Choose another."
                CssClass="box border" 
                oncreateusererror="CreateUserWizard1_CreateUserError">                
                <ContinueButtonStyle CssClass="regbutton" />
                <NavigationButtonStyle CssClass="regbutton" />                                         
                <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" Font-Size="14" HorizontalAlign="Left" />
                <TextBoxStyle Width="210px" />
                <TitleTextStyle BackColor="white" Font-Bold="True"  HorizontalAlign="Left" CssClass="bigtext" Height="26" />
                <CreateUserButtonStyle CssClass="regbutton" />
                <LabelStyle Font-Size="1.1em" />
                <WizardSteps>
                    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" align="left" width="100%">
                                <tr>
                                    <td colspan="2" valign="top" align="left">
                                        <h6>Pre-Register a Online Account for this Unit</h6>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" height="8px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="200px">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="UserName" runat="server" Width="150px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">
                                        *</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                           
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="FirstName" runat="server" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ToolTip="First Name is required."
                                            ErrorMessage="First Name is required." ValidationGroup="CreateUserWizard1" ControlToValidate="FirstName">
                                        *</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="FnameRegExpr" runat="server" ErrorMessage="Invalid Name"
                                            ControlToValidate="FirstName" ValidationExpression="^[a-zA-Z''-'\s]{1,20}$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="LastName" runat="server" Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ToolTip="Last Name is required."
                                            ErrorMessage="Last Name is required." ValidationGroup="CreateUserWizard1" ControlToValidate="LastName">
                                        *</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="LnameRegExpr" runat="server" ErrorMessage="Invalid Name"
                                            ControlToValidate="LastName" ValidationExpression="^[a-zA-Z''-'\s]{1,35}$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">Email:</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Email" runat="server" Width="250px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ToolTip="Email is required."
                                            ErrorMessage="Email is required." ValidationGroup="CreateUserWizard1" ControlToValidate="Email">
                                        *</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="EmailRegExpr" runat="server" ErrorMessage="Invalid Email"
                                            ControlToValidate="Email" ValidationExpression="^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:Label ID="phonelabel" runat="server" AssociatedControlID="Phone">Phone Number:</asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="Phone" runat="server" Width="150px"></asp:TextBox>
                                        <asp:Label ID="lblPhoneExample" runat="server" Text=" (eg: 999-999-9999) " AssociatedControlID="Phone"></asp:Label>
                                        <asp:RegularExpressionValidator ID="PhoneRegExpr" runat="server" ErrorMessage="Invalid Phone Number"
                                            ControlToValidate="Phone" ValidationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>  
                                <tr>
                                    <td style="color: red" align="center" colspan="2" class="important">
                                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>                              
                            </table>
                            <br />
                        </ContentTemplate>
                    </asp:CreateUserWizardStep>
                    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server" Title="Account Created - Confirmation Email Sent">
                    </asp:CompleteWizardStep>
                    
                </WizardSteps>
                
                <%--<MailDefinition Subject="Your Online Home Information Package" 
                  From="accountinformation@homeinformationpackages.com" IsBodyHtml="true" BodyFileName="BuilderPreRegister.txt">
                
                </MailDefinition>--%>
            </asp:CreateUserWizard>
           
        </div>
        
        <div id="footer">        
          <asp:HyperLink ID="lnkFooterLobby" runat="server" Text="Lobby" /> 
            &nbsp;|
        <asp:HyperLink ID="lnkTerms" runat="server" Text="Terms of Use" /> 
            &nbsp;|
        <asp:HyperLink ID="lnkPrivacy" runat="server" Text="Privacy Policy"  />
             
        <div style="padding-top: 5px;" class="alt">
            Copyright &copy;2013 <a href="http://www.conasysinc.com">CONASYS Inc.</a>    
        </div>
    </div>            

        <div class="wr-clearHack">.</div> 
        
        
    </div>
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>      
</body>
</html>
