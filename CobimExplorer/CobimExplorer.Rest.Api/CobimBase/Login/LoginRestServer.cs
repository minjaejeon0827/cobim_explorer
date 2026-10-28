using Serilog;

using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CobimExplorer.Data.Models.CobimBase.Login;
using CobimUtil;

namespace CobimExplorer.Rest.Api.CobimBase.Login
{
    // 주의사항 - Http 통신 로직 구현시 "System.Text.json"의 경우 .Net Core에서는 잘 호환(사용)되지만 .Net Framework에서는 호환되지 않고 에러가 발생하는 경우가 존재함.
    // JSON Object 클래스 생성시 필요한 라이브러리 "Newtonsoft.Json" VS  "System.Text.Json" 비교
    // 참고 URL - https://bigexecution.tistory.com/166
    // JSON Object 클래스 생성시 필요한 라이브러리 "System.Text.Json" 사용 방법 
    // 참고 URL - https://www.csharpstudy.com/Data/Json-SystemTextJson.aspx
    // 참고 2 URL - https://msjo.kr/2019/09/24/1/
    // 참고 3 URL - https://yeko90.tistory.com/entry/chow-to-parse-json
    // 주의사항 - "System.Text.Json"의 경우 NuGet package를 설치

    // C# HTTP POST 웹 요청 만들기
    // 참고 URL - https://www.delftstack.com/ko/howto/csharp/csharp-post-request/#google_vignette
    // C# HTTP POST "WebClient" VS "HttpWebRequest/HttpWebResponse" VS "HttpClient"비교 
    // 참고 URL - https://stackoverflow.com/questions/1694388/webclient-vs-httpwebrequest-httpwebresponse
    // 참고 2 URL - https://www.infoworld.com/article/3198673/when-to-use-webclient-vs-httpclient-vs-httpwebrequest.html
    // 참고 3 URL - https://im-first-rate.tistory.com/154
    // 참고 4 URL - https://forum.dotnetdev.kr/t/http/2382/2

    // TODO : 클래스 "BindableBase" 필요시 "UserRestServer"의 부모 클래스로 상속 예정 (2024.01.02 jbh)
    public class LoginRestServer // : BindableBase
    {
        #region PostLoginAsync

        // TODO : LoginView.cs 소스파일 -> 로그인 정보 클래스 "LoginPack"안에 속하는 프로퍼티 "inputTenantId", "id", "password" 의 접근 제한자를 
        //        private로 설정하면 오류 발생하여 접근 제한자를 public으로 수정함. (2024.03.18 jbh)
        //        오류 발생 위치 - LoginRestServer.cs -> 비동기 메서드 "PostLoginAsync" 
        //        -> 메서드 "JsonSerializer.Serialize" 호출하여 json 문자열(content)을 제대로 리턴 받지 못하고 오류 발생함. 

        // TODO : LoginView.cs 소스파일 -> 로그인 정보 클래스 "LoginPack"안에 속하는 
        //        프로퍼티 "inputTenantId", "id", "password" 프로퍼티 이름 
        //        변경하면 오류 발생하여 수정 안 하고 그대로 유지함.(2024.11.04 jbh)
        //        오류 발생 위치 - LoginRestServer.cs -> 비동기 메서드 "PostLoginAsync" 
        //        서버로 부터 제대로 리턴 받지 못하고 오류 발생함. 


        /// <summary>
        /// 1. 로그인 (POST)
        /// Rest API 메서드 파라미터 "LoginView pInputParamData"
        /// </summary>
        public static async Task<LoginResponseData> PostLoginAsync(LoginView pInputParamData)
        {
            LoginResponseData responseData = null;                  // 1. 로그인 (POST) Rest API 응답 결과 데이터 객체 초기화 
            HttpClient client = pInputParamData.GetHttpClient();    // HttpClient client 객체 가져오기
            LoginPack loginPack = pInputParamData.GetLoginPack();   // 로그인 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();      // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.08.20 jbh)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // TODO : LoginPack 클래스 객체 "loginPack" string 클래스 json 문자열(content) 구현 (2023.08.21 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
                // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc-001\"}";
                string requestData = JsonSerializer.Serialize(loginPack);

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using var jsonContent = new StringContent(
                                            requestData,
                                            Encoding.UTF8,
                                            LoginHelper.contentType
                                        );

                // TODO : 아래 소스코드 처럼 " _ = RefreshTokenAsync(pInputParamData);" 호출시
                //        해당 메서드 "RefreshTokenAsync" 몸체 안에 20분 지나서 Rest API 1. 로그인 (POST) 메서드 "PostLoginAsync" 호출을 하게 되므로 
                //        CobimExplorer 응용 프로그램 이 실행 되는 동안 메서드 2개가 백그라운드 쓰레드 처럼 무한으로 호출되도록 구현 (2024.01.02 jbh)
                //        ("PostLoginAsync" -> "RefreshTokenAsync" -> "PostLoginAsync" -> "RefreshTokenAsync" . . . )    

                // TODO : 20분, 1시간 혹은 그 이후에도 토큰 만료로 인한 오류가 발생하지 않도록
                //        아래처럼 토큰 재발급 비동기 메서드 "RefreshTokenAsync" 호출 시점을
                //        아래 Http Post 요청 비동기 메서드(client.PostAsync(LoginHelper.requestLoginURL, jsonContent))
                //        호출 되기 직전에 먼저 호출 하도록 구현 (2024.11.15 jbh)
                _ = RefreshTokenAsync(pInputParamData);
                // _ = TestAsync();

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient       
                using HttpResponseMessage response = await client.PostAsync(LoginHelper.requestLoginURL, jsonContent);

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 1. 로그인 Http 통신 결과가 성공할 경우 
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    responseData = JsonSerializer.Deserialize<LoginResponseData>(resMsg);

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "1. 로그인 성공");

