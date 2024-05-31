import { Component } from '@angular/core';
import * as XLSX from 'xlsx';
import * as flatten from 'flat';
import { saveAs } from 'file-saver';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ExceltoJson';
  flatjson: any = null;
  //語言數量
  langlist: any[] = [];
  langjson: any = {};

  //#region ExcelToJson相關
  onFileExcelChange(ev: any) {
    let workBook: any = null;
    let jsonData = null;
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = () => {
      const data = reader.result;
      //讀excel檔
      workBook = XLSX.read(data, { type: 'binary' });
      jsonData = workBook.SheetNames.reduce((initial: any, name: any) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      let Sheet1jsondata = jsonData['Sheet1'];
      //console.log(Sheet1jsondata);
      //取得語言的數量
      this.langlist = this.getLanguagelist(Sheet1jsondata);
      //console.log(this.langlist);
      this.langlist.forEach(lang => {
        let keyvalue: any = {};
        Sheet1jsondata.forEach((element: any) => {
          let keyname = Object.getOwnPropertyNames(element)[1];
          let key = element[keyname];
          let value = element[lang];
          //console.log(`key=${key},value=${value}`);
          keyvalue[key] = value;
        });
        this.langjson[lang] = keyvalue;
      });
      //console.log(this.langjson);
      //儲存各語言的json檔
      this.saveLangJson();

    };
    reader.readAsBinaryString(file);

  }
  private saveLangJson() {
    this.langlist.forEach(lang => {
      let jsonlang = flatten.unflatten(this.langjson[lang], { delimiter: '/' })
      //console.log(jsonlang);
      let jsonse = JSON.stringify(jsonlang);
      let blob = new Blob([jsonse], { type: "application/json" });
      saveAs(blob, `${lang}.json`);

    });
  }
  //取得語言的數量
  getLanguagelist(Sheet1jsondata: any) {
    let langlist: any[] = [];
    Sheet1jsondata.forEach((element: any) => {
      let propnames = Object.getOwnPropertyNames(element);
      for (let index = 3; index < propnames.length; index++) {
        const lang = propnames[index];
        if (!langlist.includes(lang)) {
          langlist.push(lang);
        }
      }
    });
    return langlist;
  }
  //#endregion //ExcelToJson相關
  //#region JsonToExcel相關
  onFileJsonChange(ev: any) {
    const reader = new FileReader();
    const file = ev.target.files[0];
    reader.onload = (e) => {
      const data = reader.result as string;
      const json = JSON.parse(data);
      //console.log(json);
      //將原json打平
      this.flatjson = flatten.flatten(json, { delimiter: '_' });
      //console.log(this.flatjson);
    };
    reader.readAsText(file);
  }
  btnJsonToExcel(ev: any) {
    if (this.flatjson == null) return;
    const workBook = XLSX.utils.book_new(); // 創建一個一作簿
    let _excelcellheaders = {
      header: ['Key', 'Comment', 'tw'],
      skipHeader: false// 是否跳過上面的標題行跳过上面的标题行
    };
    let _data = [];
    for (const key in this.flatjson) {
      if (this.flatjson.hasOwnProperty(key)) {
        const element = this.flatjson[key];
        //console.log(element);
        let myobj = {
          'Key': key, 'Comment': element, 'tw': element
        }
        _data.push(myobj);
      }
    }
    //console.log(_data);
    const workSheet = XLSX.utils.json_to_sheet(_data, _excelcellheaders);
    //console.log(workSheet);
    // 向工作簿中追加工作表
    XLSX.utils.book_append_sheet(workBook, workSheet, 'Sheet1');
    XLSX.writeFile(workBook, 'output.xlsx');
  }

  //#endregion //JsonToExcel相關
  //#region 測試相關
  private testJsonToExcel() {
    const workBook = XLSX.utils.book_new(); // 創建一個一作簿
    console.log(workBook);

    // const workSheet =
    //   XLSX.utils.json_to_sheet([
    //     { '列1': 1, '列2': 2, '列3': 3 },
    //     { '列1': 4, '列2': 5, '列3': 6 }
    //   ],
    //     {
    //       header: ['列1', '列2', '列3'],
    //       skipHeader: false// 是否跳過上面的標題行跳过上面的标题行
    //     });
    // console.log(workSheet);
    // 向工作簿中追加工作表
    // XLSX.utils.book_append_sheet(workBook, workSheet, 'helloWorld');
    // XLSX.writeFile(workBook, 'output.xlsx');


    let _headers = {
      header: ['id', 'name', 'age', 'country', 'remark'],
      skipHeader: false// 是否跳過上面的標題行跳过上面的标题行
    };
    let _data = [{
      id: '1',
      name: 'test1',
      age: '30',
      country: 'China',
      remark: 'hello'
    },
    {
      id: '2',
      name: 'test2',
      age: '20',
      country: 'America',
      remark: 'world'
    },
    {
      id: '3',
      name: 'test3',
      age: '18',
      country: 'Unkonw',
      remark: '???'
    }];

    const workSheet =
      XLSX.utils.json_to_sheet(_data, _headers);
    console.log(workSheet);

    // 向工作簿中追加工作表
    XLSX.utils.book_append_sheet(workBook, workSheet, 'Sheet1');
    XLSX.writeFile(workBook, 'output.xlsx');
  }
  //#endregion //測試相關
}
