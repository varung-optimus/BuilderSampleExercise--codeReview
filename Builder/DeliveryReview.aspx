<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryReview.aspx.cs" Inherits="HomeOwner.app.Builder.DeliveryReview"
    Title="Builder Portal - Delivery Review" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <style type="text/css">
    table {
	    border-width: 1px;
	    border-spacing: ;
	    border-style: solid;
	    border-color: black;
	    border-collapse: collapse;
	    background-color: white;
    }
    table th {
	    border-width: 1px;
	    padding: 1px;
	    border-style: inset;
	    border-color: gray;
	    background-color: white;
	    -moz-border-radius: ;
    }
    table td {
	    border-width: 1px;
	    padding: 1px;
	    border-style: inset;
	    border-color: gray;
	    background-color: white;
	    -moz-border-radius: ;
    }
    </style>
</head>
<body style="font: 85% Calibri,Tahoma,Arial,sans-serif; line-height: 1em; height: 100%; margin: 0 auto 1px 0; padding: 0;">
    <form id="form1" runat="server" onprerender="LoadReviewDetails">                
        <div id="contentFrame" style="width: 1004px; position: absolute; left: 50%; margin-left: -502px;">            
            <div id="reviewContent" style="width: 1000px;">     
                <div id="headerFrame" style="width: 1000px; height: 170px";>
                    <div id="headerImageFrame" style="width: 170px; height: 170px; float: left;">
                        <img src="../../images/HIP_Logo.jpg" width="160px" height="160px" align="middle" />
                    </div>
                    <div id="headerTextFrame" style="width: 800px; height: 170px; float: right;">
                        <h1>Delivery Review for:</h1>
                        <p>
                            <strong>Sales Order: </strong><asp:Literal ID="litSalesOrder" runat="server" /><br />
                            <strong>Project: </strong><asp:Literal ID="litProjectDetails" runat="server" />
                        </p>
                        <p>
                            Please confirm that the information presented below is complete, accurate, and ready for publication.                             
                        </p>   
                        <div align="right">
                            <asp:ImageButton ID="btnPrintReport" runat="server" onclick="btnPrintReport_Click" ImageUrl="../../App_Themes/globalImages/print.png" />
                        </div>
                                                                                        
                    </div>                                                                
                </div>                  
                <div id="detailsFrame" style="width: 1000px;">                    
                    <div id="detailsColumnHeaders" style="width: 980px;">                    
                        <div id="Col1" style="width: 100px; float: left;"><strong>Product Group</strong></div>
                        <div id="Col2" style="width: 755px; float: left;"><strong>Product Details</strong></div>
                        <div id="Col3" style="width: 105px; float: right;"><strong>Applicable Unit(s)</strong></div>                    
                    </div>
                    <br />
                    <div id="reviewContentGrid" style="width: 1000px; height: 450px; overflow-y: scroll; overflow-x: auto;">
                        <asp:GridView ID="grdProductIndex" runat="server" 
                            AutoGenerateColumns="false" OnRowDataBound="SetProductIndexRowDetails" Width="980px" ShowHeader="false" >
                            <Columns>
                                <asp:TemplateField HeaderText="Scheme/Group" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Literal ID="litSchemeDetails" runat="server" />                                
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Information" ItemStyle-Width="760px">
                                    <ItemTemplate>                                        
                                        <div id="productInfoFrame" style="width: 760px; float: left;">                                            
                                            <div id="productType" style="width: 760px; float: left;">
                                                <strong>Product Type:</strong> <asp:Literal ID="litComponentTypeDetails" runat="server" />
                                            </div>
                                            <div id="productInfoFrame" style="width: 760px; float: left;">
                                                <div id="itemDetails" style="width: 760px; float: left;">
                                                    <strong>Model:</strong> <asp:Literal ID="litItemDetails" runat="server" />
                                                </div>
                                            </div>                                                                                    
                                        </div>                                                
                                        <div id="contactFrame" style="width: 760px; float: left;">
                                            <br />
                                            <div id="manufacturerFrame" style="width: 253px; float: left;">
                                                <strong>Manufacturer</strong><br />
                                                <asp:Literal ID="litManufacturerInfo" runat="server" />
                                            </div>
                                            <div id="supplierFrame" style="width: 253px; float: left;">
                                                <strong>Supplier</strong><br />
                                                <asp:Literal ID="litSupplierInfo" runat="server" />
                                            </div>
                                            <div id="installerFrame" style="width: 253px; float: left;">
                                                <strong>Installer</strong><br />
                                                <asp:Literal ID="litInstallerInfo" runat="server" />
                                            </div>
                                            <div id="docFrame" style="width: 1px; float: right;">                                                
                                            </div>                                            
                                        </div>                                
                                        <div id="productFooterFrame" style="width: 760px; float: left;">
                                            <br />
                                            <div id="warrantyFrame" style="width: 760px;">
                                                <strong>Manufacturer Warranty:</strong><asp:Literal ID="litManufacturerWarranty" runat="server"></asp:Literal><br />
                                                <!-- 
                                                <strong>Installer Warranty:</strong><asp:Literal ID="litInstallerWarranty" runat="server" ></asp:Literal><br />
                                                <strong>Supplier Warranty:</strong><asp:Literal ID="litSupplierWarranty" runat="server" ></asp:Literal><br />
                                                -->
                                            </div>
                                            <div id="locationFrame" style="width: 370px; float: left;">
                                                <br /><strong>Location(s)</strong><br />
                                                <asp:Literal ID="litComponentLocations" runat="server" /> 
                                            </div>
                                            <div id="CommentsFrame" style="width: 370px; float: right;">
                                                <strong>Comments &amp; Additional Information</strong><br />                               
                                                <asp:Literal ID="litComments" runat="server" />                                      
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Applicable Unit(s)" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Literal ID="litApplicableUnits" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>        
                </div>    
                <div id="footerFrame" style="width: 1000px;">
                    <div id="submitFrame" style="width: 900px; position: absolute; left: 50%; margin-left: -450px;">
                        <p>
                            Please check the information above and review the Terms and Conditions below. 
                        </p>
                        <p style="text-align: right;">
                            <asp:HyperLink ID="lnkTerms" runat="server" Target="_blank" Text="Printable Version" /><br />
                            <asp:TextBox ID="txtTerms1" runat="server" TextMode="MultiLine" ReadOnly="true" Width="900px" Height="100px" />
                        </p>
                        <div id="acceptFrame" style="width: 400px; float: left;">
                            <p>
                                By clicking on 'I accept' below you are confirming that the information is ready for publication and agreeing to the 
                                <asp:HyperLink ID="lnkTerms2" runat="server" Target="_blank" Text="Terms and Conditions" /> above.
                            </p>
                            <p style="text-align: center">
                                <asp:Button ID="cmdAccept" runat="server" OnClick="Accept" Text="I accept. The information is ready for publication" />
                            </p>
                        </div>
                        <div id="rejectFrame" style="width: 400px; float: right;">
                            <p>
                                By clicking on 'I do not accept' below you are confirming that the information is not ready for publication and agreeing to the
                                <asp:HyperLink ID="lnkTerms3" runat="server" Target="_blank" Text="Terms and Conditions" /> above.
                            </p>
                            <p style="text-align: center">
                                <asp:Button ID="cmdReject" runat="server" OnClick="Reject" Text="I do not accept. The information is not ready for publication" />
                            </p>
                        </div>
                    </div>                    
                </div>         
            </div>            
        </div>
    </form>
</body>
</html>
