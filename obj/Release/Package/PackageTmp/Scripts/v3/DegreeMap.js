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
    version: "1.0.5",
    data: {},
    target: {
        coursesTable: "coursesTable",
        UCFPathwaySection: "UCFPathwaySection",
        FooterDegreeMapLegend: "footerDegreeMapLegend",
        courseMappingLegend: "courseMappingLegend"
    },
    displayCourses: function (displayName, ucfCourses, partnerCourses) {
        let row = '';
        let displayNameOutput = (displayName.length > 0) ? '<div class="row"><div class="col-md-2 col-md-2 d-none d-md-block d-lg-block">&nbsp;</div><div class="col-md-10">' + displayName + '</div></div>' : '';
        let displayNameCount = 0;
        if (ucfCourses.length > 0) {
            ucfCourse += displayNameOutput;
            for (let x = 0; x <= ucfCourses.length - 1; x++) {
                if (ucfCourses[x].Course == null) {
                    continue;
                }
                let description = (ucfCourses[x].Description.length > 0) ? '<br><em>' + ucfCourses[x].Description + '</em>' : '';
                let critical = courseMapper.getCritialCourseIcon(ucfCourses[x].Critical);
                let required = courseMapper.getRequiredCourseIcon(ucfCourses[x].Required);
                let cpp = courseMapper.getCPPIcon(ucfCourses[x].CPP);
                ucfCourseSymbolsDesktop = '<div class="col-md-2 d-none d-md-block d-lg-block">' + critical + required + cpp + '</div>';
                ucfCourseSymbolsMobile = '<div class="d-block d-sm-block d-md-none">' + critical + required + cpp + '</div>';
                let course1 = '<div class="col-md-7">' + ucfCourses[x].Course + description + '</div>';
                let course2 = '<div class="col-md-3">' + ucfCourses[x].Credit + ' credits</div>';
                ucfCourse += '<div class="row">' + ucfCourseSymbolsDesktop + course1 + course2 + '</div>' + ucfCourseSymbolsMobile;
            }
        } else {
            ucfCourse += '<div>&nbsp;</div>';
            ucfCredit += '<div>&nbsp;</div><div class="d-lg-block d-xl-none pb-2">&nbsp;</div>';
        }
        if (partnerCourses.length > 0) {
            displayNameOutput = (displayName.length > 0) ? '<div class="row"><div class="col-md-12">' + displayName + '</div></div>' : '';
            pCourse += displayNameOutput;
            for (let x = 0; x <= partnerCourses.length - 1; x++) {
                if (partnerCourses[x].Course == null) {
                    continue;
                }
                let description = (partnerCourses[x].Description.length > 0) ? '<br><em>' + partnerCourses[x].Description + '</em>' : '';
                let pcourse1 = '<div class="col-md-8">' + partnerCourses[x].Course + description + '</div>';
                let pcourse2 = '<div class="col-md-4">' + partnerCourses[x].Credit + ' credits</div>';
                pCourse += '<div class="row">' + pcourse1 + pcourse2 + '</div>';
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
            //url: "https://portal.connect.ucf.edu/pathway/api/v2/DegreeMap/GetCourseMapper?degreeId=" + courseMapper.degreeId,
            url: "/api/v2/DegreeMap/GetCourseMapper?degreeId=" + courseMapper.degreeId,
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
                        if (data[x].Alternate5UCFCourse.length > 0 || data[x].Alternate5PartnerCourse.length > 0) {
                            courseMapper.displayCourses(data[x].Alternate5DisplayName, data[x].Alternate5UCFCourse, data[x].Alternate5PartnerCourse);
                        }
                        tr += '<tr>' + courseMapper.getTD(ucfCourse) + courseMapper.getTD(pCourse) + '</tr>'
                    }
                    $("#" + courseMapper.target.coursesTable).append(tr);
                } else {
                    courseMapper.displayNoCourseMapInfo();
                }
            }
        })
    },
    displayNoCourseMapInfo: function () {
        let template = '<tr><td colspan="2"><div>No course mappings available</div></td></tr>';
        $('#' + this.target.FooterDegreeMapLegend).html(template);
        $('#' + this.target.FooterDegreeMapLegend).removeClass('d-none');
        $('#' + this.target.FooterDegreeMapLegend).show();
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
        if (cpp) return '<span class="badge badge-primary p-1" title="Common Program Prerequiste">CPP</span>';
        return '';
    },
    init: function () {
        courseMapper.degreeId = main.degreeId;
        courseMapper.institutionId = main.institutionId;
        this.getCourseMapper();
    }
}
var customCourseSemester = {
    version: "1.0.3",
    target: {},
    data: {},
    getCustomCourseSemester: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/v2/DegreeMap/GetCustomCourseSemester?degreeId=" + main.degreeId,
            url: "/api/v2/DegreeMap/GetCustomCourseSemester?degreeId=" + main.degreeId,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            async: false
        }).done(function (data) {
            customCourseSemester.data = data;
            //=> moved to set card customCourseSemester.displayCustomCourseSemester(data);
        })
    },
    displayCustomCourseSemester(data, tid) {
        let row = '';
        for (let x = 0; x <= data.length - 1; x++) {
            let semester = (data[x].Term.length > 0) ? data[x].Semester + '_' + data[x].Term : data[x].Semester;
            if (tid == semester) {
                if ($(data[x].Note).length < 1) {
                    continue;
                }
                //let el = $(data[x].Note).find('span').removeAttr('style').html();
                let textObj = customCourseSemester.removeStyleAttributes($(data[x].Note));
                let note = data[x].Note;
                row += '<tr><td colspan="2" class="pl-4">' + note + '</td></tr>';
            }
        }
        return row;
    },
    removeStyleAttributes: function (htmlObj) {
        let removeStyles = "color: #444444; font-family: Calibri, sans-serif, 'Mongolian Baiti', 'Microsoft Yi Baiti', 'Javanese Text', 'Yu Gothic'; font-size: 14.6667px; white-space: pre;";
        textObj = $(htmlObj).text().replace(removeStyles, "");
        return $("<li>" + textObj + "</li>");
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
        Section_AdditionalRequirements: "section_additionalrequirements",
        Section_GPA: "section_gpa",
        Section_LimitedAccess: "section_limitedaccess",
        Section_RestrictionAccess: "section_restrictionaccess",
        Section_ForeignLaugangeRequirements: "section_foreignlaugangerequirements",
        AdditionalRequirement: "AdditionalRequirement",
        ForeignLanguageRequirement: "ForeignLanguageRequirement",
        ListItems: "ListItems",
        Institution: "Institution",
        InstitutionList: "InstitutionList",
        DegreemapRow: "DegreemapRow",
        Degree: "Degree",
        DegreeURL: "DegreeURL",
        DegreeURLHREF: "degreeURLHREF",
        UCFPathwaySection: "UCFPathwaySection",
        CollegeName: "CollegeName",
        UCFCourseSection: "UCFCourseSection",
        NotesSection: "NotesSection",
        CatalogYear: "CatalogYear",
        UndergraduateCatalogUrl: "UndergraduateCatalogUrl",
        GlobalCourseNotes : "GlobalCourseNotes"
    },
    degreeId: 0,
    institutionId: 0,
    collegeId: 0,
    catalogId: 0,
    ucfDegreeId: 0,
    hasUCFSemesters: false,
    displayDegreeInfo(data) {
        $("." + this.target.GPA).html(data.GPA)
        $("." + this.target.LimitedAccess).html(main.getYesNo(data.LimitedAccess));
        $("." + this.target.RestrictedAccess).html(main.getYesNo(data.RestrictedAccess));
        this.displayForeignLanguageRequirements(data);
        this.displayAdditionalRequirements(data);
        $("." + this.target.Degree).html(data.CatalogYear + ' ' + data.Degree);
        $("." + this.target.Institution).html(data.Institution);
        $("." + this.target.CatalogYear).html(data.CatalogYear);
        $("." + this.target.GlobalCourseNotes).html(data.GlobalCourseNotes);
        this.displayCareerPath(data.CareerPathURL);
        this.displayDegreeURLFooter(data);
        degreemap.displayListItems(data);
    },
    displayDegreeURLFooter: function (data) {
        if (data.DegreeUrl != null && data.DegreeUrl.length > 0) {
            $('.undergraduateCatalogUrlCard').hide();
            $("." + this.target.DegreeURLHREF).attr("href", data.DegreeUrl);
        } else {
            $('.degreeURLCard').hide();
            $("." + this.target.UndergraduateCatalogUrl).attr("href", data.UndergraduateCatalogUrl);
        }
    },
    displayAdditionalRequirements: function (data) {
        if (data.AdditionalRequirement != null && data.AdditionalRequirement.length > 1) {
            $("." + this.target.AdditionalRequirement).html(data.AdditionalRequirement);
            $('#' + this.target.Section_AdditionalRequirements).removeClass('d-none');
        }
    },
    displayForeignLanguageRequirements: function (data) {
        if (data.ForeignLanguageRequirement.length > 5) {
            $("." + this.target.ForeignLanguageRequirement).html(data.ForeignLanguageRequirement);
            $('#' + this.target.Section_ForeignLaugangeRequirements).removeClass('d-none');
        }
    },
    displayListItems: function (data) {
        if (data.Notes != null && data.Notes.length > 0) {
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
        let lastObj;
        for (var x = 0; x <= data.length - 1; x++) {
            //Need to select the degree for the new 
            if (data[x].InstitutionId != 7) {
                output += '<a class="dropdown-item" href="/degree-mapping?degreeId=' + data[x].DegreeId + '">' + data[x].Institution + '</a>';
            } else {
                lastObj = '<a class="dropdown-item" href="/degree-mapping?degreeId=' + data[x].DegreeId + '">' + data[x].Institution + '</a>';
            }
        }
        $('#' + this.target.InstitutionList).html(output + lastObj);
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
            //url: "https://portal.connect.ucf.edu/pathway/api/v2/DegreeMap/GetDegreeInfo?degreeId=" + degreemap.degreeId,
            url: "/api/v2/DegreeMap/GetDegreeInfo?degreeId=" + degreemap.degreeId,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            success: function (data) {
                if (data.Institution.toLowerCase() == 'ucf') {
                    window.location.replace("/home/DegreeList");
                }
                degreemap.data = data;
                degreemap.displayDegreeInfo(data);
                degreemap.ucfDegreeId = data.UCFDegreeId;
                degreemap.getListByUCFDegree();
            }
        })
    },
    displayCareerPath: function (careerPathUrl) {
        $('.careerPathURLCard').hide();
        if (careerPathUrl != null && careerPathUrl.length > 0) {
            $('.careerPathURLCard').show();
            $(".careerPathURL").attr("href", careerPathUrl)
        }
    },
    getListByUCFDegree: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/v2/DegreeMap/GetListByUCFDegree?ucfDegreeId=" + degreemap.ucfDegreeId + "&catalogId=" + degreemap.catalogId,
            url: "/api/v2/DegreeMap/GetListByUCFDegree?ucfDegreeId=" + degreemap.ucfDegreeId + "&catalogId=" + degreemap.catalogId,
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
    setCard: function (term, cardblock) {
        let termId = term.replace(" ", "_");
        let card = '<table class="w-100 table mb-0" id="SemesterTerm_' + termId + '">';
        card += '<tr><th colspan="2" class="bg-primary"><h4> Semester ' + term + '</h4></th></tr>';
        card += '<tr class="h6"><td>Course</td><td class="text-right">Credits</td></tr>';
        card += cardblock;
        let customCourseRow = customCourseSemester.displayCustomCourseSemester(customCourseSemester.data, termId);
        card += customCourseRow;
        card += '</table>';
        return card;
    },
    displayUCFSemesterCourse: function (data) {
        data.sort((a, b) => {
            if (a.Credit > b.Credit)
                return -1;
            if (a.Credit < b.Credit)
                return 1;
            return 0;
        })
        let term = ucfSemesterTerm.semesterTerms;
        let carddeck = '';
        for (let x = 0; x <= term.length - 1; x++) {
            let cardblock = '';
            let course = '';
            let description = '';
            let credits = '';
            for (y = 0; y <= data.length - 1; y++) {
                if (data[y].SemesterTerm == term[x]) {
                    course = data[y].Course;
                    credits = data[y].Credit;
                    let description = (data[y].Description.length > 0) ? '<p><em>' + data[y].Description + '</em></p>' : '';
                    cardblock += '<tr><td>' + data[y].Course + description + '</td><td class="text-right">' + data[y].Credit + '</td></tr>';
                }
            }
            carddeck += ucfSemesterTerm.setCard(term[x], cardblock);
        }
        $('#UCFCourseSection').html('<div>' + carddeck + '</div>');
        //=>This is now called getUCFSemesterCourse customCourseSemester.getCustomCourseSemester();
    },
    getUCFSemesterCourse: function () {
        $.get({
            //url: "https://portal.connect.ucf.edu/pathway/api/v2/DegreeMap/getCustomCourseMapper?degreeId=" + ucfSemesterTerm.degreeId,
            url: "/api/v2/DegreeMap/getCustomCourseMapper?degreeId=" + ucfSemesterTerm.degreeId,
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: true,
            async: false,
            success: function (data) {

            }
        }).done(function (data) {
            ucfSemesterTerm.data = data;
            ucfSemesterTerm.setSemesterTerms(data);
            if (ucfSemesterTerm.semesterTerms != null && ucfSemesterTerm.semesterTerms.length > 0) {
                customCourseSemester.getCustomCourseSemester();
                ucfSemesterTerm.displayUCFSemesterCourse(data);
            } else {
                $("#" + ucfSemesterTerm.target.UCFPathwaySection).remove();
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
$(window).on('load', function () {
    $('#UCFCourseSection li').each(function () {
        $(this).find('span').removeAttr('style').html()
        $(this).find('br').replaceWith(' ')
    })
})