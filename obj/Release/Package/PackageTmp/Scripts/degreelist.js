var degreeList = {
    data: {},
    ucfDegrees: [],
    partnerDegrees: [],
    noPartnerColleges : [],
    target: {
        DegreeListOutput: 'DegreeListOutput'
    },
    filter: function (ucfList, pList) {
        for (x = 0; x <= ucfList.length - 1; x++) {
            let hasPartnerCollege = false;
            for (y = 0; y <= pList.length - 1; y++) {
                if (ucfList[x].DegreeId == pList[y].UCFDegreeId) {
                    hasPartnerCollege = true;
                }
            }
            if (!hasPartnerCollege) {
                this.noPartnerColleges.push(ucfList[x].DegreeId);
            }
        }
    },
    displayByDegree: function (ucfList, pList) {
        let l1 = '';
        let l2 = '';
        for (x = 0; x <= ucfList.length - 1; x++) {
            if (this.noPartnerColleges.includes(ucfList[x].DegreeId)) {
                continue;
            }
            l1 += '<li><h2>' + ucfList[x].Degree + '</h2>';
            l2 = '';
            for (y = 0; y <= pList.length - 1; y++) {
                if (ucfList[x].DegreeId == pList[y].UCFDegreeId) {
                    let link = this.setHREF(pList[y].InstitutionId, pList[y].Institution)
                    l2 += '<li>' + link + '</li>'
                }
            }
            l1 += '<ul class="pb-3">' + l2 + '</ul></li>';
        }
        $('#' + this.target.DegreeListOutput).html('<ul>' + l1 + '</ul>')
    },
    clear: function () {
        $('#keyword').val('');
        this.displayByDegree(this.ucfDegrees, this.partnerDegrees);
    },
    search: function (keyword) {
        //let keyword = $('#keyword').val();
        if (keyword.length < 1) {
            this.displayByDegree(this.ucfDegrees, this.partnerDegrees);
            return;
        }
        let obj = [];
        keyword = keyword.toLowerCase()
        if (keyword.length >= 2) {
            keyword = (keyword == 'ba') ? 'b.a.' : keyword;
            keyword = (keyword == 'bs') ? 'b.s.' : keyword;
            keyword = (keyword == 'ae') ? 'a.e.' : keyword;
            keyword = (keyword == 'ce') ? 'c.e.' : keyword;
            keyword = (keyword == 'bsw') ? 'b.s.w.' : keyword;
            console.log(keyword);
            for (x = 0; x <= this.ucfDegrees.length - 1; x++) {
                let ucfdegree = this.ucfDegrees[x].Degree.toLowerCase();
                if (ucfdegree.search(keyword) >= 0) {
                    obj.push(this.ucfDegrees[x]);
                }
            }
            degreeList.displayByDegree(obj, this.partnerDegrees);
        }
        if (obj.length == 0) {
            $('#' + this.target.DegreeListOutput).html('No degrees found')
        }
    },
    setHREF(id, degree) {
        return '<a href="/degree-mapping?degreeId=' + id + '&degree=' + degree + '" title="' + degree + '">' + degree + '</a>';
    },
    getDegreeList: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/Degree/GetListByCatalog",
            url: "/api/degree/GetListByCatalog?catalogId=1",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                degreeList.data = data;
                if (degreeList.data.length > 0) {
                    for (x = 0; x <= degreeList.data.length - 1; x++) {
                        if (degreeList.data[x].InstitutionId == 1) {
                            degreeList.ucfDegrees.push(degreeList.data[x]);
                        } else {
                            degreeList.partnerDegrees.push(degreeList.data[x]);
                        }
                    }
                    degreeList.filter(degreeList.ucfDegrees, degreeList.partnerDegrees);
                    degreeList.displayByDegree(degreeList.ucfDegrees, degreeList.partnerDegrees);
                }
                
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
        //this.setHost();
        this.getDegreeList();
    }
}
$(document).ready(function () {
    degreeList.init();
    $("#keyword").on('keyup', function (obj) {
        if ($(this).val().length >= 2) {
            degreeList.search($(this).val());
        }
        if ($(this).val().length == 0) {
            degreeList.clear();
        }
    })
})