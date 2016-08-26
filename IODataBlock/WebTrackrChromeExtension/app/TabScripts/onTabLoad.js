
//var undefined;

/* Get all links */
var links = [].slice.apply(document.getElementsByTagName("a"));
links = links.map(function (element) {
    var href = element.href;
    /* Remove #s */
    if (href) {
        var hrefval = href.value;
        if (hrefval) {
            if (hrefval.indexOf("#") >= 0) {
                var hashIndex = hrefval.indexOf("#");
                if (hashIndex >= 0) {
                    href = hrefval.substr(0, hashIndex);
                }
            }
        }
    }
    return href;
});

/* Sort the links */
links.sort();

/* Remove duplicates and invalid URLs. */
var javascriptPrefix = "javascript";
var objectPrefix = "[object";
for (var i = 0; i < links.length;) {


    var linkval = String(links[i]);
    //var linkval = links[i].value;
    console.log(linkval);
    if (typeof linkval === 'undefined') {
        links[i] = "";
    }
    if (typeof links[i] === void 0) {
        links[i] = "";
    }
    if (javascriptPrefix === linkval.toLowerCase().substr(0, javascriptPrefix.length)) {
        links[i] = "";
    }
    if (objectPrefix === linkval.toLowerCase().substr(0, objectPrefix.length)) {
        links[i] = "";
    }

    if (((i > 0) && (links[i] == links[i - 1])) ||
          (links[i] == "") ||
          (javascriptPrefix === linkval.toLowerCase().substr(0, javascriptPrefix.length))) {
        links.splice(i, 1);
    } else {
        ++i;
    }
}






//var urls = [];
//var emails = [];
//var urlPrefix = "http";
//var mailPrefix = "mailto://";
//for (var i = 0; i < links.length;) {
//    var href = element.href;
//    if (((i > 0) && (links[i] === links[i - 1])) || (links[i] === "") || (urlPrefix === links[i].toLowerCase().substr(0, urlPrefix.length))) {
//        urls.push(links[i]);
//        ++i;
//    }
//    else if (((i > 0) && (links[i] === links[i - 1])) || (links[i] === "") || (mailPrefix === links[i].toLowerCase().substr(0, mailPrefix.length))) {
//        emails.push(links[i]);
//        ++i;
//    }
//    else {
//        ++i;
//    }
//}

/* Set pageurl */
chrome.extension.sendRequest({ action: "onTabLoad", links: links, url: document.location.url, body: document.documentElement.outerHTML });