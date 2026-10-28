using Stylet;
using StyletIoC;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Runtime.InteropServices;
using CobimExplorer.Services.Page;
using CobimExplorer.Interface.Page;
using CobimExplorer.Message;
using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.LogManager;
using CobimExplorer.ViewModels.Windows.Login;
using CobimExplorer.Rest.Api.CobimBase.Explorer;
using CobimExplorer.Data.Models.CobimBase.Main;
using CobimExplorer.Services;

namespace CobimExplorer.ViewModels.Windows.Test
{
    // TODO : 추후 "CobimScreen" 클래스 구현하기 (2023.12.14 jbh)
    public class TestMainVM : CobimScreen<MainView>, IHandle<ChangeViewModelMsg>
    /* TODO: IChildSalesPage, IHandle<> 인터페이스 추후 상속 예정 (2023.07.05 jbh) */
    // public class TestMainVM : Conductor<IPageBase>.Collection.OneActive, IHandle<ChangeViewModelMsg>   
    {

        // TODO : Enum 열거형 구조체 EnumDisplayType 필요시 사용 예정 (2023.11.06 jbh)
        //#region EnumDisplayType

        //public enum EnumDisplayType : int
        //{
        //    [System.ComponentModel.Description("StartVM")]
        //    StartVMName = 0,
        //    [System.ComponentModel.Description("TestVM")]
        //    TestVMName = 1,
        //    [System.ComponentModel.Description("TestExplorerVM")]
        //    TestExpVMName = 2,
        //    [System.ComponentModel.Description("TestLazyTreeVM")]
        //    TestLazyTrVMName = 3,
        //    [System.ComponentModel.Description("ExplorerVM")]
        //    ExpVMName = 4
        //}

        //#endregion EnumDisplayType

        #region 프로퍼티 

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        // TODO : 필요시 프로퍼티 "WindowManager" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IWindowManager WindowManager { get; set; }

        public IPageParent PageParent { get; set; }

        #region ICommand

        #region System

        // TODO : Cobim Explorer 화면 상단 타이틀 바 버튼("최소화", "최대화", "종료") 필요시 사용 예정 (2023.11.09 jbh)
        /// <summary>
        /// 최소화 Command
        /// </summary>
        public ICommand MinCommand { get; set; }


        /// <summary>
        /// 최대화 Command
        /// </summary>
        public ICommand MaxCommand { get; set; }


        /// <summary>
        /// 종료 Command
        /// </summary>
        public ICommand ExitCommand { get; set; }

        #endregion System

        #region UserControl

        /// <summary>
        /// Button Command (TestViewModel)
        /// </summary>
        public ICommand TestVMCommand { get; set; }

        /// <summary>
        /// Button Command (TestExplorerVM)
        /// </summary>
        public ICommand TestExpVMCommand { get; set; }
        
        /// <summary>
        /// Button Command (TestLazyTreeVM)
        /// </summary>
        public ICommand TestLazyTreeVMCommand { get; set; }
        
        /// <summary>
        /// Button Command (ExplorerVM)
        /// </summary>
        public ICommand ExpVMCommand { get; set; }

        #endregion UserControl

        #endregion ICommand

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

        #endregion TitleBar

        #region Ribbon

        // TODO : UserControl 클래스 리본 프로퍼티 "Ribbon" 필요시 사용 예정 (2023.11.09 jbh)
        /// <summary>
        /// 탐색기 화면에서 출력하는 리본 바 뷰(RibbonControl) + 뷰모델(MainVM)
        /// </summary>
        //public UserControl Ribbon { get => _Ribbon; set { _Ribbon = value; NotifyOfPropertyChange(); } }
        //private UserControl _Ribbon = new UserControl();

        // TODO : 리본 컨트롤 프로퍼티 "RibbonMenu" 필요시 사용 예정 (2023.11.09 jbh)
        /// <summary>
        /// 리본 컨트롤
        /// </summary>
        //[Inject]
        //public RibbonControl RibbonMenu { get => _RibbonMenu; set { _RibbonMenu = value; NotifyOfPropertyChange(); } }
        //private RibbonControl _RibbonMenu;


        /// <summary>
        /// CDE 리본 탭 - Header
        /// </summary>
        public string CDE_RibbonTab { get => _CDE_RibbonTab; set { _CDE_RibbonTab = value; NotifyOfPropertyChange(); } }
        private string _CDE_RibbonTab;

        /// <summary>
        /// CDE 리본 그룹 - Header
        /// </summary>
        public string CDE_RibbonGroup { get => _CDE_RibbonGroup; set { _CDE_RibbonGroup = value; NotifyOfPropertyChange(); } }
        private string _CDE_RibbonGroup;

        /// <summary>
        /// CDE 리본 버튼 - Text
        /// </summary>
        public string CDE_RibbonButton { get => _CDE_RibbonButton; set { _CDE_RibbonButton = value; NotifyOfPropertyChange(); } }
        private string _CDE_RibbonButton;

        #endregion Ribbon

        #region UserControl

        // TODO : UserControl 클래스 프로퍼티 "UserControl"의 경우 프로퍼티 안에 속한 값을
        //        변경할 수 있는 필드(Content, DataContext)가 존재하므로 싱글톤 구현 대상 제외 (2023.11.08 jbh)
        // 참고 URL - https://minchul-son.tistory.com/317
        /// <summary>
        /// 탐색기 화면에서 출력하는 화면 뷰 + 뷰모델
        /// </summary>
        public UserControl Page { get => _Page; set { _Page = value; NotifyOfPropertyChange(); } }
        private UserControl _Page = new UserControl();

        // TODO : UserControl 뷰 "StartV" 추후 필요 없을시 삭제 예정 (2023.12.22 jbh)
        /// <summary>
        /// UserControl 뷰 StartV (싱글톤 객체)
        /// </summary>
        //[Inject]
        //public StartV StartV { get => _StartV; set { _StartV = value; NotifyOfPropertyChange(); } }
        //private StartV _StartV;

        // TODO : UserControl 뷰 "TestV" 추후 필요 없을시 삭제 예정 (2023.12.22 jbh)
        /// <summary>
        /// UserControl 뷰 TestV (싱글톤 객체)
        /// </summary>
        //[Inject]
        //public TestV TestV { get => _TestV; set { _TestV = value; NotifyOfPropertyChange(); } }
        //private TestV _TestV;

        // TODO : UserControl 뷰 "TestExplorerV" 추후 필요 없을시 삭제 예정 (2023.12.22 jbh)
        /// <summary>
        /// UserControl 뷰 TestExplorerV (싱글톤 객체)
        /// </summary>
        //[Inject]
        //public TestExplorerV TestExplorerV { get => _TestExplorerV; set { _TestExplorerV = value; NotifyOfPropertyChange(); } }
        //private TestExplorerV _TestExplorerV;

        // TODO : UserControl 뷰 "TestLazyTreeV" 추후 필요 없을시 삭제 예정 (2023.12.22 jbh)
        /// <summary>
        /// UserControl 뷰 TestLazyTreeV (싱글톤 객체)
        /// </summary>
        //[Inject]
        //public TestLazyTreeV TestLazyTreeV { get => _TestLazyTreeV; set { _TestLazyTreeV = value; NotifyOfPropertyChange(); } }
        //private TestLazyTreeV _TestLazyTreeV;

        // TODO : UserControl 뷰 "ExplorerV" 추후 필요 없을시 삭제 예정 (2023.12.22 jbh)
        /// <summary>
        /// UserControl 뷰 ExplorerV (싱글톤 객체)
        /// </summary>
        //[Inject]
        //public ExplorerV ExplorerV { get => _ExplorerV; set { _ExplorerV = value; NotifyOfPropertyChange(); } }
        //private ExplorerV _ExplorerV;

        // TODO : UserControl 리스트 객체 "UserControlItems" 추후 필요 없을시 삭제 예정 (2023.12.22 jbh)
        /// <summary>
        /// UserControl 리스트 객체 UserControlItems
        /// </summary>
        // public List<UserControl> UserControlItems { get; set; } = new List<UserControl>();

        #endregion UserControl

        // TODO : 프로퍼티 에서 사용하는 private set; 기능이 무엇인지 꼭 확인하기 (2023.07.07 jbh)
        // private set; 의 역할 - 해당 필드 값을 읽기전용으로 쓰겠다는 것을 의미
        // 참고 URL - https://yeko90.tistory.com/entry/c-private-set-%EC%82%AC%EC%9A%A9-%EC%9D%B4%EC%9C%A0%ED%94%84%EB%9F%AC%ED%8D%BC%ED%8B%B0

        #endregion 프로퍼티 

        #region 생성자

        // TODO: 생성자 MainVM 로직 보완 (2023.07.04 jbh)
        public TestMainVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;
            // this.mainVM = this;

            // UserControl Command
            // TODO : RibbonCommand "this.TestVMCommand", "this.TestExpVMCommand", "this.TestLazyTreeVMCommand" 필요시 사용 예정 (2023.11.07 jbh)
            // this.TestVMCommand = new RibbonCommand(this, Container);
            this.TestExpVMCommand = new RibbonCommand(this, Container);
            // this.TestLazyTreeVMCommand = new RibbonCommand(this, Container);
            this.ExpVMCommand = new RibbonCommand(this, Container);

            // TitleBar
            this.MainTitle = CobimHelper.MainTitle;

            // Ribbon Text
            this.CDE_RibbonTab = CobimHelper.CDE_RibbonTab;     // RibbonTab

            this.CDE_RibbonGroup = CobimHelper.CDE_RibbonGroup;   // RibbonGroup

            this.CDE_RibbonButton = CobimHelper.CDE_RibbonButton;  // RibbonButton

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

        //protected override void OnActivate()
        //{
        //    base.OnActivate();

        //    // TODO : 최소화 버튼 클릭 후 화면 사라짐. -> 최소화 버튼 다시 클릭 -> 화면 활성화될 때 메서드 "OnActivate" 활성화 될때, 
        //    //        아래처럼 EventAggregator 구독 (Subscribe)을 다시 해주도록 구현 (2023.11.09 jbh)
        //    //        EventAggregator 구독 (Subscribe)을 해주는 이유는
        //    //        아래처럼 구독을 다시 안 하게 되면 리본 컨트롤에서 버튼 클릭시 화면이 제대로 이동하지 못하고 멈추기 때문이다. 
        //    // EventAggregator.Subscribe(this);   // EventAggregator 구독 (Subscribe)

        //}

        //protected override void OnDeactivate()
        //{
        //    base.OnDeactivate();

        //    //DoDeactivate();

        //    EventAggregator.Unsubscribe(this);
        //}

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

            // TODO : 닫기("X") 버튼 클릭 시 프로그램 종료 로직 구현 (2023.08.18 jbh)
            // 참고 URL - https://insurang.tistory.com/entry/WPF-C-4%EA%B0%80%EC%A7%80-%ED%94%84%EB%A1%9C%EA%B7%B8%EB%9E%A8-%EC%A2%85%EB%A3%8C-%EC%BD%94%EB%93%9C%EB%A5%BC-%EC%83%88%EC%B0%BD%EC%97%90%EC%84%9C-Close-Exit-Kill
            if(false == vm.IsLogin && true == vm.IsExit)
            {
                Environment.Exit(0);   // 탐색기 응용 프로그램 종료

                // Application.Current.Shutdown();                          // 현재창(로그인 화면 뷰 LoginV.xaml) 종료 
                // Application.Current.MainWindow.Close();                  // 메인창(메인 화면 뷰 MainV.xaml) 종료
                // System.Diagnostics.Process.GetCurrentProcess().Kill();   // 탐색기 응용 프로그램 종료
            }
        }

