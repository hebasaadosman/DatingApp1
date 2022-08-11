import { Component, OnInit } from '@angular/core';
import { map, Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/User';
import { UserParams } from 'src/app/_models/userParams';
import { Pagination } from 'src/app/_modules/pagination';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  userParams: UserParams;
  user: User;

  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }];
  
  constructor(private memberService: MembersService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        this.user = user;
        this.userParams = new UserParams(user);
      }
    })
  }

  ngOnInit(): void {
    this, this.loadMembers();
  }
  loadMembers() {
    this, this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    })
  }
  resetFilters() {
    this.userParams = new UserParams(this.user);
    this.loadMembers();
  }
  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }
}
