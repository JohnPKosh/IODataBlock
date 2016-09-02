

/* Set pageurl */
chrome.extension.sendRequest({ action: "onTabLoad", url: document.location.url, body: document.documentElement.outerHTML });