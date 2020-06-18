const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/mainhub")
    .build();
let connectionId = "";
document.getElementById("submitForm")
    .addEventListener("click", function (e) {
        e.preventDefault();

        const data = new FormData();
        data.append("connectionId", connectionId);

        fetch("home/create", {
            method: "POST",
            body: data
        })
            .catch(error => console.error("Error: ", error));
    });

hubConnection.on("AddLobby", function (lobbies) {
    document.getElementById("mainContainer").innerHTML = "";
    for (var i = 0; i < lobbies.length; i++) {
        if (!lobbies[i].isReady) {
            let div = document.createElement("div");
            div.className = "col-2 mt-3";
            let divInside = document.createElement("div");
            divInside.className = "bg-info text-light d-flex justify-content-center text-center card";
            divInside.style = "height:64px";
            divInside.id = (i + 1).toString();
            divInside.onclick = function () { redirectToGame(divInside); };

            let text = document.createTextNode("Game #" + (i + 1));

            divInside.appendChild(text);
            div.appendChild(divInside);

            document.getElementById("mainContainer").appendChild(div);
        }
    }

});

hubConnection.on("RedirectToLobby", function (id) {

    window.location.href = "home/game/" + id;
});

hubConnection.start().then(() => {
    console.log(hubConnection.connectionId);
    connectionId = hubConnection.connectionId;
});