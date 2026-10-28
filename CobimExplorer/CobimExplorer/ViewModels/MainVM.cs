using Serilog;
using Stylet;
using StyletIoC;

using System;
using System.Windows;
using System.Reflection;

using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.LogManager;

using CobimExplorer.ViewModels.Windows.Login;
using CobimExplorer.ViewModels.Page.Parents;
using CobimExplorer.ViewModels.Message;

using CobimExplorer.Services;
using CobimExplorer.Interface.Page;
using CobimExplorer.Settings;

using CobimExplorer.Data.Models.CobimBase.Main;

namespace CobimExplorer.ViewModels
{
    // TODO : COBIM 탐색기 응용 프로그램에 아이콘 적용하기 위해 인재 INC COBIM 웹브라우저 상단 탭 아이콘(파비콘 - favicon_.ico) 다운로드 받은 후 
    //        해당 탭 아이콘 이름 변경 (기존) favicon_.ico -> (변경) CobimIcon.ico
    //        변경한 아이콘(CobimIcon.ico)을 프로젝트 파일 "CobimExplorer"에 리소스(Resources) 추가 후 
    //        프로젝트 파일 "CobimExplorer" 우클릭 -> 속성(R) 클릭 -> 프로젝트 파일 "CobimExplorer" 속성 창 출력 
    //        -> 속성 창 좌측 상단 애플리케이션 클릭 -> 애플리케이션 화면 하단 항목 "리소스"
    //        -> 항목 "아이콘 및 매니페스트(C)"에 아이콘(CobimIcon.ico) 적용 및 저장 (2024.08.23 jbh)
    // * 웹브라우저 상단 탭 아이콘(파비콘 - favicon_.ico)
    // 참고 URL - https://stay-present.tistory.com/14
    // * 응용 프로그램에 아이콘(CobimIcon.ico) 적용 방법
    // 참고 URL - https://products.conholdate.com/ko/total/net/conversion/png-to-ico/
    // 참고 2 URL - https://afsdzvcx123.tistory.com/entry/C-%EC%9C%88%ED%8F%BC-%EC%9C%88%ED%8F%BC-%ED%94%84%EB%A1%9C%EA%B7%B8%EB%9E%A8-%EC%8B%A4%ED%96%89%ED%8C%8C%EC%9D%BC-%EC%95%84%EC%9D%B4%EC%BD%98Icon-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0
    // 참고 3 URL - https://studyingpingu.tistory.com/28

    // * 엑셀(Excel) 프로그램 사용하여 아이콘(CobimIcon.ico) 및 기타 png 파일의 배경 투명(또는 제거) 하는 방법 
    // (예) CDE.png, HubBox.png
    // 참고 URL - https://youtu.be/cdK34DxYlaY?si=QOdopacQJHUU2225

    // * 이미지 파일(.png) -> 아이콘 파일(.ico) 변환 사이트
    // 참고 URL - https://convertio.co/kr/

