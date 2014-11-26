<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Electricity_page.aspx.cs" Inherits="CrebitAdminPanelNew.Electricity_page" %>

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
    <script src="Scripts/cookies.js" type="text/javascript"></script>
    <!-- Bootstrap core CSS -->
    <link href="bootstrap.min.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="dashboard.css" rel="stylesheet" />
    <link href="ListStyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.custom.js" type="text/javascript"></script>
    <script src="Scripts/cookies.js" type="text/javascript"></script>
    <style type="text/css">
        #ServicesList {
            display: none;
        }

        #StatusList {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#inputtxtDate").datepicker();
            $("#inputtxtDate").hide();
            $("#inputControl").val('');
        });
        function setModelHiddenValu(obj) {
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
            if ($("#SeletionList").val() == 4) {
                $("#StatusList").hide();
                $("#inputControl").hide();
                $("#inputtxtDate").show();
                return true;

            }
            if ($("#SeletionList").val() == 6) {
                $("#StatusList").hide();
                $("#inputControl").hide();
                $("#inputtxtDate").show();
                return true;

            }
            else if ($("#SeletionList").val() == 7) {
                $("#StatusList").show();
                $("#inputControl").hide();
                $("#inputtxtDate").hide();
                return true;
            }

            else {
                $("#StatusList").hide();
                $("#inputControl").show();
                $("#inputtxtDate").hide();

                return true;

            }
        }
    </script>
