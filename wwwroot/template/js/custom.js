function highlightSearch(elementId, text) {
    var query = new RegExp("(\\b" + text + "\\b)", "gim");
    var e = document.getElementById(elementId).innerHTML;
    var enew = e.replace(/(<span>|<\/span>)/igm, "");
    document.getElementById(elementId).innerHTML = enew;
    var newe = enew.replace(query, "<span>$1</span>");
    document.getElementById(elementId).innerHTML = newe;
}

$(function () {
    // Get the modal
    var modal = document.getElementById('myModal');

    // Get the <span> element that closes the modal
    var span = $('.close'); //document.getElementsByClassName("close")[0];

    // When the user clicks on <span> (x), close the modal
    $('body').on('click', '.close', function () {
        $('#myModal').hide();
    })

    $('body').on('click', '.btn_popup_close', function () {
        if ($('.popup_checkbox input[type=checkbox]').is(':checked'))
            closePopupFor24Hrs();
        else
            $('#myModal').hide();
    })

    if ($('#myModal') != null && $('#myModal').length > 0 && shouldDisplayPopup()) {
        $('#myModal').show();
    }
    
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
});

function closePopupFor24Hrs() {
    var curDateTime = new Date();
    localStorage.setItem("popupMsg", curDateTime)
    //setCookie('popupMsg', curDateTime, 1);
    $('#myModal').hide();
}

function shouldDisplayPopup() { 
    //if (getCookie('popupMsg').length == 0)
    if (localStorage.getItem('popupMsg') == null)
        return true;
    else
        return false;
}


function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
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
