
//function TryFindLinkedInCompanyId() {
//    //https://www.linkedin.com/company/4033370?trk=prof-exp-company-name
//    var link = $("a[href~='linkedin.com/company/']:first").attr("href");

//    alert("hello" + link);
//}

//TryFindLinkedInCompanyId();

/* Set pageurl */
chrome.extension.sendRequest({ action: "onTabLoad", url: document.location.url, body: document.documentElement.outerHTML });

