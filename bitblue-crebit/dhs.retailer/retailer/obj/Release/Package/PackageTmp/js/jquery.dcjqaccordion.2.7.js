

(function ($) {

    $.fn.dcAccordion = function (options) {

        //set default options 
        var defaults = {
            classParent: 'dcjq-parent',
            classActive: 'active',
            classArrow: 'dcjq-icon',
            classCount: 'dcjq-count',
            classExpand: 'dcjq-current-parent',
            eventType: 'click',
            hoverDelay: 300,
            menuClose: true,
            autoClose: true,
            autoExpand: false,
            speed: 'slow',
            saveState: true,
            disableLink: true,
            showCount: false
            //			cookie	: 'dcjq-accordion'
        };

        //call in the default otions
        var options = $.extend(defaults, options);

        this.each(function (options) {

            var obj = this;
            setUpAccordion();
            //			if(defaults.saveState == true){
            //				checkCookie(defaults.cookie, obj);
            //			}
            if (defaults.autoExpand == true) {
                $('li.' + defaults.classExpand + ' > a').addClass(defaults.classActive);
            }
            resetAccordion();

            if (defaults.eventType == 'hover') {

                var config = {
                    sensitivity: 2, // number = sensitivity threshold (must be 1 or higher)
                    interval: defaults.hoverDelay, // number = milliseconds for onMouseOver polling interval
                    over: linkOver, // function = onMouseOver callback (REQUIRED)
                    timeout: defaults.hoverDelay, // number = milliseconds delay before onMouseOut
                    out: linkOut // function = onMouseOut callback (REQUIRED)
                };

                $('li a', obj).hoverIntent(config);
                var configMenu = {
                    sensitivity: 2, // number = sensitivity threshold (must be 1 or higher)
                    interval: 1000, // number = milliseconds for onMouseOver polling interval
                    over: menuOver, // function = onMouseOver callback (REQUIRED)
                    timeout: 1000, // number = milliseconds delay before onMouseOut
                    out: menuOut // function = onMouseOut callback (REQUIRED)
                };

                $(obj).hoverIntent(configMenu);

                // Disable parent links
                if (defaults.disableLink == true) {

                    $('li a', obj).click(function (e) {
                        if ($(this).siblings('ul').length > 0) {
                            e.preventDefault();
                        }
                    });
                }

            } else {

                $('li a', obj).click(function (e) {

                    $activeLi = $(this).parent('li');
                    $parentsLi = $activeLi.parents('li');
                    $parentsUl = $activeLi.parents('ul');

                    // Prevent browsing to link if has child links
                    if (defaults.disableLink == true) {
                        if ($(this).siblings('ul').length > 0) {
                            e.preventDefault();
                        }
                    }

                    // Auto close sibling menus
                    if (defaults.autoClose == true) {
                        autoCloseAccordion($parentsLi, $parentsUl);
                    }

                    if ($('> ul', $activeLi).is(':visible')) {
                        $('ul', $activeLi).slideUp(defaults.speed);
                        $('a', $activeLi).removeClass(defaults.classActive);
                    } else {
                        $(this).siblings('ul').slideToggle(defaults.speed);
                        $('> a', $activeLi).addClass(defaults.classActive);
                    }

                    //					// Write cookie if save state is on
                    //					if(defaults.saveState == true){
                    //						createCookie(defaults.cookie, obj);
                    //					}
                });
            }

            // Set up accordion
            function setUpAccordion() {

                $arrow = '<span class="' + defaults.classArrow + '"></span>';
                var classParentLi = defaults.classParent + '-li';
                $('> ul', obj).show();
                $('li', obj).each(function () {
                    if ($('> ul', this).length > 0) {
                        $(this).addClass(classParentLi);
                        $('> a', this).addClass(defaults.classParent).append($arrow);
                    }
                });
                $('> ul', obj).hide();
                if (defaults.showCount == true) {
                    $('li.' + classParentLi, obj).each(function () {
                        if (defaults.disableLink == true) {
                            var getCount = parseInt($('ul a:not(.' + defaults.classParent + ')', this).length);
                        } else {
                            var getCount = parseInt($('ul a', this).length);
                        }
                        $('> a', this).append(' <span class="' + defaults.classCount + '">' + getCount + '</span>');
                    });
                }
            }

            function linkOver() {

                $activeLi = $(this).parent('li');
                $parentsLi = $activeLi.parents('li');
                $parentsUl = $activeLi.parents('ul');

                // Auto close sibling menus
                if (defaults.autoClose == true) {
                    autoCloseAccordion($parentsLi, $parentsUl);

                }

                if ($('> ul', $activeLi).is(':visible')) {
                    $('ul', $activeLi).slideUp(defaults.speed);
                    $('a', $activeLi).removeClass(defaults.classActive);
                } else {
                    $(this).siblings('ul').slideToggle(defaults.speed);
                    $('> a', $activeLi).addClass(defaults.classActive);
                }

                // Write cookie if save state is on
                if (defaults.saveState == true) {
                    createCookie(defaults.cookie, obj);
                }
            }

            function linkOut() {
            }

            function menuOver() {
            }

            function menuOut() {

                if (defaults.menuClose == true) {
                    $('ul', obj).slideUp(defaults.speed);
                    // Reset active links
                    $('a', obj).removeClass(defaults.classActive);
                    createCookie(defaults.cookie, obj);
                }
            }

            // Auto-Close Open Menu Items
            function autoCloseAccordion($parentsLi, $parentsUl) {
                $('ul', obj).not($parentsUl).slideUp(defaults.speed);
                // Reset active links
                $('a', obj).removeClass(defaults.classActive);
                $('> a', $parentsLi).addClass(defaults.classActive);
            }
            // Reset accordion using active links
            function resetAccordion() {
                $('ul', obj).hide();
                $allActiveLi = $('a.' + defaults.classActive, obj);
                $allActiveLi.siblings('ul').show();
            }
        });
    };
})(jQuery);



