import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { EmployeeserviceService } from '../employeeservice.service';
import { Employeecls } from '../employeecls';

@Component({
  selector: 'app-addemployee',
  templateUrl: './addemployee.component.html',
  styleUrls: ['./addemployee.component.css']
})
export class AddemployeeComponent implements OnInit {

reactiveForm : FormGroup;
employeeList : Employeecls[];

  constructor(private formBuilder : FormBuilder, private emplyeeService : EmployeeserviceService ) { }

  ngOnInit() {

    this.CreateForm();
  }

  CreateForm()
  {
    this.reactiveForm = this.formBuilder.group({
      firstname : [''],
      lastname : [''],
      emailid : [''],
      message : [''],
      contactno : [''],
      password : [''],
    })
  }

  addEmployee()
  {
    //console.log(reactiveForm.firstname);
    console.log(this.reactiveForm.value.firstname);
  }

  getAllEmployee()
  {
    this.emplyeeService.getAllEmployees().subscribe(

(response : Employeecls[]) =>
{
this.employeeList = response;
}

    )
  }

}
