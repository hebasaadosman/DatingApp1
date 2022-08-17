import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { observable, Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef: BsModalRef;

  constructor(private modelService: BsModalService) { }

  confirm(title = "Confirmation", message = 'Are you Sure You want to do this ?'
    , btnOktext = "OK", btnCanceltext = 'Cancel'): Observable<boolean> {
    const config = {
      initialState: {
        title,
        message,
        btnOktext,
        btnCanceltext
      }
    }
    this.bsModelRef = this.modelService.show(ConfirmDialogComponent, config)
    return new Observable<boolean>(this.getResult());
  }
  private getResult() {
    return (observable) => {
      const subscription = this.bsModelRef.onHidden.subscribe({
        next: () => {
          observable.next(this.bsModelRef.content.result);
          observable.complete()
        }
      });
      return {
        unsubscribe() {
          subscription.unsubscribe();
        }
      }
    }
  }
}
