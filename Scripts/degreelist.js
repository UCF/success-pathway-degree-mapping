degreeList = {
    init: function () {
        this.getDegreeList();
    },

    setInstitutionDropdownList: function (data) {
        let output = "";
        if (data.length > 0) {
            for (x = 0; x <= data.length - 1; x++) {
                output = "<a class=\"dropdown-item\" href=\"#\">" + data[x].Institution + "</a><br>"
            }
        }
        $("#InstitutionList").html(output);
    },
    getDegreeList: function () {
        $.get({
            url: "/api/Degree/GetDegreeList",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                degreeList.setInstitutionDropdownList(data);
            }
        })
    }
}
$(document).ready(function () {
    degreeList.init();
})