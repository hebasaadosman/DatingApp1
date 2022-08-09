import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { FileUploader } from "ng2-file-upload";
import { environment } from 'src/environments/environment';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs';
import { MembersService } from 'src/app/_services/members.service';
import { Photo } from 'src/app/_models/Photo';


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;
  uploader: FileUploader;
  hasBaseDropzonOver = false;
  baseUrl = environment.apiUrl;
  user: User;

  constructor(private accuntService: AccountService, private memberService: MembersService) {
    this.accuntService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        this.user = user;
      }
    })
  }
  fileOverBase(e: any) {
    this.hasBaseDropzonOver = e;
  }
  ngOnInit(): void {
    this.initializeUploader();
  }
  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe({
      next: () => {
        this.user.photoUrl = photo.url;
        this.accuntService.setCurrentUser(this.user);
        this.member.photoUrl = photo.url;
        this.member.photos.forEach(p => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        })
      }
    })
  }
  deletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe({
      next: () => {
        this.member.photos = this.member.photos.filter(x => x.id !== photoId);
      }
    })
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + "users/add-photo",
      authToken: "Bearer " + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
      }
    }
  }

}