        #endregion OpenLogin

        #region InitSetting

        private void InitSetting()
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // UserControl 
                // TODO : UserControl List 객체 UserControlItems 에 UserControl 객체 추가 (2023.11.06 jbh)
                // UserControlItems.Clear();       // List 객체 UserControlItems 초기화 처리 
                // UserControlItems.Add(StartV);
                // UserControlItems.Add(TestV);
                // UserControlItems.Add(TestExplorerV);
                // UserControlItems.Add(TestLazyTreeV);
                // UserControlItems.Add(ExplorerV);

                var vms = Container.GetAll<IPage>().ToArray();   // <IPage> 인터페이스를 상속 받는 모든 클래스 요소들을 컨테이너에서 가져와서(Container.GetAll)

                this.Items.AddRange(vms);
                // var start = this.Items.Where(it => string.Equals(it.GetType().Name, nameof(StartVM))).FirstOrDefault();
                var start = this.Items.Where(it => string.Equals(it.GetType().Name, CobimHelper.StartVMName)).FirstOrDefault();

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if(start is not null)
                {
                    // "StartVM" 뷰모델과 연동된 초기화면 뷰(StartV)를 열어준다.
                    this.ActivateItem(start);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // Application.Current.Shutdown();
            }
        }

        #endregion InitSetting

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
        //            case CobimHelper.StartVMName:
        //                // TODO : UserControl 객체 생성 -> 화면 뷰 "StartV" 출력 및 뷰모델 "StartVM" 연동 하도록 구현 (2023.11.06 jbh)
        //                // 참고 URL - https://www.c-sharpcorner.com/article/use-of-user-controls-in-wpf/
        //                // UserControl = new UserControl { Content = StartV, DataContext = (StartVM)message.obj };

        //                // TODO : 아래 코드(UserControl.Content, UserControl.DataContext) 사용시 오류 발생하면 위에 주석 처리된 코드
        //                //        UserControl = new UserControl { Content = StartV, DataContext = (StartVM)message.obj }; 사용해서 수정 예정 (2023.11.06 jbh)
        //                Page.Content = StartV;
        //                Page.DataContext = (StartVM)message.obj;


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
        public void Handle(ChangeViewModelMsg message)
        {
            // TODO : 아래 주석 친 UserControl Content 초기화 코드 필요시 사용 예정 (2023.12.22 jbh)
            // Page.Content     = null;   // UserControl Content 초기화
            Page.DataContext = null;   // UserControl DataContext 초기화

            foreach (var item in message.Items)
            {
                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if (item.GetType().Name.Equals(message.VMName) && message?.VMName is not null)
                {
                    // TODO : 아래 테스트 코드 추후 삭제 예정 (2023.07.26 jbh)
                    //var changeVM = message.Items.Where(it => string.Equals(it.GetType().Name, message.VMName)).FirstOrDefault();
                    //this.ActivateItem(changeVM);

                    Page.DataContext = message.obj;

                    

                    // TODO : 필요시 메서드 "DisplaySetting" 사용 예정 (2023.12.22 jbh)
                    // DisplaySetting(message);

                    // TODO : 필요시 리소스 해제 메서드 "UserControlIDispose" 사용 예정 (2023.11.06 jbh)
                    // UserControlIDispose(Page);
                }
            }

            //foreach(var item in this.Items)
            //{
                  // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                  // 참고 URL - https://husk321.tistory.com/405
            //    if(item.GetType().Name.Equals(message.VMName) && message?.VMName is not null)
            //    {
            //        this.ActivateItem(item);

            //        bool change = true;

            //        DisplaySetting(change);
            //    }

            //}
        }

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
        //        // "StartViewModel" 뷰모델과 연동된 초기화면 뷰(StartView)를 열어준다.
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

        #region Sample

        #endregion Sample
    }
}
