function closeMe() {
    if (window.opener != null) {
        window.opener.closewin();
    }
    self.close();
}

function cambio(cnt, valsapan) {
    if (cnt != undefined && cnt != null) {
        var ct = document.getElementById(valsapan);
        if (ct != undefined && ct != null) {

            if (cnt.checked) {
                ct.textContent = ' (Programado)'
                document.getElementById('dptipos').children[0].disabled = true;
                document.getElementById('dptipos').children[0].style.backgroundColor = '#FFE13C';
                document.getElementById('dptipos').selectedIndex = "1";

            } else {
                ct.textContent = ' (No programado)'
                document.getElementById('dptipos').children[0].disabled = false;
                document.getElementById('dptipos').children[0].style.backgroundColor = 'WHITE';
                document.getElementById('dptipos').selectedIndex = "0";
            }
        }
    }
}