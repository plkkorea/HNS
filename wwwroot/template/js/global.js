var ajaxbtn = {
    init: function () {
        if ($('._ajax-btn').length > 0) {
            this.action();
        }
    },
    action: function () {
        var spd = 500;
        
        $(document).on('click', '._ajax-btn', function () {
            var href = $(this).attr('href');
            var idx = $(this).data('idx');
            var type = $(this).data('type');

            if (type == undefined) {
                type = 'GET';
            }

            $.ajax({
                type: type,
                url: href,
                data: idx,
                success: function (data) {
                    $('body').find('._pop-ajax').remove().end().append(data).find('._pop-ajax').fadeIn(spd);
                }
            });
            return false;
        });

        $(document).on('click', '._pop-ajax ._bg, ._pop-ajax ._close', function () {
            $('._pop-ajax').fadeOut(spd, function () { $(this).remove() });
            return false;
        });

    }
}



var head = {
    init: function () {
        this.action();
    },
    action: function () {
        var wrap = $('#wrap');
        var a = $('#header');
        var offset = a.offset();

        var main = $('#wrap._main');
        var b = main.find('#header');

        var sub = $('#wrap._sub');
        var c = sub.find('#header');

        var open = a.find('._menu');
        var gnb = a.find('._gnb');
        var gnb_c = a.find('._gnb li ul');
        var gnb_a = $('._gnb > li > a');
        var nav = $('#_nav');
        var mgnb = nav.find('._gnb > li > a');
        var close = a.find('._menuoff');
        var check = a.find('.btn_gnb_chk');

        var snav = $('#_snav');
        var snav_offset = snav.offset();

        gnb.on('mouseenter', function () {
            if ($(window).width() > 1000) {
                a.addClass('nav-on');
            } else {
            }
        });

        a.on('mouseleave', function () {
            if ($(window).width() > 1000) {
                if (!check.is(':checked')) {
                    a.removeClass('nav-on');
                } else {

                }

            } else {

            }
        });

        open.on('click', function () {
            wrap.addClass('_mnav-on');
            nav.slideDown(500);
            return false;
        });

        close.on('click', function () {
            nav.slideUp(500);
            wrap.removeClass('_mnav-on');
            return false;
        });

        mgnb.on('click', function () {
            var par = $(this).closest('li');
            par.toggleClass('active').siblings().removeClass('active');
            return false;
        });

        //main          
        $(window).on('scroll', function () {
            if ($(window).scrollTop() > offset.top) {
                b.css('top', 0);
                b.css('position', 'fixed');
                nav.addClass('on');
            } else {
                b.css('position', 'absolute');
                nav.css('poistion', 'absolute');
                nav.removeClass('on');
            }
        });


        //sub        
        $(window).on('scroll', function () {
            // if($(window).width() > 1000){

            // } else {
            if ($(window).scrollTop() > offset.top) {
                c.css('top', 0);
                c.css('position', 'fixed');
                nav.addClass('on');
            } else {
                c.css('position', 'absolute');
                nav.css('poistion', 'absolute');
                nav.removeClass('on');
            }
            // }        
        });


        //sub_category 
        if (snav_offset != undefined) {
            $(window).on('scroll', function () {
                if ($(window).scrollTop() > snav_offset.top) {
                    snav.addClass('on');
                } else {
                    snav.removeClass('on');
                }
            });
        }

    }
}

//VISUAL
var vis = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('#_vis ._roll');
        roll.bxSlider({
            mode: 'fade',
            auto: true,
            useCSS: false,
            pause: 3000,
            speed: 1500,
            pager: true,
            controls: true,
            stopAutoOnClick: true,
            easing: 'easeInOutExpo'
        });
    }
}

