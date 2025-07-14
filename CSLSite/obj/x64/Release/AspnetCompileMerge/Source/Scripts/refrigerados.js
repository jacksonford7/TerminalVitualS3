var win;
function popUpCal(furl) {
    var url = furl;
    var width = 800;
    var height = 600;
    var left = parseInt((screen.availWidth / 2) - (width / 2));
    var top = parseInt((screen.availHeight / 2) - (height / 2));
    var windowFeatures = "width=" + width + ",height=" + height +
        ",status,resizable=no,left=" + left + ",top=" + top +
        "screenX=" + left + ",screenY=" + top + ",scrollbars=no,toolbar=no,menubar=no,dialog=yes";
    win = window.open(url, "subWind", windowFeatures, "POS");
}
function closewin() { win.close(); }