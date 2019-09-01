import { Component, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material'
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private router: Router) { }
  role : any;

  ngOnInit() {
    if(localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != ""){
      let jwtData = localStorage.jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData)


       this.role = decodedJwtData.nameid
    
    }
  }
  
  LogOut(){
    localStorage.setItem('jwt', undefined);
    this.router.navigate(['/home']);
  }

  jwtIsUndefined() : boolean{
    return localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != "";
  }

  IsAnonymus()
  {
    if(localStorage.jwt == "undefined")
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  IsNormal(){
  
    if(this.role == "admin"){
      return false;
     }
     else if (localStorage.jwt != "undefined")
     {
       return true;
     }
     
  }

  IsAdmin(){
    if(this.role == "admin")
    {
      return true;
     }
     else{
      return false;
     }
  }
}
