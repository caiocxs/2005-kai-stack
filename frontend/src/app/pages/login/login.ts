import { Component, signal } from '@angular/core';
import { FormsLogin } from '../../components/login/forms-login/forms-login';

@Component({
  selector: 'app-login',
  imports: [FormsLogin],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  protected isEmailLogin = signal(false);

  onToggleMode() {
    this.isEmailLogin.update((v) => !v);
  }
}
