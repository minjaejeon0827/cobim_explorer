using Serilog;
using Stylet;
using StyletIoC;

using System;
using System.Reflection;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.LogManager;
using CobimExplorer.Settings;
using CobimExplorer.Services;
using CobimExplorer.Utils.String;
using CobimExplorer.ViewModels.Windows.LoadingWork;

using CobimExplorer.Rest.Api.CobimBase.Login;
using CobimExplorer.Data.Models.CobimBase.Login;

namespace CobimExplorer.ViewModels.Windows.Login
{
    // TODO : 추후 로그인 뷰모델(LoginVM.cs) 소스파일 명칭 (LogInVM.cs)로 변경 및 프로젝트 파일 "CobimExplorer.Data.Models"에 LogInView.cs, TLogInView.cs 추가 예정 (2023.11.30 jbh)
    // TODO : 로그인 뷰모델의 부모 클래스 CobimScreen 추후 변경 예정 (CobimScreen<LoginView>) (2023.12.01 jbh)
    public class LoginVM : CobimScreen<LoginView>  // PageBase, IPageBase, 
    {
        #region 프로퍼티 

        /// <summary>
        /// 유효성 검사 - 1. 로그인 (POST) Rest API 요청 데이터 타입
        /// </summary>
        private enum EnumRequestDataType : int
        {
            [System.ComponentModel.Description("테넌트 아이디")]
            TENANTID = 0,
            [System.ComponentModel.Description("로그인 아이디")]
            LOGINID = 1,
            [System.ComponentModel.Description("비밀번호")]
            PASSWORD = 2,
        }

        /// <summary>
        /// 종료 Command
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// 로그인 Command
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// 테스트 Command
        /// </summary>
        public ICommand TestCommand { get; set; }

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh)
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        // TODO : 필요시 프로퍼티 "WindowManager" 사용 예정 (2024.01.04 jbh)
        //[Inject]
        //public IWindowManager WindowManager { get; set; }

        /// <summary>
        /// 로그인 요청 서버 주소 
        /// </summary>
        public string ServerPath { get => _ServerPath; set { _ServerPath = value; NotifyOfPropertyChange(nameof(_ServerPath)); } }
        private string _ServerPath = LoginHelper.requestLoginURL;

        /// <summary>
        /// 로그인 요청 서버 주소 BindableCollection 
        /// </summary>
        public BindableCollection<string> ServerPaths { get; } = new BindableCollection<string>();

        // TODO : 로그인 테마 설정 프로퍼티 "IsDarkTheme" 필요시 사용 예정 (2023.11.07 jbh)
        /// <summary>
        /// 로그인 테마 설정
        /// </summary>
        public bool IsDarkTheme { get => _IsDarkTheme; set { _IsDarkTheme = value; NotifyOfPropertyChange(nameof(IsDarkTheme)); } }
        private bool _IsDarkTheme;

        /// <summary>
        /// 로그인 (Login)
        /// </summary>
        public bool IsLogin { get => _IsLogin; set { _IsLogin = value; NotifyOfPropertyChange(nameof(IsLogin)); } }
        private bool _IsLogin;

        /// <summary>
        /// 종료 (Exit)
        /// </summary>
        public bool IsExit { get => _IsExit; set { _IsExit = value; NotifyOfPropertyChange(nameof(IsExit)); } }
        private bool _IsExit;


        // TODO : bool 자료형 프로퍼티 "IsLoginWorking"
        //        private bool 프로퍼티 및 NotifyOfPropertyChange(nameof(~~~)) 참조(사용) 안함. (2024.09.04 jbh)
        // private bool 프로퍼티 및 NotifyOfPropertyChange(nameof(~~~)) 참조(사용) 안 하는 이유?
        // "로그인" 기능 실행시 처리 중인 상태에서 해당 기능을 중복 요청하지 못 하도록
        // 버튼 "로그인"이 클릭되지 못하도록
        // 비활성화(IsEnabled = false 또는 Visibility= Visibility.Collapsed) 처리 하기 위해서 이고
        // 해당 기능을 실행하기 위해서 바인딩한 객체(프로퍼티)에 할당된 값이
        // True면 False, False면 True로 반대의 값으로 리턴되는 Converter 클래스 "InvertBooleanConverter.cs" 구현

