window.app = new function () {
    $(".recommendAction").click(function () {

        var prdId = $(this).attr("id");

        var JSONObject = {
            "productId": prdId
        };

        $.ajax({
            type: "POST",
            url: "/Administration/Products/Recommend",
            data: JSONObject,
            dataType: "json",
            success: function () {
                if ($("#rcm-" + prdId).text() == "Yes") {
                    $("#rcm-" + prdId).text("No");
                    $(".recommendAction-" + prdId).removeClass("success").addClass("danger");
                } else {
                    $("#rcm-" + prdId).text("Yes");
                    $(".recommendAction-" + prdId).removeClass("danger").addClass("success");
                }
            }
        });
    });

    $(".deleteAction").click(function () {

        var prdId = $(this).attr("id");

        var JSONObject = {
            "productId": prdId
        };

        $.ajax({
            type: "POST",
            url: "/Administration/Products/Hide",
            data: JSONObject,
            dataType: "json",
            success: function () {
                var state = $(".deleteAction-" + prdId).text();

                if (state == "Hide") {
                    $(".deleteAction-" + prdId).text("Show").removeClass("danger").addClass("success");
                } else {
                    $(".deleteAction-" + prdId).text("Hide").removeClass("success").addClass("danger");
                }
            }
        });
    });
}