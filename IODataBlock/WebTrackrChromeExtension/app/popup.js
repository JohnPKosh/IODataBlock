

var allLinks = [];
var visibleLinks = [];
var currentUrl = "na";
var currentDoc = null;

/* Display all visible links. */
function showLinks() {
    var linksTable = document.getElementById("links");
    while (linksTable.children.length > 1) {
        linksTable.removeChild(linksTable.children[linksTable.children.length-1]);
    }
    for (var i = 0; i < visibleLinks.length; ++i) {
        var row = document.createElement("tr");
        var col0 = document.createElement("td");
        var col1 = document.createElement("td");
        col1.id = "item" + i;
        var checkbox = document.createElement("input");
        checkbox.checked = true;
        checkbox.type = "checkbox";
        checkbox.id = "check" + i;
        col0.appendChild(checkbox);
        //col1.innerText = visibleLinks[i];
        col1.innerHTML = "<a href='#'>" + visibleLinks[i] + "</a>";

        AddLinkEvent(col1, i);

        
        //col1.style.whiteSpace = "nowrap";
        //col1.onclick = function () {
        //    checkbox.checked = !checkbox.checked;
        //    //chrome.tabs.create({ url: col1.innerText });
        //}
        row.appendChild(col0);
        row.appendChild(col1);
        linksTable.appendChild(row);
    }
}

function AddLinkEvent(elem, i) {
    elem.onclick = function() {
        chrome.tabs.create({ url: visibleLinks[i] });
    };
}

function AddOpenHomePageEvent() {
    var siteTitle = document.getElementById("site-title");
    siteTitle.onclick = function () {
        chrome.tabs.create({ url: "http://localhost:54438/Home/Index" });
    };
}

//function showLinks() {
//    var linksTable = document.getElementById("links");
//    while (linksTable.children.length > 1) {
//        linksTable.removeChild(linksTable.children[linksTable.children.length - 1]);
//    }
//    for (var i = 0; i < visibleLinks.length; ++i) {
//        var row = document.createElement("tr");
//        var col0 = document.createElement("td");
//        var col1 = document.createElement("td");
//        var checkbox = document.createElement("input");
//        checkbox.checked = true;
//        checkbox.type = "checkbox";
//        checkbox.id = "check" + i;
//        col0.appendChild(checkbox);
//        col1.innerText = visibleLinks[i];
//        col1.style.whiteSpace = "nowrap";
//        col1.onclick = function () {
//            checkbox.checked = !checkbox.checked;
//        }
//        row.appendChild(col0);
//        row.appendChild(col1);
//        linksTable.appendChild(row);
//    }
//}

/* Toggle the checked state of all visible links.*/
function toggleAll() {
    var checked = document.getElementById("toggle_all").checked;
    for (var i = 0; i < visibleLinks.length; ++i) {
        document.getElementById("check" + i).checked = checked;
    }
}

/* Download all visible checked links. */
function downloadCheckedLinks() {
    for (var i = 0; i < visibleLinks.length; ++i) {
        if (document.getElementById("check" + i).checked) {
            alert(visibleLinks[i]);
            chrome.downloads.download({ url: visibleLinks[i] }, function (id) {});
        }
    }
    window.close();
}

function sendData() {
    chrome.windows.getCurrent(function (currentWindow) {
        chrome.tabs.query({ active: true, windowId: currentWindow.id },
                          function (activeTabs) {
                              alert("hello");
                              chrome.tabs.executeScript(activeTabs[0].id, { file: "send_url.js", allFrames: false });
                          });
    });
};

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

/* Add links to allLinks and visibleLinks, sort and show them.  send_links.js is injected into all frames of the active tab, so this listener may be called multiple times. */
chrome.extension.onRequest.addListener(function (data) {
    if (data.action === "send_links") {
        for (var index in data.links) {
            allLinks.push(data.links[index]);
        }
        allLinks.sort();
        visibleLinks = allLinks;
        //showUrl();
        showLinks();
        //showResp();
        currentDoc = data.body;
        //currentUrl = data.url;
        //showData();
        getMachineInfo();
    }

});

