//https://tulsa.okstate.edu/transfer
var ucfCourse = '';
var ucfCredit = '';
var cpp = '';
var critical = '';
var required = '';
var pCourse = '';
var pCredit = '';
var heading = '';
var courseMapper = {
    version: "1.0.0",
    data: {},
    target: {
        coursesTable: "coursesTable"
    },
    displayCourses: function (displayName, ucfCourses, partnerCourses) {
        let row = '';
        let space = '';

        if (ucfCourses.length > 0) {
            if (displayName.length > 0) {
                space = '';
            }
            //displayName = (displayName.length > 0) ? displayName : "&nbsp;";
            ucfCourse += (displayName.length > 0) ? '<div><strong>' + displayName + '</strong></div>' : '';
            for (let x = 0; x <= ucfCourses.length - 1; x++) {
                ucfCourse += '<div>' + ucfCourses[x].Course + '</div>';
                ucfCredit += space +  '<div>' + ucfCourses[x].Credit + '</div>';
                cpp += space + '<div>' + main.getYesNo(ucfCourses[x].CPP) + '</div>';
                critical += space + '<div>' + main.getYesNo(ucfCourses[x].Critical) + '</div>';
                required += space + '<div>' + main.getYesNo(ucfCourses[x].Required) + '</div>';
            }
        } else {
            ucfCourse += '<div>&nbsp;</div>';
            ucfCredit += '<div>&nbsp;</div>';
            cpp += '<div>&nbsp;</div>';
            critical += '<div>&nbsp;</div>';
            required += '<div>&nbsp;</div>';
        }

        if (partnerCourses.length > 0) {
            //displayName = (displayName.length > 0) ? displayName : "&nbsp;";
            pCourse += (displayName.length > 0) ? '<div><strong>' + displayName + '</strong></div>' : '';
            for (let x = 0; x <= partnerCourses.length - 1; x++) {
                pCourse += '<div>' + partnerCourses[x].Course + '</div>';
                pCredit += space + '<div>' + partnerCourses[x].Credit + '</div>';
            }
        } else {
            pCourse += '<div>&nbsp;</div>';
            pCredit += '<div>&nbsp;</div>';
        }
    },
    getTD: function (item) {
        return '<td>' + item + '</td>';
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
                //console.log(data);
                courseMapper.data = data;
                let tr = '';
                console.log('start');
                if (data != null && data.length > 0) {
                    for (let x = 0; x <= data.length - 1; x++) {
                        ucfCourse = '';
                        ucfCredit = '';
                        cpp = '';
                        critical = '';
                        required = '';
                        pCourse = '';
                        pCredit = '';
                        //tr = '';
                        
                        if (data[x].UCFCourses.length > 0 || data[x].PartnerCourses.length > 0) {
                            courseMapper.displayCourses(data[x].DisplayName, data[x].UCFCourses, data[x].PartnerCourses);
                            console.log('ucf Course' + ucfCourse);
                        }
                        if (data[x].AlternateUCFCourse.length > 0 || data[x].AlternatePartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].AlternateDisplayName, data[x].AlternateUCFCourse, data[x].AlternatePartnerCourse);
                        }
                        if (data[x].Alternate2UCFCourse.length > 0 || data[x].Alternate2PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate2DisplayName, data[x].Alternate2UCFCourse, data[x].Alternate2PartnerCourse);
                        }
                        if (data[x].Alternate3UCFCourse.length > 0 || data[x].Alternate3PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate3DisplayName, data[x].Alternate3UCFCourse, data[x].Alternate3PartnerCourse);
                        }
                        if (data[x].Alternate4UCFCourse.length > 0 || data[x].Alternate4PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate4DisplayName, data[x].Alternate4UCFCourse, data[x].Alternate4PartnerCourse);
                        }
                        tr += '<tr>' + courseMapper.getTD(ucfCourse) + courseMapper.getTD(ucfCredit) + courseMapper.getTD(pCourse) + courseMapper.getTD(pCredit) + courseMapper.getTD(critical) + courseMapper.getTD(cpp) + courseMapper.getTD(required) + '</tr>'
                    }
                    $("#" + courseMapper.target.coursesTable).append(tr);
                }
                $('#coursesTable div').css('height', '30px');
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
        courseMapper.degreeId = main.degreeId;
        courseMapper.institutionId = main.institutionId;
        //console.log('Course Mapper: ' + courseMapper.degreeId);
        //this.degreeId = (this.getUrlVars()["degreeId"] > 0) ? this.getUrlVars()["degreeId"] : 0;
        //this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //this.setHost();
        this.getCourseMapper();
    }
}
$(function () {
    courseMapper.init();
})