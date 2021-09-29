$(document).ready(function () {
    BindProducts() 

});

 
 

function BindProducts() {
    
    $.ajax({
        url: "/Home/GetProductFromDatabase",
        type: "GET",
        dataType: "json",
        async: false,
        success: function (res) {
            $("#ddlProduct").append("<option selected value=''>Select Product</option>");
            $.each(res, function (data, value) {
                $("#ddlProduct").append($("<option></option>").val(value.id).html(value.name));
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
}

$(document).on("click", "#btnSave", function () {
    var productId = $('#ddlProduct').val();
    $.ajax({
        url: "/Products/SyncToWoocommerce?ProductId="+productId,
        type: "GET",
        processData: false,
        contentType: false,
        success: function (res) {
            if (res.isSuccess == true) {
                toastr.success('Product  Successfully Save To Woocommerce');
            }
            else {
                toastr.warning('Something Is Wrong Plesae Try Again!');
            }
        },
        error: function (err) {
            console.log(err);
        }
     });
});