//main_category
var category_roll = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.category_roll .roll');
        var slider = roll.bxSlider({
            auto: true,
            useCSS: false,
            pause: 3000,
            speed: 1500,
            pager: false,
            controls: true,
            stopAutoOnClick: true,
            easing: 'easeInOutExpo'
        });
        $('.ch_con').on('click', function () {
            $('.main1_dev').css('display', 'none');
            $('.main1_con').css('display', 'block');
            slider.reloadSlider();
        });
        $('.ch_dev').on('click', function () {
            $('.main1_con').css('display', 'none');
            $('.main1_dev').css('display', 'block');
            slider.reloadSlider();
        });
    }
}

//ex_category
var ex_box_list_slider;
var ex_box_list = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.ex_box_list .list4 .roll');
        ex_box_list_slider = roll.bxSlider({
            auto: true,
            useCSS: false,
            pause: 3000,
            speed: 1500,
            pager: false,
            controls: true,
            stopAutoOnClick: true,
            easing: 'easeInOutExpo'
        });

        //con-dev
        $('.ch_con').on('click', function () {
            $('.main1_dev').css('display', 'none');
            $('.main1_con').css('display', 'block');
            ex_box_list_slider.reloadSlider();
        });
        $('.ch_dev').on('click', function () {
            $('.main1_con').css('display', 'none');
            $('.main1_dev').css('display', 'block');
            ex_box_list_slider.reloadSlider();
        });

    }
}

//slider
var slider = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.sub23_slide .bxslider');
        var slider = roll.bxSlider({
            auto: true,
            useCSS: false,
            pause: 3000,
            speed: 1000,
            pagerCustom: '.bx-pager',
            controls: true,
            stopAutoOnClick: true,
            easing: 'easeOutCubic'
        });
    }
}

//compare
var slider1;
var compare_roll1_config = {
    auto: false,
    useCSS: false,
    pause: 3000,
    speed: 1000,
    pagerCustom: '.slide_pager1',
    controls: true,
    stopAutoOnClick: true,
    easing: 'easeOutCubic'
};
var compare_roll1 = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.sub23_slide .big1 .roll');
        //roll.bxSlider(compare_roll1_config).destroySlider();
        slider1 = roll.bxSlider(compare_roll1_config);
    }
}

//compare
var slider2;
var compare_roll2_config = {
    auto: false,
    useCSS: false,
    pause: 3000,
    speed: 1000,
    pagerCustom: '.slide_pager2',
    controls: true,
    stopAutoOnClick: true,
    easing: 'easeOutCubic'
};

var compare_roll2 = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.sub23_slide .big2 .roll');
        //roll.bxSlider(compare_roll2_config).destroySlider();
        slider2 = roll.bxSlider(compare_roll2_config);
    }
}


var slider3;
var compare_roll3_config = {
    auto: false,
    useCSS: false,
    pause: 1000,
    speed: 1000,
    pagerCustom: '.slide_pager3',
    controls: true,
    stopAutoOnClick: true,
    easing: 'easeOutCubic'
};
var compare_roll3 = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.big_overlayer1 .roll');
        slider3 = roll.bxSlider(compare_roll3_config);

        //big slide 1
        $('.big_modal1').on('click', function () {
            $('.big_wraplayer1').fadeIn(300);
            slider3.reloadSlider();
        });

        $('.big_overlayer1 .x_pop').on('click', function () {
            $('.big_wraplayer1').fadeOut(300);
        });

    }
}


var slider4;
var compare_roll4_config = {
    auto: false,
    useCSS: false,
    pause: 1000,
    speed: 1000,
    pagerCustom: '.slide_pager4',
    controls: true,
    stopAutoOnClick: true,
    easing: 'easeOutCubic'
};
var compare_roll4 = {
    init: function () {
        this.action();
    },
    action: function () {
        var roll = $('.big_overlayer2 .roll');
        slider4 = roll.bxSlider(compare_roll4_config);

        //big slide 2
        $('.big_modal2').on('click', function () {
            $('.big_wraplayer2').fadeIn(300);
            slider4.reloadSlider();
        });

        $('.big_overlayer2 .x_pop').on('click', function () {
            $('.big_wraplayer2').fadeOut(300);
        });

    }
}