    public class MainVM : CobimScreen<MainView>, IHandle<OpenPageMsg>   // IHandle, IHandle<OpenedPageMsg>   // , IHandle<ChangeViewModelMsg>
    /* TODO: IChildSalesPage, IHandle<> 인터페이스 추후 상속 예정 (2023.07.05 jbh) */
    // public class MainVM : Conductor<IPageBase>.Collection.OneActive, IHandle<ChangeViewModelMsg>   
    {
        #region 프로퍼티 

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        // TODO : 필요시 프로퍼티 "WindowManager" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IWindowManager WindowManager { get; set; }

        // TODO : TabParentPageV.xaml -> 추후 TabControl에 바인딩되는 ItemsSource="{Binding Tabs}" 곳에서 프로퍼티 "Tabs"의 위치를 
        //        메인 뷰모델(MainVM.cs)로 위치 이동할 필요가 생기면 "Tabs" 대신에 프로퍼티 "PageParent" 사용해서
        //        TabItem 추가/삭제 이벤트 구현하기 (2024.08.05 jbh)
        // public IPageParent PageParent { get; set; }

        #region MainView

        // TODO : Main 뷰 프로퍼티 "MainV" 필요시 사용 예정 (2023.11.09 jbh)
        // TODO : MainV 클래스 객체 MainV의 경우 CombimExplorer 화면 상단 타이틀바 버튼 ("최소화", "최대화", "닫기") 실행시
        //        MainV.xaml 파일의 xaml 코드에 기록된 값들이 계속 수정되므로 싱글톤 패턴 구현 대상 제외 (2023.11.09 jbh)
        // 참고 URL - https://geukggom.tistory.com/1
        /// <summary>
        /// Main 뷰 MainV 
        /// </summary>
        //[Inject]
        //public MainV MainV { get => _MainV; set { _MainV = value; NotifyOfPropertyChange(); } }
        //private MainV _MainV;

        // TODO : Main 뷰모델 프로퍼티 "mainVM" 필요시 사용 예정 (2023.11.09 jbh)
        /// <summary>
        /// Main 뷰모델 MainVM 
        /// </summary>
        // public MainVM mainVM { get => _mainVM; set { _mainVM = value; NotifyOfPropertyChange(); } }
        // private MainVM _mainVM;


        // TODO : Main 뷰 화면 종료 여부 확인 프로퍼티 "IsExit" 필요시 사용 예정 (2023.11.08 jbh)
        /// <summary>
        /// 종료 (Exit)
        /// </summary>
        //public bool IsExit
        //{
        //    get => _IsExit;
        //    set
        //    {
        //        _IsExit = value;
        //        NotifyOfPropertyChange(nameof(_IsExit));
        //    }
        //}
        //private bool _IsExit;

        #endregion MainView

        #region TitleBar

        // TODO : 타이틀 바 뷰(TitleBar) + 뷰모델(MainVM) 프로퍼티 "TitleBar" 필요시 사용 예정 (2023.11.09 jbh)
        /// <summary>
        /// 탐색기 화면에서 출력하는 타이틀 바 뷰(TitleBar) + 뷰모델(MainVM)
        /// </summary>
        //public UserControl TitleBar { get => _TitleBar; set { _TitleBar = value; NotifyOfPropertyChange(); } }
        //private UserControl _TitleBar = new UserControl();

        // TODO : 타이틀 바 컨트롤 프로퍼티 "TitleBarControl" 필요시 사용 예정 (2023.11.09 jbh)
        /// <summary>
        /// 타이틀 바 컨트롤 
        /// </summary>
        //[Inject]
        //public TitleBarControl TitleBarControl { get => _TitleBarControl; set { _TitleBarControl = value; NotifyOfPropertyChange(); } }
        //private TitleBarControl _TitleBarControl;

        /// <summary>
        /// CobimExplorer Title 
        /// </summary>
        public string MainTitle { get => _MainTitle; set { _MainTitle = value; NotifyOfPropertyChange(); } }
        private string _MainTitle;

        /// <summary>
        /// CobimExplorer - 로그인 접속자 환영 메시지
        /// </summary>
        public string WelcomeBack { get => _WelcomeBack; set { _WelcomeBack = value; NotifyOfPropertyChange(); } }
        private string _WelcomeBack;

        /// <summary>
        /// CobimExplorer - 로그인 사용자 아이디
        /// </summary>
        public string LoginId { get => _LoginId; set { _LoginId = value; NotifyOfPropertyChange(); } }
        private string _LoginId;

        #endregion TitleBar

        #region UserControl

        // TODO : UserControl 클래스 프로퍼티 "Page" 필요시 참고 (2024.08.05 jbh)
        // TODO : UserControl 클래스 프로퍼티 "Page"의 경우 프로퍼티 안에 속한 값을
        //        변경할 수 있는 필드(Content, DataContext)가 존재하므로 싱글톤 구현 대상 제외 (2023.11.08 jbh)
        // 참고 URL - https://minchul-son.tistory.com/317
        /// <summary>
        /// 탐색기 화면에서 출력하는 화면 뷰 + 뷰모델
        /// </summary>
        // public UserControl Page { get => _Page; set { _Page = value; NotifyOfPropertyChange(); } }
        // private UserControl _Page = new UserControl();

        /// <summary>
        /// UserControl 뷰 TabParentPageV (싱글톤 객체)
        /// </summary>
        [Inject]
        public TabParentPageVM tabParentPageVM { get; set; }

        /// <summary>
        /// UserControl 뷰 TabParentPageV (싱글톤 객체)
        /// </summary>
        // [Inject]
        // public TabParentPageV TabParentPageV { get => _TabParentPageV; set { _TabParentPageV = value; NotifyOfPropertyChange(); } }
        // private TabParentPageV _TabParentPageV;

        #endregion UserControl

        // TODO : 프로퍼티 에서 사용하는 private set; 기능이 무엇인지 꼭 확인하기 (2023.07.07 jbh)
        // private set; 의 역할 - 해당 필드 값을 읽기전용으로 쓰겠다는 것을 의미
        // 참고 URL - https://yeko90.tistory.com/entry/c-private-set-%EC%82%AC%EC%9A%A9-%EC%9D%B4%EC%9C%A0%ED%94%84%EB%9F%AC%ED%8D%BC%ED%8B%B0

        #endregion 프로퍼티 

        #region 생성자

        // TODO: 필요시 생성자 MainVM 로직 보완 예정 (2023.07.04 jbh)
        public MainVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;
            // this.mainVM = this;

            // PageParent = (IPageParent)this.Container.Get<TabParentPageVM>(); 

            //MainTitle = CobimHelper.MainTitle;   // TitleBar

            // TODO : events.Subscribe(this); 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            events.Subscribe(this);
        }

