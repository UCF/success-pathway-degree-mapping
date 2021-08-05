var degreeList = {
    data: {},
    ucfDegrees: [],
    partnerDegrees: [],
    noPartnerColleges: [],
    alphaCharList: [],
    target: {
        DegreeListOutput: 'DegreeListOutput',
        DegreeAlphaList: 'degree-alpha-list'
    },
    addToAlphaCharList: function (letter) {
        if (!this.alphaCharList.includes(letter)) {
            this.alphaCharList.push(letter);
        }
    },
    displayAlphaList: function () {
        if (this.alphaCharList.length > 0) {
            let list = '';
            for (let x = 0; x <= this.alphaCharList.length - 1; x++) {
                list += '<li style="display: table-cell;"><a class="pr-2" href="#' + this.alphaCharList[x] + '">' + this.alphaCharList[x] + '</a></li>';
            }
            $('#' + this.target.DegreeAlphaList).html('<ul class="h4 list-inline mx-auto justify-content-center">' + list + '</ul>');
        }
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
        ucfList.sort(this.getSortOrder("Degree"));
        pList.sort(this.getSortOrder("Institution"));
        let l1 = '';
        let l2 = '';
        let addtoCharList = true;
        if (degreeList.alphaCharList.length > 0) {
            addtoCharList = false;
        }
        for (x = 0; x <= ucfList.length - 1; x++) {
            if (this.noPartnerColleges.includes(ucfList[x].DegreeId)) {
                continue;
            }
            let char = ucfList[x].Degree.substring(0, 1);
            if (addtoCharList) {
                degreeList.addToAlphaCharList(char);
            }
            l1 += '<li class="degreeTitle showPlusSign" onclick="degreeList.displayPartners(this,' + ucfList[x].DegreeId + ')"><a name="' + char + '"></a> ' + ucfList[x].Degree;
            l2 = '';
            for (y = 0; y <= pList.length - 1; y++) {
                if (ucfList[x].DegreeId == pList[y].UCFDegreeId) {
                    let link = this.setHREF(pList[y].DegreeId, ucfList[x].Degree, pList[y].Institution)
                    l2 += '<li style="list-style:disc;">' + link + '</li>'
                }
            }
            l1 += '<ul class="pb-3 ' + ucfList[x].DegreeId + '"' + 'style="display:none"' + '>' + l2 + '</ul></li>';
        }
        if (addtoCharList) {
            this.displayAlphaList();
        }
        $('#' + this.target.DegreeListOutput).html('<ul class="lead">' + l1 + '</ul>')
    },
    clear: function () {
        $('#keyword').val('');
        this.displayByDegree(this.ucfDegrees, this.partnerDegrees);
    },
    displayPartners(obj,id) {
        $('.' + id).toggle();
        if ($(obj).hasClass("showPlusSign")) {
            $(obj).removeClass("showPlusSign");
            $(obj).addClass("showMinusSign");
        } else {
            $(obj).addClass("showPlusSign");
            $(obj).removeClass("showMinusSign");
        }
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
            keyword = (keyword == 'babs') ? 'b.a.b.s.' : keyword;
            keyword = (keyword == 'bsba') ? 'b.s.b.a.' : keyword;
            keyword = (keyword == 'ba') ? 'b.a.' : keyword;
            keyword = (keyword == 'bs') ? 'b.s.' : keyword;
            keyword = (keyword == 'ae') ? 'a.e.' : keyword;
            keyword = (keyword == 'ce') ? 'c.e.' : keyword;
            keyword = (keyword == 'bsw') ? 'b.s.w.' : keyword;
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
    getSortOrder: function (prop) {
        return function (a, b) {
            if (a[prop] > b[prop]) {
                return 1;
            } else if (a[prop] < b[prop]) {
                return -1;
            }
            return 0;
        }
    },
    setHREF(id, degree, institution) {
        return '<a href="/degree-mapping?degreeId=' + id + '" title="' + degree + ' for ' + institution + '">' + institution + '</a>';
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