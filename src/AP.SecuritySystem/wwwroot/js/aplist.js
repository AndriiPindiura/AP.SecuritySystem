/// <reference path="jquery-2.1.0-vsdoc.js" />
$(function () {
    var select = document.getElementById('selectServertype');
    if (select != null) {
        document.getElementById('selectServertype').selectedIndex = $('#selectServertype').attr('data-mode');
            //alert($('#editServerForm').attr('data-serverId'));
    }
    // List selection change
    $('.list').on('click', 'div', function () {
        ap.selectedId = this.getAttribute('id');
    });

});