var count = 0;
var DurationList = [];
var LicenseName = "";
var VariationList = [];
$(function () {

    $(document).on("click", "#hrefschedule", function () {
        $("#divsalepricedate").show();
        $("#hrefschedule").hide();
    });

    $(document).on("click", "#hrefcancel", function () {
        $("#divsalepricedate").hide();
        $("#hrefschedule").show();
    });

    $(document).on("change", "#chkenablestock", function () {
        if ($('#chkenablestock').is(':checked')) {
            $('#divstockquantity').show();
            $('#divstockstatus').hide();
        }
        else {
            $('#divstockquantity').hide();
            $('#divstockstatus').show();
        }
    });

    $(document).on("change", "#chkDownloadable", function () {

        if ($('#chkDownloadable').is(':checked')) {
            $("#divDownloadable").show();
        }
        else {
            $("#divDownloadable").hide();
        }
    });


    $('input[type=radio][name=imageOption]').change(function () {
        debugger;
        if ($("input:radio[name='imageOption']:checked").val() == 'AddLink') {
            $("#divchoosefiles").hide();
            $(".AddFiles").show();

        }
        if ($("input:radio[name='imageOption']:checked").val() == 'ChooseFile') {
            $("#divchoosefiles").show();
            $(".AddFiles").hide();
        }
    });
    BindFeatures();
    BindProductLicenseTypeDrodown();
    BindCategoryByParent(0, "Load");
    if (Number($("#hdnProductId").val()) > 0) {
        BindData();
    }
    appendaddFile();

});


