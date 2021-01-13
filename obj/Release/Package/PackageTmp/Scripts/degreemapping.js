
    //$.get("/api/Courses/GetCourses?keyData=1", function (data, status) {
    //    alert("Status: " + status);
    //});
var degreeMapping =
{
    institutionId: 2,
    degreeId: 3,
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

    init: function (val1, val2) {
        this.institutionId = val1;
        this.degreeId = val2;
        this.getDegree(this.institutionId, this.degreeId);
        //this.setUCFValues();
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

    displayDegreeNotes(data) {
        let div = "";
        //console.log(data.DegreeNotes.length);
        if (data.DegreeNotes.length > 0) {
            for (x = 0; x <= data.DegreeNotes.length - 1; x++) {
                if (data.DegreeNotes[x].ShowName) {
                    div += "<strong>" + data.DegreeNotes[x].Name + "</strong>";
                    div += "<div>" + data.DegreeNotes[x].Value + "</div><hr/>";
                } else {
                    div += "<div>" + data.DegreeNotes[x].Value + "</div><hr/>";
                }
                
            }
        }
        $("#NOTES").html(div);
    },

    displayUCFSemster(data, semester) {
        let output = "";
        let total = 0;
        if (data.UCFCourses.length > 0) {
            for (x = 0; x <= data.UCFCourses.length - 1; x++) {
                if (data.UCFCourses[x].UCFSemester == semester) {
                    output += "<p class=\"card - text\"><strong>" + data.UCFCourses[x].UCFCourseCode + "</strong><br>" + data.UCFCourses[x].UCFCourseName + "<br>" + data.UCFCourses[x].UCFCredits + " Units</p>";
                    total = total + 1*data.UCFCourses[x].UCFCredits;
                }
            }
        }
        $("#UCFSemester_" + semester).html(output);
        $("#UCFSemester_" + semester + "_Total").html("Total " + total + " Units");
    },




    getTrueFalse: function(val) {
        return (val) ? "Yes" : "No";
    },

    getDegree: function () {
        $.get({
        url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetDegree?institutionId=" + degreeMapping.institutionId + "&DegreeId=" + degreeMapping.degreeId,
            type: "GET",
            headers: {"APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                degreeMapping.json = data;
                //console.log(degreeMapping.json);
                degreeMapping.setUCFValues(data);
                degreeMapping.setDegreemapRow(data);
                degreeMapping.displayDegreeNotes(data);
                degreeMapping.displayUCFSemster(data, 5);
                degreeMapping.displayUCFSemster(data, 6);
                degreeMapping.displayUCFSemster(data, 7);
                degreeMapping.displayUCFSemster(data, 8);
            }
        })
    }
}
$(document).ready(function () {
    degreeMapping.init(2, 3);
})