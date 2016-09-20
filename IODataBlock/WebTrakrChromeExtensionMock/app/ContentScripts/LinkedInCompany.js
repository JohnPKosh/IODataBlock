
/* Initialize collection of hrefs */
var pageLinks = null;
function InitializePageLinksCollection() {
    pageLinks = $("[href]");
}

/* Main DTO page DOM builder function */
function GetPageDto() {
    var rv = {
        LinkedInCompanyId: GetLinkedInCompanyId(),
        LinkedInCompanyUrl: GetLinkedInCompanyUrl(),
        LinkedInCompanyName: GetLinkedInCompanyName(),
        DomainName: GetDomainName(),
        Specialties: GetSpecialties(),
        StreetAddress: GetStreetAddress(),
        Locality: GetLocality(),
        Region: GetRegion(),
        PostalCode: GetPostalCode(),
        CountryName:GetCountryName(),
        Website: GetWebsite(),
        Industry: GetIndustry(),
        CompanyType: GetCompanyType(),
        CompanySize: GetCompanySize(),
        Founded: GetFounded(),
        FollowersCount: GetFollowersCount(),
        FollowUrl: GetFollowUrl(),
        PhotoUrl: GetPhotoUrl(),
        CompanyDescription: GetCompanyDescription()
};
    return rv;
};


/* Get the ProfileId */
function GetLinkedInCompanyId() {
    try {
        //var loc = GetLinkedInCompanyUrl();
        //return loc.substring(loc.lastIndexOf("/") + 1);
        return getQueryString("id", GetFollowUrl());
    } catch (err) {
        console.log("ERR GetLinkedInCompanyId: " + err);
        return null;
    }
};

/* Get the LinkedInCompanyUrl */
function GetLinkedInCompanyUrl() {
    try {
        return "https://www.linkedin.com" + $(location).attr("pathname");
    } catch (err) {
        console.log("ERR GetLinkedInCompanyUrl: " + err);
        return null;
    }
};

