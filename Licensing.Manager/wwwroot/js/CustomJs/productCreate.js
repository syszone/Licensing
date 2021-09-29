$(document).ready(function () {
    Categories();
   // ProductFromCategory();

});

function Categories() {
    $('#ddlCategory').empty();
    $.ajax({
        url: "/Products/GetCategories",
        type: "GET",
        dataType: "json",
        async: false,
        success: function (res) {
            $("#ddlCategory").append("<option selected value=''>Select Category</option>");
            $.each(res, function (data, value) {
                $("#ddlCategory").append($("<option></option>").val(value.categoryId).html(value.categoryName));
            })
            BindProducts();
        },
        error: function (err) {
            console.log(err);
        }
    })
}
 
$(document).on('change', '#ddlCategory', function () {
    BindProducts();
});

function BindProducts() {
    var categoryId = $('#ddlCategory').val();
    $('#ddlProduct').empty();
    $.ajax({
        url: "/Products/ProductFromCategory?categoryId=" + categoryId,
        type: "POST",
        dataType: "json",
        async: false,
        success: function (res) {
            $("#ddlProduct").append("<option selected value=''>Select Product</option>");
            $.each(res, function (data, value) {
                $("#ddlProduct").append($("<option></option>").val(value.productId).html(value.name));
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
}

$(document).on("click", "#btnSave", function () {

   var productId= $('#ddlProduct').val();
   var categoryId= $('#ddlCategory').val();
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