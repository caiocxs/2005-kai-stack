import { Component, Input } from '@angular/core';
import { IconSelector } from '../../atoms/icon-selector/icon-selector';

@Component({
  selector: 'app-window-nso',
  imports: [IconSelector],
  templateUrl: './window-nso.html',
  styleUrl: './window-nso.scss',
})
export class WindowNso {
  @Input({ required: true }) name = 'home';
  @Input({ required: true }) iconName = 'home';
}
