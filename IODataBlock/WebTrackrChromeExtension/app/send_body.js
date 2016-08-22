//alert("hello");

function execute() {
    var input = document.documentElement.outerHTML;
    var url = document.url;
    var mydata = JSON.stringify({ location: "hello", document: "hello" });

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:50231/api/Values/PostBody", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            //document.getElementById("resp").innerHTML = xhr.responseText;
            alert("sent" + xhr.responseText);
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.send(mydata);
};

execute();


//var obj = { data: data, links: links };
//chrome.extension.sendRequest(input);