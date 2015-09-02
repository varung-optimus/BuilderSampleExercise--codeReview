<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryReviewFeedback.aspx.cs" Inherits="HomeOwner.app.Builder.DeliveryReviewFeedback"
 Title="Builder Portal - Delivery Review Feedback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Builder Portal - Delivery Review Feedback</title>
</head>
<body>
    <form id="form1" runat="server" onprerender="LoadReviewConfirmationDetails">
        <div id="contentFrame" style="width: 1004px; position: absolute; left: 50%; margin-left: -502px;">            
            <div id="reviewContent" style="width: 1000px;">   
                <div id="headerFrame" style="width: 1000px; height: 170px";>
                    <div id="headerImageFrame" style="width: 170px; height 170px; float: left;">
                        <img src="../../images/HIP_Logo.jpg" width="160px" height="160px" align="middle" />
                    </div>
                    <div id="headerTextFrame" style="width: 800px; height: 170px; float: right;">
                        <h1>Delivery Review for:</h1>
                        <p>
                            <strong>Sales Order: </strong><asp:Literal ID="litSalesOrder" runat="server" /><br />
                            <strong>Project: </strong><asp:Literal ID="litProjectDetails" runat="server" />
                        </p>
                        <p>
                            <strong>Optional</strong>: Tell us about the delivery review.
                        </p>                                                                      
                    </div>                                                                
                </div>                         
                <div id="commentsFrame" style="width: 1000px;">
                    <h3><asp:Literal ID="litTextHeading" runat="server"></asp:Literal></h3>
                    <asp:TextBox ID="txtTerms" runat="server" Width="1000px" Height="400px" TextMode="MultiLine" Wrap="true">                            
                    </asp:TextBox>                                                            
                </div>
                <div id="stepsFrame" style="width: 1000px; text-align: right;">
                    <br />
                    <asp:Button ID="lnkAccept" runat="server" OnClick="Commit" Text="Next" Width="60px"></asp:Button>
                </div>
            </div>            
        </div>    
    </form>
</body>
</html>
