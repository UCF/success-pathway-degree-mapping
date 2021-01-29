var degreeGeneric = {
    hostname: "",
    UCFDegreeName :"",
    displayUCFDegreeName: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (data[x].Degrees.length > 0) {
                if (data[x].Id == this.degreeId) {
                    this.UCFDegreeName = data[x].Degree;
                }
            }
        }
        $(".UCFDegreeName").html(this.UCFDegreeName);
    },
    displayPartnerInstitutions: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (data[x].Degrees.length > 0) {
                if (data[x].Id == this.degreeId) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        let qrystring = "?degreeid=" + data[x].Degrees[y].Id + "&institutionid=" + data[x].Degrees[y].InstitutionId;
                        output += this.template(this.hostname + "/degreemap"+qrystring, data[x].Degrees[y].Institution)
                    }
                }
            }
        }
        $("#output").html(output);
    },
    template: function (url, title) {
        let template = '<div><a href="' + url + '" title="' + title + '">' + title + '</a></div>';
        return template;
    },

    getDegreeList: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetDegreeList",
            url: "/api/degree/GetDegreeList",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                degreeGeneric.displayUCFDegreeName(data);
                degreeGeneric.displayPartnerInstitutions(data);
            }
        })
    },
    getTrueFalse: function (val) {
        return (val) ? "Yes" : "No";
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
        this.setHost();
        this.degreeId = (this.getUrlVars()["degreeid"] > 0) ? this.getUrlVars()["degreeid"] : 1;
        this.getDegreeList();
    }
}
$(document).ready(function () {
    degreeGeneric.init();
})