///........................
(function ($) {

    /**
	* Set it up as an object under the jQuery namespace
	*/
    $.gritter = {};

    /**
	* Set up global options that the user can over-ride
	*/
    $.gritter.options = {
        position: '',
        class_name: '', // could be set to 'gritter-light' to use white notifications
        fade_in_speed: 'medium', // how fast notifications fade in
        fade_out_speed: 1000, // how fast the notices fade out
        time: 6000 // hang on the screen for...
    }

    /**
	* Add a gritter notification to the screen
	* @see Gritter#add();
	*/
    $.gritter.add = function (params) {

        try {
            return Gritter.add(params || {});
        } catch (e) {

            var err = 'Gritter Error: ' + e;
            (typeof (console) != 'undefined' && console.error) ?
				console.error(err, params) :
				alert(err);

        }

    }

    /**
	* Remove a gritter notification from the screen
	* @see Gritter#removeSpecific();
	*/
    $.gritter.remove = function (id, params) {
        Gritter.removeSpecific(id, params || {});
    }

    /**
	* Remove all notifications
	* @see Gritter#stop();
	*/
    $.gritter.removeAll = function (params) {
        Gritter.stop(params || {});
    }

    /**
	* Big fat Gritter object
	* @constructor (not really since its object literal)
	*/
    var Gritter = {

        // Public - options to over-ride with $.gritter.options in "add"
        position: '',
        fade_in_speed: '',
        fade_out_speed: '',
        time: '',

        // Private - no touchy the private parts
        _custom_timer: 0,
        _item_count: 0,
        _is_setup: 0,
        _tpl_close: '<div class="gritter-close"></div>',
        _tpl_title: '<span class="gritter-title">[[title]]</span>',
        _tpl_item: '<div id="gritter-item-[[number]]" class="gritter-item-wrapper [[item_class]]" style="display:none"><div class="gritter-top"></div><div class="gritter-item">[[close]][[image]]<div class="[[class_name]]">[[title]]<p>[[text]]</p></div><div style="clear:both"></div></div><div class="gritter-bottom"></div></div>',
        _tpl_wrap: '<div id="gritter-notice-wrapper"></div>',

        /**
		* Add a gritter notification to the screen
		* @param {Object} params The object that contains all the options for drawing the notification
		* @return {Integer} The specific numeric id to that gritter notification
		*/
        add: function (params) {
            // Handle straight text
            if (typeof (params) == 'string') {
                params = { text: params };
            }

            // We might have some issues if we don't have a title or text!
            if (!params.text) {
                throw 'You must supply "text" parameter.';
            }

            // Check the options and set them once
            if (!this._is_setup) {
                this._runSetup();
            }

            // Basics
            var title = params.title,
				text = params.text,
				image = params.image || '',
				sticky = params.sticky || false,
				item_class = params.class_name || $.gritter.options.class_name,
				position = $.gritter.options.position,
				time_alive = params.time || '';

            this._verifyWrapper();

            this._item_count++;
            var number = this._item_count,
				tmp = this._tpl_item;

            // Assign callbacks
            $(['before_open', 'after_open', 'before_close', 'after_close']).each(function (i, val) {
                Gritter['_' + val + '_' + number] = ($.isFunction(params[val])) ? params[val] : function () { }
            });

            // Reset
            this._custom_timer = 0;

            // A custom fade time set
            if (time_alive) {
                this._custom_timer = time_alive;
            }

            var image_str = (image != '') ? '<img src="' + image + '" class="gritter-image" />' : '',
				class_name = (image != '') ? 'gritter-with-image' : 'gritter-without-image';

            // String replacements on the template
            if (title) {
                title = this._str_replace('[[title]]', title, this._tpl_title);
            } else {
                title = '';
            }

            tmp = this._str_replace(
				['[[title]]', '[[text]]', '[[close]]', '[[image]]', '[[number]]', '[[class_name]]', '[[item_class]]'],
				[title, text, this._tpl_close, image_str, this._item_count, class_name, item_class], tmp
			);

            // If it's false, don't show another gritter message
            if (this['_before_open_' + number]() === false) {
                return false;
            }

            $('#gritter-notice-wrapper').addClass(position).append(tmp);

            var item = $('#gritter-item-' + this._item_count);

            item.fadeIn(this.fade_in_speed, function () {
                Gritter['_after_open_' + number]($(this));
            });

            if (!sticky) {
                this._setFadeTimer(item, number);
            }

            // Bind the hover/unhover states
            $(item).bind('mouseenter mouseleave', function (event) {
                if (event.type == 'mouseenter') {
                    if (!sticky) {
                        Gritter._restoreItemIfFading($(this), number);
                    }
                }
                else {
                    if (!sticky) {
                        Gritter._setFadeTimer($(this), number);
                    }
                }
                Gritter._hoverState($(this), event.type);
            });

            // Clicking (X) makes the perdy thing close
            $(item).find('.gritter-close').click(function () {
                Gritter.removeSpecific(number, {}, null, true);
            });

            return number;

        },

        /**
		* If we don't have any more gritter notifications, get rid of the wrapper using this check
		* @private
		* @param {Integer} unique_id The ID of the element that was just deleted, use it for a callback
		* @param {Object} e The jQuery element that we're going to perform the remove() action on
		* @param {Boolean} manual_close Did we close the gritter dialog with the (X) button
		*/
        _countRemoveWrapper: function (unique_id, e, manual_close) {

            // Remove it then run the callback function
            e.remove();
            this['_after_close_' + unique_id](e, manual_close);

            // Check if the wrapper is empty, if it is.. remove the wrapper
            if ($('.gritter-item-wrapper').length == 0) {
                $('#gritter-notice-wrapper').remove();
            }

        },

        /**
		* Fade out an element after it's been on the screen for x amount of time
		* @private
		* @param {Object} e The jQuery element to get rid of
		* @param {Integer} unique_id The id of the element to remove
		* @param {Object} params An optional list of params to set fade speeds etc.
		* @param {Boolean} unbind_events Unbind the mouseenter/mouseleave events if they click (X)
		*/
        _fade: function (e, unique_id, params, unbind_events) {

            var params = params || {},
				fade = (typeof (params.fade) != 'undefined') ? params.fade : true,
				fade_out_speed = params.speed || this.fade_out_speed,
				manual_close = unbind_events;

            this['_before_close_' + unique_id](e, manual_close);

            // If this is true, then we are coming from clicking the (X)
            if (unbind_events) {
                e.unbind('mouseenter mouseleave');
            }

            // Fade it out or remove it
            if (fade) {

                e.animate({
                    opacity: 0
                }, fade_out_speed, function () {
                    e.animate({ height: 0 }, 300, function () {
                        Gritter._countRemoveWrapper(unique_id, e, manual_close);
                    })
                })

            }
            else {

                this._countRemoveWrapper(unique_id, e);

            }

        },

        /**
		* Perform actions based on the type of bind (mouseenter, mouseleave) 
		* @private
		* @param {Object} e The jQuery element
		* @param {String} type The type of action we're performing: mouseenter or mouseleave
		*/
        _hoverState: function (e, type) {

            // Change the border styles and add the (X) close button when you hover
            if (type == 'mouseenter') {

                e.addClass('hover');

                // Show close button
                e.find('.gritter-close').show();

            }
                // Remove the border styles and hide (X) close button when you mouse out
            else {

                e.removeClass('hover');

                // Hide close button
                e.find('.gritter-close').hide();

            }

        },

        /**
		* Remove a specific notification based on an ID
		* @param {Integer} unique_id The ID used to delete a specific notification
		* @param {Object} params A set of options passed in to determine how to get rid of it
		* @param {Object} e The jQuery element that we're "fading" then removing
		* @param {Boolean} unbind_events If we clicked on the (X) we set this to true to unbind mouseenter/mouseleave
		*/
        removeSpecific: function (unique_id, params, e, unbind_events) {

            if (!e) {
                var e = $('#gritter-item-' + unique_id);
            }

            // We set the fourth param to let the _fade function know to 
            // unbind the "mouseleave" event.  Once you click (X) there's no going back!
            this._fade(e, unique_id, params || {}, unbind_events);

        },

        /**
		* If the item is fading out and we hover over it, restore it!
		* @private
		* @param {Object} e The HTML element to remove
		* @param {Integer} unique_id The ID of the element
		*/
        _restoreItemIfFading: function (e, unique_id) {

            clearTimeout(this['_int_id_' + unique_id]);
            e.stop().css({ opacity: '', height: '' });

        },

        /**
		* Setup the global options - only once
		* @private
		*/
        _runSetup: function () {

            for (opt in $.gritter.options) {
                this[opt] = $.gritter.options[opt];
            }
            this._is_setup = 1;

        },

        /**
		* Set the notification to fade out after a certain amount of time
		* @private
		* @param {Object} item The HTML element we're dealing with
		* @param {Integer} unique_id The ID of the element
		*/
        _setFadeTimer: function (e, unique_id) {

            var timer_str = (this._custom_timer) ? this._custom_timer : this.time;
            this['_int_id_' + unique_id] = setTimeout(function () {
                Gritter._fade(e, unique_id);
            }, timer_str);

        },

        /**
		* Bring everything to a halt
		* @param {Object} params A list of callback functions to pass when all notifications are removed
		*/
        stop: function (params) {

            // callbacks (if passed)
            var before_close = ($.isFunction(params.before_close)) ? params.before_close : function () { };
            var after_close = ($.isFunction(params.after_close)) ? params.after_close : function () { };

            var wrap = $('#gritter-notice-wrapper');
            before_close(wrap);
            wrap.fadeOut(function () {
                $(this).remove();
                after_close();
            });

        },

        /**
		* An extremely handy PHP function ported to JS, works well for templating
		* @private
		* @param {String/Array} search A list of things to search for
		* @param {String/Array} replace A list of things to replace the searches with
		* @return {String} sa The output
		*/
        _str_replace: function (search, replace, subject, count) {

            var i = 0, j = 0, temp = '', repl = '', sl = 0, fl = 0,
				f = [].concat(search),
				r = [].concat(replace),
				s = subject,
				ra = r instanceof Array, sa = s instanceof Array;
            s = [].concat(s);

            if (count) {
                this.window[count] = 0;
            }

            for (i = 0, sl = s.length; i < sl; i++) {

                if (s[i] === '') {
                    continue;
                }

                for (j = 0, fl = f.length; j < fl; j++) {

                    temp = s[i] + '';
                    repl = ra ? (r[j] !== undefined ? r[j] : '') : r[0];
                    s[i] = (temp).split(f[j]).join(repl);

                    if (count && s[i] !== temp) {
                        this.window[count] += (temp.length - s[i].length) / f[j].length;
                    }

                }
            }

            return sa ? s : s[0];

        },

        /**
		* A check to make sure we have something to wrap our notices with
		* @private
		*/
        _verifyWrapper: function () {

            if ($('#gritter-notice-wrapper').length == 0) {
                $('body').append(this._tpl_wrap);
            }

        }

    }

})(jQuery);


