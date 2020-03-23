import { Component } from '@angular/core';
import { flatten } from 'flat';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'TestStringToJson';
  twjson = {
    "menu": {
      "home": "首頁",
      "feature": {
        "name": "功能模組",
        "form": "表單"
      },
      "language": "語言",
      "languageList": {
        "taiwanese": "繁體中文",
        "english": "English"
      }
    },
    "validationMessages": {
      "email": {
        "required": "郵箱必須輸入",
        "pattern": "請輸入正確的郵箱地址"
      }
    }
  };
  twstrjson = {
    menu_home: "首頁",
    menu_feature_name: "功能模組",
    menu_feature_form: "表單",
    menu_language: "語言",
    menu_languageList_taiwanese: "繁體中文",
    menu_languageList_english: "English",
    validationMessages_email_required: "郵箱必須輸入",
    validationMessages_email_pattern: "請輸入正確的郵箱地址",
    menu_languageList_china: "簡體中文",
  }
  langlist = [];
  langjson = {};
  constructor() {
    //this.testflattenJson();

  }
  onFileChange(ev) {
    //console.log(ev);
    let workBook = null;
    let jsonData = null;
    const reader = new FileReader();

    const file = ev.target.files[0];
    reader.onload = (event) => {
      const data = reader.result;
      //讀excel檔
      workBook = XLSX.read(data, { type: 'binary' });
      jsonData = workBook.SheetNames.reduce((initial, name) => {
        const sheet = workBook.Sheets[name];
        initial[name] = XLSX.utils.sheet_to_json(sheet);
        return initial;
      }, {});
      let Sheet1jsondata = jsonData['Sheet1'];
      //取得語言的數量
      this.langlist = this.getLanguagelist(Sheet1jsondata);
      this.langlist.forEach(lang => {
        let keyvalue = {};
        Sheet1jsondata.forEach(element => {
          //let key = element["Key"];
          let keyname = Object.getOwnPropertyNames(element)[1];
          let key = element[keyname];
          let value = element[lang];
          //console.log(`key=${key},value=${value}`);
          keyvalue[key] = value;
        });
        this.langjson[lang] = keyvalue;
      });
      console.log(this.langjson);
      // Sheet1jsondata.forEach(element => {
      //   //console.log(element);
      //   let lang = this.langlist[0]
      //   //let key = element["Key"];
      //   let keyname = Object.getOwnPropertyNames(element)[1];
      //   let key = element[keyname];
      //   let value = element[lang];
      //   console.log(`key=${key},value=${value}`);
      //   //
      //   let keyvaluelst = [];
      //   let keyvalue = {};
      //   keyvalue[key] = value;
      //   keyvaluelst.push(keyvalue);
      //   this.langjson[lang] = keyvaluelst;

      //   //let propnames = Object.getOwnPropertyNames(element);
      //   //console.log(propnames);
      //   //console.log(propnames.length);
      // console.log(this.langjson);
      //const dataString = JSON.stringify(Sheet1jsondata);
      //console.log(dataString);
      //document.getElementById('output').innerHTML = dataString.slice(0, 300).concat("...");
    };
    reader.readAsBinaryString(file);
  }
  //取得語言的數量
  getLanguagelist(Sheet1jsondata) {
    let langlist = [];
    Sheet1jsondata.forEach(element => {
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
  btndownload() {
    this.testDownloadJson();
  }
  testDownloadJson() {
    // var blob = new Blob(["Hello, world!"], { type: "text/plain;charset=utf-8" });
    // saveAs(blob, "hello world.txt");
    var jsonse = JSON.stringify(this.twjson);
    var blob = new Blob([jsonse], { type: "application/json" });
    saveAs(blob, "tw.json");
    saveAs(blob, "en.json");
  }
  testflattenJson() {
    //console.log(flatten);
    let flatjson = flatten.flatten(this.twjson, { delimiter: '_' });
    console.log(flatjson);
    //console.log(Object.keys(flatjson));
    Object.keys(flatjson).forEach((key) => {
      //取得key和value
      console.log(`key=${key},value=${flatjson[key]}`);
    })

    console.log("--------------------");
    let myjson = flatten.unflatten(this.twstrjson, { delimiter: '_' })
    console.log(myjson);
  }
}
