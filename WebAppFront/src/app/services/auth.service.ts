import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegUser } from 'src/app/modeli';
import { Observable } from 'rxjs/internal/Observable';

@Injectable()
export class AuthHttpService{
    base_url = "http://localhost:52295"
  constructor(private http: HttpClient){
      
    }
  user: string
    logIn(username: string, password: string): Observable<boolean> | boolean{
        let isDone: boolean = false;
        let data = `username=${username}&password=${password}&grant_type=password`;
        let httpOptions : {
            headers: {
                "Content-type": "application/x-www-form-urlencoded"
            }
        }

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

    reg(data: RegUser) : Observable<any>{
        return this.http.post<any>(this.base_url + "/api/Account/Register", data);
    }

 
    GetPolasci(id: number, dan : string) : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Linijas/GetLinija/" + id +"/" + dan);
    }

    GetLinije() : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Linijas/");
    }

    //samo da se iscita json na serveru i popuni baza
    ParsiranjeJson(id: number, dan : string) : Observable<any> {
        return this.http.get<any>(this.base_url + "/api/Linijas/GetLinija/" + id + "/" + dan + "/" + "str");
    }

    GetCenaKarte(tip: string, tipPutnika: string): Observable<any>{
        return this.http.get<any>(this.base_url + "/api/Kartas/GetKarta/" + tip);
    }
    
    GetKupiKartu(tipKarte: string): Observable<any>{
       
        return this.http.get<any>(this.base_url + "/api/Kartas/GetKartaKupi2/" + tipKarte );
    }

    GetStanicaCord(idStanice: string): Observable<any>{
        return this.http.get<any>(this.base_url + "/api/Stanicas/GetStanica/" + idStanice);
    }
    GetProveriKartu(idKorisnika: string): Observable<any>{
       
        return this.http.get<any>(this.base_url + "/api/Kartas/GetProveri/" + idKorisnika );
    }
 
}