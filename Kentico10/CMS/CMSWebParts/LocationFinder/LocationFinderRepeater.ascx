<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LocationFinderRepeater.ascx.cs" Inherits="CMSWebParts_LocationFinder_LocationFinderRepeater" %>

<style>
    #map {
        width: 500px;
        height: 400px;
    }
</style>
<table>
    <tr>
        <td>
            <div>Hover over a marker on the map to see the name of the location</div>
            <div id="map">
            <asp:Panel runat="server">
                <script>
                    //Once the page is initalized, the map is loaded in
                    (function($) {
                        (document).ready(function() {
                            initMap();
                        });
                    });

                    function initMap() {
                        //Get the arrays with the data from the Code Behind
                        var descriptions = <%=Serialize(arrTitles) %>;
                        var longitudes = <%=Serialize(arrLongitudes) %>;
                        var latitudes = <%=Serialize(arrLatitudes) %>;
                        //Initalize Map Options
                        var myOptions = {
                            //Sets the zoom to what the user entered into the webpart propterty
                            zoom: <%= ZoomLevel %>,
                            //Set the center of the map to the average long/lat of all the points
                            center: new google.maps.LatLng(<%= GetAvgLatitude() %>, <%= GetAvgLongitude()%>)
                        };
                        //Place the map at the location of the div
                        var mapDiv = document.getElementById('map');
                        var map = new google.maps.Map(mapDiv, myOptions);
                        //Place a marker on the map for every pice of data in the arrays
                        for (var i = 0; i < descriptions.length; i++) {
                            new google.maps.Marker({
                                position: new google.maps.LatLng(latitudes[i], longitudes[i]),
                                map: map,
                                title: descriptions[i]
                            });
                        }
                    }

                </script>
                <!--Code to generate and produce the map is pulled from their publicly available APIs. This one is their JavaScript API-->
                <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfYtZ_xd_3h2rSCRebendM6OMKrCDRIJk&callback=initMap" type="text/javascript" async defer></script>
            </asp:Panel>
            </div>
        </td>
        <td>
            <ul>
                <asp:Repeater runat="server" ID="rptLocatioons">
                    <ItemTemplate>
                        <li>
                            <h2><%# Eval("LocationName") %></h2>
                            <h4>Click <a href="<%# Eval("LocationDirectionsUrl") %>" target="_blank">here</a> for directions</h4>
                            <p><%# Eval("LocationStreetAddress") %></p>
                            <p><%# Eval("LocationTown") %></p>
                            <p><%# Eval("LocationState") %></p>
                            <p><%# Eval("LocationZip") %></p>
                        </li>
                        <hr>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </td>
    </tr>
</table>