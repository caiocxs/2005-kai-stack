import { Component } from '@angular/core';
import { IconSelector } from '../../atoms/icon-selector/icon-selector';
import { ButtonBarNso } from '../../atoms/button-bar-nso/button-bar-nso';

@Component({
  selector: 'app-footer-home',
  imports: [IconSelector, ButtonBarNso],
  templateUrl: './footer-home.html',
  styleUrl: './footer-home.scss',
})
export class FooterHome {}
