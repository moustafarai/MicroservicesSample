import { Component, OnInit } from '@angular/core';
import { Form } from '@angular/forms';

import { MatTableDataSource } from '@angular/material/table';

import { Note } from './note';
import { NotesService } from './notes.service';
import { LoadingIndicatorService } from '../core/loading-indicator.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent implements OnInit {
  constructor(private notesService: NotesService, private loadingIndicatorService: LoadingIndicatorService) { }

  dataSource: MatTableDataSource<Note>;
  displayedColumns = ['text', 'author', 'updated'];
  newNote = new Note();

  ngOnInit(): void {
    this.reloadNotes();
  }

  reloadNotes() {
    this.loadingIndicatorService.show();
    this.notesService.getNotes().subscribe(notes => {
      this.dataSource = new MatTableDataSource(notes);
      this.loadingIndicatorService.hide();
    });
  }

  addNote() {
    this.loadingIndicatorService.show();
    this.notesService.addNote(this.newNote).subscribe(() => {
      this.newNote = new Note();
      this.reloadNotes();
    });
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