/* Set up event handlers and inject send_links.js into all frames in the active tab. */
window.onload = function () {
    AddOpenHomePageEvent();
    document.getElementById("filter").onkeyup = filterLinks;
    document.getElementById("regex").onchange = filterLinks;
    document.getElementById("toggle_all").onchange = toggleAll;
    //document.getElementById("download0").onclick = downloadCheckedLinks;
    document.getElementById("download0").onclick = showData;
    document.getElementById("download1").onclick = downloadCheckedLinks;
    //document.getElementById("sendData").onclick = sendData;

    chrome.windows.getCurrent(function (currentWindow) {
        chrome.tabs.query({ active: true, windowId: currentWindow.id },
                          function (activeTabs) {
                              currentUrl = activeTabs[0].url;
                              chrome.tabs.executeScript(activeTabs[0].id, { file: "send_links.js", allFrames: false });
                          });
    });
};



/* Display all visible links. */
function showResp() {

    var xhr = new XMLHttpRequest();
    xhr.open("GET", "http://localhost:57754/test", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            // innerText does not let the attacker inject HTML elements.
            document.getElementById("resp").innerHTML = xhr.responseText;
        }
    }
    xhr.send();
}

function getMachineInfo() {

    var xhr = new XMLHttpRequest();
    xhr.open("GET", "http://localhost:50231/api/Values", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            var myArr = JSON.parse(xhr.responseText);
            showMachineInfo(myArr);
        } 
    }
    xhr.send();
}

function showMachineInfo(arr) {
    var out = "<ul>";
    var i;
    for (i = 0; i < arr.length; i++) {
        //out += '<a href="' + arr[i].url + '">' + arr[i].display + '</a><br>';
        out += "<li>" + arr[i] + "</li>";
    }
    out += "</ul>";
    document.getElementById("machineInfo").innerHTML = out;
}

function showData() {
    var xhr = new XMLHttpRequest();
    //xhr.open("POST", "http://localhost:50231/api/Values/PostBody", true);
    xhr.open("POST", "http://localhost:54438/Api/TrakrScrape", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            //document.getElementById("resp").innerHTML = xhr.responseText;
            var obj = JSON.parse(xhr.responseText);
            document.getElementById("resp").innerHTML = obj.LocationUrl + " <strong> Saved!</strong>";
        }
    }
    //var input = document.documentElement.outerHTML;
    xhr.setRequestHeader("Content-type", "application/json");
    //getUrl();
    //getCurrentDoc();
    xhr.send(JSON.stringify({ location: currentUrl, document: currentDoc, links: allLinks }));
}


function showUrl() {
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        var current = tabs[0];
        incognito = current.incognito;
        url = current.url;
        alert(url);
    });
}

function getUrl() {
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        var current = tabs[0];
        url = current.url;
        currentUrl = url;
    });
}

function getCurrentDoc() {
    chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
        var current = tabs[0];
        currentDoc = current.document.documentElement.outerHTML;
    });
}

/* Utility Functions */

var nonCompanyDomains = {
    gmail: "",
    googlemail: "",
    hotmail: "",
    yahoo: "",
    aol: "",
    me: ""
};

var emailPattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

