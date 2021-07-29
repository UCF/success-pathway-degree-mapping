var main = {
    catalogId: 0,
    degreeId: 0,
    institutionId: 0,
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
    getYesNo: function (val) {
        return (val) ? "Yes" : "No";
    },
    init: function () {
        main.degreeId = (this.getUrlVars()["degreeid"] > 0) ? this.getUrlVars()["degreeid"] : 0;
        main.institutionId = (this.getUrlVars()["institutionid"] > 0) ? this.getUrlVars()["institutionid"] : 0;
        if (main.degreeId == 0) {
            window.location.replace("/home/DegreeList");
        }
    }
}
$(function () {
    main.init();
})
var ucfCourse = '';
var ucfCredit = '';
var ucfCourseSymbols;
var cpp = '';
var critical = '';
var required = '';
var pCourse = '';
var pCredit = '';
var heading = '';
var courseMapper = {
    version: "1.0.0",
    data: {},
    target: {
        coursesTable: "coursesTable",
        UCFPathwaySection: "UCFPathwaySection"
    },
    displayCourses: function (displayName, ucfCourses, partnerCourses) {
        let row = '';
        let displayNameOutput = (displayName.length > 0) ? '<table class="borderless" style="width:100%"><tr><td style="width:105px">&nbsp;</td><td>' + displayName + '</td></tr></table>' : '';
        let displayNameCount = 0;
        if (ucfCourses.length > 0) {
            for (let x = 0; x <= ucfCourses.length - 1; x++) {
                if (ucfCourses[x].Course == null) {
                    continue;
                }
                displayNameCount = (displayName.length > 0) ? displayNameCount + 1 : displayNameCount;
                ucfCourse += (displayName.length > 0) ? displayNameOutput : '';

                let sybmolSpace = '<div class="float-left" style="width:80px">&nbsp;</div>';
                let critical = courseMapper.getCritialCourseIcon(ucfCourses[x].Critical);
                let required = courseMapper.getRequiredCourseIcon(ucfCourses[x].Required);
                let cpp = courseMapper.getCPPIcon(ucfCourses[x].CPP);


                ucfCourseSymbols = critical + required + cpp;
                ucfCourse += '<table class="borderless" style="width:100%"><tr><td style="width:105px">' + ucfCourseSymbols + '</td><td>' + ucfCourses[x].Course + '</td></tr></table>';
                console.log(displayNameCount);
                if (displayName.length > 0) {
                    if (displayNameCount > 1) {
                        ucfCredit += '<table class="borderless mt-3" style="width:100%"><tr><td>&nbsp;</td></tr><tr><td class="text-center">' + ucfCourses[x].Credit + '</td></tr></table>';
                    } else {
                        ucfCredit += '<table class="borderless" style="width:100%"><tr><td>&nbsp;</td></tr><tr><td class="text-center">' + ucfCourses[x].Credit + '</td></tr></table>';
                    }

                } else {
                    ucfCredit += '<table class="borderless" style="width:100%"><tr><td class="text-center">' + ucfCourses[x].Credit + '</td></tr></table>';
                }
            }
        } else {
            //ucfCourseSymbols = '<div class="d-lg-block d-xl-none pb-2">&nbsp;</div><div>3' + ucfCourseSymbols + '</div>';
            ucfCourse += '<div>&nbsp;</div>';
            ucfCredit += '<div>&nbsp;</div><div class="d-lg-block d-xl-none pb-2">&nbsp;</div>';
        }

        if (partnerCourses.length > 0) {
            for (let x = 0; x <= partnerCourses.length - 1; x++) {
                pCourse += (displayName.length > 0) ? '<table class="borderless"><tr><td>' + displayName + '</td></tr></table>' : '';
                if (displayName.length > 0) {
                    if (displayNameCount > 0) {
                        pCredit += '<table class="borderless mt-5" style="width:100%"><tr><td class="text-center">' + partnerCourses[x].Credit + '</td></tr></table>';
                    } else {
                        //pCredit += '<div>&nbsp;</div><div class="d-lg-block d-xl-none pb-2">&nbsp;</div><div class="text-center">' + partnerCourses[x].Credit + '</div>';
                        pCredit += '<table class="borderless" style="width:100%"><tr><td class="text-center">' + partnerCourses[x].Credit + '</td></tr></table>';
                    }
                } else {
                    //pCredit += '<div class="text-center">' + partnerCourses[x].Credit + '</div>';
                    pCredit += '<table class="borderless" style="width:100%"><tr><td class="text-center">' + partnerCourses[x].Credit + '</td></tr></table>';
                }
                //pCourse += '<div>' + partnerCourses[x].Course + '</div>';
                pCourse += '<table class="borderless" style="width:100%"><tr><td>' + partnerCourses[x].Course + '</td></tr></table>';
            }
        } else {
            pCourse += '<div>&nbsp;</div>';
            pCredit += '<div>&nbsp;</div><div class="">&nbsp;</div>';
        }
    },
    getTD: function (item) {
        return '<td class="px-0">' + item + '</td>';
    },
    getCourseMapper: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/degree/GetCourseMapper?degreeId=" + main.degreeId,
            url: "/api/degree/GetCourseMapper?degreeId=" + courseMapper.degreeId,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                courseMapper.data = data;
                let tr = '';
                if (data != null && data.length > 0) {
                    for (let x = 0; x <= data.length - 1; x++) {
                        ucfCourse = '';
                        ucfCredit = '';
                        cpp = '';
                        critical = '';
                        required = '';
                        pCourse = '';
                        pCredit = '';
                        ucfCourseSymbols = '';
                        if (data[x].UCFCourses.length > 0 || data[x].PartnerCourses.length > 0) {
                            courseMapper.displayCourses(data[x].DisplayName, data[x].UCFCourses, data[x].PartnerCourses);
                        }
                        if (data[x].AlternateUCFCourse.length > 0 || data[x].AlternatePartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].AlternateDisplayName, data[x].AlternateUCFCourse, data[x].AlternatePartnerCourse);
                        }
                        if (data[x].Alternate2UCFCourse.length > 0 || data[x].Alternate2PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate2DisplayName, data[x].Alternate2UCFCourse, data[x].Alternate2PartnerCourse);
                        }
                        if (data[x].Alternate3UCFCourse.length > 0 || data[x].Alternate3PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate3DisplayName, data[x].Alternate3UCFCourse, data[x].Alternate3PartnerCourse);
                        }
                        if (data[x].Alternate4UCFCourse.length > 0 || data[x].Alternate4PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate4DisplayName, data[x].Alternate4UCFCourse, data[x].Alternate4PartnerCourse);
                        }
                        //tr += '<tr>' + courseMapper.getTD(ucfCourse) + courseMapper.getTD(ucfCredit) + courseMapper.getTD(pCourse) + courseMapper.getTD(pCredit) + '</tr>'
                        tr += '<tr>' + courseMapper.getTD(ucfCourse) + courseMapper.getTD(ucfCredit) + courseMapper.getTD(pCourse) + courseMapper.getTD(pCredit) + '</tr>'
                    }
                    $("#" + courseMapper.target.coursesTable).append(tr);
                }
            }
        })
    },
    getCritialCourseIcon(critical) {
        if (critical) return '<span class="badge badge-secondary p-1" title="Critical Course">C</span>';
        return '';
    },
    getRequiredCourseIcon(required) {
        if (required) return '<span class="badge badge-danger p-1" title="Required Course">R</span>';
        return '';
    },
    getCPPIcon(cpp) {
        if (cpp) return '<span class="badge badge-primary p-1" title="Common Course Prerequiste">CPP</span>';
        return '';
    },
    init: function () {
        courseMapper.degreeId = main.degreeId;
        courseMapper.institutionId = main.institutionId;
        this.getCourseMapper();
    }
}
$(function () {
    courseMapper.init();
})
var degreemap = {
    version: "2.0.0",
    data: {},
    target: {
        GPA: "GPA",
        LimitedAccess: "LimitedAccess",
        RestrictedAccess: "RestrictedAccess",
        AdditionalRequirement: "AdditionalRequirement",
        ForeignLanguageRequirement: "ForeignLanguageRequirement",
        ListItems: "ListItems",
        Institution: "Institution",
        InstitutionList: "InstitutionList",
        DegreemapRow: "DegreemapRow",
        Degree: "Degree",
        UCFPathwaySection: "UCFPathwaySection",
        CollegeName: "CollegeName",
        UCFCourseSection: "UCFCourseSection",
        NotesSection: "NotesSection",
    },

    degreeId: 15,
    institutionId: 0,
    collegeId: 1,
    catalogId: 1,
    ucfDegreeId: 0,
    hasUCFSemesters: false,

    displayDegreeInfo(data) {
        $("." + this.target.GPA).html(data.GPA)
        $("." + this.target.LimitedAccess).html(main.getYesNo(data.LimitedAccess));
        $("." + this.target.RestrictedAccess).html(main.getYesNo(data.RestrictedAccess));
        $("." + this.target.ForeignLanguageRequirement).html(data.ForeignLanguageRequirement);
        $("." + this.target.AdditionalRequirement).html(data.AdditionalRequirement);
        $("." + this.target.Degree).html(data.CatalogYear + ' ' + data.Degree);
        $("." + this.target.Institution).html(data.Institution);

        degreemap.displayListItems(data);
    },
    displayListItems: function (data) {
        if (data.Notes.length > 0) {
            let item = '';
            for (let x = 0; x <= data.Notes.length - 1; x++) {
                item += '<li>' + data.Notes[x] + '</li>';
            }
            $("#" + degreemap.target.ListItems).append(item);
        } else {
            $("#" + degreemap.target.NotesSection).hide();
        }
    },
    displayInstitutionList: function (data) {
        let output = '';
        for (var x = 0; x <= data.length - 1; x++) {
            //Need to select the degree for the new 
            output += '<a class="dropdown-item" href="/degree-mapping?degreeId=' + data[x].DegreeId + '">' + data[x].Institution + '</a>';
        }
        $('#' + this.target.InstitutionList).html(output);
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
    getDegreeInfo: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/degree/GetDegreeInfo?degreeId=" + main.degreeId,
            url: "/api/degree/GetDegreeInfo?degreeId=" + degreemap.degreeId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                degreemap.data = data;
                degreemap.displayDegreeInfo(data);
                degreemap.ucfDegreeId = data.UCFDegreeId;
                degreemap.getListByUCFDegree();
            }
        })
    },
    getListByUCFDegree: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/degree/GetListByUCFDegree?ucfDegreeId=" + degreemap.ucfDegreeId + "&catalogId=" + degreemap.catalogId,
            url: "/api/degree/GetListByUCFDegree?ucfDegreeId=" + degreemap.ucfDegreeId + "&catalogId=" + degreemap.catalogId,
            //data : "degreeId="4,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                degreemap.displayInstitutionList(data);
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
    init: function () {
        degreemap.degreeId = main.degreeId;
        degreemap.institutionId = main.institutionId;
        degreemap.getDegreeInfo();
    }
}
$(function () {
    degreemap.init();
})
var ucfSemesterTerm = {
    version: "1.0.0",
    data: {},
    semesterTerms: [],
    target: {
        UCFCourseSection: 'UCFCourseSection',
        UCFPathwaySection: 'UCFPathwaySection'
    },
    setSemesterTerms: function (data) {
        if (data.length > 0) {
            let term = '';
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
        card += '<p class="card-text">Total ' + total + ' Credits</p>';
        card += '</div>';
        card += '<div class="card-block">' + cardblock + '</div>';
        card += '</div>';
        return card;
    },
    displayUCFSemesterCourse: function (data) {
        let term = ucfSemesterTerm.semesterTerms;
        let breakpoint = 3;
        let emptyCard = '';
        if (term.length == 4 || term.length == 8) {
            breakpoint = 3;
        } else if (term.length == 5) {
            breakpoint = 2;
            emptyCard = '<div class="card" style="visibility:hidden"><p>empty</p></div>';
        } else if (term.length == 6) {
            breakpoint = 2;
        } else if (term.length == 7) {
            breakpoint = 3;
            emptyCard = '<div class="card"style="visibility:hidden"><p>empty</p></div>';
        }
        let carddeck = '';
        let carddeck2 = '';
        for (let x = 0; x <= term.length - 1; x++) {
            let total = 0;
            let cardblock = '';
            for (y = 0; y <= data.length - 1; y++) {
                if (data[y].SemesterTerm == term[x]) {
                    total = total + data[y].Credit;
                    cardblock += '<p class="card-text">' + data[y].Course + '<br/>' + data[y].Credit + ' Credits</p>'
                }
            }
            if (x <= breakpoint) {
                carddeck += ucfSemesterTerm.setCard(term[x], total, cardblock);
            } else {
                carddeck2 += ucfSemesterTerm.setCard(term[x], total, cardblock);
            }
            $('#UCFCourseSection').html('<div class="card-deck">' + carddeck + '</div>');
            $('#UCFCourseSection2').html('<div class="card-deck">' + carddeck2 + emptyCard + '</div>');
        }
    },
    getUCFSemesterCourse: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/degree/GetUCFSemesterCourse?degreeId=" + ucfSemesterTerm.degreeId,
            url: "/api/degree/GetUCFSemesterCourse?degreeId=" + ucfSemesterTerm.degreeId,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                ucfSemesterTerm.data = data;
                ucfSemesterTerm.setSemesterTerms(data);
                if (ucfSemesterTerm.semesterTerms.length > 0) {
                    ucfSemesterTerm.displayUCFSemesterCourse(data);
                } else {
                    $("#" + ucfSemesterTerm.target.UCFPathwaySection).remove();
                }
            }
        })
    },
    init: function () {
        ucfSemesterTerm.degreeId = main.degreeId;
        ucfSemesterTerm.institutionId = main.institutionId;
        this.getUCFSemesterCourse();
    }
}
$(function () {
    ucfSemesterTerm.init();
})
