$(document).ready(function () {
    var NID = '@User.Identity.Name';
    $(favoriteDegree).on('click', function () {
        console.log(@degreeId);
    updateFavoriteDegree(@degreeId);
        })
function showFavoriteDegreeMessage(message) {
    $('#favoriteDegreeMessage').html(message).show();
}
function hideFavoriteDegreeMessage() {
    $.when($('#favoriteDegreeMessage').fadeOut(500))
        .done(function () {
            $('#favoriteDegreeMessage').removeClass('alert-success');
            $('#favoriteDegreeMessage').removeClass('alert-danger');
        });
}
function updateFavoriteDegree(degreeId) {
    $.get({
        url: "/app/SetFavoriteDegree?degreeId=" +@degreeId+"&NID=" + NID,
            //url: "/pathway/app/SetFavoriteDegree?id="+val+"&NID="+NID,
            type: "GET",
                cache: false,
                    success: function (isFavorite) {
                        console.log(isFavorite);
                        if (isFavorite) {
                            $('#favoriteDegreeMessage').addClass('alert alert-success');
                            showFavoriteDegreeMessage('<span class="fa fa-check"></span>');
                        } else {
                            $('#favoriteDegreeMessage').addClass('alert alert-danger');
                            showFavoriteDegreeMessage('<span>Removed</span>');
                        }
                        hideFavoriteDegreeMessage();
                    }
})
        }
    })