var addressPatterns = [
  /(P\.?O\.? Box\s)?(\d{1}).{0,20}\n?.{0,20}\n?.{0,20},\s(AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|DC|FM|GU|MH|MP|PW|PR|VI|Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New Hampshire|New Jersey|New Mexico|New York|North Carolina|North Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode Island|South Carolina|South Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West Virginia|Wisconsin|Wyoming|British Columbia|BC|Alberta|AB|Manitoba|MB|New Brunswick|NB|Newfoundland|NL|Northwest Territories|NT|Nova Scotia|NS|Nunavut|NU|Ontario|ON|Prince Edward Island|PE|Quebec|QC|Saskatchewan|SK|Yukon|YT)((\s|(\s.\s))((\d{5}(-\d{4})?)|(\w\d\w\s\d\w\d)))?/g, //If just one number, like 8, then make sure there's a comma right before state abbrev (more restrictive to avoid false positives) //Also this goes first, higher priority in some test cases needs to be caught first
  /(P\.?O\.? Box\s)?(\d{2,6}).{0,20}\n?.{0,20}\n?.{0,20}\s(AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|DC|FM|GU|MH|MP|PW|PR|VI|Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New Hampshire|New Jersey|New Mexico|New York|North Carolina|North Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode Island|South Carolina|South Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West Virginia|Wisconsin|Wyoming|British Columbia|BC|Alberta|AB|Manitoba|MB|New Brunswick|NB|Newfoundland|NL|Northwest Territories|NT|Nova Scotia|NS|Nunavut|NU|Ontario|ON|Prince Edward Island|PE|Quebec|QC|Saskatchewan|SK|Yukon|YT)((\s|(\s.\s))((\d{5}(-\d{4})?)|(\w\d\w\s\d\w\d)))?/g, //Base case
  /(P\.?O\.? Box\s)?(One|Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten).{0,30}\n?.{0,20}\n?.{0,20},\s(AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|DC|FM|GU|MH|MP|PW|PR|VI|Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New Hampshire|New Jersey|New Mexico|New York|North Carolina|North Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode Island|South Carolina|South Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West Virginia|Wisconsin|Wyoming|British Columbia|BC|Alberta|AB|Manitoba|MB|New Brunswick|NB|Newfoundland|NL|Northwest Territories|NT|Nova Scotia|NS|Nunavut|NU|Ontario|ON|Prince Edward Island|PE|Quebec|QC|Saskatchewan|SK|Yukon|YT)((\s|(\s.\s))((\d{5}(-\d{4})?)|(\w\d\w\s\d\w\d)))?/g //If One, Two, etc.. make sure there's a comma somewhere right before the state abbrev (same reason)
];

var phonePatterns = [
  // 10 digit phone numbers
  /(\+?1)?\W?\d{3}\W?\s?\d{3}\W?\d{4}/,
  // international
  /\(?\+?\d{1,3}\)?[\s|\.-]+\d{2,3}[\s|\.-]?\d{2,3}[\s|\.-]?\d{2,3}[\s|\.-]?\d{2,4}/,
  // Toll free and word combos
  /(1?.?(888|877|866|855|844|833|800))[-\s]?[\w\s-]{8}/
];

var zip_postal_code_regex = /((\d{5}(-\d{4})?)|(\w\d\w\s\d\w\d))/;

var state_province_regex = /\s(AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|DC|FM|GU|MH|MP|PW|PR|VI|Alabama|Alaska|Arizona|Arkansas|California|Colorado|Connecticut|Delaware|Florida|Georgia|Hawaii|Idaho|Illinois|Indiana|Iowa|Kansas|Kentucky|Louisiana|Maine|Maryland|Massachusetts|Michigan|Minnesota|Mississippi|Missouri|Montana|Nebraska|Nevada|New Hampshire|New Jersey|New Mexico|New York|North Carolina|North Dakota|Ohio|Oklahoma|Oregon|Pennsylvania|Rhode Island|South Carolina|South Dakota|Tennessee|Texas|Utah|Vermont|Virginia|Washington|West Virginia|Wisconsin|Wyoming|British Columbia|BC|Alberta|AB|Manitoba|MB|New Brunswick|NB|Newfoundland|NL|Northwest Territories|NT|Nova Scotia|NS|Nunavut|NU|Ontario|ON|Prince Edward Island|PE|Quebec|QC|Saskatchewan|SK|Yukon|YT)(?!\w)/;

