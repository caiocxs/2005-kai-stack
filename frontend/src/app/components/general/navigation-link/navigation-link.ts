import { Component, Input } from '@angular/core';
import { IconSelector } from '../../atoms/icon-selector/icon-selector';
import { LowerCasePipe } from '@angular/common';

@Component({
  selector: 'app-navigation-link',
  imports: [IconSelector, LowerCasePipe],
  templateUrl: './navigation-link.html',
  styleUrl: './navigation-link.scss',
  host: {
    class: 'btn',
  },
})
export class NavigationLink {
  @Input({ required: true }) name = 'Link';
  @Input() icon?: string;
}
