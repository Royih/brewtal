import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'splitInLines'
})
export class SplitInLinesPipe implements PipeTransform {

  transform(input: string): string {
    input = input || '';
    const arr = input.split('.');
    let out = '<ul>';
    for (let i = 0; i < arr.length; i++) {
      if (arr[i].trim() !== '') {
        out = out + '<li>' + arr[i] + '</li>';
      }
    }
    return out + '<ul>';
  }
}
