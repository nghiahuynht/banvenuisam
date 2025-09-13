

function GenrateDataTableSearch(tableId, URL, columnsData, searchParams, displayLength = 20, sInfo="")
{
    // Language customization for DataTable
    var language = {
        "sProcessing": "Đang xử lý...",
        "sLengthMenu": "Hiển thị _MENU_ khách hàng",
        "sZeroRecords": "Không tìm thấy dữ liệu",
        "sInfo": "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ " + sInfo,
        "sInfoEmpty": "Hiển thị 0 đến 0 trong tổng số 0 khách hàng",
        "sInfoFiltered": "(được lọc từ _MAX_ khách hàng)",
        "sInfoPostFix": "",
        "sSearch": "Tìm kiếm:",
        "sUrl": "",
        "oPaginate": {
            "sFirst": "Đầu",
            "sPrevious": "Trước",
            "sNext": "Tiếp",
            "sLast": "Cuối"
        }
    };

    $(tableId).DataTable({
       
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "searching": false,
        "lengthChange": false,
        "paging": true,
        "iDisplayLength": displayLength,
        "columns": columnsData,
        "scrollY": "400px",
        "scrollX": true,
        "scrollCollapse": true,
        //"fixedColumns": {
        //    leftColumns: 4,
        //    rightColumns: 1
        //},

        "language": {
            "zeroRecords": "Không tìm thấy dữ liệu",
            "infoEmpty": "0/0 Kết quả",
            "sInfo": "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ " + sInfo,
            "sInfoEmpty": "Hiển thị 0 đến 0 trong tổng số 0 " + sInfo,
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Sau",
                "sLast": "Cuối"
            }
        },
        "ajax": {
            "url": URL,
            "type": "POST",
            //"headers": {
            //    'RequestVerificationToken': TokenHeaderValue()
            //},
            "data": searchParams
        },

        "destroy": true
    });

}

function ShowWaiting() {
    $("#popup-overlay").css("display", "block");
}
function HideWaiting() {
    $("#popup-overlay").css("display", "none");
}


function FormToObject(formName, suffixReplace) {
    var array = $(formName).serializeArray();
    var json = {};

    jQuery.each(array, function () {
        var nameobj = "";
        if (suffixReplace !== null && suffixReplace !== "") {
            nameobj = this.name.replace(suffixReplace, "");
        } else {
            nameobj = this.name;
        }
        json[nameobj] = this.value || '';
    });
    return json;
}

function AlertSuccess(contentMessage) {
    return "<span class='alertsuccess'><i class='glyphicon glyphicon-ok'></i>&nbsp;" + contentMessage + "</span>";
}
function AlertFail(contentMessage) {
    return "<span class='alertfail'><i class='glyphicon glyphicon-info-sign'></i>&nbsp;" + contentMessage + "</span>";
}


function ConverFormatDate(currentFormatDate) {
    var arr = currentFormatDate.split("/");
    var newFormatDate = arr[2] + "/" + arr[1] + "/" + arr[0];
    return newFormatDate;
}



function RenderDateComlumnGrid(date) {
    if (date != null && date != "") {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [day, month, year].join('/');
    } else {
        return "";
    }

}
function TokenHeaderValue()
{
    let tokenCSRF = document?.querySelector('token, input')?.getAttribute('value');
    return tokenCSRF;
}
function RenderNumerFormat(data) {
    var res = "";
    if (data !== undefined) {
        res = data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    }
    return "<div style='width:100%;text-align:right;'>" + res + "</div>";
}