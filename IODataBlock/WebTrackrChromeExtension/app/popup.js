/* Create variables */
var currentUrl = "na";
var currentPageType = null;
var currentDoc = null;
var relatedEmployeesJson = null;
var linkedInCompanyId = null;
var linkedInDto = null;


/* Set up event handlers and inject send_links.js into all frames in the active tab. */
window.onload = function () {
    InitializePopup();
    InitializePopupClickEventsOnLoad();
    PopupOnLoad();

    $('a[data-toggle="pill"]').on("shown.bs.tab", function(e) {
        var currentTab = e.target // activated tab
        var divid = $(currentTab).attr("href").substr(1);
        if (divid === "LinkedInEmployeesTab") {
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
          DisplayLinkedInProfileInfo(linkedInDto);
          DisplayLinkedInCompanyInfo(json); /*TODO: Show content from API results.  If it is old data then decide what to do here. */
        })
      .fail(function (jqxhr, textStatus, error) {
          var err = textStatus + ", " + error;
          /*TODO: Show content from page and offer to Trak it.*/
          DisplayLinkedInProfileInfo(linkedInDto);
          $("#linkedInCompanyTab_LinkedInCompanyName").text("Not Tracking!");
          DisplayLinkedInCompanyDto(linkedInDto);
      });
}

function GetLinkedInCompany_callback(value) {
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
          $("#linkedInCompanyTab_LinkedInCompanyName").text("NOT Tracking!");
          DisplayLinkedInCompanyDto(linkedInDto);
        });
}

function DisplayLinkedInCompanyInfo(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName").html("<a href='javascript:void();'><img id='linkedInCompanyTab_LinkedInCompanyName_Icon' src='img/In-2C-21px-R.png'/></a> " + json.LinkedInCompanyName + " <span id='tracking_badge' class='label label-success'>Tracking</span>");
    $("#linkedInCompanyTab_LinkedInCompanyName_Icon").click(function () {
        chrome.tabs.create({ url: json.LinkedInPage });
    });
    $("#linkedInCompanyTab_industry").text(json.industry);
    $("#linkedInCompanyTab_type").text(json.type);
    $("#linkedInCompanyTab_companySize").text(json.companySize);
    $("#linkedInCompanyTab_founded").text(json.founded);
    $("#linkedInCompanyTab_followersCount").text(json.followersCount);
    $("#linkedInCompanyTab_website").text(json.website); 
    $("#linkedInCompanyTab_website").click(function () {
        chrome.tabs.create({ url: json.website });
    });
    $("#linkedInCompanyTab_LinkedInPage").text(json.LinkedInPage);
    $("#linkedInCompanyTab_LinkedInPage").click(function () {
        chrome.tabs.create({ url: json.LinkedInPage });
    });

    $("#linkedInCompanyTab_streetAddress").text(json.streetAddress);
    $("#linkedInCompanyTab_locality").text(json.locality);
    $("#linkedInCompanyTab_region").text(json.region);
    $("#linkedInCompanyTab_postalCode").text(json.postalCode);
    $("#linkedInCompanyTab_countryName").text(json.countryName);

    
    $("#linkedInCompanyTab_LinkedInLocationTitle").html("<a id='linkedInCompanyTab_LinkedInLocationLink' href='javascript:void();'><span class='glyphicon glyphicon-globe'></span></a> Location");
    $("#linkedInCompanyTab_LinkedInLocationLink").click(function () {
        chrome.tabs.create({ url: CreateGoogleMapsLink(json) });
    });
};

