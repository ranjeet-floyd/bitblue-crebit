<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transaction_Page.aspx.cs"
    Inherits="CrebitAdminPanelNew.Transaction_Page" %>

<!DOCTYPE html>
<html lang="en" style="overflow: auto">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="Crebit Request Management" />
    <meta name="author" content="Ranjeet" />
    <link rel="icon" href="../../favicon.ico" />
    <title>Crebit Admin Dashboard</title>
    <!-- Bootstrap core CSS -->
    <link href="bootstrap.min.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="dashboard.css" rel="stylesheet" />
    <style type="text/css">
        #StatusList
        {
            display: none;
        }
    </style>
      <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.custom.js" type="text/javascript"></script>
    <script src="Scripts/cookies.js" type="text/javascript"></script>
     <script type="text/javascript">
        $(document).ready(function () {
            $("#inputtxtDate").datepicker();
            $("#inputtxtDate").hide();
            $("#ServicesList").hide();
            $("#OperaterName").hide();
            $("#inputControl").val('');
        });
        
        function setModelHiddenValu(obj) {
            // $("selectionToggle").index(listItem)
            var ArrayId = obj.id.split('_');
            var id = parseInt(ArrayId[1]);
            $("#hdnBtnId").val(id);
            var listatus = parseInt(ArrayId[2]);
            $("#hdbBtnLi").val(listatus);

        }
    </script>
      <script type="text/javascript">
          function hideshow() {
              $("#error_text").hide();

              if ($("#SeletionList").val() == 3) {
                  $("#ServicesList").show();
                  $("#StatusList").hide();
                  $("#inputControl").hide();
                  $("#inputtxtDate").hide();
                  $("#OperaterName").hide();
                  return true;

              }
              else if ($("#SeletionList").val() == 4) {
                  $("#StatusList").hide();
                  $("#inputControl").hide();
                  $("#inputtxtDate").show();
                  $("#OperaterName").hide();
                  $("#ServicesList").hide();

                  return true;

              }
              else if ($("#SeletionList").val() == 6) {
                  $("#ServicesList").hide();
                  $("#StatusList").show();
                  $("#inputControl").hide();
                  $("#inputtxtDate").hide();
                  $("#OperaterName").hide();
                  return true;
              }
              if ($("#SeletionList").val() == 7) {
                  $("#ServicesList").hide();
                  $("#StatusList").hide();
                  $("#inputControl").hide();
                  $("#inputtxtDate").hide();
                  $("#OperaterName").show();
                  return true;

              }

              else {
                  $("#StatusList").hide();
                  $("#ServicesList").hide();
                  $("#inputControl").show();
                  $("#inputtxtDate").hide();
                  $("#OperaterName").hide();
                  return true;

              }

          }


    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <div class="navbar navbar-inverse navbar-fixed-top" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed float-left" data-toggle="collapse"
                    data-target=".left_menu">
                    <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span
                        class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <button type="button" class="navbar-toggle collapsed float-right" data-toggle="collapse"
                    data-target=".subData">
                    <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span
                        class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <a class="navbar-brand " href="#">Crebit &gt; <span id="link_page">Transaction</span></a>
            </div>
            <div class="navbar-collapse collapse left_menu">
                <ul id="ul_navbar" class="nav navbar-nav navbar-right">
                    <li><a id="a_electricity" href="Electricity_page.aspx?u=<%=QueryString%>">Electricity</a></li>
                    <li><a id="a_bank" href="Bank Transfer.aspx?u=<%=QueryString%>">Bank Transfer</a></li>
                    <li><a id="a_refund" href="RefundRequest.aspx?u=<%=QueryString%>">RefundRequest </a>
                    </li>
                    <li><a id="a1" class="active" href="Transaction_Page.aspx?u=<%=QueryString%>">Transaction</a></li>
                    <li><a href="#">Profile</a></li>
                    <li><a href="Login.aspx">Logout</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="dashboard-middle">
        <div class="navbar-collapse collapse subData">
            <ul class="nav navbar-nav navbar-right margin5 ">
                <li>
                    <select class=" form-control" id="SeletionList" onchange="hideshow()" runat="server">
                        <option value="1">UserId </option>
                        <option value="2">ApiTransactionId </option>
                        <option value="3">ServiceType </option>
                        <option value="7">OperaterName </option>
                        <option value="4">Date </option>
                        <option value="5">CreditAccountNo</option>
                        <option value="6">Status </option>
                        
                    </select>
                </li>
                <li id="StatusList">
                    <select class=" form-control" id="statusList" runat="server">
                        <option value="1">Success</option>
                        <option value="0">Failed</option>
                        <option value="2">Pending</option>
                        <option value="3">In Progress</option> 
                        <option value="4"> Reject </option>
                        <option value="5"> Received </option>
                        <option value="6">Others</option>
                        <option value="7"> Not Known </option>
                        <option value="8"> Awaiting </option>
                        <option value="9"> Refunded </option>
                    </select>
                </li>
                <li id="ServicesList" >
            <select class=" form-control"  id="serviceList" runat="Server">
	<option value="1">	PostPaid</option>
	<option value="2">	PrePaid	</option>
	<option value="3">	DTH	</option>
	<option value="4">	Electricity	</option>
	<option value="5">	Gas Bill	</option>
	<option value="6">	Insurance	</option>
	<option value="7">	BroadBand	</option>
	<option value="8">	Data Card	</option>
	<option value="9">	Fund Transfer</option>
	<option value="10">	Bank Transfer</option>        
    <option value="11">Crebit Admin</option>
    <option value="12">Crebit Monthly Charge</option>
    <option value="13">Money Transfer</option>		
    <option value="14">Crebit Refund</option>	
          
		  </select>
          </li>
          <li id="OperaterName">
    <select class="form-control"  id="operaterName" runat="Server">
	<option value="1">    	Airtel Landline	</option>
 	<option value="2">		Airtel	</option>
 	<option value="3">		Cellone	</option>
 	<option value="4">		Idea	</option>
 	<option value="5">		Loop Mobile	</option>
 	<option value="6">		Reliance	</option>
 	<option value="7">		Tata Docomo	</option>
 	<option value="8">		Tata TeleServices	</option>
 	<option value="9">		Vodafone	</option>
 	<option value="10">		Aircel	</option>
 	<option value="11">		Airtel	</option>
 	<option value="12">		BSNL	</option>
 	<option value="13">		BSNL(Validity/Special)</option>
 	<option value="14">		Idea	</option>
 	<option value="15">		Loop	</option>
 	<option value="16">		MTNL(TopUp)	</option>
 	<option value="17">		MTNL(Validity)</option>
 	<option value="18">		MTS	</option>
 	<option value="19">		Reliance(CDMA)</option>
 	<option value="20">		Reliance(GSM)</option>
 	<option value="21">		T24(Flexi)	</option>
 	<option value="22">		T24(Special)</option>
 	<option value="23">		Tata Docomo(Flexi)	</option>
 	<option value="24">		Tata Docomo(Special)</option>
 	<option value="25">		Tata Indicom	</option>
 	<option value="26">		Uninor</option>
 	<option value="27">		Videocon</option>
 	<option value="28">		Virgin(CDMA)</option>
 	<option value="29">		Virgin(GSM/Flexi)</option>
 	<option value="30">		Virgin(GSM/Special)</option>
 	<option value="31">		Vodafone</option>
 	<option value="32">		Airtel Digital TV</option>
 	<option value="33">		Big TV	</option>
 	<option value="34">		Dish TV	</option>
 	<option value="35">		Sun Direct	</option>
 	<option value="36">		Tata Sky(B2C)</option>
 	<option value="37">		Videocon d2h</option>
 	<option value="38">		MSEB	</option>
 	<option value="41">		Reliance(Mumbai)</option>	
 	<option value="42">		Mahanagar Gas Limited	</option>
 	<option value="43">		ICICI Pru. Life	</option>
 	<option value="44">		Tata AIG Life	</option>
 	<option value="45">		Tikona Postpaid	</option>
 	<option value="46">		Aircel	</option>
 	<option value="47">		Airtel	</option>
 	<option value="48">		BSNL	</option>
 	<option value="49">		Idea	</option>
 	<option value="50">		MTS	</option>
 	<option value="51">		Reliance	</option>
 	<option value="52">		Tata Docomo	</option>
 	<option value="53">		Tata Indicom	</option>
 	<option value="55">		Crebit Fund Transfer	</option>
 	<option value="56">		Crebit Monthly Charge	</option>
 	<option value="57">		Money Transfer	</option>
 	<option value="58">		Crebit Refund Req.</option>
		 </select>
          </li>
                <li>
                   
                    <input type="text" name="name" id="inputtxtDate" runat="server" />
                    
                </li>
                <li>
                    <asp:TextBox class="form-control" ID="inputControl" runat="server"> </asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="error_text" runat="server"> </asp:Label></li>
                <li>
                    <asp:Button Text="Filter" class="form-control btn-primary" runat="server" ID="bqtnFilter"
                        OnClick="btnFilter_ServerClick" />
                </li>
            </ul>
        </div>
      <!--  <div class="row placeholders">
            <div class="col-xs-6 col-sm-3 placeholder">
                <img data-src="holder.js/200x200/auto/sky/text:Add <br/> \n line breaks \n anywhere." class="img-responsive" alt="Generic placeholder thumbnail" />
                <img data-src="holder.js/300x200/text:Add \n line breaks \n anywhere.">
                <h4>
                </h4>
                <span class="text-muted" style="color:#000">(Success:<%=SuccessCount%>)<br/>(TotalAmount:<%=success_AmountCount%>)<br/>(ElecMSEB:<%=success_ElecAmount%>)<br/>(CrebitFund:<%=success_FundAmount%>)<br>(MoneyTransfer:<%=success_MoneyTransferAmount%>)<br/>CyberPlate:<%=success_cyberPlate%>)</span>
            </div>
            <div class="col-xs-6 col-sm-3 placeholder">
                <img data-src="holder.js/200x200/auto/vine" class="img-responsive" alt="Generic placeholder thumbnail" />
                <h4>
                </h4>
                <span class="text-muted" style="color:#000">(Failed:<%=FailedCount%>)<br/>(TotalAmount:<%=failed_AmountCount%>)<br/>(ElecMSEB:<%=failed_ElecAmount%>)<br/>(CrebitFund:<%=failed_FundAmount%>)<br>(MoneyTransfer:<%=failed_MoneyTransferAmount%>)<br/>(CyberPlate:<%=failed_cyberPlate%>)</span>
            </div>
            <div class="col-xs-6 col-sm-3 placeholder">
                <img data-src="holder.js/200x200/auto/sky" class="img-responsive" alt="Generic placeholder thumbnail" />
                <h4>
                </h4>
                <span class="text-muted" style="color:#000">(Pending request:<%=PendingCount%>)<br/>(TotalAmount:<%=pending_AmountCount%>)<br/>(ElecMSEB:<%=pending_ElecAmount%>)<br/>(CrebitFund:<%=pending_FundAmount%>)<br>(MoneyTransfer:<%=pending_MoneyTransferAmount%>)<br/>(CyberPlate:<%=pending_cyberPlate%>)</span>
            </div>
            <div class="col-xs-6 col-sm-3 placeholder">
                <img data-src="holder.js/200x200/auto/vine" class="img-responsive" alt="Generic placeholder thumbnail" />
                <h4>
                </h4>
                <span class="text-muted" style="color:#000">(In Progress:<%=InProgressCount%>)<br/>(TotalAmount:<%=InPro_AmountCount%>)<br/>(ElecMSEB:<%=InPro_ElecAmount%>)<br/>(CrebitFund:<%=InPro_FundAmount%>)<br>(MoneyTransfer:<%=InPro_MoneyTransferAmount%>)<br/>(CyberPlate:<%=InPro_cyberPlate%>)</span>
            </div>
            
        </div> -->
        
         <div class="table-responsive">
                <table class="table table-striped" style="border: 1px solid black; width:50%; margin-left: 25%; margin-right:25%">
                    <thead>
                        <tr style="background-color:#000; color:#fff;">
                        <th># </th>
                        <th>Success</th>
                        <th>Failed</th>
                        <th>PendingRequest</th>
                        <th>InProgress</th>
                        </tr>
                        </thead>
                        <tr><td style="background-color:#000; color:#fff;">Count</td><td>Rs.<%=SuccessCount%></td><td>Rs. <%= FailedCount%></td><td>Rs.<%=PendingCount%>  </td><td>Rs.<%= InProgressCount%> </td></tr>
                        <tr><td style="background-color:#000; color:#fff;">Amount</td><td>Rs.<%=success_AmountCount%></td><td>Rs.<%= failed_AmountCount %></td><td>Rs.<%=pending_AmountCount %> </td><td> Rs.<%=InPro_AmountCount %></td></tr>
                        <tr><td style="background-color:#000; color:#fff;">MSEB</td><td>Rs.<%=success_ElecAmount%></td><td>Rs.<%= failed_ElecAmount%> </td><td>Rs.<%=pending_ElecAmount  %></td><td> Rs.<%=InPro_ElecAmount %></td></tr>
                        <tr><td style="background-color:#000; color:#fff;">CrebitFund</td><td>Rs.<%=success_FundAmount%></td><td>Rs.<%= failed_FundAmount %></td><td>Rs.<%=pending_FundAmount %> </td><td>Rs. <%=InPro_FundAmount %></td></tr>
                        <tr><td style="background-color:#000; color:#fff;">MoneyTransfer</td><td>Rs.<%=success_MoneyTransferAmount%></td><td>Rs. <%=failed_MoneyTransferAmount %></td><td>Rs.<%=pending_MoneyTransferAmount %></td><td>Rs.<%=InPro_MoneyTransferAmount %></td></tr>
                        <tr><td style="background-color:#000; color:#fff;">CyberPlate</td><td>Rs.<%=success_cyberPlate%></td><td>Rs. <%=failed_cyberPlate %></td><td>Rs.<%=pending_cyberPlate %> </td><td>Rs. <%=InPro_cyberPlate %></td></tr>
                            </table></div>

        <!--Electricity -->
        <div id="electricity-details">
            <p id="electricity" class="space">
            </p>
            <div class="navbar navbar-inverse " role="navigation">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <p class="sub-header">
                            TRANSACTION :
                                                   </p>
                        
                    </div>
                </div>
            </div>
            <div class="table-responsive">
                <table class="table table-striped">
                    <thead>
                        <tr>
                            <th>
                                Id
                            </th>
                            <th>
                                UserId
                            </th>
                            <th>
                                ApiTransactionId
                            </th>
                            <th>
                                OperaterName/ServiceType  
                            </th>
                            <th>
                                Amount
                            </th>
                            <th>
                                ApiId
                            </th>
                            <th>
                                CreditAccountNo
                            </th>
                            <th>
                                Session
                            </th>
                            <th>
                                Date
                            </th>
                            <th>
                                Status
                            </th>
                            <th>
                                &nbsp;
                            </th>
                        </tr>
                    </thead>
                    <tbody id="table_data" runat="server">
                    </tbody>
                </table>
            </div>
        </div>
        <!--End Electricity -->
        <!--Bank -->
        <div id="bank-details" hidden="">
            <p id="bankTransfer" class="space">
            </p>
            <div class="navbar navbar-inverse " role="navigation">
                -
                <%--<p class="sub-header">Bank Transfer  </p>--%>
            </div>
        </div>
        <!--End Bank -->
    </div>
    <div class="modal fade status_model" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>--%>
                    <%--<h4 class="modal-title" id="myModalLabel">Transaction Id :</h4>--%>
                </div>
                <div class="modal-body">
                    <%--<asp:Textbox  id="inputTransactionToggleForm" style="width:100%;" runat="server"></asp:TextBox>--%></div>
                <div class="modal-header">
                    <%-- <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>--%>
                    <%-- <h4 class="modal-title" id="H1">Comments :</h4>--%>
                </div>
                <div class="modal-body">
                    <%-- <input id="hdnBtnId" type="hidden" name="name"  runat="server" />--%>
                    <%-- <asp:Textbox  id="inputCommentToggleForm" style="width:100%;" runat="server"/>--%>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal" onclick="btnInsert_ServerClick">Close</button>
       <asp:Button Text="Close" class="btn btn-default" runat="server" ID="closebtn" OnClick="btnClose_ServerClick" />
                    <asp:Button Text="Save changes" class="btn btn-primary" runat="server" ID="saveChangebtn" OnClick="btnInsert_ServerClick" />--%>
                    <%-- <input id="hdnBtnId" type="hidden" name="name"    runat="server" />
                     <input id="hdbBtnLi" type="hidden"   runat="server" />
                   <button type="button" class="btn btn-primary">Save changes</button>
                    --%></div>
            </div>
        </div>
    </div>
    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <%--<script src="jquery.min.js"></script>--%>
    <script src="bootstrap.min.js"></script>
    <script src="docs.min.js"></script>
    <%--<script src="dashboard.html.0.js"></script>--%>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug 

   <!-- <script src="ie10-viewport-bug-workaround.js"></script> -->
    </form>
</body>
</html>
