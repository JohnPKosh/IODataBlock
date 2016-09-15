/* Create variables */
var currentUrl = "na";
var currentPageType = null;
var currentDoc = null;
var currentTabId = null;
var relatedEmployeesJson = null;
var linkedInCompanyId = null;
var linkedInCompanyDto = null;
var linkedInProfileDto = null;

var disposableTabId = null;


/* Set up event handlers and inject send_links.js into all frames in the active tab. */
window.onload = function () {
    chrome.tabs.onUpdated.addListener(onEmailCreated);

    InitializePopup();
    PopupOnLoad();

    $('a[data-toggle="pill"]').on("shown.bs.tab", function(e) {
        var currentTab = e.target // activated tab
        var divid = $(currentTab).attr("href").substr(1);
        if (divid === "LinkedInEmployeesTab") {
            GetLinkedInCompanyEmployees(linkedInCompanyId, divid);
        }
    });
};

/* Start - LinkedIn Company and Profile initial popup callbacks */

function msgGetLinkedInProfileDto_callback(value) {
    console.log("Profile Company Link = " + value.LinkedInCompanyId);
    linkedInProfileDto = value;

    linkedInCompanyId = value.LinkedInCompanyId;
    var apiUrl = "http://localhost:51786/Api/TrakrScrape/LinkedInCompany/" + linkedInCompanyId + "/367db296-4e00-49b1-a064-d3e838db000d";
    /* Get Results from API */
    $.getJSON(apiUrl)
      .done(function (json) {
          DisplayLinkedInProfileTracked(linkedInProfileDto);
          DisplayLinkedInCompanyTracked(json); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
        })
      .fail(function (jqxhr, textStatus, error) {
          /*TODO: Show content from page and offer to Trak it.*/
          DisplayLinkedInProfileTracked(linkedInProfileDto);
          DisplayLinkedInCompanyUntracked(linkedInProfileDto);
      });

    $("#trackPageButton").click(function () {
        POST_LinkedInProfile();
    });
}

function GetLinkedInCompanyDto_callback(value) {
    console.log("Company ID = " + value.LinkedInCompanyId);
    linkedInCompanyId = value.LinkedInCompanyId;
    linkedInCompanyDto = value;

    var apiUrl = "http://localhost:51786/Api/TrakrScrape/LinkedInCompany/" + linkedInCompanyId + "/367db296-4e00-49b1-a064-d3e838db000d";
    /* Get Results from API */
    $.getJSON(apiUrl)
      .done(function (json) {
          DisplayLinkedInCompanyTracked(json); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
      })
      .fail(function (jqxhr, textStatus, error) {
          /*TODO: Show content from page and offer to Trak it.*/
          DisplayLinkedInCompanyUntracked(linkedInCompanyDto);
        });
}

/* End - LinkedIn Company and Profile initial popup callbacks */


