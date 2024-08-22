import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CompanyService } from '../../services/company.service';
import { CompanyCreateModel } from '../../models/category/companyCreateModel';


@Component({
  selector: 'app-add-company-modal',
  templateUrl: './add-company-modal.component.html',
  styleUrls: ['./add-company-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddCompanyModalComponent {
  @Output() companyAdded = new EventEmitter<void>();
  companyForm: FormGroup;
  isVisible = false;

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,

  ) {
    this.companyForm = this.fb.group({
      name: ['', Validators.required],
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.companyForm.reset();
  }

  submit() {
    if (this.companyForm.valid) {
      const formValue = this.companyForm.value;
      const newCompany: CompanyCreateModel = {
        ...formValue
      };
      this.companyService.addCompany(newCompany).subscribe(
        () => {
          this.close();
          this.companyAdded.emit();
          this.companyForm.reset();
        },
        error => {
          console.error('Error adding Company:', error);
        }
      );
    }
  }
}

