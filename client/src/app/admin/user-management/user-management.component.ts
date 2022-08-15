import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesMadalComponent } from 'src/app/modals/roles-madal/roles-madal.component';
import { User } from 'src/app/_models/User';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;
  bsModalRef: BsModalRef;
  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }
  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe({
      next: users => {
        this.users = users;
      }
    })
  }
  openRolesModal(user: User) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }

    };
    this.bsModalRef = this.modalService.show(RolesMadalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe({
      next: values => {
        const rolesToUpdate = {
          roles: [...values.filter(el => el.checked === true).map(el => el.name)]
        };
        if (rolesToUpdate) {
          this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe({
            next: () => {
              user.roles = [...rolesToUpdate.roles];
            }
          })
        }
      }
    });
  }
  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const avaliableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'Moderator', value: 'Moderator' },
      { name: 'Member', value: 'Member' },
    ];
    avaliableRoles.forEach(role => {
      let isMatch = false;
      for (let userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }
}


