//$("#search-button").on("submit", function () {

//    $.ajax({
//        url: "/Home/LikeToPost?id=" + id,
//        method: "GET",
//        success: function (result) {
//            $('#' + id).load(document.URL + ' #' + id);
//        },
//        //error: function (error) {
//        //    alert("Fail");
//        //}
//    });

//});

$("#search-button").submit(function (e) {
    e.preventDefault();
    var username = document.getElementById("search-input");
    $.ajax({
        url: "/Utente/FindSimilarUsernames?username=" + username.value,
        method: "GET",
        success: function (result) {
            /*console.log(result);*/
            result.data.forEach(Search);
        },
        //error: function (error) {
        //    alert("Fail");
        //}
    });
});
var searchResults = document.getElementById("search-results");
function Search (item, index) {
    if (index == 0) {
        searchResults.style.display = "none";
        searchResults.innerHTML = "";
    }
    searchResults.style.display = "block";
    searchResults.innerHTML += `<div class="d-flex py-2">
                      <div class="col-3"><img src="/images/icon.png" alt="logo" width=35px></div>
                      <div class="col-9 py-1">
                        <a class="nome-utente" href="/Utente/VisualizzaUtente?username=` + item.userName + `" >` + item.userName + `</a>
                      </div>
                    </div>`;
}