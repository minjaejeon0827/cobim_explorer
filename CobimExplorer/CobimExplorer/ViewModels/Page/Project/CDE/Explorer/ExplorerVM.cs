using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.Http;
using System.Collections;
using System.Reflection;
// using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Input;

using System.Threading.Tasks;
using System.Collections.Generic;
using CobimExplorer.Services.Page;
using CobimExplorer.Interface.Page;
using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.Extensions;
using CobimExplorer.Common.LogManager;

using CobimExplorer.Rest.Api.CobimBase.Explorer;
using CobimExplorer.Rest.Api.CobimBase.HubBox;
using CobimExplorer.ViewModels.Windows.Login;
using CobimExplorer.ViewModels.Part.Project.CDE.Explorer;
using CobimExplorer.Models.User;
using CobimExplorer.Settings;
using CobimExplorer.Data.Models.CobimBase.Main;
using CobimExplorer.Data.Models.CobimBase.Explorer;
using CobimExplorer.Data.Models.CobimBase.Login;
using CobimExplorer.Data.Models.CobimBase.HubBox;
using CobimExplorer.Controls;
using CobimExplorer.Views.Windows.LoadingWork;
using CobimExplorer.ViewModels.Windows.LoadingWork;
using CobimExplorer.ViewModels.Message;
using CobimExplorer.Interface;
using CobimExplorer.Utils.String;

namespace CobimExplorer.ViewModels.Page.Project.CDE.Explorer
{
    // TODO : 데이터 모델 클래스 "ExplorerView.cs" + 파이썬 Chat GPT API 사용해서 CobimExplorer 응용 프로그램을 설명해주는 챗봇 구현 예정 (2024.01.03 minjae)
    // 참고 URL - https://topnanis.tistory.com/237
    // 참고 2 URL - https://www.innofactory.net/blog/posts/sseung/20231030-chatgptapi-3/
    public class ExplorerVM : PageBase<ExplorerView>, IPage
    {
        #region 프로퍼티 

        // TODO : 필요시 프로퍼티 "WindowManager" 사용 예정 (2024.01.04 minjae) 
        //[Inject]
        //public IWindowManager WindowManager { get; set; }

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 minjae) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 minjae) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        #region OnActivate

        // TODO : 아래 프로퍼티 "Sender" 필요시 참고 (2024.08.07 minjae)
        /// <summary>
        /// 이벤트 OnActivate 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        // private ExplorerVM Sender { get; set; }

        #endregion OnActivate

        #region Command

        // TODO : CreateCommand - 파일(컨테이너) 신규 등록의 경우 COBIM 웹 사이트(인재 INC)에서만 사용하는 기능이고 CDE 탐색기 응용 프로그램에서는 사용 안 하는 걸로 소통 완료.
        //        하여 해당 파일(컨테이너) 신규 등록 메서드는 주석 처리하고 사용 안 함. (2023.10.19 minjae)
        /// <summary>
        /// 신규등록 Command
        /// </summary>
        // public ICommand CreateCommand { get; set; }

        /// <summary>
        /// 컨테이너(파일 포함) 목록 조회 Command
        /// </summary>
        public ICommand ContainerCommand { get; set; }

        /// <summary>
        /// 사용자가 선택한 컨테이너(파일 포함) 데이터 로우 목록 SelectionChanged 이벤트 Command
        /// </summary>
        public ICommand SelectedContainerCommand { get; set; }

        // TODO : 인터페이스 ICommand 객체 "FileAddCommand" 필요 없을 시 삭제 예정 (2023.09.21 minjae) 
        /// <summary>
        /// 추가 (파일 등록) Command
        /// </summary>
        public ICommand FileAddCommand { get; set; }

        /// <summary>
        /// 보기 (특정 컨테이너에 속한 파일 변환해서 웹뷰어로 출력) Command
        /// </summary>
        //public ICommand WebViewerCommand { get; set; }
        public ICommand ShowWebViewerCommand { get; set; }

        /// <summary>
        /// 파일 기본정보 - 컨테이너 정보 조회 Command
        /// </summary>
        public ICommand DetailCommand { get; set; }

        /// <summary>
        /// 검토요청 Command
        /// </summary>
        public ICommand RequestCommand { get; set; }

        /// <summary>
        /// 수정 Command
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        /// <summary>
        /// 다운로드 Command
        /// </summary>
        public ICommand DownLoadCommand { get; set; }

        /// <summary>
        /// 탭 "변경" - 컨테이너 변경(컨테이너 안에 저장한 파일 A -> B) 및 저장 Command
        /// </summary>
        public ICommand UploadCommand { get; set; }

        /// <summary>
        /// 연관 컨테이너 Command (참조 관계 - 부모/자식 컨테이너)
        /// </summary>
        public ICommand LinkedNodeInfoCommand { get; set; }

        // TODO : 테스트 Command 프로퍼티 "TestCommand" 필요시 사용 예정 (2024.09.03 minjae)
        /// <summary>
        /// 테스트 Command
        /// </summary>
        // public ICommand TestCommand { get; set; }

        /// <summary>
        /// 컨테이너에 속한 파일열기 Command
        /// </summary>
        public ICommand OpenFileCommand { get; set; }

        /// <summary>
        /// 체크아웃 Command
        /// </summary>
        public ICommand CheckOutCommand { get; set; }

        /// <summary>
        /// 체크인 Command
        /// </summary>
        public ICommand CheckInCommand { get; set; }

        /// <summary>
        /// 작업취소 Command
        /// </summary>
        public ICommand CancelWorkCommand { get; set; }

        // TODO : 컨테이너 잠금(락 - Lock / 비활성화)(ReadYn = "Y") 처리 Command 프로퍼티 "LockContainerCommand" 필요시 사용 예정 (2023.12.12 minjae)
        // TODO : ListView의 데이터 로우 중 체크아웃 완료한 특정 컨테이너 아이디가 잠금(락 - Lock / 비활성화)(ReadYn = "Y") 처리 되도록  
        //        이벤트 "SelectionChanged" 사용해서 해당 컨테이너 아이디를 선택 및 클릭하지 못 하도록 비활성화 처리 구현 (2023.12.12 minjae)  
        // 참고 URL - https://nonstop-antoine.tistory.com/7  
        /// <summary>
        /// 컨테이너 잠금(락 - Lock / 비활성화)(ReadYn = "Y") 처리 Command
        /// </summary>
        // public ICommand LockContainerCommand { get; set; }

        #endregion Command

        #region Title

        /// <summary>
        /// 탐색기 화면 Title
        /// </summary>
        public string ExpTitle { get => _ExpTitle; set { _ExpTitle = value; NotifyOfPropertyChange(); } }
        private string _ExpTitle;

        #endregion Title

        // TODO : 화면 셋팅 타입 (Visibility) 필요시 사용 예정 (2024.10.31 minjae)
        /// <summary>
        /// 화면 셋팅 타입 (Visibility)
        /// </summary>
        // public string DisplayType { get => _DisplayType; set { _DisplayType = value; NotifyOfPropertyChange(); } }
        // private string _DisplayType;

        /// <summary>
        /// 화면 재실행시 탭 "기본정보" Default 출력
        /// </summary>
        public int SelectedTabIndex { get => _SelectedTabIndex; set { _SelectedTabIndex = value; NotifyOfPropertyChange(); } }
        private int _SelectedTabIndex;

        /// <summary>
        /// 화면 재실행시 탭 "기본정보" Default 출력
        /// </summary>
        // public Visibility visibleFirstTabItem { get => _visibleFirstTabItem; set { _visibleFirstTabItem = value; NotifyOfPropertyChange(); } }
        // private Visibility _visibleFirstTabItem = Visibility.Collapsed;

        /// <summary>
        /// 선택(클릭)한 특정 폴더 ID
        /// </summary>
        public string SelectedFolderId { get => _SelectedFolderId; set { _SelectedFolderId = value; NotifyOfPropertyChange(); } }
        private string _SelectedFolderId;

        /// <summary>
        /// 선택(클릭)한 특정 컨테이너 ID (영어 대문자)
        /// </summary>
        public string SelectedContainerId { get => _SelectedContainerId; set { _SelectedContainerId = value; NotifyOfPropertyChange(); } }
        private string _SelectedContainerId;

        /// <summary>
        /// Visibility - 검토요청 
        /// </summary>
        public Visibility visibleRequest { get => _visibleRequest; set { _visibleRequest = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleRequest = Visibility.Collapsed;

        /// <summary>
        /// Visibility - 컨테이너 정보  
        /// </summary>
        public Visibility visibleDetailInfo { get => _visibleDetailInfo; set { _visibleDetailInfo = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleDetailInfo = Visibility.Collapsed;

        /// <summary>
        /// Visibility - 파일정보  
        /// </summary>
        public Visibility visibleFileInfo { get => _visibleFileInfo; set { _visibleFileInfo = value; NotifyOfPropertyChange(); } }
        private Visibility _visibleFileInfo = Visibility.Collapsed;


        /// <summary>
        /// HttpClient client (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        [Inject]
        public HttpClient _Client { get; set; }

        /// <summary>
        /// 폴더 (디렉토리) 공통 경로 
        /// (응용 프로그램 - CobimExplorer.exe, Revit 애드인 프로그램 - CobimDockableDialogs.dll, AutoCAD 애드인 프로그램 - CobimPalette.dll)
        /// </summary>
        private string _CommonDirectoryPath { get; set; }

        // TODO : 프로젝트(+팀) 정보 프로퍼티 "_ProjectPack" 필요시 사용 예정 (2024.10.31 minjae)
        /// <summary>
        /// 프로젝트(+팀) 정보
        /// </summary>
        // private ProjectPack _ProjectPack { get; set; }

        /// <summary>
        /// 컨테이너 정보 
        /// </summary>
        private ContainerPack _ContainerPack { get; set; }

        /// <summary>
        /// 다운로드할 파일 정보
        /// </summary>
        private FilePack _FilePack { get; set; }

        /// <summary>
        /// 컨테이너 체크인 + 업로드 정보
        /// </summary>
        private CheckInPack _CheckInPack { get; set; }

        /// <summary>
        /// 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보 
        /// </summary>
        private WebViewerPack _WebViewerPack { get; set; }

        /// <summary>
        /// 컨테이너 체크인 + 업로드 성공시 
        /// HubBox 파일 변환 Rest API 요청시 필수 정보
        /// </summary>
        private TranslatePack _TranslatePack { get; set; }

        // TODO : bool 자료형 프로퍼티 "IsOpenFileWorking", "IsDownLoadWorking", "IsCheckOutWorking", "IsLinkedContainerWorking", "IsCheckInWorking", "IsTranslateWorking"
        //        private bool 프로퍼티 및 NotifyOfPropertyChange(nameof(~~~)) 참조(사용) 안함. (2024.09.04 minjae)
        // private bool 프로퍼티 및 NotifyOfPropertyChange(nameof(~~~)) 참조(사용) 안 하는 이유?
        // "파일 다운로드", "체크아웃", "체크인" 기능 실행시 처리 중인 상태에서 해당 기능을 중복 요청하지 못 하도록
        // 컨텍스트 메뉴 ("체크아웃", "체크인"), 화면 하단 탭 "기본정보" -> 항목 "파일정보"의 버튼 "다운로드"가 클릭되지 못하도록
        // 비활성화(IsEnabled = false 또는 Visibility= Visibility.Collapsed) 처리 하기 위해서 이고
        // 해당 기능을 실행하기 위해서 바인딩한 객체(프로퍼티)에 할당된 값이
        // True면 False, False면 True로 반대의 값으로 리턴되는 Converter 클래스 "InvertBooleanConverter.cs" 구현

        
        /// <summary>
        /// 컨테이너에 속한 파일열기 처리 진행 여부 
        /// </summary>
        public bool IsOpenFileWorking { get => _IsOpenFileWorking; set { _IsOpenFileWorking = value; NotifyOfPropertyChange(nameof(IsOpenFileWorking)); } }
        private bool _IsOpenFileWorking;

        /// <summary>
        /// 파일 다운로드 처리 진행 여부 
        /// </summary>
        public bool IsDownLoadWorking { get => _IsDownLoadWorking; set { _IsDownLoadWorking = value; NotifyOfPropertyChange(nameof(IsDownLoadWorking)); } }
        private bool _IsDownLoadWorking;

        /// <summary>
        /// 체크아웃 처리 진행 여부 
        /// </summary>
        public bool IsCheckOutWorking { get => _IsCheckOutWorking; set { _IsCheckOutWorking = value; NotifyOfPropertyChange(nameof(IsCheckOutWorking)); } }
        private bool _IsCheckOutWorking;

        /// <summary>
        /// 연관 컨테이너에 속한 파일 다운로드 처리 진행 여부 
        /// </summary>
        public bool IsLinkedContainerWorking { get => _IsLinkedContainerWorking; set { _IsLinkedContainerWorking = value; NotifyOfPropertyChange(nameof(IsLinkedContainerWorking)); } }
        private bool _IsLinkedContainerWorking;

        /// <summary>
        /// 체크인 처리 진행 여부 
        /// </summary>
        public bool IsCheckInWorking { get => _IsCheckInWorking; set { _IsCheckInWorking = value; NotifyOfPropertyChange(nameof(IsCheckInWorking)); } }
        private bool _IsCheckInWorking;

        /// <summary>
        /// 컨테이너 작업 취소 처리 진행 여부 
        /// </summary>
        public bool IsCancelWorking { get => _IsCancelWorking; set { _IsCancelWorking = value; NotifyOfPropertyChange(nameof(IsCancelWorking)); } }
        private bool _IsCancelWorking;

        /// <summary>
        /// 파일 변환 처리 진행 여부 
        /// </summary>
        public bool IsTranslateWorking { get => _IsTranslateWorking; set { _IsTranslateWorking = value; NotifyOfPropertyChange(nameof(IsTranslateWorking)); } }
        private bool _IsTranslateWorking;

        /// <summary>
        /// 탐색기 페이지 뷰모델
        /// </summary>
        public ExplorerVM explorerVM { get; set; }


        // TODO : 사용자 정보 프로퍼티 "UserInfo" 필요시 사용 예정 (2023.08.30 minjae)
        /// <sumary>
        /// 사용자 정보
        /// </sumary>
        //[Inject]
        //public UserInfoView UserInfo { get => _UserInfo; set { _UserInfo = value; NotifyOfPropertyChange(); } }
        //private UserInfoView _UserInfo;

        /// <summary>
        /// 전체 (프로젝트 + 팀 + 폴더 + 서브폴더 + 파일) 데이터 Explorer Collection
        /// </summary>
        public BindableCollection<ProjectView> LevelDatas { get => _LevelDatas; set { _LevelDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<ProjectView> _LevelDatas = new BindableCollection<ProjectView>();

        /// <summary>
        /// 컨테이너(파일 데이터 + 메타데이터 포함) Container Collection
        /// </summary>
        public BindableCollection<ContainerView> ContainerDatas { get => _ContainerDatas; set { _ContainerDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<ContainerView> _ContainerDatas = new BindableCollection<ContainerView>();

        // TODO : 리스트 객체 SelectedContainerData 필요시 사용 예정 (2023.11.29 minjae)
        public List<ContainerView> SelectedContainerList { get => _SelectedContainerList; set { _SelectedContainerList = value; NotifyOfPropertyChange(); } }
        private List<ContainerView> _SelectedContainerList = new List<ContainerView>();
        //public BindableCollection<ContainerView> SelectedContainerDatas { get => _SelectedContainerDatas; set { _SelectedContainerDatas = value; NotifyOfPropertyChange(); } }
        //private BindableCollection<ContainerView> _SelectedContainerDatas = new BindableCollection<ContainerView>();

        /// <summary>
        /// 컨테이너(파일 정보 + 메타데이터) Container(File 포함) Collection
        /// </summary>
        public BindableCollection<ContainerDetailView> DetailDatas { get => _DetailDatas; set { _DetailDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<ContainerDetailView> _DetailDatas = new BindableCollection<ContainerDetailView>();

        /// <summary>
        /// 연관 컨테이너 정보 Collection
        /// </summary>
        public BindableCollection<LinkedNodeView> LinkedNodeInfoDatas { get => _LinkedNodeInfoDatas; set { _LinkedNodeInfoDatas = value; NotifyOfPropertyChange(); } }
        private BindableCollection<LinkedNodeView> _LinkedNodeInfoDatas = new BindableCollection<LinkedNodeView>();

        /// <summary>
        /// 프로젝트 리스트 
        /// </summary>
        public List<ProjectView> ProjectList { get => _ProjectList; set { _ProjectList = value; NotifyOfPropertyChange(); } }
        private List<ProjectView> _ProjectList = new List<ProjectView>();

        /// <summary>
        /// 팀 리스트 
        /// </summary>
        public List<ITreeNode> TeamList { get => _TeamList; set { _TeamList = value; NotifyOfPropertyChange(); } }
        private List<ITreeNode> _TeamList = new List<ITreeNode>();

        /// <summary>
        /// 폴더 리스트 (루트 + 서브 폴더 포함) 
        /// </summary>
        public List<ITreeNode> FolderList { get => _FolderList; set { _FolderList = value; NotifyOfPropertyChange(); } }
        private List<ITreeNode> _FolderList = new List<ITreeNode>();

        /// <summary>
        /// 컨테이너(파일 포함) 리스트
        /// </summary>
        public List<ContainerView> ContainerList { get => _ContainerList; set { _ContainerList = value; NotifyOfPropertyChange(); } }
        private List<ContainerView> _ContainerList = new List<ContainerView>();

        /// <summary>
        /// 파일 컨테이너 정보 + 메타데이터 리스트
        /// </summary>
        public List<ContainerDetailView> DetailList { get => _DetailList; set { _DetailList = value; NotifyOfPropertyChange(); } }
        private List<ContainerDetailView> _DetailList = new List<ContainerDetailView>();

        /// <summary>
        /// 연관 컨테이너 정보 리스트
        /// </summary>
        public List<LinkedNodeView> LinkedNodeInfoList { get => _LinkedNodeInfoList; set { _LinkedNodeInfoList = value; NotifyOfPropertyChange(); } }
        private List<LinkedNodeView> _LinkedNodeInfoList = new List<LinkedNodeView>();

        // TODO : 필요시 "컨텍스트(ContextMenu) 메뉴 정보 Collection" 구현하기 (2023.12.12 minjae)
        // 참고 URL - https://chat.openai.com/c/b9d52fcf-aab0-49e3-8008-8852767a9982
        /// <summary>
        /// 컨텍스트(ContextMenu) 메뉴 정보 Collection
        /// </summary>
        // public BindableCollection<ContextMenuView> ContextMenuDatas { get => _ContextMenuDatas; set { _ContextMenuDatas = value; NotifyOfPropertyChange(); } }
        // private BindableCollection<ContextMenuView> _ContextMenuDatas = new BindableCollection<ContextMenuView>();

        /// <summary>
        /// 수정 메서드 "UpdateAsync" 실행시 전달할 수정된 메서드 파라미터 데이터 목록
        /// </summary>
        public Param UpdateParam { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        public ExplorerVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 minjae)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;

            this._ExpTitle = CobimHelper.ExpTitle;   // 탐색기 화면 Title 

            // TODO : CreateCommand - 파일(컨테이너) 신규 등록의 경우 COBIM 웹 사이트(인재 INC)에서만 사용하는 기능이고 CDE 탐색기 응용 프로그램에서는 사용 안 하는 걸로 소통 완료.
            //        하여 해당 파일(컨테이너) 신규 등록 메서드는 주석 처리하고 사용 안 함. (2023.10.19 minjae)
            // CreateCommand = new ButtonCommand(FileCreateAsync, CanExecuteMethod);
            // FolderListCommand = new ButtonCommand(FolderListAsync, CanExecuteMethod);
            // FolderCommand = new ButtonCommand(FolderListAsync, CanExecuteMethod);
            ContainerCommand = new ButtonCommand(CobimHelper.AsyncMethodType, ContainerSearchAsync, CanExecuteMethod);
            SelectedContainerCommand = new ButtonCommand(CobimHelper.VoidMethodType, SelectedChangedContainer, CanExecuteMethod);
            FileAddCommand = new ButtonCommand(CobimHelper.AsyncMethodType, FileAddAsync, CanExecuteMethod);
            // WebViewerCommand = new ButtonCommand(CobimHelper.AsyncMethodType, WebViewerAsync, CanExecuteMethod);
            ShowWebViewerCommand = new ButtonCommand(CobimHelper.AsyncMethodType, ShowWebViewerAsync, CanExecuteMethod);
            DetailCommand = new ButtonCommand(CobimHelper.AsyncMethodType, DetailSearchAsync, CanExecuteMethod);
            RequestCommand = new ButtonCommand(CobimHelper.AsyncMethodType, RequestAsync, CanExecuteMethod);
            UpdateCommand = new ButtonCommand(CobimHelper.AsyncMethodType, UpdateAsync, CanExecuteMethod);
            

            UploadCommand = new ButtonCommand(CobimHelper.AsyncMethodType, UpLoadAsync, CanExecuteMethod);
            LinkedNodeInfoCommand = new ButtonCommand(CobimHelper.AsyncMethodType, LinkedNodeInfoSearchAsync, CanExecuteMethod);

            // TODO : 테스트 Command 프로퍼티 "TestCommand" 필요시 사용 예정 (2024.09.03 minjae)
            // TestCommand = new ButtonCommand(CobimHelper.AsyncMethodType, TestAsync, CanExecuteMethod);
            DownLoadCommand = new ButtonCommand(CobimHelper.AsyncMethodType, DownLoadAsync, CanExecuteMethod);
            CheckOutCommand = new ButtonCommand(CobimHelper.AsyncMethodType, CheckOutAsync, CanExecuteMethod);
            CheckInCommand = new ButtonCommand(CobimHelper.AsyncMethodType, CheckInAsync, CanExecuteMethod);
            CancelWorkCommand = new ButtonCommand(CobimHelper.AsyncMethodType, CancelWorkAsync, CanExecuteMethod);
            OpenFileCommand = new ButtonCommand(CobimHelper.AsyncMethodType, OpenFileAsync, CanExecuteMethod);
            // TODO : 컨테이너 잠금(락 - Lock / 비활성화)(ReadYn = "Y") 처리 Command "LockContainerCommand" 필요시 Button Command 로 바인딩 예정 (2023.12.12 minjae)
            // LockContainerCommand = new ButtonCommand(CobimHelper.VoidMethodType, LockContainer, CanExecuteMethod);
        }

        #endregion 생성자 

        #region 기본 메소드

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            explorerVM = this;
            InitSetting();
        }

        // TODO : 이벤트 메서드 "OnActivate" 비동기(async)로 구현 (2023.09.18 minjae)
        // 참고 URL - https://vmpo.tistory.com/89
        protected async override void OnActivate()
        {
            base.OnActivate();
            DisplaySetting(CobimHelper.TypeOfOnActivate);   // 화면 셋팅 처리S
            await ExplorerListCreateAsync();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }


        private bool CanExecuteMethod(object obj)
        {
            return true;
        }

        #endregion 기본 메소드

        #region InitSetting

        // TODO : 초기 셋팅 메서드 "InitSetting" 필요시 구현하기 (2023.12.12 minjae)
        // 참고 URL - https://chat.openai.com/c/b9d52fcf-aab0-49e3-8008-8852767a9982
        /// <summary>
        /// 초기 셋팅 
        /// </summary>
        private void InitSetting()
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 아래 주석 처리한 코드 필요시 사용 예정 (2023.12.22 minjae)
                // ExplorerView.RequestHeaders.Tenant        = AppSetting.Default.Login.Tenant;                    // 테넌트 아이디 
                // ExplorerView.RequestHeaders.Authorization = AppSetting.Default.Login.Authorization.resultData;  // 로그인 토큰 키 

                // 데이터 모델 객체 InputParamData에 HttpClient client 객체 값 할당 
                InputParamData.SetHttpClient(_Client);

                // 데이터 모델 객체 InputParamData에 폴더 (디렉토리) 공통 경로 string 클래스 객체 값(문자열) 할당 
                _CommonDirectoryPath = AppSetting.Default.DirectoryBase.GetCommonDirectoryPath();
                InputParamData.SetCommonDirectoryPath(_CommonDirectoryPath);
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion InitSetting

        #region DisplaySetting

        /// <summary>
        /// 화면셋팅 - 화면에 필요한 컨트롤러(DockPanel, Button 등등...) 출력, 리스트 및 Collection 초기화(Clear)
        /// </summary>
        private void DisplaySetting(string pDisplayType)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                switch (pDisplayType)
                {
                    // case CobimHelper.TypeOfOnInitialActivate:
                    case CobimHelper.TypeOfOnActivate:
                        // InitSetting();              // 화면 초기 셋팅  

                        // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
                        //        화면 종료 또는 다른 화면 이동 후 해당 탐색기 화면 재실행 시
                        //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
                        //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
                        // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
                        // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
                        // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
                        SelectedTabIndex = 0;

                        // 메서드 "ExplorerListCreateAsync" 사용되는 리스트, Collection
                        ProjectList.Clear();                                      // 프로젝트 리스트 초기화
                        TeamList.Clear();                                         // 팀 리스트 초기화
                        FolderList.Clear();                                       // 폴더 리스트 초기화 
                        // LevelDatas.Clear();

                        // 메서드 "ContainerSearchAsync" 사용되는 리스트
                        ContainerList.Clear();                                    // 컨테이너 리스트 초기화
                        ContainerDatas.Clear();                                   // 컨테이너 메타 데이터 Collection 초기화

                        // 메서드 "DetailSearchAsync" 사용되는 리스트
                        DetailList.Clear();                                       // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
                        // DetailDatas.Clear();

                        // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
                        LinkedNodeInfoList.Clear();                               // 연관 컨테이너 정보 리스트 초기화
                        LinkedNodeInfoDatas.Clear();                              // 연관 컨테이너 정보 Collection 초기화

                        // TODO : 컨테이너 ID (영어 대문자) 프로퍼티 exp_UpperContainerId 필요시 사용 예정 (2023.10.23 minjae) 
                        // exp_UpperContainerId  = string.Empty;
                        // visibleFirstTabItem   = Visibility.Visible;            // 화면 활성화될 때 마다 탭 "기본정보" Default 출력
                        visibleRequest           = Visibility.Collapsed;          // 화면 활성화될 때 마다 버튼 "검토요청" 숨김 처리 
                        visibleDetailInfo        = Visibility.Collapsed;          // 화면 활성화될 때 마다 DockPanel 영역 "컨테이너 정보" 숨김 처리
                        visibleFileInfo          = Visibility.Collapsed;          // 화면 활성화될 때 마다 DockPanel 영역 "파일정보" 숨김 처리
                        // visibleLinkedNodeInfo = Visibility.Collapsed;          // 화면 활성화될 때 마다 DockPanel 영역 "연관 컨테이너 정보 TreeView" 숨김 처리
                        break;
                    case CobimHelper.TypeOfExplorerListCreateAsync:
                        TeamList.Clear();                                         // 팀 리스트 초기화
                        FolderList.Clear();                                       // 폴더 리스트 초기화
                        visibleRequest    = Visibility.Collapsed;                 // 버튼 "검토요청" 숨김 처리
                        visibleDetailInfo = Visibility.Collapsed;                 // DockPanel 영역 "컨테이너 정보" 숨김 처리
                        visibleFileInfo = Visibility.Collapsed;                   // DockPanel 영역 "파일정보" 숨김 처리
                        break;
                    case CobimHelper.TypeOfDetailSearch:
                    // case CobimHelper.TypeOfLinkedNodeInfoCreate:
                        // InitSetting();
                        // 메서드 "ContainerSearchAsync" 사용되는 리스트
                        // ContainerList.Clear();                                 // 컨테이너 리스트 초기화

                        // 메서드 "ContainerDeatilSearchAsync" 사용되는 리스트
                        // ContainerList.Clear();                                 // 컨테이너(파일 정보 + 메타데이터) 리스트 초기화

                        // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
                        //        컨테이너 ID 클릭 후 메서드 "ContainerDeatilSearchAsync" 실행시 
                        //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
                        //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
                        // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
                        // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
                        // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
                        SelectedTabIndex = 0;

                        DetailList.Clear();                                       // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
                        DetailDatas.Clear();                                      // 파일 컨테이너 정보 + 메타데이터 Collection 초기화

                        // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
                        LinkedNodeInfoList.Clear();                               // 연관 컨테이너 정보 리스트 초기화
                        LinkedNodeInfoDatas.Clear();                              // 연관 컨테이너 정보 Collection 초기화

                        visibleRequest           = Visibility.Visible;            // 검토요청 활성화 처리
                        visibleDetailInfo        = Visibility.Visible;            // 컨테이너 정보 활성화 처리
                        visibleFileInfo          = Visibility.Visible;            // 파일정보 활성화 처리
                        // visibleLinkedNodeInfo = Visibility.Visible;            // 연관 컨테이너 정보 TreeView 활성화 처리
                        break;
                    // case CobimHelper.TypeOfContainerSearch:
                    // case CobimHelper.TypeOfCheckOut:
                    // case CobimHelper.TypeOfCheckIn:
                    case CobimHelper.TypeOfContainerListCreateAsync:
                        SelectedTabIndex = 0;                                     // 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정

                        DetailList.Clear();                                       // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
                        DetailDatas.Clear();                                      // 파일 컨테이너 정보 + 메타데이터 Collection 초기화

                        // 메서드 "ContainerSearchAsync" 사용되는 리스트
                        ContainerList.Clear();
                        ContainerDatas.Clear();
                        visibleRequest = Visibility.Collapsed;                     // 버튼 "검토요청" 숨김 처리
                        visibleDetailInfo = Visibility.Collapsed;                  // DockPanel 영역 "컨테이너 정보" 숨김 처리
                        visibleFileInfo = Visibility.Collapsed;                    // DockPanel 영역 "파일정보" 숨김 처리
                        break;
                    // TODO : 아래 case 문 "case CobimHelper.TypeOfLinkedNodeInfoCreate:" 필요시 사용 예정 (2023.12.26 minjae) 
                    //case CobimHelper.TypeOfLinkedNodeInfoCreate:
                    //    // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
                    //    LinkedNodeInfoList.Clear();                               // 연관 컨테이너 정보 리스트 초기화
                    //    LinkedNodeInfoDatas.Clear();                              // 연관 컨테이너 정보 Collection 초기화
                    //    break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion DisplaySetting

        #region ExplorerListCreateAsync

        /// <summary>
        /// 프로젝트 - 팀 - 폴더 - 서브폴더 리스트 생성 및 초기화 / 파일 메타 데이터 Collection 초기화 
        /// 메서드 파라미터 List<ITreeNode> pSubFolderFileList 추가 여부 확인 
        /// </summary>
        private async Task ExplorerListCreateAsync()
        {
            string projectId     = string.Empty;   // 프로젝트 아이디
            string projectName   = string.Empty;   // 프로젝트 이름 
                                                      
            string teamProjectId = string.Empty;   // 팀의 상위 프로젝트 아이디
            string teamId        = string.Empty;   // 팀 아이디
            string teamName      = string.Empty;   // 팀 이름
                                                                               
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                InputParamData.SetData(LoginView.GetRequestHeader());   // Http 웹서버 통신 요청 Header 값 가져와서 탐색기 데이터 모델 객체 InputParamData에 값 셋팅 

                var datas = await ExplorerRestServer.GetProjectTeamListAsync(InputParamData);   // 프로젝트 + 팀 리스트 데이터 받아오기 

                foreach(var data in datas)
                {
                    DisplaySetting(CobimHelper.TypeOfExplorerListCreateAsync);   // 화면 셋팅 처리

                    var projectDic = data[ProjectHelper.projectEntity].ToDictionary();   // 프로젝트 메타 데이터 Dictionary
                    // Dictionary 객체 projectDic 특정 키(Key) "projectId"에 속하는 값(Value) 문자열로 변환 (2023.09.11 minjae)
                    // 참고 URL - https://developer-talk.tistory.com/694
                    projectId      = projectDic[ProjectHelper.projectId].ToString();     // 해당 프로젝트 아이디 
                    projectName    = projectDic[ProjectHelper.projectName].ToString();   // 해당 프로젝트 이름 

                    var teamDic    = data[TeamHelper.teamEntity].ToDictionary();         // 팀 메타 데이터 Dictionary
                    teamProjectId  = teamDic[ProjectHelper.projectId].ToString();        // 해당 팀이 소속된 프로젝트 이름 
                    teamId         = teamDic[TeamHelper.teamId].ToString();              // 해당 팀 아이디 
                    teamName       = teamDic[TeamHelper.teamName].ToString();            // 해당 팀 이름 

                    // 컨테이너 정보(ContainerPack) 객체 생성해서 탐색기 데이터 모델 객체 InputParamData에 값 셋팅 
                    InputParamData.SetContainerPack(new ContainerPack(projectId,
                                                                      teamId, 
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      string.Empty));

                    var folderList = await ExplorerRestServer.GetFolderListAsync(InputParamData);   // 폴더 리스트 데이터 받아오기

                    if((int)EnumFolder.NONE == folderList.Count) continue;   // 폴더 목록 리스트 객체 folderList 에 데이터가 없는 경우((int)EnumFolder.NONE) 폴더가 없으므로 continue 처리 

                    // TODO : Linq Where 확장메서드에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                    // 참고 URL - https://husk321.tistory.com/405
                    // 프로젝트 - 팀 하위에 속하는 루트 폴더 리스트 데이터 추출
                    var rootFolderList = folderList.Where(folder => folder[FolderHelper.parentFolderId] is null).Select(folder => new FolderView
                    {
                        FolderDataInfo = folder,
                        // TODO : 개체 이니셜라이저 사용해서 폴더 목록 리스트 객체 FolderItems 생성시 객체 값(folder) 할당 (2023.11.23 minjae)
                        // 참고 URL - https://hyokye0ng.tistory.com/86
                        FolderItems    = new List<Dictionary<string, object>> { folder },
                        // TODO : == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                        // 참고 URL - https://husk321.tistory.com/405
                        // TODO : NULL 예외처리 연산자 ?. 사용시 오류 발생으로 인하여 NULL 예외처리 삼항 연산자 사용하는 로직으로 수정 (2023.09.11 minjae)
                        // 참고 URL - https://jellyho.com/blog/54/
                        exp_ParentFolderId = (folder[FolderHelper.parentFolderId] is null) ? (string.Empty) : (folder[FolderHelper.parentFolderId].ToString()),
                        exp_FolderGroupId  = folder[FolderHelper.folderGroupId].ToString(),
                        exp_FolderLevelNo  = folder[FolderHelper.levelNo].ToString(),
                        exp_ProjectId      = projectId,
                        exp_TeamId         = teamId,
                        exp_FolderId = folder[FolderHelper.folderId].ToString(),
                        exp_TreeName = folder[FolderHelper.folderName].ToString(),
                        // TODO : 파일 리스트 추가 작업시 ChildNodes 사용 예정 (2023.09.18 minjae)
                        // ChildNodes = subNodes
                    });

                    // 루트 폴더 하위 서브 폴더 추가
                    foreach(var rootFolder in rootFolderList)
                    {
                        await FillFolders(rootFolder, folderList);   // 서브 폴더 추가

                        FolderList.Add(rootFolder);                  // (루트 + 서브) 폴더 리스트에 루트 폴더 추가
                    }

                    // 프로젝트 하위 디렉토리 (팀) 존재하지 않을 경우 새로 생성
                    // TODO : bool 변수 "hasTeamInfo" 구현 (2023.09.18 minjae)
                    // 참고 URL - https://frogand.tistory.com/211
                    bool hasTeamInfo =    false == string.IsNullOrWhiteSpace(teamProjectId)
                                       && false == string.IsNullOrWhiteSpace(teamId)
                                       && false == string.IsNullOrWhiteSpace(teamName);

                    if(true == hasTeamInfo 
                       && projectId == teamProjectId
                       && false == string.IsNullOrWhiteSpace(projectId))
                    {
                        var teamInfo = new TeamView()
                        {
                            TeamDataInfo = teamDic,
                            // TODO : 개체 이니셜라이저 사용해서 팀 목록 리스트 객체 TeamItems 생성시 객체 값(teamDic) 할당 (2023.11.23 minjae)
                            // 참고 URL - https://hyokye0ng.tistory.com/86
                            // TODO : TeamItems값이 NULL인 원인 파악 후 해결하기 (2023.09.11 minjae) 
                            // TeamItems = teamArray.Where(key => key["teamId"].Equals(teamIdValue)).Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList(),
                            TeamItems    = new List<Dictionary<string, object>> { teamDic },
                            // TODO : 팀에 소속된 사용자 리스트 JSON 데이터 인재 INC 로부터 REST API 공유 받으면 구현 진행 예정 (2023.09.12 minjae)
                            // TeamUserList = new List<Dictionary<string, object>> { teamUserDic },
                            exp_TeamId   = teamId,
                            // TODO : 프로퍼티 "exp_TeamName" 필요시 사용 예정 (2023.10.23 minjae)
                            // exp_TeamName = teamName,   // 화면 우측 상단 팀 이름 출력할 때 사용할 바인딩 객체 "exp_TeamName"
                            exp_TreeName = teamName,
                            // TestChildNodes = pFolderList

                            // TestChildNodes = TestFolderNodes
                            // ChildNodes   = FolderList[0]
                        };

                        teamInfo.ChildNodes.Add(FolderList[CobimHelper.RootFolderIndex]);

                        TeamList.Add(teamInfo);
                    }

                    // 루트 디렉토리 (프로젝트) 존재하지 않을 경우 새로 생성
                    // TODO : bool 변수 "hasProjectInfo" 구현 (2023.09.18 minjae)
                    // 참고 URL - https://frogand.tistory.com/211
                    bool hasProjectInfo =    false == string.IsNullOrWhiteSpace(projectId)
                                          && projectId == teamProjectId
                                          && false == string.IsNullOrWhiteSpace(projectName);

                    if(true == hasProjectInfo)
                    {
                        var projectInfo = new ProjectView()
                        {
                            ProjectDataInfo = projectDic,
                            // TODO : 개체 이니셜라이저 사용해서 프로젝트 목록 리스트 객체 ProjectItems 생성시 객체 값(projectDic) 할당 (2023.11.23 minjae)
                            // 참고 URL - https://hyokye0ng.tistory.com/86
                            // TODO : ProjectItems값이 NULL인 원인 파악 후 해결하기(2023.09.11 minjae)
                            ProjectItems = new List<Dictionary<string, object>> { projectDic },
                            exp_ProjectId = projectId,

                            exp_TreeName = projectName,
                            ChildNodes = TeamList.Where(team => team.exp_TreeName.Equals(teamName)).ToList()
                        };
                    
                        ProjectList.Add(projectInfo);
                    }
                }

                LevelDatas.Clear();                           // Explorer Collection 초기화
                LevelDatas.AddRange(ProjectList.ToArray());   // Explorer Collection에 전체 데이터(프로젝트 + 팀 + 폴더 + 서브폴더 + 파일) 추가 
                return;
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 비동기 메서드 "ExplorerListCreateAsync" 실행시 발견한 오류 메시지 "'<' is an invalid start of a value. LineNumber: 0 | BytePositionInLine: 0." 오류 원인 파악하기 (2023.11.27 minjae)
                // 참고 URL - https://stackoverflow.com/questions/61487289/exception-is-an-invalid-start-of-a-value
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion ExplorerListCreateAsync

        #region FillFolders

        /// <summary>
        /// 서브 폴더 생성 및 부모(루트) 폴더의 자식 노드로 연결
        /// </summary>
        /// <param name="parentFolder">부모(루트) 폴더</param>
        /// <param name="pFolderList">(루트 + 서브)폴더 리스트</param>
        private async Task FillFolders(FolderView parentFolder, List<Dictionary<string, object>> pFolderList)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // 부모 폴더(루트 또는 서브)에 하위 서브 폴더 데이터 추출 
                var childNodes = pFolderList.Where(folder => parentFolder.exp_FolderId == folder[FolderHelper.parentFolderId]?.ToString());

                if(false == childNodes.Any()) return;

                foreach(var childNode in childNodes)
                {
                    // TODO : 개체 이니셜라이저 사용해서 서브 폴더 객체 subFolder 생성시 객체 값(FolderDataInfo ~ exp_TeamId) 할당 (2023.11.23 minjae)
                    // 참고 URL - https://hyokye0ng.tistory.com/86
                    // TODO : Dictionary 특정 키(Key)에 해당하는 값(Value) 구해서 FolderView 클래스에 존재하는 프로퍼티에 값 할당 (2023.09.15 minjae)
                    // 참고 URL - https://developer-talk.tistory.com/694
                    var subFolder = new FolderView {
                                            FolderDataInfo     = childNode,
                                            FolderItems        = new List<Dictionary<string, object>> { childNode },
                                            exp_ParentFolderId = parentFolder.exp_FolderId,
                                            exp_FolderGroupId  = childNode[FolderHelper.folderGroupId].ToString(),
                                            exp_FolderLevelNo  = childNode[FolderHelper.levelNo].ToString(),
                                            exp_TreeName       = childNode[FolderHelper.folderName].ToString(),
                                            exp_ProjectId      = parentFolder.exp_ProjectId,
                                            exp_TeamId         = parentFolder.exp_TeamId,
                                            exp_FolderId       = childNode[FolderHelper.folderId].ToString()
                                        };

                    parentFolder.ChildNodes.Add(subFolder);      // 부모 폴더의 자식 노드로 서브 폴더 추가

                    await FillFolders(subFolder, pFolderList);   // 서브 폴더의 자식 노드로 또 다른 서브 폴더 추가
                }
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion FillFolders

        #region Set 

        // TODO : 추후 필요시 사용 예정 (2023.08.30 minjae)
        //public static void Set(UserInfoView userInfoView)
        //{
        //    try
        //    {
        //        UserInfo = userInfoView;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion Set

        #region FileCreateAsync

        // TODO : 파일(컨테이너) 신규 등록의 경우 COBIM 웹 사이트(인재 INC)에서만 사용하는 기능이고 CDE 탐색기 응용 프로그램에서는 사용 안 하는 걸로 소통 완료.
        //        하여 해당 파일(컨테이너) 신규 등록 메서드는 주석 처리하고 사용 안 함. (2023.10.19 minjae)
        /// <summary>
        /// 파일(컨테이너) 신규 등록 
        /// </summary>
        // private async Task FileCreateAsync(object obj)
        // {
        //     var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록
           
        //     try
        //     {
        //         MessageBox.Show("파일 신규등록 기능 필요시 구현 예정");
        //         return;
        //     }
        //     catch (Exception ex)
        //     {
        //         Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //         MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //     }
        //     return;
        // }

        #endregion FileCreateAsync

        #region ContainerSearchAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너(파일 포함) 목록 조회
        /// </summary>
        private async Task ContainerSearchAsync(object obj)
        {
            // TODO : 아래 주석친 소스 코드 3줄(parameters, containerDatas, containerDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
            // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
            // 참고 URL - https://nonstop-antoine.tistory.com/36
            // var parameters        = obj as IEnumerable;                 // 폴더 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 
            // var containerDatas    = new List<string>();                 // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDatas"
            // var containerDataDic  = new Dictionary<string, string>();   // 컨테이너 데이터를 담는 Dictionary 객체 containerDataDic 생성

            // TODO : 폴더 버튼 클릭 시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            var parameters = (object[])obj;                                // 폴더 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 
            // var containerDatas = parameters.ToList();                   // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "containerDatas"

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] containerDatas  = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃", "체크인" 클릭시
                //        탐색기 뷰모델(ExplorerVM.cs)   -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
                //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
                //        처음 사용자가 선택(클릭)한 특정 폴더 ID 프로퍼티 "SelectedFolderId" 에 폴더 ID 문자열을 할당해서 
                //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync"에 메서드 파라미터를
                //        프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae) 
                // 선택(클릭)한 특정 폴더 ID    
                SelectedFolderId = containerDatas[(int)EnumContainer.FOLDERID];

                _ContainerPack = new ContainerPack(containerDatas[(int)EnumContainer.PROJECTID],
                                                  containerDatas[(int)EnumContainer.TEAMID],
                                                  string.Empty,
                                                  SelectedFolderId,
                                                  string.Empty);

                await ContainerListCreateAsync(_ContainerPack);
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion ContainerSearchAsync

        #region SelectedChangedContainer

        // TODO : 추후 필요시 이벤트 메서드 "SelectedChangedContainer" 구현 예정
        // <DataGrid> 영역에서 출력되는 데이터로 로우들을 선택해서 멀티 체크아웃, 체크인 작업 처리하기 위해
        // 아래처럼 SelectionChanged 이벤트 메서드 "SelectedContainerExecuteAsync"  구현하기(2024.09.06 minjae) 
        // 참고 URL - https://nonstop-antoine.tistory.com/104  

        /// <summary>
        /// 사용자가 선택한 컨테이너(파일 포함) 데이터 로우 목록 SelectionChanged 이벤트
        /// </summary>
        //private async Task SelectedContainerExecuteAsync(object obj)
        private void SelectedChangedContainer(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                SelectedContainerList.Clear();

                IList items = (IList)obj;

                IEnumerable<ContainerView> collection = items.Cast<ContainerView>();
                SelectedContainerList = collection.ToList();
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion SelectedChangedContainer

        #region ContainerListCreateAsync

        /// <summary>
        /// 컨테이너 리스트 생성(재조회 - Refresh 기능 포함)
        /// </summary>
        private async Task ContainerListCreateAsync(ContainerPack pContainerPack)
        {
            List<Dictionary<string, object>> containerList = new List<Dictionary<string, object>>();   // 컨테이너 목록(리스트) 객체 초기화
            Dictionary<string, object> resultWebViewerData = new Dictionary<string, object>();         // HubBox Rest API 2. HubBox 웹뷰어 요청 및 정보 리스트 객체 초기화
            
            string createUserId   = string.Empty;   // 작성자 
            string createDateTime = string.Empty;   // 등록일시
                                                                                        
            string readYn = string.Empty;           // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)

            string translateStatusCode = string.Empty;   // HubBox Rest API 2. 파일 뷰어 요청 HttpStatusCode 상태 코드 값

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                DisplaySetting(CobimHelper.TypeOfContainerListCreateAsync);   // 화면 셋팅 처리

                // Http 웹서버 통신 요청 Header. 컨테이너 정보(ContainerPack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅 
                InputParamData.SetData(LoginView.GetRequestHeader(),
                                       pContainerPack);   

                // TODO : 오류 메시지 "Id = 3455, Status = WaitingForActivation, Method = "{null}", Result = "{Not yet computed}"
                //        해결하기 위해 비동기 메서드 호출시 키워드 "await" 추가 (2023.12.21 minjae)
                // 참고 URL - https://www.codeproject.com/Questions/1156449/How-to-call-async-methods-Csharp
                containerList = await ExplorerRestServer.GetContainerListAsync(InputParamData);   // 컨테이너 목록(리스트) 가져오기

                // 컨테이너 리스트에 데이터가 존재하지 않는 경우 
                if(CobimHelper.ContainerNotExist == containerList.Count)
                {
                    MessageBox.Show("성과품을 찾을 수 없습니다.");
                    return;
                } 

                foreach(var container in containerList)
                {
                    // HubBox Rest API 2. 파일 뷰어 요청 데이터(WebViewerPack) 객체 생성해서 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                    InputParamData.SetWebViewerPack(new WebViewerPack(container[ContainerHelper.projectId].ToString(),
                                                                      container[ContainerHelper.teamId].ToString(),
                                                                      container[ContainerHelper.containerId].ToString(),
                                                                      container[ContainerHelper.fileId].ToString()));

                    // HubBox Rest API 2. 파일 뷰어 요청 및 데이터 가져오기 
                    resultWebViewerData = await HubBoxRestServer.GetWebViewerURLAsync(InputParamData);

                    var fileTypeCodeInfoDic = container[ContainerHelper.fileTypeCodeInfo].ToDictionary();
                    var stepCodeInfoDic     = container[ContainerHelper.stepCodeInfo].ToDictionary();
                    var statusCodeInfoDic   = container[ContainerHelper.statusCodeInfo].ToDictionary();


                    createUserId = container[ContainerHelper.createUserId].ToString();     // 작성자 

                    // TODO : 특정 문자 'T'를 포함한 등록일시 문자열 (예) "2023-07-25T10:44:45.988+00:00" 을 DateTime 객체 dt로 변환 (2023.09.21 minjae)
                    // 참고 URL - https://stackoverflow.com/questions/14252312/how-to-convert-date-with-t-to-from-string-in-c-sharp
                    var createDT = container[ContainerHelper.createDateTime].ToString();   // 등록일시

                    // TODO : UTC 협정 세계시로 변환 필요시 DateTimeOffset 클래스 객체 dto 선언 및 초기화 
                    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-NET%EC%9D%98-DataTime-%EB%AA%A8%EB%B2%94-%EC%82%AC%EB%A1%80
                    //var dto = DateTimeOffset.Parse(createDT);

                    // TODO : UTC 협정 세계시로 변환할 필요 없이 대한민국 시간(로컬 타임)으로 변환하기 위해 DateTime 클래스 객체 dt 선언 및 초기화(2023.09.26 minjae)
                    // DataTime VS DateTimeOffset 비교 문서
                    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-%EC%97%90%EC%84%9C-IronPython-%EC%9D%B4%EC%9A%A9%ED%95%98%EC%97%AC-Python-%EC%97%B0%EB%8F%99
                    // 문자열 ->  DateTime 객체 dt로 형변환
                    // Get the date object from the string. 
                    var dt = DateTime.Parse(createDT);

                    // DateTime 객체 dt -> 문자열로 형변환 (대문자 HH - 24시간제 기준 / 소문자 hh - 12시간제 기준(오전 12시, 자정 12시))
                    // 참고 URL - https://wwwi.tistory.com/327
                    createDateTime = dt.ToString("yyyy/MM/dd HH:mm");

                    translateStatusCode = resultWebViewerData[HubBoxHelper.code].ToString();

                    var containerInfo = new ContainerView()
                    {
                        FileDataInfo = container,
                        // TODO : 개체 이니셜라이저 사용해서 파일 목록 리스트 객체 FileItems 생성시 객체 값(file) 할당 (2023.11.23 minjae)
                        // 참고 URL - https://hyokye0ng.tistory.com/86
                        FileItems = new List<Dictionary<string, object>> { container },
                        FileTypeCodeInfo = container[ContainerHelper.fileTypeCodeInfo].ToDictionary(),
                        StepCodeInfo = container[ContainerHelper.stepCodeInfo].ToDictionary(),
                        StatusCodeInfo = container[ContainerHelper.statusCodeInfo].ToDictionary(),
                        CheckCodeInfo = container[ContainerHelper.checkCodeInfo].ToDictionary(),
                        
                        // TODO : "SharedContainerList" 필요시 구현 예정 (2023.09.22 minjae)
                        SharedContainerList = new List<Dictionary<string, object>>(),
                        
                        CreateUserIdAndTime = createUserId + " | " + createDateTime,
                        exp_FileName = container[ContainerHelper.fileNm].ToString(),
                        exp_ContainerGroup = container[ContainerHelper.containerGroup].ToString(),
                        // TODO : 컨테이너 ID(영어 대문자) 프로퍼티 "exp_UpperContainerId" 영어 대문자로 구현(2023.09.22 minjae)
                        //        프로퍼티 "exp_UpperContainerId" 용도 - 탐색기 화면 뷰(ExplorerV.xaml) "ListView" 영역에 바인딩할 컬럼 용도
                        // 참고 URL - https://developer-talk.tistory.com/669
                        exp_UpperContainerId = container[ContainerHelper.containerId].ToString().ToUpper(),
                        // TODO : 컨테이너 ID(영어 소문자) 프로퍼티 "exp_ContainerId" 영어 대문자로 구현(2023.10.12 minjae)
                        //        프로퍼티 "exp_ContainerId" 용도
                        //        - Rest API 메서드 "ExplorerRestServer.GetContainerDetailListAsync" 파라미터 "ContainerPack"에 속하는
                        //          컨테이너 아이디(containerId)에 영어 소문자로 넘겨줘야 인재 INC 서버 로부터 응답 결과 JSON 데이터를 정상적으로 넘겨 받을 수 있음. 
                        exp_ContainerId = container[ContainerHelper.containerId].ToString(),
                        exp_ProjectId = container[ContainerHelper.projectId].ToString(),
                        exp_TeamId = container[ContainerHelper.teamId].ToString(),
                        
                        exp_FolderId = container[ContainerHelper.folderId].ToString(),
                        exp_FileId = container[ContainerHelper.fileId].ToString(),
                        exp_FileTypeCode = container[ContainerHelper.fileTypeCode].ToString(),
                        exp_Title = container[ContainerHelper.title].ToString(),
                        
                        exp_StatusCode = container[ContainerHelper.statusCode].ToString(),
                        exp_RevsnVal = container[ContainerHelper.revsnVal].ToString(),
                        exp_ReadYn = container[ContainerHelper.readYn].ToString(),
                        exp_CheckStatusCode = container[ContainerHelper.checkStatusCode].ToString(),
                        
                        exp_CreateUserId = createUserId,
                        exp_CreateDateTime = createDateTime,
                        exp_TypeCodeName = fileTypeCodeInfoDic[ContainerHelper.fileTypeCodeName].ToString(),
                        exp_StepCodeName = stepCodeInfoDic[ContainerHelper.stepCodeName].ToString(),
                        exp_StatusCodeName = statusCodeInfoDic[ContainerHelper.statusCodeName].ToString(),
                        
                        exp_TranslateStatusCode = translateStatusCode,
                        exp_TranslateStatusMsg = resultWebViewerData[HubBoxHelper.message].ToString(),
                        exp_TranslateData = resultWebViewerData.ContainsKey(HubBoxHelper.data) ? resultWebViewerData[HubBoxHelper.data].ToString() : string.Empty,
                        
                        exp_VisibleWebViewerImg = translateStatusCode.Equals(HubBoxHelper.successStatusCode) ? Visibility.Visible : Visibility.Collapsed,    // (파일) 변환 성공(success) -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "돋보기" 이미지 출력  
                        exp_VisibleTranslateImg = translateStatusCode.Equals(HubBoxHelper.translateStatusCode) ? Visibility.Visible : Visibility.Collapsed,   // (파일) 변환중입니다.(ACC에서 파일 변환 진행 중) -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "시계" 이미지 변경
                        exp_VisibleFailureImg = translateStatusCode.Equals(HubBoxHelper.failureStatusCode) ? Visibility.Visible : Visibility.Collapsed,       // (파일) 변환에 실패 한 파일입니다.(ACC에서 파일 변환 실패) -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "세모" 이미지 변경
                        exp_VisibleNotExistTranslateImg = translateStatusCode.Equals(HubBoxHelper.notExistTranslateStatusCode) ? Visibility.Visible : Visibility.Collapsed,   // (파일) 변환 내역이 없습니다.(해당 프로젝트ID, 팀ID, 컨테이너ID, 파일ID로 이미 변환 된 파일이 없는 경우)
                                                                                                                                                                              // -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "동그라미(사선 포함)" 이미지 변경
                    };

                    // 아래 Visibility 클래스 프로퍼티 모두 Visibility.Collapsed(표시 안함)일 경우 
                    // "exp_VisibleWebViewerImg", "exp_VisibleTranslateImg", "exp_VisibleFailureImg", "exp_VisibleNotExistTranslateImg"
                    if(Visibility.Collapsed == containerInfo.exp_VisibleWebViewerImg
                       && Visibility.Collapsed == containerInfo.exp_VisibleTranslateImg
                       && Visibility.Collapsed == containerInfo.exp_VisibleFailureImg
                       && Visibility.Collapsed == containerInfo.exp_VisibleNotExistTranslateImg) 
                        containerInfo.exp_VisibleErrorImg = Visibility.Visible;   // 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "오류" 이미지 변경

                    // Visibility 클래스 프로퍼티들 중 단 하나라도 Visibility.Visible(표시)일 경우 
                    // "exp_VisibleWebViewerImg", "exp_VisibleTranslateImg", "exp_VisibleFailureImg", "exp_VisibleNotExistTranslateImg"
                    else containerInfo.exp_VisibleErrorImg = Visibility.Collapsed;   // 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "오류" 이미지 표시 안함

                    // 컨테이너 잠금 상태 ("Y": 잠금 ON, "N": 잠금 OFF)
                    readYn = containerInfo.exp_ReadYn;

                    switch(readYn)
                    {
                        case ContainerHelper.yes:   // 체크아웃 요청
                            containerInfo.exp_WorkUserId = AppSetting.Default.LoginBase.GetLoginId();   // containerInfo.exp_WorkUserId 필드 값은 로그인 사용자 아이디로 변경하여 잠금상태 설정
                            break;

                        case ContainerHelper.no:   // 체크인 요청, 컨테이너 작업 취소 요청
                            // TODO : 컨테이너 작업 중인 사용자 아이디 데이터(workUserId) 구할 때 삼항 연산자 구현 (2023.12.20 minjae)
                            // 참고 URL - https://spaghetti-code.tistory.com/51
                            // TODO : switch - case 문 (체크인 요청 - case ContainerHelper.no:) 필요시 로직 수정 예정(2023.12.19 minjae)
                            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.12.20 minjae)
                            // 참고 URL - https://husk321.tistory.com/405
                            containerInfo.exp_WorkUserId = (container[ContainerHelper.workUserId] is null) ? string.Empty : container[ContainerHelper.workUserId].ToString();
                            break;

                        // TODO : 변수 "readYn"에 문자열 값이 잘못 들어간 경우가 존재하면 아래 "그 외" default 레이블 추가 구현 예정 (2024.10.30 minjae)
                        // default:   // 그 외
                        //     containerInfo.exp_WorkUserId = string.Empty;
                        //     break;
                    }

                    ContainerList.Add(containerInfo);
                }

                // TODO : 추후 필요시 컨테이너 리스트에 저장된 컨테이너 아이디 오름차순 정렬 구현 예정 (2024.10.30 minjae)
                // ContainerList.Sort();   
                ContainerDatas.AddRange(ContainerList.ToArray());
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion ContainerListCreateAsync

        #region FileAddAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 파일 추가
        /// </summary>
        private async Task FileAddAsync(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                //MessageBox.Show("파일 추가 개발 예정");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion FileAddAsync

        #region ShowWebViewerAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2025.08.22 minjae)
        /// <summary>
        /// 보기 (특정 컨테이너에 속한 파일 변환해서 웹뷰어로 출력)
        /// </summary>
        private async Task ShowWebViewerAsync(object obj)
        {
            ContainerView webViewerContainerInfo = null;   // 사용자가 선택한 변환 처리 완료된 파일의 상위 컨테이너 정보 
            string webViewerURL = string.Empty;    // 변환 처리 완료된 파일 웹뷰어 URL 주소 

            // TODO : 보기 (파일 정보 자세히 보기) 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2024.08.22 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            var parameters = (object[])obj;                      // 보기 버튼으로 부터 받은 특정 컨테이너 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileIdexp_FileId (Object Array -> IEnumerable 형변환) 

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] showWebViewerDatas" 형식으로 형변환 구현 (2024.08.22 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] showWebViewerDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // (파일) 변환 실패한 경우
                // 1. (파일) 변환중입니다.(ACC에서 파일 변환 진행 중)
                // 2. (파일) 변환에 실패 한 파일입니다.(ACC에서 파일 변환 실패)
                // 3. (파일) 변환 내역이 없습니다.(해당 프로젝트ID, 팀ID, 컨테이너ID, 파일ID로 이미 변환 된 파일이 없는 경우)
                // 4. 기타 등등...
                if(HubBoxHelper.successWebViewerVisibility != showWebViewerDatas[(int)EnumWebViewer.WEBVIEWER]) return;   // 메서드 종료

                // 사용자가 선택한 변환 처리 완료된 파일의 상위 컨테이너 정보 가져오기 
                webViewerContainerInfo = ContainerDatas.Where(containerInfo => containerInfo.exp_ProjectId.Equals(showWebViewerDatas[(int)EnumWebViewer.PROJECTID])
                                                                               && containerInfo.exp_TeamId.Equals(showWebViewerDatas[(int)EnumWebViewer.TEAMID])
                                                                               && containerInfo.exp_ContainerId.Equals(showWebViewerDatas[(int)EnumWebViewer.CONTAINERID])
                                                                               && containerInfo.exp_FileId.Equals(showWebViewerDatas[(int)EnumWebViewer.FILEID]))
                                                       .Select(containerInfo => containerInfo)
                                                       .FirstOrDefault();

                webViewerURL = webViewerContainerInfo.exp_TranslateData;   // 변환 처리 완료된 파일 웹뷰어 URL 주소 가져오기

                // TODO : 추후 필요시 if 조건절 안의 로직 수정 예정 (2024.10.30 minjae) 
                // 변환 처리 완료된 파일 웹뷰어 URL 주소가 공백 또는 null이 아닌 경우 
                if(false == string.IsNullOrWhiteSpace(webViewerURL))
                {
                    Process.Start(new ProcessStartInfo(webViewerURL) { UseShellExecute = true });   // 변환 처리 완료된 파일 웹뷰어 출력 실행 

                    // 변환 처리 완료된 파일 웹뷰어가 정상적으로 출력된 경우 
                    // 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보 (WebViewerPack) 객체 생성해서 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                    InputParamData.SetWebViewerPack(new WebViewerPack(showWebViewerDatas[(int)EnumWebViewer.PROJECTID],
                                                                      showWebViewerDatas[(int)EnumWebViewer.TEAMID],
                                                                      showWebViewerDatas[(int)EnumWebViewer.CONTAINERID],
                                                                      showWebViewerDatas[(int)EnumWebViewer.FILEID]));
                }

                // 변환 처리 완료된 파일 웹뷰어 주소가 공백 또는 null인 경우 
                else // 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 볼 수 없는 경우
                     // (예) - "code": 404, "message": "변환 내역이 없습니다."
                     // (예2) - "code": 204, "message": "변환에 실패 한 파일입니다."
                     // (예3) - "code": 202, "message": "변환중입니다."
                    throw new Exception($"* 웹뷰어 실행 오류 - ({ webViewerContainerInfo.exp_TranslateStatusCode }) *"
                                        + $"\r\n\r\n제목 - { webViewerContainerInfo.exp_Title }"
                                        + $"\r\n컨테이너 아이디 - { webViewerContainerInfo.exp_ContainerId.ToUpper() }"
                                        + $"\r\n사유 - { webViewerContainerInfo.exp_TranslateStatusMsg }");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion ShowWebViewerAsync

        #region DetailSearchAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 정보 + 파일정보 조회
        /// </summary>
        private async Task DetailSearchAsync(object obj)
        {
            List<Dictionary<string, object>> linkedNodeInfoList = new List<Dictionary<string, object>>();   // 연관된 부모 컨테이너 정보 리스트
            List<Dictionary<string, object>> metaDataList = new List<Dictionary<string, object>>();         // 메타데이터 정보 리스트

            Dictionary<string, object> detailDic = null;              // 컨테이너 정보, 파일정보 Dictionary                                                                    
            Dictionary<string, object> fileTypeCodeInfoDic = null;    // 파일유형 Dictionary
            Dictionary<string, object> taskDetailDic = null;          // 마일스톤 업무 Dictionary
            Dictionary<string, object> milestoneTaskInfoDic = null;   // 특정 마일스톤 업무 정보 Dictionary

            string createUserId = string.Empty;         // 작성자
            string containerId = string.Empty;          // 컨테이너 아이디
                                                                  
            string projectId = string.Empty;            // 프로젝트 아이디
            string teamId = string.Empty;               // 팀 아이디
                                                                     
            string revsnVal = string.Empty;             // 버전
            string title = string.Empty;                // 제목
            string fileTypeSystemCode = string.Empty;   // 파일유형 - systemCode
            string fileTypeCodeNm = string.Empty;       // 파일유형 - codeName
                                                                     
            string softMetaNm = string.Empty;           // 소프트웨어 metaName
            string softNm = string.Empty;               // 소프트웨어
                                                                     
            string softVerMetaNm = string.Empty;        // 소프트웨어버전 metaName
            string softVer = string.Empty;              // 소프트웨어버전
                                                                     
            string keywordMetaNm = string.Empty;        // 키워드/태그 metaName
            string keyword = string.Empty;              // 키워드/태그
                                                                     
            string taskDetailIdx = string.Empty;        // 마일스톤 업무 인덱스
            string taskDetailType = string.Empty;       // 마일스톤 업무 유형
                                                                     
            string fileNm = string.Empty;               // 파일명
            string fileId = string.Empty;               // 파일 아이디

            // TODO : 아래 주석친 소스 코드 3줄(parameters, containerDatas, containerDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
            // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
            // 참고 URL - https://nonstop-antoine.tistory.com/36
            // var parameters = obj as IEnumerable;                       // 컨테이너 아이디 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 
            // var containerDetails = new List<string>();                 // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDetails"
            // var containerDataDic = new Dictionary<string, string>();   // 컨테이너 정보 데이터를 담는 Dictionary 객체 containerDataDic 생성

            // 컨테이너 아이디 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf 
            var parameters = (object[])obj;                               // 컨테이너 아이디 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 
            // var containerDetails = parameters.ToList();                // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDetails"

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerDetails" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] containerDetails = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // Http 웹서버 통신 요청 Header, 컨테이너 정보(ContainerPack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                InputParamData.SetData(LoginView.GetRequestHeader(),
                                       new ContainerPack(containerDetails[(int)EnumContainerDetail.PROJECTID],
                                                         containerDetails[(int)EnumContainerDetail.TEAMID],
                                                         containerDetails[(int)EnumContainerDetail.CONTAINERID],
                                                         string.Empty,
                                                         string.Empty));   

                // 탐색기 화면 뷰(ExplorerV.xaml) 우측 하단에 할당할 선택한 특정 컨테이너 아이디 값 영어 대문자로 할당
                SelectedContainerId = InputParamData.GetContainerPack().GetContainerId().ToUpper();

                // Rest API 특정 컨테이너 상세조회 실행
                var total_Info_List = await ExplorerRestServer.GetDetailListAsync(InputParamData);   

                foreach(var infoDic in total_Info_List)
                {
                    // TODO : Dictionary 객체 "infoDic"에 존재하는 value 값 가져와서 리스트로 구현 (2023.10.18 minjae)
                    var containerList = infoDic.SelectMany(container => container.Value).ToList();

                    // 연관 컨테이너 정보(ContainerHelper.linkedNodeInfo)가 존재하지 않을 경우 continue 처리
                    if((int)EnumLinkedRelation.NONE == containerList.Count) continue;

                    // TODO : Dictionary<string, List<Dictionary<string, object>>> 클래스 객체 "infoDic"에 존재하는 Key 값 가져와서 리스트로 변환 (2023.10.18 minjae)
                    // 참고 URL - https://www.techiedelight.com/ko/get-list-of-keys-and-values-in-a-dictionary-in-csharp/
                    var infoTypeList = new List<string>(infoDic.Keys);

                    // TODO : 리스트 객체 "infoTypeList"의 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.18 minjae)
                    // 참고 URL - https://developer-talk.tistory.com/294
                    var infoType = infoTypeList.FirstOrDefault();

                    foreach(var data in containerList)
                    {
                        switch(infoType)
                        {
                            case ContainerHelper.linkedNodeInfo:   // 연관된 부모 컨테이너 정보 
                                linkedNodeInfoList.Add(data);
                                // TODO : 아래 프로퍼티 "linkedNodeMetaDataList"에 할당 해야할 메타 데이터 리스트 "MetaDataList"의 경우 
                                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                                //        문서 정리하여 공유 받고 구현 예정 (2023.11.01 minjae)
                                // linkedNodeMetaDataList = MetaDataList;
                                break;

                            case ContainerHelper.detail:   // 컨테이너 정보, 파일정보
                                // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 객체 containerDetailList 구현 예정 (2024.10.30 minjae)
                                // containerDetailList = infoList;
                                detailDic = data;
                                createUserId = data[ContainerHelper.createUserId].ToString();   // 작성자 
                                containerId = data[ContainerHelper.containerId].ToString();     // 컨테이너 아이디
                                                    
                                projectId = data[ContainerHelper.projectId].ToString();         // 프로젝트 아이디
                                teamId = data[ContainerHelper.teamId].ToString();               // 팀 아이디
                                                    
                                revsnVal = data[ContainerHelper.revsnVal].ToString();           // 버전
                                title = data[ContainerHelper.title].ToString();                 // 제목
                                fileNm = data[ContainerHelper.fileNm].ToString();               // 파일명
                                fileId = data[ContainerHelper.fileId].ToString();               // 파일 아이디 

                                // 파일유형 Dictionary
                                fileTypeCodeInfoDic = data[ContainerHelper.fileTypeCodeInfo].ToDictionary();
                                fileTypeSystemCode = fileTypeCodeInfoDic[ContainerHelper.systemCode].ToString();   // 파일유형 - systemCode
                                fileTypeCodeNm = fileTypeCodeInfoDic[ContainerHelper.codeName].ToString();         // 파일유형 - codeName
                                break;

                            case ContainerHelper.metaDataList:   // 메타데이터 정보
                                metaDataList.Add(data);

                                var metaCode = data[ContainerHelper.metaCode].ToString();

                                if(metaCode == ContainerHelper.soft_ifo)   // "metaCode": "soft-ifo" 일 경우 
                                {
                                    var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
                                    softMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();           // 소프트웨어 metaName
                                    softNm = data[ContainerHelper.metaComment].ToString();                      // 소프트웨어
                                }
 
                                if(metaCode == ContainerHelper.soft_ver)   // "metaCode": "soft-ver" 일 경우
                                {
                                    var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
                                    softVerMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();        // 소프트웨어버전 metaName
                                    softVer = data[ContainerHelper.metaComment].ToString();                     // 소프트웨어버전
                                }

                                if(metaCode == ContainerHelper.find_tag)   // "metaCode": "find-tag" 일 경우
                                {
                                    var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
                                    keywordMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();        // 키워드/태그 metaName
                                    keyword = data[ContainerHelper.metaComment].ToString();                     // 키워드/태그
                                }

                                break;

                            case ContainerHelper.taskDetail:   // 마일스톤 업무
                                taskDetailDic = data;
                                // TODO : Rest API 특정 컨테이너 상세조회 실행 되고 결과로 리턴 받은 json 데이터 결과값 중 "milestoneTaskInfo" 사라져서
                                //        탭 "미리보기" 항목에 출력되는 참조(연관)컨테이너 리스트가 출력되지 않는 오류 발생
                                //        해당 오류 원인 파악 후 아래 코드 taskDetailDic[ContainerHelper.milestoneTaskInfo] 개선 작업 예정 (2024.08.21 minjae)
                                milestoneTaskInfoDic = taskDetailDic is null ? null : taskDetailDic[ContainerHelper.milestoneTaskInfo].ToDictionary();

                                taskDetailIdx = milestoneTaskInfoDic is null ? ContainerHelper.taskIdxNone : milestoneTaskInfoDic[ContainerHelper.idx].ToString();

                                // TODO : 마일스톤 업무 리스트 객체 "taskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
                                // taskDetailList = infoDic[key];
                                // 마일스톤 - 업무 유형 데이터 마일스톤 관련 Rest API를 통해 데이터를 받아와야 하는 경우 아래 소스코드 수정 예정(2023.10.17 minjae)
                                // taskDetailType = taskDetailDic == null ? ContainerHelper.taskDetailTypeNone : taskDetailDic.Values.ToString();
                                taskDetailType       = milestoneTaskInfoDic is null ? ContainerHelper.taskTypeNone : milestoneTaskInfoDic[ContainerHelper.title].ToString();

                                break;
                        }
                    }
                }

                var detailData = new ContainerDetailView()
                {
                    LinkedNodeInfoList = linkedNodeInfoList,

                    // TODO : 아래 프로퍼티 "LinkedNodeMetaDataList"에 할당 해야할 메타 데이터 리스트 "linkedNodeMetaDataList"의 경우 
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 구현 예정 (2023.11.01 minjae)
                    // LinkedNodeMetaDataList = linkedNodeMetaDataList,

                    // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 객체 containerDetailList 구현 예정 (2023.10.17 minjae)
                    // ContainerDetailList = containerDetailList,
                    DetailDic = detailDic,
                                           
                    FileTypeCodeDic = fileTypeCodeInfoDic,
                                           
                    MetaDataList = metaDataList,

                    // TODO : 추후 필요시 "마일스톤 업무" 리스트 객체 "TaskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
                    // TaskDetailList = taskDetailList,
                    TaskDetailDic = taskDetailDic,
                    MilestoneTaskInfoDic = milestoneTaskInfoDic,

                    // Detail
                    exp_CreateUserId = createUserId,
                    exp_ContainerId = containerId,

                    // TODO : 추후 필요시 아래 주석친 코드 참고 (2024.10.30 minjae)
                    // exp_SelectedContainerId = ContainerPack.containerId.ToUpper(),   
                    // exp_UpperContainerId = containerId.ToUpper(),                 // 탐색기 화면 뷰(ExplorerV.xaml) 우측 하단에 할당할 컨테이너 아이디 값 영어 대문자로 할당

                    exp_ProjectId = projectId,
                    exp_TeamId = teamId, 

                    exp_RevsnVal = revsnVal,
                    exp_Title = title,
                    exp_FileTypeSystemCode = fileTypeSystemCode,
                    exp_FileTypeCodeName = fileTypeCodeNm,

                    // 메타 데이터 
                    // TODO : 프로퍼티 "exp_SoftMetaName" 필요시 사용 예정 (2024.10.30 minjae)
                    // exp_SoftMetaName = softMetaNm,
                    exp_SoftName = softNm,

                    // TODO : 프로퍼티 "exp_SoftVerMetaName" 필요시 사용 예정 (2024.10.30 minjae)
                    // exp_SoftVerMetaName = softVerMetaNm,
                    exp_SoftVer = softVer,

                    // TODO : 프로퍼티 "exp_KeywordTagMetaName" 필요시 사용 예정 (2024.10.30 minjae)
                    // exp_KeywordTagMetaName = keywordMetaNm,
                    exp_KeywordTag = keyword,

                    // 마일스톤
                    exp_TaskDetailIdx = taskDetailIdx,
                    exp_TaskDetailType = taskDetailType,

                    // Detail - 파일명(이름)
                    exp_FileName = fileNm,

                    // Detail - 파일 아이디
                    exp_FileId = fileId,

                    // 기본정보 탭 - Label (구분 항목)
                    exp_DivLabel = ContainerHelper.divLabel,
                    exp_ContentsLabel = ContainerHelper.contentsLabel,
                    exp_CreateUserLabel = ContainerHelper.createUserLabel,
                    exp_VersionLabel = ContainerHelper.versionLabel,
                    exp_TitleLabel = ContainerHelper.titleLabel,
                    exp_SoftLabel = softMetaNm,
                    exp_SoftVerLabel = softVerMetaNm,
                    exp_KeywordTagLabel = keywordMetaNm,
                    exp_FileTypeCodeLabel = ContainerHelper.fileTypeCodeLabel,
                    exp_TaskDetailLabel = ContainerHelper.taskDetailLabel,
                    exp_FileNameLabel = ContainerHelper.fileNameLabel
                };

                // TODO : 메서드 "DetailTypeListCreate" 호출시 전달 인자로 detailData 설정할 때
                //        ref(기존 변수 detailData안에 들어있는 값을 메서드 "DetailTypeListCreate" 실행시 수정)로 구현 (2023.10.23 minjae)
                // 참고 URL - https://yeko90.tistory.com/entry/c-%EA%B8%B0%EC%B4%88-ref-vs-out-%EC%B0%A8%EC%9D%B4
                // 참고 2 URL - https://kdsoft-zeros.tistory.com/142
                // 참고 3 URL - https://codingcoding.tistory.com/56
                DetailTypeListCreate(ref detailData, fileTypeCodeInfoDic, taskDetailDic);   // ComboBox "파일유형", "마일스톤" List 생성 

                DisplaySetting(CobimHelper.TypeOfDetailSearch);   // 화면 셋팅 처리

                // 연관 컨테이너의 정보(ContainerHelper.linkedNodeInfo)가 존재할 경우(EnumLinkedRelation.EXISTENCE 보다 크거나 같은 경우)
                // 비동기 메서드 "LinkedNodeInfoCreate" 호출
                if(linkedNodeInfoList.Count >= ((int)EnumLinkedRelation.EXISTENCE))
                {
                    // TODO : 아래 비동기 메서드 "LinkedNodeInfoCreate"에 전달할 메서드 파라미터 메타 데이터 리스트 "MetaDataList"의 경우 
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                    // await LinkedNodeInfoCreate(linkedNodeInfoList, linkedNodeMetaDataList); // 연관 컨테이너 정보 Tree 생성 
                    await LinkedNodeInfoCreate(linkedNodeInfoList); // 연관 컨테이너 정보 Tree 생성 
                }

                // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측으로 마일스톤 목록 조회 RestAPI 요청 후 개발 진행 (2023.10.20 minjae)
                // "마일스톤" List 생성 
                // var taskDetailList = await ExplorerRestServer.GetTaskDetailListAsync();
                // detailData.TaskDetailType = await ExplorerRestServer.GetTaskDetailListAsync();

                DetailList.Add(detailData);
                DetailDatas.AddRange(DetailList.ToArray());
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion DetailSearchAsync

        #region DetailTypeListCreate

        // TODO : 메서드 "DetailTypeListCreate" 호출시 메서드 파라미터로 pContainerInfo 설정할 때
        //        ref(기존 변수 pContainerInfo안에 들어있는 값을 메서드 "DetailTypeListCreate" 실행시 수정)로 구현 (2023.10.23 minjae)
        // 참고 URL - https://yeko90.tistory.com/entry/c-%EA%B8%B0%EC%B4%88-ref-vs-out-%EC%B0%A8%EC%9D%B4
        // 참고 2 URL - https://kdsoft-zeros.tistory.com/142
        // 참고 3 URL - https://codingcoding.tistory.com/56
        /// <summary>
        /// ComboBox "파일유형", "마일스톤" List 생성 메서드 DetailTypeListCreate
        /// </summary>
        private void DetailTypeListCreate(ref ContainerDetailView pContainerInfo, Dictionary<string, object> pFileTypeCodeInfoDic, Dictionary<string, object> pTaskDetailDic)
        {
            string fileTypeSystemCode = string.Empty;   // 파일유형 - systemCode
            string fileTypeCodeNm = string.Empty;       // 파일유형 - codeName

            string taskIdx = string.Empty;              // 마일스톤 - 업무 인덱스                             
            string taskType = string.Empty;             // 마일스톤 - 업무 유형

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                fileTypeSystemCode = pContainerInfo.exp_FileTypeSystemCode;   // 파일유형 - systemCode
                fileTypeCodeNm = pContainerInfo.exp_FileTypeCodeName;         // 파일유형 - codeName

                taskIdx = pContainerInfo.exp_TaskDetailIdx;                   // 마일스톤 - 업무 인덱스
                taskType = pContainerInfo.exp_TaskDetailType;                 // 마일스톤 - 업무 유형

                // TODO : pContainerInfo.FileTypeList.Clear(); 소스코드 실행시 FileTypeList 값이 null이므로 null Exception 발생해서 사용 안함.(2023.10.23 minjae)
                // pContainerInfo.FileTypeList.Clear();    // ComboBox - 파일유형 리스트 초기화

                // TODO : ComboBox "파일유형" 리스트 생성 구현 (2023.10.24 minjae)
                pContainerInfo.FileTypeList = new List<FileTypeView>
                {
                    // TODO : 개체 이니셜라이저 사용해서 파일 유형 리스트 객체 pContainerInfo.FileTypeList 생성시 객체 값(exp_systemCode, exp_codeName) 할당 (2023.11.23 minjae)
                    // 참고 URL - https://hyokye0ng.tistory.com/86
                    new FileTypeView(ContainerHelper.systemCodeModel, ContainerHelper.codeNameModel),
                    new FileTypeView(ContainerHelper.systemCodeReprt, ContainerHelper.codeNameReprt)
                };

                // TODO : ComboBox "마일스톤" 리스트 생성 구현 (2023.10.24 minjae)
                pContainerInfo.TaskDetailList = new List<TaskDetailView>
                {
                    // TODO : 개체 이니셜라이저 사용해서 마일스톤 리스트 객체 pContainerInfo.FileTypeList 생성시 객체 값(exp_idx, exp_title) 할당 (2023.11.23 minjae)
                    // 참고 URL - https://hyokye0ng.tistory.com/86
                    new TaskDetailView (ContainerHelper.taskIdxNone,     ContainerHelper.taskTypeNone     ),     // 관련업무선택
                    new TaskDetailView (ContainerHelper.taskIdxDefine,   ContainerHelper.taskTypeDefine   ),     // 요구사항정의            
                    new TaskDetailView (ContainerHelper.taskIdxEntity,   ContainerHelper.taskTypeEntity   ),     // 엔터티도출
                    new TaskDetailView (ContainerHelper.taskIdxCreation, ContainerHelper.taskTypeCreation ),     // 요구사항정의서 작성
                    new TaskDetailView (ContainerHelper.taskIdxUI,       ContainerHelper.taskTypeUI       )      // UI설계서 작성
                };


                // TODO : 리스트 객체 "pContainerInfo.FileTypeList"의 조건을 만족하는 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.18 minjae)
                // 참고 URL - https://developer-talk.tistory.com/294
                // TODO : 리스트 객체 "pContainerInfo.FileTypeList"의 FirstOrDefault 메서드 사용할 때, Contains 메서드 사용해서 조건 1, 조건 2 만족하는 데이터 있는지 찾기 (2023.10.23 minjae) 
                // 참고 URL - https://developer-talk.tistory.com/585 
                // FirstOrDefault (where 조건절과 하는 역할 비슷하고 조건을 만족하는 요소가 존재하는지 체크 기능)
                // 조건 1 - pContainerInfo.FileTypeList에 할당된 (파일유형 - systemCode - fileType.exp_systemCode) 값과 fileTypeSystemCode에 할당된 (컨테이너 정보 파일유형 - systemCode) 가 동일해야 한다.
                // 조건 2 - pContainerInfo.FileTypeList에 할당된 (파일유형 - codeName - fileType.exp_codeName) 값과 fileTypeCodeNm에 할당된 (컨테이너 정보 파일유형 - codeName) 가 동일해야 한다. 
                pContainerInfo.SelectParam.FileTypeCodeInfo = pContainerInfo.FileTypeList.FirstOrDefault(fileType => fileType.exp_systemCode.Contains(fileTypeSystemCode)
                                                                                                                  && fileType.exp_codeName.Contains(fileTypeCodeNm));

                // TODO : 리스트 객체 "pContainerInfo.TaskDetailList"의 조건을 만족하는 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.24 minjae)
                pContainerInfo.SelectParam.TaskDetail = pContainerInfo.TaskDetailList.FirstOrDefault(taskDetailType => taskDetailType.exp_idx.Contains(taskIdx)
                                                                                                                    && taskDetailType.exp_title.Contains(taskType));

                // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 minjae)
                // pContainerInfo.TaskDetailType = pMilestoneTaskInfoDic.Where().Select()).
                // pContainerInfo.TaskDetailType.Title
                // var result = await ExplorerRestServer.GetTaskDetailListAsync();


                // TODO : ComboxBox - 파일유형에 바인딩할 BindableCollection에 값 할당 및 서버로부터 받은 값을 화면에 출력 할 수 있도록
                //        메서드 파라미터 "pContainerInfo", "pFileTypeCodeInfoDic" 활용하기 (2023.10.20 minjae)
                //        컨테이너 정보 영역 "마일스톤" ComboBox 구현시 바인딩할 BindableCollection 객체 TaskDetailType 구현(2023.10.20 minjae)
                // pContainerInfo.FileType
                // exp_systemCode 
                // exp_codeName 
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion DetailTypeListCreate

        #region LinkedNodeInfoCreate

        // TODO : 아래 비동기 메서드 "LinkedNodeInfoCreate"에 전달할 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우 
        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        /// <summary>
        /// 연관 컨테이너 정보 Tree 생성 
        /// </summary>
        private async Task LinkedNodeInfoCreate(List<Dictionary<string, object>> pLinkedNodeInfoList)
        {
            string rootContainerId = string.Empty;      // 루트 컨테이너 아이디

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : Linq 확장메서드 Where 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                // TODO : 추후 using static CobimExplorer.Data.Models.CobimBase.Explorer.ExplorerView; 문을 사용하지 않고도 ExplorerView.LinkedNodeView -> LinkedNodeView만 쓸 수 있도록 구현하기 (2023.11.21 minjae) 
                // 루트 노드(컨테이너) 찾기
                // 부모 컨테이너 아이디(ContainerHelper.targetId)는 존재하지 않고(== null),
                // 자식 컨테이너 아이디(또는 자기 자신 컨테이너 ID)(ContainerHelper.sourceId)만 존재하는 경우(is not null)
                LinkedNodeView rootContainer = pLinkedNodeInfoList.Where(node => node[ContainerHelper.targetId] is null
                                                                              && node[ContainerHelper.sourceId] is not null)
                                                                  //.Select(node => new LinkedNodeView
                                                                  //                    {
                                                                  //                        // TreeView 영역 루트 노드 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
                                                                  //                        exp_TreeName = node[ContainerHelper.sourceId].ToString().ToUpper()
                                                                  //                        // LinkedNodeInfoDic = linkedNodeInfoList.Where(node => node.Values.Equals())
                                                                  //                    })

                                                                  // TreeView 영역 루트 노드 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
                                                                  .Select(node => new LinkedNodeView(node[ContainerHelper.sourceId].ToString().ToUpper()))
                                                                  .FirstOrDefault();


                // TODO : 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                // TODO : 루트 컨테이너 "rootContainer"에 루트 컨테이너 아이디 "rootContainerId"에 속하는 메타데이터를
                //        메타데이터 리스트 객체 "pLinkedNodeMetaDataList"에서 Linq로 찾아서 "new LinkedNodeView" 프로퍼티 "LinkedNodeInfoDic"에 할당하기 (2023.11.01 minjae)
                // LinkedNodeView rootContainer = pLinkedNodeMetaDataList.Where(meta => meta[ContainerHelper.containerId].ToString().Equals(rootContainerId))
                //                                                       .Select(meta => new LinkedNodeView
                //                                                                           {
                //                                                                               exp_TreeName = rootContainerId,
                //                                                                               LinkedNodeInfoDic = meta
                //                                                                           })
                //                                                       .FirstOrDefault();

                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                // 루트 노드(컨테이너)가 존재하지 않을 경우 리턴 
                if(rootContainer is null) return;

                LinkedNodeInfoList.Add(rootContainer);

                // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                // 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가
                // await FillLinkedNodes(rootContainer, pLinkedNodeInfoList, pLinkedNodeMetaDataList);
                await FillLinkedNodes(rootContainer, pLinkedNodeInfoList);

                LinkedNodeInfoDatas.Clear();   // 연관 컨테이너 정보 Collection 초기화
                LinkedNodeInfoDatas.AddRange(LinkedNodeInfoList.ToArray());
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion LinkedNodeInfoCreate

        #region FillLinkedNodes

        // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        /// <summary>
        /// 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가
        /// </summary>
        private async Task FillLinkedNodes(LinkedNodeView parentContainer, List<Dictionary<string, object>> pLinkedNodeInfoList)
        {
            string subContainerId = string.Empty;   // 서브 컨테이너 아이디

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : pLinkedNodeInfoList의 WHERE 조건절 -> TreeView 영역 부모 노드 이름 프로퍼티 " parentContainer.exp_TreeName" 영어 대문자 -> 소문자로 변환(ToLower()) 후
                //        node[ContainerHelper.targetId]?.ToString() 문자열(영어 소문자) 값과 비교하여 같은 값만 추출하기 (2023.11.01 minjae)
                // 부모 컨테이너(루트 또는 서브)에 하위 참조관계에 속한 자식 컨테이너 데이터 추출 
                var childNodes = pLinkedNodeInfoList.Where(node => parentContainer.exp_TreeName.ToLower() == node[ContainerHelper.targetId]?.ToString());

                if(false == childNodes.Any()) return;   // 자식 컨테이너가 존재하지 않는 경우 메서드 종료 

                foreach(var childNode in childNodes)
                {
                    // TODO : TreeView 영역 부모 노드 하위 자식 노드(childNode) 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
                    subContainerId = childNode[ContainerHelper.sourceId].ToString().ToUpper();

                    var subContainer = new LinkedNodeView(childNode[ContainerHelper.sourceId].ToString().ToUpper());   // 참조 컨테이너 "subContainer" 객체 생성

                    // TODO : 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                    // var subContainer = pLinkedNodeMetaDataList.Where(meta => meta[ContainerHelper.containerId].ToString().Equals(subContainerId))
                    //                                           .Select(meta => new LinkedNodeView 
                    //                                                               { 
                    //                                                                   exp_TreeName = subContainerId,
                    //                                                                   LinkedNodeInfoDic = meta
                    //                                                               })
                    //                                           .FirstOrDefault();

                    // 부모 컨테이너(루트 또는 서브)의 자식 노드로 subContainer 추가 
                    parentContainer.ChildNodes.Add(subContainer);

                    // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
                    //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
                    //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
                    //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
                    // 서브 컨테이너의 자식 노드로 또 다른 서브 컨테이너 추가 
                    // await FillLinkedNodes(subContainer, pLinkedNodeInfoList, pLinkedNodeMetaDataList);
                    await FillLinkedNodes(subContainer, pLinkedNodeInfoList);
                }
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion FillLinkedNodes

        #region LinkedNodeInfoSearchAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 연관 컨테이너(참조관계 - 부모/자식 컨테이너) 메타데이터 정보 출력
        /// </summary>
        private async Task LinkedNodeInfoSearchAsync(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // MessageBox.Show("연관 컨테이너(참조관계 - 부모/자식 컨테이너) 메타데이터 정보 출력 기능 필요시 구현 예정");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion LinkedNodeInfoSearchAsync

        #region RequestAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 검토요청
        /// </summary>
        private async Task RequestAsync(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // MessageBox.Show("승인 / 검토 요청 필요시 구현 예정");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion RequestAsync

        #region UpdateAsync

        // TODO : 수정 메서드 "UpdateAsync" 필요시 구현 예정 (2024.10.28 minjae)
        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 수정
        /// </summary>
        private async Task UpdateAsync(object obj)
        {
            // TODO : 아래 주석친 소스 코드 (upContainerDatas) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
            // Object 배열 객체 obj 리스트로 변환
            // 참고 URL - https://nonstop-antoine.tistory.com/36
            // var upContainerDatas = obj as IEnumerable;

            // TODO : 아래 주석친 코드 필요시 참고 (2024.08.26 minjae)
            // 컨테이너 아이디 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf 
            // var parameters = (object[])obj;                                   // 컨테이너 아이디 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerUpdate" 형식으로 형변환 구현 (2024.01.03 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            // string[] containerUpdate = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
            // var upContainerDatas    = parameters.ToList();          // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "upContainerDatas"

            // string upSystemCode  = string.Empty;              // 수정할 systemCode - typmodel, typreprt
            // string upCodeName    = string.Empty;              // 수정할 codeName - 모델 / 보고서

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try 
            {
                // TODO : 아래 foreach 문 필요시 참고 (2023.12.05 minjae)
                //foreach (ContainerDetailView data in upContainerDatas)
                //{
                //    // TODO : 아래 소스코드에 들어갈 데이터가 추가로 필요할 시 소스코드 수정 예정 (2023.10.23 minjae)
                //    //        UpdateParam 프로퍼티 필요 없어도 변경된 데이터(data.SelectParam)들이 정상적으로 서버로 넘어갈 수 있으면 UpdateParam 삭제 예정 (2023.10.23 minjae)
                //    UpdateParam = new Param();
                //    UpdateParam = data.SelectParam;
                //}

                // var test = DetailDatas.(detailData => detailData.SelectParam).Object;
                // UpdateParam  = obj as IEnumerable; // .Select(s => s.SelectParam.FileTypeCodeInfo);

                // UpdateAsync 메서드 실행시 ComboBox 파일유형에서 변경된 값을 가져오도록 Linq 식으로 구현하기 (2023.10.23 minjae) 
                // var obj      = null;
                // FileTypeView fileTypeCodeInfo = ContainerDatas.Select(x => x.Param.FileTypeCodeInfo);

                // upSystemCode = fileTypeCodeInfo
                // Select(x => x.Param.FileTypeCodeInfo);

                //MessageBox.Show("수정 기능 필요시 구현 예정");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion UpdateAsync

        #region UpLoadAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 탭 "변경" - 컨테이너 변경(컨테이너 안에 저장한 파일 A -> B) 및 저장 
        /// </summary>
        private async Task UpLoadAsync(object obj)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {

            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion UpLoadAsync

        #region Btn_MouseRightButtonDown

        // TODO : 필요시 마우스 오른쪽 클릭 이벤트 메서드 "Btn_MouseRightButtonDown" 뷰모델에 구현 예정 (2023.11.02 minjae)
        // TODO : 마우스 오른쪽 클릭 이벤트 메서드 "Btn_MouseRightButtonDown" 구현 (2023.11.02 minjae)
        // 참고 URL - https://yeko90.tistory.com/entry/c-wpf-%EB%A7%88%EC%9A%B0%EC%8A%A4-%EC%9D%B4%EB%B2%A4%ED%8A%B8%EB%8D%94%EB%B8%94%ED%81%B4%EB%A6%AD-%EC%98%A4%EB%A5%B8%EC%AA%BD%ED%81%B4%EB%A6%AD-%EC%99%BC%EC%AA%BD-%ED%81%B4%EB%A6%AD
        // 참고 2 URL - https://poorman.tistory.com/263
        // public void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        // {
        //     MessageBox.Show("마우스 오른쪽 클릭 : 향후 체크인/체크아웃 항목 구현 예정");
        // }

        #endregion Btn_MouseRightButtonDown

        #region TestAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 테스트
        /// </summary>
        private async Task TestAsync(object obj)
        {
            // bool IsCompleted = false;   // 테스트 처리 완료 여부

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // MessageBox.Show("테스트 중입니다.");

                // 테스트 처리 시작 
                // explorerVM.IsCompleted = false;
                // IsCompleted = false;

                var vm = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.Test_LoadingWork);

                vm.ShowWindow();

                // 테스트 처리 중 화면 출력 직후 1분 대기 
                // await Task.Delay(TimeSpan.FromMinutes(1));

                // 테스트 처리 중 화면 출력 직후 30초 대기 
                await Task.Delay(TimeSpan.FromSeconds(30));

                // 테스트 처리 완료
                // explorerVM.IsCompleted = true;
                // IsCompleted = true;
                vm.RequestClose(null);

                //if (IsCompleted) 
                //{
                //    // DeactivateItem(vm);

                //    vm.RequestClose(null);

                //    // CloseItem(vm);
                //}

                // vm.RequestClose(explorerVM.IsCompleted);

                // vm.RequestClose(IsCompleted);

                // vm.OnClose();
                // _ = vm.DisposeChildren;

                // CommonProgressV commonProgressV = new CommonProgressV();
                // commonProgressV.Show();

                // await Task.Delay(TimeSpan.FromMinutes(1));

                // commonProgressV.Close();


                // TODO : await Task.Yield(); 사용법 제대로 확인 후 사용 예정 (2023.12.04 minjae)
                // 참고 URL - https://www.sysnet.pe.kr/2/0/12241
                // 참고 2 URL - https://www.sysnet.pe.kr/2/0/12245
                // 참고 3 URL - https://stackoverflow.com/questions/22645024/when-would-i-use-task-yield
                // await Task.Yield();

                // TODO : 로그인/체크아웃/체크인/작업취소 시 사용할 공용 컨트롤 BackgroundWorkerControl.xaml 화면 출력 테스크 코드 필요시 참고 (2024.01.03 minjae) 
                // 참고 URL - https://2bbear-gds.tistory.com/entry/WPF-C%EC%9C%BC%EB%A1%9C-UserControll%EC%9D%84-%EB%B0%94%EB%A1%9C-%EB%8B%A4%EC%9D%B4%EC%96%BC%EB%A1%9C%EA%B7%B8-%ED%99%94%EB%A9%B4%EC%9C%BC%EB%A1%9C-%EC%B6%9C%EB%A0%A5-%ED%95%98%EA%B3%A0-%EC%8B%B6%EC%9D%84%EB%95%8C
                // BackgroundWorkerControl testDialog = new BackgroundWorkerControl();
                // Window window = new Window { Title = "테스트", Content = testDialog, Width=300, Height=200};
                // window.ShowDialog();

                // return;
            }
            catch(Exception ex)
            {
                // 테스트 처리중 화면 종료
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            finally
            {
                // TODO : 추후 필요 시 finally문에 코드 추가 예정 (2023.08.24 minjae)
            }
            return;
        }

        #endregion TestAsync

        #region IsOpenedFile

        // TODO : 해당 파일 경로(filePath)에 존재하는 Revit 문서가 Revit 응용 프로그램에서 열려있는 경우
        //        체크인 처리시 오류가 발생하므로 Revit 문서가 Revit 응용 프로그램에서 열려 있으면 오류 처리 메서드 "IsOpenedFile" 구현 (2024.10.22 minjae)
        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.file.open?view=net-8.0
        // 참고 2 URL - https://www.csharpstudy.com/Tip/Tip-file-open.aspx
        // 참고 3 URL - https://bigexecution.tistory.com/288
        // 참고 4 URL - https://blog.naver.com/wonjinho81/221630185577

        /// <summary>
        /// Revit 파일(확장자 .rvt) 열려있는지 여부 확인
        /// </summary>
        private bool IsOpenedFile(string filePath)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
                return false;   // Revit 파일(확장자 .rvt)이 열려있지 않은 경우 
            }
            catch (Exception ex)
            {
                // 이미 열려있는 Revit 파일(확장자 .rvt)을 다시 open 하는경우 예외 발생
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                return true;
            }
        }

        #endregion IsOpenedFile

        #region DownLoadAsync

        // TODO : 체크아웃, 체크인, 파일 다운로드 중복 처리 시 컨텍스트 메뉴창 비활성화(Enabled = false;) 구현 예정 (2024.09.03 minjae)
        // 컨테스트 메뉴창 객체 이름 설정(예) x:Name="ExplorerContextMenu"

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 파일 다운로드
        /// </summary>
        private async Task DownLoadAsync(object obj)
        {
            var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.DownLoad_LoadingWork);   // 다운로드 처리 중 화면 뷰모델 할당

            // TODO : 다운로드 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            var parameters = (object[])obj;   // 다운로드 버튼으로 부터 받은 다운로드 받을 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] downLoadDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] downLoadDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

            string fileId = string.Empty;     // 파일 아이디
            string fileName = string.Empty;   // 파일명(이름)

            string resMsg = string.Empty;     // 파일 다운로드 처리 결과(성공 또는 실패) 문자열 초기화  

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                if(true == IsDownLoadWorking)   // 파일 다운로드 처리 중인 경우
                    throw new Exception(CobimHelper.DownLoad_LoadingWork);

                IsDownLoadWorking = true;

                // Http 웹서버 통신 요청 Header, 컨테이너 정보(ContainerPack), 파일 정보(FilePack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                InputParamData.SetData(LoginView.GetRequestHeader(),
                                       new ContainerPack(downLoadDatas[(int)EnumDownLoad.PROJECTID],
                                                         downLoadDatas[(int)EnumDownLoad.TEAMID],
                                                         downLoadDatas[(int)EnumDownLoad.CONTAINERID],
                                                         string.Empty,
                                                         string.Empty),
                                       new FilePack(downLoadDatas[(int)EnumDownLoad.FILEID],
                                                    downLoadDatas[(int)EnumDownLoad.FILENAME]));

                loadingWorkVM.ShowWindow();   // 파일 다운로드 처리 중 화면 출력

                await ExplorerRestServer.GetFileDownLoadAsync(InputParamData);   // 파일 다운로드 시작

                resMsg = "success";  // 파일 다운로드 성공  
                MessageBox.Show("파일 다운로드 완료!");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
                resMsg = ex.Message;   // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
            }
            finally
            {
                // 파일 다운로드 처리 중이 아닌 경우 
                if(resMsg != CobimHelper.DownLoad_LoadingWork)
                {
                    // TODO : 파일 다운로드 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
                    // 참고 URL - https://cosmosproject.tistory.com/563
                    IsDownLoadWorking = false;          // 파일 다운로드 처리 중 종료
                    loadingWorkVM.RequestClose(null);   // 파일 다운로드 처리 중 화면 종료
                }
            }
            return;
        }

        #endregion DownLoadAsync

        #region CheckOutAsync

        // TODO : 체크아웃, 체크인, 파일 다운로드 중복 처리 시 컨텍스트 메뉴창 비활성화(Enabled = false;) 구현 예정 (2024.09.03 minjae)
        // 컨테스트 메뉴창 객체 이름 설정(예) x:Name="ExplorerContextMenu"

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드  
        /// </summary>
        private async Task CheckOutAsync(object obj)
        {
            var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CheckOut_LoadingWork);   // 체크아웃 처리 중 화면 뷰모델 할당

            string checkOutContainerId = string.Empty;   // 체크아웃 요청 컨테이너 아이디
            string linkedContainerId = string.Empty;     // 연관 컨테이너 아이디

            // TODO : Context Menu(컨텍스트 메뉴) "체크아웃" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            var parameters = (object[])obj;   // Context Menu(컨텍스트 메뉴) "체크아웃" 클릭시 받은 체크아웃 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId") (Object Array -> IEnumerable 형변환)

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] checkOutDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] checkOutDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

            string resMsg = string.Empty;   // 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드 처리 결과(성공 또는 실패) 문자열 초기화

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // 연관 컨테이너 파일 다운로드 처리 중인 상태에서 다른 컨테이너의 체크아웃을 중복 요청하는 경우 
                if(true == IsLinkedContainerWorking)
                    throw new Exception(CobimHelper.LinkedContainer_LoadingWork);

                if(true == IsCheckOutWorking)   // 체크아웃 처리 중인 경우
                    throw new Exception(CobimHelper.CheckOut_LoadingWork);

                IsCheckOutWorking = true;   // 체크아웃 처리 중 시작

                // 1 단계 : 비동기 메서드 "CheckOutAsync"에 파라미터로 받은 전달 인자(object 배열 객체)로 "checkOutDatas" 리스트에 값 할당 
                //          체크아웃 요청 컨테이너 아이디 "checkOutContainerId", "folderId", "ContainerPack", "FilePack" 생성
                // 체크아웃 요청 컨테이너 아이디 문자열 값 string 클래스 객체 checkOutContainerId에 할당 
                checkOutContainerId = checkOutDatas[(int)EnumCheckOut.CONTAINERID];

                _ContainerPack = new ContainerPack(checkOutDatas[(int)EnumCheckOut.PROJECTID],
                                                   checkOutDatas[(int)EnumCheckOut.TEAMID],
                                                   checkOutDatas[(int)EnumCheckOut.CONTAINERID],
                                                   SelectedFolderId,
                                                   string.Empty);

                // Http 웹서버 통신 요청 Header, 컨테이너 정보(ContainerPack), 파일 정보(FilePack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                InputParamData.SetData(LoginView.GetRequestHeader(),
                                       _ContainerPack,
                                       new FilePack(checkOutDatas[(int)EnumCheckOut.FILEID],
                                                    checkOutDatas[(int)EnumCheckOut.FILENAME]));   

                loadingWorkVM.ShowWindow();

                // 2 단계 : Rest API 10. 컨테이너 체크아웃 + 다운로드 호출 
                // 체크아웃 호출을 위해 만족 해야할 사항(조건)
                // 1) 컨테이너 생성자 == 체크아웃 요청자(로그인 사용자 아이디)
                // 2) 컨테이너 readYn 필드 값이 'N' 일 때
                // 3) 검토 요청 중인 컨테이너가 아닐 때(statusCode != 'comstt20')
                await ExplorerRestServer.GetCheckOutAsync(InputParamData);

                // TODO : 체크아웃 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
                // 참고 URL - https://cosmosproject.tistory.com/563
                loadingWorkVM.RequestClose(null);   // 체크아웃 처리 중 화면 종료 

                // 4 단계: 체크아웃 요청한 컨테이너 아이디와 연관된 컨테이너 목록 조회 시작 
                var linkedContainerList = await ExplorerRestServer.GetLinkedContainerListAsync(InputParamData);

                // (참조/연관) 컨테이너 목록이 존재하지 않는 경우 
                if(linkedContainerList.Count == (int)EnumLinkedRelation.NONE)
                    MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 목록이 존재하지 않습니다.");

                // TODO : 아래 주석친 테스트 코드 (연관 컨테이너가 존재하는 경우) 코드 필요시 참고 (2024.01.03 minjae)
                // TODO : 아래 연관 컨테이너가 존재하는 경우 리스트 객체 "linkedContainerList" 컨테이너 아이디(containerId) 오름차순 정렬 (Sort 메서드) (2023.11.29 minjae)
                // 참고 URL - https://www.techiedelight.com/ko/sort-list-in-descending-order-in-csharp/
                // (참조/연관) 컨테이너가 존재하는 경우 
                // if (linkedContainerList.Count >= (int)EnumLinkedRelation.EXISTENCE)
                //     linkedContainerList.Sort((containerA, containerB) => containerA[ContainerHelper.containerId].ToString().CompareTo(containerB[ContainerHelper.containerId].ToString()));

                // (참조/연관) 컨테이너 목록이 존재하는 경우 
                // && (참조/연관) 컨테이너 파일 다운로드 처리 중 화면 출력 
                if(linkedContainerList.Count >= (int)EnumLinkedRelation.EXISTENCE)
                {
                    // MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 다운로드 시작합니다.");

                    IsLinkedContainerWorking = true;   // (참조/연관) 컨테이너 파일 다운로드 처리 중 시작

                    loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.LinkedContainer_LoadingWork);   // (참조/연관) 컨테이너 파일 다운로드 처리 중 화면 뷰모델 할당
                    loadingWorkVM.ShowWindow();   // (참조/연관) 컨테이너 파일 다운로드 처리 중 화면 출력   

                    foreach(var linkedContainer in linkedContainerList) 
                    {
                        // (참조/연관) 컨테이너에 속한 파일 정보(FilePack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                        InputParamData.SetFilePack(new FilePack(linkedContainer[ContainerHelper.fileId].ToString(),
                                                                linkedContainer[ContainerHelper.fileNm].ToString()));

                        // TODO : (참조/연관) 컨테이너 다운로드 하는 경우 다운로드 받은 파일의 상위 컨테이너(컨테이너 아이디)을
                        //        각각의 (참조/연관) 컨테이너 아이디가 아니라 루트 컨테이너 아이디(체크아웃 요청한 컨테이너 아이디)
                        //        하나로만 상위 컨테이너로 설정 해야 하므로 아래 소스코드는 주석 처리 진행 (2023.12.14 minjae)
                        // 체크아웃 요청한 컨테이너 아이디 
                        // ContainerPack.containerId = linkedContainer[ContainerHelper.containerId].ToString();
                        // InputParamData.GetContainerPack().SetContainerId(linkedContainer[ContainerHelper.containerId].ToString());

                        linkedContainerId = linkedContainer[ContainerHelper.containerId].ToString();   // (참조/연관) 컨테이너 아이디

                        // (참조/연관) 컨테이너 아이디 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                        InputParamData.GetContainerPack().SetLinkedContainerId(linkedContainerId);

                        // TODO : 체크아웃 요청한 컨테이너 아이디(checkOutContainerId) 문자열 값과 연관 컨테이너 아이디(linkedContainerId)의 문자열이 동일한 경우 파일 다운로드 대상 제외
                        //        이미 체크아웃 + 파일 다운로드 처리 동시에 하였으므로 파일 중복 다운로드 안 하기 위해 continue 처리 구현 (2024.10.31 minjae)
                        if(checkOutContainerId == linkedContainerId) continue;

                        // (참조/연관) 컨테이너에 속한 파일 다운로드 진행
                        else await ExplorerRestServer.GetFileDownLoadAsync(InputParamData);
                    }

                    MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 파일 다운로드 완료!");
                }

                resMsg = "success";   // 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드 성공
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

                resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
            }
            finally
            {
                // 컨테이너 체크아웃 또는 (참조/연관) 관계 컨테이너 파일 다운로드 처리 중이 아닌 경우
                //if(true == IsCheckOutWorking || true == IsLinkedContainerWorking)
                if(resMsg != CobimHelper.CheckOut_LoadingWork && resMsg != CobimHelper.LinkedContainer_LoadingWork)
                {
                    IsCheckOutWorking = false;
                    IsLinkedContainerWorking = false;

                    // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
                    //        체크아웃 비동기 메서드 "CheckOutAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
                    //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
                    //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 메서드 파라미터 _ContainerPack에
                    //        프로퍼티 "SelectedFolderId" 같이 추가해서 전달함. (2024.10.30 minjae) 
                    // 체크아웃 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
                    await ContainerListCreateAsync(_ContainerPack);

                    // TODO : 체크아웃 처리 중 또는 연관 컨테이너 파일 다운로드 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.05 minjae)
                    // 참고 URL - https://cosmosproject.tistory.com/563
                    loadingWorkVM.RequestClose(null);   // 체크아웃 처리 중 또는 연관 컨테이너 파일 다운로드 처리 중 화면 종료 


                    // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.11.01 minjae)
                    // if(false == string.IsNullOrWhiteSpace(SelectedFolderId))
                    //     await ContainerListCreateAsync(_ContainerPack);
                    // else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");
                }
            }
            return;
        }

        #endregion CheckOutAsync

        #region CheckInAsync

        // TODO : 체크아웃, 체크인, 파일 다운로드 중복 처리 시 컨텍스트 메뉴창 비활성화(Enabled = false;) 구현 예정 (2024.09.03 minjae)
        // 컨테스트 메뉴창 객체 이름 설정(예) x:Name="ExplorerContextMenu"

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 + HubBox 파일 변환 요청 
        /// 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 성공하면 
        /// HubBox 파일 변환 요청 Rest API 메서드 HubBoxRestServer.PostTranslateAsync 호출 해야함.
        /// </summary>
        private async Task CheckInAsync(object obj)
        {
            var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CheckIn_LoadingWork);   // 체크인 처리 중 화면 뷰모델 할당

            CheckInPartVM checkInPartVM = null;   // 체크인 팝업 화면 객체 초기화

            // string resultTranslateCode = string.Empty;   // 특정 컨테이너에 속한 파일 변환 처리 결과 상태 코드 문자열 초기화
            // string resultTranslateMsg = string.Empty;    // 특정 컨테이너에 속한 파일 변환 처리 관련 공통 메세지 초기화

            // TODO : Context Menu(컨텍스트 메뉴) "체크인" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            var parameters = (object[])obj;              // Context Menu(컨텍스트 메뉴) "체크인" 클릭시 받은 체크인 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId", "exp_FileName") (Object Array -> IEnumerable 형변환) 

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] checkInDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95

            // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열
            //        단, 변경할 때, (기존) object 배열에 속하는 데이터 중 "제목"을
            //        갖고 있는 데이터를 제외하기 위해 Linq 조건절 확장 메서드 Where 사용해서
            //        특정 인덱스 "(int)EnumCheckIn.TITLE" 제외한 
            //        데이터만 추출해서 string 배열 객체 "checkInDatas" 구현 (2024.08.26 minjae)
            // 참고 URL - https://chatgpt.com/c/332ba391-669d-4e7b-8e5a-af86cbd6516e
            string[] checkInDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString())
                                         .Where((checkInRequestData, index) => index != (int)EnumCheckIn.TITLE)
                                         .Select(checkInRequestData => checkInRequestData)
                                         .ToArray();
         
            // string[] translateDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());   // HubBox 파일 변환 요청 Rest API 요청 데이터 string 배열 객체 "translateDatas"

            // string checkInContainerId = string.Empty;   // 체크인 요청 컨테이너 아이디            
            // string statusCode = string.Empty;           // 컨테이너 검토상태("comstt10": 미진행, "comstt20": 진행, "comstt30": 완료)
            // string readYn     = string.Empty;           // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
            // string workUserId = string.Empty;           // 컨테이너 작업중인 사용자 아이디
                                                           
            string fileName = string.Empty;                // 체크인 처리할 컨테이너에 속한 파일명
            string fileId   = string.Empty;                // 체크인 처리하기 전 파일 아이디  
            string revsnVal = string.Empty;                // 체크인 처리하기 전 파일 버전 
                                                           
            string checkInFileId = string.Empty;           // 체크인 처리 완료 후 파일 아이디  
            string checkInRevsnVal = string.Empty;         // 체크인 처리 완료 후 파일 버전 

            // TODO : C# 문자열 보간 기능 사용해서 웹서버로 업로드 할 상위 폴더 경로 문자열로 변환 (2024.09.02 minjae)
            // 참고 URL - https://gaeunhan.tistory.com/61
            string dirPath = string.Empty;                 // 웹서버로 업로드 할 파일의 상위 디렉토리(폴더) 경로
            string filePath= string.Empty;                 // 웹서버로 업로드 할 파일 전체 경로 

            string deleteContainerId = string.Empty;       // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 컨테이너 아이디
            string deleteDirPath = string.Empty;           // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 디렉토리(폴더) 경로
            
            string resMsg = string.Empty;                  // 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 처리 결과(성공 또는 실패) 문자열 초기화

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                if(true == IsTranslateWorking)   // 파일 변환 처리 중인 상태에서 다른 컨테이너의 체크인을 중복 요청하는 경우
                    throw new Exception(CobimHelper.Translate_LoadingWork);
 
                if(true == IsCheckInWorking)     // 체크인 처리 중인 경우
                    throw new Exception(CobimHelper.CheckIn_LoadingWork);

                IsCheckInWorking = true;         // 체크인 처리 중 시작 

                checkInPartVM = Container.Get<CheckInPartVM>().SetCheckInPack(new CheckInPack(checkInDatas[(int)EnumCheckIn.FOLDERID],
                                                                                              string.Empty));
                checkInPartVM.ShowDialog();      // 체크인 팝업 화면 생성 및 결과 리턴

                // TODO : await Task.Yield(); 사용법 제대로 확인 후 사용 예정 (2023.12.04 minjae)
                // 참고 URL - https://www.sysnet.pe.kr/2/0/12241
                // 참고 2 URL - https://www.sysnet.pe.kr/2/0/12245
                // 참고 3 URL - https://stackoverflow.com/questions/22645024/when-would-i-use-task-yield
                await Task.Yield();

                if(true == checkInPartVM.IsExit) return;   // 체크인 팝업 화면 종료(닫기("X") 버튼 클릭)시 메서드 CheckInAsync 종료 

                // TODO : 다른 PC에서도 파일 업로드 기능이 실행될 수 있도록 웹서버로 업로드 할 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.12.05 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
                // 웹서버로 업로드 할 파일의 상위 디렉토리 폴더 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002"
                // SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 사용
                // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.10.14 minjae)
                // dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}" +
                //           $"\\{CobimHelper.CobimBaseDirectory}" +
                //           $"\\{CobimHelper.AssemblyName}" +
                //           $"\\{checkInDatas[(int)EnumCheckIn.PROJECTID]}" +
                //           $"\\{checkInDatas[(int)EnumCheckIn.TEAMID]}" +
                //           $"\\{checkInDatas[(int)EnumCheckIn.CONTAINERID]}";

                // TODO : StringBuilder 클래스 객체 sbDirPath 생성 -> string 클래스 객체 dirPath에 문자열 추가 (2024.10.15 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                // StringBuilder sbDirPath = new StringBuilder($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
                // sbDirPath.Append($"\\{CobimHelper.AssemblyName}");
                StringBuilder sbDirPath = new StringBuilder($"{_CommonDirectoryPath}");
                sbDirPath.Append($"\\{checkInDatas[(int)EnumCheckIn.PROJECTID]}");
                sbDirPath.Append($"\\{checkInDatas[(int)EnumCheckIn.TEAMID]}");
                sbDirPath.Append($"\\{checkInDatas[(int)EnumCheckIn.CONTAINERID]}");
                
                dirPath = sbDirPath.ToString();

                // 체크인 처리 하기전 파일 이름, 파일 아이디, 파일 버전 문자열 할당 
                fileName = checkInDatas[(int)EnumCheckIn.FILENAME];
                fileId = checkInDatas[(int)EnumCheckIn.FILEID];
                revsnVal = checkInDatas[(int)EnumCheckIn.REVSNVAL];

                // TODO : 웹서버로 업로드 할 파일 전체 경로 구현 (2023.12.04 minjae)
                // 참고 URL - https://dlgnlfus.tistory.com/129
                filePath = dirPath + $"\\{fileName}";   // 웹서버로 업로드 할 파일 전체 경로 

                // TODO : 웹서버로 업로드 할 파일 전체 경로에 있는 파일 정보 불러 오도록 구현 (2023.12.05 minjae)
                // 참고 URL   - https://chashtag.tistory.com/37
                // 참고 2 URL - https://www.delftstack.com/ko/howto/csharp/get-filesize-in-csharp/
                // 참고 3 URL - https://dlgnlfus.tistory.com/129
                if(true == File.Exists(filePath))
                {
                    if(true == IsOpenedFile(filePath))   // Revit 파일(확장자 .rvt), AutoCAD 파일(확장자 .dwg)이 이미 열려있는 경우 
                        throw new Exception($"파일명 : {fileName}\r\n\r\n해당 문서가 열려있는 상태입니다.\r\n문서를 닫은 후 다시 체크인 요청하세요.");

                    _ContainerPack = new ContainerPack(checkInDatas[(int)EnumCheckIn.PROJECTID],
                                                       checkInDatas[(int)EnumCheckIn.TEAMID],
                                                       checkInDatas[(int)EnumCheckIn.CONTAINERID],
                                                       SelectedFolderId,
                                                       string.Empty);

                    // Rest API 컨테이너 체크인 요청하기 전 웹서버로 업로드 할 파일 읽어오기(File.OpenRead)
                    using(FileStream upLoadFileStream = File.OpenRead(filePath))
                    {
                        _CheckInPack = checkInPartVM.GetCheckInPack();   // 사용자가 comment 입력한 컨테이너 체크인 + 파일 웹서버 업로드 정보 가져오기

                        // Http 웹서버 통신 요청 Header, 컨테이너 정보(ContainerPack), 파일 정보(FilePack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                        InputParamData.SetData(LoginView.GetRequestHeader(),
                                               _ContainerPack,
                                               new FilePack(string.Empty, 
                                                            checkInDatas[(int)EnumCheckIn.FILENAME],
                                                            upLoadFileStream.Length.ToString(),
                                                            upLoadFileStream),
                                               _CheckInPack);

                        loadingWorkVM.ShowWindow();   // 체크인 처리 중 화면 출력 

                        var resultCheckInData = await ExplorerRestServer.PostCheckInAsync(InputParamData);   // 컨테이너 체크인 + 파일 웹서버 업로드 

                        // Dictionary 객체 resultCheckInData 특정 키(Key) "fileName", "fileId", "revsnVal"에 속하는 값(Value) 문자열로 변환 (2024.08.26 minjae)
                        // 참고 URL - https://developer-talk.tistory.com/694
                        // TODO : 체크인 처리 완료 후 파일명 string 객체 checkInFileName 필요시 사용 예정 (2024.08.26 minjae)
                        // checkInFileName = resultCheckInData[ContainerHelper.fileNm].ToString();
                        checkInFileId = resultCheckInData[ContainerHelper.fileId].ToString();
                        checkInRevsnVal = resultCheckInData[ContainerHelper.revsnVal].ToString();

                        // TODO : 체크인 처리중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
                        // 참고 URL - https://cosmosproject.tistory.com/563
                        loadingWorkVM.RequestClose(null);   // 체크인 처리 중 화면 종료 

                    }   // Rest API 컨테이너 체크인 요청 성공후 웹서버로 업로드 할 파일 리소스 해제(Dispose()) 처리 -> 리소스 해제(Dispose()) 해야 사용자 로컬 PC에 설치된 파일 삭제 처리 가능

                    // 컨테이너 체크인 + 업로드 성공한 경우 
                    // && 파일 변환 처리 중 화면 출력 
                    // checkInFileId(체크인 처리 완료 후 파일 아이디) != fileId(체크인 처리하기 전 파일 아이디) 
                    // checkInRevsnVal(체크인 처리 완료 후 파일 버전) != revsnVal(체크인 처리하기 전 파일 버전)
                    // if(checkInFileId != fileId && checkInRevsnVal != revsnVal && false == IsTranslateWorking)
                    if(checkInFileId != fileId && checkInRevsnVal != revsnVal)
                    {
                        IsTranslateWorking = true;   // 파일 변환 처리 시작

                        // 체크인 완료된 이후 변경된 파일 아이디 값(checkInFileId)을 메서드 "InputParamData.GetFilePack().SetFileId()"에 인자 값으로 전달
                        InputParamData.GetFilePack().SetFileId(checkInFileId);

                        // HubBox 파일 변환 정보(TranslatePack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                        InputParamData.SetTranslatePack(new TranslatePack(_ContainerPack.GetProjectId(),
                                                                          _ContainerPack.GetTeamId(),
                                                                          _ContainerPack.GetContainerId(),
                                                                          checkInFileId));

                        // checkInContainerId = InputParamData.GetTranslatePack().GetContainerId();;
                        // MessageBox.Show($"컨테이너 아이디 {checkInContainerId}\r\n체크인 완료!\r\n파일 변환 요청 시작합니다.");

                        // 연관 컨테이너 파일 다운로드 처리 중 화면 뷰모델 할당
                        loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.Translate_LoadingWork);

                        // 파일 변환 처리 중 화면 출력 
                        loadingWorkVM.ShowWindow();

                        // HubBox Rest API 3. 컨테이너에 속한 특정 파일 변환 API 호출 
                        var resultTranslateData = await HubBoxRestServer.PostTranslateAsync(InputParamData);

                        // MessageBox.Show("체크인 완료!");

                        // TODO : 아래 주석친 코드 필요시 참고 (2024.10.31 minjae)
                        // TODO : Dictionary 객체 resultTranslateData에 담긴 파일 변환 API 결과 값 중 파일 변환 성공/처리 중일 경우 키 "code"에 매핑된 값으로 할당
                        //        만약 파일 변환 실패했을 경우 키 "status"에 매핑된 값으로 할당 하도록 삼항연산자 사용하여 로직 구현하기 (2024.08.26) 
                        // bool result =  resultTranslateData.TryGetValue(resultTranslateData[EnumTranslateResult.CODE.ToDescription()], out resultTranslateCode))
                        // resultTranslateCode = true == resultTranslateData.ContainsKey(EnumTranslateResult.CODE.ToDescription()) ? 
                        //                         resultTranslateData[EnumTranslateResult.CODE.ToDescription()].ToString() : 
                        //                       (true == resultTranslateData.ContainsKey(EnumTranslateResult.STATUS.ToDescription()) ? resultTranslateData[EnumTranslateResult.STATUS.ToDescription()].ToString() : null);
                        // if(true == resultTranslateData.ContainsKey(EnumTranslateResult.CODE.ToDescription())) 
                        //     resultTranslateCode = resultTranslateData[EnumTranslateResult.CODE.ToDescription()].ToString();

                        // else if(true == resultTranslateData.ContainsKey(EnumTranslateResult.STATUS.ToDescription()))
                        //     resultTranslateCode = resultTranslateData[EnumTranslateResult.STATUS.ToDescription()].ToString();

                        // else resultTranslateCode = null;

                        // TryGetValue vs ContainsKey 차이
                        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.dictionary-2.trygetvalue?view=net-8.0
                        // 참고 2 URL - https://stackoverflow.com/questions/38377318/contains-in-dictionary-in-c-sharp
                        // 참고 3 URL - https://ckhyeok.tistory.com/37
                        // 참고 4 URL - https://m.blog.naver.com/PostView.nhn?blogId=crusaderholy&logNo=100112263890&proxyReferer=https:%2F%2Fwww.google.com%2F
                        // 참고 5 URL - https://coding-shop.tistory.com/53
                        // 참고 6 URL - https://developer-talk.tistory.com/686

                        // 컨테이너에 속한 특정 파일 변환 Dictionary 객체 resultTranslateData 에
                        // 키값 "code", "status" 둘 다 포함되지 않은 경우 
                        // if(false == resultTranslateData.TryGetValue(EnumTranslateResult.CODE.ToDescription(), out resultTranslateCode)
                        //    && false == resultTranslateData.TryGetValue(EnumTranslateResult.STATUS.ToDescription(), out resultTranslateCode)) 
                        //      resultTranslateCode = null;

                        // resultTranslateMsg =  $"* 파일 변환 안내 *"
                        //                     + $"\r\n\r\n처리 결과 - {resultTranslateData[EnumTranslateResult.MESSAGE.ToDescription()]} ({resultTranslateCode})"
                        //                     + $"\r\n컨테이너 아이디  - {translateDatas[(int)EnumTranslate.CONTAINERID].ToUpper()}"
                        //                     + $"\r\n제목 - {translateDatas[(int)EnumTranslate.TITLE]}"
                        //                     + $"\r\n파일 이름 - {translateDatas[(int)EnumTranslate.TRANSLATE_FILENAME]}";

                        // MessageBox.Show(resultTranslateMsg);   // 파일 변환 처리 결과 메세지 출력 
                    }
                    // 컨테이너 체크인 + 업로드 실패한 경우
                    else throw new Exception("체크인 오류 발생!\r\n담당자에게 문의하세요.");

                    // TODO : 체크인 완료(+ 컨테이너에 속한 파일 웹서버로 업로드) 후 파일 삭제 기능이 실행될 수 있도록 삭제 할 파일의 상위 폴더 경로 "deleteDirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2024.08.30 minjae)
                    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
                    // 삭제할 파일의 상위 폴더 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002"
                    // SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 사용
                    deleteDirPath = dirPath;
                   
                    // 삭제할 파일의 상위 컨테이터 폴더(_ContainerPack.GetContainerId())가 존재하는 경우 
                    if(true == Directory.Exists(deleteDirPath))
                    {
                        deleteContainerId = checkInDatas[(int)EnumCheckIn.CONTAINERID];
                        // MessageBox.Show($"로컬에 존재하는 루트 컨테이너 폴더({deleteContainerId})\r\n다운로드 받은 파일 삭제 진행합니다.");

                        // TODO : 체크인 완료 후(+ 파일 웹서버 업로드 완료) 로컬 PC에 다운로드 받은 기존 루트 컨테이너 폴더(deleteContainerId) 및 폴더에 속한 모든 파일 삭제 
                        // TODO : 폴더(디렉토리) 클래스 "Directory", "DirectoryInfo" 차이점 비교 후 static 클래스 "Directory" 사용 (2024.08.30 minjae)
                        // 참고 URL - https://nomad-programmer.tistory.com/207
                        // TODO : Directory.Delete 메서드의 2번째 파라미터 값 설정 구현 (2024.08.30 minjae)
                        //        deleteDirPath 경로의 폴더(디렉터리), 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거하려면 true, 제거하지 않으려면 false 설정
                        //        true로 설정하면 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거할 때, 재귀적으로 (recursive) Directory.Delete 메서드 실행 
                        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.directory.delete?view=net-8.0
                        Directory.Delete(deleteDirPath, true);

                        // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 완료한 경우 
                        if(false == Directory.Exists(deleteDirPath))
                            MessageBox.Show($"체크인 완료!\r\n루트 컨테이너 폴더 ({deleteContainerId})\r\n다운로드 받은 파일 삭제 완료");
                        // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 실패한 경우 
                        else throw new Exception($"[오류] 루트 컨테이너 폴더 ({deleteContainerId})\r\n삭제 오류\r\n담당자에게 문의하세요.");
                    }
                    resMsg = "success";   // 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 성공
                }
                else throw new Exception("[오류] 컨테이너 체크인 + 업로드 파일이 존재하지 않습니다.\r\n다시 확인 하세요."); 
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
                resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
            }
            finally
            {
                // 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 처리 중이 아닌 경우
                // && 파일 변환 처리 중이 아닌 경우
                //if (true == IsCheckInWorking || true == IsTranslateWorking)
                if(resMsg != CobimHelper.CheckIn_LoadingWork && resMsg != CobimHelper.Translate_LoadingWork)
                {
                    // 체크인 처리 중 또는 파일 변환 처리 완료
                    IsCheckInWorking = false;
                    IsTranslateWorking = false;

                    // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크인" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
                    //        체크인 비동기 메서드 "CheckInAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
                    //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
                    //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 메서드 파라미터 _ContainerPack에
                    //        프로퍼티 "SelectedFolderId" 같이 추가해서 전달함. (2024.10.30 minjae) 
                    // 체크인 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
                    await ContainerListCreateAsync(_ContainerPack);

                    // TODO : 체크인 처리 중 또는 파일 변환 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.05 minjae)
                    // 참고 URL - https://cosmosproject.tistory.com/563
                    loadingWorkVM.RequestClose(null);   // 체크인 처리 중 또는 파일 변환 처리 중 화면 종료

                    // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.11.01 minjae)
                    // if(false == string.IsNullOrWhiteSpace(SelectedFolderId))
                    //     await ContainerListCreateAsync(_ContainerPack);
                    // else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");
                }
            }
            return;
        }

        #endregion CheckInAsync

        #region OpenCheckIn

        // TODO : 체크인 팝업화면 오픈 메서드 "OpenCheckIn" 필요시 사용 예정 (2023.12.04 minjae)
        /// <summary>
        /// 체크인 팝업화면 오픈 
        /// </summary>
        //private void OpenCheckIn()
        //{
        //    // var vm = this.Container.Get<CheckInPartVM>().ToShowData();    // 체크인 화면 생성 및 결과 리턴 
        //    var con = Container.Get<CheckInPartVM>();
        //    WindowManager.ShowDialog(con);
        //}

        #endregion OpenCheckIn

        #region CancelWorkAsync

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 작업 취소
        /// 체크아웃 상태의 컨테이너 ID 락 해제 (ReadYn = "Y" -> "N")
        /// 작업자 이름(exp_WorkUserId) 공백(null) 처리
        /// </summary>
        private async Task CancelWorkAsync(object obj)
        {
            var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CancelWork_LoadingWork);   // 컨테이너 작업 취소 처리 중 화면 뷰모델 할당 

            // TODO : Context Menu(컨텍스트 메뉴) "작업취소" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.19 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            // var parameters = (object[])obj;
            object[] parameters = (object[])obj;

            // TODO : Context Menu(컨텍스트 메뉴) "작업취소" 클릭시 MultiCommandParameter 데이터(obj)를
            //        (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] cancelWorkDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] cancelWorkDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
            // var cancelWorkDatas = parameters.ToList();   // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "cancelWorkDatas"

            string fileName = string.Empty;                      // 컨테이너 작업 취소 처리할 컨테이너에 속한 파일명

            // TODO : C# 문자열 보간 기능 사용해서 삭제 처리 할 파일 + 상위 디렉토리(폴더) 경로 문자열로 변환 (2023.12.20 minjae)
            // 참고 URL - https://gaeunhan.tistory.com/61
            string dirPath = string.Empty;                       // 컨테이너 작업 취소 처리할 파일의 상위 디렉토리(폴더) 경로
            string filePath = string.Empty;                      // 컨테이너 작업 취소 처리할 파일 전체 경로 

            string deleteContainerId = string.Empty;             // 컨테이너 작업 취소 완료 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 컨테이너 아이디
            string deleteDirPath = string.Empty;                 // 컨테이너 작업 취소 완료 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 디렉토리(폴더) 경로

            string resMsg = string.Empty;                        // 컨테이너 작업 취소 처리 결과(성공 또는 실패) 문자열 초기화

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            { 
                if(true == IsCancelWorking)   // 컨테이너 작업 취소 처리 중인 경우 
                    throw new Exception(CobimHelper.CancelWork_LoadingWork);

                IsCancelWorking = true;   // 컨테이너 작업 취소 처리 중 시작 

                // TODO : 다른 PC에서도 루트 컨테이너 폴더 및 폴더에 속한 모든 파일 삭제 기능이 실행될 수 있도록 업로드 할 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.12.20 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
                // 파일 업로드 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002" - (SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) 사용)
                // dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetEntryAssembly().GetName().Name}\\{ContainerPack.projectId}\\{ContainerPack.teamId}\\{ContainerPack.containerId}";

                // TODO : StringBuilder 클래스 객체 sbDirPath 생성 -> string 클래스 객체 dirPath에 문자열 추가 (2024.10.15 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                // StringBuilder sbDirPath = new StringBuilder($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
                // sbDirPath.Append($"\\{CobimHelper.AssemblyName}");
                StringBuilder sbDirPath = new StringBuilder($"{_CommonDirectoryPath}");
                sbDirPath.Append($"\\{cancelWorkDatas[(int)EnumCancelWork.PROJECTID]}");
                sbDirPath.Append($"\\{cancelWorkDatas[(int)EnumCancelWork.TEAMID]}");
                sbDirPath.Append($"\\{cancelWorkDatas[(int)EnumCancelWork.CONTAINERID]}");

                dirPath = sbDirPath.ToString();

                fileName = cancelWorkDatas[(int)EnumCancelWork.FILENAME];

                // TODO : 컨테이너 작업 취소 처리할 파일 전체 경로 구현 (2023.12.04 minjae)
                // 참고 URL - https://dlgnlfus.tistory.com/129
                filePath = dirPath + $"\\{fileName}";   // 컨테이너 작업 취소 처리할 파일 전체 경로 

                if(true == File.Exists(filePath) && true == IsOpenedFile(filePath))   // Revit 파일(확장자 .rvt), AutoCAD 파일(확장자 .dwg)이 존재하고 이미 열려있는 경우 
                    throw new Exception($"파일명 : {fileName}\r\n\r\n해당 문서가 열려있는 상태입니다.\r\n문서를 닫은 후 다시 작업취소 요청하세요.");

                // 컨테이너 리스트 생성(재조회 - Refresh 기능 포함) 메서드 "ContainerListCreateAsync" 호출시 필요한 인자 "_ContainerPack" 객체 새로 생성 
                _ContainerPack = new ContainerPack(cancelWorkDatas[(int)EnumCancelWork.PROJECTID],
                                                   cancelWorkDatas[(int)EnumCancelWork.TEAMID],
                                                   cancelWorkDatas[(int)EnumCancelWork.CONTAINERID],
                                                   SelectedFolderId,
                                                   string.Empty);

                // Http 웹서버 통신 요청 Header, 컨테이너 작업 취소 정보 (CancelWorkPack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
                InputParamData.SetCancelWorkData(LoginView.GetRequestHeader(),
                                                 new CancelWorkPack(cancelWorkDatas[(int)EnumCancelWork.PROJECTID],
                                                                    cancelWorkDatas[(int)EnumCancelWork.TEAMID],
                                                                    cancelWorkDatas[(int)EnumCancelWork.CONTAINERID]));

                loadingWorkVM.ShowWindow();   // 컨테이너 작업 취소 처리 중 화면 출력

                // TODO : Rest API 17. 컨테이너 작업 취소 Rest API 비동기 메서드 "ExplorerRestServer.PostCancelWorkAsync" 호출시
                //        출력되는 오류 메시지 "오류 메시지 "{"status":"V000","message":"필수 입력항목이 누락 되었습니다.","error":"com.bim.common.exception.ServiceRuleException"}""
                //        오류 원인 파악 및 해결 하기 (2024.10.31 minjae)
                // 1 단계 : Rest API 17. 컨테이너 작업 취소 호출 
                var resultData = await ExplorerRestServer.PostCancelWorkAsync(InputParamData);
                

                // TODO : 컨테이너 작업 취소 결과 확인 로직 오류 발생시 수정 예정 (2023.12.21 minjae)
                // 2 단계 : 컨테이너 작업 취소 결과 확인
                // 조건 1 - resultData 값이 null 이 아님
                // 조건 2 - resultData[ContainerHelper.workUserId] 값이 null임.
                // 조건 3 - resultData[ContainerHelper.readYn] 문자열 변환시 'N'과 같아야 함.
                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.12.20 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                bool bCancelWork = (resultData is not null)
                                   && (resultData[ContainerHelper.workUserId] is null)
                                   && (resultData[ContainerHelper.readYn].ToString().Equals(ContainerHelper.no));

                // 컨테이너 작업 취소가 성공한 경우 
                if(true == bCancelWork)
                {
                    // MessageBox.Show("컨테이너 작업 취소 완료!");

                    deleteDirPath = dirPath;
                    
                    // 삭제 처리 할 파일 + 상위 디렉토리(폴더)가 존재하는 경우 
                    if(true == Directory.Exists(deleteDirPath))
                    {
                        deleteContainerId = cancelWorkDatas[(int)EnumCancelWork.CONTAINERID];

                        // MessageBox.Show($"로컬에 존재하는 루트 컨테이너 폴더({deleteContainerId})\r\n다운로드 받은 파일 삭제 진행합니다.");

                        // 3 단계 : 컨테이너 작업 취소 완료 후 로컬 PC에 다운로드 받은 기존 루트 컨테이너 폴더(deleteContainerId) 및 폴더에 속한 모든 파일 삭제 
                        // TODO : 폴더(디렉토리) 클래스 "Directory", "DirectoryInfo" 차이점 비교 후 static 클래스 "Directory" 사용 (2023.12.20 minjae)
                        // 참고 URL - https://nomad-programmer.tistory.com/207
                        // TODO : Directory.Delete 메서드의 2번째 파라미터 값 설정 구현 (2023.12.20 minjae)
                        //        deleteDirPath 경로의 폴더(디렉터리), 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거하려면 true, 제거하지 않으려면 false 설정
                        //        true로 설정하면 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거할 때, 재귀적으로 (recursive) Directory.Delete 메서드 실행 
                        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.directory.delete?view=net-8.0
                        Directory.Delete(deleteDirPath, true);

                        // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 완료한 경우 
                        if(false == Directory.Exists(deleteDirPath))
                            MessageBox.Show($"컨테이너 작업 취소 완료!\r\n루트 컨테이너 폴더 ({deleteContainerId})\r\n다운로드 받은 파일 삭제 완료");

                        else throw new Exception($"[오류] 루트 컨테이너 폴더 ({deleteContainerId})\r\n삭제 오류\r\n담당자에게 문의하세요.");
                    }
                    resMsg = "success";   // 컨테이너 작업 취소 성공
                }
                // 컨테이너 작업 취소 실패한 경우 
                else throw new Exception($"[오류] 컨테이너 작업 취소 실패\r\n담당자에게 문의 하세요.");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

                resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
            }
            finally
            {
                // 컨테이너 작업 취소 처리 중이 아닌 경우 
                if(resMsg != CobimHelper.CancelWork_LoadingWork)
                {
                    IsCancelWorking = false;  // 컨테이너 작업 취소 처리 완료  

                    // 4 단계 : 컨테이너 작업 취소 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
                    // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "작업취소" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
                    //        컨테이너 작업 취소 비동기 메서드 "CancelWorkAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
                    //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
                    //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 메서드 파라미터 _ContainerPack에
                    //        프로퍼티 "SelectedFolderId" 같이 추가해서 전달함. (2024.10.30 minjae) 
                    await ContainerListCreateAsync(_ContainerPack);

                    // TODO : 컨테이너 작업 취소 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
                    // 참고 URL - https://cosmosproject.tistory.com/563
                    loadingWorkVM.RequestClose(null);   // 컨테이너 작업 취소 처리 중 화면 종료

                    // TODO : 아래 주석친 코드 필요시 참고 (2024.10.31 minjae)
                    // if(false == string.IsNullOrWhiteSpace(SelectedFolderId))
                    //     await ContainerListCreateAsync(_ContainerPack);
                    // else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");
                }
            }
            return;
        }

        #endregion CancelWorkAsync

        
        #region OpenFileAsync

        /// <summary>
        /// 컨테이너에 속한 Revit 파일(확장자 .rvt), AutoCAD 파일(확장자 .dwg, .dxf) 열기
        /// </summary>
        private async Task OpenFileAsync(object obj)
        {
            var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.OpenFile_LoadingWork);   // 파일열기 처리 중 화면 뷰모델 할당

            // TODO : Context Menu(컨텍스트 메뉴) "파일열기" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
            // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
            var parameters = (object[])obj;

            // TODO : Context Menu(컨텍스트 메뉴) "파일열기" 클릭시 MultiCommandParameter 데이터(obj)를
            //        (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] openFileDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
            string[] openFileDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

            string fileName = string.Empty;                      // 파일열기 처리할 컨테이너에 속한 파일명

            string extensionName = string.Empty;                 // 파일열기 처리할 파일 확장자 이름(.rvt, .rft, .rfa, .ifc, .dwg, .dxf 등등...)

            // TODO : C# 문자열 보간 기능 사용해서 파일열기 처리할 파일 + 상위 디렉토리(폴더) 경로 문자열로 변환 (2023.12.20 minjae)
            // 참고 URL - https://gaeunhan.tistory.com/61
            string dirPath = string.Empty;                       // 파일열기 처리할 파일의 상위 디렉토리(폴더) 경로
            string filePath = string.Empty;                      // 파일열기 처리할 파일 전체 경로 

            string resMsg = string.Empty;                        // 파일열기 처리 결과(성공 또는 실패) 문자열 초기화

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                if(true == IsOpenFileWorking)   // 파일열기 처리 중인 경우
                    throw new Exception(CobimHelper.OpenFile_LoadingWork);

                IsOpenFileWorking = true;   // 컨테이너에 속한 파일열기 처리 시작

                // TODO : 다른 PC에서도 루트 컨테이너 폴더 및 폴더에 속한 모든 파일 삭제 기능이 실행될 수 있도록 업로드 할 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.12.20 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
                // 파일 업로드 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002" - (SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) 사용)
                // dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetEntryAssembly().GetName().Name}\\{ContainerPack.projectId}\\{ContainerPack.teamId}\\{ContainerPack.containerId}";

                // TODO : StringBuilder 클래스 객체 sbDirPath 생성 -> string 클래스 객체 dirPath에 문자열 추가 (2024.10.15 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                // StringBuilder sbDirPath = new StringBuilder($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
                // sbDirPath.Append($"\\{CobimHelper.AssemblyName}");
                StringBuilder sbDirPath = new StringBuilder($"{_CommonDirectoryPath}");
                sbDirPath.Append($"\\{openFileDatas[(int)EnumOpenFile.PROJECTID]}");
                sbDirPath.Append($"\\{openFileDatas[(int)EnumOpenFile.TEAMID]}");
                sbDirPath.Append($"\\{openFileDatas[(int)EnumOpenFile.CONTAINERID]}");

                dirPath = sbDirPath.ToString();

                fileName = openFileDatas[(int)EnumOpenFile.FILENAME];

                // TODO : 파일열기 처리할 파일 전체 경로 구현 (2023.12.04 minjae)
                // 참고 URL - https://dlgnlfus.tistory.com/129
                filePath = dirPath + $"\\{fileName}";   // 파일열기 처리할 파일 전체 경로 


                // TODO : 파일 전체 경로(filePath)에 있는 파일 정보 불러 오도록 구현 (2024.11.22 minjae)
                // 참고 URL   - https://chashtag.tistory.com/37
                // 참고 2 URL - https://dlgnlfus.tistory.com/129
                if(true == File.Exists(filePath))
                {
                    if(true == IsOpenedFile(filePath))   // Revit 파일(확장자 .rvt), AutoCAD 파일(확장자 .dwg, .dxf) 이 이미 열려있는 경우 
                        throw new Exception($"파일명 : {fileName}\r\n\r\n해당 문서가 이미 열려있는 상태입니다.");

                    // TODO : 메서드 "Path.GetExtension" 사용해서
                    //        파일열기 처리 하고자 하는 파일의 확장자 이름(.rvt, .rft, .rfa, .ifc 등등...) 문자열로 구하기 (2024.10.21 minjae)
                    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.path.getextension?view=net-8.0
                    extensionName = Path.GetExtension(filePath);

                    // 파일 확장자가 Revit 문서(.rvt), AutoCAD 문서(.dwg, .dxf)가 아닌 경우
                    if(extensionName != CobimHelper.RVTExtensionName 
                       && extensionName != CobimHelper.DWGExtensionName 
                       && extensionName != CobimHelper.DXFExtensionName)
                        throw new Exception($"확장자({CobimHelper.RVTExtensionName}, {CobimHelper.DWGExtensionName}, {CobimHelper.DXFExtensionName}) 제외 " +
                                            $"\r\n다른 형식 파일열기 불가!" +
                                            $"\r\n확인 부탁드립니다.");


                    // TODO : 1. 해당 "파일열기" 기능 실행시 중복된 파일이 존재하는 경우 (기존에 다운로드 받은 파일이 존재)
                    //        기존에 다운로드 받은 파일(.확장자)과 새로 다운로드 받은 파일(.확장자) 중에 어떤 파일을 연결 프로그램으로 열기를 진행할지는 "김명환 소장님"과 소통 후 구현 예정 
                    //        2. 해당 "파일열기" 기능 실행시 Revit 도면(확장자 .rvt)만 파일을 열고 그 외 나머지 파일들은 파일열기 대상 제외 처리 여부 또한 "김명환 소장님"과 소통 후 구현 예정 (2024.09.04 minjae)

                    loadingWorkVM.ShowWindow();   // 파일열기 처리 중 화면 출력

                    Log.Information(Logger.GetMethodPath(currentMethod) + $"파일명 - {fileName} 열기 시작!");

                    // TODO : Revit 파일(확장자 - .rvt) 열기 실행시 아래 오류 메시지 출력시 레지스트리 편집기 설정 (기본값) 변경 필수 (2024.11.27 minjae)
                    //        레지스트리 편집기 실행 -> 편집기 경로 "컴퓨터\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Revit.Project\shell\open\command" 폴더 이동
                    //        -> (기본값)에 할당할 값 데이터(V): 항목에 할당된 문자열 변경 
                    //        (기존) "C:\Program Files\Autodesk\Revit 2024\Revit.exe" /dde
                    //        (변경) "C:\Program Files\Autodesk\Revit 2025\Revit.exe" /dde
                    // 오류 메시지 
                    // (예) "~~~파일이 Revit의 최신 릴리즈를 사용하여 저장되었습니다. 이 릴리즈에서는 열 수 없습니다. Revit의 현재 세션을 닫으십시오.
                    //       Revit의 현재 세션을 닫으십시오. 필요한 경우 새 릴리즈를 설치하십시오. 새 릴리즈를 사용하여 파일을 여십시오.
                    //       파일의 Revit 릴리즈 : Autodesk Revit 2025
                    //       현재 Revit 릴리즈 : Autodesk Revit 2024" 

                    // TODO : 메서드 "Process.Start()" 사용하여 파일열기 처리할 파일 전체 경로 정보를 읽어서 해당 파일 확장자에 맞는 응용 프로그램 실행하여 문서(또는 도면) 열기 기능 구현 (2024.11.04 minjae)
                    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.diagnostics.process.start?view=net-8.0
                    // 참고 2 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-%ED%8F%B4%EB%8D%94-%EC%97%B4%EA%B8%B0-%E2%80%93-Process-Start
                    // 참고 3 URL - https://kojin777.tistory.com/99
                    // 참고 4 URL - https://insurang.tistory.com/entry/WPF-C-ProcessStart-%ED%8F%B4%EB%8D%94-%EC%97%B4%EA%B8%B0-%ED%8C%8C%EC%9D%BC-%EC%97%B4%EA%B8%B0-FileOpen-FolderOpen
                    // 참고 5 URL - https://blog.naver.com/wlvkddlwkd/221754405111
                    // 참고 6 URL - https://yongtech.tistory.com/56
                    // 참고 7 URL - https://thebook.io/006890/0419/
                    Process.Start(filePath);

                    Log.Information(Logger.GetMethodPath(currentMethod) + $"파일명 - {fileName} 열기 완료!");

                    // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.11.04 minjae)
                    // Process.Start(path + "text.txt");
                    // Process.Start("Revit.exe", path + "text.txt");

                    resMsg = "success";  // 파일열기 성공  
                    //MessageBox.Show("파일열기 완료!");
                }
                else throw new Exception($"파일명 - {fileName}\r\n\r\n" +
                                         $"파일 경로\r\n{filePath}\r\n\r\n" +
                                         $"해당 파일은 존재하지 않습니다.\r\n" +
                                         $"다시 확인 바랍니다.");
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

                resMsg = ex.Message;   // 파일열기 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
            }
            finally
            {
                // 파일열기 처리 중이 아닌 경우 
                if(resMsg != CobimHelper.OpenFile_LoadingWork)
                {
                    // TODO : 파일열기 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
                    // 참고 URL - https://cosmosproject.tistory.com/563
                    IsOpenFileWorking = false;          // 파일열기 처리 중 종료
                    loadingWorkVM.RequestClose(null);   // 파일열기 처리 중 화면 종료
                }
            }
            return;
        }

        #endregion OpenFileAsync


        #region StartCancelWorkAsync

        // TODO : 아래 주석친 비동기 메서드 "StartCancelWorkAsync" 필요시 참고 (2024.09.24 minjae)
        /// <summary>
        /// 컨테이너 작업 취소 시작 
        /// </summary>
        /// <returns></returns>
        //private async Task StartCancelWorkAsync(string pDirPath, string pCancelWorkContainerId, bool pExist)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 1 단계 : Rest API 17. 컨테이너 작업 취소 호출 
        //        var resultData = await ExplorerRestServer.PostCancelWorkAsync(Client, ContainerPack, LoginView.RequestHeaders);

        //        // TODO : 컨테이너 작업 취소 결과 확인 로직 오류 발생시 수정 예정 (2023.12.21 minjae)
        //        // 2 단계 : 컨테이너 작업 취소 결과 확인
        //        // 조건 1 - resultData 값이 null 이 아님
        //        // 조건 2 - resultData[ContainerHelper.workUserId] 값이 null임.
        //        // 조건 3 - resultData[ContainerHelper.readYn] 문자열 변환시 'N'과 같아야 함.
        //        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.12.20 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        bool bCancelWork = (resultData is not null)
        //                           && (resultData[ContainerHelper.workUserId] is null)
        //                           && (resultData[ContainerHelper.readYn].ToString().Equals(ContainerHelper.no));

        //        // 컨테이너 작업 취소가 성공한 경우 
        //        if(true == bCancelWork)
        //        {
        //            // 3 단계 : 컨테이너 작업 취소 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
        //            // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "작업취소" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
        //            //        컨테이너 작업 취소 비동기 메서드 "CancelWorkAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //            //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //            //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 파라미터를 프로퍼티 "SelectedFolderId"로 전달함. (2023.12.20 minjae)
        //            if (false == string.IsNullOrWhiteSpace(SelectedFolderId))
        //                await ContainerListCreateAsync(ContainerPack, SelectedFolderId);
        //            else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");

        //            //MessageBox.Show("컨테이너 작업 취소 완료!");

        //            // 삭제 처리 할 파일 + 상위 디렉토리(폴더)가 존재하는 경우 
        //            if(true == pExist)
        //            {
        //                //MessageBox.Show($"로컬에 존재하는 루트 컨테이너 폴더({pCancelWorkContainerId})\r\n다운로드 받은 파일 삭제 진행합니다.");

        //                // 4 단계 : 컨테이너 작업 취소 완료 후 로컬 PC에 다운로드 받은 기존 루트 컨테이너 폴더(cancelWorkContainerId) 및 폴더에 속한 모든 파일 삭제 
        //                // TODO : 폴더(디렉토리) 클래스 "Directory", "DirectoryInfo" 차이점 비교 후 static 클래스 "Directory" 사용 (2023.12.20 minjae)
        //                // 참고 URL - https://nomad-programmer.tistory.com/207
        //                // TODO : Directory.Delete 메서드의 2번째 파라미터 값 설정 구현 (2023.12.20 minjae)
        //                //        dirPath 경로의 폴더(디렉터리), 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거하려면 true, 제거하지 않으려면 false 설정
        //                //        true로 설정하면 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거할 때, 재귀적으로 (recursive) Directory.Delete 메서드 실행 
        //                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.directory.delete?view=net-8.0
        //                Directory.Delete(pDirPath, true);

        //                // 루트 컨테이너 폴더(cancelWorkContainerId) + 다운로드 받은 파일 삭제 완료한 경우 
        //                if(false == Directory.Exists(pDirPath))
        //                    MessageBox.Show($"컨테이너 작업 취소 완료!\r\n루트 컨테이너 폴더 ({pCancelWorkContainerId})\r\n다운로드 받은 파일 삭제 완료");
        //                else
        //                    throw new Exception($"[오류] 루트 컨테이너 폴더 ({pCancelWorkContainerId})\r\n삭제 오류\r\n담당자에게 문의하세요.");
        //            } 
        //        }
        //        // 컨테이너 작업 취소 실패한 경우 
        //        else
        //            throw new Exception($"[오류] 컨테이너 작업 취소 실패\r\n담당자에게 문의 하세요.");

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2024.01.26 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //    return;
        //}

        #endregion StartCancelWorkAsync

        #region 이벤트 메소드 

        #region LockContainer

        // TODO : 컨테이너 잠금(락 - Lock / 비활성화)(ReadYn = "Y") 처리 이벤트 메서드 "OnLockedContainer" 필요시 사용 예정 (2023.12.14 minjae)
        // TODO : 오류 메시지 "Stylet.Xaml.ActionNotFoundException: 'Unable to find method OnLockedContainer on target ExplorerVM'" 원인 파악 결과 
        //        탐색기 뷰 (ExplorerV.xaml) 우측 상단 컨테이너 리스트 (ListView)에 구현한 속성 "SelectionChanged"에
        //        메서드 "{s:Action OnLockedContainer}"가 바인딩 되어 있고 해당 이벤트 메서드가 바인딩 되어 정상적으로 실행되려면 
        //        탐색기 뷰모델 (ExplorerVM.cs)에 구현된
        //        이벤트 메서드 "OnLockedContainer" 접근 제한자 (기존) private -> (변경) public 처리 해야 정상적으로 실행된다.
        /// <summary>
        /// 컨테이너 잠금(락 - Lock / 비활성화)(ReadYn = "Y") 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public void OnLockedContainer(object sender, SelectionChangedEventArgs e)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        MessageBox.Show("[테스트] OnLockedContainer");

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //}

        #endregion LockContainer

        #endregion 이벤트 메소드 

        #region Sample

        // TODO : 아래 주석친 이벤트 메서드 "OnInitialActivate" 필요시 참고 (2024.10.31 minjae)
        // protected override void OnInitialActivate()
        // {
        //     base.OnInitialActivate();
        //     explorerVM = this;
        //     InitSetting();
        //     // DisplayType = CobimHelper.TypeOfOnInitialActivate;
        //     // IsVisible = false;          // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 숨기도록 비활성화
        //     // DisplaySetting(DisplayType);
        // 
        //     // await ExplorerListCreateAsync();
        // }

        // TODO : 아래 주석친 이벤트 메서드 "OnActivate" 필요시 참고 (2024.10.31 minjae)
        // TODO : 이벤트 메서드 "OnActivate" 비동기(async)로 구현 (2023.09.18 minjae)
        // 참고 URL - https://vmpo.tistory.com/89
        //protected async override void OnActivate()
        //{
        //    base.OnActivate();

        //    DisplayType = CobimHelper.TypeOfOnActivate;
        //    // IsVisible = false;         // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 숨기도록 비활성화
        //    DisplaySetting(DisplayType);
        //    await ExplorerListCreateAsync();

        //    // TODO : 아래 if 조건절(리본 버튼 클릭 여부 조건) 만족시 화면 셋팅 메서드 실행 로직 필요시 사용 예정 (2024.09.09 JB)
        //    //if(true == MainView.CommonData.BtnClickType.Equals(CobimHelper.RibbonBtnType))
        //    //{
        //    //    DisplayType = CobimHelper.TypeOfOnActivate;
        //    //    // IsVisible = false;         // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 숨기도록 비활성화
        //    //    DisplaySetting(DisplayType);
        //    //    await ExplorerListCreateAsync();
        //    //}

        //    // TODO : 초기 셋팅 메서드 "InitSetting" 필요시 구현하기 (2023.12.14 minjae)
        //    // InitSetting();                 // 화면 초기 셋팅  

        //    // TODO : 아래 테스트 코드 필요시 참고 (2024.08.07 minjae)
        //    // Sender = this;

        //    // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 minjae)
        //    // InitTreeViewText();

        //    // await ExplorerListCreateAsync();

        //    //var vm = Container.Get<ExplorerVM>();
        //    //string VMName = vm.GetViewModelName();

        //    // TODO : 필요시 var msg = new ChangeViewModelMsg(this, VMName); 사용 예정 (2024.08.06 minjae)
        //    // var msg = new ChangeViewModelMsg(this, VMName);
        //    //var msg = new OpenPageMsg((ICbVM)Sender, VMName);
        //    //EventAggregator.PublishOnUIThread(msg);
        //}

        // TODO : 아래 주석친 메서드 "DisplaySetting" 필요시 참고 (2024.10.31 minjae)
        /// <summary>
        /// 화면셋팅 - 화면에 필요한 컨트롤러(DockPanel, Button 등등...) 출력, 리스트 및 Collection 초기화(Clear)
        /// </summary>
        //private void DisplaySetting(string pDisplayType)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        switch (pDisplayType)
        //        {
        //            // case CobimHelper.TypeOfOnInitialActivate:
        //            case CobimHelper.TypeOfOnActivate:
        //                // InitSetting();              // 화면 초기 셋팅  

        //                // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
        //                //        화면 종료 또는 다른 화면 이동 후 해당 탐색기 화면 재실행 시
        //                //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
        //                //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
        //                // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
        //                // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
        //                // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
        //                SelectedTabIndex = 0;

        //                // 메서드 "ExplorerListCreateAsync" 사용되는 리스트, Collection
        //                ProjectList.Clear();                                      // 프로젝트 리스트 초기화
        //                TeamList.Clear();                                         // 팀 리스트 초기화
        //                FolderList.Clear();                                       // 폴더 리스트 초기화 
        //                // LevelDatas.Clear();

        //                // 메서드 "ContainerSearchAsync" 사용되는 리스트
        //                ContainerList.Clear();                                    // 컨테이너 리스트 초기화
        //                ContainerDatas.Clear();                                   // 컨테이너 메타 데이터 Collection 초기화

        //                // 메서드 "DetailSearchAsync" 사용되는 리스트
        //                DetailList.Clear();                                       // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
        //                // DetailDatas.Clear();

        //                // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
        //                LinkedNodeInfoList.Clear();                               // 연관 컨테이너 정보 리스트 초기화
        //                LinkedNodeInfoDatas.Clear();                              // 연관 컨테이너 정보 Collection 초기화

        //                // TODO : 컨테이너 ID (영어 대문자) 프로퍼티 exp_UpperContainerId 필요시 사용 예정 (2023.10.23 minjae) 
        //                // exp_UpperContainerId  = string.Empty;
        //                // visibleFirstTabItem   = Visibility.Visible;            // 화면 활성화될 때 마다 탭 "기본정보" Default 출력
        //                visibleRequest = Visibility.Collapsed;          // 화면 활성화될 때 마다 버튼 "검토요청" 숨김 처리 
        //                visibleDetailInfo = Visibility.Collapsed;          // 화면 활성화될 때 마다 DockPanel 영역 "컨테이너 정보" 숨김 처리
        //                visibleFileInfo = Visibility.Collapsed;          // 화면 활성화될 때 마다 DockPanel 영역 "파일정보" 숨김 처리
        //                // visibleLinkedNodeInfo = Visibility.Collapsed;          // 화면 활성화될 때 마다 DockPanel 영역 "연관 컨테이너 정보 TreeView" 숨김 처리
        //                break;
        //            case CobimHelper.TypeOfExplorerListCreateAsync:
        //                TeamList.Clear();                                         // 팀 리스트 초기화
        //                FolderList.Clear();                                       // 폴더 리스트 초기화
        //                visibleRequest = Visibility.Collapsed;                 // 버튼 "검토요청" 숨김 처리
        //                visibleDetailInfo = Visibility.Collapsed;                 // DockPanel 영역 "컨테이너 정보" 숨김 처리
        //                visibleFileInfo = Visibility.Collapsed;                   // DockPanel 영역 "파일정보" 숨김 처리
        //                break;
        //            case CobimHelper.TypeOfDetailSearch:
        //                //case CobimHelper.TypeOfLinkedNodeInfoCreate:
        //                // InitSetting();
        //                // 메서드 "ContainerSearchAsync" 사용되는 리스트
        //                // ContainerList.Clear();                                 // 컨테이너 리스트 초기화

        //                // 메서드 "ContainerDeatilSearchAsync" 사용되는 리스트
        //                // ContainerList.Clear();                                 // 컨테이너(파일 정보 + 메타데이터) 리스트 초기화

        //                // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
        //                //        컨테이너 ID 클릭 후 메서드 "ContainerDeatilSearchAsync" 실행시 
        //                //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
        //                //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
        //                // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
        //                // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
        //                // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
        //                SelectedTabIndex = 0;

        //                DetailList.Clear();                                       // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
        //                DetailDatas.Clear();                                      // 파일 컨테이너 정보 + 메타데이터 Collection 초기화

        //                // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
        //                LinkedNodeInfoList.Clear();                               // 연관 컨테이너 정보 리스트 초기화
        //                LinkedNodeInfoDatas.Clear();                              // 연관 컨테이너 정보 Collection 초기화

        //                visibleRequest = Visibility.Visible;            // 검토요청 활성화 처리
        //                visibleDetailInfo = Visibility.Visible;            // 컨테이너 정보 활성화 처리
        //                visibleFileInfo = Visibility.Visible;            // 파일정보 활성화 처리
        //                // visibleLinkedNodeInfo = Visibility.Visible;            // 연관 컨테이너 정보 TreeView 활성화 처리
        //                break;
        //            // case CobimHelper.TypeOfContainerSearch:
        //            // case CobimHelper.TypeOfCheckOut:
        //            // case CobimHelper.TypeOfCheckIn:
        //            case CobimHelper.TypeOfContainerListCreateAsync:
        //                SelectedTabIndex = 0;                                     // 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정

        //                DetailList.Clear();                                       // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
        //                DetailDatas.Clear();                                      // 파일 컨테이너 정보 + 메타데이터 Collection 초기화

        //                // 메서드 "ContainerSearchAsync" 사용되는 리스트
        //                ContainerList.Clear();
        //                ContainerDatas.Clear();
        //                visibleRequest = Visibility.Collapsed;                     // 버튼 "검토요청" 숨김 처리
        //                visibleDetailInfo = Visibility.Collapsed;                  // DockPanel 영역 "컨테이너 정보" 숨김 처리
        //                visibleFileInfo = Visibility.Collapsed;                    // DockPanel 영역 "파일정보" 숨김 처리
        //                break;
        //            // TODO : 아래 case 문 "case CobimHelper.TypeOfLinkedNodeInfoCreate:" 필요시 사용 예정 (2023.12.26 minjae) 
        //            //case CobimHelper.TypeOfLinkedNodeInfoCreate:
        //            //    // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
        //            //    LinkedNodeInfoList.Clear();                               // 연관 컨테이너 정보 리스트 초기화
        //            //    LinkedNodeInfoDatas.Clear();                              // 연관 컨테이너 정보 Collection 초기화
        //            //    break;
        //            default:
        //                break;
        //        }

        //        // TODO : 아래 테스트 코드 필요시 참고 (2023.11.01 minjae)
        //        //if (!pIsVisibie)
        //        //{
        //        //    // InitSetting();              // 화면 초기 셋팅  

        //        //    // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
        //        //    //        화면 종료 또는 다른 화면 이동 후 해당 탐색기 화면 재실행 시
        //        //    //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
        //        //    //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
        //        //    // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
        //        //    // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
        //        //    // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
        //        //    SelectedTabIndex = 0;

        //        //    // 메서드 "ExplorerListCreateAsync" 사용되는 리스트, Collection
        //        //    ProjectList.Clear();       // 프로젝트 리스트 초기화
        //        //    TeamList.Clear();          // 팀 리스트 초기화
        //        //    FolderList.Clear();        // 폴더 리스트 초기화 
        //        //                               // LevelDatas.Clear();



        //        //    // 메서드 "ContainerSearchAsync" 사용되는 리스트
        //        //    ContainerList.Clear();          // 컨테이너 리스트 초기화
        //        //    ContainerDatas.Clear();         // 컨테이너(파일 + 메타 데이터 포함) Collection 초기화

        //        //    // 메서드 "DetailSearchAsync" 사용되는 리스트
        //        //    ContainerList.Clear();     // 파일 컨테이너 정보 + 메타데이터 리스트 초기화
        //        //                               // ContainerDatas.Clear();

        //        //    // 메서드 "LinkedNodeInfoCreate" 사용되는 리스트, Collection 초기화
        //        //    LinkedNodeInfoList.Clear();          // 연관 컨테이너 정보 리스트 초기화
        //        //    LinkedNodeInfoDatas.Clear();         // 연관 컨테이너 정보 Collection 초기화

        //        //    // TODO : 컨테이너 ID (영어 대문자) 프로퍼티 exp_UpperContainerId 필요시 사용 예정 (2023.10.23 minjae) 
        //        //    // exp_UpperContainerId = string.Empty;
        //        //    // visibleFirstTabItem = Visibility.Visible;              // 화면 활성화될 때 마다 탭 "기본정보" Default 출력
        //        //    visibleRequest        = Visibility.Collapsed;            // 화면 활성화될 때 마다 버튼 "검토요청" 숨김 처리 
        //        //    visibleDetailInfo  = Visibility.Collapsed;            // 화면 활성화될 때 마다 DockPanel 영역 "컨테이너 정보" 숨김 처리
        //        //    visibleFileInfo       = Visibility.Collapsed;            // 화면 활성화될 때 마다 DockPanel 영역 "파일정보" 숨김 처리
        //        //    // visibleLinkedNodeInfo = Visibility.Collapsed;         // 화면 활성화될 때 마다 DockPanel 영역 "연관 컨테이너 정보 TreeView" 숨김 처리
        //        //}
        //        //// 컨테이너 정보 + 파일정보 조회 메서드 DetailSearchAsync 실행 
        //        //else
        //        //{
        //        //    // InitSetting();
        //        //    // 메서드 "ContainerSearchAsync" 사용되는 리스트
        //        //    ContainerList.Clear();          // 컨테이너 리스트 초기화

        //        //    // 메서드 "DetailSearchAsync" 사용되는 리스트
        //        //    // ContainerDetailList.Clear();     // 컨테이너(파일 정보 + 메타데이터) 리스트 초기화


        //        //    // TODO : 탐색기 화면(ExplorerV.xaml) 탭 "기본정보" 제외한 다른 TabItem 선택 후
        //        //    //        컨테이너 ID 클릭 후 메서드 "DetailSearchAsync" 실행시 
        //        //    //        TabControl의 첫 번째 TabItem="기본정보"로 실행(Default로 지정)하도록
        //        //    //        SelectedIndex = "0" 추가(2023.11.01 minjae) 
        //        //    // 참고 URL - https://stackoverflow.com/questions/20356239/tab-control-selecting-first-tab-by-default 
        //        //    // 참고 2 URL - https://dotnetpattern.com/wpf-tab-control-selection  
        //        //    // 화면 재실행시 첫 번째 탭 "기본정보" Default 출력 하도록 인덱스 값 0으로 설정
        //        //    SelectedTabIndex = 0;

        //        //    visibleRequest        = Visibility.Visible;              // 검토요청 활성화 처리
        //        //    visibleDetailInfo  = Visibility.Visible;                 // 컨테이너 정보 활성화 처리
        //        //    visibleFileInfo       = Visibility.Visible;              // 파일정보 활성화 처리
        //        //    // visibleLinkedNodeInfo = Visibility.Visible;           // 연관 컨테이너 정보 TreeView 활성화 처리
        //        //}
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 아래 주석친 비동기 메서드 "ExplorerListCreateAsync" 필요시 참고 (2024.10.30 minjae)
        /// <summary>
        /// 프로젝트 - 팀 - 폴더 - 서브폴더 리스트 생성 및 초기화 / 파일 메타 데이터 Collection 초기화 
        /// 메서드 파라미터 List<ITreeNode> pSubFolderFileList 추가 여부 확인 
        /// </summary>
        //private async Task ExplorerListCreateAsync()
        //{
        //    // string tokenKey      = AppSetting.Default.Login.Token.resultData;  // 로그인 토큰 키 

        //    // string testProjectId = AppSetting.Default.Project.TestProjectId;   // 테스트 프로젝트 아이디
        //    // string testTeamId    = AppSetting.Default.Project.TestTeamId;      // 테스트 팀 아이디

        //    string projectId = string.Empty;                               // JSON 프로젝트 아이디
        //    string projectName = string.Empty;                               // JSON 프로젝트 이름 

        //    string teamProjectId = string.Empty;                               // JSON 팀의 상위 프로젝트 아이디
        //    string teamId = string.Empty;                               // JSON 팀 아이디
        //    string teamName = string.Empty;                               // JSON 팀 이름

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및
        //        //        테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 추후 필요시 수정 예정 (2023.09.04 minjae)     
        //        //ProjectPack = new ProjectPack
        //        //{
        //        //    projectId = testProjectId,
        //        //    teamId    = testTeamId
        //        //};

        //        InputParamData.SetRequestHeader(LoginView.GetRequestHeader());

        //        // 프로젝트 + 팀 리스트 데이터 받아오기 
        //        //var datas = await ExplorerRestServer.GetProjectTeamListAsync(Client, LoginView.RequestHeaders);
        //        var datas = await ExplorerRestServer.GetProjectTeamListAsync(InputParamData);

        //        // 폴더 리스트 데이터 받아오기
        //        // var folderList = await ExplorerRestServer.GetFolderListAsync(client, ProjectPack, LoginView.RequestHeaders);

        //        foreach (var data in datas)
        //        {
        //            DisplaySetting(CobimHelper.TypeOfExplorerListCreateAsync);

        //            var projectDic = data[ProjectHelper.projectEntity].ToDictionary();        // 프로젝트 메타 데이터 Dictionary
        //            // Dictionary 객체 projectDic 특정 키(Key) "projectId"에 속하는 값(Value) 문자열로 변환 (2023.09.11 minjae)
        //            // 참고 URL - https://developer-talk.tistory.com/694
        //            projectId = projectDic[ProjectHelper.projectId].ToString();          // 해당 프로젝트 아이디 
        //            projectName = projectDic[ProjectHelper.projectName].ToString();        // 해당 프로젝트 이름 

        //            var teamDic = data[TeamHelper.teamEntity].ToDictionary();              // 팀 메타 데이터 Dictionary
        //            teamProjectId = teamDic[ProjectHelper.projectId].ToString();             // 해당 팀이 소속된 프로젝트 이름 
        //            teamId = teamDic[TeamHelper.teamId].ToString();                   // 해당 팀 아이디 
        //            teamName = teamDic[TeamHelper.teamName].ToString();                 // 해당 팀 이름 

        //            //ProjectPack = new ProjectPack
        //            //{
        //            //    projectId = projectId,
        //            //    teamId = teamId
        //            //};
        //            ProjectPack = new ProjectPack(projectId, teamId);

        //            var folderList = await ExplorerRestServer.GetFolderListAsync(Client, ProjectPack, LoginView.RequestHeaders);

        //            if ((int)EnumFolder.NONE == folderList.Count) continue;   // 폴더 목록 리스트 객체 folderList 에 데이터가 없는 경우((int)EnumFolder.NONE) 폴더가 없으므로 continue 처리 

        //            // TODO : Linq Where 확장메서드에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //            // 참고 URL - https://husk321.tistory.com/405
        //            // 프로젝트 - 팀 하위에 속하는 루트 폴더 리스트 데이터 추출
        //            var rootFolderList = folderList.Where(folder => folder[FolderHelper.parentFolderId] is null).Select(folder => new FolderView
        //            {
        //                FolderDataInfo = folder,
        //                // TODO : 개체 이니셜라이저 사용해서 폴더 목록 리스트 객체 FolderItems 생성시 객체 값(folder) 할당 (2023.11.23 minjae)
        //                // 참고 URL - https://hyokye0ng.tistory.com/86
        //                FolderItems = new List<Dictionary<string, object>> { folder },
        //                // TODO : == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //                // 참고 URL - https://husk321.tistory.com/405
        //                // TODO : NULL 예외처리 연산자 ?. 사용시 오류 발생으로 인하여 NULL 예외처리 삼항 연산자 사용하는 로직으로 수정 (2023.09.11 minjae)
        //                // 참고 URL - https://jellyho.com/blog/54/
        //                exp_ParentFolderId = (folder[FolderHelper.parentFolderId] is null) ? (string.Empty) : (folder[FolderHelper.parentFolderId].ToString()),
        //                exp_FolderGroupId = folder[FolderHelper.folderGroupId].ToString(),
        //                exp_FolderLevelNo = folder[FolderHelper.levelNo].ToString(),
        //                exp_ProjectId = projectId,
        //                exp_TeamId = teamId,
        //                exp_FolderId = folder[FolderHelper.folderId].ToString(),
        //                exp_TreeName = folder[FolderHelper.folderName].ToString(),
        //                // TODO : 파일 리스트 추가 작업시 ChildNodes 사용 예정 (2023.09.18 minjae)
        //                // ChildNodes = subNodes
        //            });

        //            // 루트 폴더 하위 서브 폴더 추가
        //            foreach (var rootFolder in rootFolderList)
        //            {
        //                // (루트 + 서브) 폴더 리스트에 루트 폴더 추가 
        //                //FolderList.Add(rootFolder);

        //                // 서브 폴더 추가
        //                await FillFolders(rootFolder, folderList);

        //                FolderList.Add(rootFolder);
        //            }

        //            // 프로젝트 하위 디렉토리 (팀) 존재하지 않을 경우 새로 생성
        //            // TODO : bool 변수 "hasTeamInfo" 구현 (2023.09.18 minjae)
        //            // 참고 URL - https://frogand.tistory.com/211
        //            bool hasTeamInfo = false == string.IsNullOrWhiteSpace(teamProjectId)
        //                               && false == string.IsNullOrWhiteSpace(teamId)
        //                               && false == string.IsNullOrWhiteSpace(teamName);

        //            if (true == hasTeamInfo
        //               && true == projectId.Equals(teamProjectId)
        //               && false == string.IsNullOrWhiteSpace(projectId))
        //            {
        //                var teamInfo = new TeamView()
        //                {
        //                    TeamDataInfo = teamDic,
        //                    // TODO : 개체 이니셜라이저 사용해서 팀 목록 리스트 객체 TeamItems 생성시 객체 값(teamDic) 할당 (2023.11.23 minjae)
        //                    // 참고 URL - https://hyokye0ng.tistory.com/86
        //                    // TODO : TeamItems값이 NULL인 원인 파악 후 해결하기(2023.09.11 minjae) 
        //                    // TeamItems = teamArray.Where(key => key["teamId"].Equals(teamIdValue)).Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList(),
        //                    TeamItems = new List<Dictionary<string, object>> { teamDic },
        //                    // TODO : 팀에 소속된 사용자 리스트 JSON 데이터 인재 INC 로부터 REST API 공유 받으면 구현 진행 예정 (2023.09.12 minjae)
        //                    // TeamUserList = new List<Dictionary<string, object>> { teamUserDic },
        //                    exp_TeamId = teamId,
        //                    // TODO : 프로퍼티 "exp_TeamName" 필요시 사용 예정(2023.10.23 minjae)
        //                    // exp_TeamName = teamName,   // 화면 우측 상단 팀 이름 출력할 때 사용할 바인딩 객체 "exp_TeamName"
        //                    exp_TreeName = teamName,
        //                    // TestChildNodes = pFolderList

        //                    // TestChildNodes = TestFolderNodes
        //                    // ChildNodes   = FolderList[0]
        //                };

        //                teamInfo.ChildNodes.Add(FolderList[CobimHelper.RootFolderIndex]);

        //                TeamList.Add(teamInfo);
        //            }

        //            // 루트 디렉토리 (프로젝트) 존재하지 않을 경우 새로 생성
        //            // TODO : bool 변수 "hasProjectInfo" 구현 (2023.09.18 minjae)
        //            // 참고 URL - https://frogand.tistory.com/211
        //            bool hasProjectInfo = false == string.IsNullOrWhiteSpace(projectId)
        //                                  && true == projectId.Equals(teamProjectId)
        //                                  && false == string.IsNullOrWhiteSpace(projectName);

        //            if (true == hasProjectInfo)
        //            {
        //                var projectInfo = new ProjectView()
        //                {
        //                    ProjectDataInfo = projectDic,
        //                    // TODO : 개체 이니셜라이저 사용해서 프로젝트 목록 리스트 객체 ProjectItems 생성시 객체 값(projectDic) 할당 (2023.11.23 minjae)
        //                    // 참고 URL - https://hyokye0ng.tistory.com/86
        //                    // TODO : ProjectItems값이 NULL인 원인 파악 후 해결하기(2023.09.11 minjae)
        //                    ProjectItems = new List<Dictionary<string, object>> { projectDic },
        //                    exp_ProjectId = projectId,

        //                    exp_TreeName = projectName,
        //                    ChildNodes = TeamList.Where(team => team.exp_TreeName.Equals(teamName)).ToList()
        //                };

        //                ProjectList.Add(projectInfo);
        //            }
        //        }

        //        LevelDatas.Clear();                           // Explorer Collection 초기화
        //        LevelDatas.AddRange(ProjectList.ToArray());   // Explorer Collection에 전체 데이터(프로젝트 + 팀 + 폴더 + 서브폴더 + 파일) 추가 
        //        return;
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 비동기 메서드 "ExplorerListCreateAsync" 실행시 발견한 오류 메시지 "'<' is an invalid start of a value. LineNumber: 0 | BytePositionInLine: 0." 오류 원인 파악하기 (2023.11.27 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/61487289/exception-is-an-invalid-start-of-a-value
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}


        // TODO : 필요시 아래 주석친 컨테이너(파일 포함) 목록 조회 비동기 메서드 "ContainerSearchAsync" 참고 (2024.10.30 minjae)
        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너(파일 포함) 목록 조회
        /// </summary>
        //private async Task ContainerSearchAsync(object obj)
        //{
        //    // TODO : 아래 주석친 소스 코드 3줄(parameters, containerDatas, containerDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //    // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
        //    // 참고 URL - https://nonstop-antoine.tistory.com/36
        //    // var parameters        = obj as IEnumerable;                 // 폴더 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 
        //    // var containerDatas    = new List<string>();                 // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDatas"
        //    // var containerDataDic  = new Dictionary<string, string>();   // 컨테이너 데이터를 담는 Dictionary 객체 containerDataDic 생성

        //    // TODO : 폴더 버튼 클릭 시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                                // 폴더 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] containerDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
        //    // var containerDatas = parameters.ToList();                   // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "containerDatas"

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃", "체크인" 클릭시
        //        //        탐색기 뷰모델(ExplorerVM.cs)   -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //        //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //        //        처음 사용자가 선택(클릭)한 특정 폴더 ID 프로퍼티 "SelectedFolderId" 에 폴더 ID 문자열을 할당해서 
        //        //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync"에 메서드 파라미터를
        //        //        프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae) 
        //        // 선택(클릭)한 특정 폴더 ID    
        //        // SelectedFolderId = containerDatas[(int)EnumContainer.FOLDERID].ToString();
        //        SelectedFolderId = containerDatas[(int)EnumContainer.FOLDERID];

        //        ContainerPack = new ContainerPack(containerDatas[(int)EnumContainer.PROJECTID],
        //                                          containerDatas[(int)EnumContainer.TEAMID],
        //                                          string.Empty,
        //                                          SelectedFolderId,
        //                                          string.Empty);

        //        await ContainerListCreateAsync(ContainerPack, SelectedFolderId);

        //        // MessageBox.Show("컨테이너 리스트 출력 테스트 중입니다.");
        //        //return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        Log.Error(ex.ToString());
        //        // e.ToString()
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}
        /// <summary>
        /// 컨테이너(파일 포함) 목록 조회
        /// </summary>
        //private async Task ContainerSearchAsync(object obj)
        //{
        //    // string tokenKey = AppSetting.Default.Login.Token.resultData;       // 로그인 토큰 키 

        //    // TODO : 아래 주석친 소스 코드 3줄(parameters, containerDatas, containerDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //    // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
        //    // 참고 URL - https://nonstop-antoine.tistory.com/36
        //    // var parameters        = obj as IEnumerable;                             // 폴더 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 
        //    // var containerDatas    = new List<string>();                             // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDatas"
        //    // var containerDataDic  = new Dictionary<string, string>();               // 컨테이너 데이터를 담는 Dictionary 객체 containerDataDic 생성

        //    // TODO : 폴더 버튼 클릭 시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                                  // 폴더 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_TreeFolderId") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] containerDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
        //    // var containerDatas       = parameters.ToList();                            // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "containerDatas"

        //    // string folderId       = string.Empty;                                   // 선택(클릭)한 특정 폴더 아이디 
        //    // string createUserId   = string.Empty;                                   // 작성자 
        //    // string createDateTime = string.Empty;                                   // 등록일시

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 화면 셋팅 "ContainerSearchAsync" 필요시 사용 예정 (2023.12.19 minjae) 
        //        // DisplayType = CobimHelper.TypeOfContainerSearch;
        //        // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
        //        // DisplaySetting(DisplayType);

        //        // TODO : 아래 주석친 foreach 문 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //        // containerDatas 안에 존재하는 파일 정보 반복문으로 방문 -> List 객체 containerInfos에 컨테이너 정보 데이터 추가 
        //        // foreach (var data in containerDatas)
        //        // {
        //        //     containerInfos.Add(data.ToString());
        //        // }

        //        // TODO : 아래 주석친 소스코드 3줄 (containerDataDic.Add) 필요 없을시 삭제 예정 (2023.12.05 minjae)
        //        //containerDataDic.Add(ProjectHelper.projectId, containerInfos[(int)EnumContainer.PROJECTID]);
        //        //containerDataDic.Add(TeamHelper.teamId,       containerInfos[(int)EnumContainer.TEAMID]);
        //        //containerDataDic.Add(FolderHelper.folderId,   containerInfos[(int)EnumContainer.FOLDERID]);

        //        // folderId = containerDataDic[FolderHelper.folderId];   // 폴더 아이디 
        //        // folderId = containerDatas[(int)EnumContainer.FOLDERID].ToString();   // 폴더 아이디 

        //        // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃", "체크인" 클릭시
        //        //        탐색기 뷰모델(ExplorerVM.cs)   -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //        //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //        //        처음 사용자가 선택(클릭)한 특정 폴더 ID 프로퍼티 "SelectedFolderId" 에 폴더 ID 문자열을 할당해서 
        //        //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync"에 메서드 파라미터를
        //        //        프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae) 
        //        // 선택(클릭)한 특정 폴더 ID
        //        // SelectedFolderId = containerDatas[(int)EnumContainer.FOLDERID].ToString();
        //        SelectedFolderId = containerDatas[(int)EnumContainer.FOLDERID];


        //        // TODO : 아래 주석친 ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 소스코드 필요 없을시 삭제 예정 (2023.12.05 minjae)
        //        // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및
        //        //        테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 추후 필요시 수정 예정 (2023.09.04 minjae)     
        //        // ProjectPack = new ProjectPack
        //        // {
        //        //     projectId = containerDataDic[ProjectHelper.projectId],
        //        //     teamId    = containerDataDic[TeamHelper.teamId]
        //        // };
        //        // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및
        //        //        테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 추후 필요시 수정 예정 (2023.12.05 minjae)     
        //        // ProjectPack = new ProjectPack
        //        // {
        //        //     projectId = containerDatas[(int)EnumContainer.PROJECTID],
        //        //     teamId    = containerDatas[(int)EnumContainer.TEAMID]
        //        // };

        //        //ContainerPack = new ContainerPack
        //        //{
        //        //    projectId   = containerDatas[(int)EnumContainer.PROJECTID],
        //        //    teamId      = containerDatas[(int)EnumContainer.TEAMID],
        //        //    containerId = string.Empty
        //        //};

        //        ContainerPack = new ContainerPack(containerDatas[(int)EnumContainer.PROJECTID], containerDatas[(int)EnumContainer.TEAMID], string.Empty);

        //        await ContainerListCreateAsync(ContainerPack, SelectedFolderId);

        //        // MessageBox.Show("컨테이너 리스트 출력 테스트 중입니다.");
        //        //return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        Log.Error(ex.ToString());
        //        // e.ToString()
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}

        // TODO : 필요시 아래 주석친 컨테이너 리스트 생성(재조회 - Refresh 기능 포함) 비동기 메서드 "ContainerListCreateAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 컨테이너 리스트 생성(재조회 - Refresh 기능 포함)
        /// </summary>
        /// <param name="pContainerPack">컨테이너 정보</param>
        /// <param name="pSelectedFolderId">사용자가 선택한 폴더 아이디</param>
        /// <returns></returns>
        /// 메소드 파라미터로 디폴트 매개변수 선언 방법 
        /// public DeptCommonBoardVM Set(string pTitle = null, string pTitleDesc = null)
        //private async Task ContainerListCreateAsync(ContainerPack pContainerPack, string pSelectedFolderId)
        //{
        //    List<Dictionary<string, object>> containerList = new List<Dictionary<string, object>>();       // 컨테이너 목록(리스트) 객체 초기화
        //    Dictionary<string, object> resultWebViewerData = new Dictionary<string, object>();             // HubBox Rest API 2. HubBox 웹뷰어 요청 및 정보 리스트 객체 초기화
        //    // string tokenKey       = AppSetting.Default.Login.Token.resultData;       // 로그인 토큰 키 

        //    // string folderId       = string.Empty;                                    // 폴더 아이디 
        //    string createUserId = string.Empty;                                       // 작성자 
        //    string createDateTime = string.Empty;                                       // 등록일시

        //    string readYn = string.Empty;                                       // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)

        //    string translateStatusCode = string.Empty;                                  // HubBox Rest API 2. 파일 뷰어 요청 HttpStatusCode 상태 코드 값

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 화면 셋팅 처리 
        //        DisplayType = CobimHelper.TypeOfContainerListCreateAsync;
        //        // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
        //        DisplaySetting(DisplayType);

        //        // TODO : 오류 메시지 "Id = 3455, Status = WaitingForActivation, Method = "{null}", Result = "{Not yet computed}"
        //        //        해결하기 위해 비동기 메서드 호출시 키워드 "await" 추가 (2023.12.21 minjae)
        //        // 참고 URL - https://www.codeproject.com/Questions/1156449/How-to-call-async-methods-Csharp
        //        containerList = await ExplorerRestServer.GetContainerListAsync(Client, pContainerPack, pSelectedFolderId, LoginView.RequestHeaders);


        //        // 컨테이너 리스트에 데이터가 존재하지 않는 경우 
        //        if (true == containerList.Count.Equals(CobimHelper.ContainerNotExist))
        //        {
        //            MessageBox.Show("성과품을 찾을 수 없습니다.");
        //            return;
        //        }

        //        foreach (var container in containerList)
        //        {
        //            // HubBox Rest API 2. 파일 뷰어 요청 데이터 객체 프로퍼티 "WebViewerPack" 생성하기 
        //            WebViewerPack = new WebViewerPack(container[ContainerHelper.projectId].ToString(),
        //                                              container[ContainerHelper.teamId].ToString(),
        //                                              container[ContainerHelper.containerId].ToString(),
        //                                              container[ContainerHelper.fileId].ToString());

        //            // HubBox Rest API 2. 파일 뷰어 요청 및 데이터 가져오기 
        //            resultWebViewerData = await HubBoxRestServer.GetWebViewerUrlAsync(Client, WebViewerPack, LoginView.RequestHeaders);

        //            var fileTypeCodeInfoDic = container[ContainerHelper.fileTypeCodeInfo].ToDictionary();
        //            var stepCodeInfoDic = container[ContainerHelper.stepCodeInfo].ToDictionary();
        //            var statusCodeInfoDic = container[ContainerHelper.statusCodeInfo].ToDictionary();


        //            createUserId = container[ContainerHelper.createUserId].ToString();       // 작성자 

        //            // TODO : 특정 문자 'T'를 포함한 등록일시 문자열 (예) "2023-07-25T10:44:45.988+00:00" 을 DateTime 객체 dt로 변환 (2023.09.21 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/14252312/how-to-convert-date-with-t-to-from-string-in-c-sharp
        //            var createDT = container[ContainerHelper.createDateTime].ToString();     // 등록일시

        //            // TODO : UTC 협정 세계시로 변환 필요시 DateTimeOffset 클래스 객체 dto 선언 및 초기화 
        //            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-NET%EC%9D%98-DataTime-%EB%AA%A8%EB%B2%94-%EC%82%AC%EB%A1%80
        //            //var dto = DateTimeOffset.Parse(createDT);

        //            // TODO : UTC 협정 세계시로 변환할 필요 없이 대한민국 시간(로컬 타임)으로 변환하기 위해 DateTime 클래스 객체 dt 선언 및 초기화(2023.09.26 minjae)
        //            // DataTime VS DateTimeOffset 비교 문서
        //            // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-%EC%97%90%EC%84%9C-IronPython-%EC%9D%B4%EC%9A%A9%ED%95%98%EC%97%AC-Python-%EC%97%B0%EB%8F%99
        //            // 문자열 ->  DateTime 객체 dt로 형변환
        //            // Get the date object from the string. 
        //            var dt = DateTime.Parse(createDT);

        //            // 문자열 ->  DateTime 객체 dt로 형변환
        //            // Get the date object from the string. 
        //            //var dt = dto.DateTime;

        //            // DateTimeOffset 객체 dto -> 문자열로 형변환
        //            // Convert the DateTimeOffSet to string. 
        //            // string newVal = dto.ToString("o");

        //            // DateTime 객체 dt -> 문자열로 형변환 (대문자 HH - 24시간제 기준 / 소문자 hh - 12시간제 기준(오전 12시, 자정 12시))
        //            // 참고 URL - https://wwwi.tistory.com/327
        //            createDateTime = dt.ToString("yyyy/MM/dd HH:mm");

        //            translateStatusCode = resultWebViewerData[HubBoxHelper.code].ToString();

        //            // var testName = fileTypeCodeInfoDic.Where(k => k.Key.Equals(FileHelper.fileTypeCodeName)).Select(v => v.Value).ToString();

        //            //if (folderId.Equals(container[FileHelper.folderId].ToString()))
        //            //{
        //            //    var containerInfo = new ContainerView()
        //            //    {
        //            //        FileDataInfo = container,
        //            //        FileItems = new List<Dictionary<string, object>> { container },
        //            //        exp_TreeName = container[FileHelper.fileNm].ToString(),
        //            //        exp_ContainerId = container[FileHelper.containerId].ToString(),
        //            //        exp_TypeCodeName = fileTypeCodeInfoDic[FileHelper.fileTypeCodeName].ToString(),
        //            //        exp_StepCodeName = stepCodeInfoDic[FileHelper.stepCodeName].ToString(),
        //            //        exp_StatusCodeName = statusCodeInfoDic[FileHelper.statusCodeName].ToString(),
        //            //        exp_RevsnVal = container[FileHelper.revsnVal].ToString()
        //            //    };

        //            //    ContainerList.Add(containerInfo);
        //            //}

        //            var containerInfo = new ContainerView()
        //            {
        //                FileDataInfo = container,
        //                // TODO : 개체 이니셜라이저 사용해서 파일 목록 리스트 객체 FileItems 생성시 객체 값(file) 할당 (2023.11.23 minjae)
        //                // 참고 URL - https://hyokye0ng.tistory.com/86
        //                FileItems = new List<Dictionary<string, object>> { container },
        //                FileTypeCodeInfo = container[ContainerHelper.fileTypeCodeInfo].ToDictionary(),
        //                StepCodeInfo = container[ContainerHelper.stepCodeInfo].ToDictionary(),
        //                StatusCodeInfo = container[ContainerHelper.statusCodeInfo].ToDictionary(),
        //                CheckCodeInfo = container[ContainerHelper.checkCodeInfo].ToDictionary(),

        //                // TODO : "SharedContainerList" 필요시 구현 예정 (2023.09.22 minjae)
        //                SharedContainerList = new List<Dictionary<string, object>>(),

        //                CreateUserIdAndTime = createUserId + " | " + createDateTime,
        //                exp_FileName = container[ContainerHelper.fileNm].ToString(),
        //                exp_ContainerGroup = container[ContainerHelper.containerGroup].ToString(),
        //                // TODO : 컨테이너 ID(영어 대문자) 프로퍼티 "exp_UpperContainerId" 영어 대문자로 구현(2023.09.22 minjae)
        //                //        프로퍼티 "exp_UpperContainerId" 용도 - 탐색기 화면 뷰(ExplorerV.xaml) "ListView" 영역에 바인딩할 컬럼 용도
        //                // 참고 URL - https://developer-talk.tistory.com/669
        //                exp_UpperContainerId = container[ContainerHelper.containerId].ToString().ToUpper(),
        //                // TODO : 컨테이너 ID(영어 소문자) 프로퍼티 "exp_ContainerId" 영어 대문자로 구현(2023.10.12 minjae)
        //                //        프로퍼티 "exp_ContainerId" 용도
        //                //        - Rest API 메서드 "ExplorerRestServer.GetContainerDetailListAsync" 파라미터 "ContainerPack"에 속하는
        //                //          컨테이너 아이디(containerId)에 영어 소문자로 넘겨줘야 인재 INC 서버 로부터 응답 결과 JSON 데이터를 정상적으로 넘겨 받을 수 있음. 
        //                exp_ContainerId = container[ContainerHelper.containerId].ToString(),
        //                exp_ProjectId = container[ContainerHelper.projectId].ToString(),
        //                exp_TeamId = container[ContainerHelper.teamId].ToString(),

        //                exp_FolderId = container[ContainerHelper.folderId].ToString(),
        //                exp_FileId = container[ContainerHelper.fileId].ToString(),
        //                exp_FileTypeCode = container[ContainerHelper.fileTypeCode].ToString(),
        //                exp_Title = container[ContainerHelper.title].ToString(),

        //                exp_StatusCode = container[ContainerHelper.statusCode].ToString(),
        //                exp_RevsnVal = container[ContainerHelper.revsnVal].ToString(),
        //                exp_ReadYn = container[ContainerHelper.readYn].ToString(),
        //                exp_CheckStatusCode = container[ContainerHelper.checkStatusCode].ToString(),

        //                exp_CreateUserId = createUserId,
        //                exp_CreateDateTime = createDateTime,
        //                exp_TypeCodeName = fileTypeCodeInfoDic[ContainerHelper.fileTypeCodeName].ToString(),
        //                exp_StepCodeName = stepCodeInfoDic[ContainerHelper.stepCodeName].ToString(),
        //                exp_StatusCodeName = statusCodeInfoDic[ContainerHelper.statusCodeName].ToString(),

        //                exp_TranslateStatusCode = translateStatusCode,
        //                exp_TranslateStatusMsg = resultWebViewerData[HubBoxHelper.message].ToString(),
        //                exp_TranslateData = resultWebViewerData.ContainsKey(HubBoxHelper.data) ? resultWebViewerData[HubBoxHelper.data].ToString() : string.Empty,

        //                exp_VisibleWebViewerImg = translateStatusCode.Equals(HubBoxHelper.successStatusCode) ? Visibility.Visible : Visibility.Collapsed,    // (파일) 변환 성공(success) -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "돋보기" 이미지 출력  
        //                exp_VisibleTranslateImg = translateStatusCode.Equals(HubBoxHelper.translateStatusCode) ? Visibility.Visible : Visibility.Collapsed,   // (파일) 변환중입니다.(ACC에서 파일 변환 진행 중) -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "시계" 이미지 변경
        //                exp_VisibleFailureImg = translateStatusCode.Equals(HubBoxHelper.failureStatusCode) ? Visibility.Visible : Visibility.Collapsed,       // (파일) 변환에 실패 한 파일입니다.(ACC에서 파일 변환 실패) -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "세모" 이미지 변경
        //                exp_VisibleNotExistTranslateImg = translateStatusCode.Equals(HubBoxHelper.notExistTranslateStatusCode) ? Visibility.Visible : Visibility.Collapsed,   // (파일) 변환 내역이 없습니다.(해당 프로젝트ID, 팀ID, 컨테이너ID, 파일ID로 이미 변환 된 파일이 없는 경우)
        //                                                                                                                                                                      // -> 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "동그라미(사선 포함)" 이미지 변경
        //            };

        //            // 아래 Visibility 클래스 프로퍼티 모두 Visibility.Collapsed(표시 안함)일 경우 
        //            // "exp_VisibleWebViewerImg", "exp_VisibleTranslateImg", "exp_VisibleFailureImg", "exp_VisibleNotExistTranslateImg"
        //            if (Visibility.Collapsed == containerInfo.exp_VisibleWebViewerImg
        //               && Visibility.Collapsed == containerInfo.exp_VisibleTranslateImg
        //               && Visibility.Collapsed == containerInfo.exp_VisibleFailureImg
        //               && Visibility.Collapsed == containerInfo.exp_VisibleNotExistTranslateImg)
        //                containerInfo.exp_VisibleErrorImg = Visibility.Visible;   // 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "오류" 이미지 변경

        //            // Visibility 클래스 프로퍼티들 중 단 하나라도 Visibility.Visible(표시)일 경우 
        //            // "exp_VisibleWebViewerImg", "exp_VisibleTranslateImg", "exp_VisibleFailureImg", "exp_VisibleNotExistTranslateImg"
        //            else containerInfo.exp_VisibleErrorImg = Visibility.Collapsed;   // 파일 변경 요청한 컨테이너 목록 -> 항목 "보기" 버튼 "오류" 이미지 표시 안함

        //            // 컨테이너 잠금 상태 ("Y": 잠금 ON, "N": 잠금 OFF)
        //            readYn = containerInfo.exp_ReadYn;

        //            switch (readYn)
        //            {
        //                // 체크아웃 요청
        //                case ContainerHelper.yes:
        //                    containerInfo.exp_WorkUserId = AppSetting.Default.Login.LoginId;   // containerInfo.exp_WorkUserId 필드 값은 로그인 사용자 아이디로 변경하여 잠금상태 설정

        //                    break;
        //                // 체크인 요청, 컨테이너 작업 취소 요청
        //                case ContainerHelper.no:
        //                    // TODO : 컨테이너 작업 중인 사용자 아이디 데이터(workUserId) 구할 때 삼항 연산자 구현 (2023.12.20 minjae)
        //                    // 참고 URL - https://spaghetti-code.tistory.com/51
        //                    // TODO : switch - case 문 (체크인 요청 - case ContainerHelper.no:) 필요시 로직 수정 예정(2023.12.19 minjae)
        //                    // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.12.20 minjae)
        //                    // 참고 URL - https://husk321.tistory.com/405
        //                    //containerInfo.exp_WorkUserId = (container[ContainerHelper.workUserId] is null) ? "체크인" : container[ContainerHelper.workUserId].ToString();
        //                    containerInfo.exp_WorkUserId = (container[ContainerHelper.workUserId] is null) ? string.Empty : container[ContainerHelper.workUserId].ToString();

        //                    break;

        //                    // TODO : 변수 "readYn"에 문자열 값이 잘못 들어간 경우가 존재하면 아래 "그 외" default 사항도 추가하기 (2023.12.19 minjae)
        //                    // 그 외
        //                    // default:
        //                    //     containerInfo.exp_WorkUserId = string.Empty;
        //                    //     break;
        //            }

        //            // TODO : 아래 테스트 코드 필요시 참고 (2023.12.08 minjae)
        //            // 체크아웃 요청한 컨테이너 아이디인 경우 
        //            // if (false == string.IsNullOrWhiteSpace(pCheckOutContainerId)
        //            //     && containerInfo.exp_ContainerId.Equals(pCheckOutContainerId))
        //            // {
        //            //     containerInfo.exp_WorkUserId = "체크아웃";
        //            //     // containerInfo.exp_ReadYn     = ;
        //            //     // containerInfo.exp_StatusCode = ;
        //            // }

        //            // 체크인 요청한 컨테이너 아이디인 경우 
        //            // else if (false == string.IsNullOrWhiteSpace(pCheckInContainerId)
        //            //          && containerInfo.exp_ContainerId.Equals(pCheckInContainerId))
        //            // {
        //            //     containerInfo.exp_WorkUserId = "체크인";
        //            //     // containerInfo.exp_ReadYn = ;
        //            //     // containerInfo.exp_StatusCode = ;
        //            // }
        //            // 그 외 나머지 컨테이너 아이디인 경우 
        //            // else
        //            // {
        //            //     containerInfo.exp_WorkUserId = string.Empty;
        //            // }

        //            ContainerList.Add(containerInfo);
        //        }

        //        // TODO : 추후 필요시 컨테이너 리스트에 저장된 컨테이너 아이디 오름차순 정렬 구현하기 (2023.10.27 minjae)
        //        // ContainerList.Sort();   

        //        // TODO : 필요시 ContainerDatas.Clear(); 호출 예정 (2024.09.09 minjae)
        //        // ContainerDatas.Clear();
        //        ContainerDatas.AddRange(ContainerList.ToArray());

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 필요시 아래 주석친 보기 (특정 컨테이너에 속한 파일 변환해서 웹뷰어로 출력) 비동기 메서드 "ShowWebViewerAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 보기 (특정 컨테이너에 속한 파일 변환해서 웹뷰어로 출력)
        /// </summary>
        //private async Task ShowWebViewerAsync(object obj)
        //{
        //    Dictionary<string, object> resultWebViewerData = new Dictionary<string, object>();   // 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 볼 수 있도록 웹뷰어 정보 Dictionary 객체 초기화

        //    ContainerView webViewerContainerInfo = new ContainerView();       // 변환 완료된 파일이 속한 컨테이너 정보 객체 초기화

        //    // string webViewerUrl = string.Empty;    // 변환 처리 완료된 파일 웹뷰어 Url 주소 

        //    // TODO : 보기 (파일 정보 자세히 보기) 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2024.08.22 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                      // 보기 버튼으로 부터 받은 특정 컨테이너 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileIdexp_FileId (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] showWebViewerDatas" 형식으로 형변환 구현 (2024.08.22 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] showWebViewerDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // (파일) 변환 실패한 경우
        //        // 1. (파일) 변환중입니다.(ACC에서 파일 변환 진행 중)
        //        // 2. (파일) 변환에 실패 한 파일입니다.(ACC에서 파일 변환 실패)
        //        // 3. (파일) 변환 내역이 없습니다.(해당 프로젝트ID, 팀ID, 컨테이너ID, 파일ID로 이미 변환 된 파일이 없는 경우)
        //        // 4. 기타 등등...
        //        if (false == showWebViewerDatas[(int)EnumWebViewer.WEBVIEWER].Equals(HubBoxHelper.successWebViewerVisibility)) return;   // 메서드 종료

        //        // 사용자가 선택한 변환 완료된 파일이 속한 컨테이너 정보 가져오기 
        //        webViewerContainerInfo = ContainerDatas.Where(containerInfo => containerInfo.exp_ProjectId.Equals(showWebViewerDatas[(int)EnumWebViewer.PROJECTID])
        //                                                                       && containerInfo.exp_TeamId.Equals(showWebViewerDatas[(int)EnumWebViewer.TEAMID])
        //                                                                       && containerInfo.exp_ContainerId.Equals(showWebViewerDatas[(int)EnumWebViewer.CONTAINERID])
        //                                                                       && containerInfo.exp_FileId.Equals(showWebViewerDatas[(int)EnumWebViewer.FILEID]))
        //                                               .Select(containerInfo => containerInfo)
        //                                               .FirstOrDefault();

        //        // 변환 처리 완료된 파일 웹뷰어 주소가 공백 또는 null이 아닌 경우 
        //        if (false == string.IsNullOrWhiteSpace(webViewerContainerInfo.exp_TranslateData))
        //            Process.Start(new ProcessStartInfo(webViewerContainerInfo.exp_TranslateData) { UseShellExecute = true });

        //        // 변환 처리 완료된 파일 웹뷰어 주소가 공백 또는 null인 경우 
        //        else // 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 볼 수 없는 경우
        //             // (예) - "code": 404, "message": "변환 내역이 없습니다."
        //             // (예2) - "code": 204, "message": "변환에 실패 한 파일입니다."
        //             // (예3) - "code": 202, "message": "변환중입니다."
        //            throw new Exception($"* 웹뷰어 실행 오류 - ({ webViewerContainerInfo.exp_TranslateStatusCode }) *"
        //                                + $"\r\n\r\n제목 - { webViewerContainerInfo.exp_Title }"
        //                                + $"\r\n컨테이너 아이디 - { webViewerContainerInfo.exp_ContainerId.ToUpper() }"
        //                                + $"\r\n사유 - { webViewerContainerInfo.exp_TranslateStatusMsg }");

        //        // if (false == string.IsNullOrWhiteSpace(HubBoxView.ResponseDatas.Data))
        //        //     Process.Start(new ProcessStartInfo(HubBoxView.ResponseDatas.Data) { UseShellExecute = true });
        //        // else Process.Start(new ProcessStartInfo(WebViewerUrn) { UseShellExecute = true });


        //        // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.08.30 minjae)
        //        // MessageBox.Show($" 특정 컨테이너에 속한 파일 정보 웹뷰어 URL 주소\r\n{ webViewerInfoDic }");

        //        // WebViewerPack = new WebViewerPack(showWebViewerDatas[(int)EnumWebViewer.PROJECTID],
        //        //                                   showWebViewerDatas[(int)EnumWebViewer.TEAMID],
        //        //                                   showWebViewerDatas[(int)EnumWebViewer.CONTAINERID],
        //        //                                   showWebViewerDatas[(int)EnumWebViewer.FILEID]);

        //        // 1. 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 볼수 있도록 Http URL 주소 문자열로 가져오기 
        //        // resultWebViewerData = await HubBoxRestServer.GetWebViewerUrnAsync(Client, WebViewerPack, LoginView.RequestHeaders);

        //        // 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 볼 수 있는 경우
        //        // (예) - "code": 200, "message": "success"
        //        //if (resultWebViewerData[EnumWebViewerResult.CODE.ToDescription()].ToString().Equals(HubBoxHelper.successStatusCode)
        //        //   && resultWebViewerData[EnumWebViewerResult.MESSAGE.ToDescription()].ToString().Equals(HubBoxHelper.successMessage)
        //        //   && false == string.IsNullOrWhiteSpace(resultWebViewerData[EnumWebViewerResult.DATA.ToDescription()].ToString()))
        //        //{
        //        //    // 2. 1번 과정에서 웹서버로 부터 리턴 받은 Http URL 주소 문자열 가지고 웹뷰어 출력하기 
        //        //    // TODO : 웹뷰어 팝업 화면 출력 구현 (2024.08.23 minjae)
        //        //    // 참고 URL   - https://yongtech.tistory.com/58
        //        //    // 참고 2 URL - https://findfun.tistory.com/485

        //        //    // TODO : 웹뷰어 구글 크롬(chrome.exe)으로 출력 구현 (2024.08.23 minjae)
        //        //    // 참고 URL - https://www.codeproject.com/Questions/5286855/How-do-I-open-Google-chrome-in-Csharp
        //        //    // Process.Start("chrome.exe", pURL);
        //        //    // Process.Start("firefox.exe", pURL);

        //        //    // TODO : .net FrameWork 말고 .net Core 6.0 이상 버전에서 웹뷰어) 출력 오류시 아래 처럼 구현 (2024.08.23 minjae)
        //        //    // 참고 URL - https://endev.tistory.com/m/237
        //        //    Process.Start(new ProcessStartInfo(resultWebViewerData[EnumWebViewerResult.DATA.ToDescription()].ToString()) { UseShellExecute = true });

        //        //}
        //        //// 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 볼 수 없는 경우
        //        //// (예) - "code": 404, "message": "변환 내역이 없습니다."
        //        //// (예2) - "code": 204, "message": "변환에 실패 한 파일입니다."
        //        //// (예3) - "code": 202, "message": "변환중입니다."
        //        //else
        //        //    throw new Exception($"* 웹뷰어 실행 오류 - ({ resultWebViewerData[EnumWebViewerResult.CODE.ToDescription()] }) *"
        //        //                        + $"\r\n\r\n제목 - { showWebViewerDatas[(int)EnumWebViewer.TITLE] }"
        //        //                        + $"\r\n컨테이너 아이디 - { showWebViewerDatas[(int)EnumWebViewer.CONTAINERID].ToUpper() }"
        //        //                        + $"\r\n사유 - { resultWebViewerData[EnumWebViewerResult.MESSAGE.ToDescription()] }");

        //        // MessageBox.Show($" 특정 컨테이너에 속한 파일 정보 웹뷰어 URL 주소\r\n{ webViewerInfoDic }");

        //        // return;
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}

        // TODO : 필요시 아래 주석친 컨테이너 정보 + 파일정보 조회 비동기 메서드 "DetailSearchAsync" 참고 (2024.10.30 minjae)
        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 정보 + 파일정보 조회
        /// </summary>
        //private async Task DetailSearchAsync(object obj)
        //{
        //    // string tokenKey         = AppSetting.Default.Login.Token.resultData;  // 로그인 토큰 키 
        //    // string inputTenantId    = LoginHelper.inputTenantId;                  // 테넌트 아이디

        //    // TODO : 컨테이너 정보, 파일정보 리스트 객체 "detailList" 필요시 사용 예정 (2023.10.17 minjae)
        //    // List<Dictionary<string, object>> detailList = new List<Dictionary<string, object>>();  
        //    List<Dictionary<string, object>> linkedNodeInfoList = new List<Dictionary<string, object>>();     // 연관된 부모 컨테이너 정보 리스트

        //    // TODO : 연관 컨테이너 정보(메타 데이터) 리스트 객체 "linkedNodeMetaDataList" 필요시 사용 예정 (2023.10.17 minjae)
        //    // List<Dictionary<string, object>> linkedNodeMetaDataList = new List<Dictionary<string, object>>();  // 연관 컨테이너 정보(메타 데이터) 리스트

        //    Dictionary<string, object> detailDic = null;          // 컨테이너 정보, 파일정보 Dictionary

        //    List<Dictionary<string, object>> metaDataList = new List<Dictionary<string, object>>();     // 메타데이터 정보 리스트

        //    Dictionary<string, object> fileTypeCodeInfoDic = null;          // 파일유형 Dictionary

        //    // TODO : 마일스톤 업무 리스트 객체 "taskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
        //    // List<Dictionary<string, object>> taskDetailList      = null;          // 마일스톤 업무 리스트
        //    Dictionary<string, object> taskDetailDic = null;          // 마일스톤 업무 Dictionary
        //    Dictionary<string, object> milestoneTaskInfoDic = null;          // 특정 마일스톤 업무 정보 Dictionary


        //    string createUserId = string.Empty;                                // 작성자
        //    string containerId = string.Empty;                                // 컨테이너 아이디

        //    string projectId = string.Empty;                                // 프로젝트 아이디
        //    string teamId = string.Empty;                                // 팀 아이디

        //    string revsnVal = string.Empty;                                // 버전
        //    string title = string.Empty;                                // 제목
        //    string fileTypeSystemCode = string.Empty;                                // 파일유형 - systemCode
        //    string fileTypeCodeNm = string.Empty;                                // 파일유형 - codeName

        //    string softMetaNm = string.Empty;                                // 소프트웨어 metaName
        //    string softNm = string.Empty;                                // 소프트웨어

        //    string softVerMetaNm = string.Empty;                                // 소프트웨어버전 metaName
        //    string softVer = string.Empty;                                // 소프트웨어버전

        //    string keywordMetaNm = string.Empty;                                // 키워드/태그 metaName
        //    string keyword = string.Empty;                                // 키워드/태그

        //    string taskDetailIdx = string.Empty;                                // 마일스톤 업무 인덱스
        //    string taskDetailType = string.Empty;                                // 마일스톤 업무 유형

        //    string fileNm = string.Empty;                                // 파일명
        //    string fileId = string.Empty;                                // 파일 아이디

        //    // TODO : 아래 주석친 소스 코드 3줄(parameters, containerDatas, containerDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //    // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
        //    // 참고 URL - https://nonstop-antoine.tistory.com/36
        //    // var parameters = obj as IEnumerable;                                    // 컨테이너 아이디 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 
        //    // var containerDetails = new List<string>();                              // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDetails"
        //    // var containerDataDic = new Dictionary<string, string>();                // 컨테이너 정보 데이터를 담는 Dictionary 객체 containerDataDic 생성

        //    // 컨테이너 아이디 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf 
        //    var parameters = (object[])obj;                                   // 컨테이너 아이디 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerDetails" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] containerDetails = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
        //    // var containerDetails = parameters.ToList();                             // parameters 를 리스트로 변환할 때 사용하는 리스트 객체 "containerDetails"

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 리스트 객체 "DetailList", "MetaDataList", "TaskDetailList" 필요시 구현 예정 (2023.10.12 minjae)
        //        // DetailList.Clear();            // 파일 컨테이너 정보 + 메타데이터 + 파일정보 리스트 초기화
        //        // MetaDataList.Clear();          // 메타데이터 정보 리스트 초기화
        //        // TaskDetailList.Clear();        // 마일스톤 업무 리스트 초기화

        //        // TODO : 아래 주석친 foreach문 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //        // containerDatas 안에 존재하는 파일 정보 반복문으로 방문 -> List 객체 containerDetails에 파일 정보 데이터 추가 
        //        // foreach (var data in containerDatas)
        //        // {
        //        //     containerDetails.Add(data.ToString());
        //        // }

        //        // TODO : 아래 주석친 소스코드 3줄(containerDataDic.Add) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //        // containerDataDic.Add(ProjectHelper.projectId, containerDetails[(int)EnumContainerDetail.PROJECTID]);
        //        // containerDataDic.Add(TeamHelper.teamId, containerDetails[(int)EnumContainerDetail.TEAMID]);
        //        // containerDataDic.Add(ContainerHelper.containerId, containerDetails[(int)EnumContainerDetail.CONTAINERID]);

        //        // TODO : 아래 소스코드 ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 필요 없을시 삭제 예정 (2023.12.05 minjae)
        //        // TODO : ContainerHelper.cs -> ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 (2023.10.12 minjae)  
        //        //ContainerPack = new ContainerPack
        //        //{
        //        //    projectId = containerDataDic[ProjectHelper.projectId],
        //        //    teamId = containerDataDic[TeamHelper.teamId],
        //        //    containerId = containerDataDic[ContainerHelper.containerId]
        //        //};

        //        // TODO : ContainerHelper.cs -> ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 (2023.10.12 minjae)  
        //        //ContainerPack = new ContainerPack
        //        //{
        //        //    projectId   = containerDetails[(int)EnumContainerDetail.PROJECTID],
        //        //    teamId      = containerDetails[(int)EnumContainerDetail.TEAMID],
        //        //    containerId = containerDetails[(int)EnumContainerDetail.CONTAINERID]
        //        //};
        //        ContainerPack = new ContainerPack(containerDetails[(int)EnumContainerDetail.PROJECTID],
        //                                          containerDetails[(int)EnumContainerDetail.TEAMID],
        //                                          containerDetails[(int)EnumContainerDetail.CONTAINERID],
        //                                          string.Empty,
        //                                          string.Empty);

        //        // ExplorerV.xaml 화면 우측 하단에 할당할 선택한 특정 컨테이너 아이디 값 영어 대문자로 할당
        //        SelectedContainerId = ContainerPack.containerId.ToUpper();

        //        // Rest API 특정 컨테이너 상세조회 실행
        //        var total_Info_List = await ExplorerRestServer.GetDetailListAsync(Client, ContainerPack, LoginView.RequestHeaders);

        //        foreach (var infoDic in total_Info_List)
        //        {
        //            // TODO : Dictionary 객체 "infoDic"에 존재하는 value 값 가져와서 리스트로 구현 (2023.10.18 minjae)
        //            var containerList = infoDic.SelectMany(container => container.Value).ToList();

        //            // 연관 컨테이너의 정보(ContainerHelper.linkedNodeInfo)가 존재하지 않을 경우 continue 처리
        //            if (containerList.Count.Equals((int)EnumLinkedRelation.NONE)) continue;

        //            // TODO : Dictionary<string, List<Dictionary<string, object>>> 클래스 객체 "infoDic"에 존재하는 Key 값 가져와서 리스트로 변환 (2023.10.18 minjae)
        //            // 참고 URL - https://www.techiedelight.com/ko/get-list-of-keys-and-values-in-a-dictionary-in-csharp/
        //            var infoTypeList = new List<string>(infoDic.Keys);

        //            // TODO : 리스트 객체 "infoTypeList"의 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.18 minjae)
        //            // 참고 URL - https://developer-talk.tistory.com/294
        //            var infoType = infoTypeList.FirstOrDefault();

        //            foreach (var data in containerList)
        //            {
        //                switch (infoType)
        //                {
        //                    // 연관된 부모 컨테이너 정보 
        //                    case ContainerHelper.linkedNodeInfo:
        //                        linkedNodeInfoList.Add(data);
        //                        // TODO : 아래 프로퍼티 "linkedNodeMetaDataList"에 할당 해야할 메타 데이터 리스트 "MetaDataList"의 경우 
        //                        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //                        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //                        //        문서 정리하여 공유 받고 구현 예정 (2023.11.01 minjae)
        //                        // linkedNodeMetaDataList = MetaDataList;
        //                        break;

        //                    // 컨테이너 정보, 파일정보
        //                    case ContainerHelper.detail:
        //                        // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 객체 containerDetailList 구현 예정 (2023.10.17 minjae)
        //                        // containerDetailList = infoList;
        //                        detailDic = data;
        //                        createUserId = data[ContainerHelper.createUserId].ToString();                   // 작성자 
        //                        containerId = data[ContainerHelper.containerId].ToString();                    // 컨테이너 아이디

        //                        projectId = data[ContainerHelper.projectId].ToString();                      // 프로젝트 아이디
        //                        teamId = data[ContainerHelper.teamId].ToString();                         // 팀 아이디

        //                        revsnVal = data[ContainerHelper.revsnVal].ToString();                       // 버전
        //                        title = data[ContainerHelper.title].ToString();                          // 제목
        //                        fileNm = data[ContainerHelper.fileNm].ToString();                         // 파일명
        //                        fileId = data[ContainerHelper.fileId].ToString();                         // 파일 아이디 

        //                        // 파일유형 Dictionary
        //                        fileTypeCodeInfoDic = data[ContainerHelper.fileTypeCodeInfo].ToDictionary();
        //                        fileTypeSystemCode = fileTypeCodeInfoDic[ContainerHelper.systemCode].ToString();      // 파일유형 - systemCode
        //                        fileTypeCodeNm = fileTypeCodeInfoDic[ContainerHelper.codeName].ToString();        // 파일유형 - codeName
        //                        break;

        //                    // 메타데이터 정보
        //                    case ContainerHelper.metaDataList:
        //                        metaDataList.Add(data);

        //                        var metaCode = data[ContainerHelper.metaCode].ToString();

        //                        // "metaCode": "soft-ifo" 일 경우 
        //                        if (metaCode.Equals(ContainerHelper.soft_ifo))
        //                        {
        //                            var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
        //                            softMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();   // 소프트웨어 metaName
        //                            softNm = data[ContainerHelper.metaComment].ToString();          // 소프트웨어
        //                        }

        //                        // "metaCode": "soft-ver" 일 경우 
        //                        if (metaCode.Equals(ContainerHelper.soft_ver))
        //                        {
        //                            var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
        //                            softVerMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();   // 소프트웨어버전 metaName
        //                            softVer = data[ContainerHelper.metaComment].ToString();          // 소프트웨어버전
        //                        }

        //                        // "metaCode": "find-tag" 일 경우 
        //                        if (metaCode.Equals(ContainerHelper.find_tag))
        //                        {
        //                            var parentMetaCode = data[ContainerHelper.parentMetaCode].ToDictionary();   // parentMetaCode Dictionary
        //                            keywordMetaNm = parentMetaCode[ContainerHelper.metaName].ToString();   // 키워드/태그 metaName
        //                            keyword = data[ContainerHelper.metaComment].ToString();          // 키워드/태그
        //                        }

        //                        break;

        //                    // 마일스톤 업무
        //                    case ContainerHelper.taskDetail:
        //                        taskDetailDic = data;
        //                        // TODO : Rest API 특정 컨테이너 상세조회 실행 되고 결과로 리턴 받은 json 데이터 결과값 중 "milestoneTaskInfo" 사라져서
        //                        //        탭 "미리보기" 항목에 출력되는 참조(연관)컨테이너 리스트가 출력되지 않는 오류 발생
        //                        //        해당 오류 원인 파악 후 아래 코드 taskDetailDic[ContainerHelper.milestoneTaskInfo] 개선 작업 예정 (2024.08.21 minjae)
        //                        milestoneTaskInfoDic = taskDetailDic is null ? null : taskDetailDic[ContainerHelper.milestoneTaskInfo].ToDictionary();
        //                        // taskDetailDic.Where(x => x.Key.Equals("title")).Select(value => value).ToString();


        //                        taskDetailIdx = milestoneTaskInfoDic is null ? ContainerHelper.taskIdxNone : milestoneTaskInfoDic[ContainerHelper.idx].ToString();

        //                        // TODO : 마일스톤 업무 리스트 객체 "taskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
        //                        // taskDetailList = infoDic[key];
        //                        // 마일스톤 - 업무 유형 데이터 마일스톤 관련 Rest API를 통해 데이터를 받아와야 하는 경우 아래 소스코드 수정 예정(2023.10.17 minjae)
        //                        // taskDetailType = taskDetailDic == null ? ContainerHelper.taskDetailTypeNone : taskDetailDic.Values.ToString();
        //                        taskDetailType = milestoneTaskInfoDic is null ? ContainerHelper.taskTypeNone : milestoneTaskInfoDic[ContainerHelper.title].ToString();
        //                        // taskDetailType = milestoneTaskInfoDic[ContainerHelper.title].ToString();

        //                        break;
        //                }
        //            }
        //        }

        //        var detailData = new ContainerDetailView()
        //        {
        //            LinkedNodeInfoList = linkedNodeInfoList,

        //            // TODO : 아래 프로퍼티 "LinkedNodeMetaDataList"에 할당 해야할 메타 데이터 리스트 "linkedNodeMetaDataList"의 경우 
        //            //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //            //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //            //        문서 정리하여 공유 받고 구현 예정 (2023.11.01 minjae)
        //            // LinkedNodeMetaDataList = linkedNodeMetaDataList,

        //            // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 객체 containerDetailList 구현 예정 (2023.10.17 minjae)
        //            // ContainerDetailList = containerDetailList,
        //            DetailDic = detailDic,

        //            FileTypeCodeDic = fileTypeCodeInfoDic,

        //            MetaDataList = metaDataList,

        //            // TODO : 추후 필요시 "마일스톤 업무" 리스트 객체 "TaskDetailList" 필요시 사용 예정 (2023.10.17 minjae)
        //            // TaskDetailList      = taskDetailList,
        //            TaskDetailDic = taskDetailDic,
        //            MilestoneTaskInfoDic = milestoneTaskInfoDic,

        //            // Detail
        //            exp_CreateUserId = createUserId,
        //            exp_ContainerId = containerId,
        //            // exp_SelectedContainerId = ContainerPack.containerId.ToUpper(),   
        //            // exp_UpperContainerId    = containerId.ToUpper(),                 // ExplorerV.xaml 화면 우측 하단에 할당할 컨테이너 아이디 값 영어 대문자로 할당

        //            exp_ProjectId = projectId,
        //            exp_TeamId = teamId,

        //            exp_RevsnVal = revsnVal,
        //            exp_Title = title,
        //            exp_FileTypeSystemCode = fileTypeSystemCode,
        //            exp_FileTypeCodeName = fileTypeCodeNm,

        //            // 메타 데이터 
        //            // TODO : 프로퍼티 "exp_SoftMetaName" 필요시 사용 예정 (2023.10.23 minjae)
        //            // exp_SoftMetaName    = softMetaNm,
        //            exp_SoftName = softNm,

        //            // TODO : 프로퍼티 "exp_SoftVerMetaName" 필요시 사용 예정 (2023.10.23 minjae)
        //            // exp_SoftVerMetaName = softVerMetaNm,
        //            exp_SoftVer = softVer,

        //            // TODO : 프로퍼티 "exp_KeywordTagMetaName" 필요시 사용 예정 (2023.10.23 minjae)
        //            //exp_KeywordTagMetaName = keywordMetaNm,
        //            exp_KeywordTag = keyword,

        //            // 마일스톤
        //            exp_TaskDetailIdx = taskDetailIdx,
        //            exp_TaskDetailType = taskDetailType,

        //            // Detail - 파일명(이름)
        //            exp_FileName = fileNm,

        //            // Detail - 파일 아이디
        //            exp_FileId = fileId,

        //            // 기본정보 탭 - Label (구분 항목)
        //            exp_DivLabel = ContainerHelper.divLabel,
        //            exp_ContentsLabel = ContainerHelper.contentsLabel,
        //            exp_CreateUserLabel = ContainerHelper.createUserLabel,
        //            exp_VersionLabel = ContainerHelper.versionLabel,
        //            exp_TitleLabel = ContainerHelper.titleLabel,
        //            exp_SoftLabel = softMetaNm,
        //            exp_SoftVerLabel = softVerMetaNm,
        //            exp_KeywordTagLabel = keywordMetaNm,
        //            exp_FileTypeCodeLabel = ContainerHelper.fileTypeCodeLabel,
        //            exp_TaskDetailLabel = ContainerHelper.taskDetailLabel,
        //            exp_FileNameLabel = ContainerHelper.fileNameLabel
        //        };

        //        // TODO : 메서드 "DetailTypeListCreate" 호출시 전달 인자로 detailData 설정할 때
        //        //        ref(기존 변수 detailData안에 들어있는 값을 메서드 "DetailTypeListCreate" 실행시 수정)로 구현 (2023.10.23 minjae)
        //        // 참고 URL - https://yeko90.tistory.com/entry/c-%EA%B8%B0%EC%B4%88-ref-vs-out-%EC%B0%A8%EC%9D%B4
        //        // 참고 2 URL - https://kdsoft-zeros.tistory.com/142
        //        // 참고 3 URL - https://codingcoding.tistory.com/56
        //        DetailTypeListCreate(ref detailData, fileTypeCodeInfoDic, taskDetailDic);   // ComboBox "파일유형", "마일스톤" List 생성 

        //        // TODO : 아래 소스코드 "DisplayType", 메서드 "DisplaySetting" 필요시 수정 예정 (2023.12.26 minjae) 
        //        DisplayType = CobimHelper.TypeOfDetailSearch;
        //        // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
        //        DisplaySetting(DisplayType);

        //        // 연관 컨테이너의 정보(ContainerHelper.linkedNodeInfo)가 존재할 경우(EnumLinkedRelation.EXISTENCE 보다 크거나 같은 경우)
        //        // 비동기 메서드 "LinkedNodeInfoCreate" 호출
        //        if (linkedNodeInfoList.Count >= ((int)EnumLinkedRelation.EXISTENCE))
        //        {
        //            // TODO : 아래 비동기 메서드 "LinkedNodeInfoCreate"에 전달할 메서드 파라미터 메타 데이터 리스트 "MetaDataList"의 경우 
        //            //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //            //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //            //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        //            // await LinkedNodeInfoCreate(linkedNodeInfoList, linkedNodeMetaDataList); // 연관 컨테이너 정보 Tree 생성 
        //            await LinkedNodeInfoCreate(linkedNodeInfoList); // 연관 컨테이너 정보 Tree 생성 
        //        }

        //        // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측으로 마일스톤 목록 조회 RestAPI 요청 후 개발 진행 (2023.10.20 minjae)
        //        // "마일스톤" List 생성 
        //        // var taskDetailList = await ExplorerRestServer.GetTaskDetailListAsync();
        //        // detailData.TaskDetailType = await ExplorerRestServer.GetTaskDetailListAsync();

        //        // TODO : 필요시 DetailList.Clear(); 호출 예정 (2024.09.09 minjae)
        //        // DetailList.Clear();
        //        DetailList.Add(detailData);

        //        // TODO : 필요시 DetailDatas.Clear(); 호출 예정 (2024.09.09 minjae)
        //        // DetailDatas.Clear();
        //        DetailDatas.AddRange(DetailList.ToArray());

        //        //DisplayType  = CobimHelper.TypeOfDetailSearch;
        //        //// IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
        //        //DisplaySetting(DisplayType);

        //        // TODO : 테스트 코드 - 강제로 오류 발생하도록 Exception 생성 코드 필요시 사용 예정 (2023.10.24 minjae)
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // throw new Exception("테스트 오류 코드");

        //        // MessageBox.Show("파일 기본정보 - 컨테이너 정보 테스트");
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
        //        Log.Error(ex.StackTrace);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
        //        Log.Error(ex.ToString());

        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}

        // TODO : 필요시 아래 주석친 ComboBox "파일유형", "마일스톤" List 생성 메서드 "DetailTypeListCreate" 참고 (2024.10.30 minjae)
        // TODO : 메서드 "DetailTypeListCreate" 호출시 메서드 파라미터로 pContainerInfo 설정할 때
        //        ref(기존 변수 pContainerInfo안에 들어있는 값을 메서드 "DetailTypeListCreate" 실행시 수정)로 구현 (2023.10.23 minjae)
        // 참고 URL - https://yeko90.tistory.com/entry/c-%EA%B8%B0%EC%B4%88-ref-vs-out-%EC%B0%A8%EC%9D%B4
        // 참고 2 URL - https://kdsoft-zeros.tistory.com/142
        // 참고 3 URL - https://codingcoding.tistory.com/56
        /// <summary>
        /// ComboBox "파일유형", "마일스톤" List 생성 메서드 DetailTypeListCreate
        /// </summary>
        //private void DetailTypeListCreate(ref ContainerDetailView pContainerInfo, Dictionary<string, object> pFileTypeCodeInfoDic, Dictionary<string, object> pTaskDetailDic)
        //{
        //    string fileTypeSystemCode = string.Empty;   // 파일유형 - systemCode
        //    string fileTypeCodeNm = string.Empty;   // 파일유형 - codeName

        //    string taskIdx = string.Empty;   // 마일스톤 - 업무 인덱스                             
        //    string taskType = string.Empty;   // 마일스톤 - 업무 유형

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    // var ContainerInfo = pContainerInfo;

        //    try
        //    {
        //        fileTypeSystemCode = pContainerInfo.exp_FileTypeSystemCode;   // 파일유형 - systemCode
        //        fileTypeCodeNm = pContainerInfo.exp_FileTypeCodeName;     // 파일유형 - codeName

        //        taskIdx = pContainerInfo.exp_TaskDetailIdx;        // 마일스톤 - 업무 인덱스
        //        taskType = pContainerInfo.exp_TaskDetailType;       // 마일스톤 - 업무 유형

        //        // TODO : pContainerInfo.FileTypeList.Clear(); 소스코드 실행시 FileTypeList 값이 null이므로 null Exception 발생해서 사용 안함.(2023.10.23 minjae)
        //        // pContainerInfo.FileTypeList.Clear();    // ComboBox - 파일유형 리스트 초기화

        //        // TODO : ComboBox "파일유형" 리스트 생성 구현 (2023.10.24 minjae)
        //        pContainerInfo.FileTypeList = new List<FileTypeView>
        //        {
        //            // TODO : 개체 이니셜라이저 사용해서 파일 유형 리스트 객체 pContainerInfo.FileTypeList 생성시 객체 값(exp_systemCode, exp_codeName) 할당 (2023.11.23 minjae)
        //            // 참고 URL - https://hyokye0ng.tistory.com/86
        //            // new FileTypeView { exp_systemCode = ContainerHelper.systemCodeModel, exp_codeName = ContainerHelper.codeNameModel },
        //            // new FileTypeView { exp_systemCode = ContainerHelper.systemCodeReprt, exp_codeName = ContainerHelper.codeNameReprt }
        //            new FileTypeView(ContainerHelper.systemCodeModel, ContainerHelper.codeNameModel),
        //            new FileTypeView(ContainerHelper.systemCodeReprt, ContainerHelper.codeNameReprt)
        //        };

        //        // TODO : ComboBox "마일스톤" 리스트 생성 구현 (2023.10.24 minjae)
        //        pContainerInfo.TaskDetailList = new List<TaskDetailView>
        //        {
        //            // TODO : 개체 이니셜라이저 사용해서 마일스톤 리스트 객체 pContainerInfo.FileTypeList 생성시 객체 값(exp_idx, exp_title) 할당 (2023.11.23 minjae)
        //            // 참고 URL - https://hyokye0ng.tistory.com/86
        //            //new TaskDetailView { exp_idx = ContainerHelper.taskIdxNone,     exp_title = ContainerHelper.taskTypeNone     },     // 관련업무선택
        //            //new TaskDetailView { exp_idx = ContainerHelper.taskIdxDefine,   exp_title = ContainerHelper.taskTypeDefine   },     // 요구사항정의            
        //            //new TaskDetailView { exp_idx = ContainerHelper.taskIdxEntity,   exp_title = ContainerHelper.taskTypeEntity   },     // 엔터티도출
        //            //new TaskDetailView { exp_idx = ContainerHelper.taskIdxCreation, exp_title = ContainerHelper.taskTypeCreation },     // 요구사항정의서 작성
        //            //new TaskDetailView { exp_idx = ContainerHelper.taskIdxUI,       exp_title = ContainerHelper.taskTypeUI       },     // UI설계서 작성
        //            new TaskDetailView (ContainerHelper.taskIdxNone,     ContainerHelper.taskTypeNone     ),     // 관련업무선택
        //            new TaskDetailView (ContainerHelper.taskIdxDefine,   ContainerHelper.taskTypeDefine   ),     // 요구사항정의            
        //            new TaskDetailView (ContainerHelper.taskIdxEntity,   ContainerHelper.taskTypeEntity   ),     // 엔터티도출
        //            new TaskDetailView (ContainerHelper.taskIdxCreation, ContainerHelper.taskTypeCreation ),     // 요구사항정의서 작성
        //            new TaskDetailView (ContainerHelper.taskIdxUI,       ContainerHelper.taskTypeUI       )      // UI설계서 작성
        //        };


        //        // TODO : 리스트 객체 "pContainerInfo.FileTypeList"의 조건을 만족하는 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.18 minjae)
        //        // 참고 URL - https://developer-talk.tistory.com/294
        //        // TODO : 리스트 객체 "pContainerInfo.FileTypeList"의 FirstOrDefault 메서드 사용할 때, Contains 메서드 사용해서 조건 1, 조건 2 만족하는 데이터 있는지 찾기 (2023.10.23 minjae) 
        //        // 참고 URL - https://developer-talk.tistory.com/585 
        //        // FirstOrDefault (where 조건절과 하는 역할 비슷하고 조건을 만족하는 요소가 존재하는지 체크 기능)
        //        // 조건 1 - pContainerInfo.FileTypeList에 할당된 (파일유형 - systemCode - fileType.exp_systemCode) 값과 fileTypeSystemCode에 할당된 (컨테이너 정보 파일유형 - systemCode) 가 동일해야 한다.
        //        // 조건 2 - pContainerInfo.FileTypeList에 할당된 (파일유형 - codeName - fileType.exp_codeName) 값과 fileTypeCodeNm에 할당된 (컨테이너 정보 파일유형 - codeName) 가 동일해야 한다. 
        //        pContainerInfo.SelectParam.FileTypeCodeInfo = pContainerInfo.FileTypeList.FirstOrDefault(fileType => fileType.exp_systemCode.Contains(fileTypeSystemCode)
        //                                                                                                          && fileType.exp_codeName.Contains(fileTypeCodeNm));

        //        // TODO : 리스트 객체 "pContainerInfo.TaskDetailList"의 조건을 만족하는 첫 번째 요소 값을 반환 하도록 메서드 "FirstOfDefault" 사용 (2023.10.24 minjae)
        //        pContainerInfo.SelectParam.TaskDetail = pContainerInfo.TaskDetailList.FirstOrDefault(taskDetailType => taskDetailType.exp_idx.Contains(taskIdx)
        //                                                                                                            && taskDetailType.exp_title.Contains(taskType));

        //        // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 minjae)
        //        //pContainerInfo.TaskDetailType = pMilestoneTaskInfoDic.Where().Select()).
        //        //pContainerInfo.TaskDetailType.Title
        //        // var result = await ExplorerRestServer.GetTaskDetailListAsync();


        //        // TODO : ComboxBox - 파일유형에 바인딩할 BindableCollection에 값 할당 및 서버로부터 받은 값을 화면에 출력 할 수 있도록
        //        //        메서드 파라미터 "pContainerInfo", "pFileTypeCodeInfoDic" 활용하기 (2023.10.20 minjae)
        //        //        컨테이너 정보 영역 "마일스톤" ComboBox 구현시 바인딩할 BindableCollection 객체 TaskDetailType 구현(2023.10.20 minjae)
        //        // pContainerInfo.FileType
        //        // exp_systemCode 
        //        // exp_codeName 
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
        //        Log.Error(ex.StackTrace);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.19 minjae) 
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // 참고 3 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.exception.stacktrace?view=net-7.0
        //        Log.Error(ex.ToString());

        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 필요시 연관 컨테이너 정보 Tree 생성 비동기 메서드 "LinkedNodeInfoCreate" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 연관 컨테이너 정보 Tree 생성 
        /// </summary>
        /// <returns></returns>
        // private async Task LinkedNodeInfoCreate(List<Dictionary<string, object>> pLinkedNodeInfoList, List<Dictionary<string, object>> pLinkedNodeMetaDataList)
        //private async Task LinkedNodeInfoCreate(List<Dictionary<string, object>> pLinkedNodeInfoList)
        //{
        //    string rootContainerId = string.Empty;      // 루트 컨테이너 아이디

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 아래 주석친 코드 "DisplayType", 메서드 "DisplaySetting" 필요시 사용 예정 (2023.12.26 minjae)
        //        // DisplayType = CobimHelper.TypeOfLinkedNodeInfoCreate;
        //        // DisplaySetting(DisplayType);


        //        // TODO : Linq 확장메서드 Where 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        // 루트 노드 컨테이너 아이디 찾기
        //        // 부모 컨테이너 아이디(ContainerHelper.targetId)는 존재하지 않고(== null),
        //        // 자식 컨테이너 아이디(또는 자기 자신 컨테이너 ID)(ContainerHelper.sourceId)만 존재하는 경우(is not null)
        //        // rootContainerId = pLinkedNodeInfoList.Where(node => node[ContainerHelper.targetId] is null
        //        //                                                  && node[ContainerHelper.sourceId] is not null)
        //        //                                      .Select(node => node[ContainerHelper.sourceId].ToString())
        //        //                                      .FirstOrDefault();

        //        // TODO : Linq 확장메서드 Where 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        // TODO : 추후 using static CobimExplorer.Data.Models.CobimBase.Explorer.ExplorerView; 문을 사용하지 않고도 ExplorerView.LinkedNodeView -> LinkedNodeView만 쓸 수 있도록 구현하기 (2023.11.21 minjae) 
        //        // 루트 노드(컨테이너) 찾기
        //        // 부모 컨테이너 아이디(ContainerHelper.targetId)는 존재하지 않고(== null),
        //        // 자식 컨테이너 아이디(또는 자기 자신 컨테이너 ID)(ContainerHelper.sourceId)만 존재하는 경우(is not null)
        //        LinkedNodeView rootContainer = pLinkedNodeInfoList.Where(node => node[ContainerHelper.targetId] is null
        //                                                                      && node[ContainerHelper.sourceId] is not null)
        //                                                          //.Select(node => new LinkedNodeView
        //                                                          //                    {
        //                                                          //                        // TreeView 영역 루트 노드 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
        //                                                          //                        exp_TreeName = node[ContainerHelper.sourceId].ToString().ToUpper()
        //                                                          //                        // LinkedNodeInfoDic = linkedNodeInfoList.Where(node => node.Values.Equals())
        //                                                          //                    })

        //                                                          // TreeView 영역 루트 노드 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
        //                                                          .Select(node => new LinkedNodeView(node[ContainerHelper.sourceId].ToString().ToUpper()))
        //                                                          .FirstOrDefault();


        //        // TODO : 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        //        // TODO : 루트 컨테이너 "rootContainer"에 루트 컨테이너 아이디 "rootContainerId"에 속하는 메타데이터를
        //        //        메타데이터 리스트 객체 "pLinkedNodeMetaDataList"에서 Linq로 찾아서 "new LinkedNodeView" 프로퍼티 "LinkedNodeInfoDic"에 할당하기 (2023.11.01 minjae)
        //        // LinkedNodeView rootContainer = pLinkedNodeMetaDataList.Where(meta => meta[ContainerHelper.containerId].ToString().Equals(rootContainerId))
        //        //                                                       .Select(meta => new LinkedNodeView
        //        //                                                                           {
        //        //                                                                               exp_TreeName = rootContainerId,
        //        //                                                                               LinkedNodeInfoDic = meta
        //        //                                                                           })
        //        //                                                       .FirstOrDefault();

        //        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        // 루트 노드(컨테이너)가 존재하지 않을 경우 리턴 
        //        if (rootContainer is null) return;

        //        LinkedNodeInfoList.Add(rootContainer);

        //        // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        //        // 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가
        //        // await FillLinkedNodes(rootContainer, pLinkedNodeInfoList, pLinkedNodeMetaDataList);
        //        await FillLinkedNodes(rootContainer, pLinkedNodeInfoList);

        //        LinkedNodeInfoDatas.Clear();   // 연관 컨테이너 정보 Collection 초기화
        //        LinkedNodeInfoDatas.AddRange(LinkedNodeInfoList.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
        //        Log.Error(ex.StackTrace);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
        //        Log.Error(ex.ToString());

        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 필요시 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가 비동기 메서드 "FillLinkedNodes" 참고 (2024.10.30 minjae)
        // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        /// <summary>
        /// 루트 컨테이너(노드) 하위 참조관계에 속한 자식 컨테이너(노드) 추가
        /// </summary>
        // private async Task FillLinkedNodes(LinkedNodeView parentContainer, List<Dictionary<string, object>> pLinkedNodeInfoList, List<Dictionary<string, object>> pLinkedNodeMetaDataList)
        //private async Task FillLinkedNodes(LinkedNodeView parentContainer, List<Dictionary<string, object>> pLinkedNodeInfoList)
        //{
        //    string subContainerId = string.Empty;   // 서브 컨테이너 아이디

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : pLinkedNodeInfoList의 WHERE 조건절 -> TreeView 영역 부모 노드 이름 프로퍼티 " parentContainer.exp_TreeName" 영어 대문자 -> 소문자로 변환(ToLower()) 후
        //        //        node[ContainerHelper.targetId]?.ToString() 문자열(영어 소문자) 값과 비교하여 같은 값만 추출하기 (2023.11.01 minjae)
        //        // 부모 컨테이너(루트 또는 서브)에 하위 참조관계에 속한 자식 컨테이너 데이터 추출 
        //        var childNodes = pLinkedNodeInfoList.Where(node => parentContainer.exp_TreeName.ToLower() == node[ContainerHelper.targetId]?.ToString());

        //        //if (childNodes.Any() == false) return;
        //        if (false == childNodes.Any()) return;

        //        foreach (var childNode in childNodes)
        //        {
        //            // TODO : TreeView 영역 부모 노드 하위 자식 노드(childNode) 이름 프로퍼티 "exp_TreeName" 영어 대문자로 출력 (2023.11.01 minjae)
        //            subContainerId = childNode[ContainerHelper.sourceId].ToString().ToUpper();

        //            // TODO : 참조 컨테이너 "subContainer"에 참조 컨테이너 아이디 "subContainer"에 속하는 메타데이터를 "new LinkedNodeView" 프로퍼티 "LinkedNodeInfoDic"에 할당하기 (2023.10.30 minjae)
        //            // 참조관계에 속한 자식 컨테이너(subContainer) 생성 및 아이디 값 할당 
        //            //var subContainer = new LinkedNodeView
        //            //                       {
        //            //                           exp_TreeName = childNode[ContainerHelper.sourceId].ToString().ToUpper(),
        //            //                           // LinkedNodeInfoDic
        //            //                       };
        //            var subContainer = new LinkedNodeView(childNode[ContainerHelper.sourceId].ToString().ToUpper());

        //            // TODO : 메서드 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //            //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //            //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //            //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        //            // var subContainer = pLinkedNodeMetaDataList.Where(meta => meta[ContainerHelper.containerId].ToString().Equals(subContainerId))
        //            //                                           .Select(meta => new LinkedNodeView 
        //            //                                                               { 
        //            //                                                                   exp_TreeName = subContainerId,
        //            //                                                                   LinkedNodeInfoDic = meta
        //            //                                                               })
        //            //                                           .FirstOrDefault();

        //            // 부모 컨테이너(루트 또는 서브)의 자식 노드로 subContainer 추가 
        //            parentContainer.ChildNodes.Add(subContainer);

        //            // TODO : 비동기 메서드 "FillLinkedNodes"에 속하는 파라미터 메타 데이터 리스트 "pLinkedNodeMetaDataList"의 경우
        //            //        Rest API 메서드 "ExplorerRestServer.GetLinkedNodeInfoListAsync"(Rest API 8. 연관 컨테이너 목록 조회)의 결과값이긴 하나
        //            //        현재 사용 중인 기능이 아님. 추후에 API 사용하거나 혹은 API 기능이 삭제될 시에 인재 INC측으로 부터
        //            //        문서 정리하여 공유 받고 필요시 메서드 파라미터로 구현 예정 (2023.11.01 minjae)
        //            // 서브 컨테이너의 자식 노드로 또 다른 서브 컨테이너 추가 
        //            // await FillLinkedNodes(subContainer, pLinkedNodeInfoList, pLinkedNodeMetaDataList);
        //            await FillLinkedNodes(subContainer, pLinkedNodeInfoList);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
        //        Log.Error(ex.StackTrace);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
        //        Log.Error(ex.ToString());

        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 필요시 연관 컨테이너(참조관계 - 부모/자식 컨테이너) 메타데이터 정보 출력 비동기 메서드 "LinkedNodeInfoSearchAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 연관 컨테이너(참조관계 - 부모/자식 컨테이너) 메타데이터 정보 출력
        /// </summary>
        //private async Task LinkedNodeInfoSearchAsync(object obj)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        //MessageBox.Show("연관 컨테이너(참조관계 - 부모/자식 컨테이너) 메타데이터 정보 출력 기능 필요시 구현 예정");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.StackTrace - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
        //        // Log.Error(e.StackTrace);

        //        // TODO : 오류 메시지 로그로 작성하는 다른 방법(e.ToString() - 오류 발생한 파일 경로 포함) 구현 (2023.10.27 minjae) 
        //        // Log.Error(e.ToString());
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        // throw;
        //    }
        //    return;
        //}

        // TODO : 필요시 검토요청 비동기 메서드 "RequestAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 검토요청
        /// </summary>
        //private async Task RequestAsync(object obj)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // MessageBox.Show("승인 / 검토 요청 필요시 구현 예정");
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}

        // TODO : 필요시 수정 비동기 메서드 "UpdateAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 수정
        /// </summary>
        //private async Task UpdateAsync(object obj)
        //{
        //    // TODO : 아래 주석친 소스 코드 (upContainerDatas) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //    // Object 배열 객체 obj 리스트로 변환
        //    // 참고 URL - https://nonstop-antoine.tistory.com/36
        //    // var upContainerDatas = obj as IEnumerable;

        //    // TODO : 아래 주석친 코드 필요시 참고 (2024.08.26 minjae)
        //    // 컨테이너 아이디 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf 
        //    // var parameters = (object[])obj;                                   // 컨테이너 아이디 버튼으로 부터 받은 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] containerUpdate" 형식으로 형변환 구현 (2024.01.03 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    // string[] containerUpdate = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
        //    // var upContainerDatas    = parameters.ToList();          // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "upContainerDatas"

        //    // string upSystemCode  = string.Empty;              // 수정할 systemCode - typmodel, typreprt
        //    // string upCodeName    = string.Empty;              // 수정할 codeName - 모델 / 보고서

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 아래 foreach 문 필요시 참고 (2023.12.05 minjae)
        //        //foreach (ContainerDetailView data in upContainerDatas)
        //        //{
        //        //    // TODO : 아래 소스코드에 들어갈 데이터가 추가로 필요할 시 소스코드 수정 예정 (2023.10.23 minjae)
        //        //    //        UpdateParam 프로퍼티 필요 없어도 변경된 데이터(data.SelectParam)들이 정상적으로 서버로 넘어갈 수 있으면 UpdateParam 삭제 예정 (2023.10.23 minjae)
        //        //    UpdateParam = new Param();
        //        //    UpdateParam = data.SelectParam;
        //        //}

        //        // var test = DetailDatas.(detailData => detailData.SelectParam).Object;
        //        // UpdateParam  = obj as IEnumerable; // .Select(s => s.SelectParam.FileTypeCodeInfo);

        //        // UpdateAsync 메서드 실행시 ComboBox 파일유형에서 변경된 값을 가져오도록 Linq 식으로 구현하기 (2023.10.23 minjae) 
        //        // var obj      = null;
        //        // FileTypeView fileTypeCodeInfo = ContainerDatas.Select(x => x.Param.FileTypeCodeInfo);

        //        // upSystemCode = fileTypeCodeInfo
        //        // Select(x => x.Param.FileTypeCodeInfo);

        //        //MessageBox.Show("수정 기능 필요시 구현 예정");
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //}

        // TODO : 필요시 파일 다운로드 비동기 메서드 "DownLoadAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 파일 다운로드
        /// </summary>
        //private async Task DownLoadAsync(object obj)
        //{
        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.DownLoad_LoadingWork);   // 다운로드 처리 중 화면 뷰모델 할당

        //    // TODO : 다운로드 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                                      // 다운로드 버튼으로 부터 받은 다운로드 받을 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] downLoadDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] downLoadDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

        //    string fileId = string.Empty;     // 파일 아이디
        //    string fileName = string.Empty;   // 파일명(이름)

        //    string resMsg = string.Empty;                        // 파일 다운로드 처리 결과(성공 또는 실패) 문자열 초기화  

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 파일 다운로드 처리 중인 경우 
        //        if (true == IsDownLoadWorking)
        //            throw new Exception(CobimHelper.DownLoad_LoadingWork);

        //        IsDownLoadWorking = true;

        //        // Http 웹서버 통신 요청 Header 값 가져와서 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
        //        InputParamData.SetData(LoginView.GetRequestHeader(),
        //                                     new ContainerPack(downLoadDatas[(int)EnumDownLoad.PROJECTID],
        //                                                       downLoadDatas[(int)EnumDownLoad.TEAMID],
        //                                                       downLoadDatas[(int)EnumDownLoad.CONTAINERID],
        //                                                       string.Empty,
        //                                                       string.Empty),
        //                                     new FilePack(downLoadDatas[(int)EnumDownLoad.FILEID],
        //                                                  downLoadDatas[(int)EnumDownLoad.FILENAME]));

        //        ContainerPack = new ContainerPack(downLoadDatas[(int)EnumDownLoad.PROJECTID],
        //                                          downLoadDatas[(int)EnumDownLoad.TEAMID],
        //                                          downLoadDatas[(int)EnumDownLoad.CONTAINERID],
        //                                          string.Empty,
        //                                          string.Empty);

        //        FilePack = new FilePack(downLoadDatas[(int)EnumDownLoad.FILEID],
        //                                downLoadDatas[(int)EnumDownLoad.FILENAME]);

        //        loadingWorkVM.ShowWindow();   // 파일 다운로드 처리 중 화면 출력

        //        // 파일 다운로드 진행
        //        await ExplorerRestServer.GetFileDownLoadAsync(Client, ContainerPack, FilePack, LoginView.RequestHeaders);

        //        resMsg = "success";  // 파일 다운로드 성공  
        //        MessageBox.Show("파일 다운로드 완료!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //        resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
        //    }
        //    finally
        //    {
        //        // 파일 다운로드 처리 중이 아닌 경우 
        //        if (resMsg != CobimHelper.DownLoad_LoadingWork)
        //        {
        //            // TODO : 파일 다운로드 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
        //            // 참고 URL - https://cosmosproject.tistory.com/563
        //            IsDownLoadWorking = false;   // 파일 다운로드 처리 중 종료
        //            loadingWorkVM.RequestClose(null);   // 파일 다운로드 처리 중 화면 종료
        //        }
        //    }
        //    return;
        //}

        // TODO : 필요시 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드 비동기 메서드 "CheckOutAsync" 참고 (2024.10.30 minjae)
        /// <summary>
        /// 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드  
        /// </summary>
        //private async Task CheckOutAsync(object obj)
        //{
        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CheckOut_LoadingWork);   // 체크아웃 처리 중 화면 뷰모델 할당

        //    string checkOutContainerId = string.Empty;   // 체크아웃 요청 컨테이너 아이디

        //    // TODO : Context Menu(컨텍스트 메뉴) "체크아웃" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;   // Context Menu(컨텍스트 메뉴) "체크아웃" 클릭시 받은 체크아웃 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId") (Object Array -> IEnumerable 형변환)

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] checkOutDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] checkOutDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

        //    string resMsg = string.Empty;   // 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드 처리 결과(성공 또는 실패) 문자열 초기화

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 연관 컨테이너 파일 다운로드 처리 중인 상태에서 다른 컨테이너의 체크아웃을 중복 요청하는 경우 
        //        if (true == IsLinkedContainerWorking)
        //            throw new Exception(CobimHelper.LinkedContainer_LoadingWork);

        //        // 체크아웃 처리 중인 경우 
        //        if (true == IsCheckOutWorking)
        //            throw new Exception(CobimHelper.CheckOut_LoadingWork);

        //        IsCheckOutWorking = true;   // 체크아웃 처리 중 시작

        //        // 1 단계 : 비동기 메서드 "CheckOutAsync"에 파라미터로 받은 전달 인자(object 배열 객체)로 "checkOutDatas" 리스트에 값 할당 
        //        //          체크아웃 요청 컨테이너 아이디 "checkOutContainerId", "folderId", "ContainerPack", "FilePack" 생성
        //        // 체크아웃 요청 컨테이너 아이디 문자열 값 string 클래스 객체 checkOutContainerId에 할당 
        //        checkOutContainerId = checkOutDatas[(int)EnumCheckOut.CONTAINERID];

        //        // Http 웹서버 통신 요청 Header, 컨테이너 정보(ContainerPack), 파일 정보(FilePack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
        //        InputParamData.SetData(LoginView.GetRequestHeader(),
        //                               new ContainerPack(checkOutDatas[(int)EnumCheckOut.PROJECTID],
        //                                                 checkOutDatas[(int)EnumCheckOut.TEAMID],
        //                                                 checkOutDatas[(int)EnumCheckOut.CONTAINERID],
        //                                                 string.Empty,
        //                                                 string.Empty),
        //                               new FilePack(checkOutDatas[(int)EnumCheckOut.FILEID],
        //                                            checkOutDatas[(int)EnumCheckOut.FILENAME]));

        //        ContainerPack = new ContainerPack(checkOutDatas[(int)EnumCheckOut.PROJECTID],
        //                                          checkOutDatas[(int)EnumCheckOut.TEAMID],
        //                                          checkOutDatas[(int)EnumCheckOut.CONTAINERID],
        //                                          string.Empty,
        //                                          string.Empty);

        //        FilePack = new FilePack(checkOutDatas[(int)EnumCheckOut.FILEID],
        //                                checkOutDatas[(int)EnumCheckOut.FILENAME]);

        //        loadingWorkVM.ShowWindow();

        //        // 2 단계 : Rest API 10. 컨테이너 체크아웃 + 다운로드 호출 
        //        // 체크아웃 호출을 위해 만족 해야할 사항(조건)
        //        // 1) 컨테이너 생성자 == 체크아웃 요청자(로그인 사용자 아이디)
        //        // 2) 컨테이너 readYn 필드 값이 'N' 일 때
        //        // 3) 검토 요청 중인 컨테이너가 아닐 때(statusCode != 'comstt20')
        //        await ExplorerRestServer.GetCheckOutAsync(Client, ContainerPack, FilePack, LoginView.RequestHeaders);

        //        // TODO : 체크아웃 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
        //        // 참고 URL - https://cosmosproject.tistory.com/563
        //        // 체크아웃 처리 중 화면 종료 
        //        // IsCheckOutWorking = false;
        //        loadingWorkVM.RequestClose(null);


        //        // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
        //        //        체크아웃 비동기 메서드 "CheckOutAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //        //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //        //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 파라미터를 프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae) 
        //        // 3 단계: 체크아웃 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
        //        if (false == string.IsNullOrWhiteSpace(SelectedFolderId))
        //            await ContainerListCreateAsync(ContainerPack, SelectedFolderId);
        //        // var containerList = await ExplorerRestServer.GetContainerListAsync(client, ProjectPack, folderId, tokenKey);
        //        else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");


        //        // 4 단계: 체크아웃 요청한 컨테이너 아이디와 연관된 컨테이너 목록 조회
        //        //         (Rest API 8.연관 컨테이너 목록 조회 호출) 
        //        var linkedContainerList = await ExplorerRestServer.GetLinkedContainerListAsync(Client, ContainerPack, LoginView.RequestHeaders);


        //        // 연관 컨테이너 목록이 존재하지 않는 경우 
        //        // if(true == linkedContainerList.Count.Equals((int)EnumLinkedRelation.NONE))
        //        if (linkedContainerList.Count == (int)EnumLinkedRelation.NONE)
        //            MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 목록이 존재하지 않습니다.");

        //        // TODO : 아래 주석친 테스트 코드 (연관 컨테이너가 존재하는 경우) 코드 필요시 참고 (2024.01.03 minjae)
        //        // TODO : 아래 연관 컨테이너가 존재하는 경우 리스트 객체 "linkedContainerList" 컨테이너 아이디(containerId) 오름차순 정렬 (Sort 메서드) (2023.11.29 minjae)
        //        // 참고 URL - https://www.techiedelight.com/ko/sort-list-in-descending-order-in-csharp/
        //        // 연관 컨테이너가 존재하는 경우 
        //        // if (linkedContainerList.Count >= (int)EnumLinkedRelation.EXISTENCE)
        //        //     linkedContainerList.Sort((containerA, containerB) => containerA[ContainerHelper.containerId].ToString().CompareTo(containerB[ContainerHelper.containerId].ToString()));

        //        // 연관 컨테이너 목록이 존재하는 경우 
        //        // && 연관 컨테이너 파일 다운로드 처리 중 화면 출력 
        //        //if(true == linkedContainerList.Count >= (int)EnumLinkedRelation.EXISTENCE && false == IsLinkedContainerWorking)
        //        if (linkedContainerList.Count >= (int)EnumLinkedRelation.EXISTENCE)
        //        {
        //            // MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 다운로드 시작합니다.");

        //            IsLinkedContainerWorking = true;   // 연관 컨테이너 파일 다운로드 처리 중 시작

        //            // 연관 컨테이너 파일 다운로드 처리 중 화면 뷰모델 할당
        //            loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.LinkedContainer_LoadingWork);

        //            // 연관 컨테이너 파일 다운로드 처리 중 화면 출력   
        //            loadingWorkVM.ShowWindow();

        //            foreach (var linkedContainer in linkedContainerList)
        //            {
        //                // TODO : 다운로드할 파일 정보 프로퍼티 "FilePack"에 속한 "fileId", "fileName"에 문자열 재할당 (2023.11.30 minjae)
        //                // 파일 아이디
        //                FilePack.fileId = linkedContainer[ContainerHelper.fileId].ToString();

        //                // 파일명(이름)
        //                FilePack.fileName = linkedContainer[ContainerHelper.fileNm].ToString();

        //                // TODO : 연관 컨테이너 다운로드 하는 경우 다운로드 받은 파일의 상위 폴더명(컨테이너 아이디)을
        //                //        각각의 연관 컨테이너 아이디가 아니라 루트 컨테이너 아이디(체크아웃 요청한 컨테이너 아이디)
        //                //        하나로만 상위 폴더명으로 설정 해야 하므로 아래 소스코드는 주석 처리 진행 (2023.12.14 minjae)
        //                // 체크아웃 요청한 컨테이너 아이디 
        //                // ContainerPack.containerId = linkedContainer[ContainerHelper.containerId].ToString();

        //                // 연관 컨테이너 아이디
        //                ContainerPack.linkedContainerId = linkedContainer[ContainerHelper.containerId].ToString();

        //                string linkedContainerId = ContainerPack.linkedContainerId;

        //                // TODO : 체크아웃 요청한 컨테이너 아이디(checkOutContainerId) 문자열 값과 연관 컨테이너 아이디(ContainerPack.linkedContainerId)의 문자열이 동일한 경우 파일 다운로드 대상 제외
        //                //        이미 체크아웃 + 파일 다운로드 처리 동시에 하였으므로 파일 중복 다운로드 안 하기 위해 continue 처리 구현 (2023.12.15 minjae)
        //                // if(true == ContainerPack.containerId.Equals(ContainerPack.linkedContainerId)) continue;
        //                //if(checkOutContainerId == (ContainerPack.linkedContainerId)) continue;
        //                if (checkOutContainerId == linkedContainerId) continue;

        //                // 연관 컨테이너에 속한 파일 다운로드 진행
        //                else await ExplorerRestServer.GetFileDownLoadAsync(Client, ContainerPack, FilePack, LoginView.RequestHeaders);
        //            }

        //            MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 파일 다운로드 완료!");
        //        }

        //        resMsg = "success";   // 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드 성공

        //        // TODO : 연관 컨테이너 파일 다운로드 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.05 minjae)
        //        // 참고 URL - https://cosmosproject.tistory.com/563
        //        // 연관 컨테이너 파일 다운로드 처리 중 화면 종료 
        //        // IsLinkedContainerWorking = false;

        //        // 체크아웃 처리 중 또는 연관 컨테이너 파일 다운로드 처리 완료
        //        // IsCheckOutWorking = false;
        //        // IsLinkedContainerWorking = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

        //        resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
        //    }
        //    finally
        //    {
        //        // 컨테이너 체크아웃 또는 (참조/연관) 관계 컨테이너 파일 다운로드 처리 중이 아닌 경우
        //        //if(true == IsCheckOutWorking || true == IsLinkedContainerWorking)
        //        if (resMsg != CobimHelper.CheckOut_LoadingWork && resMsg != CobimHelper.LinkedContainer_LoadingWork)
        //        {
        //            IsCheckOutWorking = false;
        //            IsLinkedContainerWorking = false;
        //            loadingWorkVM.RequestClose(null);   // 체크아웃 처리 중 또는 연관 컨테이너 파일 다운로드 처리 중 화면 종료 
        //        }
        //    }
        //    return;
        //}

        // TODO : 필요시 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 비동기 메서드 "CheckInAsync" 참고 (2024.10.31 minjae)
        /// <summary>
        /// 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드  
        /// 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 성공하면 
        /// HubBox 파일 변환 요청 Rest API 메서드 HubBoxRestServer.PostTranslateAsync 호출 해야함.
        /// </summary>
        //private async Task CheckInAsync(object obj)
        //{
        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CheckIn_LoadingWork);   // 체크인 처리 중 화면 뷰모델 할당

        //    CheckInPartVM checkInPartVM = null;          // 체크인 팝업 화면 객체 초기화

        //    string resultTranslateCode = string.Empty;   // 특정 컨테이너에 속한 파일 변환 처리 결과 상태 코드 문자열 초기화
        //    // string resultTranslateMsg = string.Empty;    // 특정 컨테이너에 속한 파일 변환 처리 관련 공통 메세지 초기화

        //    // TODO : Context Menu(컨텍스트 메뉴) "체크인" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;              // Context Menu(컨텍스트 메뉴) "체크인" 클릭시 받은 체크인 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId", "exp_FileName") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] checkInDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열
        //    //        단, 변경할 때, (기존) object 배열에 속하는 데이터 중 "제목"을
        //    //        갖고 있는 데이터를 제외하기 위해 Linq 조건절 확장 메서드 Where 사용해서
        //    //        특정 인덱스 "(int)EnumCheckIn.TITLE" 제외한 
        //    //        데이터만 추출해서 string 배열 객체 "checkInDatas" 구현 (2024.08.26 minjae)
        //    // 참고 URL - https://chatgpt.com/c/332ba391-669d-4e7b-8e5a-af86cbd6516e
        //    string[] checkInDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString())
        //                                 .Where((checkInRequestData, index) => index != (int)EnumCheckIn.TITLE)
        //                                 .Select(checkInRequestData => checkInRequestData)
        //                                 .ToArray();

        //    string[] translateDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());   // HubBox 파일 변환 요청 Rest API 요청 데이터 string 배열 객체 "translateDatas"

        //    // string checkInContainerId = string.Empty;         // 체크인 요청 컨테이너 아이디            
        //    // string statusCode = string.Empty;                 // 컨테이너 검토상태("comstt10": 미진행, "comstt20": 진행, "comstt30": 완료)
        //    // string readYn     = string.Empty;                 // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
        //    // string workUserId = string.Empty;                 // 컨테이너 작업중인 사용자 아이디

        //    string fileName = string.Empty;                   // 체크인 처리할 컨테이너에 속한 파일명

        //    string fileId = string.Empty;                      // 체크인 처리하기 전 파일 아이디  
        //    string revsnVal = string.Empty;                      // 체크인 처리하기 전 파일 버전 

        //    string checkInFileId = string.Empty;                 // 체크인 처리 완료 후 파일 아이디  
        //    string checkInRevsnVal = string.Empty;               // 체크인 처리 완료 후 파일 버전 

        //    // TODO : C# 문자열 보간 기능 사용해서 웹서버로 업로드 할 상위 폴더 경로 문자열로 변환 (2024.09.02 minjae)
        //    // 참고 URL - https://gaeunhan.tistory.com/61
        //    string dirPath = string.Empty;                       // 웹서버로 업로드 할 파일의 상위 디렉토리(폴더) 경로
        //    string filePath = string.Empty;                       // 웹서버로 업로드 할 파일 전체 경로 

        //    string deleteContainerId = string.Empty;             // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 컨테이너 아이디
        //    string deleteDirPath = string.Empty;                 // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 디렉토리(폴더) 경로
        //    string deleteFilePath = string.Empty;                // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일 경로

        //    string resMsg = string.Empty;                        // 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 처리 결과(성공 또는 실패) 문자열 초기화

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 파일 변환 처리 중인 상태에서 다른 컨테이너의 체크인을 중복 요청하는 경우
        //        if (true == IsTranslateWorking)
        //            throw new Exception(CobimHelper.Translate_LoadingWork);

        //        // 체크인 처리 중인 경우 
        //        if (true == IsCheckInWorking)
        //            throw new Exception(CobimHelper.CheckIn_LoadingWork);

        //        IsCheckInWorking = true;   // 체크인 처리 중 시작 
        //        checkInPartVM = Container.Get<CheckInPartVM>().ToShowData();   // 체크인 팝업 화면 생성 및 결과 리턴

        //        // TODO : await Task.Yield(); 사용법 제대로 확인 후 사용 예정 (2023.12.04 minjae)
        //        // 참고 URL - https://www.sysnet.pe.kr/2/0/12241
        //        // 참고 2 URL - https://www.sysnet.pe.kr/2/0/12245
        //        // 참고 3 URL - https://stackoverflow.com/questions/22645024/when-would-i-use-task-yield
        //        await Task.Yield();

        //        // TODO : 닫기("X") 버튼 클릭 시 컨테이너 체크인 + 업로드 메서드 "CheckInAsync" 종료 로직 구현 (2023.12.07 minjae)
        //        if (true == checkInPartVM.IsExit) return;

        //        ContainerPack = new ContainerPack(checkInDatas[(int)EnumCheckIn.PROJECTID],
        //                                          checkInDatas[(int)EnumCheckIn.TEAMID],
        //                                          checkInDatas[(int)EnumCheckIn.CONTAINERID],
        //                                          string.Empty,
        //                                          string.Empty);

        //        // Http 웹서버 통신 요청 Header 값 가져와서 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
        //        InputParamData.SetData(LoginView.GetRequestHeader(),
        //                               new ContainerPack(checkInDatas[(int)EnumCheckIn.PROJECTID],
        //                                                 checkInDatas[(int)EnumCheckIn.TEAMID],
        //                                                 checkInDatas[(int)EnumCheckIn.CONTAINERID],
        //                                                 string.Empty,
        //                                                 string.Empty));

        //        // TODO : 다른 PC에서도 파일 업로드 기능이 실행될 수 있도록 웹서버로 업로드 할 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.12.05 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //        // 웹서버로 업로드 할 파일의 상위 디렉토리 폴더 전체 경로 예시 - "C:\Users\bhjeon\Documents\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002"
        //        // SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 사용
        //        // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.10.14 minjae)
        //        // dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}" +
        //        //            $"\\{Assembly.GetEntryAssembly().GetName().Name}" +
        //        //            $"\\{ContainerPack.projectId}" +
        //        //            $"\\{ContainerPack.teamId}" +
        //        //            $"\\{ContainerPack.containerId}";

        //        // TODO : StringBuilder 클래스 객체 sbDirPath 생성 -> string 클래스 객체 dirPath에 문자열 추가 (2024.10.15 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
        //        StringBuilder sbDirPath = new StringBuilder($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
        //        sbDirPath.Append($"\\{Assembly.GetEntryAssembly().GetName().Name}");
        //        sbDirPath.Append($"\\{ContainerPack.projectId}");
        //        sbDirPath.Append($"\\{ContainerPack.teamId}");
        //        sbDirPath.Append($"\\{ContainerPack.containerId}");

        //        dirPath = sbDirPath.ToString();

        //        // 체크인 처리 하기전 파일 이름, 파일 아이디, 파일 버전 문자열 할당 
        //        fileName = checkInDatas[(int)EnumCheckIn.UPLOAD_FILENAME];
        //        fileId = checkInDatas[(int)EnumCheckIn.FILEID];
        //        revsnVal = checkInDatas[(int)EnumCheckIn.REVSNVAL];

        //        // TODO : 웹서버로 업로드 할 파일 전체 경로 구현 (2023.12.04 minjae)
        //        // 참고 URL - https://dlgnlfus.tistory.com/129
        //        filePath = dirPath + $"\\{fileName}";   // 웹서버로 업로드 할 파일 전체 경로 

        //        // TODO : 웹서버로 업로드 할 파일 전체 경로에 있는 파일 정보 불러 오도록 구현 (2023.12.05 minjae)
        //        // 참고 URL   - https://chashtag.tistory.com/37
        //        // 참고 2 URL - https://www.delftstack.com/ko/howto/csharp/get-filesize-in-csharp/
        //        // 참고 3 URL - https://dlgnlfus.tistory.com/129
        //        if (true == File.Exists(filePath))
        //        {
        //            // Rest API 컨테이너 체크인 요청하기 전 웹서버로 업로드 할 파일 읽어오기(File.OpenRead)
        //            using (FileStream upLoadFileStream = File.OpenRead(filePath))
        //            {
        //                // TODO : 추후 체크인 구현시 업로드 하려는 파일명(uploadFiles)과 기존 체크인 된 파일명(fileName)이 다를 경우
        //                //        enum 열거형 구조체 EnumCheckIn 멤버 변수 "FILENAME" 사용해서 로직 수정 예정 (2023.12.05 minjae)
        //                CheckInPack = new CheckInPack
        //                {
        //                    folderId = checkInDatas[(int)EnumCheckIn.FOLDERID],
        //                    uploadFiles = upLoadFileStream,
        //                    fileName = checkInDatas[(int)EnumCheckIn.UPLOAD_FILENAME],
        //                    // TODO : 필요시 파일 아이디 프로퍼티 "fileId" 사용 예정 (2024.10.14 minjae)
        //                    // fileId = string.Empty,
        //                    // TODO : pCheckInPack.fileSize의 파일 크기 단위가 바이트인지 아닌지 확인 필요 (2023.12.05 minjae)
        //                    fileSize = upLoadFileStream.Length.ToString(),
        //                    comment = checkInPartVM.InputParamData.exp_Comment
        //                };

        //                // 체크인 처리 중 화면 출력 
        //                loadingWorkVM.ShowWindow();

        //                // 컨테이너 체크인 + 업로드 
        //                // var resultCheckInData = await ExplorerRestServer.PostCheckInAsync(Client, ContainerPack, CheckInPack, filePath, LoginView.RequestHeaders);
        //                var resultCheckInData = await ExplorerRestServer.PostCheckInAsync(Client, ContainerPack, CheckInPack, LoginView.RequestHeaders);

        //                // Dictionary 객체 resultCheckInData 특정 키(Key) "fileName", "fileId", "revsnVal"에 속하는 값(Value) 문자열로 변환 (2024.08.26 minjae)
        //                // 참고 URL - https://developer-talk.tistory.com/694
        //                // TODO : 체크인 처리 완료 후 파일명 string 객체 checkInFileName 필요시 사용 예정 (2024.08.26 minjae)
        //                // checkInFileName = resultCheckInData[ContainerHelper.fileNm].ToString();
        //                checkInFileId = resultCheckInData[ContainerHelper.fileId].ToString();
        //                checkInRevsnVal = resultCheckInData[ContainerHelper.revsnVal].ToString();

        //                // TODO : 체크인 처리중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
        //                // 참고 URL - https://cosmosproject.tistory.com/563
        //                // 체크인 처리 중 화면 종료 
        //                loadingWorkVM.RequestClose(null);

        //            }   // Rest API 컨테이너 체크인 요청 성공후 웹서버로 업로드 할 파일 리소스 해제(Dispose()) 처리 -> 리소스 해제(Dispose()) 해야 사용자 로컬 PC에 설치된 파일 삭제 처리 가능

        //            // 컨테이너 체크인 + 업로드 성공한 경우 
        //            // && 파일 변환 처리 중 화면 출력 
        //            // checkInFileId(체크인 처리 완료 후 파일 아이디) != fileId(체크인 처리하기 전 파일 아이디) 
        //            // checkInRevsnVal(체크인 처리 완료 후 파일 버전) != revsnVal(체크인 처리하기 전 파일 버전)
        //            // if(checkInFileId != fileId && checkInRevsnVal != revsnVal && false == IsTranslateWorking)
        //            if (checkInFileId != fileId && checkInRevsnVal != revsnVal)
        //            {
        //                IsTranslateWorking = true;   // 파일 변환 처리 시작

        //                // 체크인 완료된 이후 변경된 파일 아이디 값(checkInFileId)을 파일 변환 요청 Dictionary(translateDatas[(int)EnumTranslate.FILEID])에 할당
        //                translateDatas[(int)EnumTranslate.FILEID] = checkInFileId;

        //                TranslatePack = new TranslatePack(translateDatas[(int)EnumTranslate.PROJECTID],
        //                                                  translateDatas[(int)EnumTranslate.TEAMID],
        //                                                  translateDatas[(int)EnumTranslate.CONTAINERID],
        //                                                  translateDatas[(int)EnumTranslate.FILEID]);

        //                // checkInContainerId = translateDatas[(int)EnumTranslate.CONTAINERID];
        //                // MessageBox.Show($"컨테이너 아이디 {checkInContainerId}\r\n체크인 완료!\r\n파일 변환 요청 시작합니다.");

        //                // 연관 컨테이너 파일 다운로드 처리 중 화면 뷰모델 할당
        //                loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.Translate_LoadingWork);

        //                // 파일 변환 처리 중 화면 출력 
        //                loadingWorkVM.ShowWindow();

        //                // HubBox Rest API 3. 컨테이너에 속한 특정 파일 변환 API 호출 
        //                var resultTranslateData = await HubBoxRestServer.PostTranslateAsync(Client, TranslatePack, LoginView.RequestHeaders);

        //                // TODO : Dictionary 객체 resultTranslateData에 담긴 파일 변환 API 결과 값 중 파일 변환 성공/처리 중일 경우 키 "code"에 매핑된 값으로 할당
        //                //        만약 파일 변환 실패했을 경우 키 "status"에 매핑된 값으로 할당 하도록 삼항연산자 사용하여 로직 구현하기 (2024.08.26) 
        //                // bool result =  resultTranslateData.TryGetValue(resultTranslateData[EnumTranslateResult.CODE.ToDescription()], out resultTranslateCode))
        //                // resultTranslateCode = true == resultTranslateData.ContainsKey(EnumTranslateResult.CODE.ToDescription()) ? 
        //                //                         resultTranslateData[EnumTranslateResult.CODE.ToDescription()].ToString() : 
        //                //                       (true == resultTranslateData.ContainsKey(EnumTranslateResult.STATUS.ToDescription()) ? resultTranslateData[EnumTranslateResult.STATUS.ToDescription()].ToString() : null);

        //                if (true == resultTranslateData.ContainsKey(EnumTranslateResult.CODE.ToDescription()))
        //                    resultTranslateCode = resultTranslateData[EnumTranslateResult.CODE.ToDescription()].ToString();

        //                else if (true == resultTranslateData.ContainsKey(EnumTranslateResult.STATUS.ToDescription()))
        //                    resultTranslateCode = resultTranslateData[EnumTranslateResult.STATUS.ToDescription()].ToString();

        //                else resultTranslateCode = null;

        //                // TryGetValue vs ContainsKey 차이
        //                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.dictionary-2.trygetvalue?view=net-8.0
        //                // 참고 2 URL - https://stackoverflow.com/questions/38377318/contains-in-dictionary-in-c-sharp
        //                // 참고 3 URL - https://ckhyeok.tistory.com/37
        //                // 참고 4 URL - https://m.blog.naver.com/PostView.nhn?blogId=crusaderholy&logNo=100112263890&proxyReferer=https:%2F%2Fwww.google.com%2F
        //                // 참고 5 URL - https://coding-shop.tistory.com/53
        //                // 참고 6 URL - https://developer-talk.tistory.com/686

        //                // 컨테이너에 속한 특정 파일 변환 Dictionary 객체 resultTranslateData 에
        //                // 키값 "code", "status" 둘 다 포함되지 않은 경우 
        //                // if(false == resultTranslateData.TryGetValue(EnumTranslateResult.CODE.ToDescription(), out resultTranslateCode)
        //                //    && false == resultTranslateData.TryGetValue(EnumTranslateResult.STATUS.ToDescription(), out resultTranslateCode)) 
        //                //      resultTranslateCode = null;

        //                //resultTranslateMsg =  $"* 파일 변환 안내 *"
        //                //                    + $"\r\n\r\n처리 결과 - {resultTranslateData[EnumTranslateResult.MESSAGE.ToDescription()]} ({resultTranslateCode})"
        //                //                    + $"\r\n컨테이너 아이디  - {translateDatas[(int)EnumTranslate.CONTAINERID].ToUpper()}"
        //                //                    + $"\r\n제목 - {translateDatas[(int)EnumTranslate.TITLE]}"
        //                //                    + $"\r\n파일 이름 - {translateDatas[(int)EnumTranslate.TRANSLATE_FILENAME]}";

        //                //MessageBox.Show(resultTranslateMsg);   // 파일 변환 처리 결과 메세지 출력 
        //            }
        //            // 컨테이너 체크인 + 업로드 실패한 경우
        //            else throw new Exception("체크인 오류 발생!\r\n담당자에게 문의하세요.");   // 체크인 오류 사유 메시지 출력 

        //            // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크인" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
        //            //        체크인 비동기 메서드 "CheckInAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //            //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //            //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 파라미터를 프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae)
        //            // 체크인 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
        //            if (false == string.IsNullOrWhiteSpace(SelectedFolderId))
        //                await ContainerListCreateAsync(ContainerPack, SelectedFolderId);
        //            else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");

        //            //MessageBox.Show("체크인 완료!");

        //            // TODO : 체크인 완료(+ 컨테이너에 속한 파일 웹서버로 업로드) 후 파일 삭제 기능이 실행될 수 있도록 삭제 할 파일의 상위 폴더 경로 "deleteDirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2024.08.30 minjae)
        //            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //            // 삭제할 파일의 상위 폴더 전체 경로 예시 - "C:\Users\bhjeon\Documents\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002"
        //            // SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 사용
        //            deleteDirPath = dirPath;

        //            // 삭제할 파일의 상위 컨테이터 폴더(ContainerPack.containerId)가 존재하는 경우 
        //            if (true == Directory.Exists(deleteDirPath))
        //            {
        //                deleteContainerId = checkInDatas[(int)EnumCheckIn.CONTAINERID];
        //                //MessageBox.Show($"로컬에 존재하는 루트 컨테이너 폴더({deleteContainerId})\r\n다운로드 받은 파일 삭제 진행합니다.");

        //                // TODO : 체크인 완료 후(+ 파일 웹서버 업로드 완료) 로컬 PC에 다운로드 받은 기존 루트 컨테이너 폴더(deleteContainerId) 및 폴더에 속한 모든 파일 삭제 
        //                // TODO : 폴더(디렉토리) 클래스 "Directory", "DirectoryInfo" 차이점 비교 후 static 클래스 "Directory" 사용 (2024.08.30 minjae)
        //                // 참고 URL - https://nomad-programmer.tistory.com/207
        //                // TODO : Directory.Delete 메서드의 2번째 파라미터 값 설정 구현 (2024.08.30 minjae)
        //                //        deleteDirPath 경로의 폴더(디렉터리), 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거하려면 true, 제거하지 않으려면 false 설정
        //                //        true로 설정하면 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거할 때, 재귀적으로 (recursive) Directory.Delete 메서드 실행 
        //                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.directory.delete?view=net-8.0
        //                Directory.Delete(deleteDirPath, true);

        //                // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 완료한 경우 
        //                if (false == Directory.Exists(deleteDirPath))
        //                    MessageBox.Show($"체크인 완료!\r\n루트 컨테이너 폴더 ({deleteContainerId})\r\n다운로드 받은 파일 삭제 완료");
        //                // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 실패한 경우 
        //                else throw new Exception($"[오류] 루트 컨테이너 폴더 ({deleteContainerId})\r\n삭제 오류\r\n담당자에게 문의하세요.");
        //            }
        //            resMsg = "success";   // 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 성공
        //        }
        //        else throw new Exception("[오류] 컨테이너 체크인 + 업로드 파일이 존재하지 않습니다.\r\n담당자에게 문의하세요.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //        resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
        //    }
        //    finally
        //    {
        //        // 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 처리 중이 아닌 경우
        //        // && 파일 변환 처리 중이 아닌 경우
        //        //if (true == IsCheckInWorking || true == IsTranslateWorking)
        //        if (resMsg != CobimHelper.CheckIn_LoadingWork && resMsg != CobimHelper.Translate_LoadingWork)
        //        {
        //            // 체크인 처리 중 또는 파일 변환 처리 완료
        //            IsCheckInWorking = false;
        //            IsTranslateWorking = false;
        //            loadingWorkVM.RequestClose(null);   // 체크인 처리 중 또는 파일 변환 처리 중 화면 종료
        //        }
        //    }
        //    return;
        //}

        // TODO : 필요시 컨테이너 작업 취소 비동기 메서드 "CancelWorkAsync" 참고 (2024.10.31 minjae)
        /// <summary>
        /// 컨테이너 작업 취소
        /// 체크아웃 상태의 컨테이너 ID 락 해제 (ReadYn = "Y" -> "N")
        /// 작업자 이름(exp_WorkUserId) 공백(null) 처리
        /// </summary>
        //private async Task CancelWorkAsync(object obj)
        //{
        //    // string tokenKey      = AppSetting.Default.Login.Token.resultData;   // 로그인 토큰 키 
        //    // string inputTenantId = LoginHelper.inputTenantId;                   // 테넌트 아이디

        //    string cancelWorkContainerId = string.Empty;                        // 컨테이너 작업 취소 요청 루트 컨테이너 아이디

        //    // TODO : Context Menu(컨텍스트 메뉴) "작업취소" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.19 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    // var parameters = (object[])obj;
        //    object[] parameters = (object[])obj;

        //    // TODO : Context Menu(컨텍스트 메뉴) "작업취소" 클릭시 MultiCommandParameter 데이터(obj)를
        //    //        (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] cancelWorkDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] cancelWorkDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());
        //    // var cancelWorkDatas = parameters.ToList();                       // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "cancelWorkDatas"

        //    // TODO : C# 문자열 보간 기능 사용해서 삭제 처리 할 파일 + 상위 디렉토리(폴더) 경로 문자열로 변환 (2023.12.20 minjae)
        //    // 참고 URL - https://gaeunhan.tistory.com/61
        //    string dirPath = string.Empty;                                      // 삭제 처리 할 파일 + 상위 디렉토리(폴더) 경로

        //    // bool isExist = false;                                            // 삭제 처리 할 파일 + 상위 디렉토리(폴더) 존재 여부 (존재 - true / 존재안함 - false)

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 컨테이너 작업 취소 요청한 컨테이너 아이디 값(cancelWorkContainerId)이
        //        //        현재 컨테이너 체크아웃, 체크인, 파일열기 중인 컨테이너 값과 동일한 경우 컨테이너 작업 취소 중지할 수 있도록 기능 구현 예정 (2024.10.11 minjae)
        //        // (예) if(true == cancelWorkContainerId.Equals(InputParamData.GetContainerPack().GetContainerId())) 
        //        //      {
        //        //          MessageBox.Show("체크아웃/체크인/파일열기 작업 중인 해당 컨테이너는 작업 취소 요청 불가합니다.
        //        //                           \r\n컨테이너 체크아웃, 체크인, 파일열기 작업 열기 완료 후 다시 진행해주세요.");
        //        //          return;
        //        //      }

        //        _ContainerPack = new ContainerPack(cancelWorkDatas[(int)EnumCancelWork.PROJECTID],
        //                                           cancelWorkDatas[(int)EnumCancelWork.TEAMID],
        //                                           cancelWorkDatas[(int)EnumCancelWork.CANCELWORK_CONTAINERID],
        //                                           string.Empty,
        //                                           string.Empty);

        //        // Http 웹서버 통신 요청 Header,  컨테이너 정보(ContainerPack) 탐색기 데이터 모델 객체 InputParamData에 값 셋팅
        //        InputParamData.SetData(LoginView.GetRequestHeader(),
        //                               new ContainerPack(cancelWorkDatas[(int)EnumCancelWork.PROJECTID],
        //                                                 cancelWorkDatas[(int)EnumCancelWork.TEAMID],
        //                                                 cancelWorkDatas[(int)EnumCancelWork.CANCELWORK_CONTAINERID],
        //                                                 string.Empty,
        //                                                 string.Empty));



        //        // 1 단계 : Rest API 17. 컨테이너 작업 취소 호출 
        //        var resultData = await ExplorerRestServer.PostCancelWorkAsync(Client, ContainerPack, LoginView.RequestHeaders);

        //        // TODO : 컨테이너 작업 취소 결과 확인 로직 오류 발생시 수정 예정 (2023.12.21 minjae)
        //        // 2 단계 : 컨테이너 작업 취소 결과 확인
        //        // 조건 1 - resultData 값이 null 이 아님
        //        // 조건 2 - resultData[ContainerHelper.workUserId] 값이 null임.
        //        // 조건 3 - resultData[ContainerHelper.readYn] 문자열 변환시 'N'과 같아야 함.
        //        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.12.20 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        bool bCancelWork = (resultData is not null)
        //                           && (resultData[ContainerHelper.workUserId] is null)
        //                           && (resultData[ContainerHelper.readYn].ToString().Equals(ContainerHelper.no));

        //        // 컨테이너 작업 취소가 성공한 경우 
        //        if (true == bCancelWork)
        //        {
        //            // 3 단계 : 컨테이너 작업 취소 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
        //            // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "작업취소" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
        //            //        컨테이너 작업 취소 비동기 메서드 "CancelWorkAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //            //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //            //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 파라미터를 프로퍼티 "SelectedFolderId"로 전달함. (2023.12.20 minjae)
        //            if (false == string.IsNullOrWhiteSpace(SelectedFolderId))
        //                await ContainerListCreateAsync(ContainerPack, SelectedFolderId);
        //            else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");

        //            // MessageBox.Show("컨테이너 작업 취소 완료!");

        //            // TODO : 다른 PC에서도 루트 컨테이너 폴더 및 폴더에 속한 모든 파일 삭제 기능이 실행될 수 있도록 업로드 할 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.12.20 minjae)
        //            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //            // 파일 업로드 전체 경로 예시 - "C:\Users\bhjeon\Documents\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002" - (SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) 사용)
        //            dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Assembly.GetEntryAssembly().GetName().Name}\\{ContainerPack.projectId}\\{ContainerPack.teamId}\\{ContainerPack.containerId}";


        //            // 삭제 처리 할 파일 + 상위 디렉토리(폴더)가 존재하는 경우 
        //            // if(true == isExist)
        //            if (true == Directory.Exists(dirPath))
        //            {
        //                cancelWorkContainerId = cancelWorkDatas[(int)EnumCancelWork.CANCELWORK_CONTAINERID];

        //                //MessageBox.Show($"로컬에 존재하는 루트 컨테이너 폴더({pCancelWorkContainerId})\r\n다운로드 받은 파일 삭제 진행합니다.");

        //                // 4 단계 : 컨테이너 작업 취소 완료 후 로컬 PC에 다운로드 받은 기존 루트 컨테이너 폴더(cancelWorkContainerId) 및 폴더에 속한 모든 파일 삭제 
        //                // TODO : 폴더(디렉토리) 클래스 "Directory", "DirectoryInfo" 차이점 비교 후 static 클래스 "Directory" 사용 (2023.12.20 minjae)
        //                // 참고 URL - https://nomad-programmer.tistory.com/207
        //                // TODO : Directory.Delete 메서드의 2번째 파라미터 값 설정 구현 (2023.12.20 minjae)
        //                //        dirPath 경로의 폴더(디렉터리), 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거하려면 true, 제거하지 않으려면 false 설정
        //                //        true로 설정하면 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거할 때, 재귀적으로 (recursive) Directory.Delete 메서드 실행 
        //                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.directory.delete?view=net-8.0
        //                Directory.Delete(dirPath, true);

        //                // 루트 컨테이너 폴더(cancelWorkContainerId) + 다운로드 받은 파일 삭제 완료한 경우 
        //                if (false == Directory.Exists(dirPath))
        //                    MessageBox.Show($"컨테이너 작업 취소 완료!\r\n루트 컨테이너 폴더 ({cancelWorkContainerId})\r\n다운로드 받은 파일 삭제 완료");
        //                else
        //                    throw new Exception($"[오류] 루트 컨테이너 폴더 ({cancelWorkContainerId})\r\n삭제 오류\r\n담당자에게 문의하세요.");
        //            }
        //        }
        //        // 컨테이너 작업 취소 실패한 경우 
        //        else
        //            throw new Exception($"[오류] 컨테이너 작업 취소 실패\r\n담당자에게 문의 하세요.");

        //        //return;

        //        // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.09.24 minjae)
        //        // 삼항 연산자 사용하여
        //        // 루트 컨테이너 폴더(cancelWorkContainerId)가 존재하는 경우 true
        //        // 루트 컨테이너 폴더(cancelWorkContainerId)가 존재하지 않는 경우 false
        //        //isExist = (true == Directory.Exists(dirPath)) ? true : false;

        //        // TODO : ContainerHelper.cs -> ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 (2023.12.20 minjae)
        //        //ContainerPack = new ContainerPack
        //        //{
        //        //    projectId         = cancelWorkDatas[(int)EnumCancelWork.PROJECTID],
        //        //    teamId            = cancelWorkDatas[(int)EnumCancelWork.TEAMID],
        //        //    containerId       = cancelWorkDatas[(int)EnumCancelWork.CANCELWORK_CONTAINERID],
        //        //    // 연관 컨테이너 아이디 컨테이너 작업 취소 대상 제외
        //        //    linkedContainerId = string.Empty
        //        //};


        //        // 컨테이너 작업취소 시작 
        //        //await StartCancelWorkAsync(dirPath, cancelWorkContainerId, isExist);


        //        // 루트 컨테이너 폴더(cancelWorkContainerId)가 존재하는 경우 
        //        // if (true == Directory.Exists(dirPath))
        //        // {
        //        //     isExist = true;
        //        //     // 컨테이너 작업취소 시작 
        //        //     await StartCancelWorkAsync(dirPath, cancelWorkContainerId, isExist);
        //        // }

        //        // 루트 컨테이너 폴더(cancelWorkContainerId)가 존재하지 않는 경우
        //        // TODO : 루트 컨테이너 폴더(cancelWorkContainerId)가 존재하지 않아도 컨테이너 작업 취소 처리하도록 구현 (2024.01.26 minjae)
        //        // else
        //        // {
        //        //     isExist = false;
        //        //     // 오류 메시지 출력 
        //        //     // throw new Exception($"[오류] 컨테이너 작업 취소 실패!\r\n사유 - 삭제 할 루트 컨테이너 ({cancelWorkContainerId})\r\n폴더 존재 안 함.\r\n담당자에게 문의하세요.");
        //        //     //MessageBox.Show($"삭제 할 루트 컨테이너 ({cancelWorkContainerId})\r\n폴더 존재 하지 않습니다.\r\n컨테이너 작업취소를 진행하시겠습니까?");
        //        // 
        //        //     // 컨테이너 작업취소 시작 
        //        //     await StartCancelWorkAsync(dirPath, cancelWorkContainerId, isExist);
        //        // }
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //    }
        //    return;
        //}

        // TODO : 필요시 아래 주석친 파일 다운로드 비동기 메서드 "DownLoadAsync" 참고 (2024.10.07 minjae)
        // TODO : 체크아웃, 체크인, 파일 다운로드 중복 처리 시 컨텍스트 메뉴창 비활성화(Enabled = false;) 구현 예정 (2024.09.03 minjae)
        // 컨테스트 메뉴창 객체 이름 설정(예) x:Name="ExplorerContextMenu"

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 파일 다운로드
        /// </summary>
        //private async Task DownLoadAsync(object obj)
        //{
        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.DownLoad_LoadingWork);   // 다운로드 처리 중 화면 뷰모델 할당

        //    // string inputTenantId = LoginHelper.inputTenantId;                  // 테넌트 아이디
        //    // string tokenKey      = AppSetting.Default.Login.Token.resultData;  // 로그인 토큰 키 

        //    // TODO : 아래 주석친 소스 코드 3줄(parameters, downLoadDatas, downLoadDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //    // TODO : Object 배열 객체 obj 리스트로 변환 (2023.09.19 minjae)
        //    // 참고 URL - https://nonstop-antoine.tistory.com/36
        //    // var parameters      = obj as IEnumerable;                         // 다운로드 버튼으로 부터 받은 다운로드 받을 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 
        //    // var downLoadInfos   = new List<string>();                         // DownLoadDatas 를 리스트로 변환할 때 사용하는 리스트 객체 "DownLoadInfos"
        //    // var downLoadDataDic = new Dictionary<string, string>();           // 컨테이너에 속한 파일 정보 데이터를 담는 Dictionary 객체 DownLoadDataDic 생성

        //    // TODO : 다운로드 버튼 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                                      // 다운로드 버튼으로 부터 받은 다운로드 받을 파일 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] downLoadDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] downLoadDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());

        //    // var downLoadDatas = parameters.ToList();                   // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "downLoadDatas"

        //    string fileId = string.Empty;                                 // 파일 아이디
        //    string fileName = string.Empty;                               // 파일명(이름)

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 아래 주석친 코드 필요시 참고 (2024.09.04 minjae)
        //        // 파일 다운로드 처리 중인 상태에서 파일 다운로드를 중복 요청하는 경우 
        //        //if(true == IsDownLoadWorking)
        //        //    throw new Exception("현재 파일 다운로드 진행 중입니다.\r\n파일 다운로드 완료까지 잠시 기다려주세요.");


        //        // MessageBox.Show("다운로드 테스트 중입니다.");

        //        // TODO : 아래 foreach문 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //        // downLoadDatas 안에 존재하는 파일 정보 반복문으로 방문 -> List 객체 downLoadInfos에 파일 정보 데이터 추가 
        //        //foreach (var data in downLoadDatas)
        //        //{
        //        //    downLoadInfos.Add(data.ToString());
        //        //}

        //        // TODO : 아래 주석친 소스코드 5줄(downLoadDataDic.Add) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //        //downLoadDataDic.Add(ProjectHelper.projectId, downLoadInfos[(int)EnumDownLoad.PROJECTID]);
        //        //downLoadDataDic.Add(TeamHelper.teamId, downLoadInfos[(int)EnumDownLoad.TEAMID]);
        //        //downLoadDataDic.Add(ContainerHelper.containerId, downLoadInfos[(int)EnumDownLoad.CONTAINERID]);
        //        //downLoadDataDic.Add(FileHelper.fileId, downLoadInfos[(int)EnumDownLoad.FILEID]);
        //        //downLoadDataDic.Add(FileHelper.fileNm, downLoadInfos[(int)EnumDownLoad.FILENAME]);

        //        // TODO : 아래 소스코드 ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 필요 없을시 삭제 예정 (2023.12.05 minjae)
        //        // TODO : ProjectHelper.cs -> ProjectPack 클래스 프로퍼티 "ProjectPack" 구현 및 테스트 프로젝트 아이디(testProjectId), 테스트 팀 아이디(testTeamId) 값 할당 (2023.09.04 minjae)
        //        //        추후 필요시 수정 예정
        //        // ContainerPack = new ContainerPack
        //        // {
        //        //     projectId   = downLoadDataDic[ProjectHelper.projectId],
        //        //     teamId      = downLoadDataDic[TeamHelper.teamId],
        //        //     containerId = downLoadDataDic[ContainerHelper.containerId]
        //        // };

        //        //ContainerPack = new ContainerPack
        //        //{
        //        //    projectId         = downLoadDatas[(int)EnumDownLoad.PROJECTID],
        //        //    teamId            = downLoadDatas[(int)EnumDownLoad.TEAMID],
        //        //    containerId       = downLoadDatas[(int)EnumDownLoad.CONTAINERID],
        //        //    // 연관 컨테이너 아이디 파일 다운로드 대상 제외
        //        //    linkedContainerId = string.Empty
        //        //};
        //        ContainerPack = new ContainerPack(downLoadDatas[(int)EnumDownLoad.PROJECTID],
        //                                          downLoadDatas[(int)EnumDownLoad.TEAMID],
        //                                          downLoadDatas[(int)EnumDownLoad.CONTAINERID],
        //                                          string.Empty);

        //        // TODO : 아래 소스코드 FilePack 클래스 프로퍼티 "FilePack" 구현 필요 없을시 삭제 예정 (2023.12.05 minjae)
        //        // FilePack = new FilePack 
        //        // {
        //        //     fileId   = downLoadDataDic[FileHelper.fileId],       // 파일 아이디
        //        //     fileName = downLoadDataDic[FileHelper.fileNm]        // 파일명(이름)
        //        // };

        //        //FilePack = new FilePack
        //        //{
        //        //    fileId      = downLoadDatas[(int)EnumDownLoad.FILEID],     // 파일 아이디
        //        //    fileName    = downLoadDatas[(int)EnumDownLoad.FILENAME]    // 파일명(이름)
        //        //};
        //        FilePack = new FilePack(downLoadDatas[(int)EnumDownLoad.FILEID], downLoadDatas[(int)EnumDownLoad.FILENAME]);

        //        // 파일 다운로드 처리 중 화면 출력 
        //        if (false == IsDownLoadWorking)
        //        {
        //            IsDownLoadWorking = true;
        //            loadingWorkVM.ShowWindow();
        //        }

        //        // 파일 다운로드 진행
        //        await ExplorerRestServer.GetFileDownLoadAsync(Client, ContainerPack, FilePack, LoginView.RequestHeaders);

        //        // TODO : 파일 다운로드 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
        //        // 참고 URL - https://cosmosproject.tistory.com/563
        //        // 파일 다운로드 처리 완료 
        //        IsDownLoadWorking = false;

        //        MessageBox.Show("파일 다운로드 완료!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

        //        if (true == IsDownLoadWorking) IsDownLoadWorking = false;

        //    }
        //    finally
        //    {
        //        loadingWorkVM.RequestClose(null);   // 파일 다운로드 처리 중 화면 종료
        //    }
        //    return;
        //}

        // TODO : 폴더 리스트 데이터 받아오기 메서드 "FolderListAsync" 필요 시 구현 예정 (2023.09.19 minjae)
        //private async Task FolderListAsync(object obj)
        //{
        //    string projectId = ProjectHelper.testProjectId;   // 프로젝트 아이디 
        //    string teamId    = ProjectHelper.testTeamId;      // 팀 아이디
        //    string tokenKey = AppSetting.Default.Login.Token.resultData;   // 로그인 토큰 키 
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 폴더 리스트 데이터 받아오기 
        //        // FolderList = await ExplorerRestServer.GetExplorerFolderListAsync(client, tokenKey, projectId, teamId);


        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage +  ex.Message);
        //    }
        //}



        // TODO : 필요시 아래 주석친 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드 비동기 메서드 "CheckOutAsync" 참고 (2024.10.08 minjae)
        // TODO : 체크아웃, 체크인, 파일 다운로드 중복 처리 시 컨텍스트 메뉴창 비활성화(Enabled = false;) 구현 예정 (2024.09.03 minjae)
        // 컨테스트 메뉴창 객체 이름 설정(예) x:Name="ExplorerContextMenu"

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 체크아웃 + (참조/연관) 관계 컨테이너 파일 다운로드  
        /// </summary>
        //private async Task CheckOutAsync(object obj)
        //{
        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CheckOut_LoadingWork);   // 체크아웃 처리 중 화면 뷰모델 할당

        //    // string tokenKey      = AppSetting.Default.Login.Token.resultData;   // 로그인 토큰 키 
        //    // string inputTenantId = LoginHelper.inputTenantId;                   // 테넌트 아이디

        //    string checkOutContainerId = string.Empty;                             // 체크아웃 요청 컨테이너 아이디

        //    // TODO : Context Menu(컨텍스트 메뉴) "체크아웃" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                                  // Context Menu(컨텍스트 메뉴) "체크아웃" 클릭시 받은 체크아웃 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId") (Object Array -> IEnumerable 형변환)

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] checkOutDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        //    string[] checkOutDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());


        //    // var checkOutDatas = parameters.ToList();                         // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "checkOutDatas"


        //    // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃" 클릭시 멀티 바인딩 대상 중 "exp_FolderId" 제외함.
        //    //        제외한 사유는 "exp_FolderId" 설정시 탐색기 뷰모델(ExplorerVM.cs) -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync"
        //    //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로 해당 프로퍼티는 멀티 바인딩 대상에서 제외 처리함. (2023.12.13 minjae) 
        //    //        하여 아래 string 클래스 객체 folderId 또한 사용 제외 (2023.12.13 minjae)
        //    // string folderId = string.Empty;                               // 폴더 아이디 

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 아래 주석친 코드 필요시 참고 (2024.09.04 minjae)
        //        // 체크아웃 처리 중인 상태에서 체크아웃을 중복 요청하는 경우 
        //        //if(true == IsCheckOutWorking)
        //        //    throw new Exception("현재 체크아웃 요청 중인 상태입니다.\r\n체크아웃 완료까지 잠시 기다려주세요.");


        //        // 연관 컨테이너 파일 다운로드 처리 중인 상태에서 다른 컨테이너의 체크아웃을 중복 요청하는 경우 
        //        if (true == IsLinkedContainerWorking)
        //            throw new Exception("현재 연관 컨테이너에 속한 파일 다운로드 중입니다.\r\n다운로드 완료까지 잠시 기다려주세요.");


        //        // TODO : 화면 셋팅 "CheckOutAsync" 필요시 사용 예정 (2023.12.19 minjae)
        //        // DisplayType = CobimHelper.TypeOfCheckOut;
        //        // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
        //        // DisplaySetting(DisplayType);

        //        // 1 단계 : 비동기 메서드 "CheckOutAsync"에 파라미터로 받은 전달 인자(object 배열 객체)로 "checkOutDatas" 리스트에 값 할당 
        //        //          체크아웃 요청 컨테이너 아이디 "checkOutContainerId", "folderId", ContainerPack, FilePack 생성
        //        // 체크아웃 요청 컨테이너 아이디 문자열 값 string 클래스 객체 checkOutContainerId에 할당 
        //        checkOutContainerId = checkOutDatas[(int)EnumCheckOut.CHECKOUT_CONTAINERID];


        //        // folderId = checkOutDatas[(int)EnumCheckOut.FOLDERID];

        //        // TODO : ContainerHelper.cs -> ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 (2023.12.05 minjae)
        //        //ContainerPack = new ContainerPack 
        //        //{ 
        //        //    projectId   = checkOutDatas[(int)EnumCheckOut.PROJECTID],
        //        //    teamId      = checkOutDatas[(int)EnumCheckOut.TEAMID],
        //        //    containerId = checkOutDatas[(int)EnumCheckOut.CHECKOUT_CONTAINERID]
        //        //};
        //        ContainerPack = new ContainerPack(checkOutDatas[(int)EnumCheckOut.PROJECTID], checkOutDatas[(int)EnumCheckOut.TEAMID], checkOutDatas[(int)EnumCheckOut.CHECKOUT_CONTAINERID]);

        //        // TODO : 다운로드할 파일 정보 프로퍼티 "FilePack"에  "fileId", "fileName" 필요시 아래 소스코드 사용 예정 (2023.12.08 minjae)
        //        //FilePack = new FilePack 
        //        //{ 
        //        //    fileId      = checkOutDatas[(int)EnumCheckOut.FILEID], 
        //        //    fileName    = checkOutDatas[(int)EnumCheckOut.FILENAME]
        //        //};

        //        FilePack = new FilePack(checkOutDatas[(int)EnumCheckOut.FILEID], checkOutDatas[(int)EnumCheckOut.FILENAME]);

        //        // 2 단계: 체크아웃 요청한 컨테이너 아이디와 연관된 컨테이너 목록 조회
        //        //         (Rest API 8.연관 컨테이너 목록 조회 호출) 
        //        var linkedContainerList = await ExplorerRestServer.GetLinkedContainerListAsync(Client, ContainerPack, LoginView.RequestHeaders);

        //        // TODO : 아래 주석친 테스트 코드 (연관 컨테이너가 존재하는 경우) 코드 필요시 참고 (2024.01.03 minjae)
        //        // TODO : 아래 연관 컨테이너가 존재하는 경우 리스트 객체 "linkedContainerList" 컨테이너 아이디(containerId) 오름차순 정렬 (Sort 메서드) (2023.11.29 minjae)
        //        // 참고 URL - https://www.techiedelight.com/ko/sort-list-in-descending-order-in-csharp/
        //        // 연관 컨테이너가 존재하는 경우 
        //        // if (linkedContainerList.Count >= (int)EnumLinkedRelation.EXISTENCE)
        //        //     linkedContainerList.Sort((containerA, containerB) => containerA[ContainerHelper.containerId].ToString().CompareTo(containerB[ContainerHelper.containerId].ToString()));

        //        // 체크아웃 요청한 컨테이너 아이디와 연관된 컨테이너 리스트가 null이 아닐 경우에만 체크아웃 실행 
        //        if (linkedContainerList is not null)
        //        {
        //            // 체크아웃 처리 중 화면 출력 
        //            if (false == IsCheckOutWorking)
        //            {
        //                IsCheckOutWorking = true;
        //                loadingWorkVM.ShowWindow();
        //            }

        //            // 3 단계 : Rest API 10. 컨테이너 체크아웃 + 다운로드 호출 
        //            // 체크아웃 호출을 위해 만족 해야할 사항(조건)
        //            // 1) 컨테이너 생성자 == 체크아웃 요청자(로그인 사용자 아이디)
        //            // 2) 컨테이너 readYn 필드 값이 'N' 일 때
        //            // 3) 검토 요청 중인 컨테이너가 아닐 때(statusCode != 'comstt20')
        //            await ExplorerRestServer.GetCheckOutAsync(Client, ContainerPack, FilePack, LoginView.RequestHeaders);

        //            // TODO : 체크아웃 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
        //            // 참고 URL - https://cosmosproject.tistory.com/563
        //            // 체크아웃 처리 중 화면 종료 
        //            // IsCheckOutWorking = false;
        //            loadingWorkVM.RequestClose(null);

        //            // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
        //            //        체크아웃 비동기 메서드 "CheckOutAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //            //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //            //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 파라미터를 프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae) 
        //            // 4 단계: 체크아웃 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
        //            if (false == string.IsNullOrWhiteSpace(SelectedFolderId))
        //                await ContainerListCreateAsync(ContainerPack, SelectedFolderId);
        //            // var containerList = await ExplorerRestServer.GetContainerListAsync(client, ProjectPack, folderId, tokenKey);
        //            else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");
        //        }
        //        else
        //            throw new Exception("[오류]연관 컨테이너 목록 null Exception.\r\n담당자에게 문의하시기 바랍니다.");



        //        // 연관 컨테이너 목록이 존재하지 않는 경우 
        //        if (true == linkedContainerList.Count.Equals((int)EnumLinkedRelation.NONE))
        //            MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 목록이 존재하지 않습니다.");

        //        // 연관 컨테이너 목록이 존재하는 경우 
        //        else
        //        {
        //            //MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 다운로드 시작합니다.");

        //            // 연관 컨테이너 파일 다운로드 처리 중 화면 뷰모델 할당
        //            loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.LinkedContainer_LoadingWork);

        //            // 연관 컨테이너 파일 다운로드 처리 중 화면 출력 
        //            if (false == IsLinkedContainerWorking)
        //            {
        //                IsLinkedContainerWorking = true;
        //                loadingWorkVM.ShowWindow();
        //            }

        //            foreach (var container in linkedContainerList)
        //            {
        //                // TODO : 다운로드할 파일 정보 프로퍼티 "FilePack"에 속한 "fileId", "fileName"에 문자열 재할당 (2023.11.30 minjae)
        //                // 파일 아이디
        //                FilePack.fileId = container[ContainerHelper.fileId].ToString();

        //                // 파일명(이름)   
        //                FilePack.fileName = container[ContainerHelper.fileNm].ToString();

        //                // TODO : 연관 컨테이너 다운로드 하는 경우 다운로드 받은 파일의 상위 폴더명(컨테이너 아이디)을
        //                //        각각의 연관 컨테이너 아이디가 아니라 루트 컨테이너 아이디(체크아웃 요청한 컨테이너 아이디)
        //                //        하나로만 상위 폴더명으로 설정 해야 하므로 아래 소스코드는 주석 처리 진행 (2023.12.14 minjae)
        //                // 체크아웃 요청한 컨테이너 아이디 
        //                // ContainerPack.containerId = container[ContainerHelper.containerId].ToString();

        //                // 연관 컨테이너 아이디
        //                ContainerPack.linkedContainerId = container[ContainerHelper.containerId].ToString();

        //                // TODO : 체크아웃 요청한 컨테이너 아이디 문자열 값과 연관 컨테이너 아이디의 문자열이 동일한 경우 파일 다운로드 대상 제외
        //                //        이미 체크아웃 + 파일 다운로드 처리 동시에 하였으므로 파일 중복 다운로드 안 하기 위해 continue 처리 구현 (2023.12.15 minjae)
        //                if (ContainerPack.containerId.Equals(ContainerPack.linkedContainerId)) continue;

        //                // 연관 컨테이너에 속한 파일 다운로드 진행
        //                else await ExplorerRestServer.GetFileDownLoadAsync(Client, ContainerPack, FilePack, LoginView.RequestHeaders);
        //            }

        //            MessageBox.Show($"컨테이너 아이디 {checkOutContainerId}\r\n체크아웃 완료!\r\n연관 컨테이너 파일 다운로드 완료!");
        //        }

        //        // TODO : 연관 컨테이너 파일 다운로드 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.05 minjae)
        //        // 참고 URL - https://cosmosproject.tistory.com/563
        //        // 연관 컨테이너 파일 다운로드 처리 중 화면 종료 
        //        // IsLinkedContainerWorking = false;

        //        // 체크아웃 처리 중 또는 연관 컨테이너 파일 다운로드 처리 완료
        //        IsCheckOutWorking = false;
        //        IsLinkedContainerWorking = false;

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //        if (true == IsCheckOutWorking || true == IsLinkedContainerWorking)
        //        {
        //            IsCheckOutWorking = false;
        //            IsLinkedContainerWorking = false;
        //        }
        //    }
        //    finally
        //    {
        //        loadingWorkVM.RequestClose(null);   // 체크아웃 처리 중 또는 연관 컨테이너 파일 다운로드 처리 중 화면 종료 
        //    }
        //    // return;
        //}

        // TODO : 필요시 아래 주석친 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 비동기 메서드 "CheckInAsync" 참고 (2024.10.11 minjae)
        // TODO : 체크아웃, 체크인, 파일 다운로드 중복 처리 시 컨텍스트 메뉴창 비활성화(Enabled = false;) 구현 예정 (2024.09.03 minjae)
        // 컨테스트 메뉴창 객체 이름 설정(예) x:Name="ExplorerContextMenu"

        // TODO : 메서드 파라미터 "object obj" -> object 배열 "object[] obj"로 변환 및 지역변수 "var parameters" 사용 안하고 리스트 변환 가능한지 테스트 진행 예정 (2023.12.18 minjae)
        /// <summary>
        /// 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드  
        /// 컨테이너 체크인 + (컨테이너에 속한 파일) 웹서버로 업로드 성공하면 
        /// HubBox 파일 변환 요청 Rest API 메서드 HubBoxRestServer.PostTranslateAsync 호출 해야함.
        /// </summary>
        //private async Task CheckInAsync(object obj)
        //{
        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.CheckIn_LoadingWork);   // 체크인 처리 중 화면 뷰모델 할당

        //    Dictionary<string, object> resultTranslateData = new Dictionary<string, object>();   // 특정 컨테이너에 속한 파일 변환 요청 Dictionary 객체 초기화

        //    string resultTranslateCode = string.Empty;                                           // 특정 컨테이너에 속한 파일 변환 처리 결과 상태 코드 문자열 초기화
        //    string resultTranslateMsg = string.Empty;                                            // 특정 컨테이너에 속한 파일 변환 처리 관련 공통 메세지 초기화

        //    // string inputTenantId = LoginHelper.inputTenantId;                   // 테넌트 아이디
        //    // string tokenKey      = AppSetting.Default.Login.Token.resultData;   // 로그인 토큰 키 

        //    // TODO : 아래 주석친 소스 코드 3줄(parameters, checkInDatas, checkInDataDic) 필요 없을 시 삭제 예정 (2023.12.05 minjae)
        //    // TODO : Object 배열 객체 obj 리스트로 변환 (2023.12.05 minjae)
        //    // 참고 URL - https://nonstop-antoine.tistory.com/36
        //    // var parameters     = obj as IEnumerable;                          // Context Menu(컨텍스트 메뉴) "체크인" 클릭시 받은 체크인 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId", "exp_FileName") (Object Array -> IEnumerable 형변환) 
        //    // var checkInDatas   = new List<string>();                          // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "checkInDatas"
        //    // var checkInDataDic = new Dictionary<string, string>();            // 체크인시 필요한 정보 데이터를 담는 Dictionary 객체 "checkInDataDic" 생성

        //    // TODO : Context Menu(컨텍스트 메뉴) "체크인" 클릭시 MultiCommandParameter 데이터(obj)를 object 배열(object[])로 형변환 (2023.12.05 minjae)
        //    // 참고 URL - https://www.melodiouscode.net/passing-multiple-parameters-to-an-icommand-in-wpf
        //    var parameters = (object[])obj;                                      // Context Menu(컨텍스트 메뉴) "체크인" 클릭시 받은 체크인 시 필요한 정보(CommandParameter "exp_ProjectId", "exp_TeamId", "exp_ContainerId", "exp_FileId", "exp_FileName") (Object Array -> IEnumerable 형변환) 

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열 "string[] checkInDatas" 형식으로 형변환 구현 (2023.12.19 minjae)
        //    // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-Object-%ED%98%95%EC%8B%9D-String-%ED%98%95%EC%8B%9D%EC%9C%BC%EB%A1%9C-%ED%98%95%EB%B3%80%ED%99%98Casting-%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95

        //    // TODO : (기존) object 배열 "(object[])obj" 형식 -> (변경) string 배열
        //    //        단, 변경할 때, (기존) object 배열에 속하는 데이터 중 "제목"을
        //    //        갖고 있는 데이터를 제외하기 위해 Linq 조건절 확장 메서드 Where 사용해서
        //    //        특정 인덱스 "(int)EnumCheckIn.TITLE" 제외한 
        //    //        데이터만 추출해서 string 배열 객체 "checkInDatas" 구현 (2024.08.26 minjae)
        //    // 참고 URL - https://chatgpt.com/c/332ba391-669d-4e7b-8e5a-af86cbd6516e
        //    string[] checkInDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString())
        //                                 .Where((checkInRequestData, index) => index != (int)EnumCheckIn.TITLE)
        //                                 .Select(checkInRequestData => checkInRequestData)
        //                                 .ToArray();


        //    string[] translateDatas = Array.ConvertAll(parameters, param => (param ?? string.Empty).ToString());   // HubBox 파일 변환 요청 Rest API 요청 데이터 string 배열 객체 "translateDatas"


        //    // var checkInDatas = parameters.ToList();           // parameters를 리스트로 변환할 때 사용하는 리스트 객체 "checkInDatas"

        //    string checkInContainerId = string.Empty;            // 체크인 요청 컨테이너 아이디            
        //    // string statusCode = string.Empty;                 // 컨테이너 검토상태("comstt10": 미진행, "comstt20": 진행, "comstt30": 완료)
        //    // string readYn     = string.Empty;                 // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
        //    // string workUserId = string.Empty;                 // 컨테이너 작업중인 사용자 아이디


        //    // Image uploadFiles = null;                         // 업로드 할 이미지 파일
        //    // byte[] uploadFileStream = null;                   // 업로드 할 파일 입력 스트림 
        //    // ImageConverter imgConverter = null;               // 이미지 Converter

        //    // string folderId    = string.Empty;                // 선택(클릭)한 폴더 아이디
        //    // string uploadFiles = string.Empty;                // 업로드 할 파일
        //    string fileName = string.Empty;                   // 체크인 처리할 컨테이너에 속한 파일명
        //    // string fileSize = string.Empty;                   // 파일사이즈
        //    // string comment  = string.Empty;                   // Comment - Description

        //    string fileId = string.Empty;                      // 체크인 처리하기 전 파일 아이디  
        //    string revsnVal = string.Empty;                      // 체크인 처리하기 전 파일 버전 

        //    // TODO : 체크인 처리 완료 후 파일명 string 객체 checkInFileName 필요시 사용 예정 (2024.08.26 minjae)
        //    //string checkInFileName = string.Empty;             // 체크인 처리 완료 후 파일명
        //    string checkInFileId = string.Empty;                 // 체크인 처리 완료 후 파일 아이디  
        //    string checkInRevsnVal = string.Empty;               // 체크인 처리 완료 후 파일 버전 

        //    // TODO : C# 문자열 보간 기능 사용해서 웹서버로 업로드 할 상위 폴더 경로 문자열로 변환 (2024.09.02 minjae)
        //    // 참고 URL - https://gaeunhan.tistory.com/61
        //    string dirPath = string.Empty;                       // 웹서버로 업로드 할 파일의 상위 디렉토리(폴더) 경로
        //    string filePath = string.Empty;                       // 웹서버로 업로드 할 파일 전체 경로 

        //    string deleteContainerId = string.Empty;             // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 컨테이너 아이디
        //    string deleteDirPath = string.Empty;                 // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일의 상위 디렉토리(폴더) 경로
        //    string deleteFilePath = string.Empty;                // 체크인 완료(파일 웹서버로 업로드 포함) 후 사용자 PC 로컬에 존재하는 삭제할 파일 경로

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 아래 주석친 코드 필요시 참고 (2024.09.04 minjae)
        //        // 체크인 처리 중인 상태에서 체크인을 중복 요청하는 경우 
        //        // if(true == IsCheckInWorking)
        //        //     throw new Exception("현재 체크인 요청 중인 상태입니다.\r\n체크인 완료까지 잠시 기다려주세요.");


        //        var partVM = Container.Get<CheckInPartVM>().ToShowData();   // 체크인 팝업 화면 생성 및 결과 리턴

        //        // TODO : 아래 주석친 테스트용 체크인 팝업 화면 출력 소스코드 필요시 참고 (2024.01.03 minjae) 
        //        // OpenCheckIn(); // 체크인 화면 출력
        //        // 체크인 팝업 화면 출력
        //        // var con = Container.Get<CheckInPartVM>();
        //        // WindowManager.ShowDialog(con);

        //        // TODO : await Task.Yield(); 사용법 제대로 확인 후 사용 예정 (2023.12.04 minjae)
        //        // 참고 URL - https://www.sysnet.pe.kr/2/0/12241
        //        // 참고 2 URL - https://www.sysnet.pe.kr/2/0/12245
        //        // 참고 3 URL - https://stackoverflow.com/questions/22645024/when-would-i-use-task-yield
        //        await Task.Yield();

        //        // TODO : 닫기("X") 버튼 클릭 시 컨테이너 체크인 + 업로드 메서드 "CheckInAsync" 종료 로직 구현 (2023.12.07 minjae)
        //        if (true == partVM.IsExit) return;

        //        // DisplayType = CobimHelper.TypeOfCheckIn;
        //        // IsVisible = true;            // 화면에 필요한 컨트롤러 (DockPanel, Button 등등...) 화면에 보이도록 활성화
        //        // DisplaySetting(DisplayType);

        //        // checkInContainerId = checkInDatas[(int)EnumCheckIn.CONTAINERID];
        //        // folderId = checkInDatas[(int)EnumCheckIn.FOLDERID];

        //        // TODO : ContainerHelper.cs -> ContainerPack 클래스 프로퍼티 "ContainerPack" 구현 (2023.12.05 minjae)
        //        //ContainerPack = new ContainerPack
        //        //{
        //        //    projectId   = checkInDatas[(int)EnumCheckIn.PROJECTID],
        //        //    teamId      = checkInDatas[(int)EnumCheckIn.TEAMID],
        //        //    containerId = checkInDatas[(int)EnumCheckIn.CHECKIN_CONTAINERID]
        //        //};

        //        ContainerPack = new ContainerPack(checkInDatas[(int)EnumCheckIn.PROJECTID],
        //                                          checkInDatas[(int)EnumCheckIn.TEAMID],
        //                                          checkInDatas[(int)EnumCheckIn.CONTAINERID]);


        //        // TODO : 다른 PC에서도 파일 업로드 기능이 실행될 수 있도록 웹서버로 업로드 할 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.12.05 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //        // 웹서버로 업로드 할 파일의 상위 디렉토리 폴더 전체 경로 예시 - "C:\Users\bhjeon\Documents\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002"
        //        // SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 사용
        //        dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Assembly.GetEntryAssembly().GetName().Name}\\{ContainerPack.projectId}\\{ContainerPack.teamId}\\{ContainerPack.containerId}";


        //        // 체크인 처리 하기전 파일 이름, 파일 아이디, 파일 버전 문자열 할당 
        //        fileName = checkInDatas[(int)EnumCheckIn.UPLOAD_FILENAME];
        //        fileId = checkInDatas[(int)EnumCheckIn.FILEID];
        //        revsnVal = checkInDatas[(int)EnumCheckIn.REVSNVAL];


        //        // TODO : 웹서버로 업로드 할 파일 전체 경로 구현 (2023.12.04 minjae)
        //        // 참고 URL - https://dlgnlfus.tistory.com/129
        //        filePath = dirPath + $"\\{fileName}";   // 웹서버로 업로드 할 파일 전체 경로 

        //        // TODO : 웹서버로 업로드 할 파일 전체 경로에 있는 파일 정보 불러 오도록 구현 (2023.12.05 minjae)
        //        // 참고 URL   - https://chashtag.tistory.com/37
        //        // 참고 2 URL - https://www.delftstack.com/ko/howto/csharp/get-filesize-in-csharp/
        //        // 참고 2 URL - https://dlgnlfus.tistory.com/129
        //        if (true == File.Exists(filePath))
        //        {
        //            // uploadFiles      = Image.FromFile(filePath);
        //            // imgConverter     = new ImageConverter();
        //            // uploadFileStream = (byte[])imgConverter.ConvertTo(uploadFiles, typeof(byte[]));

        //            // FileInfo info   = new FileInfo(filePath);

        //            // using var upLoadFileStream = File.OpenRead(filePath);   // Rest API 컨테이너 체크인 요청하기 전 웹서버로 업로드 할 파일 읽어오기(File.OpenRead)

        //            // Rest API 컨테이너 체크인 요청하기 전 웹서버로 업로드 할 파일 읽어오기(File.OpenRead)
        //            using (FileStream upLoadFileStream = File.OpenRead(filePath))
        //            {
        //                // TODO : 추후 체크인 구현시 업로드 하려는 파일명(uploadFiles)과 기존 체크인 된 파일명(fileName)이 다를 경우
        //                //        enum 열거형 구조체 EnumCheckIn 멤버 변수 "FILENAME" 사용해서 로직 수정 예정 (2023.12.05 minjae)
        //                CheckInPack = new CheckInPack
        //                {
        //                    folderId = checkInDatas[(int)EnumCheckIn.FOLDERID],
        //                    uploadFiles = upLoadFileStream,
        //                    fileName = checkInDatas[(int)EnumCheckIn.UPLOAD_FILENAME],
        //                    fileId = string.Empty,
        //                    // TODO : pCheckInPack.fileSize의 파일 크기 단위가 바이트인지 아닌지 확인 필요 (2023.12.05 minjae)
        //                    fileSize = upLoadFileStream.Length.ToString(),
        //                    // TODO : 아래 주석친 코드 "comment = ExplorerView.RequestData.exp_Comment" 필요시 사용 예정 (2024.01.05 minjae)
        //                    // comment  = ExplorerView.RequestData.exp_Comment
        //                    comment = partVM.SelectedData.exp_Comment
        //                };

        //                // 체크인 처리 중 화면 출력 
        //                if (false == IsCheckInWorking)
        //                {
        //                    IsCheckInWorking = true;
        //                    loadingWorkVM.ShowWindow();
        //                }

        //                // 컨테이너 체크인 + 업로드 
        //                var resultCheckInData = await ExplorerRestServer.PostCheckInAsync(Client, ContainerPack, CheckInPack, filePath, LoginView.RequestHeaders);

        //                // Dictionary 객체 resultCheckInData 특정 키(Key) "fileName", "fileId", "revsnVal"에 속하는 값(Value) 문자열로 변환 (2024.08.26 minjae)
        //                // 참고 URL - https://developer-talk.tistory.com/694
        //                // TODO : 체크인 처리 완료 후 파일명 string 객체 checkInFileName 필요시 사용 예정 (2024.08.26 minjae)
        //                // checkInFileName = resultCheckInData[ContainerHelper.fileNm].ToString();
        //                checkInFileId = resultCheckInData[ContainerHelper.fileId].ToString();
        //                checkInRevsnVal = resultCheckInData[ContainerHelper.revsnVal].ToString();

        //                // TODO : 체크인 처리중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 minjae)
        //                // 참고 URL - https://cosmosproject.tistory.com/563
        //                // 체크인 처리 중 화면 종료 
        //                // IsCheckInWorking = false;
        //                loadingWorkVM.RequestClose(null);

        //            }   // Rest API 컨테이너 체크인 요청 성공후 웹서버로 업로드 할 파일 리소스 해제(Dispose()) 처리 -> 리소스 해제(Dispose()) 해야 사용자 로컬 PC에 설치된 파일 삭제 처리 가능

        //            // 컨테이너 체크인 + 업로드 성공한 경우 
        //            // checkInFileId(체크인 처리 완료 후 파일 아이디) != fileId(체크인 처리하기 전 파일 아이디) 
        //            // checkInRevsnVal(체크인 처리 완료 후 파일 버전) != revsnVal(체크인 처리하기 전 파일 버전)
        //            if (checkInFileId != fileId && checkInRevsnVal != revsnVal)
        //            {
        //                // 체크인 완료된 이후 변경된 파일 아이디 값(checkInFileId)을 파일 변환 요청 Dictionary(translateDatas[(int)EnumTranslate.FILEID])에 할당
        //                translateDatas[(int)EnumTranslate.FILEID] = checkInFileId;

        //                TranslatePack = new TranslatePack(translateDatas[(int)EnumTranslate.PROJECTID],
        //                                                  translateDatas[(int)EnumTranslate.TEAMID],
        //                                                  translateDatas[(int)EnumTranslate.CONTAINERID],
        //                                                  translateDatas[(int)EnumTranslate.FILEID]);

        //                checkInContainerId = translateDatas[(int)EnumTranslate.CONTAINERID];

        //                //MessageBox.Show($"컨테이너 아이디 {checkInContainerId}\r\n체크인 완료!\r\n파일 변환 요청 시작합니다.");

        //                // 연관 컨테이너 파일 다운로드 처리 중 화면 뷰모델 할당
        //                loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.Translate_LoadingWork);

        //                // 파일 변환 처리 중 화면 출력 
        //                if (false == IsTranslateWorking)
        //                {
        //                    IsTranslateWorking = true;
        //                    loadingWorkVM.ShowWindow();
        //                }

        //                // HubBox Rest API 3. 컨테이너에 속한 특정 파일 변환 API 호출 
        //                resultTranslateData = await HubBoxRestServer.PostTranslateAsync(Client, TranslatePack, LoginView.RequestHeaders);

        //                // TODO : Dictionary 객체 resultTranslateData에 담긴 파일 변환 API 결과 값 중 파일 변환 성공/처리 중일 경우 키 "code"에 매핑된 값으로 할당
        //                //        만약 파일 변환 실패했을 경우 키 "status"에 매핑된 값으로 할당 하도록 삼항연산자 사용하여 로직 구현하기 (2024.08.26) 
        //                // bool result =  resultTranslateData.TryGetValue(resultTranslateData[EnumTranslateResult.CODE.ToDescription()], out resultTranslateCode))
        //                // resultTranslateCode = true == resultTranslateData.ContainsKey(EnumTranslateResult.CODE.ToDescription()) ? 
        //                //                         resultTranslateData[EnumTranslateResult.CODE.ToDescription()].ToString() : 
        //                //                       (true == resultTranslateData.ContainsKey(EnumTranslateResult.STATUS.ToDescription()) ? resultTranslateData[EnumTranslateResult.STATUS.ToDescription()].ToString() : null);

        //                if (true == resultTranslateData.ContainsKey(EnumTranslateResult.CODE.ToDescription()))
        //                    resultTranslateCode = resultTranslateData[EnumTranslateResult.CODE.ToDescription()].ToString();

        //                else if (true == resultTranslateData.ContainsKey(EnumTranslateResult.STATUS.ToDescription()))
        //                    resultTranslateCode = resultTranslateData[EnumTranslateResult.STATUS.ToDescription()].ToString();

        //                else resultTranslateCode = null;

        //                // TryGetValue vs ContainsKey 차이
        //                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.dictionary-2.trygetvalue?view=net-8.0
        //                // 참고 2 URL - https://stackoverflow.com/questions/38377318/contains-in-dictionary-in-c-sharp
        //                // 참고 3 URL - https://ckhyeok.tistory.com/37
        //                // 참고 4 URL - https://m.blog.naver.com/PostView.nhn?blogId=crusaderholy&logNo=100112263890&proxyReferer=https:%2F%2Fwww.google.com%2F
        //                // 참고 5 URL - https://coding-shop.tistory.com/53
        //                // 참고 6 URL - https://developer-talk.tistory.com/686

        //                // 컨테이너에 속한 특정 파일 변환 Dictionary 객체 resultTranslateData 에
        //                // 키값 "code", "status" 둘 다 포함되지 않은 경우 
        //                // if(false == resultTranslateData.TryGetValue(EnumTranslateResult.CODE.ToDescription(), out resultTranslateCode)
        //                //    && false == resultTranslateData.TryGetValue(EnumTranslateResult.STATUS.ToDescription(), out resultTranslateCode)) 
        //                //      resultTranslateCode = null;

        //                //resultTranslateMsg =  $"* 파일 변환 안내 *"
        //                //                    + $"\r\n\r\n처리 결과 - {resultTranslateData[EnumTranslateResult.MESSAGE.ToDescription()]} ({resultTranslateCode})"
        //                //                    + $"\r\n컨테이너 아이디  - {translateDatas[(int)EnumTranslate.CONTAINERID].ToUpper()}"
        //                //                    + $"\r\n제목 - {translateDatas[(int)EnumTranslate.TITLE]}"
        //                //                    + $"\r\n파일 이름 - {translateDatas[(int)EnumTranslate.TRANSLATE_FILENAME]}";

        //                //MessageBox.Show(resultTranslateMsg);   // 파일 변환 처리 결과 메세지 출력 
        //            }
        //            // 컨테이너 체크인 + 업로드 실패한 경우
        //            else
        //                throw new Exception("체크인 오류 발생!\r\n담당자에게 문의하세요.");   // 체크인 오류 사유 메시지 출력 



        //            // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크인" 클릭시 탐색기 뷰모델(ExplorerVM.cs)
        //            //        체크인 비동기 메서드 "CheckInAsync" 호출 -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 호출 
        //            //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로
        //            //        컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync" 파라미터를 프로퍼티 "SelectedFolderId"로 전달함. (2023.12.13 minjae)
        //            // 체크인 요청한 컨테이너 아이디를 가지고 Rest API 6.컨테이너 목록 조회 호출
        //            if (false == string.IsNullOrWhiteSpace(SelectedFolderId))
        //                await ContainerListCreateAsync(ContainerPack, SelectedFolderId);
        //            // var containerList = await ExplorerRestServer.GetContainerListAsync(client, ProjectPack, folderId, tokenKey);
        //            else throw new Exception("선택(클릭)한 폴더 ID 정보가 존재하지 않습니다.\r\n담당자에게 문의하시기 바랍니다.");

        //            // 체크인 처리 중 또는 파일 변환 처리 완료
        //            IsCheckInWorking = false;
        //            IsTranslateWorking = false;

        //            //MessageBox.Show("체크인 완료!");

        //            deleteContainerId = checkInDatas[(int)EnumCheckIn.CONTAINERID];

        //            // TODO : 체크인 완료(+ 컨테이너에 속한 파일 웹서버로 업로드) 후 파일 삭제 기능이 실행될 수 있도록 삭제 할 파일의 상위 폴더 경로 "deleteDirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2024.08.30 minjae)
        //            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //            // 삭제할 파일의 상위 폴더 전체 경로 예시 - "C:\Users\bhjeon\Documents\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002"
        //            // SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 사용
        //            deleteDirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Assembly.GetEntryAssembly().GetName().Name}\\{ContainerPack.projectId}\\{ContainerPack.teamId}\\{ContainerPack.containerId}";

        //            // 삭제할 파일의 상위 컨테이터 폴더(ContainerPack.containerId)가 존재하는 경우 
        //            if (true == Directory.Exists(deleteDirPath))
        //            {
        //                //MessageBox.Show($"로컬에 존재하는 루트 컨테이너 폴더({deleteContainerId})\r\n다운로드 받은 파일 삭제 진행합니다.");

        //                // TODO : 체크인 완료 후(+ 파일 웹서버 업로드 완료) 로컬 PC에 다운로드 받은 기존 루트 컨테이너 폴더(deleteContainerId) 및 폴더에 속한 모든 파일 삭제 
        //                // TODO : 폴더(디렉토리) 클래스 "Directory", "DirectoryInfo" 차이점 비교 후 static 클래스 "Directory" 사용 (2024.08.30 minjae)
        //                // 참고 URL - https://nomad-programmer.tistory.com/207
        //                // TODO : Directory.Delete 메서드의 2번째 파라미터 값 설정 구현 (2024.08.30 minjae)
        //                //        deleteDirPath 경로의 폴더(디렉터리), 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거하려면 true, 제거하지 않으려면 false 설정
        //                //        true로 설정하면 해당 폴더의 하위 폴더(디렉터리) 및 파일을 제거할 때, 재귀적으로 (recursive) Directory.Delete 메서드 실행 
        //                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.io.directory.delete?view=net-8.0
        //                Directory.Delete(deleteDirPath, true);

        //                // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 완료한 경우 
        //                if (false == Directory.Exists(deleteDirPath))
        //                    MessageBox.Show($"체크인 완료!\r\n루트 컨테이너 폴더 ({deleteContainerId})\r\n다운로드 받은 파일 삭제 완료");
        //                // 루트 컨테이너 폴더(deleteContainerId) + 다운로드 받은 파일 삭제 실패한 경우 
        //                else
        //                    throw new Exception($"[오류] 루트 컨테이너 폴더 ({deleteContainerId})\r\n삭제 오류\r\n담당자에게 문의하세요.");
        //            }
        //        }
        //        else
        //            throw new Exception("[오류] 컨테이너 체크인 + 업로드 파일이 존재하지 않습니다.\r\n담당자에게 문의하세요.");

        //        //return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);

        //        if (true == IsCheckInWorking || true == IsTranslateWorking)
        //        {
        //            IsCheckInWorking = false;
        //            IsTranslateWorking = false;
        //        }
        //    }
        //    finally
        //    {
        //        loadingWorkVM.RequestClose(null);   // 체크인 처리 중 또는 파일 변환 처리 중 화면 종료 
        //    }
        //    return;
        //}

        #endregion Sample
    }
}
