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

    degreeId: 15,
    institutionId: 0,
    collegeId: 1,
    catalogId: 1,
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
    displayInstitutionList: function (data) {
        let output = '';
        for (var x = 0; x <= data.length - 1; x++) {
            //Need to select the degree for the new 
            output += '<a class="dropdown-item" href="/degreemap?degreeId=' + data[x].DegreeId + '">' + data[x].Institution + '</a>';
        }
        $('#' + this.target.InstitutionList).html(output);
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
            url: "/api/degree/GetDegreeInfo?degreeId=" + degreemap.degreeId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                console.log('degree map info');
                console.log(data);
                //console.log('--> ' + data.Id);
                degreemap.data = data;
                degreemap.displayDegreeInfo(data);
                degreemap.ucfDegreeId = data.UCFDegreeId;
                degreemap.getListByUCFDegree();
            }
        })
    },
    getListByUCFDegree: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/GetListByUCFDegree?degreeId="+degreemap.degreeId,
            url: "/api/degree/GetListByUCFDegree?ucfDegreeId=" + degreemap.ucfDegreeId + "&catalogId=" + degreemap.catalogId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                console.log(data);
                degreemap.displayInstitutionList(data);
            }
        })
    },
    getCritialCourseIcon() {
        return "*";
    },
    getRequiredCourseIcon() {
        return "~";
    },
    getCPPIcon() {
        return "+";
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
        degreemap.degreeId = main.degreeId;
        degreemap.institutionId = main.institutionId;
        degreemap.getDegreeInfo();
    }
}
$(function () {
    degreemap.init();
})