</head>
<body>
    <form runat="server">
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
                    <a class="navbar-brand " href="#">Crebit &gt; <span id="link_page">Electricity</span></a>
                </div>
                <div class="navbar-collapse collapse left_menu">
                    <ul id="ul_navbar" class="nav navbar-nav navbar-right">
                        <li><a id="a_electricity" class="active" href="Electricity_page.aspx?u=<%=QueryString%>">Electricity</a></li>
                        <li><a id="a_bank" href="Bank Transfer.aspx?u=<%=QueryString%>">Bank Transfer</a></li>
                        <li><a id="a_refund" href="RefundRequest.aspx?u=<%=QueryString%>">RefundRequest </a></li>
                        <li><a id="a_transction" href="Transaction_Page.aspx?u=<%=QueryString%>">Transaction</a></li>
                        <li><a href="#">Profile</a></li>
                        <li><a href="Login.aspx">Logout</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="dashboard-middle">
            <div class="navbar-collapse collapse subData">
                <ul class="nav navbar-nav navbar-right margin5 ">
                    <li id="ListItems">
                        <select class=" form-control" id="SeletionList" onchange="hideshow()" runat="server">
                            <option value="1">UserID</option>
                            <%-- <option value="2">Services </option>--%>
                            <option value="3">AccountNo</option>
                            <option value="4">DueDate</option>
                            <option value="5">TransactionId</option>
                            <option value="6">ReqDate</option>
                            <option value="7">Status</option>
                        </select>
                    </li>
                    <%--<li id="ServicesList">
                    <select class=" form-control" id="serviceList" runat="server">
                        <option value="1">MH Electricity</option>
                        <option value="2">Reliance Electricity</option>
                    </select>
                </li>--%>
                    <li id="StatusList">
                        <select class=" form-control" id="statusList" runat="server">
                            <option value="1">Success</option>
                            <option value="0">Failed</option>
                            <option value="3">In Progress</option>
                            <option value="4">Reject </option>
                        </select>
                    </li>

                    <li>



                        <input type="text" name="name" id="inputtxtDate" runat="server" />


                    </li>
                    <li>
                        <asp:TextBox class="form-control" ID="inputControl" runat="server"> </asp:TextBox>
                    </li>
                    <li>
                        <asp:Label ID="error_text" runat="server"></asp:Label></li>
                    <li>
                        <asp:Button Text="Filter" class="form-control btn-primary" runat="server" ID="bqtnFilter"
                            OnClick="btnFilter_ServerClick" />
                    </li>
                </ul>
            </div>
            <div class="row placeholders">
                <div class="col-xs-6 col-sm-3 placeholder">
                    <img data-src="holder.js/200x200/auto/sky" class="img-responsive" alt="Generic placeholder thumbnail" />
                    <h4></h4>
                    <span class="text-muted">(Success ,
                    <%=SuccessCount %>)</span>
                </div>
                <div class="col-xs-6 col-sm-3 placeholder">
                    <img data-src="holder.js/200x200/auto/vine" class="img-responsive" alt="Generic placeholder thumbnail" />
                    <h4></h4>
                    <span class="text-muted">(Failed,
                    <%=FailedCount %>)</span>
                </div>
                <div class="col-xs-6 col-sm-3 placeholder">
                    <img data-src="holder.js/200x200/auto/sky" class="img-responsive" alt="Generic placeholder thumbnail" />
                    <h4></h4>
                    <span class="text-muted">(Pending request,
                    <%=PendingCount %>)</span>
                </div>
                <div class="col-xs-6 col-sm-3 placeholder">
                    <img data-src="holder.js/200x200/auto/vine" class="img-responsive" alt="Generic placeholder thumbnail" />
                    <h4></h4>
                    <span class="text-muted">(In Progress,
                    <%=InProgressCount %>)</span>
                </div>
            </div>
            <!--Electricity -->
            <div id="electricity-details">
                <p id="electricity" class="space">
                </p>
                <div class="navbar navbar-inverse " role="navigation">
                    <div class="container-fluid">
                        <div class="navbar-header">
                            <p class="sub-header">
                                Electricity : <span class="sub-header">(TotalAmount ,<%=totalAmount%>)</span>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>Id
                                </th>
                                <th>UserId
                                </th>
                                <%-- <th>
                                ServiceId
                            </th>--%>
                                <th>Amount
                                </th>
                                <th>BUId
                                </th>
                                <th>AccountNo
                                </th>
                                <th>CusMob
                                </th>
                                <th>DueDate
                                </th>
                                <th>TransactionId
                                </th>
                                <th>ReqDate
                                </th>
                                <th>Comments
                                </th>
                                <th>&nbsp;
                                </th>
                                <th>Status
                                </th>
                            </tr>
                        </thead>
                        <tbody id="table_data" runat="server">
                        </tbody>
                    </table>
                </div>
            </div>
            <!--End Electricity -->
            <!-- Modal -->
            <div class="modal fade status_model" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title" id="myModalLabel">Transaction Id :</h4>
                        </div>
                        <div class="modal-body">
                            <asp:TextBox ID="inputTransactionToggleForm" Style="width: 100%;" runat="server"></asp:TextBox>
                        </div>
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>--%>
                            <h4 class="modal-title" id="H1">Comments :</h4>
                        </div>
                        <div class="modal-body">
                            <%-- <input id="hdnBtnId" type="hidden" name="name"  runat="server" />--%>
                            <asp:TextBox ID="inputCommentToggleForm" Style="width: 100%;" runat="server" />
                        </div>
                        <div class="modal-footer">
                            <%--<button type="button" class="btn btn-default" data-dismiss="modal" onclick="btnInsert_ServerClick">Close</button>--%>
                            <asp:Button Text="Close" class="btn btn-default" runat="server" ID="closebtn" OnClick="btnClose_ServerClick" />
                            <asp:Button Text="Save changes" class="btn btn-primary" runat="server" ID="saveChangebtn"
                                OnClick="btnInsert_ServerClick" />
                            <input id="hdnBtnId" type="hidden" name="name" runat="server" />
                            <input id="hdbBtnLi" type="hidden" runat="server" />
                            <%-- <button type="button" class="btn btn-primary">Save changes</button>--%>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Bootstrap core JavaScript
    ================================================== -->
            <!-- Placed at the end of the document so the pages load faster -->
            <%--  <script src="jquery.min.js"></script>--%>
            <script src="bootstrap.min.js"></script>
            <script src="docs.min.js"></script>
            <script src="dashboard.html.0.js"></script>
            <!-- IE10 viewport hack for Surface/desktop Windows 8 bug 

   <!-- <script src="ie10-viewport-bug-workaround.js"></script> -->
    </form>
</body>
</html>
