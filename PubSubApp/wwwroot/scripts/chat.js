var scheme = document.location.protocol === "https:" ? "wss" : "ws";
var port = document.location.port ? (":" + document.location.port) : "";
var socket;
var connectionUrl = scheme + "://" + document.location.hostname + port + "/ws";

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

    $("#connect").click(function (event) {
        console.log(event);
        alert('connected');
    })
});

function addActiveUser(data) {
    let userTemplate = $("#userTemplate").html();
    $(userTemplate).data('userdata', data);
    var compiled = _.template(userTemplate);
    data.curr_date = data.curr_date.getDate() + ' / ' + data.curr_date.getMonth() + ' / ' + data.curr_date.getFullYear();
    $(".inbox_chat").append(compiled(data));
    //$("#connect").click();
    connect(data.userid);
}

function connect(userid) {
    //stateLabel.innerHTML = "Connecting";
    
    socket = new WebSocket(connectionUrl + "?userid=" + userid);
    socket.onopen = function (event) {
        //updateState();
        //commsLog.innerHTML += '<tr>' +
        //    '<td colspan="3" class="commslog-data">Connection opened</td>' +
        //    '</tr>';
    };
    socket.onclose = function (event) {
        //updateState();
        //commsLog.innerHTML += '<tr>' +
        //    '<td colspan="3" class="commslog-data">Connection closed. Code: ' + htmlEscape(event.code) + '. Reason: ' + htmlEscape(event.reason) + '</td>' +
        //    '</tr>';
    };
    socket.onerror = updateState;
    socket.onmessage = function (event) {
        //commsLog.innerHTML += '<tr>' +
        //    '<td class="commslog-server">Server</td>' +
        //    '<td class="commslog-client">Client</td>' +
        //    '<td class="commslog-data">' + htmlEscape(event.data) + '</td></tr>';
    };
}

function updateState() {
    function disable() {
        //sendMessage.disabled = true;
        //sendButton.disabled = true;
        //closeButton.disabled = true;
    }
    function enable() {
        //sendMessage.disabled = false;
        //sendButton.disabled = false;
        //closeButton.disabled = false;
    }

    //connectionUrl.disabled = true;
    //connectButton.disabled = true;

    if (!socket) {
        disable();
    } else {
        switch (socket.readyState) {
            case WebSocket.CLOSED:
                //stateLabel.innerHTML = "Closed";
                disable();
                //connectionUrl.disabled = false;
                //connectButton.disabled = false;
                break;
            case WebSocket.CLOSING:
                //stateLabel.innerHTML = "Closing...";
                disable();
                break;
            case WebSocket.CONNECTING:
                //stateLabel.innerHTML = "Connecting...";
                disable();
                break;
            case WebSocket.OPEN:
                //stateLabel.innerHTML = "Open";
                enable();
                break;
            default:
                //stateLabel.innerHTML = "Unknown WebSocket State: " + htmlEscape(socket.readyState);
                disable();
                break;
        }
    }
}