//https://portal.connect.ucf.edu/pathway
var degreeMapping =
{
    hostname:"",
    institutionId: 0,
    degreeId: 0,
    json: "",
    UCFDegreeMap: {

    },
    UCFCourseMap: {
        GPA: "",
        LimitedAccess:"",
        RestrictedAccess: "",
        Notes: "",
        Coures : ""
    },
    init: function () {
        this.degreeId = (this.getUrlVars()["degreeid"] > 0) ? this.getUrlVars()["degreeid"] : 0;
        this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //console.log("degreeid = " + this.degreeId + "institutionid=" + this.institutionId);
        if (this.degreeId < 1 || this.institutionId < 1) {
            window.location.href = "?degreeid=3&institutionId=2";
        }
        this.getDegree(this.institutionId, this.degreeId);
    },
    setUCFValues: function (data) {
        $("#UCFGPA").html(data.UCFGPA);
        $("#UCFLimitedAccess").html(this.getTrueFalse(data.UCFLimitedAccess));
        $("#UCFRestrictedAccess").html(this.getTrueFalse(data.UCFRestrictedAccess))
    },
    setDegreemapRow: function (data) {
        let row = "";
        if (data.CourseMap.length-1 > 0) {
            for (x = 0; x <= data.CourseMap.length-1; x++) {
                row += "<tr>";
                row += "<td>" + data.CourseMap[x].UCFCourseCode + " " + data.CourseMap[x].UCFCourseName + "</td>";
                row += "<td><center>" + data.CourseMap[x].UCFCredits + "</center></td>";
                row += "<td>" + data.CourseMap[x].CourseCode + " " + data.CourseMap[x].CourseName + "</td>";
                row += "<td><center>" + data.CourseMap[x].Credits + "</center></td>";
                row += "<td><center>" + this.getTrueFalse(data.CourseMap[x].UCFCritical) + "</center></td>";
                row += "<td><center>" + this.getTrueFalse(data.CourseMap[x].CommonProgramPrerequiste) + "</center></td>";
                row += "<td><center>" + this.getTrueFalse(data.CourseMap[x].Required) + "</center></td>";
                row += "</tr>";
            }
            $("#DegreemapRow").html(row);
        }
    },
    displayDegreeTitle: function (data) {
        $("#DegreeTitle").html(data.Degree);
    },
    displayDegreeNotes(data) {
        let div = "";
        let section2 = "";
        if (data.DegreeNotes.length > 0) {
            for (x = 0; x <= data.DegreeNotes.length - 1; x++) {
                if (!data.DegreeNotes[x].ForeignLanguageRequirement) {
                    if (data.DegreeNotes[x].ShowName) {
                        div += "<strong>" + data.DegreeNotes[x].Name + "</strong>";
                        div += "<div>" + data.DegreeNotes[x].Value + "</div><hr/>";
                    } else if (data.DegreeNotes[x].Section1) {
                        div += "<div>" + data.DegreeNotes[x].Value + "</div><hr/>";
                    } else {
                        section2 += "<li>" + data.DegreeNotes[x].Value + "</li>";
                    }
                } else {
                    let output = "<p class=\"card-text\"><strong>Foreign Language Requirement</strong><br>" + data.DegreeNotes[x].Value + "</p>"
                    $("#ForeignLanguageRequirement").html(output);
                }
            }
        }
        $("#Section1").html(div);
        $("#Section2").append(section2);
    },
    displayUCFSemster(data, semester) {
        let output = "";
        let total = 0;
        if (data.UCFCourses.length > 0) {
            for (x = 0; x <= data.UCFCourses.length - 1; x++) {
                if (data.UCFCourses[x].UCFSemester == semester) {
                    output += "<p class=\"card-text\"><strong>" + data.UCFCourses[x].UCFCourseCode + "</strong><br>" + data.UCFCourses[x].UCFCourseName + "<br>" + data.UCFCourses[x].UCFCredits + " Units</p>";
                    total = total + 1*data.UCFCourses[x].UCFCredits;
                }
            }
        }
        $("#UCFSemester_" + semester).html(output);
        $("#UCFSemester_" + semester + "_Total").html("Total " + total + " Units");
    },
    displayForeginLanguageRequirement() {
        let output = "";
        output = "<p class=\"card-text\"><strong>Foreign Language Requirement</strong><span id=\"ForeignLanguageRequirement\"></span></p>";
    },
    getTrueFalse: function(val) {
        return (val) ? "Yes" : "No";
    },
    getDegree: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetDegree?institutionId=" + degreeMapping.institutionId + "&DegreeId=" + degreeMapping.degreeId,
            url: "/api/Degree/GetDegree?institutionId=" + degreeMapping.institutionId + "&DegreeId=" + degreeMapping.degreeId,
            type: "GET",
            headers: {"APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                degreeMapping.json = data;
                degreeMapping.displayDegreeTitle(data);
                degreeMapping.setUCFValues(data);
                degreeMapping.setDegreemapRow(data);
                degreeMapping.displayDegreeNotes(data);
                degreeMapping.displayUCFSemster(data, 5);
                degreeMapping.displayUCFSemster(data, 6);
                degreeMapping.displayUCFSemster(data, 7);
                degreeMapping.displayUCFSemster(data, 8);
                degreeMapping.displayDegreeDropdown(data);
                degreeMapping.displatDegreeListOutput(data);
            }
        })
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
    displayDegreeDropdown(data) {
        let output = "";
        let generic = "";
        for (x = 0; x <= data.DegreeList.length - 1; x++) {
            if (data.DegreeList[x].Degrees.length > 0) {
                //console.log(this.degreeId)
                if (data.DegreeList[x].Degree == data.UCFDegree) {
                    //console.log(data.DegreeList[x].Id);
                    //console.log(data.DegreeList[x].Degree);
                    //console.log(data.DegreeList[x].Institution);
                    for (y = 0; y <= data.DegreeList[x].Degrees.length - 1; y++) {
                        //console.log(data.DegreeList[x].Degrees[y]);
                        if (data.DegreeList[x].Degrees[y].InstitutionId == this.institutionId) {
                            $("#Institution").html(data.DegreeList[x].Degrees[y].Institution + " Pathway");
                        }
                        if (data.DegreeList[x].Degrees[y].Institution.toLowerCase() == "generic") {
                            generic = "<a class=\"dropdown-item\" href=\"?degreeid=" + data.DegreeList[x].Degrees[y].Id + "&institutionId=" + data.DegreeList[x].Degrees[y].InstitutionId + "\">" + data.DegreeList[x].Degrees[y].Institution + "</a><br>";
                        } else {
                            output += "<a class=\"dropdown-item\" href=\"?degreeid=" + data.DegreeList[x].Degrees[y].Id + "&institutionId=" + data.DegreeList[x].Degrees[y].InstitutionId + "\">" + data.DegreeList[x].Degrees[y].Institution + "</a><br>";
                        }
                    }
                }
            }
        }
        $("#InstitutionList").html(generic + output);
    },
    displatDegreeListOutput(data) {
        let output = "";
        for (x = 0; x <= data.DegreeList.length - 1; x++) {
            if (data.DegreeList[x].Degrees.length > 0) {
                if (data.DegreeList[x].Id == this.degreeId) {
                    output += "<div>" + data.DegreeList[x].Degree + "</div>";
                    for (y = 0; y <= data.DegreeList[x].Degrees.length - 1; y++) {
                    }
                }
            }
        }
        $("#DegreeListOutput").html(output);
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
    }
}
$(document).ready(function () {
    degreeMapping.setHost();
    degreeMapping.init();
})