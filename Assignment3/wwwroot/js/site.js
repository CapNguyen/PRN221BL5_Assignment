var connection = new signalR.HubConnectionBuilder().withUrl("/myHub").build();
console.log("OK")

connection.start().then(
).catch(
    function (err) {
        return console.error(err.toString());
    }
);
connection.on("ReloadItem", function () {
    location.reload();
});
