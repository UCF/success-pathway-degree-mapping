//https://tulsa.okstate.edu/transfer
var courseMapper = {
    version: "1.0.0",
    data: {},
    target: {

    },

    //courseMapper.data[0].UCFCourses[0].Course
    displayCourses: function (data) {
        let td1 = '';
        let td2 = '';
        let td3 = '';
        let td4 = '';
        let td5 = '';
        let tr = '';
        if (data.length > 0) {
            let course = '';
            let credit = '';
            let critical = '';
            let cpp = '';
            let required = '';
            for (let x = 0; x <= data.length - 1; x++) {
                if (data[0].UCFCourses.length > 0) {
                    for (let c = 0; c <= data[0].UCFCourses.length - 1; c++) {
                        console.log(data[x].UCFCourses[c].critical);
                        course += '<div>' + data[x].UCFCourses[c].Course + '</div>';
                        credit += '<div>' + data[x].UCFCourses[c].Credit + '</div>';
                        critical += '<div>' + courseMapper.getYesNo(data[x].UCFCourses[c].Critical) + '</div>';
                        cpp += '<div>' + courseMapper.getYesNo(data[x].UCFCourses[c].CPP) + '</div>';
                        required += '<div>' + courseMapper.getYesNo(data[x].UCFCourses[c].Required) + '</div>';
                        //let selectOne = data[x].SelectOne;
                    }
                    /*Next to Partner courses*/


                    tr += '<tr><td>' + course + '</td><td>' + credit + '</td><td>' + critical + '</td><td>' + cpp + '</td><td>' + required + '</td></tr>';
                    course = '';
                    credit = '';
                    critical = '';
                    cpp = '';
                    required = '';
                }
            }
            $('#coursesTable').html(tr);
        }
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
    getCourseMapper: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/GetCourseMapper?degreeId="+courseMapper.degreeId,
            url: "/api/degree/GetCourseMapper?degreeId=" + courseMapper.degreeId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                console.log(data);
                courseMapper.data = data;
                courseMapper.displayCourses(data);
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
        this.degreeId = (this.getUrlVars()["degreeId"] > 0) ? this.getUrlVars()["degreeId"] : 303;
        this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //this.setHost();
        this.getCourseMapper();
    }
}
$(function () {
    courseMapper.init();
})