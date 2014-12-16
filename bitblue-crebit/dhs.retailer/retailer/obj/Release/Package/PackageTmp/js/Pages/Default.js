
'Use Strict';
var opId = "";
$(document).ready(function () {
    try {

        //$(function () {
        //    $("#txtDueDate").datepicker({ minDate: +1, maxDate: "+1M" });
        //    //$("#format").change(function () {
        //    // $("#datepicker").datepicker("option", "dateFormat", $(this).val());
        //    //});
        //});

        //reset model
        $('.modal').on('shown.bs.modal', function () {
            $(".showMe-reset").removeClass('hidden');
            if ($('#drpElectricityBillOp').val() == 40) {
                $(".cusdetails").show();
                $(".paybill").hide();
            }
            else {
                $(".cusdetails").hide();
            }

            $(".msg-box").html('');//reset message
            $('input[type=text] ,input[type=number]').each(function (i, obj) {
                $(obj).val('');
            })
        });

        userId = USERID;
        key = KEY;
        NOTIFI = false;
        $("#date-popover").popover({ html: true, trigger: "manual" });
        $("#date-popover").hide();
        $("#date-popover").click(function (e) {
            $(this).hide();
        });

        $("#cr_name").text(NAME);
        $("#cr_availBal").text(BALANCE);
        //Personal | EnterPrise user
        if (UTYPE == 2)
            $("#li_marginNav").addClass("hidden");//Hide margin list

        //Get operator json || on page load ajax call
        //operators();

        //Electricity drop down Change|| Change form field
        $("#drpElectricityBillOp").change(function (e) {
            if ($("#drpElectricityBillOp").val() == "41") //Reliance
            {
                $(".mseb").addClass('hidden');
                $(".cusAccountNo").removeClass('hidden').show();
                $(".reliance").removeClass('hidden').show();
                $(".torrentPower").addClass('hidden');

            }
            else if ($("#drpElectricityBillOp").val() == "40") //MSEB
            {
                $(".torrentPower").addClass('hidden');
                $(".mseb").removeClass('hidden').show();
                $(".cusAccountNo").removeClass('hidden').show();
                $(".reliance").addClass('hidden');
                $(".paybill").addClass('hidden');

            }
            else if ($("#drpElectricityBillOp").val() == "42")//TorrentPower
            {
                $(".cusAccountNo").addClass('hidden');
                $(".mseb").addClass('hidden');
                $(".paybill").addClass('hidden');
                $(".reliance").addClass('hidden');
                $(".payAmount").removeClass('hidden').show();
                $(".torrentPower").removeClass('hidden').show();
            }
        });

        ////Fund Transafer
        //$("#btnTransfer").click(function () {
        //    // alert(opId);
        //    var sr_number = $("#ft_number").val();
        //    var sr_amount = $("#ft_amount").val();
        //    transferFund(userId, key, sr_number, sr_amount, 4);
        //});


        //pop up || Admin Pop Up Message on page load
        $('.gritter-close').click(function () {
            $('#notification_Model').modal('hide');
        });

        //cookies
        if (readCookie("cr_notify") != null) {
            popUp(); se
            eraseCookie("cr_notify");
        }
        var unique_id = $.gritter.add({
            title: '',
            // (string | mandatory) the text inside the notification
            text: 'Online dd recharge Portal',
            // (string | optional) the image to display on the left
            //image: '/img/ui-sam.jpg',
            image: '',
            // (bool | optional) if you want it to fade out on its own or just sit there
            sticky: true,
            // (int | optional) the time you want it to be alive for before fading out
            time: '',
            // (string | optional) the class name you want to apply to that specific message
            class_name: 'my-sticky-class'
        });

        //Clear text box for recharge
        $('#model_recharge').on('shown.bs.modal', function () {
            $("#sr_number").val("");
            $("#sr_amount").val("");
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function form_Fund() {
    var sr_number = $("#ft_number").val();
    var sr_amount = $("#ft_amount").val();
    transferFund(userId, key, sr_number, sr_amount, 4);
    return false;
}

//fund Transfer   //Status ://    0 - Some Error    //1- Got Profit + Success     //2- Success   //3-  Not Enough Balance  //4- User not exist
function transferFund(userId, key, mobileTo, amount) {
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = userId; dataJSON.key = key; dataJSON.mobileTo = mobileTo; dataJSON.amount = amount;
    AjaxCall("POST", "/dashboard/transfer", dataJSON, transferFundCallBack);//call api
}
//Ajax call back for Transfer fund
function transferFundCallBack(resObj) {
    try {
        var html = "";
        var mobileTo = $("#ft_number").val();
        var amount = $("#ft_amount").val();
        //Success
        if (parseInt(resObj[0].status) == 1) {//

            html = '<div class="alert alert-success"><h3>Successfully Transfered! .</h3>';
            html += '<p>Amount :' + amount + ' </p><p>Number :' + mobileTo + ' </p></div>'
        }
        else {
            if (parseInt(resObj[0].status) == 2) {
                html = '<div class="alert alert-success"><h3>Successfully Transfered! .</h3>';
                html += '<p>Amount :' + amount + ' </p><p>Number :' + mobileTo + ' </p></div>'
            }
            else {
                if (parseInt(resObj[0].status) == 3) {
                    html = '<div class="alert alert-danger">Not enough Balance</div>';

                }
                else {
                    if (parseInt(resObj[0].status) == 4)
                        html = '<div class="alert alert-danger">Mobile Number  not Exist with Crebit.</div>';
                    else {
                        if (parseInt(resObj[0].status) == 5)
                            html = '<div class="alert alert-danger">You can not tranfer to Enterprise type account.</div>';
                        else {
                            if (parseInt(resObj[0].status) == 6)
                                html = '<div class="alert alert-danger">You can not tranfer to same account.</div>';
                            else
                                html = '<div class="alert alert-danger">Some Error..Please try again later.</div>';
                        }
                    }
                }
            }
        }
        BALANCE = resObj[0].availableBalance;
        UpdateCrCookies();
    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in">Invalid data ! Check Input data.</div>';
    }

    $("#model_msg_body").html(html);
    $('#model_msg').modal('show');

}


//MSEB
function verifyMSEBCustomer() {
    $(".cusdetails").prop('required', true);
    if ($("#txtCustomerAccNo").val() != "") {
        MSEBVerification(USERID, KEY, 40, $("#txtCustomerAccNo").val(), $("#drpBuElectricity").val());
        return true;
    }
    else
        //$("#txtCustomerAccNo").css('style', 'border-color:red');
        return false;
}

function MSEBVerification(userId, key, serviceId, cusAcc, bu) {
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = userId; dataJSON.key = key; dataJSON.serviceId = serviceId; dataJSON.consumerNo = cusAcc; dataJSON.buCode = bu;
    AjaxCall("POST", "/dhs/getMSEBCusDetails", dataJSON, MSEBVerificationCallBack);//call api  dashboard/electricity
    return false;
}

function MSEBVerificationCallBack(resObj) {
    var html = '';
    try {
        if (resObj != null & resObj != "") {
            if (new Date(resObj.dueDate) > new Date()) { //check due date || should greater than today's date
                html = "<div class='alert alert-success'><div>Bill Amount :<span id='txtmsebElectAmount'>" + resObj.billAmount + "</span></div>";
                html += "<div>DueDate :<span id='spnDueDate'>" + resObj.dueDate + "</span></div>";
                html += "<div>BillMonth :" + resObj.billMonth + "</div>";
                html += "<div>ConsumptionUnits :" + resObj.consumptionUnits + "</div></div>";
                //$("#btnPayElectricity").removeClass('hidden');
                $(".reliance").hide();
                $(".cusdetails").hide();
                $(".paybill").removeClass('hidden').show().prop('required', true);
                //$(".paybill").removeClass('hidden');
                //$("#txtElectCusMobile").removeClass('hidden');
                //$("#btnMSEBVerify").addClass('hidden');

            }
            else {
                html = '<div class="alert alert-danger fade in msg-box"><div>Due date exceed !!</div><div>DueDate Was :' + resObj.dueDate + '</div></div>';
                $("#btnMSEBVerify").addClass('hidden');
            }
        }
        else {
            html = '<div class="alert alert-danger fade in msg-box"><div>Not valid Entry !!.</div></div>';

        }

    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in msg-box">Invalid data ! Check Input data.</div>';
        //$("#btnMSEBVerify").addClass('hidden');
    }
    $("#customerMSEBStatus").show().html(html);
    $("#loding_Model").hide();//hide loading image
}

//Torrent Power
function TorrentBillPayment() {
    $(".torrentPower").prop('required', true);
    if ($("#txtMobileNo").val() != "" && parseFloat($("#txtBillAmount").val()) > 0) {
        TorrentVerification($("#txtMobileNo").val(), $("#drptorrentpwcity").val(), $("#txtServiceNo").val(), $("#txtBillAmount").val());
        return true;
    }
    else {
        //stmt 
        //alert("Enter Correct values");
        return false;
    }
}

function TorrentVerification(cusMob, Bu, cusAcc, amount) {
    //$("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = USERID; dataJSON.key = KEY; dataJSON.cusMob = cusMob; dataJSON.Bu = Bu; dataJSON.cusAcc = cusAcc; dataJSON.amount = amount;
    AjaxCall("POST", "/dhs/torrentPower", dataJSON, TorrentVerificationCallBack);//call api  dashboard/electricity
    return false;
}

function TorrentVerificationCallBack(torrentObj) {
    var html = '';
    var torrentStatus = '';

    try {
        if (torrentObj != null & torrentObj != "") {
            html = "<div class='alert alert-success'><div><span id='txtMsg'> Bill Payment Request Successfully Accepted </span></div>";

            //            html = "<div class='alert alert-success'><div>Avail Amount :<span id='txtCusAvailAmount'>" + torrentObj["AvaiBal"] + "</span></div>";
            //switch (torrentObj["Status"])
            //{
            //    case 1:
            //        torrentStatus = "Success";
            //        break;
            //    case 0:
            //        torrentStatus = "Failed";
            //        break;
            //    case 2:
            //        torrentStatus = "Pending";
            //        break;
            //    case 3:
            //        torrentStatus = "InProgress";
            //        break;
            //}
            //html = "<div>Status :<span id='txtCusStatus'>" + torrentStatus + " </span></div>";
            //  html += "<div>Message :" + torrentObj["Message"] + "</div>";

        }
        else {
            html = '<div class="alert alert-danger fade in msg-box"><div>Not valid Entry !!.</div></div>';
        }
    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in msg-box">Invalid data ! Check Input data.</div>';
    }
    $("#model_msg_body").html(html);
    $('#model_msg').modal('show');
}

function form_bank() {
    bankTransfer(USERID, KEY, $("#drpBankList").val(), $("#txtBankTransAmount").val());
}

//Electricity
function electricity_pay() {
    var serviceId = $("#drpElectricityBillOp").val();
    var cusAcc = $("#txtCustomerAccNo").val();
    var cyDiv = $("#txtElecCycleDivCode").val();
    var amount = $("#txtElectAmount").val();
    var cusMob = $("#txtElectCusMobile").val();
    var transType = 4;
    if (cusMob != '' || cusMob.length != 10) {
        if (serviceId == "40") { //MSEB || Just save 
            var bu = $("#drpBuElectricity").val();
            payElectricityBills(USERID, KEY, serviceId, cusAcc, bu, cusMob, $("#txtmsebElectAmount").text(), $("#spnDueDate").text());
        }
        else if (serviceId == "41")//recharge| Reliance
        {
            services(USERID, KEY, cusAcc, amount, serviceId, cyDiv);
        }
    }
    return false;

}


function form_recharge() {
    var sr_operatorId = $(opId).val();
    var sr_number = $("#sr_number").val();
    var sr_amount = $("#sr_amount").val();
    services(USERID, KEY, sr_number, sr_amount, sr_operatorId, "");
    return false;
}

//for electricity
function payElectricityBills(userId, key, serviceId, cusAcc, bu, cusMob, amount, dueDate) {
    // alert("kjlj");
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = userId; dataJSON.key = key; dataJSON.serviceId = serviceId; dataJSON.cusAcc = cusAcc; dataJSON.bU = bu;
    dataJSON.amount = amount; dataJSON.cusMob = cusMob; dataJSON.dueDate = dueDate;
    AjaxCall("POST", "/dashboard/electricity", dataJSON, payElectricityBillsCallBack);//call api  dashboard/electricity
    return false;
}

function payElectricityBillsCallBack(res) {
    try {
        var resObj = res;//eval('(' + eval('(' + jsonString.value + ')') + ')');
        var html = "";
        //Success
        if (resObj.status == 1) {
            // createCookie("cr_av_bal", resObj.avaiBal);
            //$("#cr_availBal").text(resObj.avaiBal);
            html = '<div class="alert alert-success"><h4>Successfully Accepted! .</h4>';
            //$("#drpElectricityBillOp").val("");
            //$("#txtCustomerAccNo").val("");
            //$("#drpBuElectricity").val("");
            //$("#txtElecCycleDivCode").val("");
            //$("#txtElectAmount").val("");
            //$("#txtElectCusMobile").val("");
            //$("#txtDueDate").val("");
            //$("#customerMSEBStatus").html("");
            //$("#btnPayElectricity").addClass("hidden");
        }
        else {
            if (resObj.status == 2)
                html = '<div class="alert alert-danger"> Insufficient Balance </div>';
            else
                html = '<div class="alert alert-danger">Some Error Occured!!</div>';
        }
        BALANCE = resObj.avaiBal;
        UpdateCrCookies();
    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in">Invalid data ! Check Input data.</div>';
    }
    $("#model_msg_body").html(html);
    $('#model_msg').modal('show');
    $("#loding_Model").hide();//hide loading image
}

//admin pop up message
function popUp() {
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    AjaxCall("GET", "dhs/adminPopup", dataJSON, popUpCallBack);//call api
}

//Admin Pop up call back for the ajax call.
function popUpCallBack(res) {
    try {
        if (res != "") {
            var resObj = res;
            html = resObj[0].text;
            var fDate = resObj[0].fDate;
            var tDate = resObj[0].tDate;
            $("#admin_popUpMsg").html(html);
            $('#notification_Model').modal('show');
        }
        else { $("#admin_popUpMsg").html("No Updates"); }
    }
    catch (ex) {
        console.log(ex.message);
    }
}

//Set the operator dropDown a/c to type
function setOperators(operatorType) {
    opId = "";
    //.prePaid 2, .postPaid 1, .dth 3, .insurance=7, .gasBill=6, .dataCard=9, .energy = 4, .landLine=5, .broadBand=8
    $(".select").addClass('hideMe');
    try {
        if (operatorType == 1) //filter
        {
            $("#post_operatorId ").removeClass('hideMe');

            opId = "#post_operatorId";
        }
        if (operatorType == 2) //filter
        {
            $("#pre_operatorId ").removeClass('hideMe');
            opId = "#pre_operatorId";
        }
        if (operatorType == 3) //filter
        {
            $("#dth_operatorId ").removeClass('hideMe');
            opId = "#dth_operatorId";
        }
        if (operatorType == 4) //filter
        {
            $("#energy_operatorId ").removeClass('hideMe');
            opId = "#energy_operatorId";
        }
        if (operatorType == 5) //filter
        {
            $("#landLine_operatorId").removeClass('hideMe');
            opId = "#landLine_operatorId";
        }
        if (operatorType == 6) //filter
        {
            $("#gasBill_operatorId").removeClass('hideMe');
            opId = "#gasBill_operatorId";

        }
        if (operatorType == 7) //filter
        {
            $("#insurance_operatorId ").removeClass('hideMe');
            opId = "#insurance_operatorId";
        }
        if (operatorType == 8) //filter
        {
            $("#broadBand_operatorId").removeClass('hideMe');
            opId = "#broadBand_operatorId";
        }
        if (operatorType == 9) //filter
        {
            $("#dataCard_operatorId").removeClass('hideMe');
            opId = "#dataCard_operatorId";
        }
    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in">Invalid data ! Check Input data.</div>';
        $("#model_msg_body").html(html);
        $("#model_msg").show();
    }

}

//Set the model pop-up values on services click
function setValues(obj) {
    //alert("rrr");

    var $headerLabel = $("#headerLabel");
    var $sr_operatorId = $("#sr_operatorId");
    var $sr_amount = $("#sr_amount");
    var $sr_amountLabel = $("#sr_amountLabel");
    var operatorType = "";
    //postpaid
    if (obj.id == "btn_post_Paid") {
        $("#hdnType").val("1");
        $headerLabel.text("Post-Paid");
        //$sr_operatorIdLabel.text("Operator Text");
        $sr_operatorId.appendTo();

        operatorType = 1;
    }
    else {
        //prepaid
        if (obj.id == "btn_pre_Paid") {
            $("#hdnType").val("2");
            $headerLabel.text("Pre-Paid");
            //$sr_operatorIdLabel.text("Operator Text");
            $sr_operatorId.appendTo();

            operatorType = 2;
        }
        else {
            //datacard
            if (obj.id == "btn_dataCard") {
                $headerLabel.text("Data Card");
                //$sr_operatorIdLabel.text("Operator Text");
                $sr_operatorId.appendTo();

                operatorType = 9;
            }
            else {
                if (obj.id == "btn_dth") {
                    $("#hdnType").val("3");
                    $headerLabel.text("DTH");
                    //$sr_operatorIdLabel.text("Operator Text");
                    $sr_operatorId.appendTo();

                    operatorType = 3;
                }
                else {
                    if (obj.id == "btn_insurance") {
                        $("#hdnType").val("7");
                        $headerLabel.text("Insurance");
                        //$sr_operatorIdLabel.text("Operator Text");
                        $sr_operatorId.appendTo();

                        operatorType = 7;
                    }
                    else {
                        if (obj.id == "btn_gasBill") {
                            $headerLabel.text("Gas Bill");
                            //$sr_operatorIdLabel.text("Operator Text");
                            $sr_operatorId.appendTo();

                            operatorType = 6;
                        }
                        else {
                            if (obj.id == "btn_broadBand") {
                                $("#hdnType").val("8");
                                $headerLabel.text("BroadBand");
                                //   $sr_operatorIdLabel.text("Operator Text");
                                $sr_operatorId.appendTo();

                                operatorType = 8;
                            }
                            else {
                                if (obj.id == "btn_electricityBill") {
                                    $("#hdnType").val("4");
                                    $headerLabel.text("Electricity Bill");
                                    //  $sr_operatorIdLabel.text("Operator Text");
                                    $sr_operatorId.appendTo();

                                    operatorType = 4;
                                }
                            }
                        }
                    }
                }
            }
        }


    }
    if (operatorType != "") {
        setOperators(operatorType);
        transType = operatorType;
    }
}

function myNavFunction(id) {
    $("#date-popover").hide();
    var nav = $("#" + id).data("navigation");
    var to = $("#" + id).data("to");
    console.log('nav ' + nav + ' to: ' + to.month + '/' + to.year);
}

//Get Operators || Ajax call
function operators() {
    var dataJSON = {};
    AjaxCall("GET", "/dhs/operators", dataJSON, OperatorsCallBack);//call api
}

//.........OperatorsCallBack for ajax
function OperatorsCallBack(jsonString) {
    try {
        var resObj = jsonString;
        for (var i in resObj) {
            if (resObj[i].serviceType == 1) //Postpaid
            {
                $("#post_operatorId").append("<option class='postPaid op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
            }
            else {
                if (resObj[i].serviceType == 2) //Prepaid
                {
                    $("#pre_operatorId").append("<option class='prePaid op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                }
                else {
                    if (resObj[i].serviceType == 3) //DTH
                    {
                        $("#dth_operatorId").append("<option class='dth op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                    }
                    else {
                        if (resObj[i].serviceType == 4) //Energy
                        {
                            $("#energy_operatorId").append("<option class='energy op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                        }
                        else {
                            if (resObj[i].serviceType == 5) //LandLine
                            {
                                $("#landLine_operatorId").append("<option class='landLine op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                            }
                            else {
                                if (resObj[i].serviceType == 6) //Gas Bill Pay
                                {
                                    $("#gasBill_operatorId").append("<option class='gasBill op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                                }
                                else {
                                    if (resObj[i].serviceType == 7) //Insurance
                                    {
                                        $("#insurance_operatorId").append("<option class='insurance op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                                    }
                                    else {
                                        if (resObj[i].serviceType == 8) //Broadband
                                        {
                                            $("#broadBand_operatorId").append("<option class='broadBand op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                                        }
                                        else (resObj[i].serviceType == 9) //DataCard
                                        {
                                            $("#dataCard_operatorId").append("<option class='dataCard op_hide' value=" + resObj[i].operaterId + ">" + resObj[i].operaterName + "</option>");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in">Invalid data ! Check Input data.</div>';
        $("#model_body").html(html);
    }
}


//Recharge || Ajax call
function services(userId, pass, number, amount, operatorId, account) {
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = userId; dataJSON.key = key; dataJSON.number = number; dataJSON.operatorId = operatorId; dataJSON.amount = amount;
    dataJSON.account = account; dataJSON.source = "1";//used for electricity
    AjaxCall("POST", "/dashboard/service", dataJSON, servicesCallBack);//call api
    return false;

}
// call function for services ajax call 
function servicesCallBack(jsonString) {
    try {
        var resObj = jsonString;//eval('(' + eval('(' + jsonString.value + ')') + ')');
        var html = "";
        //Success
        if (resObj.transId > 0 && resObj.statusCode == 1) {//availableBalance
            //$("#cr_availBal").text(resObj.availableBalance);
            html = '<div class="alert alert-success"><h3>Successfully Recharged! .</h3>';
            html += '<p>Amount :' + $("#sr_amount").val(); + ' for </p><p>Number :' + $("#sr_number").val() + ' </p></div>'
            $("#sr_number").val("");
            $("#sr_amount").val("");
        }
        else {
            if (resObj.statusCode == 2)
                html = '<div class="alert alert-danger"> Insufficient Balance </div>';
            else
                html = '<div class="alert alert-danger">' + resObj.message + '</div>';
            $("#sr_number").val("");
            $("#sr_amount").val("");
        }
        BALANCE = resObj.availableBalance;
        UpdateCrCookies();
    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in">Invalid data ! Check Input data.</div>';
    }
    $("#model_msg_body").html(html);
    $('#model_msg').modal('show');
    $("#loding_Model").hide();//hide loading image
}

//Bank Transfer   
function bankTransfer(userId, key, bankId, amount) {
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = userId; dataJSON.key = key; dataJSON.bankId = bankId; dataJSON.amount = amount;
    AjaxCall("POST", "/dashboard/bankReq", dataJSON, bankTransferCallBack);//call api
}
//Ajax call back for Transfer fund
function bankTransferCallBack(resObj) {
    try {
        var html = "";
        //Success
        if (parseInt(resObj.status) == 1) {//
            //createCookie("cr_av_bal", resObj.availableBalance);
            //$("#cr_availBal").text(resObj.availableBalance);
            html = '<div class="alert alert-success">Successfully Accepted !.Ref No .' + resObj.refId + '</div>';
        }

        else {
            if (parseInt(resObj.status) == 2) {
                html = '<div class="alert alert-danger">Not enough Balance</div>';

            }
            else {
                if (parseInt(resObj.status) == 3)
                    html = '<div class="alert alert-danger">Account Number not Exist with Crebit.</div>';
                else
                    html = '<div class="alert alert-danger">Some Error..Please try again later.</div>';
            }
        }
        BALANCE = resObj.availableBalance;
        UpdateCrCookies();
    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in">Invalid data ! Check Input data.</div>';
    }
    $("#formBankTrans").html(html);
}

//Get registered banks account for user
function getBanks() {
    $("#loding_Model").show();//show loading image
    var dataJSON = {};
    dataJSON.userId = USERID; dataJSON.Password = KEY;
    AjaxCall("POST", "/dhs/getBanks", dataJSON, getBanksCallBack);//call api
}

function getBanksCallBack(resobj) {
    try {
        var html = "";
        if (resobj.length > 0) {
            for (var i in resobj) {
                html += "<option value=" + resobj[i].id + ">" + resobj[i].bankAccNum + "</option>";
            }
        }
        else
            html = "<option value='-1'>--No Account registered --</option>";
        $("#drpBankList").html(html);
    }
    catch (ex) {
        console.log(ex.message);
    }
}

function setColorForRequiredField(obj) {
    $(obj).attr('required', true);
}