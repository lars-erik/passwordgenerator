// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

let search = location.search.substr(1) || format(new Date());
let pwd = "";

function update(data) {
    document.getElementById("password").value = pwd = data.Password;
    //data.Password = undefined;
    //document.getElementById("output").innerText = JSON.stringify(data, null, '  ');
}

function reload() {
    let salt = document.getElementById("salt").value;
    fetch('/api/passwords/' + search + (salt ? "/" + salt : "")).then(response => response.json()).then(update);
}

function format(date) {
    return date.getFullYear().toString() +
        (date.getMonth() < 9 ? "0" : "") +
        (date.getMonth() + 1).toString() +
        (date.getDate() < 10 ? "0" : "") +
        date.getDate();
}

function parseDate(value) {
    let year = Number(value.toString().substr(0, 4)),
        month = Number(value.toString().substr(4, 2)) - 1,
        day = Number(value.toString().substr(6, 2)),
        date = new Date(year, month, day);
    return date;
}

function go(direction) {
    let date = parseDate(search);
    date.setDate(date.getDate() + direction);
    search = format(date);
    reload();
}

function toggle() {
    let el = document.getElementById("password");
    if (el.type === 'password') {
        el.type = 'text';
    } else {
        el.type = 'password';
    }
}

function select() {
    document.getElementById("password").select();
}

function copy() {
    navigator.clipboard.writeText(pwd);
}

window.addEventListener("load", reload);

document.getElementById("left").addEventListener("click", () => go(-1));
document.getElementById("right").addEventListener("click", () => go(+1));
document.getElementById("salt").addEventListener("keyup", reload);
document.getElementById("password").addEventListener("focus", select);
document.getElementById("toggle").addEventListener("click", toggle);
document.getElementById("copy").addEventListener("click", copy);