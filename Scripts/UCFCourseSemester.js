var ucfSemesterTerm = {
    version: "1.0.0",
    data: {},
    semesterTerms: [],
    target: {
        UCFCourseSection: 'UCFCourseSection'
    },
    setSemesterTerms: function (data) {
        if (data.length > 0) {
            let term = '';
            //ucfSemesterTerm.data[0].SemesterTerm;
            for (let x = 0; x <= data.length - 1; x++) {
                if (term == '' || term != data[x].SemesterTerm) {
                    ucfSemesterTerm.semesterTerms.push(data[x].SemesterTerm);
                    term = data[x].SemesterTerm;
                }
            }
        }
    },
    setCard: function (term, total, cardblock) {
        let card = '<div class="card">';
        card += '<div class="card-header card-inverse"><h4> Semester ' + term + '</h4>';
        card += '<p class="card-text">Total ' + total + ' Units</p>';
        card += '</div>';
        card += '<div class="card-block">' + cardblock + '</div>';
        card += '</div>';
        return card;
    },
    displayUCFSemesterCourse: function (data) {
        let term = ucfSemesterTerm.semesterTerms;
        let carddeck = '';
        for (let x = 0; x <= term.length - 1; x++) {
            let total = 0;
            let cardblock = '';
            for (y = 0; y <= data.length - 1; y++) {
                if (data[y].SemesterTerm == term[x]) {
                    total = total + data[y].Credit;
                    cardblock += '<p class="card-text"><strong>' + data[y].Course + '</strong><br/>' + data[y].Credit + '</p>'
                }
            }
            carddeck += ucfSemesterTerm.setCard(term[x], total, cardblock);
            $('#' + ucfSemesterTerm.target.UCFCourseSection).html('<div class="card-deck">' + carddeck + '</div>');
        }
        
    },
    getUCFSemesterCourse: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/GetCourseMapper?degreeId="+courseMapper.degreeId,
            url: "/api/degree/GetUCFSemesterCourse?degreeId=" + ucfSemesterTerm.degreeId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                //console.log(data);
                ucfSemesterTerm.data = data;
                ucfSemesterTerm.setSemesterTerms(data);
                ucfSemesterTerm.displayUCFSemesterCourse(data);
            }
        })
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
        ucfSemesterTerm.degreeId = main.degreeId;
        ucfSemesterTerm.institutionId = main.institutionId;
        //this.degreeId = (this.getUrlVars()["degreeId"] > 0) ? this.getUrlVars()["degreeId"] : 0;
        //this.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        //this.setHost();
        this.getUCFSemesterCourse();
    }
}
$(function () {
    ucfSemesterTerm.init();
})