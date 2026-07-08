import { Component, Input } from '@angular/core';
import { IconSelector } from '../icon-selector/icon-selector';

@Component({
  selector: 'li[app-route-bar-nso]',
  imports: [IconSelector],
  templateUrl: './route-bar-nso.html',
  styleUrl: './route-bar-nso.scss',
  host: {
    class: 'btn btn-bar-nso',
    '(mousedown)': 'isPressed = true',
    '(mouseup)': 'isPressed = false',
    '(mouseleave)': 'isPressed = false',
    '[class.is-active]': 'isPressed',
    '(touchstart)': 'isPressed = true',
    '(touchend)': 'isPressed = false',
  },
})
export class RouteBarNso {
  @Input() icon: string = 'home-mini';
  @Input() route: string = 'home';
  isPressed = false;
}