function DisplayLinkedInCompanyTracked(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName").html("<a href='javascript:void();'><img id='linkedInCompanyTab_LinkedInCompanyName_Icon' src='img/In-2C-21px-R.png'/></a> " + json.LinkedInCompanyName + " <span id='linkedInCompanyTab_TrackingBadge' class='label label-success'>Tracking</span>");
    DisplayLinkedInCompanyBase(json);

    //$("#linkedInCompanyTab_LinkedInCompanyName_Icon").click(function () {
    //    chrome.tabs.create({ url: json.LinkedInCompanyUrl });
    //});
    //$("#linkedInCompanyTab_Industry").text(json.Industry);
    //$("#linkedInCompanyTab_CompanyType").text(json.CompanyType);
    //$("#linkedInCompanyTab_CompanySize").text(json.CompanySize);
    //$("#linkedInCompanyTab_Founded").text(json.Founded);
    //$("#linkedInCompanyTab_FollowersCount").text(json.FollowersCount);
    //$("#linkedInCompanyTab_Website").text(json.Website); 
    //$("#linkedInCompanyTab_Website").click(function () {
    //    chrome.tabs.create({ url: json.Website });
    //});
    //$("#linkedInCompanyTab_LinkedInCompanyUrl").text(json.LinkedInCompanyUrl);
    //$("#linkedInCompanyTab_LinkedInCompanyUrl").click(function () {
    //    chrome.tabs.create({ url: json.LinkedInCompanyUrl });
    //});

    //$("#linkedInCompanyTab_StreetAddress").text(json.StreetAddress);
    //$("#linkedInCompanyTab_Locality").text(json.Locality);
    //$("#linkedInCompanyTab_Region").text(json.Region);
    //$("#linkedInCompanyTab_PostalCode").text(json.PostalCode);
    //$("#linkedInCompanyTab_CountryName").text(json.CountryName);


    //$("#linkedInCompanyTab_LinkedInLocationTitle").html("<a id='linkedInCompanyTab_LinkedInLocationLink' href='javascript:void();'><span class='glyphicon glyphicon-globe'></span></a> Location");
    //$("#linkedInCompanyTab_LinkedInLocationLink").click(function () {
    //    chrome.tabs.create({ url: CreateGoogleMapsLink(json) });
    //});
};

function DisplayLinkedInCompanyUntracked(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName").html("<a href='javascript:void();'><img id='linkedInCompanyTab_LinkedInCompanyName_Icon' src='img/In-2C-21px-R.png'/></a> " + json.LinkedInCompanyName + " <span id='linkedInCompanyTab_TrackingBadge' class='label label-default'>Track Now!</span>");
    DisplayLinkedInCompanyBase(json);

    //$("#linkedInCompanyTab_LinkedInCompanyName_Icon").click(function () {
    //    CreateLinkedInCompanyDtoNewTabAndListener(json);
    //});
    //$("#linkedInCompanyTab_Industry").text(json.Industry);
    //$("#linkedInCompanyTab_CompanyType").text(json.CompanyType);
    //$("#linkedInCompanyTab_CompanySize").text(json.CompanySize);
    //$("#linkedInCompanyTab_Founded").text(json.Founded);
    //$("#linkedInCompanyTab_FollowersCount").text(json.FollowersCount);
    //$("#linkedInCompanyTab_Website").text(json.Website);
    //$("#linkedInCompanyTab_Website").click(function () {
    //    chrome.tabs.create({ url: json.Website });
    //});
    //$("#linkedInCompanyTab_LinkedInCompanyUrl").text(json.LinkedInCompanyUrl);
    //$("#linkedInCompanyTab_LinkedInCompanyUrl").click(function () {
    //    chrome.tabs.create({ url: json.LinkedInCompanyUrl });
    //});

    //$("#linkedInCompanyTab_StreetAddress").text(json.StreetAddress);
    //$("#linkedInCompanyTab_Locality").text(json.Locality);
    //$("#linkedInCompanyTab_Region").text(json.Region);
    //$("#linkedInCompanyTab_PostalCode").text(json.PostalCode);
    //$("#linkedInCompanyTab_CountryName").text(json.CountryName);


    //$("#linkedInCompanyTab_LinkedInLocationTitle").html("<a id='linkedInCompanyTab_LinkedInLocationLink' href='javascript:void();'><span class='glyphicon glyphicon-globe'></span></a> Location");
    //$("#linkedInCompanyTab_LinkedInLocationLink").click(function () {
    //    chrome.tabs.create({ url: CreateGoogleMapsLink(json) });
    //});
};

