import { Injectable } from '@angular/core';
import { HttpClient } from  '@angular/common/http'
import { Observable } from 'rxjs';
import { Employeecls } from './employeecls';



@Injectable({
  providedIn: 'root'
})
export class EmployeeserviceService {

  constructor(private httpClient : HttpClient) { 
  }

  getAllEmployees () : Observable<Employeecls[]>{
  return this.httpClient.get<Employeecls[]>("http://localhost:56882/api/getemployee");
   
  };

  saveAllEmployees (empl : Employeecls)
  {
    return this.httpClient.post("http://localhost:56882/api/InsertEmployee",empl)
  }

}
