using Stylet;
using StyletIoC;

using System.Windows.Navigation;

using CobimExplorer.Interface.Page;
using CobimExplorer.Services.Page;

namespace CobimExplorer.ViewModels.Page.Test
{
    public class TestWebBrowserVM : PageBase, IPage
    {
        #region 프로퍼티

        #region OnActivate

        // TODO : 아래 프로퍼티 "Sender" 필요시 참고 (2024.08.07 jbh)
        /// <summary>
        /// 이벤트 OnActivate 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        // private TestWebBrowserVM Sender { get; set; }

        #endregion OnActivate

        #endregion 프로퍼티

        #region 생성자

        public TestWebBrowserVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;
            // MsgCommand = new RelayCommand(execute => DoSomething()); 
            // MsgCommand = new ViewModelCommand(ExecuteMethod, CanExecuteMethod);
        }

        #endregion 생성자 

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // TODO : 아래 테스트 코드 필요시 참고 (2024.08.07 jbh)
            //Sender = this;

            //var vm = Container.Get<TestWebBrowserVM>();
            //string VMName = vm.GetViewModelName();

            // TODO : 필요시 var msg = new ChangeViewModelMsg(this, VMName); 사용 예정 (2024.08.06 jbh)
            //var msg = new ChangeViewModelMsg(this, VMName);
            //var msg = new OpenPageMsg((ICbVM)Sender, VMName);
            //EventAggregator.PublishOnUIThread(msg);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        #endregion 기본 메소드 

        #region TestWebBrowser_Navigated

        /// <summary>
        /// TestWebBrowser Navigated 이벤트 
        /// </summary>
        public void TestWebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            //BrowserHandler.SetSilent();
        }

        #endregion TestWebBrowser_Navigated


        #region Sample

        // ** 인터넷 익스플로러 전용<WebBrowser> 참고 자료
        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.controls.webbrowser?view=windowsdesktop-8.0
           
        // 참고 2 URL - https://www.csharpstudy.com/WinForms/WinForms-webbrowser.aspx
           
        // 참고 3 URL - https://wpf-tutorial.com/misc-controls/the-webbrowser-control/
           
        // 참고 4 URL - https://www.c-sharpcorner.com/UploadFile/rahul4_saxena/webbrowser-control-in-wpf/
           
        // 참고 5 URL - https://youtu.be/iotQKsKIuMc?si=wh1SRRa9K0xtj3Es
           
        // 참고 6 URL - https://youtu.be/X78Q3AEvvyg?si=RPKBEl1cBgO7yHjm
           
        // 참고 7 URL - https://blog.naver.com/codeknight2/222210163649
           
        // 참고 8 URL - https://lift.tistory.com/32
           
        // 참고 9 URL - https://blog.naver.com/webmcr/220875099734
           
        // 참고 10 URL - https://chatgpt.com/c/caac0521-65d1-45ea-8009-0008b9eaafcd
           
        // 참고 11 URL - https://givemethesocks.tistory.com/8

        #endregion Sample
    }
}
