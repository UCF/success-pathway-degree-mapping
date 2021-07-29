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
    setDivHeight: function (text) {
        if (text.length <= 15) {
            return "20px";
        } else if (text.length <= 50) {
            return "60px";
        } else {
            return "70px";
        }
    },
    displayCourses: function (displayName, ucfCourses, partnerCourses) {
        let row = '';
        let space = '';

        if (ucfCourses.length > 0) {
            if (displayName.length > 0) {
                space = '';
            }
            //displayName = (displayName.length > 0) ? displayName : "&nbsp;";
            space = (displayName.length > 0) ? '<div>&nbsp;</div>' : '';
            for (let x = 0; x <= ucfCourses.length - 1; x++) {
                ucfCourse += (displayName.length > 0) ? '<div><strong>' + displayName + '</strong></div>' : '';
                ucfCourse += '<div>' + ucfCourses[x].Course + '</div>';
                if (displayName.length > 0) {
                    ucfCredit += '<div>&nbsp;</div><div>' + ucfCourses[x].Credit + '</div>';
                    cpp += '<div>&nbsp;</div><div>' + main.getYesNo(ucfCourses[x].CPP) + '</div>';
                    critical += '<div>&nbsp;</div><div>' + main.getYesNo(ucfCourses[x].Critical) + '</div>';
                    required += '<div>&nbsp;</div><div>' + main.getYesNo(ucfCourses[x].Required) + '</div>';
                } else {
                    ucfCredit += '<div>' + ucfCourses[x].Credit + '</div>';
                    cpp += '<div>' + main.getYesNo(ucfCourses[x].CPP) + '</div>' + space;
                    critical += '<div>' + main.getYesNo(ucfCourses[x].Critical) + '</div>' + space;
                    required += '<div>' + main.getYesNo(ucfCourses[x].Required) + '</div>' + space;
                }
            }
        } else {
            ucfCourse += '<div>&nbsp;1</div>';
            ucfCredit += '<div>&nbsp;2</div>';
            cpp += '<div>&nbsp;3</div>';
            critical += '<div>&nbsp;4</div>';
            required += '<div>&nbsp;5</div>';
        }
        if (partnerCourses.length > 0) {
            //displayName = (displayName.length > 0) ? displayName : "&nbsp;";
            space = (displayName.length > 0) ? '<div>&nbsp;</div>' : '';
            for (let x = 0; x <= partnerCourses.length - 1; x++) {
                pCourse += (displayName.length > 0) ? '<div><strong>' + displayName + '</strong></div>' : '';
                if (displayName.length > 0) {
                    pCredit += '<div>&nbsp;</div><div>' + partnerCourses[x].Credit + '</div>';
                } else {
                    pCredit += '<div>' + partnerCourses[x].Credit + '</div>';
                }
                pCourse += '<div>' + partnerCourses[x].Course + '</div>';
                space = '';
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
                courseMapper.data = data;
                let tr = '';
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
                //$('#coursesTable div').css('height', '30px');
                //$('#coursesTable div').addClass('py-2');
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
        this.getCourseMapper();
    }
}
$(function () {
    courseMapper.init();
})