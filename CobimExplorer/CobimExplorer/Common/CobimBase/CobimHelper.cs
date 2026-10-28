using System;
using System.Reflection;

namespace CobimExplorer.Common.CobimBase
{
    public class CobimHelper
    {
        #region 공통

        #region 어셈블리

        /// <summary>
        /// 어셈블리(.dll 파일) 이름 - CobimExplorer
        /// </summary>
        public static string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// 어셈블리(.dll 파일) 경로 - CobimExplorer
        /// </summary>
        public static string AssemblyFilePath = Assembly.GetExecutingAssembly().Location;

        #endregion 어셈블리

        #region 로그 파일

        /// <summary>
        /// 로그 파일 갯수 설정 (테스트용 30일 이전)
        /// </summary>
        public const int LogFileCountLimit = 30;

        #endregion 로그 파일

        #region 메소드 타입

        /// <summary>
        /// 반환형 void 메소드 타입
        /// </summary>
        public const string VoidMethodType = "void";

        /// <summary>
        /// 비동기 메소드 타입
        /// </summary>
        public const string AsyncMethodType = "async";

        // TODO : 이벤트 메소드 타입 const string 객체 "EventMethodType" 필요시 사용 예정 (2023.12.12 minjae)
        /// <summary>
        /// 이벤트 메소드 타입
        /// </summary>
        // public const string EventMethodType = "event";

        #endregion 메소드 타입

        #region 폴더(디렉토리) 경로 

        /// <summary>
        /// 상위 폴더 (디렉토리) 이름
        /// </summary>
        public static string CobimBaseDirectory = "COBIM";

        /// <summary>
        /// 폴더 (디렉토리) 공통 경로 
        /// (응용 프로그램 - CobimExplorer.exe, Revit 애드인 프로그램 - CobimDockableDialogs.dll, AutoCAD 애드인 프로그램 - CobimPalette.dll)
        /// </summary>
        public static string CommonDirectoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{CobimBaseDirectory}\\{AssemblyName}";

        /// <summary>
        /// 로그(Logs) 폴더(디렉토리) 경로 
        /// </summary>
        public static string LogDirectoryPath = $"{CommonDirectoryPath}\\Logs";

        #endregion 폴더(디렉토리) 경로

        #region MessageBox 타이틀

        /// <summary>
        /// 타이틀 - 테스트
        /// </summary>
        public const string TestTitle = "테스트";

        /// <summary>
        /// 타이틀 - 완료 
        /// </summary>
        public const string CompletedTitle = "완료";

        /// <summary>
        /// 타이틀 - 오류  
        /// </summary>
        public const string ErrorTitle = "오류";

        /// <summary>
        /// 타이틀 - 알림
        /// </summary>
        public const string NoticeTitle = "알림";

        #endregion MessageBox 타이틀

        #endregion 공통

        #region TitleBarButtonBehavior

        // TODO : 아래 주석친 코드 필요시 참고 (2024.08.06 minjae) 
        /// <summary>
        /// Title Button - 최소화(Min)
        /// </summary>
        public const string MinBtn = "Min";

        /// <summary>
        /// Title Button - 최대화(Max)
        /// </summary>
        public const string MaxBtn = "Max";

        /// <summary>
        /// Title Button - 종료(Exit)
        /// </summary>
        public const string ExitBtn = "Exit";

        #endregion TitleBarButtonBehavior

        #region TestMainVM

        /// <summary>
        /// StartVM
        /// </summary>
        public const string StartVMName = "StartVM";
        // public const string StartVMName = (const)EnumDisplayType.StartVMName.ToDescription();

        /// <summary>
        /// 작업표시룰 하단 CobimExplorer 아이콘 버튼 타입
        /// </summary>
        public const string IconBtnType = "IconBtn";

        #endregion TestMainVM

        #region TestVM

        /// <summary>
        /// TestVM
        /// </summary>
        public const string TestVMName = "TestVM";

        #endregion TestVM

        #region MainVM

        #region TitleBar 

        /// <summary>
        /// 어셈블리 이름(프로젝트 파일 이름) - CobimExplorer 
        /// </summary>
        public static string Cobim_AssemblyName = $"{Assembly.GetEntryAssembly().GetName().Name}";

        // TODO : CobimExplorer 응용 프로그램 상단 타이틀 바에 적용할 Title Text 문자열에 필요한 어셈블리 버전 string 객체 "CDE_AssemblyVersion" 구현 (2023.11.09 minjae)
        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-%EC%96%B4%EC%85%88%EB%B8%94%EB%A6%AC-%EB%B2%84%EC%A0%84-%EA%B5%AC%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95
        /// <summary>
        /// 어셈블리 버전(프로젝트 파일 버전) - CobimExplorer
        /// </summary>
        public static string Cobim_AssemblyVersion = $"{Assembly.GetEntryAssembly().GetName().Version.Major}.{Assembly.GetEntryAssembly().GetName().Version.Minor}";

        /// <summary>
        /// Title Text - CobimExplorer  
        /// </summary>
        public static string MainTitle = $"{Cobim_AssemblyName}_Beta Ver {Cobim_AssemblyVersion}";

