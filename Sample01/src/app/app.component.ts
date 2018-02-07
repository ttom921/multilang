import { Component } from '@angular/core';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';

  constructor(private translate: TranslateService) {
    // 判斷瀏覽器語言，如果抓不到預設為中文
    let userLang = navigator.language.split('-')[0];
    this.translate.setDefaultLang('en');
    userLang = /(zh-tw|en)/gi.test(userLang) ? userLang : 'zh-tw';
    // the lang to use, if the lang isn't available, it will use the current loader to get them
    this.translate.use('zh-tw');
    console.log(navigator.language);

  }
  changeLang(langKey) {
    this.translate.use(langKey);
    // 在ts檔裏取值
    this.translate.get('HelloTrans').subscribe((val: string) => {
      console.log('HelloTrans',val);
    });
  }
}
