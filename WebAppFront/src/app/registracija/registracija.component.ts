import { Component, OnInit, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { User } from '../modeli';
import { AuthHttpService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registracija',
  templateUrl: './registracija.component.html',
  styleUrls: ['./registracija.component.css']
})
export class RegistracijaComponent implements OnInit {

  constructor(private http: AuthHttpService, private router: Router) { }

  isLogin : boolean = false;

  ngOnInit() {
  }

  login(user: User, form: NgForm){
    let l = this.http.logIn(user.username, user.password);
    // form.reset();
    console.log(l);
    if(l){
      console.log("pusi ga ");
      this.router.navigate(["/adminview"]);
    }
  }

}