        #endregion 생성자 

        #region 기본 메소드

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            OpenLogin();                       // 로그인 화면 출력 

            EventAggregator.Subscribe(this);   // EventAggregator 구독 (Subscribe)

            InitSetting();                     // 화면 초기화 
        }

        // TODO : OnClose 메서드 로직 추후 수정 예정 (2023.07.04 jbh)
        protected override void OnClose()
        {
            base.OnClose();
            // EventAggregator.Unsubscribe();
            this.EventAggregator.Unsubscribe((IHandle)this);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        // TODO : 메서드 "CanExecuteMethod" 필요시 수정 예정 (2023.11.08 jbh)
        private bool CanExecuteMethod(object obj)
        {
            return true;
        }

        #endregion 기본 메소드      

        #region OpenLogin

        /// <summary>
        /// 로그인 화면 오픈 
        /// </summary>
        private void OpenLogin()
        {
            // TODO : 오류 발생시 로그인 생성 로직 수정 예정 (2023.08.17 jbh)
            // this.Container.Get<LoginVM>().ToShowData();
            //bool result = this.Container.Get<LoginVM>().ToShowData().IsLogin;

            // TODO : 오류 발생 또는 로그인 실패시 종료 로직 필요시 수정 예정 (2023.08.17 jbh)
            // if (!result) return;  // 오류 발생, 로그인 실패, 프로그램 종료(닫기 "X" 버튼) 시 종료 

            //bool result = this.Container.Get<LoginVM>().ToShowData().IsExit;
            //if (result) Application.Current.Shutdown();
            var vm = this.Container.Get<LoginVM>().ToShowData();  // 로그인 화면 생성 및 결과 리턴

            // TODO : 닫기("X") 버튼 클릭 시 탐색기 응용 프로그램 종료 로직 구현 (2023.08.18 jbh)
            // 참고 URL - https://insurang.tistory.com/entry/WPF-C-4%EA%B0%80%EC%A7%80-%ED%94%84%EB%A1%9C%EA%B7%B8%EB%9E%A8-%EC%A2%85%EB%A3%8C-%EC%BD%94%EB%93%9C%EB%A5%BC-%EC%83%88%EC%B0%BD%EC%97%90%EC%84%9C-Close-Exit-Kill
            if (false == vm.IsLogin && true == vm.IsExit)
            {
                Environment.Exit(0);   // 탐색기 응용 프로그램 종료

                // Application.Current.Shutdown();                          // 현재창(로그인 화면 뷰 LoginV.xaml) 종료 
                // Application.Current.MainWindow.Close();                  // 메인창(메인 화면 뷰 MainV.xaml) 종료
                // System.Diagnostics.Process.GetCurrentProcess().Kill();   // 탐색기 응용 프로그램 종료
            }
        }

        #endregion OpenLogin

        #region InitSetting

        // TODO : 아래 테스트용 뷰모델(TabParentPageVM) 초기 셋팅 메서드 "InitSetting" 
        //        테스트 기간 끝나면 주석 처리 예정 (2024.08.07 jbh)
        /// <summary>
        /// 메인 뷰모델(MainVM) 화면 초기 셋팅 
        /// </summary>
        private void InitSetting()
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                this.MainTitle = CobimHelper.MainTitle;       // TitleBar
                this.LoginId = AppSetting.Default.LoginBase.GetLoginId();   // 로그인 사용자 아이디
                this.WelcomeBack = CobimHelper.WelcomeBack;   // 로그인 접속자 환영 메시지

                this.tabParentPageVM.IsTestNewTabItem = true;
                this.tabParentPageVM.InitSetting();   // 테스트용 뷰모델(TabParentPageVM) 초기화 메서드 "InitSetting" 호출 

                // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.08.07 jbh)
                // this.ActivateItem(this.tabParentPageVM.ActiveItem);
                // var vms = Container.GetAll<IPage>().ToArray();   // <IPageBase> 인터페이스를 상속 받는 모든 클래스 요소들을 컨테이너에서 가져와서(Container.GetAll)

                // this.Items.AddRange(vms);

                // var tabParent = this.Items.Where(it => string.Equals(it.GetType().Name, CobimHelper.TabParentPageVMName)).FirstOrDefault();

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                // if(tabParent is not null)
                // {
                //     // "TabParentPageVM" 뷰모델과 연동된 뷰(TabParentPageV)를 열어준다.
                //     this.ActivateItem(tabParentV);
                // 
                //     // Page.Content = TabParentPageV;
                //     // Page.DataContext = (TabParentPageVM)tabParent;
                // }

                // var tabParent = (IPage)this.tabParentPageVM;
                // var test = this.Items.Where(it => string.Equals(it.GetType().Name, nameof(TestVM))).FirstOrDefault();
                // var test = this.Items.Where(it => string.Equals(it.GetType().Name, CobimHelper.TestVMName)).FirstOrDefault();

                // // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.08.07 jbh)
                // // 참고 URL - https://husk321.tistory.com/405
                // if(test is not null)
                // {
                //     // "TestVM" 뷰모델과 연동된 뷰(testV)를 열어준다.
                //     this.ActivateItem(test);

                //     // Page.Content = TabParentPageV;
                //     // Page.DataContext = (TabParentPageVM)tabParent;
                // }

                // UserControl 
                // TODO : UserControl List 객체 UserControlItems 에 UserControl 객체 추가 (2023.11.06 jbh)
                // UserControlItems.Clear();       // List 객체 UserControlItems 초기화 처리 
                // UserControlItems.Add(ShellV);
                // UserControlItems.Add(TestV);
                // UserControlItems.Add(TestExplorerV);
                // UserControlItems.Add(TestLazyTreeV);
                // UserControlItems.Add(ExplorerV);
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // Application.Current.Shutdown();
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion InitSetting

        #region Handle

        // TODO : TabParentPageV.xaml -> 추후 TabControl에 바인딩되는 ItemsSource="{Binding Tabs}" 곳에서 프로퍼티 "Tabs"의 위치를 
        //        메인 뷰모델(MainVM.cs)로 위치 이동할 필요가 생기면 아래 Handle(OpenPageMsg message) 이벤트 메서드 꼭 사용하기 (2024.08.05 jbh)
        // TODO : 추후 필요시 메서드 "Handle" 구현 예정 (2024.01.15 jbh)
        /// <summary>
        /// 받은 메세지로 동작한다.(받은 메세지로 TabItem Content 영역에 화면 출력)
        /// </summary>
        /// <param name="message"></param>
        public void Handle(OpenPageMsg message)
        {
            IPage openPage = null;                 // 리본 버튼 클릭 후 탐색기 응용 프로그램에서 오픈하고자 하는 화면 초기화 
            string openPageName = string.Empty;    // 리본 버튼 클릭 후 탐색기 응용 프로그램에서 오픈하고자 하는 화면 이름 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                openPage = message.Page;

                //openPageName = message.Page.GetType().Name;
                openPageName = message.VMName;

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.08.06 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if(openPage is not null)
                {
                    // 리본 버튼 클릭한 경우 로직 필요시 수정 예정 (2023.12.14 jbh) 
                    // 리본 버튼 클릭한 경우
                    MainView.CommonData.BtnClickType = string.Empty;
                    MainView.CommonData.BtnClickType = CobimHelper.RibbonBtnType;
                    // MainView.CommonData.IsRibbonBtnClick = true;   // 리본 버튼 클릭한 경우

                    // openPage에 할당된 뷰모델 가지고 TabItem Content 영역에 화면 출력 하도록 this.ActivateItem 호출
                    this.ActivateItem(openPage);
                }
                else throw new Exception($"화면-{openPageName} 출력 오류!\r\n담당자에게 문의하세요.");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion Handle

        #region Handle

        // TODO : 추후 필요시 메서드 "Handle" 구현 예정 (2024.01.15 jbh)
        /// <summary>
        /// 받은 메세지로 동작한다.
        /// </summary>
        /// <param name="message"></param>
        //public void Handle(OpenedPageMsg message)
        //{
        //    var test = message.Page;
        //}

        #endregion Handle

        #region Sample

        // TODO : 아래 주석친 코드 필요시 참고 (2024.08.05 jbh)

        #region OnActivate

        //protected override void OnActivate()
        //{
        //    base.OnActivate();

        //    // TODO : 최소화 버튼 클릭 후 화면 사라짐. -> 최소화 버튼 다시 클릭 -> 화면 활성화될 때 메서드 "OnActivate" 활성화 될때, 
        //    //        아래처럼 EventAggregator 구독 (Subscribe)을 다시 해주도록 구현 (2023.11.09 jbh)
        //    //        EventAggregator 구독 (Subscribe)을 해주는 이유는
        //    //        아래처럼 구독을 다시 안 하게 되면 리본 컨트롤에서 버튼 클릭시 화면이 제대로 이동하지 못하고 멈추기 때문이다. 
        //    // EventAggregator.Subscribe(this);   // EventAggregator 구독 (Subscribe)

        //}

        #endregion OnActivate

        #region OnDeactivate

        //protected override void OnDeactivate()
        //{
        //    base.OnDeactivate();

        //    //DoDeactivate();

        //    EventAggregator.Unsubscribe(this);
        //}

        #endregion OnDeactivate

        #region DisplaySetting

        // TODO : 필요시 메서드 "DisplaySetting" 사용 예정 (2023.12.22 jbh)
        //private void DisplaySetting(ChangeViewModelMsg message)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.07 jbh)
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // UserControl Content, DataContext 초기화 사용시 오류 발생하면 수정 예정 (2023.11.08 jbh)
        //        Page.Content     = null;   // UserControl Content 초기화
        //        Page.DataContext = null;   // UserControl DataContext 초기화

        //        switch (message.VMName)
        //        {
        //            case CobimHelper.ShellVMName:
        //                // TODO : UserControl 객체 생성 -> 화면 뷰 "ShellV" 출력 및 뷰모델 "ShellVM" 연동 하도록 구현 (2023.11.06 jbh)
        //                // 참고 URL - https://www.c-sharpcorner.com/article/use-of-user-controls-in-wpf/
        //                // UserControl = new UserControl { Content = ShellV, DataContext = (ShellVM)message.obj };

        //                // TODO : 아래 코드(UserControl.Content, UserControl.DataContext) 사용시 오류 발생하면 위에 주석 처리된 코드
        //                //        UserControl = new UserControl { Content = ShellV, DataContext = (ShellVM)message.obj }; 사용해서 수정 예정 (2023.11.06 jbh)
        //                Page.Content = ShellV;
        //                Page.DataContext = (ShellVM)message.obj;


        //                // TODO : 리소스 해제 메서드 "TryDispose" Stylet 솔루션 파일(Stylet.sln) -> static 클래스 "ScreenExtensions.cs" -> static 메서드 "TryDispose" 참고해서 구현 (2023.11.06 jbh)
        //                // ScreenExtensions.TryDispose(TestV);
        //                // ScreenExtensions.TryDispose(TestExplorerV);
        //                // ScreenExtensions.TryDispose(TestLazyTreeV);
        //                // ScreenExtensions.TryDispose(ExplorerV);

        //                // TODO : 위의 소스코드를 반복적으로 실행할 수 있도록 메서드 "UserControlIDispose" 구현 (2023.11.06 jbh)
        //                UserControlIDispose(Page);
        //                break;

        //            case CobimHelper.TestVMName:
        //                // TODO : UserControl 객체 생성 -> 화면 뷰 "TestV" 출력 및 뷰모델 "TestVM" 연동 하도록 구현 (2023.11.06 jbh)
        //                // 참고 URL - https://www.c-sharpcorner.com/article/use-of-user-controls-in-wpf/
        //                // UserControl = new UserControl { Content = TestV, DataContext = (TestVM)message.obj };

        //                // TODO : 아래 코드(UserControl.Content, UserControl.DataContext) 사용시 오류 발생하면 위에 주석 처리된 코드
        //                //        UserControl = new UserControl { Content = TestV, DataContext = (TestVM)message.obj }; 사용해서 수정 예정 (2023.11.06 jbh)
        //                Page.Content = TestV;
        //                Page.DataContext = (TestVM)message.obj;

        //                // TODO : 리소스 해제 메서드 "UserControlIDispose" 구현 (2023.11.06 jbh)
        //                UserControlIDispose(Page);
        //                break;

        //            case CobimHelper.TestExpVMName:
        //                // TODO : UserControl 객체 생성 -> 화면 뷰 "TestExplorerV" 출력 및 뷰모델 "TestExplorerVM" 연동 하도록 구현 (2023.11.06 jbh)
        //                // 참고 URL - https://www.c-sharpcorner.com/article/use-of-user-controls-in-wpf/
        //                // UserControl = new UserControl { Content = TestExplorerV, DataContext = (TestExplorerVM)message.obj };

        //                // TODO : 아래 코드(UserControl.Content, UserControl.DataContext) 사용시 오류 발생하면 위에 주석 처리된 코드
        //                //        UserControl = new UserControl { Content = TestV, DataContext = (TestVM)message.obj }; 사용해서 수정 예정 (2023.11.06 jbh)
        //                Page.Content = TestExplorerV;
        //                Page.DataContext = (TestExplorerVM)message.obj;

        //                // TODO : 리소스 해제 메서드 "UserControlIDispose" 구현 (2023.11.06 jbh)
        //                UserControlIDispose(Page);
        //                break;

        //            case CobimHelper.TestLazyTrVMName:
        //                // TODO : UserControl 객체 생성 -> 화면 뷰 "TestLazyTreeV" 출력 및 뷰모델 "TestLazyTreeVM" 연동 하도록 구현 (2023.11.06 jbh)
        //                // 참고 URL - https://www.c-sharpcorner.com/article/use-of-user-controls-in-wpf/
        //                // UserControl = new UserControl { Content = TestLazyTreeV, DataContext = (TestLazyTreeVM)message.obj };

        //                // TODO : 아래 코드(UserControl.Content, UserControl.DataContext) 사용시 오류 발생하면 위에 주석 처리된 코드
        //                //        UserControl = new UserControl { Content = TestLazyTreeV, DataContext = (TestLazyTreeVM)message.obj }; 사용해서 수정 예정 (2023.11.06 jbh)
        //                Page.Content = TestLazyTreeV;
        //                Page.DataContext = (TestLazyTreeVM)message.obj;

        //                // TODO : 리소스 해제 메서드 "UserControlIDispose" 구현 (2023.11.06 jbh)
        //                UserControlIDispose(Page);
        //                break;

        //            case CobimHelper.ExpVMName:
        //                // TODO : UserControl 객체 생성 -> 화면 뷰 "ExplorerV" 출력 및 뷰모델 "ExplorerVM" 연동 하도록 구현 (2023.11.06 jbh)
        //                // 참고 URL - https://www.c-sharpcorner.com/article/use-of-user-controls-in-wpf/
        //                // UserControl = new UserControl { Content = ExplorerV, DataContext = (ExplorerVM)message.obj };

        //                // TODO : 아래 코드(UserControl.Content, UserControl.DataContext) 사용시 오류 발생하면 위에 주석 처리된 코드
        //                //        UserControl = new UserControl { Content = ExplorerV, DataContext = (ExplorerVM)message.obj }; 사용해서 수정 예정 (2023.11.06 jbh)
        //                Page.Content = ExplorerV;
        //                Page.DataContext = (ExplorerVM)message.obj;

        //                // TODO : 리소스 해제 메서드 "UserControlIDispose" 구현 (2023.11.06 jbh)
        //                UserControlIDispose(Page);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        return;
        //    }
        //}

        #endregion DisplaySetting

        #region UserControlIDispose

        // TODO : 필요시 UserControl 리소스 해제 메서드 "UserControlIDispose" 사용 예정 (2023.12.22 jbh)
        /// <summary>
        /// 실행하는 화면 뷰를 제외한 나머지 UserControl 리소스 해제 (TryDispose 처리)
        /// </summary>
        /// <param name="userControl"></param>
        /// <returns></returns>
        //private void UserControlIDispose(UserControl userControl)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.07 jbh)
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // TODO : Linq 확장 메서드 "FindAll" "ForEach" 사용
        //        // List 의 FindAll()
        //        // 참고 URL - https://jwidaegi.blogspot.com/2019/05/unity-list-findall.html

        //        // C# LINQ List 의 All(), Any()
        //        // 참고 URL - https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=empty_wagon&logNo=20148303261

        //        // C# LINQ List 의 ForEach()
        //        // 참고 URL - https://www.codegrepper.com/code-examples/csharp/linq+foreach+c%23
        //        // TODO : 실행하는 화면 뷰를 제외한 나머지 UserControl 리소스 해제 (2023.11.06 jbh)
        //        // TODO : 리소스 해제 메서드 "TryDispose" Stylet 솔루션 파일(Stylet.sln) -> static 클래스 "ScreenExtensions.cs" -> static 메서드 "TryDispose" 참고해서 구현 (2023.11.06 jbh)
        //        // Dispose와 Close 차이점
        //        // 참고 URL - https://blueasa.tistory.com/258
        //        // UserControlItems.FindAll(view => !view.Equals(userControl.Content))
        //        //                 .ForEach(view => { ScreenExtensions.TryDispose(view); });
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //    }
        //}

        #endregion UserControlIDispose

        #region Handle

        /// <summary>
        /// 받은 메세지로 동작한다.
        /// </summary>
        /// <param name="message"></param>
        //public void Handle(ChangeViewModelMsg message)
        //{
        //    // TODO : 아래 주석 친 UserControl Content 초기화 코드 필요시 사용 예정 (2023.12.22 jbh)
        //    // Page.Content     = null;   // UserControl Content 초기화
        //    Page.DataContext = null;   // UserControl DataContext 초기화

        //    foreach (var item in message.Items)
        //    {
        //        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        if (item.GetType().Name.Equals(message.VMName) && message?.VMName is not null)
        //        {
        //            // TODO : 아래 테스트 코드 추후 삭제 예정 (2023.07.26 jbh)
        //            //var changeVM = message.Items.Where(it => string.Equals(it.GetType().Name, message.VMName)).FirstOrDefault();
        //            //this.ActivateItem(changeVM);

        //            Page.DataContext = message.obj;



        //            // TODO : 필요시 메서드 "DisplaySetting" 사용 예정 (2023.12.22 jbh)
        //            // DisplaySetting(message);

        //            // TODO : 필요시 리소스 해제 메서드 "UserControlIDispose" 사용 예정 (2023.11.06 jbh)
        //            // UserControlIDispose(Page);
        //        }
        //    }

        //    //foreach(var item in this.Items)
        //    //{
        //    // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
        //    // 참고 URL - https://husk321.tistory.com/405
        //    //    if(item.GetType().Name.Equals(message.VMName) && message?.VMName is not null)
        //    //    {
        //    //        this.ActivateItem(item);

        //    //        bool change = true;

        //    //        DisplaySetting(change);
        //    //    }

        //    //}
        //}

        #endregion Handle

        #region TEST

        // TODO : 추후 필요 없을시 삭제 예정 (2023.07.13 jbh)
        //public void ChangeExplorerViewModel(string vmName)
        //{
        //    //var vms = Container.GetAll<IPageBase>().ToArray();   // <IPageBase> 인터페이스를 상속 받는 모든 클래스 요소들을 컨테이너에서 가져와서(Container.GetAll)

        //    //this.Items.Clear();
        //    //this.Items.AddRange(vms);
        //    // var changeVM = this.Items.Where(it => string.Equals(it.GetType().Name, nameof(ExplorerViewModel))).FirstOrDefault();
        //    var changeVM = this.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();

        //    //var changeVM = this.Items.Where(it => it.GetType().Equals(TestViewModel.GetType())).FirstOrDefault();

        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
        // 참고 URL - https://husk321.tistory.com/405
        //    if (changeVM is not null)
        //    {
        //        // "ShellViewModel" 뷰모델과 연동된 초기화면 뷰(ShellView)를 열어준다.
        //        this.ActivateItem(changeVM);
        //    }
        //}

        /// <summary>
        /// 버튼 테스트 
        /// </summary>
        // 메시지 박스 출력 참고 URL - https://gigong.tistory.com/52
        //public void DoSomething()
        //{
        //    // Debug.WriteLine("DoSomething called");
        //    MessageBox.Show("Button Test");
        //}

        #endregion TEST

        #endregion Sample
    }
}
