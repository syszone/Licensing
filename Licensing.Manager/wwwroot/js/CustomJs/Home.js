$(function () {
    BindProduct();
    //BindData(0);
});


function BindProduct() {

    $.ajax({
        url: "/Home/GetProductFromDatabase",
        method: "GET",
        dataType: "json",
        success: function (result) {
            $("#tblProducts").DataTable({
                data: result,

                "columns": [
                    { "data": "id" },
                    { "data": "name" },
                    { "data": "description" },
                    { "data": "short_description" },
                    { "data": "regular_price" },
                    { "data": "sale_price" },
                    { "data": "licensetype" },
                    { "data": "durations" },
                    {
                        "render": function (data, type, full, row) {
                            debugger;
                            var editLink = "/Products/Index?productId=" + full.id + "&WCProductId=" + full.wcProductId;
                            return '<a class="btn btn-sm btn-warning" href="' + editLink+'">Edit</a>' + " " +

                                "<a href='javascript:void(0);' class='btn btn-sm btn-danger' onclick=DeleteData('" + full.id + "'); >Delete</a>";
                        }
                    },
                ]
            });
        }
    });
}


