$(document).ready(function () {
    $("#btnLogin").click(function () {
        $('.modal-body').load('login.html', function (data) {
            //console.log(data);
            $('#loginModel').modal({ show: true });
        });
    });

    $("#frmLogin").on("submit", function () {
        alert('test');
    });
});

function addActiveUser(data) {
    var compiled = _.template($("#userTemplate").html());
    data.curr_date = data.curr_date.getDate() + ' / ' + data.curr_date.getMonth() + ' / ' + data.curr_date.getFullYear();
    $(".inbox_chat").append(compiled(data));
}