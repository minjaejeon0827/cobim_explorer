using System.IO;
using System.Windows;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel;

using CobimExplorer.Data.Models.CobimBase.Login;
using CobimNet;

namespace CobimExplorer.Data.Models.CobimBase.Explorer
{
    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    public class ExplorerView<TData> : ExplorerView where TData : ExplorerView<TData> { }

    /// <summary>
    /// CobimExplorer
    /// 탐색기 화면 뷰모델(ExplorerVM.cs) 데이터 모델
    /// </summary>
    public class ExplorerView : TExplorerView<ExplorerView>
    {
        #region 프로퍼티

        /// <summary>
        /// HttpClient client (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        private HttpClient _Client { get; set; }

        /// <summary>
        /// 폴더 (디렉토리) 공통 경로 
        /// (응용 프로그램 - CobimExplorer.exe, Revit 애드인 프로그램 - CobimDockableDialogs.dll, AutoCAD 애드인 프로그램 - CobimPalette.dll)
        /// </summary>
        private string _CommonDirectoryPath { get; set; }

        /// <summary>
        /// Http 서버 통신 요청 Header
        /// </summary>
        private RequestHeader _RequestHeader { get; set; }

        /// <summary>
        /// 컨테이너 정보 
        /// </summary>
        private ContainerPack _ContainerPack { get; set; }

        /// <summary>
        /// 다운로드할 파일 정보
        /// </summary>
        private FilePack _FilePack { get; set; }

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 정보
        /// </summary>
        private CheckInPack _CheckInPack { get; set; }

        /// <summary>
        /// 컨테이너 작업 취소 정보
        /// </summary>
        private CancelWorkPack _CancelWorkPack { get; set; }

        /// <summary>
        /// HubBox 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보 
        /// </summary>
        private WebViewerPack _WebViewerPack { get; set; }

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 성공시 
        /// HubBox 파일 변환 Rest API 요청시 필수 정보
        /// </summary>
        private TranslatePack _TranslatePack { get; set; }

        /// <summary>
        /// 컨테이너에 속한 파일(확장자 .rvt) 
        /// Revit 응용 프로그램 열기 작업시 필수 정보 
        /// </summary>
        // private OpenFilePack _OpenFilePack { get; set; }

        #endregion 프로퍼티

        #region 생성자

        // TODO : 생성자 "ExplorerView()" 탐색기 화면 뷰모델(ExplorerVM.cs)에서 필요시 호출해서 ExplorerView 클래스 객체 생성 예정 (2023.11.21 jbh)
        public ExplorerView() : base() { }

        #endregion 생성자

        #region SetData

#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고

        // TODO : 아래 주석친 메서드 "SetData" 필요시 참고 (2024.10.31 jbh)
        /// <summary>
        /// 데이터 모델 클래스 CefSharpExplorerView.cs에 속하는
        /// 데이터 프로퍼티에 값 셋팅 (또는 값 복제)
        /// </summary>
        public void SetData(RequestHeader requestHeader, ContainerPack? containerPack = null, FilePack? filePack = null, CheckInPack? checkInPack = null)
        {
            // this.SetHttpClient(client);
            this.SetRequestHeader(requestHeader);
            this.SetContainerPack(containerPack);
            this.SetFilePack(filePack);
            this.SetCheckInPack(checkInPack);
        }

#nullable disable  // nullable 전처리 비활성화

        #endregion SetData

        #region SetCancelWorkData

        /// <summary>
        /// 탐색기 화면 데이터 모델 클래스 ExplorerView.cs에 속하는
        /// 데이터 프로퍼티 값 셋팅 (또는 값 복제)
        /// "_RequestHeader - (Http 서버 통신 요청 Header)",
        /// "_CancelWorkPack - (컨테이너 작업 취소 정보)" 
        /// </summary>
        public void SetCancelWorkData(RequestHeader requestHeader, CancelWorkPack cancelWorkPack)
        {
            this.SetRequestHeader(requestHeader);
            this.SetCancelWorkPack(cancelWorkPack);
        }

        #endregion SetCancelWorkData

        #region HttpClient client

        /// <summary>
        /// HttpClient client 값 셋팅 
        /// </summary>
        public void SetHttpClient(HttpClient client)
        {
            _Client = client;
        }

        /// <summary>
        /// HttpClient client 가져오기 
        /// </summary>
        public HttpClient GetHttpClient()
        {
            return _Client;
        }

        #endregion HttpClient client

        #region 폴더 (디렉토리) 공통 경로

        /// <summary>
        /// 폴더 (디렉토리) 공통 경로 값 셋팅 
        /// (응용 프로그램 - CobimExplorer.exe, Revit 애드인 프로그램 - CobimDockableDialogs.dll, AutoCAD 애드인 프로그램 - CobimPalette.dll)
        /// </summary>
        public void SetCommonDirectoryPath(string commonDirectoryPath)
        {
            _CommonDirectoryPath = commonDirectoryPath;
        }

        /// <summary>
        /// 폴더 (디렉토리) 공통 경로 가져오기
        /// (응용 프로그램 - CobimExplorer.exe, Revit 애드인 프로그램 - CobimDockableDialogs.dll, AutoCAD 애드인 프로그램 - CobimPalette.dll)
        /// </summary>
        public string GetCommonDirectoryPath()
        {
            return _CommonDirectoryPath;
        }

        #endregion 폴더 (디렉토리) 공통 경로

        #region Http 웹서버 통신 요청 Header

        /// <summary>
        /// Http 웹서버 통신 요청 Header 값 셋팅 
        /// </summary>
        public void SetRequestHeader(RequestHeader requestHeader)
        {
            _RequestHeader = requestHeader;
        }

        /// <summary>
        /// Http 웹서버 통신 요청 Header 가져오기 
        /// </summary>
        public RequestHeader GetRequestHeader()
        {
            return _RequestHeader;
        }

        #endregion Http 웹서버 통신 요청 Header

        #region 컨테이너 정보 

        /// <summary>
        /// 컨테이너 정보 값 셋팅 
        /// </summary>
        public void SetContainerPack(ContainerPack containerPack)
        {
            _ContainerPack = containerPack;
        }

        /// <summary>
        /// 컨테이너 정보 가져오기 
        /// </summary>
        public ContainerPack GetContainerPack()
        {
            return _ContainerPack;
        }

        #endregion 컨테이너 정보 

        #region 다운로드할 파일 정보

        /// <summary>
        /// 다운로드할 파일 정보 값 셋팅 
        /// </summary>
        public void SetFilePack(FilePack filePack)
        {
            _FilePack = filePack;
        }

        /// <summary>
        /// 다운로드할 파일 정보 가져오기 
        /// </summary>
        public FilePack GetFilePack()
        {
            return _FilePack;
        }

        #endregion 다운로드할 파일 정보

        #region 컨테이너 체크인 + 파일 웹서버 업로드 정보

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 정보 값 셋팅 
        /// </summary>
        public void SetCheckInPack(CheckInPack checkInPack)
        {
            _CheckInPack = checkInPack;
        }

        /// <summary>
        /// 컨테이너 체크인 + 파일 웹서버 업로드 정보 가져오기 
        /// </summary>
        public CheckInPack GetCheckInPack()
        {
            return _CheckInPack;
        }

        #endregion 컨테이너 체크인 + 파일 웹서버 업로드 정보

        #region 컨테이너 작업 취소 정보

        /// <summary>
        /// 컨테이너 작업 취소 정보 값 셋팅 
        /// </summary>
        public void SetCancelWorkPack(CancelWorkPack cancelWorkPack)
        {
            _CancelWorkPack = cancelWorkPack;
        }

        /// <summary>
        /// 컨테이너 작업 취소 정보 가져오기 
        /// </summary>
        public CancelWorkPack GetCancelWorkPack()
        {
            return _CancelWorkPack;
        }


        #endregion 컨테이너 작업 취소 정보 

        #region HubBox 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보

        /// <summary>
        /// HubBox 컨테이너에 속한 파일 웹뷰어 정보 값 세팅 
        /// </summary>
        public void SetWebViewerPack(WebViewerPack webViewerPack)
        {
            _WebViewerPack = webViewerPack;
        }

        /// <summary>
        /// HubBox 컨테이너에 속한 파일 웹뷰어 정보 가져오기 
        /// </summary>
        public WebViewerPack GetWebViewerPack()
        {
            return _WebViewerPack;
        }

        #endregion HubBox 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보

        #region HubBox 파일 변환 Rest API 요청시 필수 정보

        /// <summary>
        /// HubBox 파일 변환 정보 값 셋팅 
        /// </summary>
        public void SetTranslatePack(TranslatePack translatePack)
        {
            _TranslatePack = translatePack;
        }

        /// <summary>
        /// HubBox 파일 변환 정보 가져오기
        /// </summary>
        public TranslatePack GetTranslatePack()
        {
            return _TranslatePack;
        }

        #endregion HubBox 파일 변환 Rest API 요청시 필수 정보

        #region 컨테이너에 속한 파일(확장자 .rvt) Revit 응용 프로그램 열기 작업시 필수 정보 

        // TODO : 필요시 메서드 "SetOpenFilePack" 사용 예정 (2024.10.29 jbh)
        /// <summary>
        /// 컨테이너에 속한 파일(확장자 .rvt) Revit 응용 프로그램 열기 작업시 필수 정보 값 셋팅
        /// </summary>
        // public void SetOpenFilePack(OpenFilePack openFilePack)
        // {
        //     _OpenFilePack = openFilePack;
        // }

        // TODO : 필요시 메서드 "GetOpenFilePack" 사용 예정 (2024.10.29 jbh)
        /// <summary>
        /// 컨테이너에 속한 파일(확장자 .rvt) Revit 응용 프로그램 열기 작업시 필수 정보 가져오기 
        /// </summary>
        // public OpenFilePack GetOpenFilePack()
        // {
        //     return _OpenFilePack;
        // }

        #endregion 컨테이너에 속한 파일(확장자 .rvt) Revit 응용 프로그램 열기 작업시 필수 정보 
    }

    #region ExplorerVM

    #region EnumFolder

    public enum EnumFolder : int
    {
        [Description("폴더_부재")]
        NONE = 0,

        [Description("폴더_존재")]
        EXISTENCE = 1
    }


    #endregion EnumFolder

    #region EnumContainer

    public enum EnumContainer : int
    {
        [Description("프로젝트_아이디")]
        PROJECTID = 0,
        [Description("팀_아이디")]
        TEAMID = 1,
        [Description("폴더_아이디")]
        FOLDERID = 2
    }

    #endregion EnumContainer

    #region EnumContainerDetail

    public enum EnumContainerDetail : int
    {
        [Description("프로젝트_아이디")]
        PROJECTID = 0,
        [Description("팀_아이디")]
        TEAMID = 1,
        [Description("컨테이너_아이디")]
        CONTAINERID = 2
    }

    #endregion EnumContainerDetail

    #region EnumDownLoad

    public enum EnumDownLoad : int
    {
        [Description("projectId")]   // 프로젝트_아이디
        PROJECTID = 0,
        [Description("teamId")]      // 팀_아이디
        TEAMID = 1,
        [Description("containerId")] // 컨테이너_아이디
        CONTAINERID = 2,
        [Description("fileId")]      // 파일_아이디
        FILEID = 3,
        [Description("fileNm")]      // 파일_이름
        FILENAME = 4,
        [Description("Tenant")]      // 테넌트 아이디
        TENANT = 5,
        [Description("Authorization")]   // 로그인 토큰 키   
        AUTHORIZATION = 6,
        [Description("Access")]    // 접속자 아이디 (Access Id, accessKey 같은 값을 의미, 로그인 아이디와는 다름)
        ACCESS = 7,
        [Description("Refresh")]   // Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 
        REFRESH = 8,
    }

    #endregion EnumDownLoad

    #region EnumLinkedRelation

    // TODO : enum 열거형 구조체 "EnumLinkedRelation" 변수 "EXISTENCE" 필요시 사용 예정 (2023.11.29 jbh)
    /// <summary>
    /// 참조/연관 컨테이너 관계 표현 
    /// </summary>
    public enum EnumLinkedRelation : int
    {
        [Description("참조_연관컨테이너_부재")]
        NONE = 0,

        [Description("참조_연관컨테이너_존재")]
        EXISTENCE = 1
    }

    #endregion EnumLinkedRelation

    #region EnumCheckOut

    public enum EnumCheckOut : int
    {
        [Description("projectId")]   // 프로젝트_아이디
        PROJECTID = 0,
        [Description("teamId")]      // 팀_아이디
        TEAMID = 1,
        [Description("containerId")] // 체크아웃_컨테이너_아이디
        CONTAINERID = 2,

        // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃" 클릭시 멀티 바인딩 대상 중 "exp_FolderId" 제외함.
        //        제외한 사유는 "exp_FolderId" 설정시 탐색기 뷰모델(ExplorerVM.cs) -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync"
        //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로 해당 프로퍼티는 멀티 바인딩 대상에서 제외 처리함. (2023.12.13 jbh) 
        //        하여 아래 enum 열거형 구조체 EnumCheckOut 멤버 변수 FOLDERID 또한 사용 제외 (2023.12.13 jbh)
        // [Description("folderId")]   // 폴더_아이디
        // FOLDERID = 3,
        [Description("fileId")]   // 파일_아이디
        FILEID = 3,
        [Description("fileNm")]   // 파일명
        FILENAME = 4,
        [Description("Tenant")]   // 테넌트 아이디
        TENANT = 5,
        [Description("Authorization")]   // 로그인 토큰 키   
        AUTHORIZATION = 6,
        [Description("Access")]    // 접속자 아이디 (Access Id, accessKey 같은 값을 의미, 로그인 아이디와는 다름)
        ACCESS = 7,
        [Description("Refresh")]   // Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 
        REFRESH = 8
    }

    #endregion EnumCheckOut

    #region EnumCheckOutResult

    /// <summary>
    /// 체크아웃 실행 결과
    /// </summary>
    public enum EnumCheckOutResult : int
    {
        [Description("체크아웃_실패")]
        FAILURE = 0,
        [Description("체크아웃_성공")]
        SUCCESS = 1
    }

    #endregion EnumCheckOutResult

    #region EnumCheckIn

    public enum EnumCheckIn : int
    {
        [Description("projectId")]   // 프로젝트_아이디
        PROJECTID = 0,
        [Description("teamId")]      // 팀_아이디
        TEAMID = 1,
        [Description("containerId")] // 컨테이너_아이디
        CONTAINERID = 2,
        [Description("folderId")]    // 폴더_아이디
        FOLDERID = 3,
        // TODO : enum 열거형 구조체 EnumCheckIn 멤버 변수 "UPLOADFILES" 필요시 사용 예정 (2023.12.05 jbh)
        // [Description("uploadFiles")]  // 업로드_파일
        // UPLOADFILES = 4,
        [Description("fileId")]      // 파일_아이디
        FILEID = 4,
        // TODO : 추후 체크인 구현시 업로드 하려는 파일명(UPLOAD_FILENAME)과 기존 체크인 된 파일명(FILENAME)이 다를 경우
        //        enum 열거형 구조체 EnumCheckIn 멤버 변수 "FILENAME" 사용 예정 (2023.12.05 jbh)
        [Description("fileNm")]      // 파일명
        FILENAME = 5,

        // TODO : enum 열거형 구조체 EnumCheckIn 멤버 변수 "FILESIZE" 필요시 사용 예정 (2023.12.05 jbh)
        // [Description("fileSize")]     // 파일_사이즈
        // FILESIZE = 6                  
        [Description("revsnVal")]        // 업로드_전_파일_버전
        REVSNVAL = 6,                    
        [Description("title")]           // 제목
        TITLE = 7,
        [Description("Tenant")]          // 테넌트 아이디
        TENANT = 8,
        [Description("Authorization")]   // 로그인 토큰 키   
        AUTHORIZATION = 9,
        [Description("comment")]   // comment
        COMMENT = 10,
        [Description("Access")]    // 접속자 아이디 (Access Id, accessKey 같은 값을 의미, 로그인 아이디와는 다름)
        ACCESS = 11,
        [Description("Refresh")]   // Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 
        REFRESH = 12
    }

    #endregion EnumCheckIn

    #region EnumCancelWork

    public enum EnumCancelWork : int
    {
        [Description("projectId")]   // 프로젝트_아이디
        PROJECTID = 0,
        [Description("teamId")]      // 팀_아이디
        TEAMID = 1,
        [Description("containerId")] // 작업취소_컨테이너_아이디
        CONTAINERID = 2,
        [Description("fileNm")]   // 파일명
        FILENAME = 3,
        // TODO : enum 열거형 구조체 EnumCancelWork 멤버 변수 "WORKUSERID" 필요시 사용 예정 (2023.12.20 jbh)
        // [Description("작업중인_사용자_아이디")]
        // WORKUSERID = 3,
        [Description("Tenant")]   // 테넌트 아이디
        TENANT = 4,
        [Description("Authorization")]   // 로그인 토큰 키   
        AUTHORIZATION = 5,
        [Description("Access")]    // 접속자 아이디 (Access Id, accessKey 같은 값을 의미, 로그인 아이디와는 다름)
        ACCESS = 6,
        [Description("Refresh")]   // Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 
        REFRESH = 7
    }

    #endregion EnumCancelWork

    #region EnumOpenFile

    public enum EnumOpenFile : int
    {
        [Description("프로젝트_아이디")]
        PROJECTID = 0,
        [Description("팀_아이디")]
        TEAMID = 1,
        [Description("파일열기_컨테이너_아이디")]
        CONTAINERID = 2,
        [Description("파일명")]
        FILENAME = 3,
    }

    #endregion EnumOpenFile

    // TODO : TreeView Multi Level (프로젝트 - 팀 - 폴더 - 서브폴더 / 파일) 구현 (2023.09.18 jbh)
    // 참고 URL - https://stackoverflow.com/questions/50760316/wpf-treeview-with-three-levels-in
    /// <summary>
    /// 프로젝트, 팀, 폴더, 연관 컨테이너 정보 Tree interface
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// 프로젝트, 팀, 폴더, 서브폴더 이름, 연관 컨테이너 이름 (파일 이름 제외)
        /// </summary>
        string exp_TreeName { get; set; }

