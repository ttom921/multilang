# 有關多國語言的相關測試記錄

## 網頁版



會使用網頁來轉換`json到excel`

tw.json的結構

```json
{
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
  "error": {
    "require": "{{itemName}}為必埴欄位"
  },
  "enum": {
    "gender": "男"
  },
  "validationMessages": {
    "email": {
      "required": "郵箱必須輸入",
      "pattern": "請輸入正確的郵箱地址"
    }
  }
}
```

有找到相關的程式碼，可以將json物件轉成key和value

```
npm install flat
npm install @types/flat --save-dev
```

將上面的json檔轉成

```json
{
menu_home: "首頁", 
menu_feature_name: "功能模組", 
menu_feature_form: "表單", 
menu_language: "語言", 
menu_languageList_taiwanese: "繁體中文", 
…}
```

用法

```typescript
import { flatten } from 'flat';
//...
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
  //...
 
    //console.log(flatten);
    let flatjson = flatten.flatten(this.twjson, { delimiter: '_' });
    console.log(flatjson);
```

下載檔案測試

```
npm install file-saver --save
npm install @types/file-saver --save-dev
```

程式碼

```typescript
  testDownloadJson() {
    // var blob = new Blob(["Hello, world!"], { type: "text/plain;charset=utf-8" });
    // saveAs(blob, "hello world.txt");
    var jsonse = JSON.stringify(this.twjson);
    var blob = new Blob([jsonse], { type: "application/json" });
    saveAs(blob, "tw.json");
    saveAs(blob, "en.json");
  }
```

讀取xlsx

```
npm install xlsx
```







excel的結構

| Key                               | Comment                | en                      | tw                     |
| --------------------------------- | ---------------------- | ----------------------- | ---------------------- |
| menu_home                         | 首頁                   | home                    | 首頁                   |
| menu_feature_name                 | 功能模組               | module                  | 功能模組               |
| menu_feature_form                 | 表單                   | form                    | 表單                   |
| menu_language                     | 語言                   | language                | 語言                   |
| menu_languageList_taiwanese       | 繁體中文               | 繁體中文                | 繁體中文               |
| menu_languageList_english         | english                | english                 | english                |
| error_require                     | {{itemName}}為必埴欄位 | {{itemName}} is require | {{itemName}}為必埴欄位 |
| enum_gender                       | 男                     | man                     | 男                     |
| validationMessages_email_required | 郵箱必須輸入           | email is require        | 郵箱必須輸入           |
| validationMessages_email_pattern  | 請輸入正確的郵箱地址   | email had problem       | 請輸入正確的郵箱地址   |



```

```





## 參考資料



[Fastest way to flatten / un-flatten nested JSON objects](https://stackoverflow.com/questions/19098797/fastest-way-to-flatten-un-flatten-nested-json-objects)

[Build nested JSON from string of nested keys [duplicate\]](https://stackoverflow.com/questions/44168616/build-nested-json-from-string-of-nested-keys)

[用 JavaScript 學習資料結構和演算法：字典（Dictionary）和雜湊表（Hash Table）篇](https://blog.kdchang.cc/2016/09/23/javascript-data-structure-algorithm-dictionary-hash-table/)

## vc版

### 建立新的專案
```
ng new Sample01 --routing --style scss
```
### 加入多國語系的模組
```
npm install @ngx-translate/core --save
npm install @ngx-translate/http-loader --save
```
### 模組impot
```
import {TranslateModule} from '@ngx-translate/core';
import {HttpClientModule, HttpClient} from '@angular/common/http';
import {TranslateModule, TranslateLoader} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
}

imports: [
        .....
         HttpClientModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      }
  })
        .....
    ],

```
使用預設的 loader來讀取多國語言檔
這樣就會載入transModule，此設定翻譯檔預設會抓網站底下的i18n目錄，如果你要自訂翻譯檔的路徑可以改由以下設定:
```
export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
```
### 使用多國語系在你的元件裏
記得要import service
```
import {TranslateService} from '@ngx-translate/core';
```
以下是參考的語言碼
http://4umi.com/web/html/languagecodes.php