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

    updateState();

    $(".msg_send_btn").click(function (event) {
        if (!socket || socket.readyState !== WebSocket.OPEN) {
            alert("socket not connected");
        }

        if ($(".write_msg").val() == '') return;
        if ($(".ui-selected").legth == 0) return;

        let topic = $(".ui-selected").data("userid");
        
        let data = {
            Topic: topic,
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
        setCurrentDate(data);
        $(".msg_history").append(compiled(data));
    });
});

function addActiveUser(data) {
    connect(data.userid);
    listAllUsers(data.userid);
}

function listAllUsers(userid) {
    $.get('/api/user', function (data) {
        //console.log(data);
        $(data).each(function (index, item) {
            if (item.UserID !== userid) {   //ignore loggedin user
                let userTemplate = $("#userTemplate").html();
                $(userTemplate).data('userdata', item);
                var compiled = _.template(userTemplate);
                let data1 = {
                    name: item.DisplayName,
                    id: item.Id,
                    userid: item.UserID,
                    status: item.Status
                };
                setCurrentDate(data1);
                $(".inbox_chat").append(compiled(data1));
            }
        });
        $(".inbox_chat").selectable();
    });
}

function setCurrentDate(data) {
    let currdate = new Date();
    data.curr_date = currdate.getDate() + ' / ' + currdate.getMonth() + ' / ' + currdate.getFullYear();
    data.curr_time = formatAMPM(currdate);
}

function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function connect(userid) {
    //stateLabel.innerHTML = "Connecting";

    socket = new WebSocket(connectionUrl + "?userid=" + userid);
    socket.onopen = function (event) {
        updateState();
        //commsLog.innerHTML += '<tr>' +
        //    '<td colspan="3" class="commslog-data">Connection opened</td>' +
        //    '</tr>';
    };
    socket.onclose = function (event) {
        updateState();
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
        let data = JSON.parse(event.data);
        setCurrentDate(data);
        $(".msg_history").append(compiled(data));
    };
}

function updateState() {
    function disable() {
        //sendMessage.disabled = true;
        //sendButton.disabled = true;
        //closeButton.disabled = true;
        $(".msg_send_btn").prop('disabled', true);
    }
    function enable() {
        //sendMessage.disabled = false;
        //sendButton.disabled = false;
        //closeButton.disabled = false;
        $(".msg_send_btn").prop('disabled', false);
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