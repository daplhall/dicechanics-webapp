import {Component, Input} from '@angular/core';

import {PythonResult} from '../python-result';

@Component({
  selector: 'app-results',
  imports: [],
  templateUrl: './results.html',
  styleUrl: './results.css',
})
export class Results {
  @Input() results: PythonResult = {result: ''};
}
