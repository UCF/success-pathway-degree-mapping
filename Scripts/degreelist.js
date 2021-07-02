var degreeList = {
    hostname: "",
    catalogId: 1,
    catalogYear: "2020-2021",
    institutionId: 1,
    displayDegreeList: function (data) {
        $(".catalogYear").html(degreeList.catalogYear)
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            output += '<li>' + data[x].Degree + ' ' + data[x].DegreeId + '</li>';
            /*Figure out best way to display partner institutin that have this degree for this year*/
        }
        $("#DegreeListOutput").html("<ul>" + output + "</ul>");
    },
    getDegreeList: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetDegreeListv2",
            url: "/api/degree/GetListByInstitution?institutionId=" + degreeList.institutionId+"&catalogId="+degreeList.catalogId,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log('start');
                console.log(data);
                degreeList.displayDegreeList(data);
                //degreeList.displayDegreeList(data);
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