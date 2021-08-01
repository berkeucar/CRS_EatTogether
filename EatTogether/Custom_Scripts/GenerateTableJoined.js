$(document).ready(function () {
    $.ajax({
        url: '/SessionModel/GenerateJoinedSessionsTable',
        success: function (result) {
            $('#tableDiv2').html(result);
        }
    });
});