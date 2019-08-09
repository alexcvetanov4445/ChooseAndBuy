﻿window.app = new function () {
    $(".deleteBtn").click(function () {

        var catId = $(this).attr("id");

        var JSONObject = {
            "id": catId
        };

        $.ajax({
            type: "POST",
            url: "/Administration/Categories/Delete",
            data: JSONObject,
            dataType: "json",
            success: function () {
                $("#tr-" + catId).fadeOut();
            }
        });
    });
}