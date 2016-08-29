
var url = document.url;
chrome.extension.sendRequest({ action: "send_url", data: url });