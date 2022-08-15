import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-roles-madal',
  templateUrl: './roles-madal.component.html',
  styleUrls: ['./roles-madal.component.css']
})
export class RolesMadalComponent implements OnInit {

  @Input() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];
  constructor(public bsModalRef:BsModalRef) { }

  ngOnInit(): void {
  }
  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
  
}
