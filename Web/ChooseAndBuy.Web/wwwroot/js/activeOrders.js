window.app = new function () {
    $(".deliverAction").click(function () {

        var ordId = $(this).attr("id");
        ordId = ordId.substring(14, ordId.length);

        var JSONObject = {
            "orderId": ordId
        };

        $.ajax({
            type: "POST",
            url: "/Administration/Orders/Deliver",
            data: JSONObject,
            dataType: "json",
            success: function () {
                $(".statusText-" + ordId).text("Delivered");
                $(this).addClass("disable");
                $("#cancelAction-" + ordId).addClass("disable");
                $("#deliverAction-" + ordId).addClass("disable");
                $("#return-" + ordId).fadeOut();

                var d = new Date();

                var dateString = d.getUTCDate().toString()
                    + '.' + (d.getUTCMonth() + 1).toString()
                    + '.' + d.getFullYear().toString();

                $("#deliveryDate-" + ordId).text(dateString);
            },
        });
    });

    $(".cancelAction").click(function () {

        var ordId = $(this).attr("id");
        ordId = ordId.substring(13, ordId.length);

        var JSONObject = {
            "orderId": ordId
        };

        $.ajax({
            type: "POST",
            url: "/Administration/Orders/Cancel",
            data: JSONObject,
            dataType: "json",
            success: function () {
                $(".statusText-" + ordId).text("Canceled");
                $("#cancelAction-" + ordId).addClass("disable");
                $("#deliverAction-" + ordId).addClass("disable");
                $("#deliveryDate-" + ordId).text("Canceled");
                $("#return-" + ordId).fadeOut();
            },
        });
    });
}