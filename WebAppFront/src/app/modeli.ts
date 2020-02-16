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
    Id : string
    Name: string
    Surname: string
    Username: string
    Password: string
    ConfirmPassword: string
    Email: string
    Date: string
    Tip : string
    Adress : string
    Files : string
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

export class PayPalPaymentDetails {
    Id : string
    Intent : string
    State : string
    Cart : string
    CreateTime : string
    PayerPaymentMethod : string
    PayerStatus : string
    PayerEmail : string
    PayerFirstName : string
    PayerMiddleName : string
    PayerLastName : string
    PayerId : string
    PayerCountryCode : string
    ShippingAddressRecipientName : string
    ShippingAddressStreet : string
    ShippingAddressCity : string
    ShippingAddressState : string
    ShippingAddressPostalCode :number
    ShippingAddressCountryCode : string
    TransactionsAmountTotal : number
    TransactionsAmountCurrency : string
    TransactionsDetailsSubtotal : number
    TransactionsDetailsShipping : number
    TransactionsDetailsHandlingFee : number
    TransactionsDetailsInsurance : number
    TransactionsShippingDiscount : number
    TransactionsItemListItemsName : string
    TransactionsItemListItemsPrice : number
    TransactionsItemListItemsCurrencty : string
    TransactionsItemListItemsQuantity : number
    TransactionsItemListItemsTax : number

    constructor(jsonData : any) {
        this.Id = jsonData['id'];
        this.Intent = jsonData['intent'];
        this.State = jsonData['state'];
        this.Cart = jsonData['cart'];
        this.CreateTime = jsonData['create_time'];
        this.PayerPaymentMethod = jsonData['payer']['payment_method'];
        this.PayerStatus = jsonData['payer']['status'];
        this.PayerEmail = jsonData['payer']['payer_info']['email'];
        this.PayerFirstName = jsonData['payer']['payer_info']['first_name'];
        this.PayerMiddleName = jsonData['payer']['payer_info']['middle_name'];
        this.PayerLastName = jsonData['payer']['payer_info']['last_name'];
        this.PayerId = jsonData['payer']['payer_info']['payer_id'];
        this.PayerCountryCode = jsonData['payer']['payer_info']['country_code'];
        this.ShippingAddressRecipientName = jsonData['payer']['payer_info']['shipping_address']['recipient_name'];
        this.ShippingAddressStreet = jsonData['payer']['payer_info']['shipping_address']['line1'];
        this.ShippingAddressCity = jsonData['payer']['payer_info']['shipping_address']['city'];
        this.ShippingAddressState = jsonData['payer']['payer_info']['shipping_address']['state'];
        this.ShippingAddressCountryCode = jsonData['payer']['payer_info']['shipping_address']['country_code'];
        this.ShippingAddressPostalCode = jsonData['payer']['payer_info']['shipping_address']['postal_code'];
        this.TransactionsAmountTotal = jsonData['transactions']['0']['amount']['total'];
        this.TransactionsAmountCurrency = jsonData['transactions']['0']['amount']['currency'];
        this.TransactionsDetailsSubtotal = jsonData['transactions']['0']['amount']['details']['subtotal'];
        this.TransactionsDetailsShipping = jsonData['transactions']['0']['amount']['details']['shipping'];
        this.TransactionsDetailsHandlingFee = jsonData['transactions']['0']['amount']['details']['handling_fee'];
        this.TransactionsDetailsInsurance = jsonData['transactions']['0']['amount']['details']['insurance'];
        this.TransactionsDetailsShipping = jsonData['transactions']['0']['amount']['details']['shipping_discount'];
        this.TransactionsItemListItemsName = jsonData['transactions']['0']['item_list']['items']['0']['name'];
        this.TransactionsItemListItemsPrice = jsonData['transactions']['0']['item_list']['items']['0']['price'];
        this.TransactionsItemListItemsCurrencty = jsonData['transactions']['0']['item_list']['items']['0']['currency'];
        this.TransactionsItemListItemsQuantity = jsonData['transactions']['0']['item_list']['items']['0']['quantity'];
        this.TransactionsItemListItemsTax = jsonData['transactions']['0']['item_list']['items']['0']['tax'];
    }
}