import { Component } from '@angular/core';
import { IconSelector } from '../../atoms/icon-selector/icon-selector';
import { ButtonBarNso } from '../../atoms/button-bar-nso/button-bar-nso';
import { PipeBarNso } from '../../atoms/pipe-bar-nso/pipe-bar-nso';
import { ClockNso } from '../../atoms/clock-nso/clock-nso';
import { CalendarNso } from '../../atoms/calendar-nso/calendar-nso';

@Component({
  selector: 'app-footer-home',
  imports: [IconSelector, ButtonBarNso, PipeBarNso, ClockNso, CalendarNso],
  templateUrl: './footer-home.html',
  styleUrl: './footer-home.scss',
})
export class FooterHome {}