        /// <summary>
        /// CobimExplorer - 로그인 접속자 환영 메시지
        /// </summary>
        public const string WelcomeBack = "Welcome back";

        #endregion TitleBar

        #endregion MainVM

        #region TabParentPageVM

        /// <summary>
        /// TabParentPageVM
        /// </summary>
        public const string TabParentPageVMName = "TabParentPageVM";

        /// <summary>
        /// ExplorerVM - TabItem 이름 
        /// </summary>
        public const string ExplorerTabItemName = "성과품 조회";

        /// <summary>
        /// HubBoxVM - TabItem 이름 
        /// </summary>
        public const string HubBoxTabItemName = "ACC 변환 내역";

        /// <summary>
        /// ExplorerVM - 탐색기 페이지
        /// </summary>
        public const string ExplorerVMName = "ExplorerVM";

        /// <summary>
        /// TestWebBrowserVM - 인터넷 익스플로러(Interner Explorer) 웹 브라우저 
        /// </summary>
        public const string TestWebBrowserVMName = "TestWebBrowserVM";

        /// <summary>
        /// TestCefSharpVM - 구글 크롬(Google Chrome) 웹 브라우저
        /// </summary>
        public const string TestCefSharpVMName = "TestCefSharpVM";

        /// <summary>
        /// HubBoxVM - HubBox 관리자 페이지(Admin-1)
        /// </summary>
        public const string HubBoxVMName = "HubBoxVM";

        /// <summary>
        /// 테스트 TabItem 신규 추가 하고자 하는 뷰모델 이름 리스트에 뷰모델 이름 존재
        /// </summary>
        public const int VMNameExist = 1;

        /// <summary>
        /// 테스트 TabItem 신규 추가 하고자 TabItem 존재
        /// </summary>
        public const int TabItemExist = 1;

        /// <summary>
        /// 화면상에 출력 되는 테스트 TabItem 인덱스 (탐색기 화면(인덱스 0))
        /// </summary>
        public const int TestSelectedTabIndex = 0;

        /// <summary>
        /// TabItem 닫기 요청 이벤트 핸들러(Tabs_CollectionChanged) 가입/해제 처리
        /// 대상 TabItem Index
        /// </summary>
        public const int TargetTabItemIndex = 0;


        #endregion TabParentPageVM

        #region LoginVM

        public const string Login_LoadingWork = "로그인 처리 중입니다...";

        #endregion LoginVM

        #region ExplorerVM

        public const string ExpTitle = "성과품 조회";

        #region RibbonControl

        /// <summary>
        /// CDE 리본 탭 - Header
        /// </summary>
        public const string CDE_RibbonTab = "성과품";

        /// <summary>
        /// CDE 리본 그룹 - Header
        /// </summary>
        public const string CDE_RibbonGroup = "성과품(CDE)";

        /// <summary>
        /// CDE 리본 버튼 - Text
        /// </summary>
        public const string CDE_RibbonButton = "성과품(CDE)";

        /// <summary>
        /// HubBox 리본 그룹 - Header
        /// </summary>
        public const string HubBox_RibbonGroup = "HubBox";

        /// <summary>
        /// HubBox 리본 버튼 - Text
        /// </summary>
        public const string HubBox_RibbonButton = "HubBox";

        /// <summary>
        /// 리본 버튼 타입 
        /// </summary>
        public const string RibbonBtnType = "RibbonBtn";

        #endregion RibbonControl

        #region NavigationBar

        /// <summary>
        /// CDE 내비게이션 바 버튼 - Text
        /// </summary>
        public const string CDE_NavButton = "성과품(CDE)";

        /// <summary>
        /// HubBox 내비게이션 바 버튼 - Text
        /// </summary>
        public const string HubBox_NavButton = "HubBox";

        #endregion NavigationBar

        // TODO : 화면 셋팅 - OnInitialActivate const string 객체 "TypeOfOnInitialActivate" 필요시 사용 예정 (2023.11.09 minjae)
        /// <summary>
        /// 화면 셋팅 - OnInitialActivate
        /// </summary>
        public const string TypeOfOnInitialActivate = "OnInitialActivate";

        /// <summary>
        /// 화면 셋팅 - OnActivate
        /// </summary>
        public const string TypeOfOnActivate = "OnActivate";

        /// <summary>
        /// 화면 셋팅 - ExplorerListCreateAsync
        /// </summary>
        public const string TypeOfExplorerListCreateAsync = "ExplorerListCreateAsync";

        /// <summary>
        /// 화면 셋팅 - ContainerSearch
        /// </summary>
        // public const string TypeOfContainerSearch = "ContainerSearch";

        /// <summary>
        /// 화면 셋팅 - ContainerDetailSearch
        /// </summary>
        public const string TypeOfDetailSearch = "DetailSearch";

        /// <summary>
        /// 화면 셋팅 - LinkedNodeInfoCreate
        /// </summary>
        public const string TypeOfLinkedNodeInfoCreate = "LinkedNodeInfoCreate";

