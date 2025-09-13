$(document).ready(function () {

    $(".date").datepicker({
        format: "dd/mm/yyyy"
    }).on('changeDate', function (e) {
        $(this).datepicker('hide');
    });


    //$.validator.addMethod(
    //    "checkPhone",
    //    function (value, element) {
    //        return phonenumber(value);
    //    },
    //    "Số điện thoại không hợp lệ!"
    //);
    //$.validator.addMethod(
    //    "checkTaxCode",
    //    function (value, element) {
    //        return checkTaxCode(value);
    //    },
    //    "MST phải là 10 hoặc 14 ký tự!"
    //);
    //$.validator.addMethod(
    //    "checkVisitDate",
    //    function (value, element) {
    //        return checkVisitDate(value);
    //    },
    //    "Ngày không hợp lệ!"
    //);
    $("#btn-cancel").click(function () {
        location.href = "/";
    });
    $("#btn-save").click(function () {
        var validForm = ValidInfoOrder();
        let validTien = validTotal();
        if (validForm)
        {
            if (validTien == "ok") {
                SaveOrder();
            } else {
                bootbox.alert(AlertFail("Đơn mua vé tổng tiền phải >= 3,000,000 VNĐ"));
            }
            
        }
        
    });

    $("#btn-plus-num").click(function () {
        let numQua1 = $("#quantiTicket").val();
        $("#quantiTicket").val(parseInt(numQua1)+1);
    });
    $("#btn-minus-num").click(function () {
        let numQua2 = $("#quantiTicket").val();
        if (parseInt(numQua2) >=0 )
        {
            $("#quantiTicket").val(parseInt(numQua2) - 1);
        }
        
    });

});
function SaveOrder() {
    
    var viewModel = {
        OrderId: parseFloat($("#Id").val()),
        TicketCode: $("#TicketCode").val(),
        Quantity: $("#quantiTicket").val(),
        PhoneNumber: $("#inp_Phone").val(),
        FullName: $("#inp_Fullname").val(),
        Email: $("#inp_Email").val(),
        AgencyName: $("#inp_AgencyName").val(),
        TaxCode: $("#inp_TaxCode").val(),
        TaxAddress: $("#inp_TaxAddress").val(),
        VisitDate: ConverFormatDate($("#VisitDate").val()),
        GateCode: $("#GateCode").val(),

    };
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        url: "/condao/SaveOrder",
        dataType: 'json',
        headers: {
            'RequestVerificationToken': TokenHeaderValue()
        },
        data: JSON.stringify(viewModel),
        beforeSend: function () {

        },
        success: function (result) {
            if (result.isSuccess
                && result.data > 0) {// tạo đơn hàng thành công
                location.href = `/system/paymentorder/${result.data}`;
            }
            else {
                bootbox.alert(AlertFail(result.Desc));
            }
        }

    });
}

function validTotal() {
    let quantiBuy = parseFloat($("#quantiTicket").val());
    let price = parseFloat($("#Price").val());
    let totalBuy = price * quantiBuy;
    let resValid = "ok";
    if (totalBuy < 3000000)
    {
        resValid = "error";
    }
    return resValid;
}


function ValidInfoOrder() {
    var resutlValid = $("#form-order").validate({
        rules: {
            "quantiTicket": "required",
            "inp_Fullname": "required",
            "inp_Phone": "required",
            "VisitDate": "required",
            //"inp_Phone": {
            //    required: true,
            //    checkPhone: true
            //},
            //"inp_Email": {
            //    email: true
            //},
            //"inp_TaxCode": {
            //    checkTaxCode: true
            //},
            //"visitDate": {
            //    checkVisitDate: true
            //},
        },
        messages: {
            "quantiTicket": "Thông tin bắt buộc!",
            "inp_Fullname": "Thông tin bắt buộc!",
            "inp_Phone": "Thông tin bắt buộc!",
            "inp_Email": "Thông tin bắt buộc!",
            "VisitDate": "Thông tin bắt buộc!",
            //"inp_Phone": {
            //    required: "Thông tin bắt buộc!",
            //    checkPhone: "Số điện thoại không hợp lệ!"
            //},
            //"inp_Email": {
            //    email: "Email không hợp lệ!"
            //},
            //"inp_TaxCode": {
            //    checkTaxCode: "MST phải là 10 hoặc 14 ký tự!"
            //},
            //"VisitDate": {
            //    checkVisitDate: "Thông tin bắt buộc!"
            //},
        }
    }).form();
    return resutlValid;
}
function phonenumber(inputtxt) {
    let isPhone = false;
    const phoneno = /^(08|09|03|05|07|02)\d{8}$/;
    if (inputtxt.match(phoneno))
        isPhone = true;
    return isPhone;
}
function checkTaxCode(inputtxt) {
    let isValid = false;
    if (inputtxt == "" || inputtxt.length == 10 || inputtxt.length == 14)
        isValid = true;
    return isValid;
}
function checkVisitDate(inputtxt) {
    let isvalidDate = Date.parse(inputtxt);
    if (isNaN(isvalidDate))
        return false;
    return true;
}
function ConverFormatDate(currentFormatDate) {
    var arr = currentFormatDate.split("/");
    var newFormatDate = arr[2] + "/" + arr[1] + "/" + arr[0];
    return newFormatDate;
}