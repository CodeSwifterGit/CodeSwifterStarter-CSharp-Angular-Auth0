import { AfterContentInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[autoFocus]'
})
export class AutofocusDirective implements AfterContentInit {

  @Input()
  autoFocus: boolean;

  public constructor(private el: ElementRef) {

  }

  ngAfterContentInit() {

    setTimeout(() => {

        this.el.nativeElement.focus();

      },
      500);

  }

}