$(function () {
    head.init();
    category_roll.init();
    ex_box_list.init();
    pro_data();
    fold1();
    product_intro();
    slide();

    $('.go_top').click(function () {
        $('body, html').animate({ scrollTop: 0 }, 500);
        return false;
    });

    $('.top_wrap .close').on('click', function () {
        $('.top_wrap').slideUp(500);
    });

    $('.autobtn').on('click', function () {
        if ($('.sch_autobox').css('display') == 'none') {
            $('.sch_autobox').css('display', 'block');
        } else {
            $('.sch_autobox').css('display', 'none');
        }
    });

    $('.sub71_btn .direct').on('click', function () {
        $('.print_wrap').css('display', 'block');
    });
    $('.print_x').on('click', function () {
        $('.print_wrap').css('display', 'none');
    });

    //mobile nav swipe
    $("#_svis ._inner").wipetouch({
        wipeLeft: function (result) { wipeStatus("basic", "LEFT", result); },
        wipeRight: function (result) { wipeStatus("basic", "RIGHT", result); }
    });

    //smartxframework
    var $smartBtn = $('.fold > dl dt'),
        smartSpd = '250',
        smartAni = 'easeInOutExpo';
    $smartBtn.on('click', function () {
        var $this = $(this),
            $smartA = $this.next('dd');
        $smartParent = $this.parents('dl');
        if ($smartA.is(':hidden')) {
            $smartA.slideDown(smartSpd, smartAni);
            $smartParent.addClass('over');
        } else {
            $smartA.slideUp(smartSpd, smartAni);
            $smartParent.removeClass('over')
        }
    });

    //bbs popup
    $('.go_pop').on('click', function () {
        $('.layer_wrap').fadeIn(300);
        $('.pop_layer .pop_pw').focus();
    });

    $('.pop_close').on('click', function () {
        $('.layer_wrap').fadeOut(300);
    });

    if ($(window).width() > 1000) {
        $('.pro_data .cnt').addClass('over');
        $('.pro_data .cnt dd').css('display', 'block');
    } else {

        $('#n4, #n5, #n6, #n7, #n8, #n9').removeClass('over');
        $('#n1,#n2,#n3').addClass('over');
        $('#n1 dd, #n2 dd, #n3 dd').css('display', 'block');
    }


    $('.sub_fold .top').on('click', function () {
        if ($(this).next('.bot').css('display') == 'none') {
            $(this).next('.bot').slideDown(300);
            $(this).parent().addClass('over');
        } else {
            $(this).next('.bot').slideUp(300);
            $(this).parent().removeClass('over')
        }
    });

    //con-dev
    $('.ch_con').on('click', function () {
        $('.main1_dev').css('display', 'none');
        $('.main1_con').css('display', 'block');
        $('.top_wrap .top_btn2').removeClass('active');
        $('.top_wrap .top_btn1').addClass('active');
    });

    $('.ch_dev').on('click', function () {
        $('.main1_con').css('display', 'none');
        $('.main1_dev').css('display', 'block');
        $('.top_wrap .top_btn1').removeClass('active');
        $('.top_wrap .top_btn2').addClass('active');
    });

    //smartxframework
    var $smartBtn = $('.smartxfold dl dt'),
        smartSpd = '250',
        smartAni = 'easeInOutExpo';
    $smartBtn.on('click', function () {
        var $this = $(this),
            $smartA = $this.next('dd');
        $smartParent = $this.parents('dl');
        if ($smartA.is(':hidden')) {
            $smartA.slideDown(smartSpd, smartAni);
            //$smartParent.addClass('over').siblings().removeClass('over');
            //$smartParent.siblings().removeClass('over').find('dd').not(':hidden').slideUp(smartSpd,smartAni);
        } else {
            $smartA.slideUp(smartSpd, smartAni);
            $smartParent.removeClass('over')
        }
    });

});

//mobile nav swipe
function wipeStatus(span, dir, result) {
    //alert(dir);
}

