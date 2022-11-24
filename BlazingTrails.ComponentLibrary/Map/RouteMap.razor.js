// Using a JS map library (Leaflet) inside Blazor app.
// Parameter is a reference to the element the map should be rendered in.
// Take a reference to the 'RouteMap' component, so that JavaScript can call C# methods inside it.
export function initialize(hostElement, routeMapComponent, existingWaypoints, isReadOnly)
{
    // Initialize Leaflet on the hostElement.
    hostElement.map = L.map(hostElement)
        .setView([51.700, -0.10], 3);

    // Layer that displays a copyright message in the bottom right of the map.
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
        {
        attribution: "&copy; <a href = 'https://www.openstreetmap.org/copyright' > OpenStreetMap</a> contributors",
        maxZoom: 18,
        opacity: .75
    }).addTo(hostElement.map);

    hostElement.waypoints = [];
    hostElement.lines = [];

    // Create markers and lines for existing waypoints.
    if (existingWaypoints && existingWaypoints.length > 0)
    {
        existingWaypoints.forEach(cord =>
        {
            let waypoint = L.marker(cord);
            waypoint.addTo(hostElement.map);
            hostElement.waypoints.push(waypoint);

            let line = L.polyline(hostElement.waypoints.map(m => m.getLatLng()),
                { color: "var(--brand)" }).addTo(hostElement.map);
            hostElement.lines.push(line);
        })
    }

    // Zoom the map to fit all the existing waypoints and center the route.
    if (hostElement.waypoints.length > 0)
    {
        let waypointsGroup = new L.featureGroup(hostElement.waypoints);
        hostElement.map.fitBounds(waypointsGroup.getBounds().pad(1));
    }

    if (!isReadOnly)
    {
        // Hook up a handler for the click event exposed by the map.
        // The handler will add a waypoint & add lines between the waypoints.
        hostElement.map.on("click", function (e)
        {
            let waypoint = L.marker(e.latlng);
            waypoint.addTo(hostElement.map);

            hostElement.waypoints.push(waypoint);

            let line = L.polyline(hostElement.waypoints.map(m => m.getLatLng()),
                { color: "var(--brand)" }).addTo(hostElement.map);

            hostElement.lines.push(line);

            // Call the C# 'WaypointAdded' method.
            routeMapComponent.invokeMethodAsync("WaypointAdded", e.latlng.lat, e.latlng.lng);
        })
    }

}

// Checks and deletes the last waypoint & line from the map.
export function deleteLastWaypoint(hostElement)
{
    if (hostElement.waypoints.length > 0)
    {
        let lastWaypoint = hostElement.waypoints[hostElement.waypoints.length - 1];
        hostElement.map.removeLayer(lastWaypoint);
        hostElement.waypoints.pop();

        if (hostElement.lines.length > 0)
        {
            let lastLine = hostElement.lines[hostElement.lines.length - 1];
            lastLine.remove(hostElement.map);
            hostElement.lines.pop();
        }

        // Return an object that can be deserialized into a 'LatLong' C# record.
        return { "Lat": lastWaypoint.getLatLng().lat, "Lng": lastWaypoint.getLatLng().lng };
    }
}