        /// <summary>
        /// 로그인 처리 진행 여부 
        /// </summary>
        public bool IsLoginWorking { get => _IsLoginWorking; set { _IsLoginWorking = value; NotifyOfPropertyChange(nameof(IsLoginWorking)); } }
        private bool _IsLoginWorking;

        /// <summary>
        /// 테넌트 아이디
        /// </summary>
        public string TenantId { get; set; }
        // public string TenantId { get => this._TenantId; set { _TenantId = value; NotifyOfPropertyChange(nameof(_TenantId)); } }
        // private string _TenantId;

        /// <summary>
        /// 로그인 아이디
        /// </summary>
        public string LoginId { get; set; }
        // public string LoginId { get => this._LoginId; set { _LoginId = value; NotifyOfPropertyChange(nameof(_LoginId)); } }
        // private string _LoginId;

        /// <summary>
        /// 비밀번호
        /// </summary>
        public string Password { get; set; }
        // public string Password { get => _Password; set { _Password = value; NotifyOfPropertyChange(nameof(_Password)); } }
        // private string _Password;

        /// <summary>
        /// 비밀번호 문자열 공백(Null)여부 확인
        /// </summary>
        //public bool PasswordNullCheck 
        //{
        //    get => _PasswordNullCheck;
        //    set
        //    {
                   // TODO : 삼항 연산자에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                   // 참고 URL - https://husk321.tistory.com/405
        //        _PasswordNullCheck = Password is null ? true : (Password == "" ? true : false);
        //        NotifyOfPropertyChange(nameof(_PasswordNullCheck));
        //    }
        //} 
        //private bool _PasswordNullCheck;

        /// <summary>
        /// HttpClient client (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        [Inject]
        public HttpClient _Client { get; set; }

        // TODO : 로그인 정보 프로퍼티 "LoginPack"을 싱글톤으로 구현 후 테스트 예정 (2024.01.02 jbh)
        /// <summary>
        /// 로그인 정보 
        /// </summary>
        //public LoginPack LoginPack { get; set; }
        private LoginPack LoginPack { get; set; }

        /// <summary>
        /// 로그인 토큰 키 (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        //[Inject]
        //public LoginHelper.Login_Access_Token Token { get; set; }

        /// <sumary>
        /// 사용자 정보
        /// </sumary>
        //[Inject]
        //public UserInfoView UserInfo { get => _UserInfo; set { _UserInfo = value; NotifyOfPropertyChange(); } }
        //private UserInfoView _UserInfo;

        /// <summary>
        /// 탐색기 화면
        /// </summary>\
        // public ExplorerVM explorerVM { get; set; }

        /// <summary>
        /// 로그인 뷰모델
        /// </summary>
        public LoginVM loginVM { get; set; }

        // private readonly PaletteHelper paletteHelper = new PaletteHelper();

        #endregion 프로퍼티 

        #region 생성자 

        public LoginVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;

