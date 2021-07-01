//https://tulsa.okstate.edu/transfer
var degreemap = {
    version: "2.0.0",
    data: {},
    target: {
        GPA: "GPA",
        LimitedAccess: "LimitedAccess",
        RestrictedAccess: "RestrictedAccess",
        AdditionalRequirement: "AdditionalRequirement",
        ForeignLanguageRequirement: "ForeignLanguageRequirement",
        ListItems: "ListItems",
        Institution: "Institution",
        InstitutionList: "InstitutionList",
        DegreemapRow: "DegreemapRow",
        Degree: "Degree",
        UCFPathwaySection: "PathwaySection",
        CollegeName: "CollegeName",
        UCFCourseSection: "UCFCourseSection",
    },

    degreeId: 0,
    institutionId: 0,
    collegeId: 0,
    catalogId: 0,
    ucfDegreeId: 0,
    hasUCFSemesters: false,

    displayDegreeInfo(data) {
        $("." + this.target.GPA).html(data.GPA)
        $("." + this.target.LimitedAccess).html(this.getYesNo(data.LimitedAccess));
        $("." + this.target.RestrictedAccess).html(this.getYesNo(data.RestrictedAccess));
        $("." + this.target.ForeignLanguageRequirement).html(data.ForeignLanguageRequirement);
        $("." + this.target.AdditionalRequirement).html(data.AdditionalRequirement);
        $("." + this.target.Degree).html(data.CatalogYear + ' ' + data.Degree);
        $("." + this.target.Institution).html(data.Institution);
    },
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
    getDegreeInfo: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/GetDegreeMap?degreeId="+degreemap.degreeId,
            url: "/api/degree/GetDegreeInfo?degreeId="+degreemap.degreeId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                console.log('--> ' + data.Id);
                degreemap.data = data;
                degreemap.displayDegreeInfo(data);
            }
        })
    },
    getYesNo: function (val) {
        return (val) ? "Yes" : "No";
    },
    setHost: function () {
        let host = window.location.hostname.toLowerCase();
        if (host == "portal.connect.ucf.edu") {
            host = "/pathway/home";
        }
        if (host == "localhost") {
            host = "/home";
        }
        this.hostname = host;
    },
    init: function () {
        this.degreeId = (this.getUrlVars()["degreeId"] > 0) ? this.getUrlVars()["degreeId"] : 15;
        this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //this.setHost();
        this.getDegreeInfo();
    }
}
$(function () {
    degreemap.init();
})