window.app = new function () {
    $(".removeProduct").click(function () {

        var prdId = $(this).attr("id");

        var JSONObject = {
            "productId": prdId
        };

        $.ajax({
            type: "POST",
            url: "/Favorites/Remove",
            data: JSONObject,
            dataType: "json",
            success: function () {
                $("#" + prdId).fadeOut();
            }
        });
    });
}