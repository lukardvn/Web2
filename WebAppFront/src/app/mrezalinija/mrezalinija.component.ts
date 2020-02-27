import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { BusService } from '../services/bus.service';
import {} from 'googlemaps';
import {TransportLine,Vehicle,Station,StationsOnLine} from '../modeli';

@Component({
  selector: 'app-mrezalinija',
  templateUrl: './mrezalinija.component.html',
  styleUrls: ['./mrezalinija.component.css']
})
export class MrezalinijaComponent implements OnInit {

  constructor(private http: AuthHttpService,private bus: BusService) { }

  linije : string[];
  allIds : string[];
  selectedLinija : string;
  poruka : string;
  latitude = 45.267136;
  longitude = 19.833549;
  line: TransportLine;
  item: Station;
  stationsOnLines : StationsOnLine;
  stations: Station[];
  displayedBuses: Array<any> = new Array<any>();
  displayedLines: Array<any> = new Array<any>();
  displayedPointsForLines: Array<any> = new Array<any>();
  stanicePrikazaneNaMapi: Array<any> = new Array<any>();
  @ViewChild('map',{static:true}) mapEl: ElementRef;
  map: google.maps.Map;
  isConnectedWS: Boolean = false;

  ngOnInit() {
    this.http.GetLinije().subscribe((allLines)=> {
      this.linije = allLines;
      err => console.log(err);
    });

    this.initMap();
    this.initWS();
  }

  private initWS() {
    this.checkConnection();
    this.subscribeForBusPositions();
  }

  private checkConnection(){
    this.bus.startConnection().subscribe(e => {
      this.isConnectedWS = e;
    });
  }
  
  private subscribeForBusPositions () {
    this.bus.registerVehiclesLocations().subscribe(
      data => {
        console.log('data iz subscribe metode',data);
        data.forEach(bus => {
          this.DrawBusOnMap(bus);
        });
      }
    )
  }

