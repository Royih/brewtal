import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';

export class XDataFetcher<T> {


  constructor(private http: HttpClient, private url: string) {

  }


  private data: T[];
  getOptions(): Observable<T[]> {
    if (!this.data) {
      console.log("Fetching..");
      return this.http.get<T[]>("user/list").pipe(tap((res) => this.data = res));
    }
    return of(this.data);
  }



}
