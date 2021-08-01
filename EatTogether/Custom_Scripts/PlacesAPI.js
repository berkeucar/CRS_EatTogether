function callPlaces() {
    autocomplete = new google.maps.places.Autocomplete(document.getElementById("PlaceAutocomplete"),
        {
            componentRestrictions: { 'country': ['tr'] },
            fields: ['name', 'types'],
            types: ['establishment']

        })
    autocomplete.addListener("place_changed", () =>
    {
        const place = autocomplete.getPlace();
        document.getElementById("PlaceNameData").value = place.name;
        document.getElementById("PlaceTypeData").value = place.types;
    })
}