//...............Next.......................
var Gritter = function () {

    $('#add-sticky').click(function () {

        var unique_id = $.gritter.add({
            // (string | mandatory) the heading of the notification
            title: 'This is a Sticky Notice!',
            // (string | mandatory) the text inside the notification
            text: 'Hover me to enable the Close Button. This note also contains a link example. Thank you so much to try Dashgum. Developed by <a href="#" style="color:#FFD777">Alvarez.is</a>.',
            // (string | optional) the image to display on the left
            image: 'assets/img/ui-sam.jpg',
            // (bool | optional) if you want it to fade out on its own or just sit there
            sticky: true,
            // (int | optional) the time you want it to be alive for before fading out
            time: '',
            // (string | optional) the class name you want to apply to that specific message
            class_name: 'my-sticky-class'
        });

        // You can have it return a unique id, this can be used to manually remove it later using
        /*
         setTimeout(function(){

         $.gritter.remove(unique_id, {
         fade: true,
         speed: 'slow'
         });

         }, 6000)
         */

        return false;

    });

    $('#add-regular').click(function () {

        $.gritter.add({
            // (string | mandatory) the heading of the notification
            title: 'This is a Regular Notice!',
            // (string | mandatory) the text inside the notification
            text: 'This will fade out after a certain amount of time. This note also contains a link example. Thank you so much to try Dashgum. Developed by <a href="#" style="color:#FFD777">Alvarez.is</a>.',
            // (string | optional) the image to display on the left
            image: 'assets/img/ui-sam.jpg',
            // (bool | optional) if you want it to fade out on its own or just sit there
            sticky: false,
            // (int | optional) the time you want it to be alive for before fading out
            time: ''
        });

        return false;

    });

    $('#add-max').click(function () {

        $.gritter.add({
            // (string | mandatory) the heading of the notification
            title: 'This is a notice with a max of 3 on screen at one time!',
            // (string | mandatory) the text inside the notification
            text: 'This will fade out after a certain amount of time. This note also contains a link example. Thank you so much to try Dashgum. Developed by <a href="#" style="color:#FFD777">Alvarez.is</a>.',
            // (string | optional) the image to display on the left
            image: 'assets/img/ui-sam.jpg',
            // (bool | optional) if you want it to fade out on its own or just sit there
            sticky: false,
            // (function) before the gritter notice is opened
            before_open: function () {
                if ($('.gritter-item-wrapper').length == 3) {
                    // Returning false prevents a new gritter from opening
                    return false;
                }
            }
        });

        return false;

    });

    $('#add-without-image').click(function () {

        $.gritter.add({
            // (string | mandatory) the heading of the notification
            title: 'This is a Notice Without an Image!',
            // (string | mandatory) the text inside the notification
            text: 'This will fade out after a certain amount of time. This note also contains a link example. Thank you so much to try Dashgum. Developed by <a href="#" style="color:#FFD777">Alvarez.is</a>.'
        });

        return false;
    });

    $('#add-gritter-light').click(function () {

        $.gritter.add({
            // (string | mandatory) the heading of the notification
            title: 'This is a Light Notification',
            // (string | mandatory) the text inside the notification
            text: 'Just add a "gritter-light" class_name to your $.gritter.add or globally to $.gritter.options.class_name',
            class_name: 'gritter-light'
        });

        return false;
    });

    $("#remove-all").click(function () {

        $.gritter.removeAll();
        return false;

    });
}();

