var genericList = {
    target: {
        DegreeListOutput: "DegreeListOutput"
    },
    displayGenericList(data) {
        let output = "";
        for (x = 0; x<= data.length-1; x++) {
            output += "<div class=\"py-2\">";
            output += "<a href=\"/degree-mapping?degreeid=" + data[x].Id + "\" title=\"" + data[x].Name + "\">" + data[x].Name + "</a>";
            output += "</div>";
        }
        $("#" + this.target.DegreeListOutput).html(output);
    },
    getGenericList: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetGenericList",
            url: "/api/degree/GetGenericList",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                genericList.displayGenericList(data);
            }
        })
    },
    init: function () {
        genericList.getGenericList();
    }
}
$(document).ready(function () {
    genericList.init();
})