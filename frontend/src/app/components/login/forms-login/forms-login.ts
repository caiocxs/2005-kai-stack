import { Component, input, output } from '@angular/core';
import { RandomIcon } from '../../general/random-icon/random-icon';

@Component({
  selector: 'app-forms-login',
  imports: [RandomIcon],
  templateUrl: './forms-login.html',
  styleUrl: './forms-login.scss',
})
export class FormsLogin {
  isEmailLogin = input.required<boolean>();
  toggleMode = output<void>();

  identifier = '';
  password = '';
}
