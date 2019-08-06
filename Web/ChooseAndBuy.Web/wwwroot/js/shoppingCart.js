window.app = new function () {

    $(".removeProduct").click(function () {

        var prdId = $(this).attr("id");
        var prdPrice = $(this).attr("accesskey");
        var totalPrice = $(".totalP").text().substr(1);

        var JSONObject = {
            "productId": prdId
        };

        $.ajax({
            type: "POST",
            url: "/ShoppingCart/Remove",
            data: JSONObject,
            dataType: "json",
            success: function () {
                $("#" + prdId).fadeOut();

                var newPrice = parseFloat(totalPrice) - parseFloat(prdPrice);

                $(".totalP").text("$" + newPrice.toFixed(2));

                if (newPrice == 0) {
                    $("#checkoutBtn").hide();
                }
            }
        });
    });

}