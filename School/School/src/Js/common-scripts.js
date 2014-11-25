

////Ajax call for api
//function AjaxCall(type, url, dataJSON, callback) {
//    $.ajax({
//        type: type,//"POST",
//        url: url,//"/dashboard/balanceUse", //
//        async: true,
//        data: JSON.stringify(dataJSON),
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        success: function (response, httpObj) {
//            if (httpObj == 'success') {

//                var html = "";
//                // var jsonString = eval('(' + response + ')');
//                if (callback && typeof (callback) === "function") {
//                    callback(response);
//                }
//            }
//            else
//                alert("Error !! Check input data.");
//            $("#loding_Model").hide();//hide loading image
//        },
//        error: function (httpObj, textStatus) {
//            $("#loding_Model").hide();
//            alert("Not Valid Entry !!");
//            console.log("error");
//            console.log("ResponseText" + httpObj.responseText);
//            if (httpObj.status == 401) {
//                window.location.replace("/Login.htm");//dashboad page
//            }

//            // alert("Some Error Occured !. Please try again later.");
//        }
//    });
//}


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


var USERID = "";
var KEY = "";
//Validate cookies
function checkUser() {
    if (readCookie("_mteresa") != null) {
        var _mteresa = readCookie("_mteresa").split('~');
        if (_mteresa.length > 1) {
            USERID = _mteresa[0];
            KEY = _mteresa[1];
        }
    }
    else {
        eraseCookie("_mteresa");
        window.location.replace("/login.aspx");
    }
}

//update cookies values
function UpdateCrCookies() {
    try {
        eraseCookie("_mteresa");//erase cookies
        createCookie("_mteresa", USERID + "~" + KEY, 60);
    }
    catch (ex) { console.log(ex.message); }
}