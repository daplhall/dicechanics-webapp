import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { PythonError } from './python-error';
@Injectable({
  providedIn: 'root',
})
export class PythonService {
  private http = inject(HttpClient);
  errMsg = signal<PythonError>(
    {
      status: 0,
      error: "",
    }
  );

  setErrorMsg(msg: PythonError) {
    this.errMsg.set(msg);
  }

  getErrorMsg() {
    return this.errMsg();
  }

  Execute(program: string): void {
    var retval = "";
    try {

      this.http.post(`${environment.pythonUrl}`, {
        program: program
      }).subscribe({
        next: (response) => {
          this.setErrorMsg({
            status: 0,
            error: "",
          })
        },
        error: (err) => {
          this.setErrorMsg({
            status: err.status,
            error: err.error,
          });
        }
      })
    } catch (error) {
      console.log(error)
    }
  }

}
