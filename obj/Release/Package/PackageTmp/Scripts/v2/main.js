var main = {
    catalogId : 0,
    degreeId : 0,
    institutionId : 0,
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].toLowerCase().split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getYesNo: function (val) {
        return (val) ? "Yes" : "No";
    },
    init: function () {
        main.degreeId = (this.getUrlVars()["degreeid"] > 0) ? this.getUrlVars()["degreeid"] : 0;
        main.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
    }
}
$(function () {
    main.init();
})