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
        let url = this.getURL(this.catalogData[x].Id);
                card += this.cardTemplate(this.catalogData[x].Year, url);
            }
            $('#' + this.targetId.catalogList).html(card);
        },
        getURL: function (id) {
            switch (id) {
                case 1: return "Catalog2020_2021";
                    break;
                case 2: return "Catalog2021_2022";
                    break;
                case 3: return "Catalog2022_2023";
                    break;
                case 4: return "Catalog2023_2024"
                    break;
                case 5: return "Catalog2024_2025";
                    break;
                default: return "Catalog2020_2021";
            }
        },
        cardTemplate: function (year, url,) {
        let template = '<div class="card text-center">';
            template += '<div class="card-header card-primary">';
            template += '<a href="/DegreeMap2/' + url + '" class="text-secondary">';
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