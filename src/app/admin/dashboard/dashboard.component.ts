import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

Name : string;
Designation : string;
MobileNo : number;
Salary:number;
DeveloperList : string [] ;

DropdownList : string [];

EmployeeList : {ID: number, EmpName: string,  TypeN: string,  Scientific: string}[];

ngOnInit() {

this.Name = "Niraj Bisen";
this.Designation = "Developer";
this.MobileNo = 4545554545;
this.Salary = 10000;

this.DeveloperList = ["A","B","C","D"];
this.DropdownList = ["Item1", "Item2" , "Item3"];


this.EmployeeList = [
  {ID: 1, EmpName: 'Eurasian Collared-Dove',  TypeN: 'Dove',  Scientific: 'Streptopelia'},
  {ID: 2, EmpName: 'Bald Eagle',  TypeN: 'Hawk',  Scientific: 'Haliaeetus leucocephalus' },
  {ID: 3, EmpName: 'Coopers Hawk',  TypeN: 'Hawk',  Scientific: 'Accipiter cooperii' }
];



}
  
  DDLChange($event)
  {
    console.log("ddl change called");
    console.log($event.target.value)
  }
  


}
