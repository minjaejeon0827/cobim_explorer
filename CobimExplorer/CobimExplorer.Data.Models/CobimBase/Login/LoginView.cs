using System.Net.Http;

namespace CobimExplorer.Data.Models.CobimBase.Login
{
    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    public class LoginView<TData> : LoginView where TData : LoginView<TData> { }

    /// <summary>
    /// CobimExplorer
    /// 로그인 화면 뷰모델(LoginVM) 데이터 모델
    /// </summary>
    public class LoginView : TLoginView<LoginView>
    {
        #region 프로퍼티 

        /// <summary>
        /// HttpClient client (계속 호출하지 말고 싱글톤으로 사용해야함.)
        /// </summary>
        private HttpClient _Client { get; set; }

        // TODO : Http 웹서버 통신 요청 Header 프로퍼티 "RequestHeaders" 필요시 사용/수정 예정 (2023.12.22 minjae)
        /// <summary>
        /// Http 웹서버 통신 요청 Header
        /// </summary>
        private static RequestHeader _RequestHeader { get; set; }
        //public static RequestHeader RequestHeaders
        //{
        //    // 싱글톤 패턴 
        //    get => _RequestHeaders ?? (_RequestHeaders = new RequestHeader());

        //    set
        //    {
        //        _RequestHeaders = value;
        //        StaticChanged(nameof(RequestHeaders));
        //    }
        //}
        //private static RequestHeader _RequestHeaders;

        /// <summary>
        /// 로그인 정보 
        /// </summary>
        private LoginPack _LoginPack { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        public LoginView() : base() { }

        #endregion 생성자

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

        #region Http 웹서버 통신 요청 Header

        /// <summary>
        /// Http 웹서버 통신 요청 Header 값 셋팅 
        /// </summary>
        public static void SetRequestHeader(RequestHeader requestHeader)
        {
            _RequestHeader = requestHeader;
        }

        /// <summary>
        /// Http 웹서버 통신 요청 Header 값 가져오기 
        /// </summary>
        public static RequestHeader GetRequestHeader()
        {
            return _RequestHeader;
        }

        #endregion Http 웹서버 통신 요청 Header

        #region 로그인 정보 

        /// <summary>
        /// 로그인 정보 값 셋팅
        /// </summary>
        public void SetLoginPack(LoginPack loginPack)
        {
            _LoginPack = loginPack;
        }

        /// <summary>
        /// 로그인 정보 가져오기 
        /// </summary>
        public LoginPack GetLoginPack()
        {
            return _LoginPack;
        }

        #endregion 로그인 정보 

        #region Sample

        #endregion Sample
    }

    #region RequestHeader

    /// <summary>
    /// Http 웹서버 통신 요청 Header
    /// </summary>
    public class RequestHeader
    {
        #region 프로퍼티

        /// <summary>
        /// inputTenantId - 테넌트 아이디
        /// </summary>
        private string _Tenant { get; set; }

        /// <summary>
        /// Authorization - 로그인 토큰 키  
        /// </summary>
        private string _Authorization { get; set; }

        /// <summary>
        /// Access - 접속자 아이디 (Access Id, accessKey 같은 값을 의미, 로그인 아이디와는 다름)
        /// </summary>
        private string _Access { get; set; }

        /// <summary>
        /// Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 
        /// </summary>
        public string _Refresh { get; set; }

        #endregion 프로퍼티

        #region 생성자

        public RequestHeader(string tenant, string authorization, string access, string? refresh = null)
        {
            _Tenant = tenant;
            _Authorization = authorization;
            _Access = access;
            _Refresh = refresh;
        }

        #endregion 생성자 

        #region 테넌트 아이디

        /// <summary>
        /// inputTenantId - 테넌트 아이디 값 셋팅
        /// </summary>
        public void SetTenant(string tenant)
        {
            _Tenant = tenant;
        }

        /// <summary>
        /// inputTenantId - 테넌트 아이디 값 가져오기
        /// </summary>
        public string GetTenant()
        {
            return _Tenant;
        }

        #endregion 테넌트 아이디

        #region 로그인 토큰 키  

        /// <summary>
        /// Authorization - 로그인 토큰 키 값 셋팅 
        /// </summary>
        public void SetAuthorization(string authorization)
        {
            _Authorization = authorization;
        }

        /// <summary>
        /// Authorization - 로그인 토큰 키 값 가져오기  
        /// </summary>
        public string GetAuthorization()
        {
            return _Authorization;
        }

