window.app = new function () {
    $(".approveAction").click(function () {

        var ordId = $(this).attr("id");

        var JSONObject = {
            "orderId": ordId
        };

        $.ajax({
            type: "POST",
            url: "/Administration/Orders/Approve",
            data: JSONObject,
            dataType: "json",
            success: function () {
                $("#" + ordId).fadeOut();
            }
        });
    });
}