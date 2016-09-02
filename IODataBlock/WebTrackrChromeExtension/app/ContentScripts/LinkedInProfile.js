//var link = $("a[href~='linkedin.com/company/']:first").attr("href");

// Call the specified callback, passing the web-page's DOM content back as argument
chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
    if (msg.text === "find_companyLink") {
        //var link = $("[href*='linkedin.com/compan']").first.attr("href");
        //https://www.linkedin.com/company/4033370?trk=prof-0-ovw-curr_pos
        var link = null;
        var links = $("[href]");
        $.each(links, function (i, field) {
            var lowerUrl = field.href.toLowerCase();
            if (lowerUrl.indexOf("/company/") > -1 & lowerUrl.indexOf("?trk=prof-0-ovw-curr_pos") > -1) {
                var start = lowerUrl.lastIndexOf("/company/") + "/company/".length;
                var end = lowerUrl.indexOf("?");
                link = lowerUrl.substring(start, end);
                return false;
            }
        });
        if (link == null) {
            //https://www.linkedin.com/company/10152667?trk=prof-exp-company-name
            $.each(links, function (i, field) {
                var lowerUrl = field.href.toLowerCase();
                if (lowerUrl.indexOf("/company/") > -1 & lowerUrl.indexOf("?trk=prof-exp-company-name") > -1) {
                    var start = lowerUrl.lastIndexOf("/company/") + "/company/".length;
                    var end = lowerUrl.indexOf("?");
                    link = lowerUrl.substring(start, end);
                    return false;
                }
            });
        }
        sendResponse(link);
    }
});
