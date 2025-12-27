import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PythonProgram } from "../python-program/python-program";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PythonProgram],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Client');
}
