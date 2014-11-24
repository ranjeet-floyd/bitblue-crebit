// bgIframs version 2.1.2
(function (a) { a.fn.bgiframe = (a.browser.msie && /msie 6\.0/i.test(navigator.userAgent) ? function (d) { d = a.extend({ top: "auto", left: "auto", width: "auto", height: "auto", opacity: true, src: "javascript:false;" }, d); var c = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="' + d.src + '"style="display:block;position:absolute;z-index:-1;' + (d.opacity !== false ? "filter:Alpha(Opacity='0');" : "") + "top:" + (d.top == "auto" ? "expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+'px')" : b(d.top)) + ";left:" + (d.left == "auto" ? "expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+'px')" : b(d.left)) + ";width:" + (d.width == "auto" ? "expression(this.parentNode.offsetWidth+'px')" : b(d.width)) + ";height:" + (d.height == "auto" ? "expression(this.parentNode.offsetHeight+'px')" : b(d.height)) + ';"/>'; return this.each(function () { if (a(this).children("iframe.bgiframe").length === 0) { this.insertBefore(document.createElement(c), this.firstChild) } }) } : function () { return this }); a.fn.bgIframe = a.fn.bgiframe; function b(c) { return c && c.constructor === Number ? c + "px" : c } })(jQuery);

/* Prepand grey box layout at page load */
$(document).ready(function () {
    var grayContent = "<div id='gb-overlay'></div>";
    grayContent += "<div id='gb-window'><div id='gb-inner'>";
    grayContent += "<div id='gb-head'><span id='gb-title'></span><a id='gb-close'><span class='sprite-fixed gbClose'></span></a><div class='clear'></div></div>";
    grayContent += "<div id='gb-content'></div></div>";
    grayContent += "</div>";

    $("body").prepend(grayContent);
});

var GB_DONE = false;
var GB_HEIGHT = 400;
var GB_WIDTH = 400;

function GB_show(caption, url, width, height, applyIframe, GB_Html) {
    try {
       
        GB_HEIGHT = height || 400;
        GB_WIDTH = width || 400;
 
        // show loading gif image if taking time in loading
        $("#loading").show();
        $("#gb-content").hide();

        if (!GB_DONE) {// append only once			
            $("#gb-close").click(GB_hide);
           // $("#gb-overlay").click(GB_hide);

            if (applyIframe) { // Apply iframe on demand				
                $("#gb-overlay").bgiframe();
            }

            GB_DONE = true;
        }
        
        $("#gb-title").html(caption);
        $("#gb-overlay").show().css({ height: $(document).height() + "px", opacity: "0.9" });
        $("#gb-window").show();
        positionGB();

        if (url != "#" && GB_Html == "") { // url available to load external page.
            $("#gb-content").load(url, loadingDone);            
        } else { // 
            $("#gb-content").html(GB_Html);
            loadingDone();
        }

    } catch (e) {
        alert(e);
    }
}

function getTopMargin(){   
    var topMargin = 0;
    var offsetHt = document.documentElement.offsetHeight;    
    if (GB_HEIGHT < offsetHt) {
        topMargin = (offsetHt - GB_HEIGHT) / 3;
    }   
    return topMargin;   
}

/*Hide GreyBox on Esc key press*/
$(document).keydown(function (e) {
    if (e.keyCode == "27") {
        GB_hide();
    }
});

/* As finished gray box loading */
function loadingDone() {
    $("#loading").hide();
    $("#gb-content").fadeIn(300);
}

/* hide GB loading */
function GB_hide() {
    $("#hrfgoback").remove();
    $("#gb-window,#gb-overlay").hide();
    $('#gb-title').removeAttr("title");
}

/* Position GB */
function positionGB() {
    var de = document.documentElement;
    var w = self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;

    var gbTop = getTopPos() + getTopMargin();
    
    $("#gb-window").css({ width: GB_WIDTH + "px", height: 'auto', left: ((w - GB_WIDTH) / 2) + "px", top: gbTop });
    $("#gb-content").css({ height: GB_HEIGHT + "px" });
}

function getTopPos() {
    return getTopResults(window.pageYOffset ? window.pageYOffset : 0, document.documentElement ? document.documentElement.scrollTop : 0, document.body ? document.body.scrollTop : 0);
}

function getTopResults(n_win, n_docel, n_body) {
    var n_result = n_win ? n_win : 0;
    if (n_docel && (!n_result || (n_result > n_docel)))
        n_result = n_docel;
    return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
}