import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { PythonService } from '../python-service';
import { ErrorMessage } from "../error-message/error-message";
@Component({
  selector: 'app-python-program',
  imports: [ReactiveFormsModule, ErrorMessage],
  templateUrl: './python-program.html',
  styleUrls: ['./python-program.css', '../../styles.css'],
})
export class PythonProgram {
  private console = inject(PythonService)
  applyForm = new FormGroup({
    program: new FormControl('')
  });


  submitProgram() {
    this.console.Execute(this.applyForm.value.program ?? '');
  }

  getError() {
    return this.console.getErrorMsg()
  }

}
