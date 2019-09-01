import { Component, OnInit, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { User } from '../modeli';
import { RegUser } from '../modeli';
import { AuthHttpService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { FormArray } from '@angular/forms';
import { Validators } from '@angular/forms';

@Component({
  selector: 'app-registracija',
  templateUrl: './registracija.component.html',
  styleUrls: ['./registracija.component.css']
})
export class RegistracijaComponent implements OnInit {

  rf = this.fb.group({
    name: ['', Validators.required],
    surname: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
    adress: ['', Validators.required],
    email: ['', Validators.email],
    date: ['', Validators.required],
    tip: ['', Validators.required]
  });

  constructor(private http: AuthHttpService, private router: Router, private fb: FormBuilder) { }

  isLogin : boolean = false;
  tip : string;
  poruka : boolean = false;
  tipovi: string[] = [ "Obican", "Student", "Penzioner" ];

  ngOnInit() {
  }

  login(user: User, form: NgForm){
    let l = this.http.logIn(user.username, user.password);
     form.reset();
    console.log(l);
    if(l){
      if(user.username == "admin@yahoo")
      {
        this.router.navigate(["/adminview"]);
      }
      else
      {
        this.router.navigate(["/home"]);
      }
    }
  }
  
  regUse() {
    
    let rus: RegUser = this.rf.value;
    console.log(this.rf.value);
     this.http.reg(rus).subscribe((odg)=> {
      this.poruka = odg;
      if (this.poruka)
      {
        alert("registrovali ste se!");
      }

    });
   
  }

}
