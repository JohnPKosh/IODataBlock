/* Initialize collection of hrefs */
var pageLinks = null;
function InitializePageLinksCollection() {
    pageLinks = $("[href]");
}

/* Main DTO page DOM builder function */
function GetPageDto() {
    var rv = {
        ProfileId: GetProfileId(),
        ProfileUrl: GetProfileUrl(),
        FullName: GetFullName(),
        Connections: GetConnections(),
        Title: GetTitle(),
        CompanyName: GetCompanyName(),
        CompanyId: GetCompanyId(),
        Industry: GetIndustry(),
        Location: GetLocation(),
        Email: GetEmail(),
        Im: GetIm(),
        Twitter: GetTwitter(),
        Address: GetAddress(),
        Phone: GetPhone(),
        PhotoUrl: GetPhotoUrl()
    };
    return rv;
};

/* Get the ProfileId */
function GetProfileId() {
    try {
        return $(".masthead").attr("id").replace("member-", "");
    } catch (err) {
        console.log("ERR GetProfileId: " + err);
        return null;
    }
};

/* Get the ProfileUrl */
function GetProfileUrl() {
    try {
        return $(".view-public-profile").attr("href");
    } catch (err) {
        console.log("ERR GetProfileUrl: " + err);
        return null;
    }
};

/* Get the FullName */
function GetFullName() {
    try {
        return $(".full-name").text().trim();
    } catch (err) {
        console.log("ERR FullName: " + err);
        return null;
    }
};

/* Get the Connections */
function GetConnections() {
    try {
        return $(".member-connections").text().replace("connections", "").replace("+", "").trim();
    } catch (err) {
        console.log("ERR GetConnections: " + err);
        return null;
    }
};

/* Get the Title */
function GetTitle() {
    try {
        //<p class="title" dir="ltr">President and CEO at CloudRoute, LLC</p>
        var titlestr = $(".title").first().text().trim();
        if (titlestr.indexOf(" at ") > -1) {
            var end = titlestr.lastIndexOf(" at ");
            return titlestr.substring(0, end);
        } else {
            return titlestr;
        }
    } catch (err) {
        console.log("ERR GetTitle: " + err);
        return null;
    }
};

/* Get the CompanyName */
function GetCompanyName() {
    try {
        var rv = null;
        $.each(pageLinks, function (i, field) {
            try {
                var f = field;
                var lowerUrl = f.href.toLowerCase();
                if (lowerUrl.indexOf("/company/") > -1 & lowerUrl.indexOf("?trk=prof-0-ovw-curr_pos") > -1) {
                    rv = f.text;
                    if (f.text) {
                        //console.log("GetCompanyName loop 1: " + f.text);
                         return false;
                    }
                }
            } catch (e) {
                console.log("ERR GetCompanyName loop 1: " + e);
            }
        });
        if (rv == null) {
            $.each(pageLinks, function (i, elem) {
                try {
                    var f = elem;
                    var lowerUrl = f.href.toLowerCase();
                    if (lowerUrl.indexOf("/company/") > -1 & lowerUrl.indexOf("?trk=prof-exp-company-name") > -1) {
                        rv = f.text;
                        if (f.text) {
                            //console.log("GetCompanyName loop 2: " + f.text);
                            return false;
                        }
                    }
                } catch (e) {
                    console.log("ERR GetCompanyName loop 2: " + e);
                }
            });
        }
        if (rv == null) {
            var titlestr = $(".title").text().trim();
            if (titlestr.indexOf(" at ") > -1) {
                var start = titlestr.lastIndexOf(" at ") + 4;
                return titlestr.substring(start, titlestr.length);
            } else {
                return titlestr;
            }
        }
        return rv;
    } catch (err) {
        console.log("ERR GetCompanyName: " + err);
        return null;
    }
};

/* Get the CompanyId */
function GetCompanyId() {
    try {
        var rv = null;
        //var links = $("[href]");
        $.each(pageLinks, function (i, field) {
            var lowerUrl = field.href.toLowerCase();
            if (lowerUrl.indexOf("/company/") > -1 & lowerUrl.indexOf("?trk=prof-0-ovw-curr_pos") > -1) {
                var start = lowerUrl.lastIndexOf("/company/") + "/company/".length;
                var end = lowerUrl.indexOf("?");
                rv = lowerUrl.substring(start, end);
                return false;
            }
        });
        if (rv == null) {
            //https://www.linkedin.com/company/10152667?trk=prof-exp-company-name
            $.each(pageLinks, function (i, field) {
                var lowerUrl = field.href.toLowerCase();
                if (lowerUrl.indexOf("/company/") > -1 & lowerUrl.indexOf("?trk=prof-exp-company-name") > -1) {
                    var start = lowerUrl.lastIndexOf("/company/") + "/company/".length;
                    var end = lowerUrl.indexOf("?");
                    rv = lowerUrl.substring(start, end);
                    return false;
                }
            });
        }
        return rv;
    } catch (err) {
        console.log("ERR GetCompanyId: " + err);
        return null;
    }
};

