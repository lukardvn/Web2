import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [AuthHttpService]
})
export class HomeComponent implements OnInit {

  constructor(private service: AuthHttpService, private router: Router) { }

  role : any;

  ngOnInit() {
    if(localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != ""){
      let jwtData = localStorage.jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData)


       this.role = decodedJwtData.nameid
       console.log(decodedJwtData);
    
    }
    this.IsAdmin();
  }

  IsAdmin(){
    if(this.role == "admin"){
      this.router.navigate(["/adminview"]);
      console.log("admin");
     }
     else{
      this.router.navigate(["/home"]);
      console.log("obican");
     }
  }

  LogOut(){
    
    localStorage.setItem('jwt', undefined);
    this.router.navigate(['/home']);


}

}
