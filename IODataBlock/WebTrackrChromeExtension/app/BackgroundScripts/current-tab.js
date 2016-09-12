//// Check if email addresses are available for the current domain and update the color of the browser icon
////
//function sendTabUrl() {
//  chrome.tabs.query(
//    {currentWindow: true, active : true},
//    function (tabArray) {
//        SendData(tabArray[0]);
//      if (tabArray[0]["url"] != window.currentDomain) {
//        window.currentDomain = url_domain(tabArray[0]["url"]).replace("www.", "");
//          //updateIconColor(); 
//          /* TODO: rework this file to update icon with Tracked Data from API? */
//      }
//    }
//  );
//}


//// When an URL changes
////
//chrome.tabs.onUpdated.addListener(function(tabId, changeInfo, tab) {
//    sendTabUrl();
//});


//// When active tab changes
////
//chrome.tabs.onActivated.addListener(function(tabId, changeInfo, tab) {
//    sendTabUrl();
//});


//// API call to check if there is at least one email address
////
//function updateIconColor() {
//  $.ajax({
//    url : 'https://api.emailhunter.co/v1/email-count?domain=' + window.currentDomain,
//    type : 'GET',
//    success : function(response){
//      if (response.count > 0) { setColoredIcon(); }
//      else { setGreyIcon(); }
//    },
//    error : function() {
//      setColoredIcon();
//    }
//  });
//}

//function setGreyIcon() {
//    console.log("setGreyIcon called!");
//    chrome.browserAction.setBadgeText({ text: 'No' });
//    //chrome.browserAction.setIcon({
//    //  path : {
//    //    "19": chrome.extension.getURL("shared/img/icon19_grey.png"),
//    //    "38": chrome.extension.getURL("shared/img/icon38_grey.png")
//    //  }
//    //});
//}

//function setColoredIcon() {
//    console.log("setColorIcon called!");
//    chrome.browserAction.setBadgeText({ text: 'Yes' });
//    //chrome.browserAction.setIcon({
//    //  path : {
//    //    "19": chrome.extension.getURL("shared/img/icon19.png"),
//    //    "38": chrome.extension.getURL("shared/img/icon38.png")
//    //  }
//    //});
//}

//function url_domain(data) {
//  var    a      = document.createElement('a');
//         a.href = data;
//  return a.hostname;
//}

//document.body.onload = function () {

//    chrome.storage.local.set({ 'linkedInCompanyDto': "hello" }, function () {
//        console.log("linkedInCompanyDto saved.");
//        //alert("linkedInCompanyDto saved.");
//    });
//}