/* Get the LinkedInCompanyName */
function GetLinkedInCompanyName() {
    try {
        //<h1 class="name" itemscope="" itemtype="http://schema.org/Corporation"><span dir="ltr" itemprop="name">CloudRoute</span></h1>
        return $(".name span[itemprop='name'").text().trim();
    } catch (err) {
        console.log("ERR LinkedInCompanyName: " + err);
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

/* Get the StreetAddress */
function GetStreetAddress() {
    try {
        //<span class="street-address" itemprop="streetAddress">779 Carle Avenue</span>
        return $(".adr .street-address").first().text().trim();
    } catch (err) {
        console.log("ERR GetStreetAddress: " + err);
        return null;
    }
};

/* Get the Locality */
function GetLocality() {
    try {
        var loc = $(".adr .locality").first().text().trim();
        if (loc.lastIndexOf(",") === loc.length - 1) {
            return loc.substring(0, loc.lastIndexOf(","));
        } else {
            $(".adr .locality").first().text().trim();
        }
    } catch (err) {
        console.log("ERR GetLocality: " + err);
        return null;
    }
};

/* Get the Region */
function GetRegion() {
    try {
        return $(".adr .region").first().text().trim();
    } catch (err) {
        console.log("ERR GetRegion: " + err);
        return null;
    }
};

/* Get the PostalCode */
function GetPostalCode() {
    try {
        return $(".adr .postal-code").first().text().trim();
    } catch (err) {
        console.log("ERR GetPostalCode: " + err);
        return null;
    }
};

/* Get the CountryName */
function GetCountryName() {
    try {
        //<span class="country-name" itemprop="addressCountry">United States</span>
        return $(".country-name").first().text().trim();
    } catch (err) {
        console.log("ERR GetCountryName: " + err);
        return null;
    }
};

/* Get the Website */
function GetWebsite() {
    try {
        return $(".website p > a").first().text().trim();
    } catch (err) {
        console.log("ERR GetWebsite: " + err);
        return null;
    }
};

/* Get the Industry */
function GetIndustry() {
    try {
        return $(".industry p").first().text().trim();
    } catch (err) {
        console.log("ERR GetIndustry: " + err);
        return null;
    }
};

/* Get the CompanyType */
function GetCompanyType() {
    try {
        return $(".type p").first().text().trim();
    } catch (err) {
        console.log("ERR GetCompanyType: " + err);
        return null;
    }
};

/* Get the CompanySize */
function GetCompanySize() {
    try {
        return $(".company-size p").first().text().trim();
    } catch (err) {
        console.log("ERR GetCompanySize: " + err);
        return null;
    }
};

/* Get the Founded */
function GetFounded() {
    try {
        return $(".founded p").first().text().trim();
    } catch (err) {
        console.log("ERR GetFounded: " + err);
        return null;
    }
};

/* Get the FollowersCount */
function GetFollowersCount() {
    try {
        return $(".followers-count").first().text().replace("followers", "").replace("follower", "").replace(",", "").trim();
    } catch (err) {
        console.log("ERR GetFollowersCount: " + err);
        return null;
    }
};

/* Get the FollowUrl */
function GetFollowUrl() {
    try {
        //<a class="follow-start" href="https://www.linkedin.com/company/follow/submit?id=9403981&amp;fl=start&amp;ft=0_Y3kiyf9HWAmYDU6VAEIvRVAObMxlNM_IGCvo1b5lkvTF_k9D5fA1WjmaCTx-TCZL&amp;csrfToken=ajax%3A8554679308426727204&amp;trk=co_followed-start" role="button">Follow</a>
        return $(".follow-start").attr("href");
    } catch (err) {
        console.log("ERR GetFollowUrl: " + err);
        return null;
    }
};

/* Get the PhotoUrl */
function GetPhotoUrl() {
    try {
        //<div class="image-wrapper"><img src="https://media.licdn.com/media/AAEAAQAAAAAAAAJ6AAAAJGM4ZGZkYzFiLTE4ZDItNDcyYy05ZmZhLTcxYTM4MTJmMzdhMQ.png" alt="Royalty Tech LLC" class="image"></div>
        return $(".header .image-wrapper img").attr("src");
    } catch (err) {
        console.log("ERR GetPhotoUrl: " + err);
        return null;
    }
};

/* Get the CompanyDescription */
function GetCompanyDescription() {
    try {
        //<div class="image-wrapper"><img src="https://media.licdn.com/media/AAEAAQAAAAAAAAJ6AAAAJGM4ZGZkYzFiLTE4ZDItNDcyYy05ZmZhLTcxYTM4MTJmMzdhMQ.png" alt="Royalty Tech LLC" class="image"></div>
        return $(".basic-info-description").text().trim();
    } catch (err) {
        console.log("ERR GetCompanyDescription: " + err);
        return null;
    }
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



// Call the specified callback, passing the web-page's DOM content back as argument
chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
    if (msg.text === "msgGetLinkedInCompanyDto") {
        InitializePageLinksCollection();
        var dto = GetPageDto();

        console.log("LinkedInCompanyUrl = " + dto.LinkedInCompanyUrl);
        console.log("LinkedInCompanyId = " + dto.LinkedInCompanyId);
        console.log("LinkedInCompanyName = " + dto.LinkedInCompanyName);
        console.log("DomainName = " + dto.DomainName);
        console.log("Specialties = " + dto.Specialties);
        console.log("StreetAddress = " + dto.StreetAddress);
        console.log("Locality = " + dto.Locality);
        console.log("Region = " + dto.Region);
        console.log("PostalCode = " + dto.PostalCode);
        console.log("CountryName = " + dto.CountryName);
        console.log("Website = " + dto.Website);
        console.log("Industry = " + dto.Industry);
        console.log("CompanyType = " + dto.CompanyType);
        console.log("CompanySize = " + dto.CompanySize);
        console.log("Founded = " + dto.Founded);
        console.log("FollowersCount = " + dto.FollowersCount);
        console.log("FollowUrl = " + dto.FollowUrl);
        console.log("PhotoUrl = " + dto.PhotoUrl);
        console.log("CompanyDescription = " + dto.CompanyDescription);

        sendResponse(dto);
    }
});



//// Call the specified callback, passing the web-page's DOM content back as argument
//chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
//    if (msg.text === "find_companyId") {

//        var dto = GetPageDto();
//        console.log("LinkedInCompanyUrl = " + dto.LinkedInCompanyUrl);
//        console.log("LinkedInCompanyId = " + dto.LinkedInCompanyId);

//        sendResponse($(".follow-start:first").attr("href"));
//    }
//    //if (msg.text === "get_DOMElements") {
//    //    sendResponse(document);
//    //}
//});


//document.body.onload = function () {
//    var dto = GetPageDto();
//    var storeDto = JSON.stringify(dto).toString();
//    console.log(storeDto);

//    //chrome.storage.sync.clear(function () {
//    //    console.log("storage cleared.");
//    //    //alert("linkedInCompanyDto saved.");
//    //});
//    chrome.storage.sync.set({ 'linkedInCompanyDto': storeDto }, function () {
//        console.log("linkedInCompanyDto saved.");
//        //alert("linkedInCompanyDto saved.");
//    });
//    //chrome.storage.sync.set({ 'ehlo': "hello" }, function () {
//    //    console.log("hello saved.");
//    //    //alert("linkedInCompanyDto saved.");
//    //});
//}





