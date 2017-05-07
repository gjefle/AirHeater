import { AlarmAppPage } from './app.po';

describe('alarm-app App', () => {
  let page: AlarmAppPage;

  beforeEach(() => {
    page = new AlarmAppPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
