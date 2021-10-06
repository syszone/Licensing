$(function () {
    BindProduct();
   
});


function BindProduct() {
    $("#tblProducts").DataTable().destroy();
    $.ajax({
        url: "/Home/GetProductFromDatabase",
        method: "GET",
        dataType: "json",
        success: function (result) {
            $("#tblProducts").DataTable({
                "paging": true,
                data: result,

                "columns": [
                    {
                        "render": function (data, type, full, row) {
                            if (full.wcProductId > 0) {
                                return full.wcProductId;
                            }
                            return "";
                        }
                    },
                    { "data": "name" },
                    { "data": "description" },
                    { "data": "short_description" },
                    { "data": "regular_price" },
                    { "data": "sale_price" },
                    { "data": "licensetype" },
                    { "data": "durations" },
                    {
                        "render": function (data, type, full, row) {
                           
                            var editLink = "/Products/Create?productId=" + full.id + "&WCProductId=" + full.wcProductId;
                            var html = '<a class="btn btn-sm btn-warning" href="' + editLink + '"><i class="far fa-edit"></i></a>';

                            if (full.wcProductId == "0") {
                                html += "&nbsp;<a href='javascript:void(0);' class='btn btn-sm btn-primary' onclick='SyncToWooCommerce(" + full.id + ");'> <i class='fas fa-sync-alt'></i></a>";
                                 
                            }

                            return html;
                            
                        },
                    },
                ],
                "columnDefs": [
                    { "width": "100px", "targets": 8 }
                ]

            });
        }
    });
}

function SyncToWooCommerce(ProductId) {
  
    $.ajax({
        url: "/Products/SyncToWoocommerce?ProductId=" + ProductId,
        type: "GET",
        processData: false,
        contentType: false,
        success: function (res) {
            if (res.isSuccess == true) {
                toastr.success('Product  Successfully Save To Woocommerce');
                BindProduct();
            }
            else {
                toastr.warning('Something Is Wrong Plesae Try Again!');
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}


