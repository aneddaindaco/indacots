import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {

  @Output() confirm = new EventEmitter<boolean>();
  @Input() issueNo: number | null = null;
  
  constructor() { }

  ngOnInit(): void {
  }

  agree(): void{
    this.confirm.emit(true);
    this.issueNo = null;
  }
  
  disAgree(){
    this.confirm.emit(false);
    this.issueNo = null;
  }

}
