    var pathwayCatalog = {
        catalogData: [],
        targetId: {
        catalogList: "catalogList"
        },
        populateCatalogList: function () {
            if (this.catalogData.length > 0) {
                for (let x = 0; x <= this.catalogData.length - 1; x++) {
        $('#' + this.targetId.catalogData).append($('<option>', {
            value: this.catalogData[x].Id,
            text: this.catalogData[x].Year
        }));
                }
            }
        },
        displayCatalogList: function () {
        let card = '';
            for (let x = 0; x <= this.catalogData.length - 1; x++) {
                let url = this.getURL(this.catalogData[x].Id, this.catalogData[x].Current);
                card += this.cardTemplate(this.catalogData[x].Year, url);
            }
            $('#' + this.targetId.catalogList).html(card);
        },
        getURL: function (id, current) {
            if (current == 1) {
                return "/pathway-degree-list";
            }
            switch (id) {
                case 1: return "pathway-catalog-2020-2021";
                    break;
                case 2: return "pathway-catalog-2021-2022";
                    break;
                case 3: return "pathway-catalog-2022-2023";
                    break;
                case 4: return "pathway-catalog-2023-2024"
                    break;
                case 5: return "pathway-catalog-2024-2025";
                    break;
                default: return "pathway-degree-list";
            }
        },
        cardTemplate: function (year, url,) {
        let template = '<div class="card text-center">';
            template += '<div class="card-header card-primary">';
            template += '<a href="/' + url + '" class="text-secondary">';
            template += '<h1>Pathway Catalog<br />' + year + '</h1>';
            template += '<div><i class="fa fa-book fa-4x"></i></div>';
            template += '</a>';
            template += '</div>';
            template += '</div>';
            return template;
        },

        getCatalogList: function () {
        $.get({
            url: "https://portal.connect.ucf.edu/pathway/api/v2/DegreeMap/GetCatalogs",
            //url: "/api/v2/DegreeMap/GetCatalogs",
            type: "GET",
            headers: { "APIKey": "Th1sIsth3Way" },
            cache: false,
            success: function (data) {
                console.log(data);
                pathwayCatalog.catalogData = data;
                pathwayCatalog.displayCatalogList();
            }
        })
    },
        init: function () {
        this.populateCatalogList();
        }
    }
    $(document).ready(function () {
        pathwayCatalog.getCatalogList();
        pathwayCatalog.init();
    })