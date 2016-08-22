/* Get all links */
var links = [].slice.apply(document.getElementsByTagName("a"));
links = links.map(function (element) {
    var href = element.href;
    /* Remove #s */
    var hashIndex = href.indexOf("#");
    if (hashIndex >= 0) {
        href = href.substr(0, hashIndex);
    }
    return href;
});

/* Sort the links */
links.sort();

/* Remove duplicates and invalid URLs. */
var kBadPrefix = "javascript";
for (var i = 0; i < links.length;) {
    if (((i > 0) && (links[i] === links[i - 1])) ||
        (links[i] === "") ||
        (kBadPrefix === links[i].toLowerCase().substr(0, kBadPrefix.length))) {
        links.splice(i, 1);
    } else {
        ++i;
    }
}

var urls = [];
var emails = [];
var urlPrefix = "http";
var mailPrefix = "mailto://";
for (var i = 0; i < links.length;) {
    var href = element.href;
    if (((i > 0) && (links[i] === links[i - 1])) || (links[i] === "") || (urlPrefix === links[i].toLowerCase().substr(0, urlPrefix.length))) {
        urls.push(links[i]);
        ++i;
    }
    else if (((i > 0) && (links[i] === links[i - 1])) || (links[i] === "") || (mailPrefix === links[i].toLowerCase().substr(0, mailPrefix.length))) {
        emails.push(links[i]);
        ++i;
    }
    else {
        ++i;
    }
}

/* Set pageurl */
chrome.extension.sendRequest({ action: "onTabLoad", links: links, url: document.location.url, body: document.documentElement.outerHTML });