window.app = new function () {

    $(".ratingStar").hover(function () {
        $(".ratingStar").addClass("fa-star-o").removeClass("fa-star");

        $(this).addClass("fa-star").removeClass("fa-star-o");
        $(this).prevAll(".ratingStar").addClass("fa-star").removeClass("fa-star-o");
    });

    $(".ratingStar").click(function () {
        var starRating = $(this).attr("dataValue");

        $("#ratingsValue").val(starRating);
    });

    $("#addCart").click(function () {

        var property1 = $("#productIdValue").val();
        var property2 = $("#sst").val();

        var JSONObject = {
            "ProductId": property1,
            "Quantity": property2
        };

        $.ajax({
            type: "POST",
            url: "/ShoppingCart/Add",
            data: JSON.stringify(JSONObject),
            success: function () {
                $(".successP").show().delay(2000).fadeOut();
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    });

    $("#addFav").click(function () {

        var property1 = $("#productIdValue").val();

        var JSONObject = {
            "productId": property1,
        };

        $.ajax({
            type: "POST",
            url: "/Favorites/Add",
            data: JSONObject,
            success: function (result) {
                if (result.success) {
                    $(".successFav").show().delay(2000).fadeOut();
                } else {
                    $(".failFav").show().delay(2000).fadeOut();
                }
            },
            dataType: "json"
        });
    });

    $("#loginFav").click(function () {
        $(".restrictFav").show().delay(2000).fadeOut();
    });

}