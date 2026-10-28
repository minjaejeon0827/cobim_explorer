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
    // TODO : ���� �ڵ� "CS1705" ���� �޽��� - ID�� 'CobimExplorer.Core.Rest.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'�� 'CobimExplorer.Core.Rest.Api' ��������� ID�� 'System.Runtime, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'�� ������ ������� 'System.Runtime' ���� ������ 'System.minjaetime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'��(��) ����մϴ�. (2023.08.29 jbh)
    // �ش� ���� �ذ��ϱ� ���� -> ASP .Net Core ������Ʈ ���� "CobimExplorer.Core.Rest.Api" �Ӽ� ȭ�� -> ���� .NET 5.0 -> .NET Core 3.1�� ���� 
    public class Program
    {
        // TODO : �������̽� "IHttpClientFactory" ����ؼ� Http ��� GET ���minjae�Ʈ �ڵ� ���� ���� (2023.08.28 jbh)
        // ���� URL - https://youtu.be/1rw9eDFTTlY?si=fvj2tHTzwfLubYUc
        // ����2 URL - https://youtu.be/1rw9eDFTTlY?si=REwdCYp-ofDY2egH
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            // Bad idea for mid-size or big projects 
            // ū �Ը��� ������Ʈ ���߽� �Ʒ��� ���� using�� �ȿ� "HttpClient" Ŭ���� ��ü "httpClient"�� �����ϴ� ���� ���� �ʴ�.
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
