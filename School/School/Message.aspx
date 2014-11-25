<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="Message.School.Message" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <title>Thakur Vidya Mandir  High School</title>
    <meta name="description" content="Mother Teresa, app, web app, responsive, admin dashboard, flat, flat ui">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="stylesheet" href="/src/css/font.css">
    <link href="/src/css/app.v2.css" rel="stylesheet" />
    <!-- This is what you need -->
    <script src="/src/js/sweet-alert.js"></script>
    <link rel="stylesheet" href="/src/css/sweet-alert.css">
    <!--.......................-->
    <!--[if lt IE 9]> <script src="js/ie/respond.min.js"></script> <script src="js/ie/html5.js"></script> <![endif]-->
    <style>
        @media (min-width: 768px) {
            .marginleft20 {
                margin-left: 17%;
            }

            .navbar-brand {
                float: none;
                display: block;
                margin: 0;
                text-align: center;
            }
        }
    </style>
</head>
<body>
    <!-- header -->
    <header id="header" class="navbar" style="min-height: 72px;">
        <div class="top-menu " id="menu_item">
            <div class="btn-group nav pull-right" style="margin-top: 15px;">
                <a class="btn btn-primary dropdown-toggle" data-toggle="dropdown" href="#">
                    <i class="fa fa-user fa-fw"></i>
                    <span class="fa fa-caret-down"></span>
                </a>
                <ul class="dropdown-menu" role="menu">
                    <li class="divider"></li>
                    <li>
                        <a class="logout" href="/login.aspx">Logout</a>
                    </li>
                </ul>
            </div>
        </div>

        <a class="navbar-brand" style="line-height: 109%;" href="#">Thakur Vidya Mandir  High School</a>
        <button type="button" class="btn btn-link pull-left nav-toggle visible-xs" data-toggle="class:slide-nav slide-nav-left" data-target="body"><i class="fa fa-bars fa-lg text-default"></i></button>


    </header>

    <!-- / header -->
    <!-- / header -->
    <!-- nav -->
    <nav id="nav" class="nav-primary hidden-xs nav-vertical">
        <ul class="nav" data-spy="affix" data-offset-top="50">
            <li><a href="Attendance.aspx"><i class="fa fa-dashboard fa-lg "></i><span>Attendance</span></a></li>
            <li><a href="ShowAtt.aspx"><i class="fa fa-edit fa-lg"></i><span>Show </span></a></li>
            <li class="active"><a href="Message.aspx"><i class="fa fa-signal fa-lg"></i><span>SMS</span></a></li>
        </ul>
    </nav>
    <!-- / nav -->
    <section id="content">
        <section class="main padder">
            <div class="clearfix">
                <h4><i class="fa fa-table"></i>&nbsp;Send Message</h4>
            </div>
            <div class="row marginleft20">
                <div class="col-sm-10 ">
                    <section class="panel">
                        <div class="panel-body form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">Medium</label>
                                <div class="col-lg-8">
                                    <select id="drpMedium" name="medium" class="form-control" onchange="getStandard()">
                                        <option value="0">--Select--</option>
                                        <option value="English">English</option>
                                        <option value="Hindi">Hindi</option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">Standard</label>
                                <div class="col-lg-8" id="">
                                    <select id="drpStandard" name="standard" class="form-control" onchange="getSection()">
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">Section</label>
                                <div class="col-lg-8">
                                    <select id="drpSection" name="section" class="form-control">
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-9 col-lg-offset-3">
                                    <button type="submit" class="btn btn-primary" onclick="getSMSNumbers()">Apply</button>
                                </div>
                            </div>
                            <div class="divider" style="border-bottom: 1px solid #eee;"></div>
                            <div class="input-group" style="max-width: 300px; margin-top: 10px; float: right;">
                                <input type="text" id="txtGrSearch" class=" form-control" placeholder="or search by GR number" runat="server">
                                <span class="input-group-btn">
                                    <input type="button" name="name" id="btnGrSearch" value="Add" class=" btn  btn-primary " onclick="getMobileByGRNumber()" />
                                </span>
                            </div>
                            <%--<asp:Button Text="Apply" runat="server" ID="btnApply" class="btn btn-sm btn-white" OnClick="btnApply_Click" OnClientClick="return applyFilter()" />--%>
                        </div>
                    </section>
                </div>
            </div>
            <div class="row marginleft20">
                <div class="col-sm-10">
                    <section class="panel">
                        <div class="panel-body">
                            <form class="form-horizontal" method="get" data-validate="parsley" runat="server">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">Mobiles </label>
                                    <div class="col-lg-8">
                                        <textarea id="txtAreaMobile" runat="server" placeholder="Enter , seperated Mobile  Numbers" rows="5" class="form-control parsley-validated" data-trigger="keyup" data-rangelength="[20,200]"></textarea>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">Message </label>
                                    <div class="col-lg-8">
                                        <textarea id="txtAreaMessage" runat="server" placeholder="Enter  Message" rows="5" class="form-control parsley-validated" data-trigger="keyup" data-rangelength="[20,200]"></textarea>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-lg-9 col-lg-offset-3">
                                        <input type="button" id="btnSendSMS" class="btn btn-primary" value="Send Message" onclick="sendSMS()" />
                                    </div>
                                </div>

                            </form>
                        </div>
                    </section>

                </div>





            </div>
        </section>
    </section>
    <!-- footer -->
    <footer id="footer">
        <div class="text-center padder clearfix">
            <p>
                <small>© BitBlue 2014 </small>
                <br>
            </p>
        </div>
    </footer>
    <!-- / footer -->

    <!-- app -->
    <script src="src/Js/app.v2.js"></script>
    <script src="src/Js/fuelux.js"></script>
    <script src="src/Js/jquery.dataTables.min.js"></script>
    <script src="src/Js/underscore-min.js"></script>
    <script>

        function getMobileByGRNumber() {
            if ($("#txtGrSearch").val() != "") {
                swal({ title: "Please Wait ...", text: "Please Wait ...", imageUrl: "src/Img/spiffygif_96x96.gif" });
                $(".confirm").hide();
                callHandler("GetMobileByGRNumber", $("#txtGrSearch").val(), getMobileByGRNumberCallBack)
            }
            else
                sweetAlert("Please enter any GR number");
        }
        function getMobileByGRNumberCallBack(res) {
            try {
                if (res != null && res != 0) {
                    if (document.getElementById('txtAreaMobile').value != "")
                        //$("#txtAreaMobile").text($("#txtAreaMobile").text() + ',' + res);
                        document.getElementById('txtAreaMobile').value = document.getElementById('txtAreaMobile').value + ',' + res
                    else
                        document.getElementById('txtAreaMobile').value = res;
                }
                else
                    sweetAlert("No Data.");
                swal({ title: "Please Wait ...", timer: 1 });
                $(".confirm").show();
            }
            catch (ex) { console.log(ex); }
        }

        function getSMSNumbers() {
            drpMedium = $("#drpMedium").val();
            drpStandard = $("#drpStandard").val();
            drpSection = $("#drpSection").val();
            try {
                if (drpMedium.length > 0 && drpStandard.length > 0 && drpSection.length > 0) {
                    swal({ title: "Please Wait ...", text: "Please Wait ...", imageUrl: "src/Img/spiffygif_96x96.gif" });
                    $(".confirm").hide();
                    callHandler("GetSMSNumbers", drpMedium + "|" + drpSection + "|" + drpStandard, getSMSNumbersCallBack);
                }
                else
                    sweetAlert("Please choose dropDwon Fileds.");
            }
            catch (ex) { console.log(ex); }

        }

        function getSMSNumbersCallBack(res) {
            if (res != null && res.Table.length > 0) {
                var mobiles = "";
                for (var i in res.Table) {
                    if (res.Table[i].mobile.length == 10)
                        mobiles += res.Table[i].mobile.split(' ').join(',') + " , ";
                }
                //alert($("#txtAreaMobile").text());
                $("#txtAreaMobile").text(mobiles);
            }
            else
                sweetAlert("No Data.");
            swal({ title: "Please Wait ...", timer: 1 });
            $(".confirm").show();
        }

        function sendSMS() {
            swal({ title: "Please Wait ...", text: "Please Wait ...", imageUrl: "src/Img/spiffygif_96x96.gif" });
            $(".confirm").hide();
            var txtAreaMobile = $("#txtAreaMobile").val().trim(',');
            var dataJSON = {};
            if (txtAreaMobile != "No Data !!" && $("#txtAreaMessage").val().trim().length < 140 && $("#txtAreaMessage").val().trim().length > 0) {
                dataJSON.mobiles = txtAreaMobile; dataJSON.methodName = "SendSMS"; dataJSON.message = $("#txtAreaMessage").val().trim();
                jQuery.ajax({
                    type: "POST",
                    data: dataJSON,
                    async: true,
                    url: "src/model/schHandler.ashx",
                    success: function (data) {
                        //alert(data);
                        sendSMSCallBack(data);
                    },
                    error: function (data) {
                        if (data.responseText == "True") {
                            sweetAlert("Message Successfully Sent.");
                        }
                        console.log(data);
                    }
                });
                //callHandler("SendSMS", txtAreaMobile, sendSMSCallBack);
            }
            else { sweetAlert("Either  no Mobile Number or Message is too big (keep 140 chars)"); }
            // swal({ title: "Please Wait ...", timer: 1 });
            $(".confirm").show();
        }

        function sendSMSCallBack(res) {
            if (res = "True") {
                sweetAlert("Successfully Sent !!");
            }
        }

        function callHandler(methodName, qData, callBack) {
            jQuery.ajax({
                type: "GET",
                async: true,
                url: "src/model/schHandler.ashx",
                data: "MethodName=" + methodName + "&q=" + qData,
                success: function (data) {
                    callBack(data);
                }
            });
        }

        function getStandard() {
            var medium = $("#drpMedium").val();
            callHandler("GetStandard", medium, setDrpStandard);
            var html = "<option value='0'>-- Wait --</option>";
            $("#drpStandard").html(html);
        }

        function getSection() {
            var medium = $("#drpMedium").val();
            var section = $("#drpStandard").val();
            callHandler("GetSection", medium + "|" + section, setDrpSection);
            var html = "<option value='0'>-- Wait --</option>";
            $("#drpSection").html(html);
        }

        function setDrpStandard(res) {
            var html = "";
            try {
                if (res.Table.length > 0) {
                    resObj = res.Table;
                    html += "<option value='0'>-- Select --</option>";
                    for (var i in resObj) {
                        html += "<option value=" + resObj[i].Std + ">" + resObj[i].Std + "</option>";
                    }
                }
                else
                    html += "<option value='0'>-- No values --</option>";
                $("#drpStandard").html(html);
                $("#hdndrpStandard").val(html);//store 

            }
            catch (ex)
            { }
        }

        function setDrpSection(res) {
            var html = "";
            var text = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "G"];
            try {
                if (res.Table.length > 0) {
                    resObj = res.Table;
                    html += "<option value='0'>-- Select --</option>";
                    var noOfDiv = resObj[0].div;
                    for (var i = 0 ; i < noOfDiv; i++) {
                        html += "<option value=" + text[i] + ">" + text[i] + "</option>";
                    }
                }
                else
                    html += "<option value='0'>-- No values --</option>";
                $("#drpSection").html(html);
                $("#hdndrpSection").val(html);//store 
            }
            catch (ex)
            { }
        }
    </script>


</body>
</html>