//var city_regex = /Abilene|Akron|Albuquerque|Alexandria|Allentown|Amarillo|Anaheim|Anchorage|Ann Arbor|Antioch|Arlington|Arlington|Arvada|Athens|Atlanta|Augusta|Aurora|Aurora|Austin|Bakersfield|Baltimore|Baton Rouge|Beaumont|Bellevue|Berkeley|Billings|Birmingham|Boise|Boston|Bridgeport|Brownsville|Buffalo|Burbank|Cambridge|Cape Coral|Carlsbad|Carrollton|Cary|Cedar Rapids|Centennial|Chandler|Charleston|Charlotte|Chattanooga|Chesapeake|Chicago|Chula Vista|Cincinnati|Clarksville|Clearwater|Cleveland|Colorado Springs|Columbia|Columbia|Columbus|Columbus|Concord|Coral Springs|Corona|Corpus Christi|Costa Mesa|Dallas|Daly City|Dayton|Denton|Denver|Des Moines|Detroit|Downey|Durham|El Monte|El Paso|Elgin|Elizabeth|Elk Grove|Erie|Escondido|Eugene|Evansville|Everett|Fairfield|Fargo|Fayetteville|Flint|Fontana|Fort Collins|Fort Lauderdale|Fort Wayne|Fort Worth|Fremont|Fresno|Frisco|Fullerton|Gainesville|Garden Grove|Garland|Gilbert|Glendale|Glendale|Grand Prairie|Grand Rapids|Green Bay|Greensboro|Gresham|Hampton|Hartford|Hayward|Henderson|Hialeah|High Point|Hollywood|Honolulu|Houston|Huntington Beach|Huntsville|Independence|Indianapolis|Inglewood|Irvine|Irving|Jackson|Jacksonville|Jersey City|Joliet|Kansas City|Kansas City|Killeen|Knoxville|Lafayette|Lakewood|Lancaster|Lansing|Laredo|Las Vegas|Lexington|Lincoln|Little Rock|Long Beach|Los Angeles|Louisville|Lowell|Lubbock|Madison|Manchester|McAllen|McKinney|Memphis|Mesa|Mesquite|Miami|Miami Gardens|Midland|Milwaukee|Minneapolis|Miramar|Mobile|Modesto|Montgomery|Moreno Valley|Murfreesboro|Murrieta|Naperville|Nashville|New Haven|New Orleans|New York|Newark|Newport News|Norfolk|Norman|North Las Vegas|Norwalk|Oakland|Oceanside|Oklahoma City|Olathe|Omaha|Ontario|Orange|Orlando|Overland Park|Oxnard|Palm Bay|Palmdale|Pasadena|Pasadena|Paterson|Pembroke Pines|Peoria|Peoria|Philadelphia|Phoenix|Pittsburgh|Plano|Pomona|Port Saint Lucie|Portland|Providence|Provo|Pueblo|Raleigh|Rancho Cucamonga|Reno|Richmond|Richmond|Riverside|Rochester|Rochester|Rockford|Roseville|Sacramento|Saint Louis|Saint Paul|Saint Petersburg|Salem|Salinas|Salt Lake City|San Antonio|San Bernardino|San Buenaventura|San Diego|San Francisco|San Jose|Santa Ana|Santa Clara|Santa Clarita|Santa Rosa|Savannah|Scottsdale|Seattle|Shreveport|Simi Valley|Sioux Falls|South Bend|Spokane|Springfield|Springfield|Springfield|Stamford|Sterling Heights|Stockton|Sunnyvale|Surprise|Syracuse|Tacoma|Tallahassee|Tampa|Temecula|Tempe|Thornton|Thousand Oaks|Toledo|Topeka|Torrance|Tucson|Tulsa|Vallejo|Vancouver|Victorville|Virginia Beach|Visalia|Waco|Warren|Washington|Waterbury|West Covina|West Jordan|West Valley City|Westminster|Wichita|Wichita Falls|Wilmington|Winston-Salem|Worcester|Yonkers|Shanghai|Karachi|Istanbul|Mumbai|Beijing|Moscow|Sao Paulo|Tianjin|Guangzhou|Delhi|Seoul|Shenzhen|Jakarta|Tokyo|Mexico City|Kinshasa|Bangalore|Dongguan|New York City|Lagos|London|Lima|Tehran|Ho Chi Minh City|Hong Kong|Bangkok|Dhaka|Hyderabad|Cairo|Hanoi|Wuhan|Rio de Janeiro|Lahore|Ahmedabad|Baghdad|Riyadh|Singapore|Santiago|Saint Petersburg|Chennai|Chongqing|Kolkata|Surat|Yangon|Ankara|Alexandria|Shenyang|New Taipei City|Johannesburg|Los Angeles|Yokohama|Abidjan|Busan|Cape Town|Durban|Jeddah|Berlin|Pyongyang|Madrid|Nairobi|Pune|Jaipur|Casablanca|Paris/;

