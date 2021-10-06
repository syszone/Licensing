$(function () {
    BindFeature();
});
$(document).on('click', "#btnSaveFeature", function () {
    
    var name = $('#txtName').val().trim();
    var value = $('#txtValue').val().trim();
    var featureId = $('#hdnFeatureId').val();

    if (name == '' || name == undefined) {
        alert('Please Enter Name');
        return;
    }    
    
        let obj = {
            id : featureId,
            name: name,
            value: value
        }

        $.ajax({
            url: "/Products/InsertUpdateFeature",
            type: "POST",
            data: obj,
            async: false,
            success: function (res) {
                if (res == 1) {
                    toastr.success('Product Feature Successfully Save');
                    $('#txtName').val("");
                    $('#txtValue').val("");
                    $('#hdnFeatureId').val("");
                    BindFeature();
                }
                else {
                    toastr.warning('Something Is Wrong Plesae Try Again!');
                }

            },
            error: function (err) {
                
                console.log(err);
            }
        })    
});




function BindFeature() {
    $("#tblFeatures").DataTable().destroy();
    $.ajax({
        url: "/Products/FeatureList",
        method: "GET",
        dataType: "json",
        success: function (result) {
            $("#tblFeatures").DataTable({
                "paging": true,

                data: result,

                "columns": [
                    
                    { "data": "name" },
                    { "data": "value" },
                    {
                        "render": function (data, type, row) {
                            return "<a href='javascript:void(0);' class='btn btn-sm btn-warning' onclick= 'return EditFeature(" + JSON.stringify(row) + ");' >Edit</a>" + " " +
                                "<a href='javascript:void(0);' class='btn btn-sm btn-danger' onclick=DeleteFeature('" + row.id + "'); >Delete</a>";
                        }
                    },
                ]
            });
        }
    });
}
function EditFeature(obj) {
    debugger;
    $("#hdnFeatureId").val(obj.id);
    $("#txtName").val(obj.name);
    $("#txtValue").val(obj.value);
}
function DeleteFeature(FeatureId) {
    if (confirm("Are you sure you want to delete this Feature?")) {
        $.ajax({
            url: "/Products/DeleteFeature",
            method: "POST",
            data: { featureId: FeatureId },
            dataType: "json",
            success: function (result) {
                if (result == 1) {
                    toastr.success("Product Feature Deleted Successfully!");
                    BindFeature();
                    

                }
                else {
                    toastr.error("Please contact administration!");
                }
            }
        });
    }
}
