import { HttpClient } from '@angular/common/http';
import { afterNextRender, Component, inject, signal } from '@angular/core';

@Component({
  selector: 'app-random-icon',
  imports: [],
  templateUrl: './random-icon.html',
  styleUrl: './random-icon.scss',
})
export class RandomIcon {
  private http = inject(HttpClient);
  icon = signal<string | null>(null);

  constructor() {
    afterNextRender(() => {
      this.http.get<string[]>('assets/icons-manifest.json').subscribe((icons) => {
        this.icon.set(icons[Math.floor(Math.random() * icons.length)]);
      });
    });
  }
}