                    return responseData;
                }
                // Rest API 1. 로그인 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);                
            }
            catch(Exception ex)
            {
                // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.08.20 jbh)
                // Log.Information(LogHelper.GetMethodPath(currentMethod) + ex.Message);
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 jbh)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }

        }

        #endregion PostLoginAsync

        #region RefreshTokenAsync

        // TODO : 로그인 토큰 만료시 토큰 갱신할 수 있도록 LoginRestServer.cs ->  비동기 Rest API 메서드 "RefreshTokenAsync" 로직 보완하기 (2024.11.11 jbh)
        /// <summary>
        /// 토큰 발급 후 20분 지나서 만료되지 않도록 토큰 재발급 처리
        /// </summary>
        private static async Task RefreshTokenAsync(LoginView pInputParamData)
        {
            // 1 단계 : 로그인 토큰 발급 직후 20분 대기
            // await Task.Delay(TimeSpan.FromMinutes(3));
            await Task.Delay(TimeSpan.FromMinutes(20));

            // 2 단계 : 토큰 만료되지 않도록 발급받고 20분 지난 직후
            // Rest API 1. 로그인 (POST) 요청 및 토큰 재발급 처리 
            var responseData = await PostLoginAsync(pInputParamData);

            // TODO : 아래 if 조건절 if (responseData.resultData is not null && responseData.resultMessage is null) 만족 못 하는 오류 발생시 else 절 추가 구현하기 (2024.01.02 jbh)
            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2024.01.02 jbh)
            // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.01.02 jbh)
            // 참고 URL - https://husk321.tistory.com/405
            if(responseData.resultData is not null && responseData.resultMessage is null)
            {
                // 3 단계 : LoginView.cs -> Http 웹서버 통신 요청 Header 프로퍼티 가져와서(LoginView.GetRequestHeader())
                //          새로 발급받은 로그인 토큰 값 할당(SetAuthorization(responseData.resultData)) (Refresh 처리) 
                LoginView.GetRequestHeader().SetAuthorization(responseData.resultData);
                LoginView.GetRequestHeader().SetRefresh(responseData.refreshToken);
            }
        }

        #endregion RefreshTokenAsync

        #region Sample

        // TODO : 아래 주석친 비동기 메서드 "RefreshTokenAsync" 필요시 참고 (2024.10.29 jbh)
        /// <summary>
        /// 토큰 발급 후 15분 지나서 만료되지 않도록 토큰 재발급 처리
        /// </summary>
        /// <returns></returns>
        //private static async Task RefreshTokenAsync(HttpClient pClient, LoginPack pLoginPack)
        //{
        //    // 1 단계 : 로그인 토큰 발급 직후 10분 대기
        //    await Task.Delay(TimeSpan.FromMinutes(10));

        //    Debug.WriteLine(DateTime.Now);

        //    // 2 단계 : 토큰 만료되지 않도록 발급받고 10분 지난 직후
        //    // Rest API 1. 로그인 (POST) 요청 및 토큰 재발급 처리 
        //    var refreshToken = await PostLoginAsync(pClient, pLoginPack);

        //    // TODO : 아래 if 조건절 if (refreshToken.resultData is not null && refreshToken.resultMessage is null) 만족 못 하는 오류 발생시 else 절 추가 구현하기 (2024.01.02 jbh)
        //    // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2024.01.02 jbh)
        //    // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2024.01.02 jbh)
        //    // 참고 URL - https://husk321.tistory.com/405
        //    if (refreshToken.resultData is not null && refreshToken.resultMessage is null)
        //    {
        //        // 3 단계 : 로그인 토큰 값 프로퍼티 "LoginView.RequestHeaders.Authorization"에 새로 발급받은 로그인 토큰 값 할당 (Refresh 처리) 
        //        LoginView.RequestHeaders.Authorization = string.Empty;
        //        LoginView.RequestHeaders.Authorization = refreshToken.resultData;
        //    }
        //    // else
        //    // {
        //    // ...... 
        //    // }
        //}

        // TODO : 아래 주석친 비동기 메서드 "TestAsync" 필요시 참고 (2024.10.29 jbh)
        // static async Task TestAsync(HttpClient pClient, LoginPack pLoginPack)
        // {
        //     while (true)
        //     {
        //         await Task.Delay(TimeSpan.FromMinutes(10));

        //         Debug.WriteLine(DateTime.Now);

        //         await PostLoginAsync(pClient, pLoginPack);
        //     }
        // }

        // TODO : 아래 주석친 비동기 메서드 "PostLoginAsync" 필요시 참고 (2024.10.29 jbh)
        //public static async Task<Login_Access_Token> PostLoginAsync(string LoginID, string PassWord)
        //{
        // // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
        // // 참고 URL - https://slaner.tistory.com/73
        // // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        // // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        // var currentMethod = MethodBase.GetCurrentMethod();
        //    try
        //    {
        //        // HttpClient 사용 (POST 방식)
        //        HttpClient pClient = new HttpClient();

        //        // TODO : 테스트 코드 - 강제로 오류 발생하도록 Exception 생성 하도록 구현 (필요시 사용)  (2023.10.19 jbh)
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // throw new Exception();

        //        // var values = new Dictionary<string, string>
        //        // {
        //        //     {"id", ""},
        //        //     {"password", ""},
        //        //     {"inputTenantId", "inc-001"}
        //        // };

        //        LoginPack LoginPack = new LoginPack {
        //            id = LoginID,
        //            password = PassWord,
        //            inputTenantId = "inc-001",
        //            requestLoginURL = "https://cobim.io/api/account/user/authorize"
        //        };


        //        Login_Access_Token token = await httpManager.PostLoginConnect(pClient, LoginPack);
        //        Console.WriteLine("Access Token = " + token.resultData);

        //        // [테스트 완료] TODO : 로그 레벨(Debug, Verbose, Warning, Error, Fatal) 기록 테스트 진행 (2023.10.11 jbh)
        //        // Logger.Debug(currentMethod, "테스트 Debug 로그인 토큰");
        //        // Logger.Verbose(currentMethod, "테스트 Verbose 로그인 토큰");
        //        // Logger.Warning(currentMethod, "테스트 Warning 로그인 토큰");
        //        // Logger.Error(currentMethod, "테스트 Error 로그인 토큰");
        //        // Logger.Fatal(currentMethod, "테스트 Fatal 로그인 토큰");

        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
        // 참고 URL - https://husk321.tistory.com/405
        //        //if (token.resultMessage is null)
        //        //{
        //        //    Console.WriteLine("\nLogin is OK...");

        //        //    // MessageBox.Show("Login is OK...");
        //        //}
        //        //else
        //        //{
        //        //    Console.WriteLine("Login is NOT OK...");
        //        //    Console.WriteLine("Error = " + token.resultMessage);
        //        //}
        //        return token;

        //        // else return null;

        //        // string url = "http://bim.v2.service.backend.me.10.1.59.141.nip.io:31380/api/account/user/signin";
        //        // C# HttpClient Header "ContentType" 설정 방법 
        //        // 참고 URL - https://heyoonow.tistory.com/86
        //        //var data = new FormURLEncodedContent(values);
        //        //data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        //var response = await pClient.PostAsync(url, data);
        //        //return response;


        //        //HttpClient pClient = new HttpClient();   

        //        // return response;

        //        //var JsonString = ""; // http 통신 (HttpClient - Connect)

        //        //var options = new JsonSerializerOptions
        //        //{
        //        //    PropertyNameCaseInsensitive = true,                          // case-insensitive 설정
        //        //    ReadCommentHandling = JsonCommentHandling.Skip,              // comments 허용
        //        //    AllowTrailingCommas = true,                                  // trailing commas 허용
        //        //    IncludeFields = true,                                        // fields도 serialize, deserialize 포함
        //        //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // null values 무시 설정 
        //        //};

        //        //var LoginUser = JsonSerializer.Deserialize<LoginPack>(JsonString, options);
        //        //string JsonLoginUser = JsonSerializer.Serialize(LoginUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(currentMethod, Logger.errorMessage + ex.Message);
        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 jbh)
        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }

        //}

        #endregion Sample
    }
}
