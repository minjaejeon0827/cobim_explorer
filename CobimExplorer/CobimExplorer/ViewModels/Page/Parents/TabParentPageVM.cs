using Serilog;
using Stylet;
using StyletIoC;

using System;
using System.Linq;
using System.Reflection;

using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.Extensions;
using CobimExplorer.Common.LogManager;

using CobimExplorer.Interface;
using CobimExplorer.Interface.Tab;
using CobimExplorer.Interface.Page;

using CobimExplorer.Services.Page;

using CobimExplorer.ViewModels.Tab.Child;
using CobimExplorer.ViewModels.Message;
using CobimExplorer.ViewModels.Page.Start;
using CobimNet.Extensions;

namespace CobimExplorer.ViewModels.Page.Parents
{
    // TODO : 리본 버튼 클릭시 출력되는 화면을 탭 페이지로 출력 + 버튼 "닫기" 클릭시 탭 페이지 화면 종료 되도록 구현 (2024.01.10 jbh)  
    // 참고 URL - https://youtu.be/rFhmTccRUpQ?si=dNYuEcN6487nTeYt  
    public class TabParentPageVM : ParentOnePageBase, IHandle
    {
        #region 프로퍼티 

        #region EnumTabItem

        private enum EnumTabItem : int
        {
            [System.ComponentModel.Description("TabItem 부재")]
            NONE = 0,

            [System.ComponentModel.Description("TabItem 존재")]
            EXISTENCE = 1
        }

        #endregion EnumTabItem

        #region ChangeActiveItem

        /// <summary>
        /// 이벤트 ChangeActiveItem 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        private TabParentPageVM Sender { get; set; }

        #endregion ChangeActiveItem

        #region Test

        /// <summary>
        /// 테스트용 탭 생성 Command (테스트 화면 - TestExplorerVM)
        /// </summary>
        public ICommand NewTabCommand { get; set; }

        /// <summary>
        /// 테스트용 메인 뷰모델 (MainVM) 객체 
        /// </summary>
        private MainVM mainVM { get; set; }

        /// <summary>
        /// 테스트 TabItem 신규 추가 여부 (TabItem Content 영역 화면 새로 출력) 
        /// 테스트 출력 대상 화면 - 탐색기 화면, 웹브라우저 화면
        /// </summary>
        public bool IsTestNewTabItem { get; set; }
        //public bool IsTestNewTabItem { get => _IsTestNewTabItem; set { _IsTestNewTabItem = value; NotifyOfPropertyChange(nameof(_IsTestNewTabItem)); } }
        //private bool _IsTestNewTabItem = false;

        #endregion Test

        #region NavigationBar

        #region Command

        /// <summary>
        /// Button Command (NavigationBarControl 출력 또는 숨김 처리)
        /// </summary>
        public ICommand NavBarCommand { get; set; }

        /// <summary>
        /// Button Command (테스트 화면 - TestExplorerVM)
        /// </summary>
        public ICommand TestExpVMCommand { get; set; }

        /// <summary>
        /// Button Command (탐색기 화면 - ExplorerVM)
        /// </summary>
        public ICommand ExpVMCommand { get; set; }

        /// <summary>
        /// Button Command (HubBox 화면 - HubBoxVM)
        /// </summary>
        public ICommand HubBoxVMCommand { get; set; }

        #endregion Command

