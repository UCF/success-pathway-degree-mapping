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
            let pcCourse = '';
            let pcCredit = '';

            //let hasUCFAlternateCourse = false;
            let ucfaltCourse = '';
            let ucfaltCredit = '';
            let ucfaltCritical = '';
            let ucfaltCPP = '';
            let ucfaltRequired = '';

            //let hasPartnerAlternateCourse = false;
            let apCourse = '';
            let apCredit = '';

            for (let x = 0; x <= data.length - 1; x++) {
                if (data[0].UCFCourses.length > 0) {
                    for (let c = 0; c <= data[x].UCFCourses.length - 1; c++) {
                        course += '<div>' + data[x].UCFCourses[c].Course + '</div>';
                        credit += '<div>' + data[x].UCFCourses[c].Credit + '</div>';
                        critical += '<div>' + courseMapper.getYesNo(data[x].UCFCourses[c].Critical) + '</div>';
                        cpp += '<div>' + courseMapper.getYesNo(data[x].UCFCourses[c].CPP) + '</div>';
                        required += '<div>' + courseMapper.getYesNo(data[x].UCFCourses[c].Required) + '</div>';
                        //let selectOne = data[x].SelectOne;
                    }
                    for (let pc = 0; pc <= data[x].PartnerCourses.length - 1; pc++) {
                        pcCourse += '<div>' + data[x].PartnerCourses[pc].Course + '</div>';
                        pcCredit += '<div>' + data[x].PartnerCourses[pc].Credit + '</div>';
                    }
                    //courseMapper.data[5].AlternateUCFCourse[0]
                    //console.log(x +'| data[x].AlternateUCFCourse.length: ' + data[x].AlternateUCFCourse.length);
                    if (data[x].AlternateUCFCourse.length > 0) {
                        //hasUCFAlternateCourse = true;
                        //courseMapper.data[5].AlternateUCFCourse[0].Course
                        for (let ucfalt = 0; ucfalt <= data[x].AlternateUCFCourse.length - 1; ucfalt++) {
                            ucfaltCourse += '<div>' + data[x].AlternateUCFCourse[ucfalt].Course + '</div>';
                            ucfaltCredit += '<div>' + data[x].AlternateUCFCourse[ucfalt].Credit + '</div>';
                            ucfaltCritical += '<div>' + courseMapper.getYesNo(data[x].AlternateUCFCourse[ucfalt].Critical) + '</div>';
                            ucfaltCPP += '<div>' + courseMapper.getYesNo(data[x].AlternateUCFCourse[ucfalt].CPP) + '</div>';
                            ucfaltRequired += '<div>' + courseMapper.getYesNo(data[x].AlternateUCFCourse[ucfalt].Required) + '</div>';
                            //console.log(ucfalt + ' ucfaltCourse: ' + ucfaltCourse);
                        }
                        course += '<div><strong>Alternate Course</strong></div>';
                        course += ucfaltCourse;
                        credit += '<div>&nbsp;</div>' + ucfaltCredit;
                        critical += '<div>&nbsp;</div>' + ucfaltCritical;
                        cpp += '<div>&nbsp;</div>' + ucfaltCPP;
                        required += '<div>&nbsp</div>' + ucfaltRequired;
                    }
                    if (data[x].AlternatePartnerCourse.length > 0) {
                        //hasPartnerAlternateCourse = true;
                        for (let apc = 0; apc <= data[x].AlternatePartnerCourse.length - 1; apc++) {
                            apCourse += '<div>' + data[x].AlternatePartnerCourse[apc].Course + '</div>';
                            apCredit += '<div>' + data[x].AlternatePartnerCourse[apc].Credit + '</div>';
                        }
                        pcCourse += '<div><strong>Alternate Course</strong></div>';
                        pcCourse += apCourse;
                        pcCredit += '<div>&nbsp;</div>';
                        pcCredit += apCredit;
                    }
                    if (data[x].DisplayValue == 1) {
                        tr += '<tr><td colspan=7" class="text-center">Select One</td></tr>';
                    }
                    tr += '<tr><td>' + course + '</td><td>' + credit + '</td><td>' + critical + '</td><td>' + cpp + '</td><td>' + required + '</td><td>' + pcCourse + '</td><td>' + pcCredit + '</td></tr>';
                    course = '';
                    credit = '';
                    critical = '';
                    cpp = '';
                    required = '';
                    pcCourse = '';
                    pcCredit = '';

                    //hasUCFAlternateCourse = false;
                    ucfaltCourse = '';
                    ucfaltCredit = '';
                    ucfaltCritical = '';
                    ucfaltCPP = '';
                    ucfaltRequired = '';

                    //hasPartnerAlternateCourse = false;
                    apCourse = '';
                    apCredit = '';
                }
            }
            $('#coursesTable').html(tr);
        }
    },
    setDiv: function (val) {
        return '<div>' + val + '</div>';
    },
    setTD : function (val) {
        return '<td>' + val +'</td>';
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
        courseMapper.degreeId = main.degreeId;
        courseMapper.institutionId = main.institutionId;
        console.log('Course Mapper: ' + courseMapper.degreeId);
        //this.degreeId = (this.getUrlVars()["degreeId"] > 0) ? this.getUrlVars()["degreeId"] : 0;
        //this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //this.setHost();
        this.getCourseMapper();
    }
}
$(function () {
    courseMapper.init();
})