function slide(e) {
    var h = $('#header').height();
    var c = $('#_snav').height();
    var h_top = h + c + 20;
    var n;
    console.log(h);
    $('._scate li').on('click', function (e) {
        n = $(this).index();
        console.log(n);        
        e.preventDefault();

        if ($('#_snav').hasClass('on')) {
            $('html, body').stop().animate({
                scrollTop: $('.cnt').eq(n).offset().top - h_top
            });
        } else {
            $('html, body').stop().animate({
                scrollTop: $('.cnt').eq(n).offset().top - h_top - c
            });
        }
    })
}

function fileNameInput() {
    var fName1 = $('#file_add').val();
    $('#filename').val(fName1);
}

function pro_data() {
    var h = $('#header').height();
    var c = $('#_snav').height();
    var h_top = h + c + 20;
    
    $(window).on('scroll', function () {
       
        $('.pro_data .cnt').each(function () {
            if ($(window).scrollTop() > $(this).offset().top - h_top - c) {
                var id = $(this).attr('id');
                $('._scate li a').parent('li').removeClass('over');
                $('._scate li a[href=#' + id + ']').parent('li').addClass('over');
            }
        });
    });
}

function product_intro() {
    var h = $('#header').height();
    var c = $('#_snav').height();
    var h_top = h + c + 20;

    $(window).on('scroll', function () {
       
        $('.pro_intro .cnt').each(function () {
            if ($(window).scrollTop() > $(this).offset().top - h_top - c) {
                var id = $(this).attr('id');
                $('._scate li a').parent('li').removeClass('over');
                $('._scate li a[href=#' + id + ']').parent('li').addClass('over');

            }
        });
    });
}

function fold1() {
    //smartx_data
    var $smartBtn1 = $('.fold1 .fold_top'),
        smartSpd1 = '250',
        smartAni1 = 'easeInOutExpo';
    $smartBtn1.on('click', function () {
        var $this = $(this),
            $smartA1 = $this.next('.fold_bot');
        $smartParent1 = $this.parent('.fold_wrap');
        if ($smartA1.is(':hidden')) {
            $smartA1.slideDown(smartSpd1, smartAni1);
            $smartParent1.addClass('over1');
        } else {
            $smartA1.slideUp(smartSpd1, smartAni1);
            $smartParent1.removeClass('over1')
        }
    });
}

$(function () {

    $('body').on('click', '.popup', function () {
        var num = $(this).data('number');
        //console.log(num);        

        $('html, body').css('overflow', 'hidden');
        $('.wraplayer').fadeIn(300);
        $('.overlayer').hide();
        $('.overlayer' + num).show();

    });

    $('body').on('click', '.wraplayer .x_pop', function () {
        $('html, body').css('overflow', 'auto');
        $('.wraplayer').fadeOut(300);

    });


    //big slide
    $('body').on('click','.big_modal1', function () {
        $('.big_wraplayer1').fadeIn(300);
        slider3.reloadSlider();
    });

    $('body').on('click','.big_modal2', function () {
        $('.big_wraplayer2').fadeIn(300);
        slider4.reloadSlider();
    });

    $('body').on('click','.big_overlayer .x_pop', function () {
        $('.big_wraplayer').fadeOut(300);
    });

});



function smarxslide(n) {
    var h = $('#header').height();
    var c = $('#_snav').height();
    var h_top = h + c + 20;

    //alert(h_top)
    $('html, body').stop().animate({
        scrollTop: $('.cnt').eq(n).offset().top - h_top
    });
}

function checkIfNavOpen() {
    if (getCookieByName("NavOpen").length > 0) {
        $('#header').addClass('nav-on');
        $('#gnb_fix').prop('checked', true);
        return true;
    }
    else
        return false;
}

function addCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookieByName(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function delete_cookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

$(function () {
    checkIfNavOpen();

    $('body').on('click', '#gnb_fix', function () {
        if (!$(this).is(':checked')) {
            addCookie('NavOpen', '', -1);
        }
        else {
            addCookie('NavOpen', 'NavOpen', 100);
        }
    });
});