function DisplayLinkedInCompanyBase(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName_Icon").click(function () {
        chrome.tabs.create({ url: json.LinkedInCompanyUrl });
    });
    $("#linkedInCompanyTab_Industry").text(json.Industry);
    $("#linkedInCompanyTab_CompanyType").text(json.CompanyType);
    $("#linkedInCompanyTab_CompanySize").text(json.CompanySize);
    $("#linkedInCompanyTab_Founded").text(json.Founded);
    $("#linkedInCompanyTab_FollowersCount").text(json.FollowersCount);
    $("#linkedInCompanyTab_Website").text(json.Website);
    $("#linkedInCompanyTab_Website").click(function () {
        chrome.tabs.create({ url: json.Website });
    });
    $("#linkedInCompanyTab_LinkedInCompanyUrl").text(json.LinkedInCompanyUrl);
    $("#linkedInCompanyTab_LinkedInCompanyUrl").click(function () {
        chrome.tabs.create({ url: json.LinkedInCompanyUrl });
    });

    $("#linkedInCompanyTab_StreetAddress").text(json.StreetAddress);
    $("#linkedInCompanyTab_Locality").text(json.Locality);
    $("#linkedInCompanyTab_Region").text(json.Region);
    $("#linkedInCompanyTab_PostalCode").text(json.PostalCode);
    $("#linkedInCompanyTab_CountryName").text(json.CountryName);


    $("#linkedInCompanyTab_LinkedInLocationTitle").html("<a id='linkedInCompanyTab_LinkedInLocationLink' href='javascript:void();'><span class='glyphicon glyphicon-globe'></span></a> Location");
    $("#linkedInCompanyTab_LinkedInLocationLink").click(function () {
        chrome.tabs.create({ url: CreateGoogleMapsLink(json) });
    });
}

/* Create Tab and new onUpdated listener for LinkedInCompanyDto scrape */
function CreateLinkedInCompanyDtoNewTabAndListener(json) {
    chrome.tabs.create({ active: false, url: "https://linkedin.com/company/" + json.LinkedInCompanyId }, function (tab) {
        disposableTabId = tab.id;
        chrome.tabs.onUpdated.addListener(CreateLinkedInCompanyDtoNewTabAndListener_callback);
    });
}

/* Capture New Tab LinkedInCompanyDto listener */
function CreateLinkedInCompanyDtoNewTabAndListener_callback(tabId, changeInfo, tabInfo) {
    if (tabId === disposableTabId && changeInfo.status === "complete") {
        chrome.tabs.sendMessage(disposableTabId, { text: "msgGetLinkedInCompanyDto" }, POST_LinkedInCompanyDtoNewTabResponse);
    }
}

