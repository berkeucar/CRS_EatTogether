$(document).ready(function () {
    $.ajax({
        url: '/SessionModel/GenerateSessionModelTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });
});