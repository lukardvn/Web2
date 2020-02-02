import { Component, OnInit, OnChanges , ViewChild, ElementRef } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm } from '@angular/forms';
import { Kupac, Karta, PayPalPaymentDetails } from '../modeli';

declare let paypal: any;

@Component({
  selector: 'app-cenovnik',
  templateUrl: './cenovnik.component.html',
  styleUrls: ['./cenovnik.component.css']
})
export class CenovnikComponent implements OnInit {

  karte : string[] = ["regularna"];
  kupci : string[] = ["regularan"];

  kursEura : number = 0.00850;
  tipKarte : string;
  tipKupca : string;
  email : string;
  retVal : boolean;
  role : any;
  userType: string = "regularan";

  cenaTemp : number;
  cenaEur : number;
  cenaEura : string;
  kupljenaKartaID : number;
  kupljenaKartaKupljena : any;
  kupljenaKartaIstice : any;
  kupljenaKartaKarta : string;
  @ViewChild('divPaypal',{static: false}) divPaypal : ElementRef;


  paypalConfig = {
    env: 'sandbox',
    client: {
      sandbox: 'AYd0Uim6_hlWm4Xxhwc2FheQPv8IsxxVurGXhD-wWFH8nVrN9LdLDO3Y1B2yH0y3pvaLCa8ZePkU4D2j',
      production: '<your-production-key here>'
    },
    commit: true,
    locale: 'en_US',
    style: {
      size: 'medium',
      shape: 'pill',
      label: 'paypal',
      color: 'blue'
    },
    payment: (data, actions) => {
      return actions.payment.create({
        payment: {
          intent: 'sale',
           transactions: [
             { 
              amount: 
                { total: this.cenaEur.toFixed(2), currency: 'EUR' },
              item_list: {
                items:[{
                 name: this.tipKarte,
                 quantity: 1,
                 price: this.cenaEur.toFixed(2),
                 currency:'EUR'
                }]
              }
            }
           ]
        }
      });
    },
    onAuthorize: (data, actions) => {           
      return actions.payment.execute()
        .then((payPalPaymentDetails) =>  {
          var paymentDetailsObj = new PayPalPaymentDetails(payPalPaymentDetails);
          this.buyTicket(this.tipKarte,this.tipKupca,JSON.stringify(paymentDetailsObj));
        });
    },
    onError: (err) => {
      console.log(err);
    }
  };

  constructor(private http: AuthHttpService) { }
  
  ngOnInit() {
    if(localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != ""){
      let jwtData = localStorage.jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData)

      console.log('ONInit');
      this.karte  = ["regularna", "dnevna", "mesecna", "godisnja" ];
      this.kupci = ["regularan", "student", "penzioner"];

       this.role = decodedJwtData.nameid;
       console.log(this.role);
       this.http.GetUserType().subscribe( tip => {
        this.userType = tip;
        console.log('usertype iz cenovnika',this.userType);

        if(this.userType == "regularan") {
          this.kupci = ["regularan"];
        }else if (this.userType == "student") {
          this.kupci = ["student"];
        }else if (this.userType == "penzioner") {
          this.kupci = ["penzioner"];
        } else {
          this.kupci = ["regularan"];
          this.karte = ["regularna"];
        }
        
        err=> console.log(err);
       });
    }
  }

  buyTicket(karta: string, korisnik: string, json: any) {
    this.http.BuyTicket(karta,this.userType,json).subscribe( karta => {
      this.kupljenaKartaID = karta.TicketID;
      this.kupljenaKartaKupljena = Date.parse(karta.BoughtAt);
      this.kupljenaKartaIstice = karta.Expires.slice(0,16).replace('T',' ');
      this.kupljenaKartaKarta = karta.TicketType;

      err=> console.log(err);
    });

  }

  proveriCenu() {
    this.http.GetCenaKarte(this.tipKarte, this.userType).subscribe((cena)=>{
      this.cenaTemp = cena;
      this.cenaEur = this.cenaTemp * this.kursEura;
      this.cenaEura = this.cenaEur.toFixed(2);
      if(!document.querySelector(".paypal-button")){
        paypal.Button.render(this.paypalConfig, '#paypal-checkout-btn');
      }
      if(this.divPaypal.nativeElement.hidden == true)
        this.divPaypal.nativeElement.hidden = false;
     
      err => console.log(err);
    });
    console.log(this.cenaTemp);
  }

  kupiKartu(email : string, form : NgForm) {
    console.log(email);
    if(this.IsAnonymus())
    {
    this.http.GetKupiKartu(email).subscribe((cena)=>{
      this.retVal = cena;
     
      err => console.log(err);
    });
    console.log(this.retVal);
  }
  }

  kupiKartu2(email : string, form : NgForm) {
    console.log(email);
    if(this.IsNormal())
    {
    this.http.KupiKartu(email, this.tipKarte).subscribe((cena)=>{
      this.retVal = cena;
     
      err => console.log(err);
    });
    console.log(this.retVal);
  }
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
}
