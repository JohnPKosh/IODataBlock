

chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
    if (msg.text === 'find_companyId') {
        // Call the specified callback, passing
        // the web-page's DOM content as argument
        sendResponse(document.getElementsByClassName("follow-start")[0].getAttribute("href"));        
    }
});


