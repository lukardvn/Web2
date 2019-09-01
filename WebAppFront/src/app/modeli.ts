export class Osoba{
    name: string
    surname: string
    
}

export class User{
    username: string
    password: string
    // grant_type: string
}

export class RegUser{
    name: string
    surname: string
    username: string
    password: string
    confirmPassword: string
    email: string
    date: string
    tip : string
    adress : string

}

export class CenovnikEdit
{
    ID : number;
    CenaRegularna: number;
    CenaDnevna: number;
    CenaMesecna: number;
    CenaGodisnja: number;
}

export class RegUser2{
    Name: string
    Surname: string
    Username: string
    Password: string
    ConfirmPassword: string
    Email: string
    Date: string
    Tip : string
    Adress : string

}

export class Sifra{
    OldPassword: string
    NewPassword: string
    ConfirmPassword: string
    

}

export class Stanica{
    naziv: string
    adresa: string
    x: number
    y: number
}

export class StanicaUpdate{
    ID : number
    Naziv: string
    Adresa: string
    X: number
    Y: number
}

export class StaniceNaLiniji{
    transportLineID: string
    stationID: number
}

export class raspored {
    timeTable : string
    transportLineID : string

}

export class rasporedUpdate {
    ID : number
    TimeTable : string
    TransportLineID : string

}

export class linijica {
    FromTo : string
    TransportLineID : string

}
export class linja {

    linije : number[]
}
export class dani{
    id : number
    dan : string
}

export class Karta{
    id : number
    tip : string
    cena : number
}

export class Kupac {
    id : number
    tip : string
    koeficijent : number
}