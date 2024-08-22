import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company.service';
import { CompanyReturnModel } from '../../models/company/companyReturnModel';
import { CompanyCreateModel } from '../../models/category/companyCreateModel';

@Component({
  selector: 'app-edit-company-modal',
  templateUrl: './edit-company-modal.component.html',
  styleUrls: ['./edit-company-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class EditCompanyModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  companyForm: FormGroup;
  company!: CompanyReturnModel;

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService
  ) {
    this.companyForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  open(company: CompanyReturnModel) {
    this.isVisible = true;
    this.company = company;
    this.companyForm.patchValue({
      name: company.name
    });
  }

  close() {
    this.isVisible = false;
    this.companyForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.companyForm.valid) {
      const formValue = this.companyForm.value;
      const updatedCompany: CompanyCreateModel = {
        name: formValue.name
      };
      this.companyService.updateCompany(this.company.id, updatedCompany).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error updating company:', error);
        }
      );
    }
  }
}
