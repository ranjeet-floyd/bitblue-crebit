

//Ajax call for api
function AjaxCall(type, url, dataJSON, callback) {
    $.ajax({
        type: type,//"POST",
        url: url,//"/dashboard/balanceUse", //
        async: true,
        data: JSON.stringify(dataJSON),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response, httpObj) {
            if (httpObj == 'success') {

                var html = "";
                // var jsonString = eval('(' + response + ')');
                if (callback && typeof (callback) === "function") {
                    callback(response);
                }
            }
            else
                alert("Error !! Check input data.");
            $("#loding_Model").hide();//hide loading image
        },
        error: function (httpObj, textStatus) {
            $("#loding_Model").hide();
            alert("Not Valid Entry !!");
            console.log("error");
            console.log("ResponseText" + httpObj.responseText);
            if (httpObj.status == 401) {
                window.location.replace("/Login.htm");//dashboad page
            }

            // alert("Some Error Occured !. Please try again later.");
        }
    });
}


function createCookie(name, value, min) {
    if (min) {
        var date = new Date();
        date.setTime(date.getTime() + (min * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

//GLOBAL VARIABLES
var NAME = "";
var BALANCE = "";
var USERID = "";
var KEY = "";
var UTYPE = "";
var NOTIFYPOP = "";
//Validate cookies
function checkUser() {
    if (readCookie("_crbt") != null) {
        var _crebit = readCookie("_crbt").split('~');
        if (_crebit.length > 1) {
            USERID = _crebit[0];
            KEY = _crebit[1];
            if (!isNaN(parseFloat(_crebit[2])))
                BALANCE = _crebit[2];
            else
                BALANCE = 0.00;

            NAME = _crebit[3];
            UTYPE = _crebit[4];
            NOTIFYPOP = _crebit[5];
            $("#cr_availBal").text(BALANCE);//update balance on refresh page
        }
        //var nameBalance = readCookie("cr_nm_bal").split('~');
        //BALANCE = nameBalance[0];
        //NAME = nameBalance[1];
    }
    else {
        eraseCookie("_crbt");
        //eraseCookie("cr_nm_bal");
        //eraseCookie("cr_notify");
        window.location.replace("/login.htm");
    }
}

//update cookies values
function UpdateCrCookies() {
    try {
        eraseCookie("_crbt");//erase cookies
        createCookie("_crbt", USERID + "~" + KEY + "~" + BALANCE + "~" + NAME + "~" + UTYPE + "~" + NOTIFYPOP, 30);
        //  $("#cr_name").text(NAME);
        $("#cr_availBal").text(BALANCE);
    }
    catch (ex) { console.log(ex.message); }
}