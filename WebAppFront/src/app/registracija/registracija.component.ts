import { Component, OnInit, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { User } from '../modeli';
import { RegUser } from '../modeli';
import { AuthHttpService } from '../services/auth.service';
import { Router } from '@angular/router';
//import { FormBuilder } from '@angular/forms';
import { FormArray } from '@angular/forms';
//import { Validators } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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
    type: ['', Validators.required]
  });

  registerForm: FormGroup;
  file: File;
  imgURL: any;

  constructor(private http: AuthHttpService, private router: Router, private fb: FormBuilder) { }

  isLogin : boolean = false;
  tip : string;
  poruka : boolean = false;
  tipovi: string[] = [ "Obican", "Student", "Penzioner" ];
  //selectedValue: string;

  ngOnInit() {

    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
      adress: ['', Validators.required],
      email: ['', Validators.email],
      date: ['', Validators.required],
      type: ['', Validators.required]
    });
  }

  get f() { return this.rf.controls; }

  login(user: User, form: NgForm){
    let l = this.http.logIn(user.username, user.password);
     form.reset();
    console.log(l);
    if(l){
      if(user.username == "admin@yahoo")
      {
        this.router.navigate(["/adminview"]);
      }
      else if(user.username =="controller@yahoo")
      {
        this.router.navigate(["/controllerview"]);
      }
      else
      {
        this.router.navigate(["/home"]);
      }
    }
  }

  onFileChanged(event) {
    this.file = event.target.files[0];

    var reader = new FileReader();
    reader.readAsDataURL(this.file); 
    reader.onload = (_event) => { 
      this.imgURL = reader.result; 
    }
  }
  selectChange(){
    this.tip = this.registerForm.get('type').value;
    console.log(this.f.type.value);
  }
  
  regUse() {
    
    //let rus: RegUser = this.rf.value;
    //console.log(this.registerForm.value);
    //console.log(this.f.registerForm.value);
    let rus = new RegUser();
    rus.email = this.f.email.value;
    rus.username = this.f.username.value;
    rus.password = this.f.password.value;
    rus.confirmPassword = this.f.confirmPassword.value;
    rus.adress = this.f.adress.value;
    rus.date = this.f.date.value;
    rus.name = this.f.name.value;
    rus.surname = this.f.surname.value;
    rus.tip = this.f.type.value;
    console.log(this.f.type.value);
    //this.file = this.f.files.value;

    console.log(rus);

    console.log(this.rf.value);
    //console.log(this.f.rf.value);
     this.http.reg(rus,this.file).subscribe((odg)=> {
      this.poruka = odg;
      if (this.poruka)
      {
        alert("registrovali ste se!");
      }

    });
   
  }

}
