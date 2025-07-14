function getdata(file) {

    var iframe = document.createElement("iframe");
    iframe.src = "../handler/excel.ashx?file=" + file;
    iframe.style.display = "none";
    document.body.appendChild(iframe);
}

// datepart: 'y', 'm', 'w', 'd', 'h', 'n', 's'
Date.dateDiff = function (datepart, fromdate, todate) {
    datepart = datepart.toLowerCase();
    var diff = todate - fromdate;
    var divideBy = { w: 604800000,
        d: 86400000,
        h: 3600000,
        n: 60000,
        s: 1000
    };
    return Math.floor(diff / divideBy[datepart]);
}