## 有關多國語言的相關測試記錄

## 建立新的專案
```
ng new Sample01 --routing --style scss
```
## 加入多國語系的模組
```
npm install @ngx-translate/core --save
npm install @ngx-translate/http-loader --save
```
## 模組impot
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
## 使用多國語系在你的元件裏
記得要import service
```
import {TranslateService} from '@ngx-translate/core';
```
以下是參考的語言碼
http://4umi.com/web/html/languagecodes.php