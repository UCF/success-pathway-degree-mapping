var degreeList = {
    hostname : "",
    displayDegreeList: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (data[x].Degrees.length > 0) {
                //console.log(data[x].Degrees);
                for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                    if (data[x].Degrees[y].Institution.toLowerCase() == "generic") {
                        let qrystring = '?degreeid=' + data[x].Degrees[y].Id;
                        qrystring += "&institutionid=" + data[x].Degrees[y].InstitutionId;
                        output += this.template("/degreemap" + qrystring, data[x].Degrees[y].Degree);
                    }
                }
            }
        }
        $("#DegreeListOutput").html(output);
    },
    template: function (url, title) {
        let template = '<div class="py-2"><a target="_blank" href="' + this.hostname + url + '" title="' + title + '">' + title + '</a></div>';
        return template;
    },
    getDegreeList: function () {
        $.get({
            url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetDegreeList",
            //url: "/api/degree/GetDegreeList",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                degreeList.displayDegreeList(data);
            }
        })
    },
    getTrueFalse: function (val) {
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
        this.setHost();
        this.getDegreeList();
    }
}
$(document).ready(function () {
    degreeList.init();
})