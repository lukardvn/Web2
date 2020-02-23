import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegUser, StaniceNaLiniji, RegUser2 } from 'src/app/modeli';
import { Observable } from 'rxjs/internal/Observable';
import { Stanica } from '../modeli';
import { raspored } from '../modeli';
import { linijica } from '../modeli';
import { Sifra } from '../modeli';
import { CenovnikEdit } from '../modeli';
import { StanicaUpdate } from '../modeli';
import { rasporedUpdate } from '../modeli';
import { JsonPipe } from '@angular/common';

@Injectable()
export class AuthHttpService{
    base_url = "http://localhost:52295"
  constructor(private http: HttpClient){
      
    }
  user: string
  tempStr : string[]
    logIn(username: string, password: string): Observable<boolean> | boolean{
        let isDone: boolean = false;
        let data = `username=${username}&password=${password}&grant_type=password`;
        let httpOptions : {
            headers: {
                "Content-type": "application/x-www-form-urlencoded"
            }
        }
        console.log(username);
        this.http.post<any>(this.base_url + "/oauth/token", data, httpOptions).subscribe(data => {
            localStorage.jwt = data.access_token;
            let jwtData = localStorage.jwt.split('.')[1]
            let decodedJwtJsonData = window.atob(jwtData)
            let decodedJwtData = JSON.parse(decodedJwtJsonData)

  
            let role = decodedJwtData.role
            this.user = decodedJwtData.unique_name;
        });

        if(localStorage.jwt != "undefined"){
            isDone = true;
        }
        else{
            isDone = false;
        }
        console.log(isDone);

        return isDone;
        
    }

    reg(data: RegUser,file: File) : Observable<any>{
        var formData = new FormData();


        if(file != null)
            formData.append("0",file, file.name);

        for(var k in data)
            formData.append(k,data[k]);

        return this.http.post<any>(this.base_url + "/api/Account/Register", formData);
    }

    ChangePW(sif: Sifra) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/Account/ChangePassword", sif);
    }

    UploadPicture(file: File) : Observable<any> {
        var formData = new FormData();


        if(file != null)
            formData.append("0",file, file.name);

        return this.http.post<any>(this.base_url + "/api/Account/UploadPictures",formData);
    }

    GetUsersToVerify() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Account/ProcessingUsers");
    }

    VerifyUser(id: string) {
        return this.http.put<any>(this.base_url + "/api/Account/VerifyUser/" + id,id);
    }
    DenyUser(id: string) {
        return this.http.put<any>(this.base_url + "/api/Account/VerifyUser/",id);
    }
 
    GetPolasci(id: number, dan : string) : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Linijas/GetLinija/" + id +"/" + dan);
    }

    GetPriceList() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/priceList/getPriceList/");
    }

    DeletePriceList(id: number) : Observable<any>{
        return this.http.delete<any>(this.base_url + "/api/priceList/deleteCenovnik/" + id);
    }

    UpdateCenovnik(st : CenovnikEdit) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/priceList/updateCenovnik/", st);
    }

    PostCenovnik(st : CenovnikEdit) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/priceList/newCenovnik/", st);
    }

    GetLinije() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/transportlines/getAllLines/");
    }

    GetTrenutniCenovnik() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/priceList/getTrenutniCenovnik/");
    }

    GetLinije2() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/transportlines/getAllLinesForUpdate/");
    }

    GetProfileInfo(str : string) : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Account/getProfileInfo/" + str);
    }

    GetRedove() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/departures/getAllDepartures/");
    }

    GetRedove2() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/departures/getAllDeparturesForUpdate/");
    }

    GetStanice() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/stations/getAllStations/");
    }

    GetStanice2() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/stations/getAllStationsForUpdate/");
    }

    DeleteStation(id: number) : Observable<any>{
        return this.http.delete<any>(this.base_url + "/api/stations/deleteStation/" + id);
    }



    DeleteLinija(id: number) : Observable<any>{
        return this.http.delete<any>(this.base_url + "/api/transportLines/deleteLine/" + id);
    }

    PostDeparture(rasp : raspored) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/departures/postDeparture/", rasp);
    }

    
    UpdateProfil(ru : RegUser2) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/Account/updateProfileInfo/", ru);
    }

    UpdateLine(linija : linijica) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/transportLines/updateLiniju/", linija);
    }

    UpdateStation(st : StanicaUpdate) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/stations/updateStanicu/", st);
    }

    UpdateDeparture(r : rasporedUpdate) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/departures/updateDeparture/", r);
    }

    PostLine(lin : linijica) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/transportLines/dodajLiniju/", lin);
    }

    DeleteRed(id: number) : Observable<any>{
        return this.http.delete<any>(this.base_url + "/api/departures/deleteDepartures/" + id);
    }

    //samo da se iscita json na serveru i popuni baza
    ParsiranjeJson(id: number, dan : string) : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Linijas/GetLinija/" + id + "/" + dan + "/" + "str");
    }

    DodajStanicuNaLiniju(stanicice : StaniceNaLiniji) : Observable<any>{
        console.log(stanicice);
        return this.http.post<any>(this.base_url + "/api/transportlines/addStationInLine/", stanicice);
    }

    DodajStanicu(stanica : Stanica) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/stations/dodajStanicu", stanica);
    }

    GetCenaKarte(tip: string, tipPutnika: string): Observable<any>{
        console.log(tip);
        console.log(tipPutnika);
        return this.http.get<any>(this.base_url + "/api/tickets/getCena/" + tipPutnika + "/" + tip);
    }
    
    GetKupiKartu(email: string): Observable<any>{
       console.log(email);
        this.tempStr = JSON.stringify(email).split('"');
        return this.http.get<any>(this.base_url + "/api/tickets/BuyTicketAnonymus/" + this.tempStr[3]);
    }

    KupiKartu(email: string, karta : string): Observable<any>{
        console.log(email);
         this.tempStr = JSON.stringify(email).split('"');
         return this.http.get<any>(this.base_url + "/api/tickets/BuyTicketRegular/" + this.tempStr[3] + "/" + karta);
    }
    //zavrsiti ovde sam stao
    // BuyTicket(karta: string, json: any): Observable<any> {
    //     return this.http.post(this.base_url + "/api/tickets/BuyTicket/" + karta ,json, { "headers" : {'Content-type' : 'application/json'}});
    // }
    BuyTicket(karta: string, korisnik: string,json: any): Observable<any> {
        return this.http.post(this.base_url + "/api/tickets/BuyTicket/" + karta + "/" + korisnik ,json,{ "headers" : {'Content-type' : 'application/json'}} );
    }

    GetUserType(): Observable<any> {
        return this.http.get<any>(this.base_url + "/api/tickets/UserType");
    }

    GetStanicaCord(idStanice: string): Observable<any>{
        return this.http.get<any>(this.base_url + "/api/Stanicas/GetStanica/" + idStanice);
    }
    GetProveriKartu(idKorisnika: string): Observable<any>{
       
        return this.http.get<any>(this.base_url + "/api/Kartas/GetProveri/" + idKorisnika );
    }

    GetLine(lineId: string): Observable<any> {
        return this.http.get<any>(this.base_url + "/api/TransportLines/" + lineId);
    }
    GetStationsOnLine(): Observable<any> {
        return this.http.get<any>(this.base_url + "/api/StationsOnLines");
    }
 
}