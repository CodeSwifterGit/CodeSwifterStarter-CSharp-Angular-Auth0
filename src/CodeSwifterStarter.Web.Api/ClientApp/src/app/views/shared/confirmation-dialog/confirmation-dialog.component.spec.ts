/* tslint:disable:no-unused-variable */
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from './confirmation-dialog.component';

describe('ConfirmationDialogComponent',
  () => {
    let component: ConfirmationDialogComponent;
    let fixture: ComponentFixture<ConfirmationDialogComponent>;

    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
          providers: [
            { provide: MAT_DIALOG_DATA, useValue: {} },
            { provide: MatDialogRef, useValue: {} }
          ],
          declarations: [ConfirmationDialogComponent]
        })
        .compileComponents();
    }));

    beforeEach(() => {
      fixture = TestBed.createComponent(ConfirmationDialogComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    });

    it('should create',
      () => {
        expect(component).toBeTruthy();
      });
  });
