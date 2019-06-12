import { BaseHttpService } from 'src/app/services/home-http.service';

export class ValuesHttpService extends BaseHttpService<string>{
    specificUrl = "/api/values";
}