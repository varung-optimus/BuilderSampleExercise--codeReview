<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryReviewTerms.aspx.cs" Inherits="HomeOwner.app.Builder.DeliveryReviewTerms"
     Title="Builder Portal - Delivery Review Terms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" onprerender="LoadDetails">
        <div id="contentFrame" style="width: 1004px; position: absolute; left: 50%; margin-left: -502px;">            
            <div id="reviewContent" style="width: 1000px;">                
                <div id="headerFrame" style="width: 1000px; height: 170px";>
                    <div id="headerImageFrame" style="width: 170px; height 170px; float: left;">
                        <img src="../../images/HIP_Logo.jpg" width="160px" height="160px" align="middle" />
                    </div>
                    <div id="headerTextFrame" style="width: 800px; height: 170px; float: right;">
                        <h1>Terms and Conditions as of <asp:Literal ID="litTermsDate" runat="server"></asp:Literal></h1>                        
                    </div>                                                                
                </div>
                <div id="commentsFrame" style="width: 1000px;">
                    <asp:Literal ID="litTerms" runat="server" />
                </div>
            </div>            
        </div>    
    </form>
</body>
</html>
