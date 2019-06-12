import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable()//kreiran servis
export class HttpService{

    getName(): Observable<string>{  
        //return "Neko ime Salo";
        return of("Neko ime Salo");
    }
}