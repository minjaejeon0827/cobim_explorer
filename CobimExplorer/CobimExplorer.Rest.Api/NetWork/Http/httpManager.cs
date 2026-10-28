using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using CobimExplorer.Rest.Api.CobimBase.Login;
using CobimExplorer.Rest.Api.CobimBase.Explorer;
using CobimUtil;
using CobimExplorer.Data.Models.CobimBase.Login;
// using CobimExplorer.Rest.Api.CobimBase.User.LoginHelper;
//using static CobimExplorer.Rest.Api.CobimBase.User.LoginHelper;
//using static CobimExplorer.Rest.Api.CobimBase.User.LoginHelper;

namespace CobimExplorer.Rest.Api.NetWork.Http
{
    // TODO : httpManager 클래스(http 통신 역할) 필요 시 사용 예정(2023.08.22 jbh) 
    // 주의사항 - Http 통신 로직 구현시 "System.Text.json"의 경우 .Net Core에서는 잘 호환(사용)되지만 .Net Framework에서는 호환되지 않고 에러가 발생하는 경우가 존재함.
    public class httpManager
    {
        /// <summary>
        /// Http POST 통신 메서드 
        /// Rest API 메서드 파라미터 "HttpClient pClient, LoginPack pLoginPack"
        /// </summary>
        public static async Task<LoginResponseData> PostLoginConnect(HttpClient pClient, LoginPack pLoginPack)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                var grant = new List<KeyValuePair<string, string>>();
                grant.Add(new KeyValuePair<string, string>("id", pLoginPack.id));
                grant.Add(new KeyValuePair<string, string>("password", pLoginPack.password));
                grant.Add(new KeyValuePair<string, string>("inputTenantId", pLoginPack.inputTenantId));

                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, LoginHelper.requestLoginURL);
                req.Content = new FormUrlEncodedContent(grant);
                // req.Content.("id", LoginPack.id);
                // req.Content.TryAddWithoutValidation("password", LoginPack.password);
                // req.Content.TryAddWithoutValidation("inputTenantId", LoginPack.inputTenantId);
                req.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                // req.Headers.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                // req.Headers.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // C# HttpClient Header "ContentType" 설정 방법 
                // 참고 URL - https://heyoonow.tistory.com/86
                // var data = new FormUrlEncodedContent(values);
                // data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                HttpResponseMessage response = await Connect(pClient, req);
                string json = response.Content.ReadAsStringAsync().Result;
                LoginResponseData responseData = JsonSerializer.Deserialize<LoginResponseData>(json);

                return responseData;
            }
            catch(Exception ex)
            {
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 jbh)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        /// <summary>
        /// Http Connect
        /// </summary>
        public static async Task<HttpResponseMessage> Connect(HttpClient pClient, HttpRequestMessage pRequest)
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using var jsonContent = new StringContent(
                    "\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc001\""
                    , Encoding.UTF8, "application/json");

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using var res = await pClient.PostAsync("https://cobim.io/api/account/user/authorize", jsonContent);

                // TODO : HttpResponseMessage response이 null로 리턴받아 Null Expcetion 발생하는 원인 파악하기 (2023.08.21 jbh)
                //HttpRequestMessage req = request;
                // TODO : HttpClient 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 jbh)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await pClient.SendAsync(pRequest);//.Result;
                string msg = "HTTP Client Connect:\n";
                msg += "Request:\n" + pRequest.ToString() + "\n";

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if(pRequest.Content is not null)
                {
                    msg += pRequest.Content.ReadAsStringAsync().Result + "\n\n";
                }

                msg += "response:\n" + response.ToString() + "\n";

                if(response.Content is not null)
                {
                    msg += response.Content.ReadAsStringAsync().Result + "\n\n";
                }

                // Replace password and client secret
                // Serilog.Log.Debug(msg);
                // Logger.Debug(currentMethod, Logger.debugMessage + msg);
                Log.Logger.Debug(Logger.GetMethodPath(currentMethod) + Logger.debugMessage + msg);
                return response;
            }
            catch(Exception ex)
            {
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 jbh)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
            
        }
    }
}
