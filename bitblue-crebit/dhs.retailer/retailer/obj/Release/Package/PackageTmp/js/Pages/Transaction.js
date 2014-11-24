var _CHECK_TYPEID = 0; //used for refund or status check

$(document).ready(function () {
    //  $("#loding_Model").show();//show loading image

    //Check User
    var userId = USERID;
    var key = KEY;
    var fromDate = "";
    var toDate = "";
    var statusId = -1;
    var typeId = -1;
    //Personal | EnterPrise user
    if (UTYPE == 2)
        $("#li_marginNav").addClass("hidden");//Hide margin list

    transactionSummary(userId, key, fromDate, toDate, statusId, typeId);//Load top 10 transactions
    $("#cr_name").text(NAME);
    $("#cr_availBal").text(BALANCE);

    $("#btnTransSumm").click(function () {
        $("#loding_Model").show();//show loading image
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        var statusId = $("#drpTransStatus").val();
        var typeId = $("#drpTransType").val();
        transactionSummary(userId, key, fromDate, toDate, statusId, typeId);
    });
    $("#searchTransNumber").click(function () {
        //alert("ssa");
        $("#loding_Model").show();//show loading image
        var searchTransNumber = $("#txtsearchTransNumber").val();
        transactionSearchByNumber(userId, key, searchTransNumber);
        return false;
    });

    $(".profit-information").fadeOut(15000);
});

//Transaction search by number
function transactionSearchByNumber(userId, key, searchTransNumber) {
    var dataJson = {};
    dataJson.userId = userId; dataJson.key = key; dataJson.value = searchTransNumber;
    AjaxCall("POST", "/dashboard/tranSearch", dataJson, transactionSearchByNumberCallBack);//call api

}

//call back on search button
function transactionSearchByNumberCallBack(res) {
    transactionSummaryCallBack(res);
}


//transaction summary || Ajax call
function transactionSummary(userId, key, fromDate, toDate, statusId, typeId) {
    $("#loding_Model").show();
    var dataJSON = {};
    dataJSON.userId = userId; dataJSON.key = key; dataJSON.fromDate = fromDate; dataJSON.toDate = toDate; dataJSON.statusId = statusId; dataJSON.typeId = typeId;
    AjaxCall("POST", "/dashboard/tranDetails", dataJSON, transactionSummaryCallBack);//call api
}

//Call back function for ajax call Transactions
function transactionSummaryCallBack(res) {
    var html = "";
    var status = "Fail";
    var srNum = 0;
    
    try {

        $("#spnTransTotalAmount").text(res.totalAmount);
        $("#spnTransTotalProfit").text(res.totalProfit);
        var resObj = res.dL_TransactionReturns;
        if (resObj.length > 0) {
            var options = {
                weekday: "long", year: "numeric", month: "short",
                day: "numeric", hour: "2-digit", minute: "2-digit"
            };
            var profit = "";
            var currBal = "";
            var charge = "";
            for (var i in resObj) {
                if (resObj[i].operaterId == 1400) { //crebit refund req.
                    profit = "--";
                    currBal = resObj[i].cBalance;
                    charge = "--";
                }
                else {
                    if (resObj[i].operaterId == 1200) {//crebit monthly charge
                        profit = "--";
                        charge = resObj[i].charge;
                    }
                    else {
                        profit = resObj[i].profit;
                        currBal = resObj[i].cBalance;
                        charge = resObj[i].charge;
                    }
                }


                srNum = srNum + 1;
                html += ' <tr> <td class="numeric text-center" data-title="#">' + srNum + '</td>';
                html += '<td class="numeric" data-title="Amount">' + resObj[i].amount + '</td>';
                html += '<td data-title="Status">' + resObj[i].status + '</td>';
                html += '<td class="numeric" data-title="Number">' + resObj[i].source + '</td>';
                html += '<td class="" data-title="Operator">' + resObj[i].operaterName + '</td>';
                html += '<td class="" data-title="Profit">' + profit + '</td>';
                html += '<td class="" data-title="Charge">' + charge + '</td>';
                html += '<td class="" data-title="Current Balance">' + currBal + '</td>';
                var transDate = resObj[i].tDate.split('T');
                html += '<td class="" data-title="Date">' + new Date(resObj[i].tDate).toDateString() + " " + transDate[1].split(':')[0] + ":" + transDate[1].split(':')[1] + '</td>';
                if (resObj[i].status == "Success" && resObj[i].operaterId != 1200) {
                    html += '<td><div class="btn-group"><button id="refund_' + resObj[i].id + '_' + resObj[i].operaterId + '" class="btn btn-default dropdown-toggle" data-target="#model_TransStatus" data-toggle="modal" onclick="getTransVal(this)">Check Status</button></td>';
                    //html += '<td><div class="btn-group"><button class="btn btn-default dropdown-toggle" data-toggle="dropdown">Refund</button><ul class=" dropdown-menu" role="menu">';
                    //html += '<li><button class="btn-primary" data-target="#model_TransStatus">Refund Req</button></li><li><a class=" btn-primary" data-target="#model_TransStatus">Status Check</a></li><li><a class=" btn-primary" data-target="#model_TransStatus">Other</a></li></ul></div></td>';
                }
                else
                    html += '<td>&nbsp;</td>'
                html += "</tr>";
            }//profit
        }

        else //no data
            html = html = '<tr ><td colspan="8"><div class="alert alert-danger fade in textCenter">No Data found !!</div></td></tr>';

        $("#tbody_trans").html(html)

    }
    catch (ex) {
        console.log(ex.message);
        html = '<div class="alert alert-danger fade in textCenter">Invalid data ! Check Input data.</div>';
        $("#model_body").html(html);
    }
    $("#loding_Model").hide();
}

