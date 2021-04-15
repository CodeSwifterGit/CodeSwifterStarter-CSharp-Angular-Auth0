import { Directive } from '@angular/core';

@Directive({
  selector: '[allow-tab]',
  host: {
    '(keydown)': 'onKeyUp($event)'
  }
})
export class AllowTabDirective {
  onKeyUp($event) {
    const el = $event.currentTarget;

    if ($event.keyCode === 9) {
      const val = el.value;
      const start = el.selectionStart;
      const end = el.selectionEnd;

      el.value = val.substring(0, start) + '\t' + val.substring(end);
      el.selectionStart = el.selectionEnd = start + 1;

      return false;
    }
  }
}