/* Get the Title */
function GetIndustry() {
    try {
        //<dd class="industry"><a href="/vsearch/p?f_I=6&amp;trk=prof-0-ovw-industry" name="industry" title="Find other members in this industry">Internet</a></dd>
        return $(".industry").text().trim();
    } catch (err) {
        console.log("ERR GetIndustry: " + err);
        return null;
    }
};

/* Get the Location */
function GetLocation() {
    try {
        //<span class="locality"><a href="/vsearch/p?countryCode=us&amp;trk=prof-0-ovw-location" name="location" title="Find other members in United States">United States</a></span>
        return $(".locality").first().text().trim();
    } catch (err) {
        console.log("ERR GetLocation: " + err);
        return null;
    }
};

/* Get the Email */
function GetEmail() {
    try {
        //<div id="email-view"><ul><li><a href="mailto:byrd_david@yahoo.com">byrd_david@yahoo.com</a></li></ul></div>
        return $("#email-view li > a").first().text().trim();
    } catch (err) {
        console.log("ERR GetEmail: " + err);
        return null;
    }
};

/* Get the Im */
function GetIm() {
    try {
        //<div id="im-view"><ul><li>john.kosh1 (Skype)</li></ul></div>
        return $("#im-view li").first().text().trim();
    } catch (err) {
        console.log("ERR GetIm: " + err);
        return null;
    }
};

/* Get the Twitter */
function GetTwitter() {
    try {
        //<div id="twitter-view"><ul><li><a href="/redir/redirect?url=http%3A%2F%2Ftwitter%2Ecom%2Fjgale540&amp;urlhash=riaR">jgale540</a></li></ul></div>
        return $("#twitter-view li > a").first().text().trim();
    } catch (err) {
        console.log("ERR GetTwitter: " + err);
        return null;
    }
};

/* Get the Address */
function GetAddress() {
    try {
        return $("#address").first().text().trim();
    } catch (err) {
        console.log("ERR GetAddress: " + err);
        return null;
    }
};


/* Get the Phones */
function GetPhone() {
    try {
        var rv = JoinPhoneElements($("#phone-view li").toArray());
        return rv;
    } catch (err) {
        console.log("ERR GetPhone: " + err);
        return null;
    }
};

/* Join Phone Elements Utility */
function JoinPhoneElements(elems) {
    try {
        var a = [];
        for (var i = 0; i < elems.length; i++) {
            a.push(elems[i].innerText);
        }
        return a.join(", ");
    } catch (err) {
        console.log("ERR JoinPhoneElements: " + err);
    }
};

/* Get the PhotoUrl */
function GetPhotoUrl() {
    try {
        //<div class="profile-picture"> <img src="https://media.licdn.com/media/p/3/000/2a8/15f/03d0286.jpg" alt="Giorgiy Shepov"><span></span></div>
        return $(".profile-picture img").attr("src");
    } catch (err) {
        console.log("ERR GetEmail: " + err);
        return null;
    }
};


// Call the specified callback, passing the web-page's DOM content back as argument
chrome.runtime.onMessage.addListener(function (msg, sender, sendResponse) {
    if (msg.text === "msgGetLinkedInProfileDto") {
        InitializePageLinksCollection();
        var dto = GetPageDto();
        
        console.log("ProfileId = " + dto.ProfileId);
        console.log("ProfileUrl = " + dto.ProfileUrl);
        console.log("FullName = " + dto.FullName);
        console.log("Connections = " + dto.Connections);
        console.log("Title = " + dto.Title);
        console.log("CompanyName = " + dto.CompanyName);
        console.log("CompanyId = " + dto.CompanyId);
        console.log("Industry = " + dto.Industry);
        console.log("Location = " + dto.Location);
        console.log("Email = " + dto.Email);
        console.log("Im = " + dto.Im);
        console.log("Twitter = " + dto.Twitter);
        console.log("Address = " + dto.Address);
        console.log("Phone = " + dto.Phone);
        console.log("PhotoUrl = " + dto.PhotoUrl);

        sendResponse(dto);
    }
});