/* POST LinkedInCompanyDto to API from New Tab listener */
function POST_LinkedInCompanyDtoNewTabResponse(json) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:51786/Api/TrakrScrape", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            var obj = JSON.parse(xhr.responseText);
            document.getElementById("resp").innerHTML = " <strong> Saved!</strong> ";
            DisplayLinkedInCompanyTracked(obj);
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.send(JSON.stringify({ location: json.LinkedInCompanyUrl, inputDto: json, document: null, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
    chrome.tabs.onUpdated.removeListener(CreateLinkedInCompanyDtoNewTabAndListener_callback);
    chrome.tabs.remove(disposableTabId);
}

function DisplayLinkedInProfileTracked(json) {

    $("#linkedInCompanyTab_LinkedInProfileName").html("<a href='javascript:void();'><img id='linkedInCompanyTab_LinkedInProfileName_Icon' src='img/In-2C-21px-R.png'/></a> " + json.LinkedInFullName + " <span id='linkedInProfileTab_TrackingBadge' class='label label-success'>Tracking</span>");
    $("#linkedInCompanyTab_LinkedInProfileName_Icon").click(function () {
        chrome.tabs.create({ url: json.LinkedInPage });
    });


    $("#linkedInProfileTab_LinkedInFullName").text(json.LinkedInFullName);
    $("#linkedInProfileTab_LinkedInTitle").text(json.LinkedInTitle);
    $("#linkedInProfileTab_Location").text(json.Location);
    //$("#linkedInProfileTab_Email").text(json.Email);

    $("#linkedInProfileTab_Email").html("<a href='javascript:void();'>" + json.Email + "</a>");
    $("#linkedInProfileTab_Email").click(function () {
        //chrome.tabs.create({ active: false, url: "mailto://" + json.Email });
        chrome.tabs.create({ active: false, url: "https://linkedin.com/company/" + json.LinkedInCompanyId }, function (tab) {
            chrome.tabs.sendMessage(tab.id, { text: "msgGetLinkedInCompanyDto" }, DisplayLinkedInCompanyTracked); // TODO: try to save company if not exists!!!!
            setTimeout(function () {
                console.log("closing tab");
                 chrome.tabs.remove(tab.id);
            }, 2000);
        });
    });


    $("#linkedInProfileTab_Im").text(json.Im);
    $("#linkedInProfileTab_Twitter").text(json.Twitter);
    $("#linkedInProfileTab_Phone").text(json.Phone);
};

function onEmailCreated(tabId, changeInfo, tab ) {
    //chrome.tabs.remove(tabId);
    if (changeInfo.url) {
        console.log("Tab: " + tabId + " URL changed to " + changeInfo.url);
    }
}

function CreateGoogleMapsLink(json) {
    try {
        //https://www.google.com/maps/place/1910+Towne+Centre+Blvd,+Annapolis,+MD+21401
        var a = [];
        if (json.StreetAddress) a.push(json.StreetAddress.trim().replace(" ", "+"));
        if (json.Locality) a.push(json.Locality.trim().replace(" ", "+"));
        if (json.Region) a.push(json.Region.trim().replace(" ", "+"));
        if (json.PostalCode) a.push(json.PostalCode.trim().replace(" ", "+"));
        if (json.CountryName) a.push(json.CountryName.trim().replace(" ", "+"));
        return "https://www.google.com/maps/place/" + a.join(",");
    } catch (err) {
        console.log("ERR CreateGoogleMapsLink: " + err);
    }
};

function GetLinkedInCompanyEmployees(value, divid) {

    //$("#" + divid).text("hello"); /*TODO: Check if already called API for employee info, if no call API and store as variable, else yes pull from variable. */

    var apiUrl = "http://localhost:51786/Api/TrakrScrape/LinkedInCompanyProfiles/" + linkedInCompanyId + "/367db296-4e00-49b1-a064-d3e838db000d";
    /* Get Results from API */
    $.getJSON(apiUrl)
      .done(function (json) {
          DisplayLinkedInCompanyEmployees(json); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
      })
      .fail(function (jqxhr, textStatus, error) {
          var err = textStatus + ", " + error;
          /*TODO: Show content from page and offer to Trak it.*/
          $("#linkedInCompanyProfilesTab_EmployeesTitle").text("Not Tracking!");
      });
}

function DisplayLinkedInCompanyEmployees(json) {
    $("#linkedInCompanyProfilesTab_Employees").html("");
    var cnt = 0;
    $.each(json, function (i, field) {
        $("#linkedInCompanyProfilesTab_Employees").append("<p><span><strong>" + field.LinkedInFullName + "</strong> - " + field.LinkedInTitle + "</span></p>");
        $("#linkedInCompanyProfilesTab_Employees").append("<p><span class='site-field-label-primary'><a href='javascript:void();' id='tab_employee_link" + i + "_Icon'><img src='img/In-2C-14px.png'/></a> LinkedIn Page:</span> <a href='void();' id='tab_employee_link" + i + "'>" + field.LinkedInPage + "</a></p>");
        $("#tab_employee_link" + i + "").click(function () {
            chrome.tabs.create({ url: field.LinkedInPage });
        });
        $("#tab_employee_link" + i + "_Icon").click(function () {
            chrome.tabs.create({ url: field.LinkedInPage });
        });
    });
    $("#linkedInCompanyProfilesTab_EmployeesTitle").html("Tracked Employees <span class='badge'>" + json.length + "</span>");
};

/**
 * Get the value of a querystring
 * @param  {String} field The field to get the value of
 * @param  {String} url   The URL to get the value from (optional)
 * @return {String}       The field value
 */
var getQueryString = function (field, url) {
    var href = url ? url : window.location.href;
    var reg = new RegExp("[?&]" + field + "=([^&#]*)", "i");
    var string = reg.exec(href);
    return string ? string[1] : null;
};

/* Add links to allLinks and visibleLinks, sort and show them.  send_links.js is injected into all frames of the active tab, so this listener may be called multiple times. */
chrome.extension.onRequest.addListener(function (data) {
    /* If the action is send_links Do This*/
    if (data.action === "onTabLoad") {
        currentDoc = data.body;
    }
});

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

function POST_LinkedInCompany() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:51786/Api/TrakrScrape", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            var obj = JSON.parse(xhr.responseText);
            document.getElementById("resp").innerHTML = " <strong> Saved!</strong> ";
            DisplayLinkedInCompanyTracked(obj);
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.send(JSON.stringify({ location: currentUrl, inputDto: linkedInCompanyDto, document: null, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
}

function POST_LinkedInProfile() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:51786/Api/TrakrScrape", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            var obj = JSON.parse(xhr.responseText);
            document.getElementById("resp").innerHTML = " <strong> Saved!</strong> ";
            DisplayLinkedInCompanyTracked(obj);
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.send(JSON.stringify({ location: currentUrl, inputDto: linkedInProfileDto, document: null, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
    CreateLinkedInCompanyDtoNewTabAndListener(linkedInProfileDto);
}




/* Start -  Configure popup for specific pages. */

function PopupOnLoad_LinkedInCompany() {
    $("#site-title").click(function () {
        chrome.tabs.create({ url: "http://localhost:51786/Home/Index" });
    });

    $("#trackPageButton").click(function () {
        POST_LinkedInCompany();
    });

    $('#mainTabs a[href="#LinkedInProfileTab"]').hide();
    chrome.tabs.sendMessage(currentTabId, { text: "msgGetLinkedInCompanyDto" }, GetLinkedInCompanyDto_callback);
}

function PopupOnLoad_LinkedInProfile() {
    $("#site-title").click(function () {
        chrome.tabs.create({ url: "http://localhost:51786/Home/Index" });
    });

    //$("#trackPageButton").click(function () {
    //    POST_LinkedInProfile();
    //});

    chrome.tabs.sendMessage(currentTabId, { text: "msgGetLinkedInProfileDto" }, msgGetLinkedInProfileDto_callback);
    $('#mainTabs a[href="#LinkedInProfileTab"]').tab('show');
}

function PopupOnLoad_Default() {
    $("#site-title").click(function () {
        chrome.tabs.create({ url: "http://localhost:51786/Home/Index" });
    });
}

/* End -  Configure popup for specific pages. */


function InitializePopup() {
    /* Set current tab URL */
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        var current = tabs[0];
        currentUrl = current.url;

        /* Set current tab type */
        if (currentUrl.indexOf("linkedin.com/compan") > -1) {
            currentPageType = "LinkedInCompany";
        }
        else if (currentUrl.indexOf("linkedin.com/in/") > -1) {
            currentPageType = "LinkedInProfile";
        }
    });
}

function PopupOnLoad() {

    chrome.tabs.getSelected(null, function (tab) {
        currentTabId = tab.id;
        switch (currentPageType) {
            case "LinkedInCompany":
                PopupOnLoad_LinkedInCompany();
                break;
            case "LinkedInProfile":
                PopupOnLoad_LinkedInProfile();
                break;
            default:
                PopupOnLoad_Default();
                console.log("Page type not found! " + currentPageType + " " + currentUrl);
        }
    });


}


//function getCurrentDoc() {
//    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
//        var current = tabs[0];
//        currentDoc = current.document.documentElement.outerHTML;
//    });
//}