//Send Trans Id to model || On click refund button
function getTransVal(obj) {
    $("#hdnTransId").val(obj.id.split("_")[1]);
    var opId = obj.id.split("_")[2];
    if (opId > 1000) //disable check for recharge status 
        $("#btnStatusCheck_2").addClass('hidden');
    else
        $("#btnStatusCheck_2").removeClass('hidden');
    $(".msg").html(""); //reset msg content

}

//Set _CHECK_TYPEID a/c to button clicked || Used for refund or check status
function setTypeId(obj) {
    //alert(_CHECK_TYPEID);
    _CHECK_TYPEID = obj.id.split('_')[1];
}

//for refund req.| Check status
function form_refundTransStatus(obj) {
    //var check = confirm("Are You sure want to Send Refund Req.");
    //alert(_CHECK_TYPEID);
    $("#loding_Model").show();
    var Comments = $("#ft_Comments").val();
    var transId = $("#hdnTransId").val(); // $("#transId").val();
    var dataJSON = {};
    dataJSON.userId = USERID; dataJSON.key = KEY; dataJSON.typeId = _CHECK_TYPEID; dataJSON.Comments = Comments; dataJSON.transId = transId;
    AjaxCall("POST", "/dashboard/refundOrTransStatus", dataJSON, refundTransStatusCallBack);//call api
    return false;
}

//...............
function refundTransStatusCallBack(resObj) {
    try {
        if (resObj.typeId == "1")//refund req.
        {
            if (resObj.status == "1")//successfully placed
            {
                $("#refundOrStatusMsg").html("<div class='btn-success'>Sussfully sent Refund Req..You can track status in Transaction Summary</div>");
            }
            else {
                //error
                $("#refundOrStatusMsg").html("<div class='btn-danger'>Already appiled for Refund. </div>");
            }
        }
        else {
            if (resObj.typeId == "2")//check trans status
            {
               // if (resObj.status == "1") {
                    var html = "<div class='btn-success'>Status : <div> Operator Transaction Id : " + resObj.cybertransId + "</div>";
                    html += "<div> Message : " + resObj.message + "</div></div>";

                    $("#refundOrStatusMsg").html(html);
               // }

               // else { $("#refundOrStatusMsg").html("<div class='btn-danger'>" + resObj.message + " </div>"); }
            }
        }
    }
    catch (ex) { console.log(ex.message); $("#refundOrStatusMsg").html("<div class='btn-danger'>Error !!. Try again. </div>"); }
    $("#loding_Model").hide();
}
