import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PythonProgram } from './python-program';

describe('PythonProgram', () => {
  let component: PythonProgram;
  let fixture: ComponentFixture<PythonProgram>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PythonProgram]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PythonProgram);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
