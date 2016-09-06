/* Initialize collection of hrefs */
var pageLinks = null;
function InitializePageLinksCollection() {
    pageLinks = $("[href]");
}

/* Main DTO page DOM builder function */
function GetPageDto() {
    var rv = {
        LocationUrl: GetLocationUrl(),
        CompanyId: GetCompanyId(),
        CompanyName: GetCompanyName(),
        DomainName: GetDomainName(),
        Specialties: GetSpecialties()
    };
    return rv;
};


/* Get the LocationUrl */
function GetLocationUrl() {
    try {
        return "https://www.linkedin.com" + $(location).attr("pathname");
    } catch (err) {
        console.log("ERR GetLocationUrl: " + err);
        return null;
    }
};

/* Get the ProfileId */
function GetCompanyId() {
    try {
        var loc = GetLocationUrl();
        return loc.substring(loc.lastIndexOf("/") + 1);
    } catch (err) {
        console.log("ERR GetProfileId: " + err);
        return null;
    }
};

/* Get the CompanyName */
function GetCompanyName() {
    try {
        //<h1 class="name" itemscope="" itemtype="http://schema.org/Corporation"><span dir="ltr" itemprop="name">CloudRoute</span></h1>
        return $(".name span[itemprop='name'").text().trim();
    } catch (err) {
        console.log("ERR CompanyName: " + err);
        return null;
    }
};

/* Get the DomainName */
function GetDomainName() {
    try {
        return new URL(GetWebsite()).hostname.toLowerCase().replace("www.", "");
    } catch (err) {
        console.log("ERR GetDomainName: " + err);
        return null;
    }
};

/* Get the Website */
function GetWebsite() {
    try {
        return $(".website p a[rel='nofollow'").text().trim();
    } catch (err) {
        console.log("ERR GetWebsite: " + err);
        return null;
    }
};

/* Get the Specialties */
function GetSpecialties() {
    try {
        return $(".specialties p").first().text().trim();
    } catch (err) {
        console.log("ERR GetSpecialties: " + err);
        return null;
    }
};


//StreetAddress



// Call the specified callback, passing the web-page's DOM content back as argument
chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
    if (msg.text === "msgGetLinkedInCompanyDto") {
        InitializePageLinksCollection();
        var dto = GetPageDto();

        console.log("LocationUrl = " + dto.LocationUrl);
        console.log("CompanyId = " + dto.CompanyId);
        console.log("CompanyName = " + dto.CompanyName);
        console.log("DomainName = " + dto.DomainName);
        console.log("Specialties = " + dto.Specialties);

        sendResponse(dto);
    }
});







//// Call the specified callback, passing the web-page's DOM content back as argument
//chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
//    if (msg.text === "find_companyId") {

//        var dto = GetPageDto();
//        console.log("LocationUrl = " + dto.LocationUrl);
//        console.log("CompanyId = " + dto.CompanyId);

//        sendResponse($(".follow-start:first").attr("href"));
//    }
//    //if (msg.text === "get_DOMElements") {
//    //    sendResponse(document);
//    //}
//});


