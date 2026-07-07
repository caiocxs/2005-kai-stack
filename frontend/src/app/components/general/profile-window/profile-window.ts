import { Component, Input } from '@angular/core';
import { WindowNso } from '../../atoms/window-nso/window-nso';

@Component({
  selector: 'app-profile-window',
  imports: [WindowNso],
  templateUrl: './profile-window.html',
  styleUrl: './profile-window.scss',
})
export class ProfileWindow {
  @Input() title = 'Hi, I am KANANON! (^o^)';
  @Input() content = [
    'Likes: cats, art, math, computer science and idk..\n',
    'Dislikes: isnotreal >///<',
  ];
}
