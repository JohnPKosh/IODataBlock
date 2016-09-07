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
        Specialties: GetSpecialties(),
        StreetAddress: GetStreetAddress(),
        Locality: GetLocality(),
        Region: GetRegion(),
        PostalCode: GetPostalCode(),
        CountryName:GetCountryName(),
        Website: GetWebsite(),
        Industry: GetIndustry(),
        Type: GetType(),
        CompanySize: GetCompanySize(),
        Founded: GetFounded(),
        FollowersText: GetFollowersText(),
        PhotoUrl: GetPhotoUrl(),
        CompanyDescription: GetCompanyDescription(),
        FollowUrl: GetFollowUrl()
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
        return $(".adr .region").first().text().trim();
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

/* Get the Type */
function GetType() {
    try {
        return $(".type p").first().text().trim();
    } catch (err) {
        console.log("ERR GetType: " + err);
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

/* Get the FollowersText */
function GetFollowersText() {
    try {
        return $(".followers-count").first().text().trim();
    } catch (err) {
        console.log("ERR GetFollowersText: " + err);
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
        console.log("StreetAddress = " + dto.StreetAddress);
        console.log("Locality = " + dto.Locality);
        console.log("Region = " + dto.Region);
        console.log("PostalCode = " + dto.PostalCode);
        console.log("CountryName = " + dto.CountryName);
        console.log("Website = " + dto.Website);
        console.log("Industry = " + dto.Industry);
        console.log("Type = " + dto.Type);
        console.log("CompanySize = " + dto.CompanySize);
        console.log("Founded = " + dto.Founded);
        console.log("FollowersText = " + dto.FollowersText);
        console.log("PhotoUrl = " + dto.PhotoUrl);
        console.log("CompanyDescription = " + dto.CompanyDescription);
        console.log("FollowUrl = " + dto.FollowUrl);

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


