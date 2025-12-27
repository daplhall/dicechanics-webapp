import { Component, Input } from '@angular/core';
import { PythonError } from '../python-error';

@Component({
  selector: 'app-error-message',
  imports: [],
  templateUrl: './error-message.html',
  styleUrl: './error-message.css',
})
export class ErrorMessage {
  @Input() errMsg: PythonError = { status: 0, error: '' };
}