        /// <summary>
        /// Visibility - NavigationBarControl 출력 또는 숨김 처리
        /// </summary>
        public Visibility visibleNavBar { get => _visibleNavBar; set { _visibleNavBar = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleNavBar = Visibility.Collapsed;

        #endregion NavigationBar

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

        /// <summary>
        /// HubBox 리본 그룹 - Header
        /// </summary>
        public string HubBox_RibbonGroup { get => _HubBox_RibbonGroup; set { _HubBox_RibbonGroup = value; NotifyOfPropertyChange(); } }
        private string _HubBox_RibbonGroup;

        /// <summary>
        /// HubBox 리본 버튼 - Text
        /// </summary>
        public string HubBox_RibbonButton { get => _HubBox_RibbonButton; set { _HubBox_RibbonButton = value; NotifyOfPropertyChange(); } }
        private string _HubBox_RibbonButton;

        /// <summary>
        /// CDE 내비게이션 바 버튼 - Text
        /// </summary>
        public string CDE_NavButton { get => _CDE_NavButton; set { _CDE_NavButton = value; NotifyOfPropertyChange(); } }
        private string _CDE_NavButton;

        /// <summary>
        /// HubBox 내비게이션 바 버튼 - Text
        /// </summary>
        public string HubBox_NavButton { get => _HubBox_NavButton; set { _HubBox_NavButton = value; NotifyOfPropertyChange(); } }
        private string _HubBox_NavButton;

        #endregion Ribbon

        #region UserControl

        /// <summary>
        /// 시작화면 뷰모델 (싱글톤 객체)
        /// 용도 - TabParentPageV.xaml 파일 XAML 코드 중 <ContentControl> 태그에서 
        ///        해당 뷰모델을 바인딩 하여 탐색기 응용 프로그램 로그인 성공 후 
        ///        TabParentPageV.xaml 화면에서 시작화면 뷰(StartV.xaml)가 출력 되도록 하기 위해 사용 
        /// </summary>
        [Inject]
        public StartVM startVM { get; set; }

        #endregion UserControl

        #region TabControl

        /// <summary>
        /// Visibility - TabItem 포함한 TabControl
        /// </summary>
        public Visibility visibleTabControl { get => _visibleTabControl; set { _visibleTabControl = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleTabControl = Visibility.Collapsed;

        // TODO : 아래 프로퍼티 "_Tabs", "Tabs" 구현한 소스코드 필요시 수정 예정 (2024.08.22 jbh)
        /// <summary>
        /// TabItem Collection 
        /// </summary>
        //public ICollection<ITab> Tabs { get; set; }
        //public ObservableCollection<ITab> Tabs { get; set; }
        //private readonly ObservableCollection<ITab> _Tabs;
        public ObservableCollection<ITab> Tabs { get => _Tabs; set { _Tabs = value; NotifyOfPropertyChange(); } }
        private ObservableCollection<ITab> _Tabs = new ObservableCollection<ITab>();

        // TODO : 프로퍼티 "Items" 필요시 사용 예정
        /// <summary>
        /// 인터페이스 IPage를 상속받는 뷰모델 목록을 담는 Collection
        /// </summary>
        // public IObservableCollection<IPage> Items { get; } = new BindableCollection<IPage>();
        // IObservableCollection<IPage> Items { get; }


        /// <summary>
        /// TabItem Header 이름 리스트 
        /// </summary>
        public List<string> TabItemNameList { get => _TabItemNameList; set { _TabItemNameList = value; NotifyOfPropertyChange(); } }
        private List<string> _TabItemNameList = new List<string>();

        /// <summary>
        /// 리본 버튼 클릭시 화면상에 활성화 되는 탭 인덱스        
        /// </summary>
        public int SelectedTabIndex { get => _SelectedTabIndex; set { _SelectedTabIndex = value; NotifyOfPropertyChange(); } }
        private int _SelectedTabIndex;

        /// <summary>
        /// 탐색기 화면에 출력된 TabItem
        /// </summary>
        private ITab TabItem { get; set; }

        /// <summary>
        /// 탭 목록 순서가 바뀌어지는 TabItem
        /// </summary>
        private ITab SourceTabItem { get; set; }

        /// <summary>
        /// 탭 목록 순서를 바꾸려고 하는 TabItem
        /// </summary>
        private ITab TargetTabItem { get; set; }

        #endregion TabControl

        #endregion 프로퍼티 

        #region 생성자 

        public TabParentPageVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2024.01.10 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;

            NavBarCommand = new ButtonCommand(CobimHelper.VoidMethodType, DisplayNavBar, CanExecuteMethod);
            NewTabCommand = new ButtonCommand(CobimHelper.VoidMethodType, NewTabItem, CanExecuteMethod);
            
            TestExpVMCommand = new ButtonCommand(CobimHelper.VoidMethodType, NewTabItem, CanExecuteMethod);
            ExpVMCommand = new ButtonCommand(CobimHelper.VoidMethodType, NewTabItem, CanExecuteMethod);
            HubBoxVMCommand = new ButtonCommand(CobimHelper.VoidMethodType, NewTabItem, CanExecuteMethod);

            // NavigationBar Display Setting
            visibleNavBar = Visibility.Visible;

            // Ribbon Text
            this._CDE_RibbonTab = CobimHelper.CDE_RibbonTab;               // RibbonTab

            this._CDE_RibbonGroup = CobimHelper.CDE_RibbonGroup;           // CDE - RibbonGroup

            this._CDE_RibbonButton = CobimHelper.CDE_RibbonButton;         // CDE - RibbonButton

            this._HubBox_RibbonGroup = CobimHelper.HubBox_RibbonGroup;     // HubBox - RibbonGroup

            this._HubBox_RibbonButton = CobimHelper.HubBox_RibbonButton;   // HubBox - RibbonButton

            // NavigationBar Text
            this._CDE_NavButton = CobimHelper.CDE_NavButton;               // CDE - NavigationBar Button
            this._HubBox_NavButton = CobimHelper.HubBox_NavButton;         // HubBox - NavigationBar Button

            // TODO : 아래 프로퍼티 "_Tabs", "Tabs" 사용하여 이벤트 CollectionChanged 등록한 소스코드 필요시 수정 예정 (2024.08.22 jbh)
            // this._Tabs = new ObservableCollection<ITab>();
            // this._Tabs.CollectionChanged -= Tabs_CollectionChanged;
            this._Tabs.CollectionChanged += Tabs_CollectionChanged;
            // this.Tabs = _Tabs;

            if(true == IsTestNewTabItem) InitSetting();   // TabParentPage 화면 초기화 
        }

        #endregion 생성자 

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            // EventAggregator.Subscribe(this);   // EventAggregator 구독 (Subscribe)
        }

        /// <summary>
        /// 리본 버튼 클릭시 화면상에 TabItem으로 신규 추가되어 활성화 되는 뷰모델 이벤트 
        /// </summary>
        protected override void ChangeActiveItem(IPage newItem, bool closePrevious)
        {
            Sender = this;

            PageBase pageBase = (PageBase)newItem;
            if(pageBase is not null)
            {
                //IEventAggregator eventAggregator = this.EventAggregator;
                OpenPageMsg message = new OpenPageMsg((ICbVM)Sender);

                // TabParentPageVM.cs -> 이벤트 메서드 "ChangeActiveItem" 실행시
                // -> OpenPageMsg 클래스 객체 message 생성하는 경우에만 아래 코드 2줄 필요 (2024.08.07 jbh)
                // -> 아래 코드 2줄은 MainVM.cs -> 메서드 Handle(OpenPageMsg message) 에서 사용하는 데이터임.
                // -> message.Page = newItem; / message.VMName = message.Page.GetType().Name; 
                message.Page = newItem;
                message.VMName = message.Page.GetType().Name;

                string[] strArray = Array.Empty<string>();
                //eventAggregator.PublishOnUIThread((object)message, strArray);
                EventAggregator.PublishOnUIThread((object)message, strArray);
            }

            base.ChangeActiveItem(newItem, closePrevious);
        }

        // TODO : 테스트 해보고 필요 하면 이벤트 OnActivate 사용 예정 (2024.08.08 jbh)
        //protected override void OnActivate()
        //{
        //    base.OnActivate();

        //    // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.08.07 jbh)
        //    //var vm = Container.Get<TabParentPageVM>();
        //    //string VMName = vm.GetViewModelName();

        //    //var msg = new ChangeViewModelMsg(this, VMName);
        //    //new OpenPageMsg
        //    //EventAggregator.PublishOnUIThread(msg);
        //}

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }


        // TODO : 메서드 "CanExecuteMethod" 필요시 수정 예정 (2024.08.08 jbh)
        private bool CanExecuteMethod(object obj)
        {
            return true;
        }

        #endregion 기본 메소드 

        #region InitSetting

        /// <summary>
        /// TabParentPage 화면 초기화 
        /// </summary>
        public void InitSetting()
        {
            // List<string> VMNameList = new List<string>();        // 테스트 TabItem 신규 추가 하고자 하는 뷰모델 이름 리스트 초기화 (탐색기 화면, 웹브라우저 화면)
            // List<string> tabNameList = new List<string>();       // 테스트 TabItem 이름 리스트 초기화("성과품 조회", "ACC 변환 내역")
            List<Dictionary<string, string>> tabItemList = new List<Dictionary<string, string>>();       // 테스트 TabItem 리스트 (Dictionary 유형 key - "뷰모델 이름" / value = "TabItem 이름") 초기화

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                var vms = Container.GetAll<IPage>().ToArray();   // <IPageBase> 인터페이스를 상속 받는 모든 클래스 요소들을 컨테이너에서 가져와서(Container.GetAll)

                this.Items.AddRange(vms);

                visibleTabControl = Visibility.Collapsed;        // 화면 활성화될 때 마다 Grid 영역 -> TabControl 숨김 처리

                // TODO : 테스트 TabItem 리스트 객체 tabItemList 에 Dictionary<string, string>() 형식 데이터 추가 구현 (2024.10.28 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.list-1.add?view=net-8.0#system-collections-generic-list-1-add(-0)
                // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0
                // 참고 3 URL - https://developer-talk.tistory.com/693
                tabItemList.Add(new Dictionary<string, string>() { { CobimHelper.ExplorerVMName, CobimHelper.ExplorerTabItemName } });
                tabItemList.Add(new Dictionary<string, string>() { { CobimHelper.HubBoxVMName, CobimHelper.HubBoxTabItemName } });

                TestNewTabItem(tabItemList);   // 테스트 TabItem 신규 추가 메서드 TestNewTabItem 호출 

                // TODO : 아래 주석친 코드 필요시 참고 (2024.08.06 jbh)
                // TODO : 아래 VMNameList 데이터 추가 및 메서드 "TestNewTabItem" 호출문은 테스트 끝나고 나면 코드 삭제 처리 예정 (2024.08.07 jbh)
                // tabNameList.Add(CobimHelper.ExplorerTabItemName);
                // tabNameList.Add(CobimHelper.HubBoxTabItemName);
                //TestNewTabItem(tabNameList);   // 테스트 TabItem 신규 추가 메서드 TestNewTabItem 호출 

                // VMNameList.Add(CobimHelper.ExplorerVMName);
                // VMNameList.Add(CobimHelper.HubBoxVMName);

                // TODO : TestCefSharpVM - 구글 크롬(Google Chrome) 웹 브라우저 필요시 아래 주석친 코드 참고 (2024.08.22 jbh)
                // VMNameList.Add(CobimHelper.TestCefSharpVMName);

                // TestNewTabItem(VMNameList);   // 테스트 TabItem 신규 추가 메서드 TestNewTabItem 호출 
                // var start = this.Items.Where(it => string.Equals(it.GetType().Name, CobimHelper.StartVMName)).FirstOrDefault();

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.08.06 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                // if(start is not null)
                // {
                //     // "StartVM" 뷰모델과 연동된 초기화면 뷰(StartV)를 열어준다.
                //     this.ActivateItem(start);
                // }
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // Application.Current.Shutdown();
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion InitSetting

        #region TestNewTabItem 

        /// <summary>
        /// 테스트 TabItem 신규 추가(TabItem Content 영역 화면 새로 출력) 
        /// 테스트 출력 대상 화면 - 탐색기 화면, 구글 크롬(Google Chrome) 웹 브라우저 화면
        /// </summary>
        //private void TestNewTabItem(List<string> pVMNameList)
        private void TestNewTabItem(List<Dictionary<string, string>> pTabItemList)
        {
            // int fileCnt = 0;                                     // 화면상 중복된 뷰/뷰모델 출력시 TabItem Header 문자열에 붙일 숫자(N) 

            string vmName = string.Empty;                        // TabItem 뷰모델 이름 문자열 초기화
            string tabItemName = string.Empty;                   // TabItem Header 문자열 초기화

            IPage openPage = null;                               // TabControl Content 영역 안에 새롭게 열고자 하는 화면

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 추후 메서드 "TestNewTabItem" 필요시 아래 MessageBox.Show / return; 삭제 예정 (2024.08.06 jbh)
                // MessageBox.Show("TabItem 신규 추가 추후 구현 예정");
                // return;

                //if(CobimHelper.VMNameExist < pVMNameList.Count)
                //if(CobimHelper.TabItemNameExist < pVMNameList.Count)
                if(CobimHelper.TabItemExist < pTabItemList.Count)
                {
                    foreach(Dictionary<string, string> tabItem in pTabItemList)
                    {
                        vmName = tabItem.Keys.FirstOrDefault();
                        tabItemName = tabItem.Values.FirstOrDefault();

                        openPage = this.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();

                        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.08.06 jbh)
                        // 참고 URL - https://husk321.tistory.com/405
                        if(openPage is not null)
                        {
                            // openPage 뷰모델(openPage ViewModel)과 연동된 초기화면 뷰(openPage View)를 열어준다.
                            this.ActivateItem(openPage);
                        }

                        Tabs.Add(new ChildTabVM(EventAggregator, Container, vmName, tabItemName, this.ActiveItem));
                    }
                }
                else throw new Exception("[테스트 TabItem 화면 출력 오류]\r\n대상 화면 - 탐색기 / 웹브라우저 화면\r\n담당자에게 문의하세요.");


                // 화면상에 출력 되는 TabItem 인덱스 값 구하기 (탐색기 화면(인덱스 0) 출력 되도록 설정)
                //this.SelectedTabIndex = Tabs.Count - 1;
                this.SelectedTabIndex = CobimHelper.TestSelectedTabIndex;

                // TabItem 추가될 때 Grid 영역 -> TabControl이 숨김 처리 되어있는 경우 
                if(Tabs.Count >= (int)EnumTabItem.EXISTENCE
                   && Visibility.Collapsed == visibleTabControl) visibleTabControl = Visibility.Visible;


                // TODO : 아래 주석친 테스트 코드 참고(2024.08.07 jbh) 
                //vmName = obj.ToString();
                //tabItemName = vmName;

                //openPage = this.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();

                //// TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.08.06 jbh)
                //// 참고 URL - https://husk321.tistory.com/405
                //if (openPage is not null)
                //{
                //    // openPage 뷰모델(openPage ViewModel)과 연동된 초기화면 뷰(openPage View)를 열어준다.
                //    this.ActivateItem(openPage);
                //}

                //TabItemNameList.Clear();
                //TabItemNameList = Tabs.Select(tabInfo => tabInfo.TabItemName).ToList();

                //// TabItem Header 문자열 할당하기 
                //foreach (ITab tab in Tabs)
                //{
                //    // 리본 버튼으로 클릭해서 화면에 활성화 하고자 하는 뷰모델 이름이
                //    // TabItem Collection에 이미 존재하는 경우 
                //    if (true == tab.VMName.Equals(vmName))
                //    {
                //        // TabItem 이름 리스트 객체 TabItemNameList에 탭 이름(tabItemName)과
                //        // 동일한 탭 이름이 존재하지 않는 경우 반복문 탈출 
                //        if (false == TabItemNameList.Contains(tabItemName)) break;

                //        fileCnt++;
                //        tabItemName = vmName + $"-({fileCnt})";
                //    }
                //}

                //Tabs.Add(new ChildTabVM(EventAggregator, Container, vmName, tabItemName, this.ActiveItem));

                // 리본 버튼 클릭시 화면상에 활성화 되는 탭 인덱스 값 구하기 
                //this.SelectedTabIndex = Tabs.Count - 1;

                //// TabItem 추가될 때 Grid 영역 -> TabControl이 숨김 처리 되어있는 경우 
                //if (Tabs.Count >= (int)EnumTabItem.EXISTENCE
                //   && Visibility.Collapsed == visibleTabControl) visibleTabControl = Visibility.Visible;


            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion TestNewTabItem

        #region DisplayNavBar

        /// <summary>
        /// NavigationBarControl 출력 또는 숨김 처리
        /// </summary>
        private void DisplayNavBar(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                if(Visibility.Visible == visibleNavBar) visibleNavBar = Visibility.Collapsed;
                else visibleNavBar = Visibility.Visible;
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion DisplayNavBar

        #region NewTabItem

        /// <summary>
        /// TabItem 신규 추가(TabItem Content 영역 화면 새로 출력)
        /// </summary>
        private void NewTabItem(object obj)
        {
            int fileCnt = 0;                                     // 화면상 중복된 뷰/뷰모델 출력시 TabItem Header 문자열에 붙일 숫자(N) 
            string vmName = string.Empty;                        // 화면상 TabControl 안에 출력될 뷰모델 이름 문자열 초기화
            string tabItemName = string.Empty;                   // TabItem Header 문자열 초기화

            IPage openPage = null;                               // TabControl Content 영역 안에 새롭게 열고자 하는 화면           

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 추후 메서드 "NewTabItem" 필요시 아래 MessageBox.Show / return; 삭제 예정 (2024.08.06 jbh)
                //MessageBox.Show("TabItem 신규 추가 추후 구현 예정");
                return;

                vmName = obj.ToString();
                tabItemName = vmName;

                openPage = this.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.08.06 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if(openPage is not null)
                {
                    // openPage 뷰모델(openPage ViewModel)과 연동된 초기화면 뷰(openPage View)를 열어준다.
                    this.ActivateItem(openPage);
                }

                TabItemNameList.Clear();
                TabItemNameList = Tabs.Select(tabInfo => tabInfo.TabItemName).ToList();

                // TabItem Header 문자열 할당하기 
                foreach(ITab tab in Tabs)
                {
                    // 리본 버튼으로 클릭해서 화면에 활성화 하고자 하는 뷰모델 이름이
                    // TabItem Collection에 이미 존재하는 경우 
                    // if(true == tab.VMName.Equals(vmName))
                    if(tab.VMName == vmName)
                    {
                        // TabItem 이름 리스트 객체 TabItemNameList에 탭 이름(tabItemName)과
                        // 동일한 탭 이름이 존재하지 않는 경우 반복문 탈출 
                        if(false == TabItemNameList.Contains(tabItemName)) break;

                        fileCnt++;
                        tabItemName = vmName + $"-({fileCnt})";
                    }
                }

                Tabs.Add(new ChildTabVM(EventAggregator, Container, vmName, tabItemName, this.ActiveItem));

                // 리본 버튼 클릭시 화면상에 활성화 되는 탭 인덱스 값 구하기 
                this.SelectedTabIndex = Tabs.Count - 1;

                // TabItem 추가될 때 Grid 영역 -> TabControl이 숨김 처리 되어있는 경우 
                if(Tabs.Count >= (int)EnumTabItem.EXISTENCE
                   && Visibility.Collapsed == visibleTabControl) visibleTabControl = Visibility.Visible;

                // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.07.22 jbh)
                // this.mainVM = (MainVM)this.Parent;

                // Tabs.Where(x => x.TabItemName)

                // if(Tabs.Count >= 1)
                // {
                //     foreach(ITab tab in Tabs)
                //     {
                //         if (true == tab.VMName.Equals(vmName)) fileCnt++;
                //     }
                // }

                // foreach (ITab tab in Tabs)
                // {
                //     if(true == tab.VMName.Equals(vmName)
                //        && false == tab.TabItemName.Equals(tabItemName))
                //     {
                //         fileCnt++;
                //         tabItemName = vmName + $"-({fileCnt})";
                //     }
                // }

                // if (0 == fileCnt) tabItemName = vmName;
                // else tabItemName = Tabs.Where(x => x.TabItemName.Equals(tabItemName)).Select(x => x.VMName).FirstOrDefault() + $"-({fileCnt + 1})";


                // if (tab is null) 
                // {
                //     var activeVM = mainVM.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();
                // 
                //     Tabs.Add(new DateTabVM(obj, activeVM));
                // } 

                // TabItemName이 존재 안 하면 새로운 TabItemName 설정
                // if(0 == fileCnt) tabItemName = vmName;

                // TabItemName이 존재하면 기존 TabItemName + (N)으로 TabItemName 변경 
                // if(fileCnt >= 1) 
                // {
                //     //tabItemName = vmName + $"-({fileCnt})";

                //     // 중복된 탭이름 가져오기 
                //     string duplicateTabItemName = Tabs.Where(tabInfo => tabInfo.TabItemName.Equals(tabItemName))
                //                                   .Select(tabInfo => tabInfo.TabItemName)
                //                                   .FirstOrDefault();

                //     // 중복된 탭이름 존재하는 경우  
                //     if(false == string.IsNullOrWhiteSpace(duplicateTabItemName))
                //         tabItemName = vmName + $"-({fileCnt + 1})";
                // }

                // TabItemName이 존재 안 하면 새로운 TabItemName 설정 
                // else tabItemName = vmName;

                // var tab = Tabs.Where(x => x.VMName.Contains(vmName)).FirstOrDefault();

                // var activeVM = this.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();

                // Tabs.Add(new ChildTabVM(EventAggregator, Container, obj, this.ActiveItem));
                // Tabs.Add(new ChildTabVM(EventAggregator, Container, vmName, this.ActiveItem));
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion NewTabItem

        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2024.08.01 jbh)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2024.08.01 jbh)
        // 참고 URL - https://kukuta.tistory.com/359
        // C# Nullable 타입 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
        // C# Nullable 타입 예제 코드
#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고


        #region Tabs_CollectionChanged

        /// <summary>
        /// TabItem 닫기 요청 이벤트 핸들러 가입/해제 처리 이벤트 
        /// </summary>
        private void Tabs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    //TabItem = (ITab) e.NewItems[0];
                    TabItem = (ITab)e.NewItems[CobimHelper.TargetTabItemIndex];
                    TabItem.CloseRequested += OnTabCloseRequested;   // TabItem 닫기 요청 이벤트 핸들러 가입
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //TabItem = (ITab) e.OldItems[0];
                    TabItem = (ITab)e.OldItems[CobimHelper.TargetTabItemIndex];
                    TabItem.CloseRequested -= OnTabCloseRequested;   // TabItem 닫기 요청 이벤트 핸들러 해제
                    break;
            }
        }