//country_regex updated 18 April 2016 https://www.cia.gov/library/publications/the-world-factbook/rankorder/2004rank.html
var country_regex = /US|USA|United States|Canada|China|Denmark|India|United States|Indonesia|Brazil|Pakistan|Nigeria|Russia|Bangladesh|Japan|Mexico|Philippines|Vietnam|Ethiopia|Germany|Egypt|Iran|Turkey|Thailand|Congo|France|United Kingdom|Italy|South Africa|South Korea|Myanmar|Colombia|Spain|Ukraine|Tanzania|Argentina|Kenya|Poland|Algeria|Canada|Uganda|Morocco|Afghanistan|Iraq|Sudan|Peru|Venezuela|Malaysia|Uzbekistan|Saudi Arabia|Nepal|Ghana|North Korea|Yemen|Taiwan|Israel|France|Bahrain|Qatar|Luxembourg|Liechtenstein|Macau|Singapore|Bermuda|Isle of Man|Brunei|Monaco|Kuwait|Norway|United Arab Emirates|Sint Maarten|Australia|San Marino|Switzerland|Hong Kong|Jersey|United States|Falkland Islands|Islas Malvinas|Saudi Arabia|Ireland|Guernsey|Bahrain|Netherlands|Sweden|Austria|Taiwan|Germany|Iceland|Oman|Canada|Denmark|Belgium|Cayman Islands|Gibraltar|British Virgin Islands|France|Finland|United Kingdom|New Caledonia|Japan|Greenland|European Union|Andorra|South Korea|New Zealand|Virgin Islands|Italy|Spain|Saint Pierre and Miquelon|Malta|Israel|Equatorial Guinea|Trinidad and Tobago|Czech Republic|Cyprus|Slovenia|Faroe Islands|Guam|Slovakia|Turks and Caicos Islands|Estonia|Puerto Rico|Lithuania|Portugal|Seychelles|Malaysia|Poland|French Polynesia|Hungary|Greece|Bahamas|Aruba|Kazakhstan|Latvia|Chile|Antigua and Barbuda|Russia|Saint Kitts and Nevis|Argentina|Uruguay|Gabon|Croatia|Panama|Romania|Turkey|Mauritius|Saint Martin|Azerbaijan|Lebanon|Mexico|Bulgaria|Belarus|Iran|Botswana|Barbados|Suriname|Venezuela|Thailand|Brazil|Montenegro|Turkmenistan|Iraq|Costa Rica|Libya|Curacao|Dominican Republic|Nauru|Palau|Algeria|China|Colombia|Macedonia|Maldives|Serbia|South Africa|Northern Mariana Islands|American Samoa|Grenada|Mongolia|Jordan|Cook Islands|Peru|Saint Lucia|Albania|Dominica|Tunisia|Egypt|Ecuador|Indonesia|Namibia|Sri Lanka|Saint Vincent and the Grenadines|Cuba|Bosnia|Herzegovina|Swaziland|Georgia|Paraguay|Jamaica|Fiji|Belize|Montserrat|Armenia|El Salvador|Morocco|Bhutan|Ukraine|Guatemala|Saint Helena|Ascension|Tristan da Cunha|Angola|Philippines|Guyana|Republic of the Congo|Cabo Verde|Bolivia|Nigeria|India|Vietnam|Uzbekistan|Niue|Timor-Leste|Laos|Samoa|Burma|Tonga|Syria|Honduras|Nicaragua|Moldova|Pakistan|Mauritania|Sudan|West Bank|Ghana|Zambia|Wallis and Futuna|Bangladesh|Cambodia|Sao Tome and Principe|Tuvalu|Marshall Islands|Kyrgyzstan|Cote d'Ivoire|Kenya|Djibouti|Cameroon|Tanzania|Lesotho|Federated States of Micronesia|Tajikistan|Papua New Guinea|Chad|Yemen|Vanuatu|Senegal|Nepal|Western Sahara|Kiribati|Zimbabwe|Uganda|Afghanistan|South Sudan|Solomon Islands|Benin|Rwanda|Burkina Faso|Mali|North Korea|Haiti|Gambia, The|Ethiopia|Comoros|Sierra Leone|Togo|Guinea-Bissau|Madagascar|Mozambique|Guinea|Malawi|Eritrea|Niger|Tokelau|Liberia|Burundi|Democratic Republic of the Congo|Central African Republic|Somalia|Kosovo|Anguilla"/;