  private initMap() {
    const mapProperties = {
      center: new google.maps.LatLng(45.248636, 19.833549),
      zoom: 14,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
      disableDefaultUI: true,
      mapTypeControlOptions: {
      mapTypeIds: ['styled_map']
    }
  };

  var styledMap = new google.maps.StyledMapType([ { "elementType": "geometry", "stylers": [{ "color": "#ebe3cd" }] }, { "elementType": "labels.text.fill", "stylers": [{ "color": "#523735" }] }, { "elementType": "labels.text.stroke", "stylers": [{ "color": "#f5f1e6" }] }, { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#c9b2a6" }] }, { "featureType": "administrative.land_parcel", "elementType": "geometry.stroke", "stylers": [{ "color": "#dcd2be" }] }, { "featureType": "administrative.land_parcel", "elementType": "labels.text.fill", "stylers": [{ "color": "#ae9e90" }] }, { "featureType": "landscape.natural", "elementType": "geometry", "stylers": [{ "color": "#dfd2ae" }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#dfd2ae" }] }, { "featureType": "poi", "elementType": "labels.text.fill", "stylers": [{ "color": "#93817c" }] }, { "featureType": "poi.park", "elementType": "geometry.fill", "stylers": [{ "color": "#a5b076" }] }, { "featureType": "poi.park", "elementType": "labels.text.fill", "stylers": [{ "color": "#447530" }] }, { "featureType": "road", "elementType": "geometry", "stylers": [{ "color": "#f5f1e6" }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#fdfcf8" }] }, { "featureType": "road.highway", "elementType": "geometry", "stylers": [{ "color": "#f8c967" }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#e9bc62" }] }, { "featureType": "road.highway.controlled_access", "elementType": "geometry", "stylers": [{ "color": "#e98d58" }] }, { "featureType": "road.highway.controlled_access", "elementType": "geometry.stroke", "stylers": [{ "color": "#db8555" }] }, { "featureType": "road.local", "elementType": "labels.text.fill", "stylers": [{ "color": "#806b63" }] }, { "featureType": "transit.line", "elementType": "geometry", "stylers": [{ "color": "#dfd2ae" }] }, { "featureType": "transit.line", "elementType": "labels.text.fill", "stylers": [{ "color": "#8f7d77" }] }, { "featureType": "transit.line", "elementType": "labels.text.stroke", "stylers": [{ "color": "#ebe3cd" }] }, { "featureType": "transit.station", "elementType": "geometry", "stylers": [{ "color": "#dfd2ae" }] }, { "featureType": "transit.station.bus", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "elementType": "geometry.fill", "stylers": [{ "color": "#b9d3c2" }] }, { "featureType": "water", "elementType": "labels.text.fill", "stylers": [{ "color": "#92998d" }] }], {name: "styled_map"});

  this.map = new google.maps.Map(this.mapEl.nativeElement, mapProperties);
  this.map.mapTypes.set('styled_map', styledMap);
  this.map.setMapTypeId('styled_map');
  }



  onChange(event) {
    this.DrawLineOnMap();
  }


  private DrawBusOnMap(bus:Vehicle) {
    console.log('bus',bus);
    console.log(this.displayedLines);
    if ((bus.TransportLineID in this.displayedLines)) {
      if (bus.VehicleID in this.displayedBuses) {
        var linija = this.displayedLines[bus.TransportLineID];
        console.log('Linija iz drugog ifa',linija);
        if(linija.Vehicles.find(x => x.Id == bus.VehicleID) == null){
          linija.Vehicles.push(bus);
        }
        console.log('Linija iz drugog iffa posle zadnjeg unutrasnjeg ifa',linija);
        this.displayedBuses[bus.VehicleID].setTitle(bus.VehicleID);
        this.displayedBuses[bus.VehicleID].setZIndex(120);
        this.displayedBuses[bus.VehicleID].setPosition(
          new google.maps.LatLng(bus.X, bus.Y)
        );
        console.log('usao u unutrasnji if');
      } else {
        let markerIconPath = "../../assets/rsz_google-maps-bus-icon-55.png";
        let busMarker = this.DrawMarkerOnMap(bus.X, bus.Y, bus.VehicleID + "|" + bus.TransportLineID, markerIconPath) as google.maps.Marker;
        busMarker.setZIndex(120);
        busMarker.addListener('click', () => {
          let content =
            `<div>Registracija: <b>${bus.VehicleID}</b></div>
            <div>Trenutna linija: ${bus.TransportLineID}</div>`;
          var infoWindow = new google.maps.InfoWindow();
          infoWindow.setContent(content);
          infoWindow.open(this.map, busMarker);
        });
        this.displayedBuses[bus.VehicleID] = busMarker;
      }
    } else if (bus.VehicleID in this.displayedBuses) {
      this.RemoveBusFromMap(bus);
    }
  }

  private RemoveBusFromMap(bus:Vehicle) {
    if (bus.VehicleID in this.displayedBuses) {
      this.displayedBuses[bus.VehicleID].setMap(null);
      delete this.displayedBuses[bus.VehicleID];
    }
  }

  Submit(){
    for(let l in this.displayedLines){
      this.displayedPointsForLines[l].setMap(null);

      let deleteLine = this.displayedLines[l] as TransportLine;

      deleteLine.Stations.forEach(station => {
        this.stanicePrikazaneNaMapi[station.StationID.toString() + "_" + l].setMap(null);
      });

      deleteLine.Vehicles.forEach(vehicle =>{
        this.RemoveBusFromMap(vehicle);
      });

      delete this.displayedPointsForLines[l];
      delete this.displayedLines[l];
    }
  }

  DrawLineOnMap() {
    if(this.selectedLinija in this.displayedLines){
      let linijaZaBrisanje = this.displayedLines[this.selectedLinija] as TransportLine;
      this.displayedPointsForLines[this.selectedLinija].setMap(null);
      
      linijaZaBrisanje.Stations.forEach(element => {
        this.stanicePrikazaneNaMapi[element.StationID.toString() + "_" + this.selectedLinija].setMap(null);
      });
  
      linijaZaBrisanje.Vehicles.forEach(element => {
        this.RemoveBusFromMap(element);
      });
      delete this.displayedPointsForLines[this.selectedLinija];
      delete this.displayedLines[this.selectedLinija];
      return;
    }

    this.http.GetLine(this.selectedLinija).subscribe( data=>{
      this.line = data;
      this.http.GetStationsOnLine().subscribe( stations =>{
        this.stations = stations;
        this.line.Stations = stations.filter(a=>a.TransportLineID == this.selectedLinija);
        let lineCoordinatesGoogleArray = new Array<google.maps.LatLng>();
        this.line.LinePoints.forEach(item=>{
          lineCoordinatesGoogleArray.push(new google.maps.LatLng(item.X,item.Y))
        });
        console.log('Data iz getlinija ',data);
    
        let polyOptions = {
          path: lineCoordinatesGoogleArray,
          geodesic: true,
          strokeColor: 'red',
          strokeOpacity: 1,
          strokeWeight: 4,
        }
    
        this.displayedPointsForLines[this.line.TransportLineID] = new google.maps.Polyline(polyOptions);
        this.displayedPointsForLines[this.line.TransportLineID].setMap(this.map);
        this.displayedLines[this.line.TransportLineID] = this.line;

        if( this.line.Stations.length > 0){
          this.line.Stations.forEach(station=>{
            this.item = station;
            this.DrawBusStopsforLine(this.item,this.line.TransportLineID);
          });
        }
        err=>console.log(err);
      });
      err=>console.log(err);
    });
  }

  private DrawMarkerOnMap(latX: number, lngY: number, title: string, customIcon:string = "", admin: boolean = false): google.maps.Marker {
    let marker = null;
    if (customIcon.trim().length > 0 && admin == true) {
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(latX, lngY),
        map: this.map,
        title: title,
        icon: customIcon,
        draggable: true
      });
    } else if (customIcon.trim().length > 0) {
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(latX, lngY),
        map: this.map,
        //title: title,
        icon: customIcon
      });
    } else {
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(latX, lngY),
        map: this.map,
        title: title
      });
    }
    
    marker.setMap(this.map);

    return marker;
  }

  private DrawBusStopsforLine(station:any,lineId: string) {
    let markerIconPath = "";
    if(lineId.endsWith("A")){
      markerIconPath = "../../assets/rsz_busmarkeradirection.png";
    }
    else{
      markerIconPath = "../../assets/rsz_busmarkerbdirection.png";
    }

    //let marker = this.DrawMarkerOnMap(X, Y, address + "|" + address, markerIconPath);
    let marker = null;
    // if (markerIconPath.trim().length > 0 && admin == true) {
    //   marker = new google.maps.Marker({
    //     position: new google.maps.LatLng(latX, lngY),
    //     map: this.map,
    //     title: title,
    //     icon: customIcon,
    //     draggable: true
    //   });
    // } else
    if (markerIconPath.trim().length > 0) {
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(station.Station.X, station.Station.Y),
        map: this.map,
        //title: title,
        icon: markerIconPath
      });
    } else {
      marker = new google.maps.Marker({
        position: new google.maps.LatLng(station.Station.X, station.Station.Y),
        map: this.map,
        title: station.Station.Name
      });
    }
    
    marker.setMap(this.map);

    let infoWindow = new google.maps.InfoWindow();
    marker.addListener('click', () => {
      let content = "";

      content += `<div><b>${station.Station.Name}</b></div>`;
      content += `<div>${station.Station.Address}</div>`;
      infoWindow.setContent(`${content}`);
      infoWindow.open(this.map, marker);
    });

    this.stanicePrikazaneNaMapi[station.Station.StationID.toString() + "_" + lineId] = marker;
  }
}