//.........Nav Bar
/*---LEFT BAR ACCORDION----*/
$(function () {
    $('#nav-accordion').dcAccordion({
        eventType: 'click',
        autoClose: true,
        saveState: true,
        disableLink: true,
        speed: 'slow',
        showCount: false,
        autoExpand: true,
        //        cookie: 'dcjq-accordion-1',
        classExpand: 'dcjq-current-parent'
    });
});

var Script = function () {
    //    sidebar dropdown menu auto scrolling
    jQuery('#sidebar .sub-menu > a').click(function () {
        var o = ($(this).offset());
        diff = 250 - o.top;
        if (diff > 0)
            $("#sidebar").scrollTo("-=" + Math.abs(diff), 500);
        else
            $("#sidebar").scrollTo("+=" + Math.abs(diff), 500);
    });



    //    sidebar toggle

    $(function () {
        function responsiveView() {
            var wSize = $(window).width();
            if (wSize <= 768) {
                $('#container').addClass('sidebar-close');
                $('#sidebar > ul').hide();
            }

            if (wSize > 768) {
                $('#container').removeClass('sidebar-close');
                $('#sidebar > ul').show();
            }
        }
        $(window).on('load', responsiveView);
        $(window).on('resize', responsiveView);
    });

    $('.fa-bars').click(function () {
        if ($('#sidebar > ul').is(":visible") === true) {
            $('#main-content').css({
                'margin-left': '0px'
            });
            $('#sidebar').css({
                'margin-left': '-210px'
            });
            $('#sidebar > ul').hide();
            $("#container").addClass("sidebar-closed");
        } else {
            $('#main-content').css({
                'margin-left': '210px'
            });
            $('#sidebar > ul').show();
            $('#sidebar').css({
                'margin-left': '0'
            });
            $("#container").removeClass("sidebar-closed");
        }
    });

    //// custom scrollbar
    //    $("#sidebar").niceScroll({styler:"fb",cursorcolor:"#4ECDC4", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', spacebarenabled:false, cursorborder: ''});

    //    $("html").niceScroll({styler:"fb",cursorcolor:"#4ECDC4", cursorwidth: '6', cursorborderradius: '10px', background: '#404040', spacebarenabled:false,  cursorborder: '', zindex: '1000'});

    // widget tools

    jQuery('.panel .tools .fa-chevron-down').click(function () {
        var el = jQuery(this).parents(".panel").children(".panel-body");
        if (jQuery(this).hasClass("fa-chevron-down")) {
            jQuery(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
            el.slideUp(200);
        } else {
            jQuery(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
            el.slideDown(200);
        }
    });

    jQuery('.panel .tools .fa-times').click(function () {
        jQuery(this).parents(".panel").parent().remove();
    });


    //    tool tips

    $('.tooltips').tooltip();

    //    popovers

    $('.popovers').popover();



    // custom bar chart

    if ($(".custom-bar-chart")) {
        $(".bar").each(function () {
            var i = $(this).find(".value").html();
            $(this).find(".value").html("");
            $(this).find(".value").animate({
                height: i
            }, 2000)
        })
    }


}();
