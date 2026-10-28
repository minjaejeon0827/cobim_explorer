using Serilog;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CobimExplorer.Core.Rest.Api.CobimBase.User.LoginHelper;

namespace CobimExplorer.Core.Rest.Api.CobimBase.User
{
    // TODO : ASP.NET Core 로드맵 보면서 공부 및 구현하기 (2023.08.29 jbh)
    // 참고 URL - https://github.com/Elfocrash/.NET-Backend-Developer-Roadmap
    public class UserRestServer
    {
        //private readonly HttpClient client;
        private static IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// LoginHelper 클래스 - 싱글톤 패턴 
        /// </summary>
        /// <param name="httpClientFactory"></param>

        public UserRestServer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //public UserRestServer(HttpClient client)
        //{
        //    this.client = client;
        //}

        ///// <summary>
        ///// 사용자 로그인 (POST)
        ///// Rest API 메서드 파라미터 "HttpClient client, string PassWord, string inputTenantId"
        ///// </summary>
        public static async Task<LogIn_Access_Token> PostUserLogInAsync(string LoginID, string Password)
        {
            try
            {
                // var client = IHttpClientFactory.creteClinet();
                // Bad idea for mid-size or big projects 
                // 큰 규모의 프로젝트 개발시 아래와 같이 using문 안에 "HttpClient" 클래스 객체 "httpClient"를 생성하는 것은 좋지 않다.
                //using (var httpClient = new HttpClient())
                //{

                //}

                // Example 1: Basic use (기본적인 인터페이스 "IHttpClientFactory" 사용 방법)
                //var httpClient = httpClientFactory.CreateClient();
                //var responseMessage = await httpClient.GetAsync(peopleURL);
                //responseMessage.EnsureSuccessStatusCode();

                // Example 2: Name-Clients (이름 사용 - 인터페이스 "IHttpClientFactory" 사용 방법)
                //var httpClientPeople = httpClientFactory.CreateClient("people");
                //var responseMessage2 = await httpClientPeople.GetAsync("");
                //responseMessage2.EnsureSuccessStatusCode();

                //var httpClientWeatherForecast = httpClientFactory.CreateClient("weatherForecast");
                //var responseMessage3 = await httpClientWeatherForecast.GetAsync(""); ;
                //responseMessage3.EnsureSuccessStatusCode();

                // Example 3: Typed-Clients (클래스 타입(이름) 사용 - 인터페이스 "IHttpClientFactory" 사용 방법)
                //var peopleClient = services.GetRequiredService<IUserRestServer>();
                //var people = await peopleClient.PostUserLogInAsync("dvl001", "qwer1234!");



                var client = _httpClientFactory.CreateClient(LoginHelper.httpClient);

                LogInPack LoginPack = new LogInPack
                {
                    id = LoginID,
                    password = Password,
                    inputTenantId = LoginHelper.inputTenantId,
                };

                // TODO : LogInPack 클래스 객체 "LogInPack" string 클래스 json 문자열(content) 구현 (2023.08.21 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
                // json 문자열(하드코딩) 생성 코드 예시 -> (예) string content = "{\"id\":\"dvl001\",\"password\":\"qwer1234!\",\"inputTenantId\":\"inc-001\"}";
                string content = JsonSerializer.Serialize(LoginPack);

                var jsonContent = new StringContent(
                                        content,
                                        Encoding.UTF8,
                                        LoginHelper.contentType
                                      );

                HttpResponseMessage response = await client.PostAsync(LoginHelper.auth_server_url, jsonContent);

                string json = await response.Content.ReadAsStringAsync();

                LogIn_Access_Token token = JsonSerializer.Deserialize<LogIn_Access_Token>(json);

                return token;
            }
            catch (Exception e)
            {
                Log.Logger.Information(e.Message);
                throw;
            }
        }
    }
}
