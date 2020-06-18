
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/mainhub")
    .build();
let connectionId = "";
function dostep(sender) {
    if (document.getElementById("readyId").value == "true") {
        const data = new FormData();
        data.append("index", sender.id);
        data.append("id", document.getElementById("lobbyId").value);
        let myturnId = document.getElementById("myturnId");
        hubConnection.invoke("DoStep", sender.id, document.getElementById("lobbyId").value, connectionId, (myturnId.value == "true"));
    }
}

function Ready(sender) {
    hubConnection.invoke("AddToGameGroup", document.getElementById("lobbyId").value, connectionId);
    sender.innerText = "You'r ready now";
    sender.className = "btn btn-success mb-3"
    sender.onclick = null;
    
}

hubConnection.on("SetPlayerTurn", function (turn) {
    let myturnId = document.getElementById("myturnId");
    myturnId.value = turn;
});

hubConnection.on("UpdateField", function (lobby) {
    for (let i = 0; i < lobby.gameSteps.length; i++) {
        fieldPart = document.getElementById(lobby.gameSteps[i].index.toString());
        fieldPart.src = document.getElementById(lobby.gameSteps[i].player == false ? "cross" : "circle").src;
    }
});

hubConnection.on("WinningPanel", function (winner) {
    let infoButton = document.getElementById("infoButton");
    if (winner == (myturnId.value == "true")) {
        infoButton.innerText = "You won!";
        infoButton.className = "btn btn-success mb-3";
    }
    else {
        infoButton.innerText = "You lose!";
        infoButton.className = "btn btn-danger mb-3";
    }
    let readyId = document.getElementById("readyId");
    readyId.value = false;
});

hubConnection.on("DrawPanel", function () {
    let infoButton = document.getElementById("infoButton");
    infoButton.innerText = "Tie!";
    let readyId = document.getElementById("readyId");
    readyId.value = false;
});

hubConnection.on("ReadyNotify", function () {
    let infoButton = document.getElementById("infoButton");
    infoButton.innerText = "Playing";
    infoButton.className = "btn btn-warning mb-3"
    let readyId = document.getElementById("readyId");
    readyId.value = true;
});

hubConnection.start().then(() => {
    console.log(hubConnection.connectionId);
    connectionId = hubConnection.connectionId;
});