function DisplayLinkedInCompanyDto(json) {
    $("#linkedInCompanyTab_LinkedInCompanyName").html("<a href='javascript:void();'><img id='linkedInCompanyTab_LinkedInCompanyName_Icon' src='img/In-2C-21px-R.png'/></a> " + json.CompanyName + " <span id='tracking_badge' class='label label-default'>NOT Tracking</span>");
    $("#linkedInCompanyTab_LinkedInCompanyName_Icon").click(function () {
        chrome.tabs.create({ url: json.LocationUrl });
    });
    $("#linkedInCompanyTab_industry").text(json.Industry);
    //$("#linkedInCompanyTab_type").text(json.type);
    //$("#linkedInCompanyTab_companySize").text(json.companySize);
    //$("#linkedInCompanyTab_founded").text(json.founded);
    //$("#linkedInCompanyTab_followersCount").text(json.followersCount);
    //$("#linkedInCompanyTab_website").text(json.website);
    //$("#linkedInCompanyTab_website").click(function () {
    //    chrome.tabs.create({ url: json.website });
    //});
    //$("#linkedInCompanyTab_LinkedInPage").text(json.LinkedInPage);
    //$("#linkedInCompanyTab_LinkedInPage").click(function () {
    //    chrome.tabs.create({ url: json.LinkedInPage });
    //});

    //$("#linkedInCompanyTab_streetAddress").text(json.streetAddress);
    //$("#linkedInCompanyTab_locality").text(json.locality);
    //$("#linkedInCompanyTab_region").text(json.region);
    //$("#linkedInCompanyTab_postalCode").text(json.postalCode);
    //$("#linkedInCompanyTab_countryName").text(json.countryName);


    //$("#linkedInCompanyTab_LinkedInLocationTitle").html("<a id='linkedInCompanyTab_LinkedInLocationLink' href='javascript:void();'><span class='glyphicon glyphicon-globe'></span></a> Location");
    //$("#linkedInCompanyTab_LinkedInLocationLink").click(function () {
    //    chrome.tabs.create({ url: CreateGoogleMapsLink(json) });
    //});
};

function DisplayLinkedInProfileInfo(json) {
    $("#linkedInProfileTab_FullName").text(json.FullName);
    $("#linkedInProfileTab_Title").text(json.Title);
    $("#linkedInProfileTab_Location").text(json.Location);
    //$("#linkedInProfileTab_Email").text(json.Email);

    $("#linkedInProfileTab_Email").html("<a href='javascript:void();'>" + json.Email + "</a>");
    $("#linkedInProfileTab_Email").click(function () {
        chrome.tabs.create({ url: "mailto://" + json.Email });
    });


    $("#linkedInProfileTab_Im").text(json.Im);
    $("#linkedInProfileTab_Twitter").text(json.Twitter);
    $("#linkedInProfileTab_Phone").text(json.Phone);
};

function CreateGoogleMapsLink(json) {
    try {
        //https://www.google.com/maps/place/1910+Towne+Centre+Blvd,+Annapolis,+MD+21401
        var a = [];
        if (json.streetAddress) a.push(json.streetAddress.trim().replace(" ", "+"));
        if (json.locality) a.push(json.locality.trim().replace(" ", "+"));
        if (json.region) a.push(json.region.trim().replace(" ", "+"));
        if (json.postalCode) a.push(json.postalCode.trim().replace(" ", "+"));
        if (json.countryName) a.push(json.countryName.trim().replace(" ", "+"));
        return "https://www.google.com/maps/place/" + a.join(",");
    } catch (err) {
        console.log("ERR CreateGoogleMapsLink: " + err);
    }
}


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

function trackPagePOST() {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:51786/Api/TrakrScrape", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            var obj = JSON.parse(xhr.responseText);
            document.getElementById("resp").innerHTML = " <strong> Saved!</strong> ";
            DisplayLinkedInCompanyInfo(obj);
        }
    }
    xhr.setRequestHeader("Content-type", "application/json");
    //getUrl();
    //xhr.send(JSON.stringify({ location: currentUrl, document: currentDoc, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
    xhr.send(JSON.stringify({ location: currentUrl, inputDto: linkedInDto, document: null, apiKey: "4AC29893-E63A-42A9-B8A1-85180A330AAD" }));
}


/* Add logo click event to open home page. */
function InitializePopupClickEventsOnLoad() {
    $("#site-title").click(function () {
        chrome.tabs.create({ url: "http://localhost:51786/Home/Index" });
    });

    $("#trackPageButton").click(function () {
        trackPagePOST();
    });
}

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
        switch (currentPageType) {
            case "LinkedInCompany":
                $('#mainTabs a[href="#LinkedInProfileTab"]').hide();
                chrome.tabs.sendMessage(tab.id, { text: "msgGetLinkedInCompanyDto" }, GetLinkedInCompany_callback);
                break;
            case "LinkedInProfile":
                chrome.tabs.sendMessage(tab.id, { text: "msgGetLinkedInProfileDto" }, msgGetLinkedInProfileDto_callback);
                $('#mainTabs a[href="#LinkedInProfileTab"]').tab('show');
                break;
            default:
                /* TODO: perform some default action */
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
