let marker
let map

function onMapClick(e) {
    document.querySelector("#latitude").value = e.latlng.lat
    document.querySelector("#longitude").value = e.latlng.lng
    if (marker) {
        map.removeLayer(marker)
    }
    marker = L.marker(e.latlng).addTo(map)
}

function initMap() {
    let lat = document.querySelector("#latitude").value
    let lng = document.querySelector("#longitude").value

    map = L.map('map');

    if (lat && lng) {
        map.setView([parseFloat(lat), parseFloat(lng)], 12);
        marker = L.marker([parseFloat(lat), parseFloat(lng)]).addTo(map);
    } else {
        map.setView([50.4501, 30.5234], 12);
    }
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);
    map.on('click', onMapClick);
}

initMap();