        #endregion 로그인 토큰 키  

        #region 로그인 아이디

        /// <summary>
        /// Access - 로그인 아이디(Access Id) 값 셋팅 
        /// </summary>
        public void SetAccess(string access)
        {
            _Access = access;
        }

        /// <summary>
        /// Access - 로그인 아이디(Access Id) 가져오기 
        /// </summary>
        public string GetAccess()
        {
            return _Access;
        }

        #endregion 로그인 아이디

        #region 로그인 재발급 토큰 키 

        /// <summary>
        /// Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 값 셋팅
        /// </summary>
        public void SetRefresh(string refresh)
        {
            _Refresh = refresh;
        }

        /// <summary>
        /// Refresh Token - 로그인 토큰 만료시 재발급을 위한 토큰 키 가져오기
        /// </summary>
        public string GetRefresh()
        {
            return _Refresh;
        }

        #endregion 로그인 재발급 토큰 키 

        #region Sample

        #endregion Sample
    }

    #endregion RequestHeader

    #region LoginPack

    /// <summary>
    /// 로그인 정보 
    /// </summary>
    public class LoginPack
    {
        #region 프로퍼티 

        // TODO : 로그인 정보 클래스 "LoginPack"안에 속하는 프로퍼티 "inputTenantId", "id", "password" 의 접근 제한자를 
        //        private로 설정하면 오류 발생하여 접근 제한자를 public으로 수정함. (2024.03.18 minjae)
        //        오류 발생 위치 - LoginRestServer.cs -> 비동기 메서드 "PostLoginAsync" 
        //        -> 메서드 "JsonSerializer.Serialize" 호출하여 json 문자열(content)을 제대로 리턴 받지 못하고 오류 발생함. 

        // TODO : 로그인 정보 클래스 "LoginPack"안에 속하는 
        //        프로퍼티 "inputTenantId", "id", "password" 프로퍼티 이름 
        //        변경하면 오류 발생하여 수정 안 하고 그대로 유지함.(2024.11.04 minjae)
        //        오류 발생 위치 - LoginRestServer.cs -> 비동기 메서드 "PostLoginAsync" 
        //        서버로 부터 제대로 리턴 받지 못하고 오류 발생함. 

        /// <summary>
        /// 요청 파라미터 - inputTenantId
        /// </summary>
        public string inputTenantId { get; set; }

        /// <summary>
        /// 요청 파라미터 - 아이디
        /// </summary>
        public string id { get; set; }
        // public string realm { get; set; }

        /// <summary>
        /// 요청 파라미터 - 비밀번호
        /// </summary>
        public string password { get; set; }

        #endregion 프로퍼티 

        #region 생성자 

        public LoginPack (string inputTenantId, string clientId, string clientPW)
        {
            this.inputTenantId = inputTenantId;
            this.id            = clientId;
            this.password      = clientPW;
        }

        #endregion 생성자
    }

    #endregion LoginPack

    #region LoginResponseData

    /// <summary>
    /// 로그인 응답 결과 데이터
    /// </summary>
    public class LoginResponseData
    {
        #region 프로퍼티 

        /// <summary>
        /// 로그인 토큰 키 (Access Token)
        /// </summary>
        public string resultData { get; set; }

        /// <summary>
        /// 로그인 실패 횟수
        /// </summary>
        public int resultCount { get; set; }

        /// <summary>
        /// 로그인 토큰 발급 요청 결과 메시지
        /// </summary>
        public string resultMessage { get; set; }

        /// <summary>
        /// 테넌트 아이디
        /// </summary>
        public string tenantId { get; set; }

        /// <summary>
        /// 로그인 토큰 만료시 재발급을 위한 토큰 키 (Refresh Token)
        /// </summary>
        public string refreshToken { get; set; }

        /// <summary>
        /// 모든 API 요청시 요청 헤더(RequestHeader)에 저장하여 
        /// 필수로 전달해야하는 정보 (Access Id, accessKey 같은 값을 의미)
        /// </summary>
        public string accessKey { get; set; }

        public string param { get; set; }

        public string payload { get; set; }

        public string actionUserInfo { get; set; }

        /// <summary>
        /// jwt 토큰
        /// </summary>
        // public string jwt { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        // TODO : 필요시 생성자 LoginResponseData 구현 예정 (2024.11.14 minjae)
        public LoginResponseData()
        {

        }

        #endregion 생성자
    }

    #endregion LoginResponseData

    #region Sample

    #endregion Sample
}
