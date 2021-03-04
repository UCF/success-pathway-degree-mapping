var degreemap = {

    data: {},
    target: {
        UCFGPA: "UCFGPA",
        UCFLimitedAccess: "UCFLimitedAccess",
        UCFRestrictedAccess: "UCFRestrictedAccess",
        AdditionalRequirements: "AdditionalRequirements",
        ForeignLanugage: "ForeignLanugage",
        ListItems: "ListItems",
        Institution: "Institution",
        InstitutionList: "InstitutionList",
        DegreemapRow: "DegreemapRow",
        DegreeTitle: "DegreeTitle",
    },
    degreeId: 0,
    institutionId: 0,
    ucfDegreeId: 0,
    noteType: {
        additionalRequirement: 4,
        foreginLaguage: 3,
        listItem: 2,
        note: 1
    },
    displayGeneric: function (data) {
        let generic = ""
        let template = "";
        for (y = 0; y <= data.Generic.length - 1; y++) {
            if (data.Generic[y].Institution.toLowerCase() == "generic") {
                generic = "<a class=\"dropdown-item\" href=\"/degree-mapping?degreeid="+ data.Generic[y].Id + "\">" + data.Generic[y].Institution + "</a><br>";
            } else {
                template += "<a class=\"dropdown-item\" href=\"/degree-mapping?degreeid="+ data.Generic[y].Id + "\">" + data.Generic[y].Institution + "</a><br>";
            }
            
        } 
        $("#" + this.target.InstitutionList).html(generic + template)
    },
    displayDefaultInfo: function (data) {
        let output = "";
        let gpa = data.GPA;
        let limitedAccess = this.getTrueFalse(data.LimitedAccess);
        let restrictedAccess = this.getTrueFalse(data.RestrictedAccess);
        $("#" + this.target.UCFGPA).html(gpa);
        $("#" + this.target.UCFLimitedAccess).html(limitedAccess);
        $("#" + this.target.UCFRestrictedAccess).html(restrictedAccess);
        $("#" + this.target.DegreeTitle).html(data.Degree);
        $("#" + this.target.Institution).html(data.Institution + " Pathway")
    },
    displayNotes: function (data) {
        let output = "";
        if (data.Notes.length > 0) {
            for (y = 0; y <= data.Notes.length - 1; y++) {
                if (data.Notes[y].NoteType == this.noteType.additionalRequirement) {
                    output = data.Notes[y].Content;
                    $("#" + this.target.AdditionalRequirements).html(output);
                }
                if (data.Notes[y].NoteType == this.noteType.foreginLaguage) {
                    output = data.Notes[y].Content;
                    $("#" + this.target.ForeignLanugage).html(output);
                }
                if (data.Notes[y].NoteType == this.noteType.listItem) {
                    output = data.Notes[y].Content;
                    $("#" + this.target.ListItems).append(output);
                }
            }
        }
    },
    displayUCFSemesterCourse(data, semester) {
        let output = '';
        let totalcredits = 0;
        let x = 0;
        if (data.Courses.length > 0) {
            for (y = 0; y <= data.Courses.length - 1; y++) {
                if (data.Courses[y].Semester == semester) {
                    let coursecode = data.Courses[y].CourseCode;
                    let coursename = data.Courses[y].CourseName;
                    let credits = data.Courses[y].Credits;
                    totalcredits = 1 * totalcredits + 1 * credits;
                    output += degreemap.semesterCourseTemplate(coursecode, coursename, credits);
                }
            }
            $("#UCFSemester_" + semester + "_Total").html("Total " + totalcredits + " Units");
            $("#UCFSemester_" + semester).html(output);
            return;
        }
    },
    semesterCourseTemplate(coursecode, coursename, credits) {
        let template = '';
        template += '<span id="UCFSemester_5">';
        template += '<p class="card-text">';
        template += '<strong>' + coursecode + '</strong>';
        if (coursename.length > 0) {
            template += '<br>' + coursename;
        }
        template += '<br>' + credits + ' Units</p>';
        return template;
    },
    displayCourseTable: function (data) {
        let courseTable = '';
        if (data.Courses.length > 0) {
            for (y = 0; y <= data.Courses.length - 1; y++) {
                if (data.Courses[y].UCFCourseName == null) {
                    continue;
                }
                let courseRow = "";
                let ucfCredits = data.Courses[y].UCFCourseCredits;
                let ucfCourse = data.Courses[y].UCFCourseName;
                let partnerCourse = data.Courses[y].CourseCode + " " + data.Courses[y].CourseName;
                let partnerCredits = data.Courses[y].Credits;
                let critical = data.Courses[y].Critical;
                let cpp = data.Courses[y].CommonProgramPrerequiste;
                let required = data.Courses[y].Required;
                courseRow = this.courseDisplayTemplate(ucfCourse, ucfCredits, partnerCourse, partnerCredits, critical, cpp, required);
                courseTable += courseRow;
            }
            $("#" + this.target.DegreemapRow).html(courseTable);
        }
    },
    courseDisplayTemplate(ucfCourse, ucfCredits, partnerCourse, partnerCredits, critical, cpp, required) {
        let output = "<tr>";
        output += "<td>" + ucfCourse + "</td>";
        output += "<td>" + ucfCredits + "</td>";
        output += "<td>" + partnerCourse + "</td>";
        output += "<td>" + partnerCredits + "</td>";
        output += "<td>" + degreemap.getTrueFalse(critical) + "</td>";
        output += "<td>" + degreemap.getTrueFalse(cpp) + "</td>";
        output += "<td>" + degreemap.getTrueFalse(required) + "</td>";
        output += "</tr>";
        return output;
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
    getDegreeList: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/GetDegreeMap?degreeId="+degreemap.degreeId,
            url: "/api/degree/GetDegreeMap?degreeId="+degreemap.degreeId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                degreemap.data = data;
                degreemap.displayDefaultInfo(data);
                degreemap.displayNotes(data);
                degreemap.displayCourseTable(data);
                degreemap.displayUCFSemesterCourse(data, 5);
                degreemap.displayUCFSemesterCourse(data, 6);
                degreemap.displayUCFSemesterCourse(data, 7);
                degreemap.displayUCFSemesterCourse(data, 8);
                degreemap.displayGeneric(data);
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
        this.degreeId = (this.getUrlVars()["degreeid"] > 0) ? this.getUrlVars()["degreeid"] : 0;
        this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //this.setHost();
        console.log(this.degreeId);
        this.getDegreeList();
    }
}
$(function () {
    degreemap.init();
})