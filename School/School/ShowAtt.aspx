<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowAtt.aspx.cs" Inherits="School.ShowAtt" ValidateRequest="false" %>

<!DOCTYPE html>
<html lang="en" style="">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <title>Thakur Vidya Mandir  High School</title>
    <meta name="description" content="school">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <%--<link href="src/css/font-awesome.min.css" rel="stylesheet" />--%>
    <%--<link rel="stylesheet" href="/src/css/font.css">--%>
    <link href="/src/css/app.v2.css" rel="stylesheet" />
    <link href="/src/css/jquery-ui.css" rel="stylesheet" />
    <%--<link href="src/css/bootstrap.css" rel="stylesheet" />--%>
    <!-- This is what you need -->
    <script src="src/js/sweet-alert.js"></script>
    <link rel="stylesheet" href="src/css/sweet-alert.css">
    <!--.......................-->
        <style>
        @media (min-width: 768px) {

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

        <a class="navbar-brand" style="line-height: 109%;"  href="#">Thakur Vidya Mandir  High School</a>
        <button type="button" class="btn btn-link pull-left nav-toggle visible-xs" data-toggle="class:slide-nav slide-nav-left" data-target="body"><i class="fa fa-bars fa-lg text-default"></i></button>


    </header>
    <!-- / header -->
    <!-- nav -->
    <nav id="nav" class="nav-primary hidden-xs nav-vertical">
        <ul class="nav" data-spy="affix" data-offset-top="50">
            <li><a href="Attendance.aspx"><i class="fa fa-dashboard fa-lg "></i><span>Attendance</span></a></li>
            <li class="active"><a href="ShowAtt.aspx"><i class="fa fa-edit fa-lg"></i><span>Show </span></a></li>
            <li><a href="Message.aspx"><i class="fa fa-signal fa-lg"></i><span>SMS</span></a></li>
        </ul>
    </nav>
    <!-- / nav -->
    <section id="content">
        <section class="main padder">
            <div class="clearfix">
                <h4><i class="fa fa-table"></i>&nbsp; Show Attendance</h4>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <section class="panel">
                        <div class="panel-body">
                            <div class="row">
                                <form runat="server">
                                    <div class="col-sm-9 ">
                                        <select id="drpMedium" name="txt" class="input-sm inline form-control" style="width: 130px" runat="server" onchange="getStandard()">
                                            <option value="0">--Select--</option>
                                            <option value="English">English</option>
                                            <option value="Hindi">Hindi</option>
                                            <%--<option value="Marathi">Marathi</option>--%>
                                        </select>
                                        <select id="drpStandard" name="txt" class="input-sm inline form-control" style="width: 130px" runat="server" onchange="getSection()">
                                        </select>
                                        <select id="drpSection" class="input-sm inline form-control" style="width: 130px" runat="server">
                                        </select>
                                        <asp:Button Text="Apply" runat="server" ID='btnApply' class="btn btn-sm btn-primary" OnClick="btnApply_Click" OnClientClick="return applyFilter()" />
                                        <input id="hdndrpStandard" type="hidden" name="hdndrpStandard" runat="server" />
                                        <input id="hdndrpSection" type="hidden" name="hdndrpSection" runat="server" />
                                        <input id="hdnSelectedDrpStandard" type="hidden" name="hdnSelectedDrpStandard" runat="server" />
                                        <input id="hdnelectedDrpSection" type="hidden" name="hdnelectedDrpSection" runat="server" />
                                    </div>

                                    <div class="col-lg-3 right">
                                        <div class="input-group" style="max-width: 200px; margin-top: 10px;">
                                            <input type="text" id="txtGrNumber" class="input-sm form-control" placeholder="Search by GR number" runat="server">
                                            <span class="input-group-btn">
                                                <asp:Button Text="Go" runat="server" class="btn btn-sm btn-primary" ID="btnSearchGr" OnClick="btnSearchGr_Click" OnClientClick="return grSearch()" />
                                            </span>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>

                        <div id="table_responsive" class="table-responsive">
                            <asp:Repeater ID="rptTableData" runat="server">
                                <HeaderTemplate>
                                    <table class="table  table-striped b-t text-small ">
                                        <tr class="">
                                            <th class="">Name</th>
                                            <th class="">Gr </th>
                                            <th class="">Standard</th>
                                            <th class="">Medium</th>
                                            <th class="">Section</th>
                                            <th class="">&nbsp;</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Name") %></td>
                                        <td><%# Eval("Gr_num") %></td>
                                        <td><%# Eval("Std") %></td>
                                        <td><%# Eval("Medium") %></td>
                                        <td><%# Eval("Section") %></td>
                                        <td>
                                            <%--<button type="button" data-toggle="modal" data-target="#myModal">Launch modal</button>--%>
                                            <!-- Button to trigger modal -->
                                            <input id="<%# Eval("Gr_num") %>" class="btn show-att" value="Show" type="button" onclick="getAttendance(this)" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate></table></FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <footer id="panel_footer" class="panel-footer" runat="server">
                            <div class="row">
                            </div>
                        </footer>
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
    <script src="src/Js/jquery.dataTables.min.js"></script>
    <script src="src/Js/underscore-min.js"></script>
    <script>
        //global variables to store gr numbers
        var GR_NUMS = [];
        $(document).ready(function () {
            //set filter values after submit
            if ($("#hdndrpStandard").val() != "") {
                $drpStandard = $("#drpStandard");
                $drpSection = $("#drpSection")
                $drpStandard.html($("#hdndrpStandard").val());
                $drpSection.html($("#hdndrpSection").val());
                $drpSection.val($("#hdnelectedDrpSection").val());
                $drpStandard.val($("#hdnSelectedDrpStandard").val());
            }
        });


        function getAttendance(obj) {
            var grnum = obj.id;
            if (grnum.length > 0)
                callHandler("GetAttendance", grnum, getAttendanceCallBack);
            else
                sweetAlert("No Data.");

        }

        function getAttendanceCallBack(res) {
            var html = "";
            if (res != null && res.length > 0) {
                html = "<table class='table  table-striped b-t text-small'>";
                for (var i in res) {
                    html += "<tr><td>" + (parseInt(i)+ 1) + "</td><td>" + new Date(res[i].AbsentDate.Month + "/" + res[i].AbsentDate.Day +
                        "/" + res[i].AbsentDate.Year).toString().split('00:00:00')[0] + "</td></tr>";
                }
                html += "</table>";

                sweetAlert("Absent Days..");
                $(".lead.text-muted").html(html);

            }
            else {
                html = "No Absent Days";
                sweetAlert(html);
            }

        }

        function submitAttendance() {
            var qData = "";
            //$(".attendance").each(function (obj) { console.log(this.id) });
            $(".attendance").each(function (obj) {
                if (!$(this).hasClass("active"))
                    qData += this.id + "_";
            });
            if (qData.length > 0)
                callHandler("SubmitAttendance", qData, submitAttendanceCallBack);
            else
                sweetAlert("No Data.");
            //alert("No Data.");

        }
        //retrun on submit attendance
        function submitAttendanceCallBack(res) {
            try {
                var status = res[0].Status;
                if (status == 1)//success
                {
                    sweetAlert("Successfully saved.");
                    //alert("Successfully saved.");
                    //window.location.reload(true);
                }
                else { sweetAlert("Some error!!"); console.log(status) }
            }
            catch (ex) { console.log(ex.message) }
        }


        //$(document).ready(function () {
        function applyFilter() {
            $("input").prop('required', true);
            $("#txtGrNumber").removeAttr('required');
            $txtMedium = $("#drpMedium");
            $txtSection = $("#drpSection");
            $txtStandard = $("#drpStandard");
            var msg = "Enter Req. Field."
            var retVal = false;
            if ($txtMedium.val().length == 0) {
                retVal = false;
                msg = "Select any Medium";
            }
            else {
                if ($txtSection.val().length == 0) {
                    retVal = false;
                    msg = "Select any Section";
                }
                else {
                    if ($txtStandard.val().length == 0) {
                        retVal = false;
                        msg = "Select any Standard";
                    }
                }
            }

            $("#hdnSelectedDrpStandard").val($txtStandard.val());
            $("#hdnelectedDrpSection").val($txtSection.val());

            return true;
        }

        function grSearch() {
            $txtGrNumber = $("#txtGrNumber");
            if ($txtGrNumber.val().length == 0)
                return false;
            return true;
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