        /// <summary>
        /// 화면 셋팅 - CheckOut
        /// </summary>
        // public const string TypeOfCheckOut = "CheckOut";

        /// <summary>
        /// 화면 셋팅 - CheckIn
        /// </summary>
        // public const string TypeOfCheckIn = "CheckIn";

        /// <summary>
        /// 화면 셋팅 - ContainerListCreateAsync
        /// </summary>
        public const string TypeOfContainerListCreateAsync = "ContainerListCreateAsync";

        /// <summary>
        /// 루트 폴더 인덱스(Root Folder Index)
        /// </summary>
        public const int RootFolderIndex = 0;

        /// <summary>
        /// 컨테이너 리스트에 데이터가 존재하지 않는 경우 
        /// </summary>
        public const int ContainerNotExist = 0;

        /// <summary>
        /// 파일 다운로드 대기 처리 
        /// </summary>
        public const string DownLoad_LoadingWork = "파일 다운로드 처리 중입니다...";

        /// <summary>
        /// 테스트 대기 처리
        /// </summary> 
        public const string Test_LoadingWork = "테스트 처리 중입니다...";

        /// <summary>
        /// 체크아웃 대기 처리
        /// </summary>
        public const string CheckOut_LoadingWork = "체크아웃 처리 중입니다...";

        /// <summary>
        /// 체크인 대기 처리
        /// </summary>
        public const string CheckIn_LoadingWork = "체크인 처리 중입니다...";

        /// <summary>
        /// 체크인 Comment 메시지 공백 알림
        /// </summary>
        public const string CheckIn_CommentWhiteSpace = "Comment 메세지가 비었습니다." +
                                                        "\r\nComment 메세지 입력 후 다시 시도하시기 바랍니다.";

        /// <summary>
        /// 연관 컨테이너에 속한 파일 다운로드 대기 처리 
        /// </summary>
        public const string LinkedContainer_LoadingWork = "연관 컨테이너에 속한\r\n파일 다운로드 처리 중입니다...";

        /// <summary>
        /// 파일 변환 요청 대기 처리
        /// </summary>
        public const string Translate_LoadingWork = "파일 변환 요청 중입니다...";

        /// <summary>
        /// 컨테이너 작업 취소 대기 처리 
        /// </summary>
        public const string CancelWork_LoadingWork = "컨테이너 작업 취소\r\n처리 중입니다...";

        /// <summary>
        /// 파일열기 대기 처리 
        /// </summary>
        public const string OpenFile_LoadingWork = "파일 열기 처리 중입니다...";

        /// <summary>
        /// 파일열기 가능한 Revit 파일 확장자 이름
        /// </summary>
        public const string RVTExtensionName = ".rvt";

        /// <summary>
        /// 파일열기 가능한 AutoCAD 파일 확장자 이름
        /// </summary>
        public const string DWGExtensionName = ".dwg";

        /// <summary>
        /// 파일열기 가능한 AutoCAD 파일 확장자 이름
        /// </summary>
        public const string DXFExtensionName = ".dxf";

        #endregion ExplorerVM

        #region CheckInPartVM

        /// <summary>
        /// 체크인 팝업화면 Title Text   
        /// </summary>
        public const string CheckIn_Title = "체크인_업로드";

        #endregion CheckInPartVM

        #region Sample

        ///// <summary>
        ///// TestVM
        ///// </summary>
        //public const string TestVMName = "TestVM";

        ///// <summary>
        ///// TestExplorerVM
        ///// </summary>
        //public const string TestExpVMName = "TestExplorerVM";

        ///// <summary>
        ///// TestLazyTreeVM
        ///// </summary>
        //public const string TestLazyTrVMName = "TestLazyTreeVM";

        #region 컨텍스트(ContextMenu) 메뉴 Header

        // TODO : 컨텍스트(ContextMenu) 메뉴 Header const string 객체 "TestHeader" 필요시 사용 예정 (2023.12.13 minjae)
        /// <summary>
        /// 테스트
        /// </summary> 
        // public const string TestHeader = "테스트";

        // TODO : 컨텍스트(ContextMenu) 메뉴 Header const string 객체 "CheckOutHeader" 필요시 사용 예정 (2023.12.13 minjae)
        /// <summary>
        /// 체크아웃
        /// </summary> 
        // public const string CheckOutHeader = "체크아웃";

        // TODO : 컨텍스트(ContextMenu) 메뉴 Header const string 객체 "CheckInHeader" 필요시 사용 예정 (2023.12.13 minjae)
        /// <summary>
        /// 체크인
        /// </summary> 
        // public const string CheckInHeader = "체크인";

        // TODO : 컨텍스트(ContextMenu) 메뉴 Header const string 객체 "CancelWorkHeader" 필요시 사용 예정 (2023.12.13 minjae)
        /// <summary>
        /// 작업취소
        /// </summary> 
        // public const string CancelWorkHeader = "작업취소";

        #endregion 컨텍스트(ContextMenu) 메뉴 Header

        #endregion Sample
    }
}
