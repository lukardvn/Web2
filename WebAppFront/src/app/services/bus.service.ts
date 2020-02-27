import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Vehicle } from 'src/app/modeli';

declare var $;

@Injectable({
  providedIn: 'root'
})

export class BusService {
  private proxy: any;  
  private proxyName: string = 'Vehicle';  
  private connection: any;  
  public connectionExists: Boolean; 

  public notificationReceived: EventEmitter <string>;  

  constructor(private http: HttpClient) {
      this.notificationReceived = new EventEmitter<string>();
      this.connectionExists = false;  
      // create a hub connection  
      this.connection = $.hubConnection("http://localhost:52295/");
      console.log('connection iz constructora',this.connection);
      // create new proxy with the given name 
      this.proxy = this.connection.createHubProxy(this.proxyName);  
      console.log('proxy iz constructora',this.proxy);
  }

  // browser console will display whether the connection was successful    
  public startConnection(): Observable<Boolean> { 
      
    return Observable.create((observer) => {
        this.connection.start()
        .done((data: any) => {  
            console.log('Now connected ' + data.transport.name + ' (Vehicles Location WS), connection ID=' + data.id)
            this.connectionExists = true;

            observer.next(true);
            observer.complete();
        })
        .fail((error: any) => {  
            console.log('Could not connect (Vehicles Location WS) ' + error);
            this.connectionExists = false;

            observer.next(false);
            observer.complete(); 
        });  
      });
  }

  public registerVehiclesLocations() : Observable<Array<Vehicle>> {
    return Observable.create((observer) => {
        this.proxy.on('newPositions', (data: string) => {
          console.log('usaosam u metodu za registrovanje lokacije vozila');
          console.log(data);
            let vehicles = new Array<Vehicle>();
            data.split('|').forEach(vehicle => {
              let splitedData = vehicle.split(",");
              let bus = new Vehicle();
              bus.VehicleID = splitedData[0];
              bus.X = Number.parseFloat(splitedData[1]);
              bus.Y = Number.parseFloat(splitedData[2]);
              bus.TransportLineID = splitedData[3];
              vehicles.push(bus);
            });
            
            observer.next(vehicles);
        });  
    });
  }
}