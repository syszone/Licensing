
// Insert and Update
$(document).on('click', "#btnSaveFeature", function () {
    debugger;
    //alert('Save Feature');
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
                    location.reload();
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


function EditRole(FeatureId, Name, Value) {
    $("#hdnFeatureId").val(FeatureId);
    $("#txtName").val(Name);
    $("#txtValue").val(Value);
}
function DeleteRole(FeatureId) {
    if (confirm("Are you sure you want to delete this Feature?")) {
        $.ajax({
            url: "/Products/DeleteFeature",
            method: "POST",
            data: {featureId:FeatureId},
            dataType: "json",
            success: function (result) {
                if (result==1) {
                    toastr.success("Product Feature Deleted Successfully!");
                    
                    location.reload();
                     
                }
                else {
                    toastr.error("Please contact administration!");
                }
            }
        });
    }
}