using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CobimExplorer.Core.Rest.Api
{
    // TODO : 오류 코드 "CS1705" 오류 메시지 - ID가 'CobimExplorer.Core.Rest.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'인 'CobimExplorer.Core.Rest.Api' 어셈블리는 ID가 'System.Runtime, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'인 참조된 어셈블리 'System.Runtime' 이후 버전인 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'을(를) 사용합니다. (2023.08.29 jbh)
    // 해당 오류 해결하기 위해 -> ASP .Net Core 프로젝트 파일 "CobimExplorer.Core.Rest.Api" 속성 화면 -> 버전 .NET 5.0 -> .NET Core 3.1로 변경 
    public class Program
    {
        // TODO : 인터페이스 "IHttpClientFactory" 사용해서 Http 통신 GET 방식 테스트 코드 로직 구현 (2023.08.28 jbh)
        // 참고 URL - https://youtu.be/1rw9eDFTTlY?si=fvj2tHTzwfLubYUc
        // 참고2 URL - https://youtu.be/1rw9eDFTTlY?si=REwdCYp-ofDY2egH
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            // Bad idea for mid-size or big projects 
            // 큰 규모의 프로젝트 개발시 아래와 같이 using문 안에 "HttpClient" 클래스 객체 "httpClient"를 생성하는 것은 좋지 않다.
            //using (var httpClient = new HttpClient())
            //{

            //}
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