var sendLocales = [
  "send",
  "invia", //italian
  "envoyer", //french
  "enviar", //spanish and portuguese
  "senden" //german
];

var titlePrefixes = [
  "[co-]*found[ing|er|ed]*",
  "cofound[ing|er|ed]*",
  "contract[ing|or|ed]*",
  "lead[ing|er]*",
  "VP",
  "manag[ing|er]*",
  "[web\\s|software\\s|app\\s]*develop[ing|er]*",
  "engineer",
  "work[ing|er]*?",
  "editor",
  "design[ing|er]*",
  "direct[ing|or]*",
  "officer",
  "educator",
  "partner",
  "recruiter",
  "associate",
  "(?:\\w+\\s?){0,2}analyst",
  "accountant",
  "(?:\\w+\\s?){0,2}operations",
  "[\\W]?C.O",
  "head of (?:\\w+\\s?){1,2}"
];

var titleDepartments = [
  "art",
  "design",
  "general",
  "product",
  "project",
  "program",
  "marketing",
  "community",
  "engineering",
  "support",
  "quality assurance",
  "talent"
];

function getFirstEmail(str) {
    if (!str) { return str; }
    var match = str.match(/<(.+)>/);
    if (match && match.length) {
        return match[1]; // without "<" and ">"
    }
    match = str.match(emailPattern);
    if (match && match.length) {
        return match[0];
    }
    return null;
}

function getFirstEntryInToField(to_field) {
    var first_entry;

    var emailPattern_before_comma = new RegExp(/.*?/.source + emailPattern.source + /.*?(?=,)/.source);
    var first_entry_array = emailPattern_before_comma.exec(to_field);

    if (first_entry_array) first_entry = first_entry_array[0];
    if (!first_entry_array) first_entry = to_field;

    return first_entry;
}

function getUsernameFromEmail(email_address) {
    var before_at_symbol = /.*?(?=@)/;
    var email_username = before_at_symbol.exec(email_address);
    if (email_username) {
        return email_username[0];
    } else {
        return null;
    }
}

function capitalizeFirstLetter(string) {
    if (string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    } else {
        return string;
    }
}

function ensureHTTP(string) {
    if (!string) return string;
    var prefix = string.slice(0, 4);
    if (prefix === "http") return string;
    return "http://" + string;
}

