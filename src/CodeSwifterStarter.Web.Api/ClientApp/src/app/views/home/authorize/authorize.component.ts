import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AuthService } from 'app/services/auth/auth.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-authorize',
  templateUrl: './authorize.component.html',
  styleUrls: ['./authorize.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthorizeComponent implements OnInit {

  private _destroy = new Subject<boolean>();

  constructor(private readonly auth: AuthService) {

  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._destroy.next(true);
    this._destroy.complete();
  }
}
