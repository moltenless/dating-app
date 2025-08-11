import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-set-main-image',
  imports: [],
  templateUrl: './set-main-image.html',
  styleUrl: './set-main-image.css'
})
export class SetMainImage {
  disabled = input<boolean>();
  selected = input<boolean>();
  clickEvent = output<Event>();

  onClick(event: Event){
    this.clickEvent.emit(event)
  }
}
