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
}

export class raspored {
    polasci : string

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