var degreemap = {
    data: {},
    target : {
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
    displayDefaultInfo: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                let gpa = data[x].GPA;
                let limitedAccess = this.getTrueFalse(data[x].LimitedAccess);
                let restrictedAccess = this.getTrueFalse(data[x].RestrictedAccess);
                let degreeTtitle = data[x].Degree + " " + data[x].DegreeType;
                console.log(degreeTtitle);
                $("#" + this.target.UCFGPA).html(gpa);
                $("#" + this.target.UCFLimitedAccess).html(limitedAccess);
                $("#" + this.target.UCFRestrictedAccess).html(restrictedAccess);
                $("#" + this.target.DegreeTitle).html(degreeTtitle)
                return;
            }
        }
    },
    displayAdditionalRequirements: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Degrees.length > 0) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        if (data[x].Degrees[y].Id == this.degreeId) {
                            if (data[x].Degrees[y].Notes.length > 0) {
                                for (z = 0; z <= data[x].Degrees[y].Notes.length - 1; z++) {
                                    if (data[x].Degrees[y].Notes[z].NoteType == degreemap.noteType.additionalRequirement) {
                                        output = data[x].Degrees[y].Notes[z].Content;
                                        $("#" + this.target.AdditionalRequirements).html(output);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    },
    displayForeignLanugage: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Degrees.length > 0) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        if (data[x].Degrees[y].Id == this.degreeId) {
                            if (data[x].Degrees[y].Notes.length > 0) {
                                for (z = 0; z <= data[x].Degrees[y].Notes.length - 1; z++) {
                                    if (data[x].Degrees[y].Notes[z].NoteType == degreemap.noteType.foreginLaguage) {
                                        output = data[x].Degrees[y].Notes[z].Content;
                                        $("#" + this.target.ForeignLanugage).html(output);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    },
    displayListItems: function (data) {
        let output = "";
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Degrees.length > 0) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        if (data[x].Degrees[y].Id == this.degreeId) { 
                            if (data[x].Degrees[y].Notes.length > 0) {
                                for (z = 0; z <= data[x].Degrees[y].Notes.length - 1; z++) {
                                    if (data[x].Degrees[y].Notes[z].NoteType == degreemap.noteType.listItem) {
                                        output += "<li><strong>" + data[x].Degrees[y].Notes[z].Content + "</strong></li>";
                                    }
                                }
                                $("#" + this.target.ListItems).append(output);
                            }
                        }
                    }
                }
            }
        }
    },
    displayInstitutionName: function (data) {
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Degrees.length > 0) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        if (data[x].Degrees[y].InstitutionId == this.institutionId) {
                            $("#" + this.target.Institution).html(data[x].Degrees[y].Institution + " Pathway");
                            return;
                        }
                    }
                }
            }
        }
    },
    displayInstitutionDropdownlist: function (data) {
        let output = '';
        let generic = '';
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Degrees.length > 0) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        let degreeid = data[x].Degrees[y].Id;
                        let qry = '?degreeid=' + degreeid + '&institutionid=' + data[x].Degrees[y].InstitutionId + '&ucfDegreeId=' + this.ucfDegreeId;
                        let title = data[x].Degrees[y].Institution;
                        if (data[x].Degrees[y].Institution == "Generic") {
                            generic = '<a class="dropdown-item" href="' + qry + '">' + title + '</a><br>';;
                        } else {
                            output += '<a class="dropdown-item" href="' + qry + '">' + title + '</a><br>';
                        }
                    }
                    $("#" + this.target.InstitutionList).html(generic + output);
                    return;
                }
            }
        }
    },
    displayCourseTable: function (data) {
        let courseTable = '';
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Degrees.length > 0) {
                    for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                        if (data[x].Degrees[y].Id == this.degreeId) {
                            if (data[x].Degrees[y].Courses.length > 0) {
                                for (z = 0; z <= data[x].Degrees[y].Courses.length - 1; z++) {
                                    if (data[x].Degrees[y].Courses[z].UCFCourseName == null) {
                                        continue;
                                    }
                                    let courseRow = "";
                                    let ucfCredits = data[x].Degrees[y].Courses[z].UCFCourseCredits;
                                    let ucfCourse = data[x].Degrees[y].Courses[z].UCFCourseName;
                                    let partnerCourse = data[x].Degrees[y].Courses[z].CourseCode + " " + data[x].Degrees[y].Courses[z].CourseName;
                                    let partnerCredits = data[x].Degrees[y].Courses[z].Credits;
                                    let critical = data[x].Degrees[y].Courses[z].Critical;
                                    let cpp = data[x].Degrees[y].Courses[z].CommonProgramPrerequiste;
                                    let required = data[x].Degrees[y].Courses[z].Required;
                                    courseRow = this.courseDisplayTemplate(ucfCourse, ucfCredits, partnerCourse, partnerCredits, critical, cpp, required);
                                    courseTable += courseRow;
                                }
                                $("#" + this.target.DegreemapRow).html(courseTable);
                                //return;
                            }
                        }
                    }

                }
            }
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
    displayUCFSemesterCourse(data, semester) {
        let output = '';
        let totalcredits = 0;
        for (x = 0; x <= data.length - 1; x++) {
            if (this.ucfDegreeId == data[x].Id) {
                if (data[x].Courses.length > 0) {
                    for (y = 0; y <= data[x].Courses.length - 1; y++) {
                        if (data[x].Courses[y].Semester == semester) {
                            let coursecode = data[x].Courses[y].CourseCode;
                            let coursename = data[x].Courses[y].CourseName;
                            let credits = data[x].Courses[y].Credits;
                            totalcredits = 1 * totalcredits + 1 * credits;
                            output += degreemap.semesterCourseTemplate(coursecode, coursename, credits);
                        }
                    }
                    $("#UCFSemester_" + semester + "_Total").html("Total " + totalcredits + " Units");
                    $("#UCFSemester_" + semester).html(output);
                    return;
                }
            }
        }
    },
    semesterCourseTemplate(coursecode,coursename,credits) {
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
            //url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetDegreeList",
            url: "/api/degree/GetDegreeList",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                degreemap.data = data;
                if (degreemap.ucfDegreeId == 0) {
                    degreemap.getUCFID(data);
                }
                degreemap.displayDefaultInfo(data);
                degreemap.displayInstitutionName(data);
                degreemap.displayAdditionalRequirements(data);
                degreemap.displayForeignLanugage(data);
                degreemap.displayListItems(data);
                degreemap.displayInstitutionDropdownlist(data);
                degreemap.displayCourseTable(data);
                degreemap.displayUCFSemesterCourse(data, 5);
                degreemap.displayUCFSemesterCourse(data, 6);
                degreemap.displayUCFSemesterCourse(data, 7);
                degreemap.displayUCFSemesterCourse(data, 8);
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
    getUCFID: function (data) {
        for (x = 0; x <= data.length - 1; x++) {
            id = data[x].Id;
            if (data[x].Degrees.length > 0) {
                for (y = 0; y <= data[x].Degrees.length - 1; y++) {
                    if (data[x].Degrees[y].Id == this.degreeId) {
                        this.ucfDegreeId = id;
                        return;
                    }
                }
            }
        }
    },
    init: function () {
        this.degreeId = (this.getUrlVars()["degreeid"] > 0) ? this.getUrlVars()["degreeid"] : 0;
        this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        this.setHost();
        this.getDegreeList();
    }
}
$(function () {
    degreemap.init();
})