            ExitCommand  = new ButtonCommand(CobimHelper.VoidMethodType,  ExitApp,    CanExecuteMethod);
            LoginCommand = new ButtonCommand(CobimHelper.AsyncMethodType, LoginAsync, CanExecuteMethod);
            TestCommand  = new ButtonCommand(CobimHelper.AsyncMethodType, TestAsync,  CanExecuteMethod);
        }

        #endregion 생성자

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            loginVM = this;
            InitSetting();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        //private bool CanExecuteMethod(object arg)
        //{
        //    throw new NotImplementedException();
        //}

        // TODO : 메서드 "CanExecuteMethod" 필요시 수정 예정 (2023.10.4 jbh)
        private bool CanExecuteMethod(object obj)
        {
            return true;
        }

        #endregion 기본 메소드

        #region InitSetting

        // TODO : 초기 셋팅 메서드 "InitSetting" 필요시 구현하기 (2024.11.04 jbh)
        // 참고 URL - https://chat.openai.com/c/b9d52fcf-aab0-49e3-8008-8852767a9982
        /// <summary>
        /// 초기 셋팅 
        /// </summary>
        private void InitSetting()
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // 데이터 모델 객체 InputParamData에 HttpClient client 객체 값 할당 
                InputParamData.SetHttpClient(_Client);
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion InitSetting

        #region ToShowData 

        public LoginVM ToShowData()
        {
            this.WindowManager.ShowDialog((object)this);
            return this;
        }

        #endregion ToShowData

        #region toggleTheme

        // TODO : 추후 필요시 테마 색상 변경 메서드 "toggleTheme" 구현 예정 (2023.08.17 jbh)
        /// <summary>
        /// 테마 색상 변경 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void toggleTheme(object sender, RoutedEventArgs e)
        //{
        //    ITheme theme = paletteHelper.GetTheme();

        //    if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
        //    {
        //        IsDarkTheme = false;
        //        theme.SetBaseTheme(Theme.Light);
        //    }
        //    else
        //    {
        //        IsDarkTheme = true;
        //        theme.SetBaseTheme(Theme.Dark);
        //    }
        //    paletteHelper.SetTheme(theme);
        //}

        #endregion toggleTheme

        #region SaveOption

        /// <summary>
        /// 테스트용 로그인 설정값 임시 저장
        /// </summary>
        private void SaveOption(LoginResponseData responseData)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                AppSetting.Default.LoginBase.SetLoginId(this.LoginId);
                AppSetting.Default.LoginBase.SetPassword(this.Password);
                LoginView.SetRequestHeader(new RequestHeader(this.TenantId, responseData.resultData, responseData.accessKey, responseData.refreshToken));

                // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.09.23 jbh)
                // AppSetting.Default.LoginBase.SetRequestHeader(this.TenantId, responseData.resultData);
                // AppSetting.Default.LoginBase.SetLoginServerPath(this.LoginServerPath);
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + "SaveOption Error = " + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            return;
        }

        #endregion SaveOption

        #region IsValidator

        // TODO : 유효성 검사 메서드 IsValidator 로직 필요시 수정 예정 (2024.08.21 jbh)
        /// <summary>
        /// 유효성 검사 - 테넌트 아이디(TenantId), 로그인 아이디(LoginID), 비밀번호(Password) 문자열 입력 여부 확인
        /// </summary>
        private bool IsValidator(EnumRequestDataType pEnumRequestDataType)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                switch(pEnumRequestDataType)
                {
                    case EnumRequestDataType.TENANTID:      // 테넌트 아이디
                        if(true == string.IsNullOrWhiteSpace(TenantId)) throw new Exception($"[{EnumRequestDataType.TENANTID.ToDescription()}]\r\n입력하지 않았습니다!");
                        break;

                    case EnumRequestDataType.LOGINID:       // 로그인 아이디
                        if(true == string.IsNullOrWhiteSpace(LoginId)) throw new Exception($"[{EnumRequestDataType.LOGINID.ToDescription()}]\r\n입력하지 않았습니다!");
                        break;

                    case EnumRequestDataType.PASSWORD:      // 비밀번호
                        if(true == string.IsNullOrWhiteSpace(Password)) throw new Exception($"[{EnumRequestDataType.PASSWORD.ToDescription()}]\r\n입력하지 않았습니다!");
                        break;

                    default:
                        throw new Exception("유효성 검사 오류!!\r\n관리자에게 문의 하세요.");
                        // break;
                }
                return true;
            }
            catch(Exception ex)
            {
                // TODO : 필요시 에러 로그 기록 사용 예정 (2024.07.03 jbh)
                // Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                //TaskDialog.Show(HTSHelper.ErrorTitle, ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
                return false;
            }
        }

        #endregion IsValidator

        #region LoginAsync

        /// <summary>
        /// 로그인
        /// </summary>
        private async Task LoginAsync(object obj)
        {
            // TODO : 아래 3가지 if절 유효성 검사 진행 로직 필요시 수정 예정 (2024.08.21 jbh)
            // 유효성 검사 진행 
            if(false == IsValidator(EnumRequestDataType.TENANTID)) return;   // 테넌트 아이디 입력하지 않은 경우 
            if(false == IsValidator(EnumRequestDataType.LOGINID)) return;    // 로그인 아이디 입력하지 않은 경우 
            if(false == IsValidator(EnumRequestDataType.PASSWORD)) return;   // 비밀번호 입력하지 않은 경우 

            var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.Login_LoadingWork);   // 로그인 처리 중 화면 뷰모델 할당

            // TODO : 추후 필요시 문자열 앞뒤 공백 제거(Trim) 사용 로직으로 변경 (2023.08.22 jbh)
            // 참고 URL - https://developer-talk.tistory.com/660
            string inputTenantId = TenantId.Trim();
            string clientID      = LoginId.Trim();
            string clientPW      = Password.Trim();

            string resMsg = string.Empty;                         // 로그인 처리 결과(성공 또는 실패) 문자열 초기화  

            var currentMethod = MethodBase.GetCurrentMethod();    // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            { 
                if(true == IsLoginWorking)   // 로그인 처리 중인 경우
                    throw new Exception(CobimHelper.Login_LoadingWork);

                IsLoginWorking = true;

                InputParamData.SetLoginPack(new LoginPack(inputTenantId, clientID, clientPW));

                loadingWorkVM.ShowWindow();   // 로그인 처리 중 화면 출력 

                var responseData = await LoginRestServer.PostLoginAsync(InputParamData);   // 로그인 처리 진행 

                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if(responseData.resultData is not null && responseData.resultMessage is null)
                {
                    resMsg = "success";                 // 로그인 성공  
                    loginVM.SaveOption(responseData);   // 로그인 정보 임시 저장
                    loginVM.IsLogin = true;
                    this.RequestClose(loginVM.IsLogin);   // 로그인 성공시 로그인 화면(Window - LoginV.xaml) 종료 
                }
                else throw new Exception($"로그인 {responseData.resultCount}회 실패!");   // 로그인 실패 
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
                resMsg = ex.Message;  // 인재 INC로 Rest API 통신 결과 오류 메시지 발생시 string 클래스 객체 resMsg에 할당
                loginVM.IsLogin = false;
            }
            finally
            {
                // 로그인 처리 중이 아닌 경우
                if(resMsg != CobimHelper.Login_LoadingWork)
                {
                    IsLoginWorking = false;   // 로그인 처리 중 종료 
                    // TODO : 로그인 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 jbh)
                    // 참고 URL - https://cosmosproject.tistory.com/563        
                    loadingWorkVM.RequestClose(null);   // 로그인 처리 중 화면 종료
                }
            }
            return;
        }

        #endregion LoginAsync

        #region TestAsync

        private async Task TestAsync(object obj)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.11.07 jbh)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                //MessageBox.Show("테스트");

                return;
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }


        #endregion TestAsync

        #region ExitApp

        /// <summary>
        /// 종료 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApp(object obj)
        {
            loginVM.IsExit = true;

            this.RequestClose(loginVM.IsExit);
        }

        #endregion ExitApp

        #region Sample

        // TODO : 필요시 아래 주석친 메서드 "SaveOption" 참고 (2024.10.29 jbh)
        /// <summary>
        /// 테스트용 로그인 설정값 임시 저장
        /// </summary>
        // private void SaveOption(Login_Access_Token login_access_token)
        // {
        //     // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
        //     // 참고 URL - https://slaner.tistory.com/73
        //     // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        //     // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        //     var currentMethod = MethodBase.GetCurrentMethod();

        //     try
        //     {
        //         //if (!this.ServerPaths.Contains(this.ServerPath))
        //         if (false == this.ServerPaths.Contains(this.ServerPath)) this.ServerPaths.Add(this.ServerPath);

        //         AppSetting.Default.Login.ServerPath = this.ServerPath;
        //         //AppSetting.Default.Login.SiteViewCode = this.SiteViewCode;

        //         // TODO : 추후 필요시 "this.LoginID", "this.Password" 추가 예정 (2023.08.30 jbh)
        //         AppSetting.Default.Login.LoginId = this.LoginID;
        //         AppSetting.Default.Login.Password = this.Password;

        //         AppSetting.Default.Login.ServerPaths = this.ServerPaths.ToList<string>();
        //         // AppSetting.Default.Login.Authorization = login_access_token;

        //         // TODO : 추후 필요시 아래 "LoginView.RequestHeaders" 로직 수정 예정 (2023.12.22 jbh)
        //         LoginView.RequestHeaders.Tenant = LoginHelper.inputTenantId;
        //         LoginView.RequestHeaders.Authorization = login_access_token.resultData;


        //         // TODO : 응용 프로그램 기본 설정 클래스 (AppSetting) 중 ProjectSetting 클래스 프로퍼티 "Project"에
        //         //        테스트 프로젝트 아이디(TestProjectId), 테스트 팀 아이디(TestTeamId) 추가 구현 (2023.09.04 jbh)
        //         //        추후 필요 없을시 삭제 예정
        //         // AppSetting.Default.Project.TestProjectId = ProjectHelper.testProjectId;
        //         // AppSetting.Default.Project.TestTeamId = ProjectHelper.testTeamId;
        //     }
        //     catch(Exception ex)
        //     {
        //         Log.Error(Logger.GetMethodPath(currentMethod) + "SaveOption Error = " + ex.Message);
        //         throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw 
        //     }
        // }

        // TODO : ASP .NET CORE 웹서비스 프로젝트 파일 "CobimExplorer.Core.Rest.Api" 필요시 구현 예정 (2023.11.07 jbh)
        // 참고 URL - https://ojava.tistory.com/63
        // 참고 2 URL - https://blog.naver.com/icysword/140180123082
        //private async Task LoginAsync(object obj)
        //{
        //    // TODO : 아래 3가지 if절 유효성 검사 진행 로직 필요시 수정 예정 (2024.08.21 jbh)
        //    // 유효성 검사 진행 
        //    if (false == IsValidator(EnumRequestDataType.TENANTID)) return;   // 테넌트 아이디 입력하지 않은 경우 
        //    if (false == IsValidator(EnumRequestDataType.LOGINID)) return;    // 로그인 아이디 입력하지 않은 경우 
        //    if (false == IsValidator(EnumRequestDataType.PASSWORD)) return;   // 비밀번호 입력하지 않은 경우 

        //    var loadingWorkVM = Container.Get<CommonLoadingWorkVM>().SetMessage(CobimHelper.Login_LoadingWork);   // 로그인 처리 중 화면 뷰모델 할당

        //    // TODO : 추후 필요시 문자열 앞뒤 공백 제거(Trim) 사용 로직으로 변경 (2023.08.22 jbh)
        //    // 참고 URL - https://developer-talk.tistory.com/660
        //    string inputTenantId = TenantId.Trim();
        //    string clientID = LoginId.Trim();
        //    string clientPW = Password.Trim();
        //    // string clientPW = PasswordHelper.Password;
        //    // string testID   = LoginID.Trim();
        //    // string testPW   = Password.Trim();

        //    // TODO : 테스트 로그인시 사용할 테넌드 아이디(LoginHelper.inputTenantId), 아이디(LoginHelper.testid), 비밀번호(LoginHelper.testpassword) 구현 (2023.08.23 jbh)
        //    // string inputTenantId = LoginHelper.inputTenantId;
        //    // string clientID = LoginHelper.testId;
        //    // string clientPW = LoginHelper.testPassword;

        //    var currentMethod = MethodBase.GetCurrentMethod();    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // MessageBox.Show("로그인 테스트 중입니다.");

        //        // TODO : 아래 주석친 코드 필요시 참고 (2024.09.04 jbh)
        //        // 로그인 처리 중인 상태에서 로그인을 중복 요청하는 경우 
        //        // if(true == IsLoginWorking)
        //        //     throw new Exception("현재 로그인 진행 중입니다.\r\n로그인 완료까지 잠시 기다려주세요.");

        //        LoginPack = new LoginPack(inputTenantId, clientID, clientPW);

        //        // 로그인 처리 중 화면 출력 
        //        if(false == IsLoginWorking)
        //        {
        //            IsLoginWorking = true;
        //            loadingWorkVM.ShowWindow();
        //        }

        //        // 로그인 테스트용 코드
        //        // 로그인 처리 중 화면이 정상 출력되는지 1분간 대기 
        //        // await Task.Delay(TimeSpan.FromMinutes(1));

        //        // TODO : 테스트 용 코드 필요 시 사용 예정 (2024.01.05 jbh)
        //        // 테스트 처리 중 화면 출력 직후 30초 대기 
        //        // await Task.Delay(TimeSpan.FromSeconds(30));
        //        // await Task.Delay(TimeSpan.FromSeconds(20));

        //        Log.Information(Logger.GetMethodPath(currentMethod) + "Http 통신 연결 시도");

        //        // TODO : Rest API POST 방식 사용자 로그인 static 메서드 "PostUserLogin" 호출문 구현 (2023.08.18 jbh)
        //        var token = await UserRestServer.PostLoginAsync(Client, LoginPack);


        //        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
        //        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        if (token.resultData is not null && token.resultMessage is null)
        //        {
        //            Log.Information(Logger.GetMethodPath(currentMethod) + "Http 통신 연결 성공");

        //            Log.Information(Logger.GetMethodPath(currentMethod) + "로그인 성공");

        //            // [테스트 완료] TODO : 로그 레벨(Debug, Verbose, Warning, Error, Fatal) 기록 테스트 진행 (2023.10.11 jbh)
        //            //Log.Debug(Logger.GetMethodPath(currentMethod) + "테스트 Debug 로그인");
        //            //Log.Verbose(Logger.GetMethodPath(currentMethod) + "테스트 Verbose 로그인");
        //            //Log.Warning(Logger.GetMethodPath(currentMethod) + "테스트 Warning 로그인");
        //            //Log.Error(Logger.GetMethodPath(currentMethod) + "테스트 Error 로그인");
        //            //Log.Fatal(Logger.GetMethodPath(currentMethod) + "테스트 Fatal 로그인");

        //            //MessageBox.Show("로그인 성공!");

        //            // 로그인 성공
        //            IsLoginWorking = false;
        //            loginVM.IsLogin = true;

        //            // 로그인 정보 저장
        //            loginVM.SaveOption(token);


        //            // ExplorerVM explorerVM = Container.Get(ExplorerVM).Set();

        //            // TODO : 로그인 성공시 화면(Window - LoginV.xaml) 종료 구현 (2023.08.17 jbh)
        //            // OnRequestClose(LoginVM, new EventArgs());
        //            this.RequestClose(loginVM.IsLogin);
        //        }
        //        else
        //        {
        //            // 로그인 실패 
        //            // Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + $"로그인 {token.resultCount}회 실패!");
        //            //Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + $"로그인 실패!");

        //            //Console.WriteLine("Login is NOT OK...");
        //            //MessageBox.Show($"로그인 {token.resultCount}회 실패!");
        //            throw new Exception($"로그인 {token.resultCount}회 실패!");
        //            // MessageBox.Show($"로그인 실패!");
        //            // Console.WriteLine("Error = " + token.resultData);

        //            //loginVM.IsLogin = false;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //        loginVM.IsLogin = false;

        //        // 로그인 처리 중인 경우 
        //        if(true == IsLoginWorking) IsLoginWorking = false;
        //    }
        //    finally
        //    {
        //        // TODO : 로그인 처리 중 화면이 ShowDialog(Modal)가 아니라 Show(Modaless)로 출력되므로 해당 화면 종료시 메서드 "RequestClose" 호출할 때, 메서드 파라미터 "null"로 할당 (2024.01.04 jbh)
        //        // 참고 URL - https://cosmosproject.tistory.com/563
        //        // 로그인 처리 중 화면 종료        
        //        loadingWorkVM.RequestClose(null);
        //    }
        //    return;
        //}

        #endregion Sample
    }
}