        /// <summary>
        /// 프로젝트 하위 자식 (팀, 루트 폴더, 서브폴더) 노드
        /// </summary>
        List<ITreeNode> ChildNodes { get; set; }
    }

    /// <summary>
    /// 프로젝트 
    /// </summary>
    public class ProjectView : ITreeNode
    {
        /// <summary>
        /// 프로젝트 이름 
        /// </summary>
        public string exp_TreeName { get; set; }

        /// <summary>
        /// 프로젝트 하위 자식 (팀) 
        /// </summary>
        public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
        //public List<ITreeNode> ChildNodes
        //{
        //    get
        //    {
        //        // 싱글톤 패턴 
        //        if (_ChildNodes is null)
        //        {
        //            _ChildNodes = new List<ITreeNode>();
        //        }
        //        return _ChildNodes;
        //    }
        //    set => _ChildNodes = value;
        //}
        //private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 프로젝트 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> ProjectItems { get; set; }

        /// <summary>
        /// 프로젝트 메타 데이터 Dictionary
        /// </summary>
        // public Dictionary<string, object> ProjectDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> ProjectDataInfo { get; set; }

        #region 생성자

        public ProjectView() 
        {

        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    /// <summary>
    /// 팀
    /// </summary>
    public class TeamView : ITreeNode
    {
        /// <summary>
        /// 팀 이름 
        /// </summary>
        public string exp_TreeName { get; set; }

        /// <summary>
        /// 프로젝트 하위 자식 (루트 폴더) 
        /// </summary>
        public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
        //public List<ITreeNode> ChildNodes
        //{
        //    get
        //    {
        //        // 싱글톤 패턴 
        //        if (_ChildNodes is null)
        //        {
        //            _ChildNodes = new List<ITreeNode>();
        //        }
        //        return _ChildNodes;
        //    }
        //    set => _ChildNodes = value;
        //}
        //private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 팀 아이디
        /// </summary>
        public string exp_TeamId { get; set; }


        // TODO : 프로퍼티 "exp_TeamName" 필요시 사용 예정 (2023.10.23 jbh)
        /// <summary>
        /// 화면 우측 상단 팀 이름 출력할 때 사용할 바인딩 객체 "exp_TeamName"
        /// </summary>
        // public string exp_TeamName { get; set; }

        /// <summary>
        /// 팀 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> TeamItems { get; set; }

        /// <summary>
        /// 팀 메타 데이터 Dictionary
        /// </summary>
        //public Dictionary<string, object> TeamDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> TeamDataInfo { get; set; }

        // TODO : 소속된 팀의 사용자 리스트 필요시 사용 예정 (2023.09.18 jbh)
        /// <summary>
        /// 소속된 팀의 사용자 리스트 
        /// </summary>
        public List<Dictionary<string, object>> TeamUserList { get; set; }

        #region 생성자

        public TeamView()
        {

        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    /// <summary>
    /// 폴더
    /// </summary>
    public class FolderView : ITreeNode
    {
        /// <summary>
        /// (루트 / 서브) 폴더 이름
        /// </summary>
        public string exp_TreeName { get; set; }

        /// <summary>
        /// 폴더가 속한 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 폴더가 속한 팀 아이디 
        /// </summary>
        public string exp_TeamId { get; set; }

        /// <summary>
        /// 폴더 하위 자식(서브 폴더)
        /// </summary>
        public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
        //public List<ITreeNode> ChildNodes
        //{
        //    get
        //    {
        //        // 싱글톤 패턴 
        //        if (_ChildNodes is null)
        //        {
        //            _ChildNodes = new List<ITreeNode>();
        //        }
        //        return _ChildNodes;
        //    }
        //    set => _ChildNodes = value;
        //}
        //private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 부모 폴더 아이디
        /// </summary>
        public string exp_ParentFolderId { get; set; }

        /// <summary>
        /// 폴더 그룹 아이디
        /// </summary>
        public string exp_FolderGroupId { get; set; }

        /// <summary>
        /// 폴더 아이디
        /// </summary>
        public string exp_FolderId { get; set; }

        /// <summary>
        /// 폴더 레벨
        /// </summary>
        public string exp_FolderLevelNo { get; set; }

        /// <summary>
        /// 폴더 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> FolderItems { get; set; }

        /// <summary>
        /// 폴더 메타 데이터 Dictionary
        /// </summary>
        // public Dictionary<string, object> FolderDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> FolderDataInfo { get; set; }

        #region 생성자

        public FolderView()
        {

        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    /// <summary>
    /// 컨테이너(파일 포함)
    /// </summary>
    public class ContainerView
    {
        /// <summary>
        /// 파일 이름
        /// </summary>
        public string exp_FileName { get; set; }

        // TODO : 파일 하위 자식 "ChildNodes" 필요시 사용 예정 (2023.09.18 jbh)
        /// <summary>
        /// 파일 하위 자식 노드
        /// </summary>
        //public List<ITreeNode> ChildNodes
        //{
        //    get
        //    {
        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용(2023.11.24 jbh)
        //        참고 URL - https://husk321.tistory.com/405
        //        // 싱글톤 패턴 
        //        if (_ChildNodes is null)
        //        {
        //            _ChildNodes = new List<ITreeNode>();
        //        }
        //        return _ChildNodes;
        //    }
        //    set => _ChildNodes = value;
        //}
        //private List<ITreeNode> _ChildNodes;

        /// <summary>
        /// 파일 목록 리스트
        /// </summary>
        public List<Dictionary<string, object>> FileItems { get; set; }

        // TODO : 공유 컨테이너 리스트 "SharedContainerList" 추후 필요시 사용 예정 (2023.09.22 jbh)
        /// <summary>
        /// 공유 컨테이너 리스트
        /// </summary>
        public List<Dictionary<string, object>> SharedContainerList { get; set; }

        /// <summary>
        /// 파일 메타 데이터 Dictionary
        /// </summary>
        // public Dictionary<string, object> FileDataInfo { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> FileDataInfo { get; set; }

        /// <summary>
        /// 파일 유형 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> FileTypeCodeInfo { get; set; }

        /// <summary>
        /// 파일 단계 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> StepCodeInfo { get; set; }

        /// <summary>
        /// 파일 상태 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> StatusCodeInfo { get; set; }

        /// <summary>
        /// 파일 체크 정보 Dictionary
        /// </summary>
        public Dictionary<string, object> CheckCodeInfo { get; set; }

        /// <summary>
        /// 컨테이너 그룹 
        /// </summary>
        public string exp_ContainerGroup { get; set; }

        /// <summary>
        /// 컨테이너 ID (영어 대문자)
        /// </summary>
        public string exp_UpperContainerId { get; set; }

        /// <summary>
        /// 컨테이너 ID (영어 소문자)
        /// </summary>
        public string exp_ContainerId { get; set; }

        /// <summary>
        /// 파일이 속한 상위 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 파일이 속한 상위 팀 아이디
        /// </summary>
        public string exp_TeamId { get; set; }

        /// <summary>
        /// 파일이 속한 상위 폴더 아이디
        /// </summary>
        public string exp_FolderId { get; set; }

        /// <summary>
        /// 파일 아이디
        /// </summary>
        public string exp_FileId { get; set; }

        /// <summary>
        /// 파일 유형
        /// </summary>
        public string exp_FileTypeCode { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string exp_Title { get; set; }

        /// <summary>
        /// 작성자
        /// </summary>
        public string exp_CreateUserId { get; set; }

        /// <summary>
        /// 컨테이너(파일) 등록일시(생성일시)
        /// </summary>
        public string exp_CreateDateTime { get; set; }
        // public DateTime exp_CreateDateTime { get; set; }

        /// <summary>
        /// 작성자 + " | " + 컨테이너(파일) 등록일시(생성일시)
        /// </summary>
        public string CreateUserIdAndTime { get; set; }

        /// <summary>
        /// 유형 - fileTypeCodeInfo -> codeName
        /// </summary>
        public string exp_TypeCodeName { get; set; }

        /// <summary>
        /// 단계 - stepCodeInfo -> codeName
        /// </summary>
        public string exp_StepCodeName { get; set; }

        /// <summary>
        /// 상태 - statusCodeInfo -> codeName
        /// </summary>
        public string exp_StatusCodeName { get; set; }

        /// <summary>
        /// 버전 
        /// </summary>
        public string exp_RevsnVal { get; set; }

        /// <summary>
        /// 상태 
        /// </summary>
        public string exp_StatusCode { get; set; }

        /// <summary>
        /// 컨테이너 검토상태("comstt10": 검토 미진행, "comstt20": 검토 진행, "comstt30": 검토 완료)
        /// </summary>
        public string exp_CheckStatusCode { get; set; }

        /// <summary>
        /// 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
        /// </summary>
        public string exp_ReadYn { get; set; }

        /// <summary>
        /// 컨테이너 작업중인 사용자 아이디
        /// </summary>
        public string exp_WorkUserId { get; set; }

        /// <summary>
        /// 파일 변환 처리 상태 코드 값
        /// </summary>
        public string exp_TranslateStatusCode { get; set; }

        /// <summary>
        /// 파일 변환 처리 상태 응답 메시지 
        /// </summary>
        public string exp_TranslateStatusMsg { get; set; }

        /// <summary>
        /// 변환 처리 완료된 파일 웹뷰어 URL 주소 
        /// </summary>
        public string exp_TranslateData { get; set; }

        /// <summary>
        /// Visibility - (파일) 변환 요청 성공(success)
        /// </summary>
        public Visibility exp_VisibleWebViewerImg { get; set; }

        /// <summary>
        /// Visibility - (파일) 변환 중입니다.
        /// </summary>
        public Visibility exp_VisibleTranslateImg { get; set; }

        /// <summary>
        /// Visibility - (파일) 변환에 실패 한 파일입니다.
        /// </summary>
        public Visibility exp_VisibleFailureImg { get; set; }

        /// <summary>
        /// Visibility - (파일) 변환 내역이 없습니다.
        /// </summary>
        public Visibility exp_VisibleNotExistTranslateImg { get; set; }

        /// <summary>
        /// Visibility - (파일) 변환 오류. 
        /// </summary>
        public Visibility exp_VisibleErrorImg { get; set; }


        #region 생성자

        public ContainerView()
        {

        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    /// <summary>
    /// 컨테이너(파일 포함) 메타 데이터 정보
    /// </summary>
    public class ContainerDetailView
    {
        /// <summary>
        /// 작성자
        /// </summary>
        public string exp_CreateUserId { get; set; }

        /// <summary>
        /// 컨테이너 아이디 (영어 소문자)
        /// </summary>
        public string exp_ContainerId { get; set; }


        /// <summary>
        /// 컨테이너 ID (영어 대문자)
        /// </summary>
        // public string exp_UpperContainerId { get; set; }

        /// <summary>
        /// 선택한 특정 컨테이너 ID (영어 대문자)
        /// </summary>
        // public string exp_SelectedContainerId { get; set; }


        /// <summary>
        /// 프로젝트 아이디
        /// </summary>
        public string exp_ProjectId { get; set; }

        /// <summary>
        /// 팀 아이디
        /// </summary>
        public string exp_TeamId { get; set; }


        /// <summary>
        /// 버전
        /// </summary>
        public string exp_RevsnVal { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string exp_Title { get; set; }

        /// <summary>
        /// 파일유형 - systemCode
        /// </summary>
        public string exp_FileTypeSystemCode { get; set; }

        /// <summary>
        /// 파일유형 - CodeName
        /// </summary>
        public string exp_FileTypeCodeName { get; set; }

        // TODO : 프로퍼티 "exp_SoftMetaName" 필요시 사용 예정 (2023.10.23 jbh)
        /// <summary>
        /// 소프트웨어 - metaName
        /// </summary>
        // public string exp_SoftMetaName { get; set; }

        /// <summary>
        /// 소프트웨어
        /// </summary>
        public string exp_SoftName { get; set; }

        // TODO : 프로퍼티 "exp_SoftVerMetaName" 필요시 사용 예정 (2023.10.23 jbh)
        /// <summary>
        /// 소프트웨어버전 - metaName
        /// </summary>
        // public string exp_SoftVerMetaName { get; set; }

        /// <summary>
        /// 소프트웨어버전
        /// </summary>
        public string exp_SoftVer { get; set; }

        // TODO : 프로퍼티 "exp_KeywordTagMetaName" 필요시 사용 예정 (2023.10.23 jbh)
        /// <summary>
        /// 키워드/태그 - metaName
        /// </summary>
        // public string exp_KeywordTagMetaName { get; set; }

        /// <summary>
        /// 키워드/태그
        /// </summary>
        public string exp_KeywordTag { get; set; }


        /// <summary>
        /// 마일스톤 - 업무 인덱스
        /// </summary>
        public string exp_TaskDetailIdx { get; set; }

        /// <summary>
        /// 마일스톤 - 업무 유형
        /// </summary>
        public string exp_TaskDetailType { get; set; }

        /// <summary>
        /// 파일명(이름)
        /// </summary>
        public string exp_FileName { get; set; }

        /// <summary>
        /// 파일 아이디
        /// </summary>
        public string exp_FileId { get; set; }

        #region 기본 정보 탭 - Label

        /// <summary>
        /// 공통 (컨테이너 정보, 파일정보) - 구분 
        /// </summary>
        public string exp_DivLabel { get; set; }

        /// <summary>
        /// 공통 (컨테이너 정보, 파일정보) - 내용
        /// </summary>
        public string exp_ContentsLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 작성자
        /// </summary>
        public string exp_CreateUserLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 버전
        /// </summary>
        public string exp_VersionLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 제목
        /// </summary>
        public string exp_TitleLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 소프트웨어
        /// </summary>
        public string exp_SoftLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 소프트웨어버전
        /// </summary>
        public string exp_SoftVerLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 키워드/태그
        /// </summary>
        public string exp_KeywordTagLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 파일유형
        /// </summary>
        public string exp_FileTypeCodeLabel { get; set; }

        /// <summary>
        /// 컨테이너 정보 - 마일스톤
        /// </summary>
        public string exp_TaskDetailLabel { get; set; }

        /// <summary>
        /// 파일정보
        /// </summary>
        public string exp_FileNameLabel { get; set; }

        #endregion 기본 정보 탭 - Label

        /// <summary>
        /// 연관된 부모 컨테이너 정보 리스트
        /// </summary>
        public List<Dictionary<string, object>> LinkedNodeInfoList { get; set; }

        // TODO : 연관된 컨테이너 메타 데이터 리스트 객체 "LinkedNodeMetaDataList" 필요시 사용 예정 (2023.11.01 jbh)
        /// <summary>
        /// 연관된 컨테이너 메타 데이터 리스트 
        /// </summary>
        public List<Dictionary<string, object>> LinkedNodeMetaDataList { get; set; }

        // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 리스트 객체 containerDetailList 구현 예정 (2023.10.17 jbh)
        /// <summary>
        /// 컨테이너 정보, 파일정보 리스트
        /// </summary>
        // public List<Dictionary<string, object>> ContainerDetailList { get; set; }

        /// <summary>
        /// 컨테이너 정보, 파일정보 Dictionary
        /// </summary>
        public Dictionary<string, object> DetailDic { get; set; }

        /// <summary>
        /// 파일유형 Dictionary
        /// </summary>
        public Dictionary<string, object> FileTypeCodeDic { get; set; }

        /// <summary>
        /// 메타데이터 정보 리스트
        /// </summary>
        public List<Dictionary<string, object>> MetaDataList { get; set; }

        // TODO : 추후 필요시 "마일스톤 업무" 리스트 객체 TaskDetailList 구현 예정 (2023.10.17 jbh)
        /// <summary>
        /// 마일스톤 업무 리스트
        /// </summary>
        // public List<Dictionary<string, object>> TaskDetailList { get; set; }

        /// <summary>
        /// 마일스톤 업무 Dictionary
        /// </summary>
        public Dictionary<string, object> TaskDetailDic { get; set; }

        /// <summary>
        /// 마일스톤 - 특정 마일스톤 업무 정보
        /// </summary>
        public Dictionary<string, object> MilestoneTaskInfoDic { get; set; }

        /// <summary>
        /// ComboBox - 파일유형 리스트
        /// </summary>
        public List<FileTypeView> FileTypeList { get; set; }


        /// <summary>
        /// ComboBox - 마일스톤 리스트
        /// </summary>
        public List<TaskDetailView> TaskDetailList { get; set; }

        // TODO : 컨테이너 정보 영역 "파일유형" ComboBox 구현시 바인딩할 BindableCollection 객체 FileType 필요시 사용 예정 (2023.10.20 jbh)
        // 참고 URL - https://www.reddit.com/r/csharp/comments/126sdo6/wpf_binding_a_dictionary_value_to_a_combobox/
        // 참고 2 URL - https://www.codeproject.com/Questions/536820/WPFplusBindplusandpluspopulateplusapluscombobox
        /// <summary>
        /// ComboBox - 파일유형
        /// </summary>
        // public BindableCollection<FileTypeView> FileType { get; set; }

        // TODO : 컨테이너 정보 영역 "마일스톤" ComboBox 구현시 바인딩할 BindableCollection 객체 TaskDetailType 필요시 사용 예정 (2023.10.20 jbh)
        // 참고 URL - https://www.reddit.com/r/csharp/comments/126sdo6/wpf_binding_a_dictionary_value_to_a_combobox/
        // 참고 2 URL - https://www.codeproject.com/Questions/536820/WPFplusBindplusandpluspopulateplusapluscombobox
        /// <summary>
        /// ComboBox - 마일스톤
        /// </summary>
        // public BindableCollection<TaskDetailView> TaskDetailType { get; set; }

        /// <summary>
        /// ComboBox "파일유형" List 생성 메서드 ContainerInfoTypeListCreate 실행시 화면에 출력할 ComboBox 데이터 목록
        /// </summary>
        public Param SelectParam { get; set; } = new Param();

        #region 생성자

        public ContainerDetailView()
        {

        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    // TODO : 뷰(ExplorerV.xaml)에 구현된 ComboBox 파일유형, 마일스톤에서 사용하는 속성 SelectedItem에 바인딩되는
    //        바인딩되는 UpdateParam클래스 객체 Param의 바인딩 객체(FileTypeCodeInfo, taskDetail)에 저장된 값 변경시 
    //        뷰모델 (ExplorerVM.cs)에 해당 바인딩 객체(FileTypeCodeInfo, taskDetail)의 값이 변경되었다는 프로퍼티 체인지 이벤트(NotifyOfPropertyChange();)를
    //        실행하기 위해서 UpdateParam 클래스는 부모 클래스로 ValidatingModelBase를 상속 받도록 구현 (2023.10.23 jbh)
    // 참고 URL - https://kaki104.tistory.com/749
    /// <summary>
    /// ComboBox "파일유형" List 생성 메서드 ContainerInfoTypeListCreate 실행시 ComboBox에 할당될 데이터 목록
    /// </summary>
    public class Param : BindableBase
    {
        // TODO : 수정 메서드 "UpdateAsync" 실행시 전달할 수정된 메서드 파라미터 필요시 추가 예정 (2023.10.23 jbh)

        /// <summary>
        /// 파일유형
        /// </summary>
        public FileTypeView FileTypeCodeInfo { get => _FileTypeCodeInfo; set { _FileTypeCodeInfo = value; NotifyOfPropertyChange(); } }
        private FileTypeView _FileTypeCodeInfo;

        // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 jbh)
        /// <summary>
        /// 마일스톤
        /// </summary>
        public TaskDetailView TaskDetail { get => _TaskDetail; set { _TaskDetail = value; NotifyOfPropertyChange(); } }
        private TaskDetailView _TaskDetail;

        #region 생성자

        public Param()
        {

        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    /// <summary>
    /// ComboBox - 파일유형
    /// </summary>
    public class FileTypeView
    {
        /// <summary>
        /// systemCode - typmodel, typreprt
        /// </summary>
        public string exp_systemCode { get; set; }

        /// <summary>
        /// codeName - 모델 / 보고서
        /// </summary>
        public string exp_codeName { get; set; }

        #region 생성자

        public FileTypeView(string systemCode, string codeName)
        {
            this.exp_systemCode = systemCode;
            this.exp_codeName = codeName;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 jbh)
    /// <summary>
    /// ComboBox - 마일스톤
    /// </summary>
    public class TaskDetailView
    {
        /// <summary>
        /// 마일스톤 인덱스 (웹서버로 부터 받은 JSON 데이터 중 "taskDetail"에 존재하는 "taskIdx"와 "idx"는 값이 같음.)
        /// </summary>
        // public int exp_taskIdx { get; set; }
        public string exp_idx { get; set; }

        /// <summary>
        /// 마일스톤 타입 title - 관련업무선택, 요구사항정의, 엔터티도출, 요구사항정의서 작성, UI설계서 작성
        /// </summary>
        public string exp_title { get; set; }

        #region 생성자

        public TaskDetailView(string idx, string title)
        {
            this.exp_idx = idx;
            this.exp_title = title;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }


    /// <summary>
    /// 연관 컨테이너 정보
    /// </summary>
    public class LinkedNodeView : ITreeNode
    {
        /// <summary>
        /// 연관 컨테이너 이름
        /// </summary>
        public string exp_TreeName { get; set; }

        // TODO : 파일 하위 자식 "ChildNodes" 필요시 사용 예정 (2023.09.18 jbh)
        /// <summary>
        /// 파일 하위 자식 노드
        /// </summary>
        public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
        //public List<ITreeNode> ChildNodes
        //{
        //    get
        //    {
        //        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        // 싱글톤 패턴 
        //        if (_ChildNodes is null)
        //        {
        //            _ChildNodes = new List<ITreeNode>();
        //        }
        //        return _ChildNodes;
        //    }
        //    set => _ChildNodes = value;
        //}
        //private List<ITreeNode> _ChildNodes;

        // TODO : 필요시 프로퍼티 LinkedNodeInfoDic 사용 예정 (2024.08.21 jbh)
        /// <summary>
        /// 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너) Dictionary
        /// </summary>
        public Dictionary<string, object> LinkedNodeInfoDic { get; set; }

        #region 생성자

        public LinkedNodeView(string sourceId)
        {
            this.exp_TreeName = sourceId;
            this.LinkedNodeInfoDic = new Dictionary<string, object>();
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    // TODO : 컨텍스트 메뉴(ContextMenu) 정보 클래스 "ContextMenuView" 필요시 구현 예정 (2023.12.13 jbh)
    /// <summary>
    /// 컨텍스트 메뉴(ContextMenu) 정보 
    /// </summary>
    public class ContextMenuView
    {
        /// <summary>
        /// 컨텍스트 메뉴(ContextMenu) 비활성화 여부
        /// </summary>
        public bool exp_IsEnabled { get; set; }

        /// <summary>
        /// 컨텍스트 메뉴(ContextMenu) Header
        /// </summary>
        public string exp_Header { get; set; }

        #region 생성자

        public ContextMenuView(bool isEnabled, string header)
        {
            this.exp_IsEnabled = isEnabled;
            this.exp_Header = header;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    #endregion ExplorerVM

    #region ProjectPack

    /// <summary>
    /// 프로젝트(+팀) 정보
    /// </summary>
    public class ProjectPack
    {
        /// <summary>
        /// 요청 파라미터 - 프로젝트 아이디 
        /// </summary>
        public string projectId { get; set; }

        /// <summary>
        /// 요청 파라미터 - 팀 아이디
        /// </summary>
        public string teamId { get; set; }

        #region 생성자

        public ProjectPack(string projectId, string teamId)
        {
            this.projectId = projectId;
            this.teamId = teamId;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    #endregion ProjectPack

    #region ContainerPack

    /// <summary>
    /// 컨테이너 정보
    /// </summary>
    public class ContainerPack
    {
        /// <summary>
        /// 프로젝트 아이디 
        /// </summary>
        private string _ProjectId { get; set; }

        /// <summary>
        /// 팀 아이디
        /// </summary>
        private string _TeamId { get; set; }

        /// <summary>
        /// 컨테이너 아이디
        /// </summary>
        private string _ContainerId { get; set; }

        /// <summary>
        /// 컨테이너 목록 조회 - 사용자가 선택한 폴더 아이디
        /// </summary>
        private string _SelectedFolderId { get; set; }

        /// <summary>
        /// 체크아웃 - 연관 컨테이너 아이디
        /// </summary>
        private string _LinkedContainerId { get; set; }

        #region 생성자

        public ContainerPack(string projectId, string teamId, string? containerId = null, string? selectedFolderId = null, string? linkedContainerId = null)
        {
            _ProjectId = projectId;
            _TeamId = teamId;
            _ContainerId = containerId;
            _SelectedFolderId = selectedFolderId;
            _LinkedContainerId = linkedContainerId;
        }

        #endregion 생성자 

        #region 프로젝트 아이디 

        /// <summary>
        /// 프로젝트 아이디 값 셋팅
        /// </summary>
        public void SetProjectId(string projectId)
        {
            _ProjectId = projectId;
        }

        /// <summary>
        /// 프로젝트 아이디 가져오기
        /// </summary>
        public string GetProjectId()
        {
            return _ProjectId;
        }

        #endregion 프로젝트 아이디 

        #region 팀 아이디

        /// <summary>
        /// 팀 아이디 값 셋팅
        /// </summary>
        public void SetTeamId(string teamId)
        {
            _TeamId = teamId;
        }

        /// <summary>
        /// 팀 아이디 가져오기
        /// </summary>
        public string GetTeamId()
        {
            return _TeamId;
        }

        #endregion 팀 아이디

        #region 컨테이너 아이디

        /// <summary>
        /// 컨테이너 아이디 값 셋팅
        /// </summary>
        public void SetContainerId(string containerId)
        {
            _ContainerId = containerId;
        }

        /// <summary>
        /// 컨테이너 아이디 가져오기
        /// </summary>
        public string GetContainerId()
        {
            return _ContainerId;
        }

        #endregion 컨테이너 아이디

        #region 사용자가 선택한 폴더 아이디

        /// <summary>
        /// 사용자가 선택한 폴더 아이디 값 셋팅
        /// </summary>
        public void SetSelectedFolderId(string selectedFolderId)
        {
            _SelectedFolderId = selectedFolderId;
        }

        /// <summary>
        /// 사용자가 선택한 폴더 아이디 가져오기 
        /// </summary>
        public string GetSelectedFolderId()
        {
            return _SelectedFolderId;
        }

        #endregion 사용자가 선택한 폴더 아이디

        #region 연관 컨테이너 아이디

        /// <summary>
        /// 체크아웃 - 연관 컨테이너 아이디 값 셋팅
        /// </summary>
        public void SetLinkedContainerId(string linkedContainerId)
        {
            _LinkedContainerId = linkedContainerId;
        }

        /// <summary>
        /// 체크아웃 - 연관 컨테이너 아이디 가져오기
        /// </summary>
        public string GetLinkedContainerId()
        {
            return _LinkedContainerId;
        }

        #endregion 연관 컨테이너 아이디

        #region Sample

        #endregion Sample
    }

    #endregion ContainerPack

    #region FilePack

    /// <summary>
    /// 파일 정보
    /// </summary>
    public class FilePack
    {
        /// <summary>
        /// 파일 아이디 
        /// </summary>
        private string _FileId { get; set; }

        /// <summary>
        /// 파일명
        /// </summary>
        private string _FileName { get; set; }

        // TODO : 파일 사이즈 프로퍼티 "_FileSize" 필요시 사용 예정 (2024.10.11 jbh)
        /// <summary>
        /// 파일 사이즈
        /// </summary>
        private string _FileSize { get; set; }

        /// <summary>
        /// 업로드 할 파일
        /// </summary>
        private FileStream _UploadFiles { get; set; }

        #region 생성자

        public FilePack(string fileId, string fileName, string? fileSize = null, FileStream? uploadFiles = null)
        {
            _FileId = fileId;
            _FileName = fileName;
            _FileSize = fileSize;
            _UploadFiles = uploadFiles;
        }

        #endregion 생성자 

        #region 파일 아이디 

        /// <summary>
        /// 파일 아이디 값 셋팅 
        /// </summary>
        public void SetFileId(string fileId)
        {
            _FileId = fileId;
        }

        /// <summary>
        /// 파일 아이디 가져오기 
        /// </summary>
        public string GetFileId()
        {
            return _FileId;
        }

        #endregion 파일 아이디 

        #region 파일명

        /// <summary>
        /// 파일명 값 셋팅 
        /// </summary>
        public void SetFileName(string fileName)
        {
            _FileName = fileName;
        }

        /// <summary>
        /// 파일명 가져오기 
        /// </summary>
        public string GetFileName()
        {
            return _FileName;
        }

        #endregion 파일명

        #region 파일 사이즈

        /// <summary>
        /// 파일 사이즈 값 셋팅 
        /// </summary>
        public void SetFileSize(string fileSize)
        {
            _FileSize = fileSize;
        }

        /// <summary>
        /// 파일 사이즈 가져오기
        /// </summary>
        public string GetFileSize()
        {
            return _FileSize;
        }

        #endregion 파일 사이즈

        #region 업로드 할 파일

        /// <summary>
        /// 업로드 할 파일 값 셋팅 
        /// </summary>
        public void SetUploadFiles(FileStream uploadFiles)
        {
            _UploadFiles = uploadFiles;
        }

        /// <summary>
        /// 업로드 할 파일 가져오기
        /// </summary>
        public FileStream GetUploadFiles()
        {
            return _UploadFiles;
        }

        #endregion 업로드 할 파일

        #region Sample

        #endregion Sample
    }

    #endregion FilePack

    #region CheckInPack

    /// <summary>
    /// 컨테이너 체크인 + 파일 웹서버 업로드 정보
    /// </summary>
    public class CheckInPack
    {
        /// <summary>
        /// 폴더 아이디
        /// </summary>
        private string _FolderId { get; set; }

        /// <summary>
        /// comment
        /// </summary>
        private string _Comment { get; set; }

        // TODO : 파일 정보 프로퍼티 "_FilePack" 필요시 사용 예정 (2024.10.14 jbh)
        /// <summary>
        /// 파일 정보
        /// </summary>
        // private FilePack _FilePack { get; set; }

        #region 생성자

        //public CheckInPack(string folderId, string comment, FilePack filePack)
        public CheckInPack(string folderId, string comment)
        {
            _FolderId = folderId;
            _Comment = comment;
            // _FilePack = filePack;
        }

        #endregion 생성자 

        #region 폴더 아이디

        /// <summary>
        /// 폴더 아이디 값 셋팅
        /// </summary>
        public void SetFolderId(string folderId)
        {
            _FolderId = folderId;
        }

        /// <summary>
        /// 폴더 아이디 가져오기
        /// </summary>
        public string GetFolderId()
        {
            return _FolderId;
        }

        #endregion 폴더 아이디

        #region comment

        /// <summary>
        /// comment 값 셋팅
        /// </summary>
        public void SetComment(string comment)
        {
            _Comment = comment;
        }

        /// <summary>
        /// comment 가져오기
        /// </summary>
        public string GetComment()
        {
            return _Comment;
        }

        #endregion comment

        #region 파일 정보

        // TODO : 파일 정보 값 셋팅 메서드 "SetFilePack" 필요시 사용 예정 (2024.10.14 jbh)
        /// <summary>
        /// 파일 정보 값 셋팅 
        /// </summary>
        // public void SetFilePack(FilePack filePack)
        // {
        //     _FilePack = filePack;
        // }

        // TODO : 파일 정보 가져오기 메서드 "GetFilePack" 필요시 사용 예정 (2024.10.14 jbh)
        /// <summary>
        /// 파일 정보 가져오기
        /// </summary>
        // public FilePack GetFilePack()
        // {
        //     return _FilePack;
        // }

        #endregion 파일 정보

        #region Sample

        #endregion Sample
    }

    #endregion CheckInPack

    #region CancelWorkPack

    /// <summary>
    /// 컨테이너 작업 취소 정보
    /// </summary>
    public class CancelWorkPack
    {
        // TODO : ExplorerView.cs 소스파일에 속한
        //        컨테이너 작업 취소 정보 클래스 "CancelWorkPack"안에 속하는 프로퍼티 "projectId", "teamId", "containerId" 의 접근 제한자를 
        //        private로 설정하면 오류 발생하여 접근 제한자를 public으로 수정함. (2024.11.04 jbh)
        //        오류 발생 위치 - ExplorerRestServer.cs -> 비동기 메서드 "PostCancelWorkAsync" 
        //        -> 메서드 "JsonSerializer.Serialize" 호출하여 json 문자열(content)을 제대로 리턴 받지 못하고 오류 발생함. 

        // TODO : ExplorerView.cs 소스파일에 속한
        //        컨테이너 작업 취소 정보 클래스 "CancelWorkPack"안에 속하는 프로퍼티 "projectId", "teamId", "containerId" 프로퍼티 이름 
        //        변경하면 오류 발생하여 수정 안 하고 그대로 유지함.(2024.11.04 jbh)
        //        오류 발생 위치 - ExplorerRestServer.cs -> 비동기 메서드 "PostCancelWorkAsync" 
        //        서버로 부터 제대로 리턴 받지 못하고 오류 발생함. 

        /// <summary>
        /// 프로젝트 아이디 
        /// </summary>
        public string projectId { get; set; }

        /// <summary>
        /// 팀 아이디
        /// </summary>
        public string teamId { get; set; }

        /// <summary>
        /// 컨테이너 아이디
        /// </summary>
        public string containerId { get; set; }

        #region 생성자

        public CancelWorkPack(string projectId, string teamId, string containerId)
        {
            this.projectId = projectId;
            this.teamId = teamId;
            this.containerId = containerId;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }

    #endregion CancelWorkPack

    #region WebViewerPack

    /// <summary>
    /// 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보 
    /// </summary>
    public class WebViewerPack
    {
        #region 프로퍼티

        /// <summary>
        /// 프로젝트 아이디 
        /// </summary>
        private string _ProjectId { get; set; }

        /// <summary>
        /// 팀 아이디
        /// </summary>
        private string _TeamId { get; set; }

        /// <summary>
        /// 컨테이너 아이디
        /// </summary>
        private string _ContainerId { get; set; }

        /// <summary>
        /// 컨테이너에 속한 파일 아이디
        /// </summary>
        private string _FileId { get; set; }

        #endregion 프로퍼티

        #region 생성자

        public WebViewerPack(string projectId, string teamId, string containerId, string fileId)
        {
            _ProjectId = projectId;
            _TeamId = teamId;
            _ContainerId = containerId;
            _FileId = fileId;
        }

        #endregion 생성자 

        #region 프로젝트 아이디 

        /// <summary>
        /// 프로젝트 아이디 값 셋팅
        /// </summary>
        public void SetProjectId(string projectId)
        {
            _ProjectId = projectId;
        }

        /// <summary>
        /// 프로젝트 아이디 가져오기
        /// </summary>
        public string GetProjectId()
        {
            return _ProjectId;
        }

        #endregion 프로젝트 아이디 

        #region 팀 아이디

        /// <summary>
        /// 팀 아이디 값 셋팅
        /// </summary>
        public void SetTeamId(string teamId)
        {
            _TeamId = teamId;
        }

        /// <summary>
        /// 팀 아이디 가져오기
        /// </summary>
        public string GetTeamId()
        {
            return _TeamId;
        }

        #endregion 팀 아이디

        #region 컨테이너 아이디

        /// <summary>
        /// 컨테이너 아이디 값 셋팅
        /// </summary>
        public void SetContainerId(string containerId)
        {
            _ContainerId = containerId;
        }

        /// <summary>
        /// 컨테이너 아이디 가져오기
        /// </summary>
        public string GetContainerId()
        {
            return _ContainerId;
        }

        #endregion 컨테이너 아이디

        #region 컨테이너에 속한 파일 아이디

        /// <summary>
        /// 컨테이너에 속한 파일 아이디 값 셋팅
        /// </summary>
        public void SetFileId(string fileId)
        {
            _FileId = fileId;
        }

        /// <summary>
        /// 컨테이너에 속한 파일 아이디 가져오기
        /// </summary>
        public string GetFileId()
        {
            return _FileId;
        }

        #endregion 컨테이너에 속한 파일 아이디

        #region Sample

        #endregion Sample
    }

    #endregion WebViewerPack

    #region TranslatePack

    /// <summary>
    /// 컨테이너 체크인 + 파일 웹서버 업로드 성공시 
    /// HubBox 파일 변환 Rest API 요청시 필수 정보 
    /// </summary>
    public class TranslatePack
    {
        #region 프로퍼티

        /// <summary>
        /// 프로젝트 아이디 
        /// </summary>
        private string _ProjectId { get; set; }

        /// <summary>
        /// 팀 아이디
        /// </summary>
        private string _TeamId { get; set; }

        /// <summary>
        /// 컨테이너 아이디
        /// </summary>
        private string _ContainerId { get; set; }

        /// <summary>
        /// 컨테이너에 속한 파일 아이디
        /// </summary>
        private string _FileId { get; set; }

        #endregion 프로퍼티

        #region 생성자

        public TranslatePack(string projectId, string teamId, string containerId, string fileId)
        {
            _ProjectId = projectId;
            _TeamId = teamId;
            _ContainerId = containerId;
            _FileId = fileId;
        }

        #endregion 생성자 

        #region 프로젝트 아이디 

        /// <summary>
        /// 프로젝트 아이디 값 셋팅
        /// </summary>
        public void SetProjectId(string projectId)
        {
            _ProjectId = projectId;
        }

        /// <summary>
        /// 프로젝트 아이디 가져오기
        /// </summary>
        public string GetProjectId()
        {
            return _ProjectId;
        }

        #endregion 프로젝트 아이디 

        #region 팀 아이디

        /// <summary>
        /// 팀 아이디 값 셋팅
        /// </summary>
        public void SetTeamId(string teamId)
        {
            _TeamId = teamId;
        }

        /// <summary>
        /// 팀 아이디 가져오기
        /// </summary>
        public string GetTeamId()
        {
            return _TeamId;
        }

        #endregion 팀 아이디

        #region 컨테이너 아이디

        /// <summary>
        /// 컨테이너 아이디 값 셋팅
        /// </summary>
        public void SetContainerId(string containerId)
        {
            _ContainerId = containerId;
        }

        /// <summary>
        /// 컨테이너 아이디 가져오기
        /// </summary>
        public string GetContainerId()
        {
            return _ContainerId;
        }

        #endregion 컨테이너 아이디

        #region 파일 아이디 

        /// <summary>
        /// 파일 아이디 값 셋팅 
        /// </summary>
        public void SetFileId(string fileId)
        {
            _FileId = fileId;
        }

        /// <summary>
        /// 파일 아이디 가져오기 
        /// </summary>
        public string GetFileId()
        {
            return _FileId;
        }

        #endregion 파일 아이디 

        #region Sample

        #endregion Sample
    }

    #endregion TranslatePack

    #region Sample

    // TODO : 필요시 클래스 "CheckInPack" 참고 (2024.10.29 jbh)
    /// <summary>
    /// 컨테이너 체크인 + 업로드
    /// </summary>
    //public class CheckInPack
    //{
    //    /// <summary>
    //    /// 폴더 아이디
    //    /// </summary>
    //    public string folderId { get; set; }

    //    /// <summary>
    //    /// 업로드 할 파일
    //    /// </summary>
    //    // public string uploadFiles { get; set; }
    //    public FileStream uploadFiles { get; set; }

    //    // TODO : 필요시 파일 아이디 프로퍼티 "fileId" 사용 예정 (2024.10.14 jbh)
    //    /// <summary>
    //    /// 파일 아이디
    //    /// </summary>
    //    // public string fileId { get; set; }

    //    /// <summary>
    //    /// 파일명
    //    /// </summary>
    //    public string fileName { get; set; }

    //    // TODO : 파일사이즈 프로퍼티 "fileSize" 필요시 사용 예정 (2023.12.05 jbh)
    //    /// <summary>
    //    /// 파일사이즈
    //    /// </summary>
    //    public string fileSize { get; set; }

    //    // TODO : comment - Description 프로퍼티 "comment" 인재 INC와 소통 후 사용 예정 (2023.12.05 jbh)
    //    /// <summary>
    //    /// comment - Description
    //    /// </summary>
    //    public string comment { get; set; }

    //    #region 생성자

    //    public CheckInPack()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    // TODO : 필요시 클래스 "ContainerPack" 참고 (2024.10.29 jbh)
    /// <summary>
    /// 컨테이너 정보
    /// </summary>
    //public class ContainerPack
    //{
    //    /// <summary>
    //    /// 요청 파라미터 - 프로젝트 아이디 
    //    /// </summary>
    //    public string projectId { get; set; }

    //    /// <summary>
    //    /// 요청 파라미터 - 팀 아이디
    //    /// </summary>
    //    public string teamId { get; set; }

    //    /// <summary>
    //    /// 요청 파라미터 - 컨테이너 아이디
    //    /// </summary>
    //    public string containerId { get; set; }

    //    /// <summary>
    //    /// 체크아웃 - 연관 컨테이너 아이디
    //    /// </summary>
    //    public string linkedContainerId { get; set; }

    //    #region 생성자

    //    public ContainerPack(string projectId, string teamId, string containerId, string linkedContainerId = null)
    //    {
    //        this.projectId = projectId;
    //        this.teamId = teamId;
    //        this.containerId = containerId;
    //        this.linkedContainerId = linkedContainerId;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    // TODO : 필요시 클래스 "FilePack" 참고 (2024.10.29 jbh)
    /// <summary>
    /// 파일 정보
    /// </summary>
    //public class FilePack
    //{
    //    #region Rest API 7. 파일 다운로드 사용

    //    /// <summary>
    //    /// 요청 데이터 - 파일 아이디 
    //    /// </summary>
    //    public string fileId { get; set; }

    //    #endregion Rest API 7. 파일 다운로드 사용

    //    #region 공통 - Rest API 7. 파일 다운로드 / 10. 컨테이너 체크아웃 + 다운로드

    //    /// <summary>
    //    /// 요청 데이터 또는 응답 결과 - 파일명
    //    /// </summary>
    //    public string fileName { get; set; }

    //    #endregion 공통 - Rest API 7. 파일 다운로드 / 10. 컨테이너 체크아웃 + 다운로드

    //    #region Rest API 11. 컨테이너 체크인 + 업로드

    //    /// <summary>
    //    /// 요청 데이터 - 해당 파일을 업로드 하려는 상위 폴더 아이디
    //    /// </summary>
    //    // public string folderId { get; set; }

    //    /// <summary>
    //    /// 요청 데이터 - 업로드할 파일명
    //    /// </summary>
    //    // public string uploadFiles { get; set; }

    //    /// <summary>
    //    /// 요청 데이터 - 파일 사이즈
    //    /// </summary>
    //    // public string fileSize { get; set; }

    //    #endregion Rest API 11. 컨테이너 체크인 + 업로드

    //    #region 생성자

    //    public FilePack(string fileId, string fileName)
    //    {
    //        this.fileId = fileId;
    //        this.fileName = fileName;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    // TODO : 필요시 클래스 "CheckOut" 참고 (2024.10.29 jbh)
    /// <summary>
    /// 컨테이너 체크아웃
    /// </summary>
    //public class CheckOut
    //{
    //    /// <summary>
    //    /// 응답결과 - resultData
    //    /// </summary>
    //    public string resultData { get; set; }

    //    /// 응답결과 - 체크아웃 성공 여부 
    //    public int resultCount { get; set; }

    //    /// <summary>
    //    /// Http 통신 결과 메시지
    //    /// </summary>
    //    public string resultMessage { get; set; }

    //    /// <summary>
    //    /// 테넌트 아이디
    //    /// </summary>
    //    public string tenantId { get; set; }

    //    /// <summary>
    //    /// jwt 토큰
    //    /// </summary>
    //    public string jwt { get; set; }

    //    public string refreshToken { get; set; }

    //    public string param { get; set; }

    //    public string payload { get; set; }

    //    public string actionUserInfo { get; set; }

    //    #region 생성자

    //    public CheckOut()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    // TODO : 필요시 클래스 ExplorerView 참고 (2024.10.29 jbh)
    /// <summary>
    /// CobimExplorer
    /// 탐색기 화면 뷰모델(ExplorerVM.cs) 데이터 모델
    /// </summary>
    //public class ExplorerView : TExplorerView<ExplorerView>
    //{
    //    #region 프로퍼티

    //    /// <summary>
    //    /// 탐색기 화면 -> 서버(Rest API) 요청 데이터 
    //    /// </summary>
    //    //public static ExplorerView RequestData
    //    //{
    //    //    get => ExplorerView._RequestData ?? (ExplorerView._RequestData = new ExplorerView());
    //    //    set 
    //    //    {
    //    //        ExplorerView._RequestData = value;
    //    //        BindableBase.StaticChanged(nameof(RequestData));
    //    //    }
    //    //}
    //    //private static ExplorerView _RequestData;

    //    /// <summary>
    //    /// 컨테이너 체크인 + 업로드 (comment - Description)
    //    /// </summary>
    //    public string exp_Comment { get; set; }

    //    // TODO : 체크인 업로드 요청 데이터 프로퍼티 "CheckIn" 필요시 사용 예정 (2023.12.05 jbh)
    //    /// <summary>
    //    /// 체크인 업로드 요청 데이터
    //    /// </summary>
    //    //public CheckInPack CheckIn
    //    //{
    //    //    get => this._CheckIn ?? (this._CheckIn = new CheckInPack());
    //    //    set 
    //    //    {
    //    //        this._CheckIn = value;
    //    //        this.Changed(nameof(CheckIn));
    //    //    }
    //    //}
    //    //private CheckInPack _CheckIn;

    //    #endregion 프로퍼티

    //    #region 생성자

    //    // TODO : 생성자 "ExplorerView()" 탐색기 화면 뷰모델(ExplorerVM.cs)에서 필요시 호출해서 ExplorerView 클래스 객체 생성 예정 (2023.11.21 jbh)
    //    public ExplorerView() : base() { }


    //    #endregion 생성자
    //}

    //#region 공통 - CheckInPartVM, ExplorerVM

    ///// <summary>
    ///// 컨테이너 체크인 + 업로드
    ///// </summary>
    //public class CheckInPack
    //{
    //    /// <summary>
    //    /// 폴더 아이디
    //    /// </summary>
    //    public string folderId { get; set; }

    //    /// <summary>
    //    /// 업로드 할 파일
    //    /// </summary>
    //    // public string uploadFiles { get; set; }
    //    public FileStream uploadFiles { get; set; }

    //    // TODO : 필요시 파일 아이디 프로퍼티 "fileId" 사용 예정 (2024.10.14 jbh)
    //    /// <summary>
    //    /// 파일 아이디
    //    /// </summary>
    //    // public string fileId { get; set; }

    //    /// <summary>
    //    /// 파일명
    //    /// </summary>
    //    public string fileName { get; set; }

    //    // TODO : 파일사이즈 프로퍼티 "fileSize" 필요시 사용 예정 (2023.12.05 jbh)
    //    /// <summary>
    //    /// 파일사이즈
    //    /// </summary>
    //    public string fileSize { get; set; }

    //    // TODO : comment - Description 프로퍼티 "comment" 인재 INC와 소통 후 사용 예정 (2023.12.05 jbh)
    //    /// <summary>
    //    /// comment - Description
    //    /// </summary>
    //    public string comment { get; set; }

    //    #region 생성자

    //    public CheckInPack()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //#endregion 공통 - CheckInPartVM, ExplorerVM

    //#region ExplorerVM

    //#region EnumFolder

    //public enum EnumFolder : int
    //{
    //    [Description("폴더_부재")]
    //    NONE = 0,

    //    [Description("폴더_존재")]
    //    EXISTENCE = 1
    //}


    //#endregion EnumFolder

    //#region EnumContainer

    //public enum EnumContainer : int
    //{
    //    [Description("프로젝트_아이디")]
    //    PROJECTID = 0,
    //    [Description("팀_아이디")]
    //    TEAMID = 1,
    //    [Description("폴더_아이디")]
    //    FOLDERID = 2
    //}

    //#endregion EnumContainer

    //#region EnumContainerDetail

    //public enum EnumContainerDetail : int
    //{
    //    [Description("프로젝트_아이디")]
    //    PROJECTID = 0,
    //    [Description("팀_아이디")]
    //    TEAMID = 1,
    //    [Description("컨테이너_아이디")]
    //    CONTAINERID = 2
    //}

    //#endregion EnumContainerDetail

    //#region EnumDownLoad

    //public enum EnumDownLoad : int
    //{
    //    [Description("프로젝트_아이디")]
    //    PROJECTID = 0,
    //    [Description("팀_아이디")]
    //    TEAMID = 1,
    //    [Description("컨테이너_아이디")]
    //    CONTAINERID = 2,
    //    [Description("파일_아이디")]
    //    FILEID = 3,
    //    [Description("파일_이름")]
    //    FILENAME = 4
    //}

    //#endregion EnumDownLoad

    //#region EnumLinkedRelation

    //// TODO : enum 열거형 구조체 "EnumLinkedRelation" 변수 "EXISTENCE" 필요시 사용 예정 (2023.11.29 jbh)
    ///// <summary>
    ///// 참조/연관 컨테이너 관계 표현 
    ///// </summary>
    //public enum EnumLinkedRelation : int
    //{
    //    [Description("참조_연관컨테이너_부재")]
    //    NONE = 0,

    //    [Description("참조_연관컨테이너_존재")]
    //    EXISTENCE = 1
    //}

    //#endregion EnumLinkedRelation

    //#region EnumCheckOut

    //// TODO : Enum 열거형 구조체 멤버변수 "FILEID", "FILENAME" 필요시 사용 예정 (2023.11.28 jbh)
    //public enum EnumCheckOut : int
    //{
    //    [Description("프로젝트_아이디")]
    //    PROJECTID = 0,
    //    [Description("팀_아이디")]
    //    TEAMID = 1,
    //    [Description("체크아웃_컨테이너_아이디")]
    //    CONTAINERID = 2,

    //    // TODO : 탐색기 화면 뷰(ExplorerV.xaml) -> 컨텍스트 메뉴 "체크아웃" 클릭시 멀티 바인딩 대상 중 "exp_FolderId" 제외함.
    //    //        제외한 사유는 "exp_FolderId" 설정시 탐색기 뷰모델(ExplorerVM.cs) -> 컨테이너 리스트 생성 비동기 메서드 "ContainerListCreateAsync"
    //    //        실행 결과 데이터 정보가 변경되어 컨테이너 리스트 화면이 바뀌므로 해당 프로퍼티는 멀티 바인딩 대상에서 제외 처리함. (2023.12.13 jbh) 
    //    //        하여 아래 enum 열거형 구조체 EnumCheckOut 멤버 변수 FOLDERID 또한 사용 제외 (2023.12.13 jbh)
    //    //[Description("폴더_아이디")]
    //    //FOLDERID = 3,
    //    [Description("파일_아이디")]
    //    FILEID = 3,
    //    [Description("파일_이름")]
    //    FILENAME = 4
    //}


    //#endregion EnumCheckOut

    //#region EnumCheckOutResult

    ///// <summary>
    ///// 체크아웃 실행 결과
    ///// </summary>
    //public enum EnumCheckOutResult : int
    //{
    //    [Description("체크아웃_실패")]
    //    FAILURE = 0,
    //    [Description("체크아웃_성공")]
    //    SUCCESS = 1
    //}

    //#endregion EnumCheckOutResult

    //#region EnumCheckIn

    //public enum EnumCheckIn : int
    //{
    //    [Description("프로젝트_아이디")]
    //    PROJECTID = 0,
    //    [Description("팀_아이디")]
    //    TEAMID = 1,
    //    [Description("컨테이너_아이디")]
    //    CONTAINERID = 2,
    //    [Description("폴더_아이디")]
    //    FOLDERID = 3,
    //    // TODO : enum 열거형 구조체 EnumCheckIn 멤버 변수 "UPLOADFILES" 필요시 사용 예정 (2023.12.05 jbh)
    //    //[Description("업로드_파일")]
    //    //UPLOADFILES = 4,
    //    [Description("파일_아이디")]
    //    FILEID = 4,
    //    // TODO : 추후 체크인 구현시 업로드 하려는 파일명(UPLOAD_FILENAME)과 기존 체크인 된 파일명(FILENAME)이 다를 경우
    //    //        enum 열거형 구조체 EnumCheckIn 멤버 변수 "FILENAME" 사용 예정 (2023.12.05 jbh)
    //    // [Description("파일명")]
    //    // FILENAME = 5,
    //    [Description("업로드_파일명")]
    //    UPLOAD_FILENAME = 5,
    //    // TODO : enum 열거형 구조체 EnumCheckIn 멤버 변수 "FILESIZE" 필요시 사용 예정 (2023.12.05 jbh)
    //    // [Description("파일_사이즈")]
    //    // FILESIZE    = 6
    //    [Description("업로드_전_파일_버전")]
    //    REVSNVAL = 6,
    //    [Description("제목")]
    //    TITLE = 7
    //}

    //#endregion EnumCheckIn

    //#region EnumCancelWork

    //public enum EnumCancelWork : int
    //{
    //    [Description("프로젝트_아이디")]
    //    PROJECTID = 0,
    //    [Description("팀_아이디")]
    //    TEAMID = 1,
    //    [Description("작업취소_컨테이너_아이디")]
    //    CANCELWORK_CONTAINERID = 2,
    //    // TODO : enum 열거형 구조체 EnumCancelWork 멤버 변수 "WORKUSERID" 필요시 사용 예정 (2023.12.20 jbh)
    //    //[Description("작업중인_사용자_아이디")]
    //    //WORKUSERID = 3
    //}

    //#endregion EnumCancelWork

    //// TODO : TreeView Multi Level (프로젝트 - 팀 - 폴더 - 서브폴더 / 파일) 구현 (2023.09.18 jbh)
    //// 참고 URL - https://stackoverflow.com/questions/50760316/wpf-treeview-with-three-levels-in
    ///// <summary>
    ///// 프로젝트, 팀, 폴더, 연관 컨테이너 정보 Tree interface
    ///// </summary>
    //public interface ITreeNode
    //{
    //    /// <summary>
    //    /// 프로젝트, 팀, 폴더, 서브폴더 이름, 연관 컨테이너 이름 (파일 이름 제외)
    //    /// </summary>
    //    string exp_TreeName { get; set; }

    //    /// <summary>
    //    /// 프로젝트 하위 자식 (팀, 루트 폴더, 서브폴더) 노드
    //    /// </summary>
    //    List<ITreeNode> ChildNodes { get; set; }
    //}

    ///// <summary>
    ///// 프로젝트 
    ///// </summary>
    //public class ProjectView : ITreeNode
    //{
    //    /// <summary>
    //    /// 프로젝트 이름 
    //    /// </summary>
    //    public string exp_TreeName { get; set; }

    //    /// <summary>
    //    /// 프로젝트 하위 자식 (팀) 
    //    /// </summary>
    //    public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
    //    //public List<ITreeNode> ChildNodes
    //    //{
    //    //    get
    //    //    {
    //    //        // 싱글톤 패턴 
    //    //        if (_ChildNodes is null)
    //    //        {
    //    //            _ChildNodes = new List<ITreeNode>();
    //    //        }
    //    //        return _ChildNodes;
    //    //    }
    //    //    set => _ChildNodes = value;
    //    //}
    //    //private List<ITreeNode> _ChildNodes;

    //    /// <summary>
    //    /// 프로젝트 아이디
    //    /// </summary>
    //    public string exp_ProjectId { get; set; }

    //    /// <summary>
    //    /// 프로젝트 목록 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> ProjectItems { get; set; }

    //    /// <summary>
    //    /// 프로젝트 메타 데이터 Dictionary
    //    /// </summary>
    //    // public Dictionary<string, object> ProjectDataInfo { get; set; } = new Dictionary<string, object>();
    //    public Dictionary<string, object> ProjectDataInfo { get; set; }

    //    #region 생성자

    //    public ProjectView()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    ///// <summary>
    ///// 팀
    ///// </summary>
    //public class TeamView : ITreeNode
    //{
    //    /// <summary>
    //    /// 팀 이름 
    //    /// </summary>
    //    public string exp_TreeName { get; set; }

    //    /// <summary>
    //    /// 프로젝트 하위 자식 (루트 폴더) 
    //    /// </summary>
    //    public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
    //    //public List<ITreeNode> ChildNodes
    //    //{
    //    //    get
    //    //    {
    //    //        // 싱글톤 패턴 
    //    //        if (_ChildNodes is null)
    //    //        {
    //    //            _ChildNodes = new List<ITreeNode>();
    //    //        }
    //    //        return _ChildNodes;
    //    //    }
    //    //    set => _ChildNodes = value;
    //    //}
    //    //private List<ITreeNode> _ChildNodes;

    //    /// <summary>
    //    /// 팀 아이디
    //    /// </summary>
    //    public string exp_TeamId { get; set; }


    //    // TODO : 프로퍼티 "exp_TeamName" 필요시 사용 예정 (2023.10.23 jbh)
    //    /// <summary>
    //    /// 화면 우측 상단 팀 이름 출력할 때 사용할 바인딩 객체 "exp_TeamName"
    //    /// </summary>
    //    // public string exp_TeamName { get; set; }

    //    /// <summary>
    //    /// 팀 목록 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> TeamItems { get; set; }

    //    /// <summary>
    //    /// 팀 메타 데이터 Dictionary
    //    /// </summary>
    //    //public Dictionary<string, object> TeamDataInfo { get; set; } = new Dictionary<string, object>();
    //    public Dictionary<string, object> TeamDataInfo { get; set; }

    //    // TODO : 소속된 팀의 사용자 리스트 필요시 사용 예정 (2023.09.18 jbh)
    //    /// <summary>
    //    /// 소속된 팀의 사용자 리스트 
    //    /// </summary>
    //    public List<Dictionary<string, object>> TeamUserList { get; set; }

    //    #region 생성자

    //    public TeamView()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    ///// <summary>
    ///// 폴더
    ///// </summary>
    //public class FolderView : ITreeNode
    //{
    //    /// <summary>
    //    /// (루트 / 서브) 폴더 이름
    //    /// </summary>
    //    public string exp_TreeName { get; set; }

    //    /// <summary>
    //    /// 폴더가 속한 프로젝트 아이디
    //    /// </summary>
    //    public string exp_ProjectId { get; set; }

    //    /// <summary>
    //    /// 폴더가 속한 팀 아이디 
    //    /// </summary>
    //    public string exp_TeamId { get; set; }

    //    /// <summary>
    //    /// 폴더 하위 자식(서브 폴더)
    //    /// </summary>
    //    public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
    //    //public List<ITreeNode> ChildNodes
    //    //{
    //    //    get
    //    //    {
    //    //        // 싱글톤 패턴 
    //    //        if (_ChildNodes is null)
    //    //        {
    //    //            _ChildNodes = new List<ITreeNode>();
    //    //        }
    //    //        return _ChildNodes;
    //    //    }
    //    //    set => _ChildNodes = value;
    //    //}
    //    //private List<ITreeNode> _ChildNodes;

    //    /// <summary>
    //    /// 부모 폴더 아이디
    //    /// </summary>
    //    public string exp_ParentFolderId { get; set; }

    //    /// <summary>
    //    /// 폴더 그룹 아이디
    //    /// </summary>
    //    public string exp_FolderGroupId { get; set; }

    //    /// <summary>
    //    /// 폴더 아이디
    //    /// </summary>
    //    public string exp_FolderId { get; set; }

    //    /// <summary>
    //    /// 폴더 레벨
    //    /// </summary>
    //    public string exp_FolderLevelNo { get; set; }

    //    /// <summary>
    //    /// 폴더 목록 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> FolderItems { get; set; }

    //    /// <summary>
    //    /// 폴더 메타 데이터 Dictionary
    //    /// </summary>
    //    // public Dictionary<string, object> FolderDataInfo { get; set; } = new Dictionary<string, object>();
    //    public Dictionary<string, object> FolderDataInfo { get; set; }

    //    #region 생성자

    //    public FolderView()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    ///// <summary>
    ///// 컨테이너(파일 포함)
    ///// </summary>
    //public class ContainerView
    //{
    //    /// <summary>
    //    /// 파일 이름
    //    /// </summary>
    //    public string exp_FileName { get; set; }

    //    // TODO : 파일 하위 자식 "ChildNodes" 필요시 사용 예정 (2023.09.18 jbh)
    //    /// <summary>
    //    /// 파일 하위 자식 노드
    //    /// </summary>
    //    //public List<ITreeNode> ChildNodes
    //    //{
    //    //    get
    //    //    {
    //    // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용(2023.11.24 jbh)
    //    //        참고 URL - https://husk321.tistory.com/405
    //    //        // 싱글톤 패턴 
    //    //        if (_ChildNodes is null)
    //    //        {
    //    //            _ChildNodes = new List<ITreeNode>();
    //    //        }
    //    //        return _ChildNodes;
    //    //    }
    //    //    set => _ChildNodes = value;
    //    //}
    //    //private List<ITreeNode> _ChildNodes;

    //    /// <summary>
    //    /// 파일 목록 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> FileItems { get; set; }

    //    // TODO : 공유 컨테이너 리스트 "SharedContainerList" 추후 필요시 사용 예정 (2023.09.22 jbh)
    //    /// <summary>
    //    /// 공유 컨테이너 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> SharedContainerList { get; set; }

    //    /// <summary>
    //    /// 파일 메타 데이터 Dictionary
    //    /// </summary>
    //    // public Dictionary<string, object> FileDataInfo { get; set; } = new Dictionary<string, object>();
    //    public Dictionary<string, object> FileDataInfo { get; set; }

    //    /// <summary>
    //    /// 파일 유형 정보 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> FileTypeCodeInfo { get; set; }

    //    /// <summary>
    //    /// 파일 단계 정보 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> StepCodeInfo { get; set; }

    //    /// <summary>
    //    /// 파일 상태 정보 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> StatusCodeInfo { get; set; }

    //    /// <summary>
    //    /// 파일 체크 정보 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> CheckCodeInfo { get; set; }

    //    /// <summary>
    //    /// 컨테이너 그룹 
    //    /// </summary>
    //    public string exp_ContainerGroup { get; set; }

    //    /// <summary>
    //    /// 컨테이너 ID (영어 대문자)
    //    /// </summary>
    //    public string exp_UpperContainerId { get; set; }

    //    /// <summary>
    //    /// 컨테이너 ID (영어 소문자)
    //    /// </summary>
    //    public string exp_ContainerId { get; set; }

    //    /// <summary>
    //    /// 파일이 속한 상위 프로젝트 아이디
    //    /// </summary>
    //    public string exp_ProjectId { get; set; }

    //    /// <summary>
    //    /// 파일이 속한 상위 팀 아이디
    //    /// </summary>
    //    public string exp_TeamId { get; set; }

    //    /// <summary>
    //    /// 파일이 속한 상위 폴더 아이디
    //    /// </summary>
    //    public string exp_FolderId { get; set; }

    //    /// <summary>
    //    /// 파일 아이디
    //    /// </summary>
    //    public string exp_FileId { get; set; }

    //    /// <summary>
    //    /// 파일 유형
    //    /// </summary>
    //    public string exp_FileTypeCode { get; set; }

    //    /// <summary>
    //    /// 제목
    //    /// </summary>
    //    public string exp_Title { get; set; }

    //    /// <summary>
    //    /// 작성자
    //    /// </summary>
    //    public string exp_CreateUserId { get; set; }

    //    /// <summary>
    //    /// 컨테이너(파일) 등록일시(생성일시)
    //    /// </summary>
    //    public string exp_CreateDateTime { get; set; }
    //    // public DateTime exp_CreateDateTime { get; set; }

    //    /// <summary>
    //    /// 작성자 + " | " + 컨테이너(파일) 등록일시(생성일시)
    //    /// </summary>
    //    public string CreateUserIdAndTime { get; set; }

    //    /// <summary>
    //    /// 유형 - fileTypeCodeInfo -> codeName
    //    /// </summary>
    //    public string exp_TypeCodeName { get; set; }

    //    /// <summary>
    //    /// 단계 - stepCodeInfo -> codeName
    //    /// </summary>
    //    public string exp_StepCodeName { get; set; }

    //    /// <summary>
    //    /// 상태 - statusCodeInfo -> codeName
    //    /// </summary>
    //    public string exp_StatusCodeName { get; set; }

    //    /// <summary>
    //    /// 버전 
    //    /// </summary>
    //    public string exp_RevsnVal { get; set; }

    //    /// <summary>
    //    /// 상태 
    //    /// </summary>
    //    public string exp_StatusCode { get; set; }

    //    /// <summary>
    //    /// 컨테이너 검토상태("comstt10": 검토 미진행, "comstt20": 검토 진행, "comstt30": 검토 완료)
    //    /// </summary>
    //    public string exp_CheckStatusCode { get; set; }

    //    /// <summary>
    //    /// 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
    //    /// </summary>
    //    public string exp_ReadYn { get; set; }

    //    /// <summary>
    //    /// 컨테이너 작업중인 사용자 아이디
    //    /// </summary>
    //    public string exp_WorkUserId { get; set; }

    //    /// <summary>
    //    /// 파일 변환 처리 상태 코드 값
    //    /// </summary>
    //    public string exp_TranslateStatusCode { get; set; }

    //    /// <summary>
    //    /// 파일 변환 처리 상태 응답 메시지 
    //    /// </summary>
    //    public string exp_TranslateStatusMsg { get; set; }

    //    /// <summary>
    //    /// 변환 처리 완료된 파일 웹뷰어 Urn 주소 
    //    /// </summary>
    //    public string exp_TranslateData { get; set; }

    //    /// <summary>
    //    /// Visibility - (파일) 변환 요청 성공(success)
    //    /// </summary>
    //    public Visibility exp_VisibleWebViewerImg { get; set; }

    //    /// <summary>
    //    /// Visibility - (파일) 변환 중입니다.
    //    /// </summary>
    //    public Visibility exp_VisibleTranslateImg { get; set; }

    //    /// <summary>
    //    /// Visibility - (파일) 변환에 실패 한 파일입니다.
    //    /// </summary>
    //    public Visibility exp_VisibleFailureImg { get; set; }

    //    /// <summary>
    //    /// Visibility - (파일) 변환 내역이 없습니다.
    //    /// </summary>
    //    public Visibility exp_VisibleNotExistTranslateImg { get; set; }

    //    /// <summary>
    //    /// Visibility - (파일) 변환 오류. 
    //    /// </summary>
    //    public Visibility exp_VisibleErrorImg { get; set; }


    //    #region 생성자

    //    public ContainerView()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    ///// <summary>
    ///// 컨테이너(파일 포함) 메타 데이터 정보
    ///// </summary>
    //public class ContainerDetailView
    //{
    //    /// <summary>
    //    /// 작성자
    //    /// </summary>
    //    public string exp_CreateUserId { get; set; }

    //    /// <summary>
    //    /// 컨테이너 아이디 (영어 소문자)
    //    /// </summary>
    //    public string exp_ContainerId { get; set; }


    //    /// <summary>
    //    /// 컨테이너 ID (영어 대문자)
    //    /// </summary>
    //    // public string exp_UpperContainerId { get; set; }

    //    /// <summary>
    //    /// 선택한 특정 컨테이너 ID (영어 대문자)
    //    /// </summary>
    //    // public string exp_SelectedContainerId { get; set; }


    //    /// <summary>
    //    /// 프로젝트 아이디
    //    /// </summary>
    //    public string exp_ProjectId { get; set; }

    //    /// <summary>
    //    /// 팀 아이디
    //    /// </summary>
    //    public string exp_TeamId { get; set; }


    //    /// <summary>
    //    /// 버전
    //    /// </summary>
    //    public string exp_RevsnVal { get; set; }

    //    /// <summary>
    //    /// 제목
    //    /// </summary>
    //    public string exp_Title { get; set; }

    //    /// <summary>
    //    /// 파일유형 - systemCode
    //    /// </summary>
    //    public string exp_FileTypeSystemCode { get; set; }

    //    /// <summary>
    //    /// 파일유형 - CodeName
    //    /// </summary>
    //    public string exp_FileTypeCodeName { get; set; }

    //    // TODO : 프로퍼티 "exp_SoftMetaName" 필요시 사용 예정 (2023.10.23 jbh)
    //    /// <summary>
    //    /// 소프트웨어 - metaName
    //    /// </summary>
    //    // public string exp_SoftMetaName { get; set; }

    //    /// <summary>
    //    /// 소프트웨어
    //    /// </summary>
    //    public string exp_SoftName { get; set; }

    //    // TODO : 프로퍼티 "exp_SoftVerMetaName" 필요시 사용 예정 (2023.10.23 jbh)
    //    /// <summary>
    //    /// 소프트웨어버전 - metaName
    //    /// </summary>
    //    // public string exp_SoftVerMetaName { get; set; }

    //    /// <summary>
    //    /// 소프트웨어버전
    //    /// </summary>
    //    public string exp_SoftVer { get; set; }

    //    // TODO : 프로퍼티 "exp_KeywordTagMetaName" 필요시 사용 예정 (2023.10.23 jbh)
    //    /// <summary>
    //    /// 키워드/태그 - metaName
    //    /// </summary>
    //    // public string exp_KeywordTagMetaName { get; set; }

    //    /// <summary>
    //    /// 키워드/태그
    //    /// </summary>
    //    public string exp_KeywordTag { get; set; }


    //    /// <summary>
    //    /// 마일스톤 - 업무 인덱스
    //    /// </summary>
    //    public string exp_TaskDetailIdx { get; set; }

    //    /// <summary>
    //    /// 마일스톤 - 업무 유형
    //    /// </summary>
    //    public string exp_TaskDetailType { get; set; }

    //    /// <summary>
    //    /// 파일명(이름)
    //    /// </summary>
    //    public string exp_FileName { get; set; }

    //    /// <summary>
    //    /// 파일 아이디
    //    /// </summary>
    //    public string exp_FileId { get; set; }

    //    #region 기본 정보 탭 - Label

    //    /// <summary>
    //    /// 공통 (컨테이너 정보, 파일정보) - 구분 
    //    /// </summary>
    //    public string exp_DivLabel { get; set; }

    //    /// <summary>
    //    /// 공통 (컨테이너 정보, 파일정보) - 내용
    //    /// </summary>
    //    public string exp_ContentsLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 작성자
    //    /// </summary>
    //    public string exp_CreateUserLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 버전
    //    /// </summary>
    //    public string exp_VersionLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 제목
    //    /// </summary>
    //    public string exp_TitleLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 소프트웨어
    //    /// </summary>
    //    public string exp_SoftLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 소프트웨어버전
    //    /// </summary>
    //    public string exp_SoftVerLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 키워드/태그
    //    /// </summary>
    //    public string exp_KeywordTagLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 파일유형
    //    /// </summary>
    //    public string exp_FileTypeCodeLabel { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보 - 마일스톤
    //    /// </summary>
    //    public string exp_TaskDetailLabel { get; set; }

    //    /// <summary>
    //    /// 파일정보
    //    /// </summary>
    //    public string exp_FileNameLabel { get; set; }

    //    #endregion 기본 정보 탭 - Label

    //    /// <summary>
    //    /// 연관된 부모 컨테이너 정보 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> LinkedNodeInfoList { get; set; }

    //    // TODO : 연관된 컨테이너 메타 데이터 리스트 객체 "LinkedNodeMetaDataList" 필요시 사용 예정 (2023.11.01 jbh)
    //    /// <summary>
    //    /// 연관된 컨테이너 메타 데이터 리스트 
    //    /// </summary>
    //    public List<Dictionary<string, object>> LinkedNodeMetaDataList { get; set; }

    //    // TODO : 추후 필요시 "컨테이너 정보, 파일정보" 리스트 객체 containerDetailList 구현 예정 (2023.10.17 jbh)
    //    /// <summary>
    //    /// 컨테이너 정보, 파일정보 리스트
    //    /// </summary>
    //    // public List<Dictionary<string, object>> ContainerDetailList { get; set; }

    //    /// <summary>
    //    /// 컨테이너 정보, 파일정보 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> DetailDic { get; set; }

    //    /// <summary>
    //    /// 파일유형 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> FileTypeCodeDic { get; set; }

    //    /// <summary>
    //    /// 메타데이터 정보 리스트
    //    /// </summary>
    //    public List<Dictionary<string, object>> MetaDataList { get; set; }

    //    // TODO : 추후 필요시 "마일스톤 업무" 리스트 객체 TaskDetailList 구현 예정 (2023.10.17 jbh)
    //    /// <summary>
    //    /// 마일스톤 업무 리스트
    //    /// </summary>
    //    // public List<Dictionary<string, object>> TaskDetailList { get; set; }

    //    /// <summary>
    //    /// 마일스톤 업무 Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> TaskDetailDic { get; set; }

    //    /// <summary>
    //    /// 마일스톤 - 특정 마일스톤 업무 정보
    //    /// </summary>
    //    public Dictionary<string, object> MilestoneTaskInfoDic { get; set; }

    //    /// <summary>
    //    /// ComboBox - 파일유형 리스트
    //    /// </summary>
    //    public List<FileTypeView> FileTypeList { get; set; }


    //    /// <summary>
    //    /// ComboBox - 마일스톤 리스트
    //    /// </summary>
    //    public List<TaskDetailView> TaskDetailList { get; set; }

    //    // TODO : 컨테이너 정보 영역 "파일유형" ComboBox 구현시 바인딩할 BindableCollection 객체 FileType 필요시 사용 예정 (2023.10.20 jbh)
    //    // 참고 URL - https://www.reddit.com/r/csharp/comments/126sdo6/wpf_binding_a_dictionary_value_to_a_combobox/
    //    // 참고 2 URL - https://www.codeproject.com/Questions/536820/WPFplusBindplusandpluspopulateplusapluscombobox
    //    /// <summary>
    //    /// ComboBox - 파일유형
    //    /// </summary>
    //    // public BindableCollection<FileTypeView> FileType { get; set; }

    //    // TODO : 컨테이너 정보 영역 "마일스톤" ComboBox 구현시 바인딩할 BindableCollection 객체 TaskDetailType 필요시 사용 예정 (2023.10.20 jbh)
    //    // 참고 URL - https://www.reddit.com/r/csharp/comments/126sdo6/wpf_binding_a_dictionary_value_to_a_combobox/
    //    // 참고 2 URL - https://www.codeproject.com/Questions/536820/WPFplusBindplusandpluspopulateplusapluscombobox
    //    /// <summary>
    //    /// ComboBox - 마일스톤
    //    /// </summary>
    //    // public BindableCollection<TaskDetailView> TaskDetailType { get; set; }

    //    /// <summary>
    //    /// ComboBox "파일유형" List 생성 메서드 ContainerInfoTypeListCreate 실행시 화면에 출력할 ComboBox 데이터 목록
    //    /// </summary>
    //    public Param SelectParam { get; set; } = new Param();

    //    #region 생성자

    //    public ContainerDetailView()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //// TODO : 뷰(ExplorerV.xaml)에 구현된 ComboBox 파일유형, 마일스톤에서 사용하는 속성 SelectedItem에 바인딩되는
    ////        바인딩되는 UpdateParam클래스 객체 Param의 바인딩 객체(FileTypeCodeInfo, taskDetail)에 저장된 값 변경시 
    ////        뷰모델 (ExplorerVM.cs)에 해당 바인딩 객체(FileTypeCodeInfo, taskDetail)의 값이 변경되었다는 프로퍼티 체인지 이벤트(NotifyOfPropertyChange();)를
    ////        실행하기 위해서 UpdateParam 클래스는 부모 클래스로 ValidatingModelBase를 상속 받도록 구현 (2023.10.23 jbh)
    //// 참고 URL - https://kaki104.tistory.com/749
    ///// <summary>
    ///// ComboBox "파일유형" List 생성 메서드 ContainerInfoTypeListCreate 실행시 ComboBox에 할당될 데이터 목록
    ///// </summary>
    //public class Param : BindableBase
    //{
    //    // TODO : 수정 메서드 "UpdateAsync" 실행시 전달할 수정된 메서드 파라미터 필요시 추가 예정 (2023.10.23 jbh)

    //    /// <summary>
    //    /// 파일유형
    //    /// </summary>
    //    public FileTypeView FileTypeCodeInfo { get => _FileTypeCodeInfo; set { _FileTypeCodeInfo = value; NotifyOfPropertyChange(); } }
    //    private FileTypeView _FileTypeCodeInfo;

    //    // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 jbh)
    //    /// <summary>
    //    /// 마일스톤
    //    /// </summary>
    //    public TaskDetailView TaskDetail { get => _TaskDetail; set { _TaskDetail = value; NotifyOfPropertyChange(); } }
    //    private TaskDetailView _TaskDetail;

    //    #region 생성자

    //    public Param()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    ///// <summary>
    ///// ComboBox - 파일유형
    ///// </summary>
    //public class FileTypeView
    //{
    //    /// <summary>
    //    /// systemCode - typmodel, typreprt
    //    /// </summary>
    //    public string exp_systemCode { get; set; }

    //    /// <summary>
    //    /// codeName - 모델 / 보고서
    //    /// </summary>
    //    public string exp_codeName { get; set; }

    //    #region 생성자

    //    public FileTypeView(string systemCode, string codeName)
    //    {
    //        this.exp_systemCode = systemCode;
    //        this.exp_codeName = codeName;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //// TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측에서 마일스톤 RestAPI 공유 받은 후 개발 진행 (2023.10.23 jbh)
    ///// <summary>
    ///// ComboBox - 마일스톤
    ///// </summary>
    //public class TaskDetailView
    //{
    //    /// <summary>
    //    /// 마일스톤 인덱스 (웹서버로 부터 받은 JSON 데이터 중 "taskDetail"에 존재하는 "taskIdx"와 "idx"는 값이 같음.)
    //    /// </summary>
    //    // public int exp_taskIdx { get; set; }
    //    public string exp_idx { get; set; }

    //    /// <summary>
    //    /// 마일스톤 타입 title - 관련업무선택, 요구사항정의, 엔터티도출, 요구사항정의서 작성, UI설계서 작성
    //    /// </summary>
    //    public string exp_title { get; set; }

    //    #region 생성자

    //    public TaskDetailView(string idx, string title)
    //    {
    //        this.exp_idx = idx;
    //        this.exp_title = title;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}


    ///// <summary>
    ///// 연관 컨테이너 정보
    ///// </summary>
    //public class LinkedNodeView : ITreeNode
    //{
    //    /// <summary>
    //    /// 연관 컨테이너 이름
    //    /// </summary>
    //    public string exp_TreeName { get; set; }

    //    // TODO : 파일 하위 자식 "ChildNodes" 필요시 사용 예정 (2023.09.18 jbh)
    //    /// <summary>
    //    /// 파일 하위 자식 노드
    //    /// </summary>
    //    public List<ITreeNode> ChildNodes { get; set; } = new List<ITreeNode>();
    //    //public List<ITreeNode> ChildNodes
    //    //{
    //    //    get
    //    //    {
    //    //        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
    //    //        // 참고 URL - https://husk321.tistory.com/405
    //    //        // 싱글톤 패턴 
    //    //        if (_ChildNodes is null)
    //    //        {
    //    //            _ChildNodes = new List<ITreeNode>();
    //    //        }
    //    //        return _ChildNodes;
    //    //    }
    //    //    set => _ChildNodes = value;
    //    //}
    //    //private List<ITreeNode> _ChildNodes;

    //    // TODO : 필요시 프로퍼티 LinkedNodeInfoDic 사용 예정 (2024.08.21 jbh)
    //    /// <summary>
    //    /// 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너) Dictionary
    //    /// </summary>
    //    public Dictionary<string, object> LinkedNodeInfoDic { get; set; }

    //    #region 생성자

    //    public LinkedNodeView(string sourceId)
    //    {
    //        this.exp_TreeName = sourceId;
    //        this.LinkedNodeInfoDic = new Dictionary<string, object>();
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //// TODO : 컨텍스트 메뉴(ContextMenu) 정보 클래스 "ContextMenuView" 필요시 구현 예정 (2023.12.13 jbh)
    ///// <summary>
    ///// 컨텍스트 메뉴(ContextMenu) 정보 
    ///// </summary>
    //public class ContextMenuView
    //{
    //    /// <summary>
    //    /// 컨텍스트 메뉴(ContextMenu) 비활성화 여부
    //    /// </summary>
    //    public bool exp_IsEnabled { get; set; }

    //    /// <summary>
    //    /// 컨텍스트 메뉴(ContextMenu) Header
    //    /// </summary>
    //    public string exp_Header { get; set; }

    //    #region 생성자

    //    public ContextMenuView(bool isEnabled, string header)
    //    {
    //        this.exp_IsEnabled = isEnabled;
    //        this.exp_Header = header;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //#endregion ExplorerVM

    //#region ExplorerRestServer

    //#region ProjectPack

    ///// <summary>
    ///// 프로젝트(+팀) 정보
    ///// </summary>
    //public class ProjectPack
    //{
    //    /// <summary>
    //    /// 요청 파라미터 - 프로젝트 아이디 
    //    /// </summary>
    //    public string projectId { get; set; }

    //    /// <summary>
    //    /// 요청 파라미터 - 팀 아이디
    //    /// </summary>
    //    public string teamId { get; set; }

    //    #region 생성자

    //    public ProjectPack(string projectId, string teamId)
    //    {
    //        this.projectId = projectId;
    //        this.teamId = teamId;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //#endregion ProjectPack

    //#region ContainerPack

    ///// <summary>
    ///// 컨테이너 정보
    ///// </summary>
    //public class ContainerPack
    //{
    //    /// <summary>
    //    /// 요청 파라미터 - 프로젝트 아이디 
    //    /// </summary>
    //    public string projectId { get; set; }

    //    /// <summary>
    //    /// 요청 파라미터 - 팀 아이디
    //    /// </summary>
    //    public string teamId { get; set; }

    //    /// <summary>
    //    /// 요청 파라미터 - 컨테이너 아이디
    //    /// </summary>
    //    public string containerId { get; set; }

    //    /// <summary>
    //    /// 체크아웃 - 연관 컨테이너 아이디
    //    /// </summary>
    //    public string linkedContainerId { get; set; }

    //    #region 생성자

    //    public ContainerPack(string projectId, string teamId, string containerId, string linkedContainerId = null)
    //    {
    //        this.projectId = projectId;
    //        this.teamId = teamId;
    //        this.containerId = containerId;
    //        this.linkedContainerId = linkedContainerId;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //#endregion ContainerPack

    //#region FilePack

    ///// <summary>
    ///// 파일 정보
    ///// </summary>
    //public class FilePack
    //{
    //    #region Rest API 7. 파일 다운로드 사용

    //    /// <summary>
    //    /// 요청 데이터 - 파일 아이디 
    //    /// </summary>
    //    public string fileId { get; set; }

    //    #endregion Rest API 7. 파일 다운로드 사용

    //    #region 공통 - Rest API 7. 파일 다운로드 / 10. 컨테이너 체크아웃 + 다운로드

    //    /// <summary>
    //    /// 요청 데이터 또는 응답 결과 - 파일명
    //    /// </summary>
    //    public string fileName { get; set; }

    //    #endregion 공통 - Rest API 7. 파일 다운로드 / 10. 컨테이너 체크아웃 + 다운로드

    //    #region Rest API 11. 컨테이너 체크인 + 업로드

    //    /// <summary>
    //    /// 요청 데이터 - 해당 파일을 업로드 하려는 상위 폴더 아이디
    //    /// </summary>
    //    // public string folderId { get; set; }

    //    /// <summary>
    //    /// 요청 데이터 - 업로드할 파일명
    //    /// </summary>
    //    // public string uploadFiles { get; set; }

    //    /// <summary>
    //    /// 요청 데이터 - 파일 사이즈
    //    /// </summary>
    //    // public string fileSize { get; set; }

    //    #endregion Rest API 11. 컨테이너 체크인 + 업로드

    //    #region 생성자

    //    public FilePack(string fileId, string fileName)
    //    {
    //        this.fileId = fileId;
    //        this.fileName = fileName;
    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //#endregion FilePack

    //#region CheckOut

    ///// <summary>
    ///// 컨테이너 체크아웃
    ///// </summary>
    //public class CheckOut
    //{
    //    /// <summary>
    //    /// 응답결과 - resultData
    //    /// </summary>
    //    public string resultData { get; set; }

    //    /// 응답결과 - 체크아웃 성공 여부 
    //    public int resultCount { get; set; }

    //    /// <summary>
    //    /// Http 통신 결과 메시지
    //    /// </summary>
    //    public string resultMessage { get; set; }

    //    /// <summary>
    //    /// 테넌트 아이디
    //    /// </summary>
    //    public string tenantId { get; set; }

    //    /// <summary>
    //    /// jwt 토큰
    //    /// </summary>
    //    public string jwt { get; set; }

    //    public string refreshToken { get; set; }

    //    public string param { get; set; }

    //    public string payload { get; set; }

    //    public string actionUserInfo { get; set; }

    //    #region 생성자

    //    public CheckOut()
    //    {

    //    }

    //    #endregion 생성자 

    //    #region Sample

    //    #endregion Sample
    //}

    //#endregion CheckOut

    //#endregion ExplorerRestServer

    #endregion Sample
}
