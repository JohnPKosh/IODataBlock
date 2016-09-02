
// Call the specified callback, passing the web-page's DOM content back as argument
chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
    if (msg.text === "find_companyId") {
        sendResponse($(".follow-start:first").attr("href"));
    }
    //if (msg.text === "get_DOMElements") {
    //    sendResponse(document);
    //}
});


