    $(document).ready(function () {
        let searchInput = $('#searchInput');
        var hiddenProducts = $('.hiddenProduct');
        let containerToAppend = $('#containerToAppend');
        let counter = 0;
        let searchTextCounter = 0;
        $(searchInput).keyup(function () {
            var searchText = searchInput.val().toLowerCase();
            if (counter == 0 && searchText.length >= 2) {
                if ($("#hideNoProductsMessage").length) {
                    $('#hideNoProductsMessage').show();
                }
                else {
                    containerToAppend.append('<p id="hideNoProductsMessage">No products found with that name, please try again with a new name!</p>');
                }
                }
            if (searchTextCounter <= 0) {
                searchTextCounter = searchText.length;
            
            }
            for (var i = 0; i < hiddenProducts.length; i++) {
           
                if (searchText == "") {
                    counter = 0;
                    searchTextCounter = 0;
                    hiddenProducts.hide();
                    $('#hideNoProductsMessage').hide();

                }
                else if (hiddenProducts[i].textContent.toLowerCase().indexOf(searchText) == 0) {
                    if (counter == 3) {
                        if (searchText.length > searchTextCounter) {
                            counter = 0;
                            searchTextCounter = searchText.length;
                        }
                        else {
                            return false;
                        }
                    }
                    else {
                        $('#hideNoProductsMessage').hide();
                        counter++;
                        $(hiddenProducts[i]).show();
                    }
                }
                else {
                    $(hiddenProducts[i]).hide();
                }
            }
        });
    });
