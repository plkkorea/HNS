$(document).ready(function () {
    // global veriable declaration 
    var SelectedLcdList = [];

    var isVisitorExits = localStorage.getItem("visitortype");

    if (isVisitorExits) {
        $("#top").hide();
        if (isVisitorExits == "purchaser") {
            $(".main1_dev").hide();
            $(".main1_con").show();
        }
        else if (isVisitorExits == "developer") {
            $(".main1_con").hide();
            $(".main1_dev").show();
        }

    }
    else {
        var popMsg = $('#popup-msg');
        var top = $('#top');

        $(top).append(popMsg);
        $(popMsg).show();
        $("#top").show();
    }
    $("#purchasbtn, #purchaser_sub, #purchaser_sub1, #purchaserTab, .con, .ch_con"  ).on("click", function () {

        localStorage.setItem("visitortype", "purchaser");
        $(".main1_dev").hide();
        $(".main1_con").show();
        $("#top").slideUp(300);
        console.log("purchaser-clicked");
    });

    $("#developerbtn, #developer_sub, #developer_sub1, #developerTab, .dev, .ch_dev").on("click", function () {

        localStorage.setItem("visitortype", "developer");
        $(".main1_con").hide();
        $(".main1_dev").show();
        $("#top").slideUp(300);
        console.log("developer-clicked");
    });

    $("#popup-msg > a.close").on("click", function () {
        $(".main1_con").hide();
        $(".main1_dev").show();
        $("#top").slideUp(300);
    });

    $(".pageRefresh").on('click', function () {
        location.reload();
        console.log("--pageRefresh--");
    });

    //$(".bigtit").load(function () {
    //    $(".bigtit").each(function (i) {
    //        var len = $(this).text().trim().length;
    //        if (len > 100) {
    //            $(this).text($(this).text().substr(0, 100) + '...');
    //        }
    //    });
    //    console.log("--word--");
    //});
});



    
   // pagetitle
    //$("#_nav li a").on("click", fu)
    //var pagelocation = window.location.href;
    //if (pagelocation) {
    //    var pageTitleName = "";
    //    var hnsMessage = "";
    //    var page = pagelocation.split('/');
    //    var pageName = page[page.length - 1];
    //    var hnsMessage = page[page.length - 1];
    //    console.log(pageName);
    //    switch (pageName) {
    //        case "Greeting": pageTitleName = "인사말"; break;
    //        case "History": pageTitleName = "연혁"; break;
    //        case "Business": pageTitleName = "사업분야"; break;
    //        case "Contact": pageTitleName = "연락처및위치"; break;
    //        case "Application": pageTitleName = "제품 응용분야"; break;
    //        case "Lineup": pageTitleName = "제품 라인업"; break;
    //        case "Compare": pageTitleName = "제품 비교검색"; break;
    //        case "Iec1000": pageTitleName = "IEC1000-Series"; break;
    //        case "Iec667": pageTitleName = "IEC667-Series"; break;
    //        case "Iec266": pageTitleName = "IEC266-Series"; break;
    //        case "SmartIO": pageTitleName = "SmartI/O-Series"; break;
    //        case "Option": pageTitleName = "기타 옵션제품"; break;
    //        case "SmartxFramework": pageTitleName = "SmartX Framework"; break;
    //        case "VbTool": pageTitleName = "개발툴"; break;
    //        case "Guide": pageTitleName = "개발안내"; break;
    //        case "SmartX Framework": pageTitleName = "SmartX Framework"; break;
    //        case "UiDesign": pageTitleName = "UI 디자인"; break;
    //        case "Promotion": pageTitleName = "개발자 프로모션"; break;
    //        case "ProductRelated": pageTitleName = "제품관련"; break;
    //        case "SmartxRelated": pageTitleName = "SmartX Framework관련"; break;
    //        case "DrawingNApprover": pageTitleName = "도면 및 승인원"; break;
    //        case "TechNote": pageTitleName = "TECH NOTE"; break;
    //        case "ConnectedDevice": pageTitleName = "Connected Device"; break;
    //        case "Notice": pageTitleName = "공지사항"; break;
    //        case "FAQ": pageTitleName = "자주하는 질문"; break;
    //        case "QnA": pageTitleName = "Q&A"; break;
    //        case "AsInfo": pageTitleName = "A/S안내"; break;
    //        case "RemoteTechSupport": pageTitleName = "원격기술지원"; break;
    //        case "ProductPurchaseGuide": pageTitleName = "제품 구매안내"; break;
    //        case "Search": pageTitleName = "통합검색"; break;          
    //    }
    //    $("#pagetitle").html(pageTitleName);
    //    console.log(pageTitleName);
    //    switch (pageTitleName) {
    //        case "인사말": hnsMessage = "HNS Message"; break;
    //        case "연혁": pageTitleName = "HNS History"; break;
    //        case "사업분야": pageTitleName = "Field of Technology"; break;
    //        case "연락처및위치": pageTitleName = "Contact"; break;
    //        case "제품 응용분야": pageTitleName = "Applications"; break;
    //        case "제품 라인업": pageTitleName = "Lineup"; break;
    //        case "제품 비교검색": pageTitleName = "Product Comparison"; break;
    //        case "IEC1000-Series": pageTitleName = "IEC - Series"; break;
    //        case "IEC667-Series": pageTitleName = "IEC - Series"; break;
    //        case "IEC266-Series": pageTitleName = "IEC - Series"; break;
    //        case "SmartI/O-Series": pageTitleName = "IEC - Series"; break;
    //        case "SmartX Framework": pageTitleName = "Software"; break;
    //        case "UI 디자인": pageTitleName = "UI Design"; break;
    //        case "개발자 프로모션": pageTitleName = "Promotion for Developers"; break;
    //        case "제품관련": pageTitleName = "Product Related"; break;
    //        case "SmartX Framework관련": pageTitleName = "About SmartX Framework"; break;
    //        case "도면 및 승인원": pageTitleName = "Drawings and Approval"; break;
    //        case "TECH NOTE": pageTitleName = "Technical Notes"; break;
    //        case "Connected Device": pageTitleName = "Connected Devices"; break;
    //        case "공지사항": pageTitleName = "Notice Boards"; break;
    //        case "자주하는 질문": pageTitleName = "Frequently Asked Questions"; break;
    //        case "Q&A": pageTitleName = "Q&A"; break;
    //        case "A/S안내": pageTitleName = "After Service"; break;
    //        case "원격기술지원": pageTitleName = "Remote Technical Support"; break;
    //        case "제품 구매안내": pageTitleName = "Purchasing Guide"; break;
    //        case "통합검색": pageTitleName = "Search"; break;
    //    }

    //    $("#hnsMessage").html(hnsMessage);
    //}
    //console.log(pagelocation);
