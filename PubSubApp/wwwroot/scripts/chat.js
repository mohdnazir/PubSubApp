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

    //$("#frmLogin").on("submit", function () {
    //    alert('test');
    //});

    //$("#connect").click(function (event) {
    //    console.log(event);
    //    alert('connected');
    //});

    $(".msg_send_btn").click(function (event) {
        if (!socket || socket.readyState !== WebSocket.OPEN) {
            alert("socket not connected");
        }
        let data = {
            Topic: "nazir",
            Msg: $(".write_msg").val()
        }
        socket.send(JSON.stringify(data));
        $(".write_msg").val('');
        //commsLog.innerHTML += '<tr>' +
        //    '<td class="commslog-client">Client</td>' +
        //    '<td class="commslog-server">Server</td>' +
        //    '<td class="commslog-data">' + htmlEscape(data) + '</td></tr>';
        let outgoing_msg = $("#outgoing_msg").html();
        var compiled = _.template(outgoing_msg);
        $(".msg_history").append(compiled(data));
    });
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
        //console.log(event.data);
        //$(".msg_history").append(event.data);
        //commsLog.innerHTML += '<tr>' +
        //    '<td class="commslog-server">Server</td>' +
        //    '<td class="commslog-client">Client</td>' +
        //    '<td class="commslog-data">' + htmlEscape(event.data) + '</td></tr>';

        let incoming_msg = $("#incoming_msg").html();
        var compiled = _.template(incoming_msg);        
        $(".msg_history").append(compiled(JSON.parse(event.data)));
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