import { Component, computed, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'app-icon-selector',
  imports: [],
  templateUrl: './icon-selector.html',
  styleUrl: './icon-selector.scss',
})
export class IconSelector {
  @Input({ required: true }) name = 'home';
  @Input() color?: string;
  @Input() strokeWidth = 2;
  @Input() size?: number;

  @HostBinding('style.height.px')
  get hostWidth() {
    return this.size;
  }

  @HostBinding('style.height.px')
  get hostHeight() {
    return this.size;
  }

  iconViewBox = computed(() => {
    switch (this.name) {
      case 'home':
        return '0 0 15 13';
      case 'profile':
        return '0 0 12 11';
      case 'redirect':
        return '0 0 44 44';
      case 'home-route':
        return '0 0 20 19';
      default:
        return '0 0 15 15';
    }
  });
}
