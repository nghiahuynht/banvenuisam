$(document).ready(function () {
    $("#bt-save").click(function () {
        SaveOrder();
    });


});
function SaveOrder() {
    var viewModel = {
        OrderId: 123,//$("#OrderId").val(),
        OrderCode: "123",//$("#OrderCode").val(),
        TotalVAT: 10000,//$("#TotalVAT").val(),
        PhoneNumber: "0857727702",//$("#PhoneNumber").val(),
        CustomerCode: "KH001",//$("#CustomerCode").val(),
        PaymentDate: null,//$("#PaymentDate").val(),
        PaymentTypeFinal: null,//$("#PaymentTypeFinal").val(),
        PaymentStatus: false,//$("#PaymentStatus").val(),
    };
    debugger;
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        url: "/condao/CreateOrder",
        dataType: 'json',
        data: JSON.stringify(viewModel),
        beforeSend: function () {

        },
        success: function (result) {
            if (result.isSuccess
                && result.data != "DaThanhToan") {
                location.href = result.data;
            }
            else if (result.isSuccess
                && result.data == "DaThanhToan") {
                bootbox.alert(AlertSuccess("Đơn hàng đã thanh toán!"));
            }
            else {
                bootbox.alert(AlertSuccess("Đã xảy ra lỗi trong quá trình xử lý!"));
            }
        }

    });
}