function getMatch(regex, text) {
    var match = regex.exec(text);
    if (match) return match[0];
    else return undefined;
}

function getTwitterURLFromHandle(twitter_handle) {
    if (!twitter_handle) return undefined;
    twitter_handle = twitter_handle.replace("@", "");
    return "http://twitter.com/" + twitter_handle;
}

function titleCase(str) {
    if (str.length < 4) return str.toUpperCase();
    return str.replace(/\w\S*/g, function (str) {
        return str.charAt(0).toUpperCase() + str.substr(1).toLowerCase();
    });
}

function findCompany(string) {
    if (!string) return "";

    if (string.indexOf(" at ") !== -1) {
        var parts = string.split(" at ");
        if (parts.length) {
            return titleCase($.trim(parts[parts.length - 1]));
        }
    }

    return "";
}

function findTitle(string, returnOriginal) {
    if (!string) {
        return (returnOriginal) ? string : "";
    }

    if (string.indexOf(" at ") !== -1) {
        var parts = string.split(" at ");
        if (parts.length) {
            return $.trim(parts[0]);
        }
    }

    return (returnOriginal) ? string : "";
}

function applyStylesToButton(button_obj, size, pos) {
    if (pos === null) pos = "relative";

    var css_obj = {
        cursor: "pointer",
        position: pos,
        display: "inline-block" //because we switched to a div not img
    };
    var hover_obj = {};
    var active_obj = {};

    switch (size) {
        case 20:
            css_obj.width = "20px";
            css_obj.height = "20px";
            css_obj.background = "url(" + chrome[runtimeOrExtension].getURL("") + "css/tofino/images/ecquire-button-20.png) no-repeat 0 0";
            css_obj["background-position"] = "0 0px";
            hover_obj["background-position"] = "0 -20px";
            active_obj["background-position"] = "0 -40px";
            break;
        default:
            css_obj.width = "27px";
            css_obj.height = "28px";
            css_obj.background = "url(" + chrome[runtimeOrExtension].getURL("") + "css/tofino/images/ecquire-button.png) no-repeat 0 0";
            css_obj["background-position"] = "0 0px";
            hover_obj["background-position"] = "0 -28px";
            active_obj["background-position"] = "0 -56px";
    }

    button_obj.css(css_obj);
    button_obj.hover(
      function () {
          if (!button_obj.hasClass("is_loading"))
              button_obj.css(hover_obj);
      },
      function () {
          if (!button_obj.hasClass("is_loading"))
              button_obj.css(css_obj);
      });

    button_obj.mousedown(function () {
        button_obj.css(active_obj);
    });

    button_obj.mouseup(function () {
        button_obj.css(css_obj);
    });
}

function convertNewlinesFromTags(s) {
    s = s || "";
    // spaces
    s = s.replace("&nbsp;", " ");
    // remove all closing divs
    s = s.replace(/<\/div>/g, "");
    // remove very first opening div
    s = s.replace(/<div>/, "");
    // replace opening divs with newline
    s = s.replace(/<div.*?>/g, "\n");
    // All br's become newlines
    s = s.replace(/<br ?\/?>/g, "\n");
    return $.trim(s);
}

function stripTags(s) {
    s = s || "";
    var temp_div = document.createElement("div");
    temp_div.innerHTML = s;
    s = temp_div.innerText;
    return s;
}

function findCompanyAndWebsiteFromEmail(object) {
    var domain_of_emailPattern = /@.*?(?=\.)/;
    var company_domain = domain_of_emailPattern.exec(object.email);
    if (company_domain) {
        company_domain = company_domain[0].replace("@", "");

        if (!(company_domain in nonCompanyDomains)) {
            object.company = capitalizeFirstLetter(company_domain);
            if (!object.website) try {
                object.website = "http://" + object.email.split("@")[1];
            } catch (e) { }
        }
    }
    return object;
}