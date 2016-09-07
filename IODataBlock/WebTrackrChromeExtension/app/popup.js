/* Create variables */
var currentUrl = "na";
var currentDoc = null;
var relatedEmployeesJson = null;
var linkedInCompanyId = null;
var linkedInDto = null;


/* Set up event handlers and inject send_links.js into all frames in the active tab. */
window.onload = function () {
    AddOpenHomePageEvent();
    document.getElementById("trackPageButton").onclick = trackPagePOST;

    /* Inject send_links.js into all frames in the active tab.*/
    chrome.windows.getCurrent(function (currentWindow) {
        chrome.tabs.query({ active: true, windowId: currentWindow.id },
                          function (activeTabs) {
                              currentUrl = activeTabs[0].url;
                              chrome.tabs.executeScript(activeTabs[0].id, { file: "/TabScripts/onTabLoad.js", allFrames: false });
                          });
    });

    chrome.tabs.getSelected(null, function (tab) {
        var lowerUrl = tab.url.toLowerCase();
        if (lowerUrl.indexOf("linkedin.com/compan") > -1) {
            chrome.tabs.sendMessage(tab.id, { text: "msgGetLinkedInCompanyDto" }, GetLinkedInCompanyInfo);
        }
        else if (lowerUrl.indexOf("linkedin.com/in/") > -1) {
            chrome.tabs.sendMessage(tab.id, { text: "msgGetLinkedInProfileDto" }, msgGetLinkedInProfileDto_callback);
        }
        //window.domain = new URL(tab.url).hostname.replace("www.", "");
    });

    $('a[data-toggle="pill"]').on("shown.bs.tab", function(e) {
        var currentTab = e.target // activated tab
        var divid = $(currentTab).attr("href").substr(1);
        if (divid === "tab3") {
            GetLinkedInCompanyEmployees(linkedInCompanyId, divid);
        }
    });
};

/* Test */
function msgGetLinkedInProfileDto_callback(value) {
    console.log("Profile Company Link = " + value.CompanyId);
    linkedInDto = value;

    linkedInCompanyId = value.CompanyId;
    var apiUrl = "http://localhost:51786/Api/TrakrScrape/LinkedInCompany/" + linkedInCompanyId + "/367db296-4e00-49b1-a064-d3e838db000d";
    /* Get Results from API */
    $.getJSON(apiUrl)
      .done(function (json) {
          DisplayLinkedInCompanyInfo(json); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
      })
      .fail(function (jqxhr, textStatus, error) {
          var err = textStatus + ", " + error;
          /*TODO: Show content from page and offer to Trak it.*/
          $("#linkedInCompanyTab_LinkedInCompanyName").text("Not Tracking!");
      });
}

function GetLinkedInCompanyInfo(value) {
    console.log("Company ID = " + value.CompanyId);
    linkedInCompanyId = value.CompanyId;
    linkedInDto = value;

    var apiUrl = "http://localhost:51786/Api/TrakrScrape/LinkedInCompany/" + linkedInCompanyId + "/367db296-4e00-49b1-a064-d3e838db000d";
    /* Get Results from API */
    $.getJSON(apiUrl)
      .done(function (json) {
          DisplayLinkedInCompanyInfo(json); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
      })
      .fail(function (jqxhr, textStatus, error) {
          var err = textStatus + ", " + error;
          /*TODO: Show content from page and offer to Trak it.*/
          $("#linkedInCompanyTab_LinkedInCompanyName").text("Not Tracking!");
      });
}

function DisplayLinkedInCompanyInfo(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName").html(json.LinkedInCompanyName + " <span id='tracking_badge' class='label label-success'>Tracking</span>");
    $("#linkedInCompanyTab_industry").text(json.industry);
    $("#linkedInCompanyTab_type").text(json.type);
    $("#linkedInCompanyTab_companySize").text(json.companySize);
    $("#linkedInCompanyTab_founded").text(json.founded);
    $("#linkedInCompanyTab_followersCount").text(json.followersCount);
    $("#linkedInCompanyTab_website").text(json.website); 
    $("#linkedInCompanyTab_website").click(function () {
        chrome.tabs.create({ url: json.website });
    });
    $("#linkedInCompanyTab_streetAddress").text(json.region);
    $("#linkedInCompanyTab_locality").text(json.region);
    $("#linkedInCompanyTab_region").text(json.region);
    $("#linkedInCompanyTab_postalCode").text(json.region);
    $("#linkedInCompanyTab_countryName").text(json.countryName);
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
        $("#linkedInCompanyProfilesTab_Employees").append("<p><span class='site-field-label-primary'>Name:</span> <span><strong>" + field.LinkedInFullName + "</strong> - " + field.LinkedInTitle + "</span></p>");
        $("#linkedInCompanyProfilesTab_Employees").append("<p><span class='site-field-label-primary'>Profile Page:</span> <span>" + field.LinkedInPage + "</span></p>");
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
            DisplayLinkedInCompanyInfo(obj);
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    getUrl();
    //xhr.send(JSON.stringify({ location: currentUrl, document: currentDoc, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
    xhr.send(JSON.stringify({ location: currentUrl, inputDto: linkedInDto, document: null, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
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
