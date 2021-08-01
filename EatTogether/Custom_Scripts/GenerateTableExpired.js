$(document).ready(function () {
    $.ajax({
        url: '/SessionModel/GenerateExpiredSessionsTable',
        success: function (result) {
            $('#tableDiv3').html(result);
        }
    });
});