        #endregion Tabs_CollectionChanged

        #region OnTabCloseRequested

        /// <summary>
        /// 버튼 "X" 클릭 후 TabItem 종료 이벤트  
        /// </summary>
        private void OnTabCloseRequested(object? sender, EventArgs e)
        {
            // TODO : 추후 이벤트 메서드 "OnTabCloseRequested" 필요시 아래 MessageBox.Show / return; 삭제 예정 (2024.08.06 jbh)
            //MessageBox.Show("TabItem 버튼 X 기능 추후 구현 예정");
            return;

            Tabs.Remove((ITab)sender);

            // TabItem 전부다 삭제 처리 후 Grid 영역 -> TabControl이 보임 처리(Visibility.Visible) 되어있는 경우 
            if(Tabs.Count == (int)EnumTabItem.NONE && Visibility.Visible == visibleTabControl) visibleTabControl = Visibility.Collapsed;   // TabControl 숨김 처리(Visibility.Collapsed) 진행
        }

        #endregion OnTabCloseRequested

        #region TabItem_Drop

        /// <summary>
        /// TabItem 마우스 Drop 이벤트 
        /// TabItem 들끼리 위치 Swap 처리 가능 
        /// </summary>
        public void TabItem_Drop(object sender, DragEventArgs e)
        {
            TabItem tabItemSource = null;   // 탭 목록 순서가 바뀌어지는 TabItem
            TabItem tabItemTarget = null;   // 탭 목록 순서를 바꾸려고 하는 TabItem

            int sourceIndex = 0;            // 탭 목록 순서가 바뀌어지는 tabItemSource 인덱스 
            int targetIndex = 0;            // 탭 목록 순서를 바꾸려고 하는 tabItemTarget 인덱스

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                tabItemSource = GetSourceTabItem(e.OriginalSource);

                // 탭 목록 순서가 바뀌어지는 TabItem(tabItemSource) 존재하는 경우
                if (tabItemSource is not null)
                {
                    tabItemTarget = (TabItem)e.Data.GetData(typeof(TabItem));

                    if (tabItemTarget != tabItemSource)
                    {
                        sourceIndex = Tabs.IndexOf((ITab)tabItemSource.Content);
                        targetIndex = Tabs.IndexOf((ITab)tabItemTarget.Content);

                        // TODO : 메서드 Tabs.Remove((ITab)tabItemSource.Content); 또는 Tabs.Remove((ITab)tabItemTarget.Content); 호출시 
                        //        두가지 TabItem (ITab)tabItemSource.Content, (ITab)tabItemTarget.Content 이 DisConnect 처리되어 
                        //        메서드 Tabs.Insert 호출시 아래와 같은 오류 메시지 출력
                        //        'Unable to cast object of type 'MS.Internal.NamedObject' to type 'CobimExplorer.Interface.Page.ITab'.' 
                        //        해당 오류 해결하기 위해 해당 두가지 TabItem을 메서드 Tabs.Remove 호출하기 직전
                        //        ITab 인터페이스 프로퍼티 "SourceTabItem", "TargetTabItem"에 값 할당 처리 (2024.08.05 jbh)
                        // 참고 URL - https://stackoverflow.com/questions/18866308/unable-to-cast-object-of-type-ms-internal-namedobject-to-bitmapimage
                        SourceTabItem = (ITab)tabItemSource.Content;   // 탭 목록 순서가 바뀌어지는 SourceTabItem
                        TargetTabItem = (ITab)tabItemTarget.Content;   // 탭 목록 순서를 바꾸려고 하는 TargetTabItem


                        Tabs.Remove((ITab)tabItemSource.Content);      // tabItemSource 제거 
                        Tabs.Insert(targetIndex, SourceTabItem);       // SourceTabItem 추가 

                        Tabs.Remove((ITab)tabItemTarget.Content);      // tabItemTarget 제거 
                        Tabs.Insert(sourceIndex, TargetTabItem);       // TargetTabItem 추가 

                        SelectedTabIndex = sourceIndex;  // sourceIndex에 추가된 TargetTabItem이 화면에 출력 되도록 설정 
                    }
                }
                // TODO : 아래 else 문 필요시 구현 예정 (2024.08.08 jbh)
                // 탭 목록 순서가 바뀌어지는 TabItem(tabItemSource) 존재하지 않는 경우
                // else throw new Exception("[오류]\r\n탭 목록 순서가 바뀌어지는 TabItem(tabItemSource)\r\nnull Exception 발생!\r\n담당자에게 문의하세요!");


                // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.07.23 jbh)
                // var tabItemTarget = e.Source as TabItem;
                // var tabItemSource = e.Data.GetData(typeof(TabItem)) as TabItem;

                // TODO : TabItem Header를 마우스로 드래그 앤 드롭 진행시 이벤트 메서드 TabItem_Drop 실행되고, 
                //        아래 if절 로직의 조건절 if(false == tabItemTarget.Equals(tabItemSource))에서 오류 발생함. (2024.07.25 jbh)
                // if(false == tabItemTarget.Equals(tabItemSource))
                // {
                //     int sourceIndex = tabControl.Items.IndexOf(tabItemSource);
                //     int targetIndex = tabControl.Items.IndexOf(tabItemTarget);
                // 
                //     tabControl.Items.Remove(tabItemSource);
                //     tabControl.Items.Insert(targetIndex, tabItemSource);
                // 
                //     tabControl.Items.Remove(tabItemTarget);
                //     tabControl.Items.Insert(sourceIndex, tabItemTarget);
                // 
                //     tabControl.SelectedIndex = targetIndex;
                // }
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion TabItem_Drop

        #region GetSourceTabItem

        /// <summary>
        /// 탭 목록 순서가 바뀌어지는 TabItem 얻기 
        /// </summary>
        private TabItem GetSourceTabItem(object pOrgSource)
        {
            // TODO : DependencyObject 클래스 객체 currentSource에 null을 할당할 수 있도록 물음표 연산(?)을 붙여서 Nullable 타입으로 변환 처리 (2024.08.05 jbh)
            // 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
            // DependencyObject currentSource = null;               // 탭 목록 순서가 바뀌어지는 TabItem을 DependencyObject으로 casting 후 값을 할당 받는 객체 
            DependencyObject? currentSource = null;               // 탭 목록 순서가 바뀌어지는 TabItem을 DependencyObject으로 casting 후 값을 할당 받는 객체 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                if(pOrgSource is Run) currentSource = ((Run)pOrgSource).Parent;
                else currentSource = pOrgSource as DependencyObject;

                // currentSource = pOrgSource as DependencyObject;

                // 탭 목록 순서가 바뀌어지는 TabItem이 존재하는 경우 
                // while 반복문 실행 
                while(currentSource is not null)
                {
                    var tabItem = currentSource as TabItem;

                    // tabItem을 찾은 경우 
                    if (tabItem is not null) return tabItem;

                    // currentSource = VisualTreeHelper.GetParent(currentSource);

                    currentSource = currentSource.FindTabItemParent<TabItem>();   // TabItem의 부모 찾기 
                    //currentSource = currentSource.FindTabItemParent<TabItem>()!;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 필요시 throw; 문 사용 예정 (2024.08.08 jbh)
                //throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
            return null;
        }

        #endregion GetSourceTabItem

#nullable disable  // nullable 전처리 비활성화

        #region 이벤트 메소드 

        #region CobimLogo_MouseDown

        // TODO : <Canvas> 사용해서 <Canvas> 하위에 속한 <Image> 태그 - CobimLogo 이미지 객체(x:Name= "CobimLogoImg") 마우스로 클릭하여  
        //        CobimLogo 이미지 객체(x:Name= "CobimLogoImg") 위치 이동할 수 있는 이벤트 메서드(CobimLogo_MouseDown, CobimLogo_MouseMove, CobimLogo_MouseUp, CobimLogo_MouseLeave)  
        //        필요시 탭 화면 뷰모델(TabParentPageVM.cs)에 이벤트 메서드 구현 예정 (2024.09.13 jbh)  
        // 참고 URL - https://chatgpt.com/c/66e3cdcc-cd34-8002-9c0a-a55ab4b06e94  -->
        // 참고 2 URL - https://westahn.com/wpf-panel-canvas-%EC%82%AC%EC%9A%A9%EB%B2%95-%EB%B0%8F-%EC%98%88%EC%A0%9C/  -->
        // 참고 3 URL - https://wpf-tutorial.com/ko/24/panels/canvas-%EC%BB%A8%ED%8A%B8%EB%A1%A4/  -->
        // 참고 4 URL - https://www.codeproject.com/KB/WPF/442276/DragAndDropControls.zip  -->
        // 참고 5 URL - https://crystalcube.co.kr/157  -->
        // 참고 6 URL - https://insurang.tistory.com/entry/WPF-C-%EB%8F%84%ED%98%95-%EB%93%9C%EB%9E%98%EA%B7%B8-%EC%9D%B4%EB%8F%99-Element-Move-DragMove-Button-Marjin  -->

        /// <summary>
        /// 탭 화면 뷰(TabParentPageV.xaml) - CobimLogo 이미지 객체(x:Name="CobimLogoImg") MouseDown 이벤트
        /// </summary>
        //public void CobimLogo_MouseDown(object sender, MouseEventArgs e)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        Image test = sender as Image;
        //        var mainV = test.FindParent<Window>();
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //}

        #endregion CobimLogo_MouseDown

        #region CobimLogo_MouseMove

        /// <summary>
        /// 탭 화면 뷰(TabParentPageV.xaml) - CobimLogo 이미지 객체(x:Name="CobimLogoImg") MouseMove 이벤트
        /// </summary>
        //public void CobimLogo_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        Image test = sender as Image;
        //        var mainV = test.FindParent<Window>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //}

        #endregion CobimLogo_MouseMove

        #region CobimLogo_MouseUp

        /// <summary>
        /// 탭 화면 뷰(TabParentPageV.xaml) - CobimLogo 이미지 객체(x:Name="CobimLogoImg") MouseUp 이벤트
        /// </summary>
        //public void CobimLogo_MouseUp(object sender, MouseEventArgs e)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        Image test = sender as Image;
        //        var mainV = test.FindParent<Window>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //}

        #endregion CobimLogo_MouseUp

        #region CobimLogo_MouseLeave

        /// <summary>
        /// 탭 화면 뷰(TabParentPageV.xaml) - CobimLogo 이미지 객체(x:Name="CobimLogoImg") MouseLeave 이벤트
        /// </summary>
        //public void CobimLogo_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        Image test = sender as Image;
        //        var mainV = test.FindParent<Window>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //}

        #endregion CobimLogo_MouseLeave

        #endregion 이벤트 메소드

        #region Sample

        #endregion Sample
    }
}
