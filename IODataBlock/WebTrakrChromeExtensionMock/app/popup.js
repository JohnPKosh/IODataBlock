
chrome.tabs.getSelected(null, function (tab) {
    window.domain = new URL(tab.url).hostname.replace("www.", "");
    $("#currentDomain").text(window.domain);

    chrome.tabs.sendMessage(tab.id, { text: 'find_companyId' }, HandleLinkedInCompanyId);
});

function HandleLinkedInCompanyId(value) {
    console.log('I received the following DOM content:\n' + getQueryString("id", value));
    /* DO something with the CompanyId HERE */

    //$("#currentDomain").text(getQueryString("id", value));
    var id = getQueryString("id", value);

    var apiUrl = "http://localhost:51786/Api/TrakrScrape/LinkedInCompany/" + id + "/367db296-4e00-49b1-a064-d3e838db000d";
    $.getJSON(apiUrl)
      .done(function (json) {
          $("#currentDomain").text(json.LinkedInCompanyName); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
            DisplayLinkedInCompanyInfo(json);
        })
      .fail(function (jqxhr, textStatus, error) {
          var err = textStatus + ", " + error;
          $("#currentDomain").text("Request Failed: " + err); /*TODO: Show content from page and offer to Trak it.*/
      });
}

function DisplayLinkedInCompanyInfo(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName").text(json.LinkedInCompanyName);
    $("#linkedInCompanyTab_region").text(json.region);
    $("#linkedInCompanyTab_countryName").text(json.countryName);
    $("#linkedInCompanyTab_website").text(json.website);
    $("#linkedInCompanyTab_website").attr("href", json.website);
};

/**
 * Get the value of a querystring
 * @param  {String} field The field to get the value of
 * @param  {String} url   The URL to get the value from (optional)
 * @return {String}       The field value
 */
var getQueryString = function (field, url) {
    var href = url ? url : window.location.href;
    var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
    var string = reg.exec(href);
    return string ? string[1] : null;
};






/* Create variables */
var urls = [];
var emails = [];

var allLinks = [];
var visibleLinks = [];
var currentUrl = "na";
var currentDoc = null;

/* Set up event handlers and inject send_links.js into all frames in the active tab. */
window.onload = function () {
    AddOpenHomePageEvent();
    document.getElementById("filter").onkeyup = filterLinks;
    document.getElementById("regex").onchange = filterLinks;
    document.getElementById("toggle_all").onchange = toggleAll;
    document.getElementById("trackPageButton").onclick = trackPagePOST;

    /* Inject send_links.js into all frames in the active tab.*/
    chrome.windows.getCurrent(function (currentWindow) {
        chrome.tabs.query({ active: true, windowId: currentWindow.id },
                          function (activeTabs) {
                              currentUrl = activeTabs[0].url;
                              chrome.tabs.executeScript(activeTabs[0].id, { file: "/TabScripts/onTabLoad.js", allFrames: false });
                          });
    });
};


/* Add links to allLinks and visibleLinks, sort and show them.  send_links.js is injected into all frames of the active tab, so this listener may be called multiple times. */
chrome.extension.onRequest.addListener(function (data) {
    /* If the action is send_links Do This*/
    if (data.action === "onTabLoad")
    {
        for (var index in data.links) {
            if (data.links.hasOwnProperty(index))
            {
                allLinks.push(data.links[index]);
            }
        }
        allLinks.sort();
        visibleLinks = allLinks;
        showLinks();
        currentDoc = data.body;
    }
});

/* Display all visible links. */
function showLinks() {
    var linksTable = document.getElementById("links");
    while (linksTable.children.length > 1) {
        linksTable.removeChild(linksTable.children[linksTable.children.length-1]);
    }
    for (var i = 0; i < visibleLinks.length; ++i) {
        /* Create row */
        var row = document.createElement("tr");

        /* Build column 0 */
        var col0 = document.createElement("td");
        var checkbox = document.createElement("input");
        checkbox.checked = true;
        checkbox.type = "checkbox";
        checkbox.id = "check" + i;
        col0.appendChild(checkbox);

        /* Build column 0 */
        var col1 = document.createElement("td");
        col1.id = "item" + i;
        col1.innerHTML = "<a href='#'>" + visibleLinks[i] + "</a>";
        AddLinkEvent(col1, i);

        /* Append row */
        row.appendChild(col0);
        row.appendChild(col1);
        linksTable.appendChild(row);
    }
}

/* Re-filter allLinks into visibleLinks and reshow visibleLinks. */
function filterLinks() {
    var filterValue = document.getElementById("filter").value;
    if (document.getElementById("regex").checked) {
        visibleLinks = allLinks.filter(function (link) {
            return link.match(filterValue);
        });
    } else {
        var terms = filterValue.split(" ");
        visibleLinks = allLinks.filter(function (link) {
            for (var termI = 0; termI < terms.length; ++termI) {
                var term = terms[termI];
                if (term.length != 0) {
                    var expected = (term[0] != "-");
                    if (!expected) {
                        term = term.substr(1);
                        if (term.length == 0) {
                            continue;
                        }
                    }
                    var found = (-1 !== link.indexOf(term));
                    if (found != expected) {
                        return false;
                    }
                }
            }
            return true;
        });
    }
    showLinks();
}

/* Toggle the checked state of all visible links.*/
function toggleAll() {
    var checked = document.getElementById("toggle_all").checked;
    for (var i = 0; i < visibleLinks.length; ++i) {
        document.getElementById("check" + i).checked = checked;
    }
}

/* Add link click events */
function AddLinkEvent(elem, i) {
    elem.onclick = function() {
        chrome.tabs.create({ url: visibleLinks[i] });
    };
}

/* Add logo click event to open home page. */
function AddOpenHomePageEvent() {
    var siteTitle = document.getElementById("site-title");
    siteTitle.onclick = function () {
        chrome.tabs.create({ url: "http://localhost:51786/Home/Index" });
    };
}




/* Not currently used TODO: determine if it makes sense to break up into single responsibilities. */
//function sendData() {
//    chrome.windows.getCurrent(function (currentWindow) {
//        chrome.tabs.query({ active: true, windowId: currentWindow.id },
//                          function (activeTabs) {
//                              chrome.tabs.executeScript(activeTabs[0].id, { file: "send_url.js", allFrames: false });
//                          });
//    });
//};


/* POST entire HTML document and Links to API*/
function trackPagePOST() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:51786/Api/TrakrScrape", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            var obj = JSON.parse(xhr.responseText);
            document.getElementById("resp").innerHTML = obj.LocationUrl + " <strong> Saved!</strong>";
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    getUrl();
    xhr.send(JSON.stringify({ location: currentUrl, document: currentDoc, links: allLinks, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
}


function getUrl() {
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        var current = tabs[0];
        //incognito = current.incognito;
        url = current.url;
        currentUrl = url;
    });
}

//function getCurrentDoc() {
//    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
//        var current = tabs[0];
//        currentDoc = current.document.documentElement.outerHTML;
//    });
//}
