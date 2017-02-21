
function ScrollResponsive() {
    var ancho = $(window).width();
    var scroll = $(window).scrollTop();
    if (ancho < 984) {
        $(".CHijoR").css({ "top": "-"+(scroll + 50) + "px" });
    }
}

function VerHijos(id)
{	
    var vis = $("#" + id + "p").css("display").length;
    var men = $("#" + id + "p").css("height");
    var idss = id + "p";
    var ancho = $(window).width();
    
    if (ancho > 983) {
        
        $(".CHijo").slideUp();
        $(".MenuPadre").css({
            "background": "#2f4050 url(../images/hpV.png) no-repeat right center",
            "background-size": "10px 10px",
            "border-bottom": "0",
            "border-left": "0px"
        });
        if (!$("#" + id + "p").is(":visible")) {
            $("#" + id + "p").slideToggle();
            $("#" + id + "pa").css({
                "background": "url(../images/hpV2.png) no-repeat right center",
                "background-size": "10px 10px",
                "border-bottom": "0",
                "border-left": "6px solid #23c6c8"
            });
        }
        foot()

    } else {
        var scroll = $(document).scrollTop();
        $(".CHijo").slideUp();
        $(".MenuPadre").css({
            "background": "#2f4050 url(../images/hpV.png) no-repeat right center",
            "background-size": "10px 10px",
            "border-bottom": "0",
            "border-left": "0px"
        });
        if (!$("#" + id + "p").is(":visible")) {
            $("#" + id + "p").css({ "display": "block" });
            $("#" + id + "pa").css({
                "background": "#282828",
                "background-size": "10px 10px",
                "border-bottom": "0",
                "border-left": "6px solid #23c6c8",
                "border-right": "0"
            });
        }
        
        foot()
    }
    
}




		