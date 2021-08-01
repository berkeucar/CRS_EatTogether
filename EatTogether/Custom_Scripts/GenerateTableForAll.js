$(document).ready(function () {
    $.ajax({
        url: '/SessionModel/GenerateAllSessionModelTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });
});