import { Component, OnInit } from '@angular/core';
import { EmployeeserviceService } from '../../employeeservice.service';
import { Employeecls } from '../../employeecls';
import { FormsModule, FormBuilder, FormGroup, NgModel } from '@angular/forms';
import { AlertService } from 'src/app/alert.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-employeedetails',
  templateUrl: './employeedetails.component.html',
  styleUrls: ['./employeedetails.component.css']
})


export class EmployeedetailsComponent implements OnInit {
  display=''; //default Variable
 
  editEmployee  : Employeecls;
  employeeList : Employeecls[ ];
  reactiveForm : FormGroup;
  constructor(
    private employeeService : EmployeeserviceService, 
    private formBuilder : FormBuilder ,
    private alertService : AlertService ,
    private router : Router
     
    
    ) { }

  ngOnInit() {
    this.CreateNewForm();
    this.GetAllEmployees();
  }

  GetAllEmployees()
  {
    this.employeeService.getAllEmployees().subscribe(
      (response: Employeecls[]) =>
      {
        this.employeeList = response;
      });
  }

  CreateNewForm()
  {
    this.reactiveForm = this.formBuilder.group({
    firstname: [''],
    lastname:[''],
    emailid: [''],
    contactno:[''],
    message:[''],
    password:[''],

    })
  }


  editEmployeeData(index: number)
  {
console.log(index);
this.editEmployee.contactno = this.employeeList[index].contactno;
this.editEmployee.emailid = this.employeeList[index].emailid;
this.editEmployee.firstname = this.employeeList[index].firstname;
this.editEmployee.id = this.employeeList[index].id;
this.editEmployee.lastname = this.employeeList[index].lastname;
this.editEmployee.message = this.employeeList[index].message;
this.editEmployee.password = this.employeeList[index].password;
console.log("Assigned value.............");
  }

  updateEmployee()
  {

  }

  addEmployee()
  {

    console.log("Add Employee Called.....")

    this.employeeService.saveAllEmployees(this.reactiveForm.value).subscribe((response : Employeecls)  => 
    {
     this.employeeList.push(response);

    // this.router.navigate['employeedetails']
    // },
    // e => { alert(e) }
    // );
  
    this.alertService.success("Recoed Saved sucessfully..",true)

    //this.display='none'; 
    
   this.reactiveForm.reset();

  });
}

}