import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ReactiveFormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Client');
  applyForm = new FormGroup({
    program: new FormControl('')
  });

  // Testing
  url = "http://localhost:8080/api/Python"

  async getExecutedProgram(program: string): Promise<string> {
    /*
    const data = await fetch(`${this.url}?program=${program}`);
    return await data.json() ?? '';
    */
    const response = await fetch(`${this.url}`, {
      method: "POST",
      body: JSON.stringify({ program: program }),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      // …
    });
    return await response.json() ?? '';
  }


  submitProgram() {
    this.getExecutedProgram(this.applyForm.value.program ?? '');
    console.log(this.applyForm.value.program ?? '');
  }
}
