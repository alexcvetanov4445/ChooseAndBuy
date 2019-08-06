window.app = new function () {

    $(".addCart").click(function () {
        var property1 = $(this).attr("id");
        var property2 = 1;

        var JSONObject = {
            "ProductId": property1,
            "Quantity": property2
        };

        $.ajax({
            type: "POST",
            url: "/ShoppingCart/Add",
            data: JSON.stringify(JSONObject),
            success: function () {
                $("#successCart-" + property1).show().delay(2000).fadeOut();
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    });

    $(".addFav").click(function () {
        var property1 = $(this).attr("id");

        var JSONObject = {
            "productId": property1
        };

        $.ajax({
            type: "POST",
            url: "/Favorites/Add",
            data: JSONObject,
            success: function (result) {
                if (result.success) {
                    $("#successFav-" + property1).show().delay(2000).fadeOut();
                } else {
                    $("#errorFav-" + property1).show().delay(2000).fadeOut();
                }
            },
            dataType: "json"
        });
    });

    $(".FavDenied").click(function () {
        var property1 = $(this).attr("id");

        var JSONObject = {
            "productId": property1
        };

        $("#LoginFav-" + property1).show().delay(2000).fadeOut();
    });

    $(document).ready(function () {
        var num = $(".removeSpan").text().substr(1);
        console.log(num);
        $(".removeSpan").find("span").remove();
        $(".removeSpan").append("<a></a>");
        $(".removeSpan").find("a").text(num).addClass("active");
    });

    $("#Search").autocomplete({
        source: "/Products/Search",
        minLength: 3,

        select: function (event, ui) {
            window.location.href = ui.item.url;
        }
    });

}