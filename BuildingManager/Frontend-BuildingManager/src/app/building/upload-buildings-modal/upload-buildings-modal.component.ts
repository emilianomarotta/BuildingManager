import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ImporterService } from '../../services/importer.service';

@Component({
  selector: 'app-upload-buildings-modal',
  templateUrl: './upload-buildings-modal.component.html',
  styleUrls: ['./upload-buildings-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class UploadBuildingsModalComponent {
  @Output() fileUploaded = new EventEmitter<void>();
  uploadForm: FormGroup;
  isVisible = false;
  importers: string[] = [];
  fileName: string | null = null;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private importerService: ImporterService
  ) {
    this.uploadForm = this.fb.group({
      importer: ['', Validators.required],
      file: [null, Validators.required]
    });
  }

  open() {
    this.isVisible = true;
    this.loadImporters();
  }

  close() {
    this.isVisible = false;
    this.uploadForm.reset();
    this.fileName = null;
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.uploadForm.patchValue({
        file: file
      });
      this.fileName = file.name;
    }
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();

    if (event.dataTransfer?.files.length) {
      const file = event.dataTransfer.files[0];
      this.uploadForm.patchValue({
        file: file
      });
      this.fileName = file.name;
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  submit() {
    if (this.uploadForm.valid && this.fileName) {
      const formValue = this.uploadForm.value;
      const importerName = formValue.importer;
      const file = formValue.file;

      this.importerService.uploadFile(file).subscribe(
        (response: any) => {
          const filePath = `./Importers/${file.name}`;
          this.importerService.importFile(importerName, filePath).subscribe(
            () => {
              this.close();
              this.fileUploaded.emit();
            },
            error => {
              console.error('Error importing file:', error);
            }
          );
        },
        error => {
          console.error('Error uploading file:', error);
        }
      );
    } else {
      console.warn('Form is invalid');
    }
  }

  loadImporters() {
    this.importerService.getImporters().subscribe(
      (data: string[]) => {
        this.importers = data;
      },
      error => {
        console.error('Error loading importers:', error);
      }
    );
  }
}