$(document).on("click", "#btnSubmit", function () {
    debugger;
    var categoryList = [];

    $("#jstreeAllCategory").jstree('open_all');
    $("#jstreeAllCategory li").each(function () {
        var data = {};
        data.id = $(this).attr("id");
        data.name = $(this).text().trim();

        var ulChildern = $(this).children("ul[class='jstree-children']");
        if (ulChildern.length > 0) {
            data.selected = $(ulChildern).children("li").children("a").hasClass("jstree-clicked");
        } else {
            data.selected = $(this).children("a").hasClass("jstree-clicked") ? true : false;
        }
        if (data.selected) {
            categoryList.push({ "id": data.id, "name": data.name });
        }
    });

    var ProductDetails = new FormData();

    var productData = {};
    productData.id = $("#hdnProductId").val();
    productData.name = $("#productName").val();
    productData.type = "simple";
    productData.description = $("#txtproductDescription").val();
    productData.short_description = $("#txtproductshortDescription").val();
    productData.categories = categoryList;
    productData.regular_price = $("#txtrRegularPrice").val();
    productData.sale_price = $("#txtsalePrice").val();
    productData.tax_status = $("#ddltaxStatus").val();
    productData.tax_class = $("#ddlTaxClass").val();
    productData.sku = $("#txtsku").val();
    productData.manage_stock = $("#chkenablestock").is(":checked");
    productData.stock_quantity = $("#txtStockQuantity").val();
    productData.backorders_allowed = $("#ddlAllowbackorders").val() == "Do not Allow" ? false : true;
    productData.low_stock_amount = $("#txtlowstockthreshold").val() ? $("#txtlowstockthreshold").val() : "0";
    productData.sold_individually = $("#chksolIndividually").is(":checked");
    productData.purchase_note = $("#txtpurchasenote").val();
    productData.menu_order = $("#txtmenuorder").val();
    productData.FeaturesId = $("#ddlFeatures").val();


    if ($("#chkDownloadable").is(":checked")) {
        if ($('input[type=radio][name=imageOption]:checked').val() == 'ChooseFile') {
            //Variants.downloads = [];
            var fileInput = document.getElementById("DropdownloadableImage");
            for (var i = 0; i < fileInput.files.length; i++) {
                ProductDetails.append("Image-" + i, fileInput.files[i]);

            }

        }
        if ($('input[type=radio][name=imageOption]:checked').val() == 'AddLink') {
            var FilesList = [];
            $(".AddFiles .AddFile-Field").each(function (index, files) {
                var Files = {};
                Files.name = $(files).find(".txtfilename").val();
                Files.file = $(files).find(".txtfileurl").val();
                FilesList.push(Files);
            });
            productData.downloads = FilesList;
        }
        productData.downloadable = true;
        productData.download_limit = $("#txtDownloadLimit").val();
        productData.download_expiry = $("#txtdownloadExpiry").val();
    }
    else {
        productData.downloadable = false;
        productData.download_limit = "0";
        productData.download_expiry = "0";
    }

    //ProductDetails.append("Product", JSON.stringify(FilesList));
    ProductDetails.append("Product", JSON.stringify(productData));

    var durationDetails = new FormData();

    durationDetails.licensetype = $("#drpLicenseType").val();
    if ($("#drpLicenseType").val() == 1) {
        var TrialDurationList = [];
        TrialDurationList.push($("#ddlTrialDuration").val());
        durationDetails.durationid = TrialDurationList;
    }
    else if ($("#drpLicenseType").val() == 3) {
        durationDetails.durationid = $("#ddlDuration").val();
    }
    ProductDetails.append("License", JSON.stringify(durationDetails));

    var VariantList = [];
    $(".Variations .individual-field").each(function (index, variants) {
        var Variants = {};
        Variants.price = $(variants).find(".txtvariationsRegularPrice").val();
        Variants.regular_price = $(variants).find(".txtvariationsRegularPrice").val();
        Variants.sale_price = $(variants).find(".txtvariationsalePrice").val();
        Variants.description = $(variants).find(".txtvariantsDescription").val();

        Variants.attributes = [];
        var wcProduct = DurationList.filter(r => r.id == $(variants).find(".hdnAttribiteId").val());
        if (wcProduct.length > 0) {
            var attribute = {
                id: wcProduct[0].id,
                name: LicenseName,
                options: $(variants).find(".txtLicense").text()
            }
            Variants.attributes.push(attribute);
        }
        VariantList.push(Variants);
    });



    ProductDetails.append("ProductVariation", JSON.stringify(VariantList));

    $.ajax({
        url: "/Products/CreateProduct",
        type: "POST",
        data: ProductDetails,
        processData: false,
        contentType: false,
        success: function (res) {

            if (res.isSuccess == true) {
                toastr.success('Product  Successfully Created');
            }
            else {
                toastr.warning('Something Is Wrong Plesae Try Again!', { stayTime: 60000, });
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
});

function BindCategoryByParent(parent, type) {
    $.ajax({
        url: "/Products/GetCategoryByParent?parent=" + parent,
        type: "GET",
        dataType: "json",
        async: false,
        success: function (res) {
            var nodes = JSON.parse(res);
            if (type == "Load") {
                $("#jstreeAllCategory").jstree("destroy");
                $('#jstreeAllCategory').jstree({
                    'plugins': ["wholerow", "checkbox"],
                    'core': {
                        'data': nodes,
                        'check_callback': true
                    },
                    "checkbox": {
                        "keep_selected_style": false
                    }
                });

                $("#jstreeAllCategory").on(
                    "select_node.jstree", function (evt, data) {
                        BindCategoryByParent(data.node.id, "clicked");
                    }
                );
            }
            else {
                for (var i = 0; i < nodes.length; i++) {
                    $('#jstreeAllCategory').jstree('create_node', $("#" + parent), { "text": nodes[i].text, "id": nodes[i].id }, "last", false, false);
                }
            }
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function BindProductLicenseTypeDrodown() {
    $('#drpLicenseType').empty();
    $.ajax({
        url: "/Products/GetLicenseType",
        type: "GET",
        dataType: "json",
        async: false,
        success: function (res) {
            $("#drpLicenseType").html("");
            $("#drpLicenseType").append($("<option></option>").val("0").html("Select LicenseType"));
            $.each(res, function (data, value) {
                $("#drpLicenseType").append($("<option></option>").val(value.id).html(value.type));
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
}

$(document).on("change", "#drpLicenseType", function () {

    var Id = $(this).val();
    LicenseName = $("#drpLicenseType option:selected").text();

    if (Id == 1) {
        $("#divTrialDuration").show();
        $("#divStandard").hide();
        BindTrialDuration();
    }
    else if (Id == 3) {
        $("#divStandard").show();
        $("#divTrialDuration").hide();
        BindDuration();
    }
    else {
        $("#divStandard").hide();
        $("#divTrial").hide();
    }

});

function BindDuration() {
    $("#ddlDuration").select2({ placeholder: "Select Duration" });
    //debugger;
    var licenseId = $('#drpLicenseType').val();
    $.ajax({
        url: '/Products/GetLicenseTypeDuration?licenseId=' + licenseId,
        type: "GET",
        dataType: "json",
        success: function (res) {
            $("#ddlDuration").html("");
            DurationList = [];
            $.each(res, function (data, value) {
                DurationList.push({
                    id: value.id,
                    name: value.duration
                })
                $("#ddlDuration").append($("<option></option>").val(value.id).html(value.duration));
            })

        }
    })
}


function BindTrialDuration() {
    debugger;
    var licenseId = $('#drpLicenseType').val();
    $.ajax({
        url: '/Products/GetLicenseTypeDuration?licenseId=' + licenseId,
        type: "GET",
        dataType: "json",
        success: function (res) {
            $("#ddlTrialDuration").html("");
            $("#ddlTrialDuration").append($("<option selected value=''>Select Subscription</option>"));
            DurationList = [];
            $.each(res, function (data, value) {
                DurationList.push({
                    id: value.id,
                    name: value.duration
                })
                $("#ddlTrialDuration").append($("<option></option>").val(value.id).html(value.duration));
            })

        }
    });
}


$(document).on("change", "#ddlTrialDuration", function () {
    debugger;
    var Durations = $(this).val();
    $("#ddlVariationsDuration").empty();
    var name = DurationList.filter(r => r.id == Durations)[0];
    $("#ddlVariationsDuration").append("<option value='" + Durations + "'>" + name.name + "</option>");
});

$(document).on("change", "#ddlDuration", function () {

    var Durations = $(this).val();
    $("#ddlVariationsDuration").empty();
    for (var i = 0; i < Durations.length; i++) {
        var name = DurationList.filter(r => r.id == Durations[i])[0];
        $("#ddlVariationsDuration").append("<option value='" + Durations[i] + "'>" + name.name + "</option>");
    }

});

function showLoader() {
    $('#loaderDiv').show();
}

function hideLoader() {
    $('#loaderDiv').hide();
}

$(document).on("click", "#btnAddvariations", function () {
    var varList = VariationList.filter(el => $('#ddlVariationsDuration').val().includes(el));
    if (varList.length == 0) {
        appendDiv();
        VariationList.push($('#ddlVariationsDuration').val());
    }
});
function appendDiv() {
    count++;
    $(".field").clone().appendTo(".Variations");
    //$(".field p")[1].style.display = 'none';
    $(".Variations .field").show();
    //$(".Variations .field").find(".downloadableVariationsDiv").attr("id", "variations-" + count); 
    $(".Variations .field").find("#downloadableChk").attr("class", "downloadable-" + count);
    $(".Variations .field").find("#container").attr("class", "downloadable-" + count);
    $(".Variations .field").find("#downloableShow").attr("id", "downloadable-" + count);
    $(".Variations .field").find(".txtLicense").html($("#ddlVariationsDuration option:selected").text());
    $(".Variations .field").find(".hdnAttribiteId").val($("#ddlVariationsDuration").val());
    $(".Variations .field").removeClass("field");
}

function appendaddFile() {
    $(".fieldfile").clone().appendTo(".AddFiles");
    $(".AddFiles .fieldfile").show();
    $(".AddFiles .fieldfile").removeClass("fieldfile");
}

$(document).on("click", ".btnAddFile", function () {
    appendaddFile();
});

$(document).on("click", ".downloableShow", function () {
    if ($(this).find('i').hasClass('fa-angle-double-down')) {
        $(this).find('i').removeClass('fa-angle-double-down').addClass('fa-angle-double-right');
        var cls = '.' + $(this).attr("id");
        $(cls).hide();
    } else {
        $(this).find('i').removeClass('fa-angle-double-right').addClass('fa-angle-double-down');
        var cls = '.' + $(this).attr("id");
        $(cls).show();
    }
});


function BindFeatures() {
    $("#ddlFeatures").select2({
        placeholder: "Select Features",
        multiple: true,
    });
    $.ajax({
        url: '/Products/GetProductFeatures?id=0',
        type: "GET",
        dataType: "json",
        success: function (res) {
            $("#ddlFeatures").html("");
            DurationList = [];
            $.each(res, function (data, value) {
                $("#ddlFeatures").append($("<option></option>").val(value.id).html(value.name));
            })

        }
    })
}


function BindData() {
    var productId = $("#hdnProductId").val();

    $.ajax({
        url: '/Products/GetProductData?ProductId=' + productId,
        type: "GET",
        dataType: "json",
        success: function (res) {
            console.log(JSON.stringify(res))
            $("#productName").val(res.product.name);
            $("#txtproductDescription").val(res.product.description);
            $("#txtproductshortDescription").val(res.product.short_description);
            $("#txtrRegularPrice").val(res.product.regular_price);
            $("#txtsalePrice").val(res.product.sale_price);
            $("#txtsku").val(res.product.sku);
         
            if (res.product.downloadable == true) {
                $("#chkDownloadable").prop("checked", true).trigger("change");
                var variants = [];
                if (res.varients.length > 0) {
                    variants.push(res.varients[0])

                }
                for (var i = 0; i < variants.length; i++) {
                    var downloads = res.varients[i].downloads;
                    for (var j = 0; j < downloads.length; j++) {
                        $(".AddFiles .AddFile-Field:last").find(".txtfilename").val(res.varients[i].downloads[j].name);
                        $(".AddFiles .AddFile-Field:last").find(".txtfileurl").val(res.varients[i].downloads[j].file);
                        if (downloads.length > (j + 1)) {
                            $(".AddFiles .AddFile-Field:last").find(".btnAddFile").trigger("click");
                        }
                    }
                }

                $(".AddFiles .AddFile-Field:last").find("#txtDownloadLimit").val(variants[0].download_limit);
                $(".AddFiles .AddFile-Field:last").find("#txtdownloadExpiry").val(variants[0].download_expiry);
               



            }
            else {
                $("#chkDownloadable").prop("checked", false).trigger("change");
            }



            var featureList = res.features.split(",");
            $("#ddlFeatures").val(featureList);
            $("#ddlFeatures").select2({
                placeholder: "Select Features",
                multiple: true,
            });
            var categories = res.product.categories;
            for (let val of categories) {
                $(document).find("#" + val.id + " a").trigger("click");
            }
            $("#drpLicenseType").val(res.licenseType).trigger("change");

            setTimeout(function () {
                if (res.licenseType == 3) {
                    var durationsList = res.durations.split(",");
                    $("#ddlDuration").val(durationsList).trigger("change");
                    $("#ddlDuration").select2({
                        placeholder: "Select Duration",
                        multiple: true,
                    });
                }
                else if (res.licenseType == 1) {
                    $("#ddlTrialDuration").val(res.durations).trigger("change");
                }
                
                for (var i = 0; i < res.varients.length; i++) {
                    var attribute = res.varients[i].attributes[0];
                    $("#ddlVariationsDuration option").each(function (i, option) {

                        if (option.label == attribute.option) {
                            option.selected = "selected";
                        }
                    });
                    $("#btnAddvariations").trigger("click");
                    $(".Variations .individual-field:last").find(".txtvariationsRegularPrice").val(res.varients[i].regular_price);
                    $(".Variations .individual-field:last").find(".txtvariationsalePrice").val(res.varients[i].sale_price);
                    $(".Variations .individual-field:last").find(".txtvariantsDescription").val(res.varients[i].description);
                }

            }, 3000);


        }
    });
}

$(document).on("click", "#btnSync", function () {
    var ProductId = $("#hdnProductId").val();
    $.ajax({
        url: "/Products/SyncToWoocommerce?ProductId=" + ProductId,
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


$(document).on("click", ".deletevariant", function () {
    $(this).parent().parent().remove();
});