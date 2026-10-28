using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CobimExplorer.Common.LogManager
{
    // TODO : 로그 메시지 출력 관련 클래스 필요시 구현 및 사용 예정 (2023.07.14 jbh) 
    // 참고 URL - https://github.com/canton7/Stylet/wiki/Logging
    // 참고 2 URL - https://insurang.tistory.com/entry/WPF-C-%EB%A1%9C%EA%B7%B8%ED%8C%8C%EC%9D%BC-%EB%A7%8C%EB%93%A4%EA%B8%B0-LOG
    public class Logger
    {
        // TODO : static 메서드 "GetMethodPath" 테스트 후 오류 발생시 수정 예정(2023.10.11 jbh)
        //public static string GetMethodPath(MethodBase currentMethod)
        //{
        //    var fullMethodPath = string.Empty;

        //    try
        //    {
        //        fullMethodPath = "[" + currentMethod.DeclaringType.FullName.Replace("+", " | ").Replace("d__77", "") + "] : ";
        //        return fullMethodPath;
        //    }
        //    catch (Exception ex)
        //    {
        //        var loggerPath = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        //        Log.Error(loggerPath + Logger.errorMessage + ex.Message);
        //        throw;
        //        //Log.Error($"Error name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
        //    }
        //}

        #region 프로퍼티 

        // TODO : 로그 타입 유형 문자열 (const string) 객체 다른 유형 필요시 추가 예정 (2023.10.11 jbh)
        public const string debugMessage = "디버그 - ";

        public const string infoMessage = "정보 - ";

        public const string errorMessage = "오류 - ";

        public const string warningMessage = "경고 - ";

        // TODO : 로그 파일 갯수 설정 변수 "retainedFileCountLimit"를 프로퍼티(Logger 클래스 객체의 인스턴스 변수)로 구현 안 하고
        //        StyletBootstrapper.cs -> 메서드 "OnStart" 안의 지역변수로 구현해서 static 메서드 "Logger.DeleteOldLogFiles(dirPath, retainedFileCountLimit)"의
        //        메서드 파라미터로 전달한 이유는
        //        static 메서드는 프로퍼티(클래스 객체의 인스턴스 변수) 사용 X + 지역변수(메서드 매개변수)사용 O
        //        하기 때문이다.
        // static 메서드 유튜브 참고 URL - https://youtu.be/Fl4TzjPKAMU?si=KUrhqTCO8jrNzicy
        /// <summary>
        /// 로그 파일 갯수 설정 (테스트용 2일 이전)
        /// </summary>
        // public static int retainedFileCountLimit = 2;

        #endregion 프로퍼티 

        #region ConfigureLogger

        // TODO : C# Serilog 사용해서 로그 남기도록 static 메서드 "ConfigureLogger" 구현 (2024.08.14 jbh)
        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-Serilog-%EC%82%AC%EC%9A%A9%ED%95%98%EC%97%AC-%EB%A1%9C%EA%B7%B8-%EB%82%A8%EA%B8%B0%EA%B8%B0

        /// <summary>
        /// Serilog 로그 초기 설정 
        /// </summary>
        /// <param name="pAssemblyName">로그 기록 남기려는 어셈블리 이름</param>
        public static void ConfigureLogger(int pLogFileCountLimit, string pAssemblyName, string pLogDirectoryPath)
        {
            string logDirectoryPath = string.Empty;    // 로그파일 상위 디렉토리(폴더) 경로 
            string logFilePath = string.Empty;   // 로그파일 생성 경로

            try
            {
                // 로그파일 상위 디렉토리(폴더) 경로 구하기
                StringBuilder sbLogDirectoryPath = new StringBuilder(pLogDirectoryPath);
                logDirectoryPath = sbLogDirectoryPath.ToString();

                // 상위 디렉토리(폴더) 하위 로그파일 경로 구하기 
                StringBuilder sbLogFilePath = new StringBuilder(logDirectoryPath);
                sbLogFilePath.Append($"\\{pAssemblyName}_.log");
                logFilePath = sbLogFilePath.ToString();

                // TODO : Stylet Logger 클래스 사용 안하고 Serilog 이용해서 로그 기록 및 로그 파일 생성 (2024.08.14 jbh)
                // 참고 URL - https://m.blog.naver.com/wolfre/221713399852
                // 참고 2 URL - https://afsdzvcx123.tistory.com/entry/C-%EB%AC%B8%EB%B2%95-C-Serilog-%EC%82%AC%EC%9A%A9%ED%95%98%EC%97%AC-%EB%A1%9C%EA%B7%B8-%EB%82%A8%EA%B8%B0%EA%B8%B0
                // 참고 3 URL - https://blog.naver.com/PostView.naver?blogId=wolfre&logNo=221713399852&parentCategoryNo=&categoryNo=26&viewDate=&isShowPopularPosts=true&from=search
                // 참고 4 URL - https://www.reddit.com/r/dotnet/comments/qhwa4i/serilog_remove_files_older_than_30_days/
                // 참고 5 URL - https://github.com/serilog/serilog-sinks-file#rolling-policies

                // TODO : 에러 로그를 메일로 보내는 로직(.WriteTo.Email) 필요시 추후 구현 예정 (2024.08.14 jbh)
                // 참고 URL - https://github.com/serilog/serilog-sinks-email
                var log = new LoggerConfiguration()
                              .MinimumLevel.Verbose()   // 로그 남기는 기준 ex) Verbose 이상 다 남겨야 함.
                                                        // Verbose(0) => Debug => Information => Warning => Error => Fatal(5)
                              .WriteTo.Console()        // 콘솔 파일에도 로그 출력
                              .WriteTo.Debug()
                              // 로그 파일 이름을 날짜 형식(예)"CobimDockableDialogs_20240814.log" 으로 로그 파일 생성
                              .WriteTo.File(
                                  // $"Logs\\{Assembly.GetEntryAssembly().GetName().Name}\\{Assembly.GetEntryAssembly().GetName().Name}_{DateTime.Now.ToString("yyyyMMdd")}.log",
                                  path: logFilePath,
                                  Serilog.Events.LogEventLevel.Verbose,
                                  // TODO : 로그 파일에 날짜를 수동으로 지정할 필요 없이 rollingInterval: RollingInterval.Day, 사용해서 로그파일 이름 언더바(_)옆에 로그 생성된 날짜가 추가되도록 구현
                                  //        (이렇게 구현해야 아래 프로퍼티 "retainedFileCountLimit" 사용해서 날짜가 지난 로그파일 삭제 처리 할 수 있음.) (2024.08.14 jbh) 
                                  // 참고 URL - https://stackoverflow.com/questions/32108148/serilog-rollingfile
                                  rollingInterval: RollingInterval.Day,
                                  rollOnFileSizeLimit: true,   // 로그 파일 최대 사이즈 1GB  null 옵션 가능
                                                               // TODO : 테스트 용 로그 파일 갯수 2개(2일치 로그) 설정(2일 이상 지난 오래된 로그는 삭제 처리)
                                                               //        2일 지난 로그 파일 정상적으로 삭제 처리시 아래 주석 처리된 소스코드 "retainedFileCountLimit: 62,"로 다시 구현해야 함. (2024.08.14 jbh)
                                                               // 참고 URL - https://stackoverflow.com/questions/44577336/how-do-i-automatically-tail-delete-older-logs-using-serilog-in-a-net-wpf-appl
                                  retainedFileCountLimit: pLogFileCountLimit,
                                  // retainedFileCountLimit: 62,  // 로그 파일 갯수 62개(2달치 로그) 설정(2달 지난 오래된 로그는 삭제 처리) - 기본 31개 설정 가능, null 옵션 가능
                                  encoding: Encoding.UTF8
                              )
                              .CreateLogger();
                Log.Logger = log;             // Serilog를 CobimDockableDialogs에서 사용할 수 있도록 설정 (전역 로그)

                // 기간 지난 로그 파일 삭제 (new LoggerConfiguration() -> .WriteTo.File(retainedFileCountLimit)에 할당된 값(기간) 기준)
                DeleteOldLogFiles(pLogFileCountLimit, logDirectoryPath);

                // 로그 레벨 기록 예시
                // Log.Information($"Info name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
                // Log.Debug($"Debug name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
                // Log.Verbose($"Verbose name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
                // Log.Warning($"Warning name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
                // Log.Error($"Error name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
                // Log.Fatal($"Fatal name = {System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
            }
            catch(Exception ex)
            {
                throw;   // 오류 발생시 상위 호출자 예외처리 전달
            }
        }

        #endregion ConfigureLogger

        #region DeleteOldLogFiles

        // TODO : 기간 지난 로그 파일 삭제 ((new LoggerConfiguration() -> .WriteTo.File(retainedFileCountLimit)에 할당된 값(기간) 기준)) 처리 메서드 "" 구현 (2024.02.02 jbh)
        // 참고 URL - https://chat.openai.com/c/8791804a-55ca-4bf1-b54e-1a34a68f39b5
        /// <summary>
        /// 기간 지난 로그 파일 삭제
        /// </summary>
        /// <param name="pDirPath"></param>
        /// <param name=""></param>
        //public static void DeleteOldLogFiles(string pDirPath, int pRetainedFileCountLimit)
        public static void DeleteOldLogFiles(int pLogFileCountLimit, string pLogDirectoryPath)
        {
            string logDirectoryPath = string.Empty;   // 로그 파일이 저장된 디렉토리 경로 
            int daysToKeep = 0;                       // 로그 파일 삭제할 기간 설정 (예: 30일 이전 파일 삭제 : -30으로 음수로 값 할당 처리 필요)

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록하기 

            try
            {
                logDirectoryPath = pLogDirectoryPath;     // 1. 로그 파일이 저장된 디렉토리 경로 할당

                daysToKeep = (-1) * pLogFileCountLimit;   // 2. 로그 파일 삭제할 기간 설정 (로그파일 삭제하려면 음수로 값 할당 처리 필요)

                DirectoryInfo directory = new DirectoryInfo(logDirectoryPath);

                // 3. 디렉터리(directory) 존재 여부 확인
                if(false == directory.Exists) Directory.CreateDirectory(logDirectoryPath);   // 디렉터리 새로 생성

                var fileList = directory.GetFiles().ToList();   // 4. 해당 디렉터리(di) 안에 로그 파일 존재 여부 확인 

                // 5. 오래된 파일을 찾아서 삭제 
                // TODO : Linq 확장 메서드 "FindAll" "ForEach" "DateTime.Compare" 사용 기간(날짜) 지난 로그 파일 존재여부 확인
                // DateTime.Compare 메서드
                // 참고 URL - https://jinwooking.tistory.com/22

                // DateTime.CompareTo 메서드
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.datetime.compareto?view=net-7.0

                // List 의 FindAll()
                // 참고 URL - https://jwidaegi.blogspot.com/2019/05/unity-list-findall.html

                // C# LINQ List 의 All(), Any()
                // 참고 URL - https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=empty_wagon&logNo=20148303261

                // C# LINQ List 의 ForEach()
                // 참고 URL - https://www.codegrepper.com/code-examples/csharp/linq+foreach+c%23

                // 날짜 지난 로그 파일이 존재하고
                // 로그 파일 생성 날짜가 금일 날짜 보다 2일 이전(DateTime.Now.AddDays(-2))이면 로그 파일 삭제
                // 참고 URL - https://bigenergy.tistory.com/entry/C-%ED%83%80%EC%9D%B4%EB%A8%B8%EB%A5%BC-%EC%9D%B4%EC%9A%A9%ED%95%9C-%EC%98%A4%EB%9E%98%EB%90%9C-%ED%8C%8C%EC%9D%BC-%EC%82%AD%EC%A0%9C-%EB%A1%9C%EA%B7%B8-%EC%82%AD%EC%A0%9C
                // 참고 2 URL - https://medialink.tistory.com/44
                // TODO : 아래 소스코드 실행시 당일 날짜(DateTime.Now) 30일 전일(-30 = daysToKeep) 이전 로그파일 찾아서 오래된 로그 파일 삭제 처리 구현 (2024.08.14 jbh)
                fileList.FindAll(file => file.CreationTime.CompareTo(DateTime.Now.AddDays(daysToKeep)) < 0)
                        .ForEach(file => {
                            if (Path.GetExtension(file.FullName) == ".log") File.Delete(file.FullName);
                        });
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                Console.WriteLine($"오류 발생: {ex.Message}");
            }
        }

        #endregion DeleteOldLogFiles

        #region GetMethodPath

        /// <summary>
        /// 로그 실행한 Namespace 및 메서드(비동기 메서드 포함) 경로 추출
        /// </summary>
        /// <param name="currentMethod"></param>
        /// <returns></returns>
        public static string GetMethodPath(MethodBase pCurrentMethod)
        {
            var fullMethodPath = string.Empty;

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 jbh)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var loggerMethod = MethodBase.GetCurrentMethod();

            try
            {
                // TODO : 로그 실행한 Namespace 및 메서드(비동기 메서드 포함) 경로 추출하여 문자열로 변환 (2023.10.11 jbh)
                // 참고 URL - https://stackoverflow.com/questions/2968352/using-system-reflection-to-get-a-methods-full-name
                // var generatedType = currentMethod.DeclaringType;
                // var originalType = generatedType.DeclaringType;
                // var foundMethod = originalType.GetMethods(
                //       BindingFlags.Instance | BindingFlags.Static
                //     | BindingFlags.Public | BindingFlags.NonPublic
                //     | BindingFlags.DeclaredOnly)
                //     .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                //string test =  foundMethod.DeclaringType.Name + "." + foundMethod.Name;
                // string test = foundMethod.DeclaringType.FullName + "." + foundMethod.Name;

                // fullMethodPath = "[" + currentMethod.ReflectedType.ReflectedType.FullName.Replace("+", " | ").Replace("d__77", "") + "] : ";
                // fullMethodPath = "[" + foundMethod.DeclaringType.FullName + " | " + foundMethod.Name + "] : ";
                // fullMethodPath = "[" + foundMethod.DeclaringType.FullName + " | " + foundMethod.Name + "] : ";
                // TODO : generatedType.Name에서 출력되는 특정 문자열 "d__0"을 Replace 메서드 사용해서 삭제 구현 (2023.10.20 jbh)
                // 참고 URL - https://miss-flower31.tistory.com/entry/C-%ED%8A%B9%EC%A0%95%EB%AC%B8%EC%9E%90%EC%97%B4-%EC%82%AD%EC%A0%9C
                // string methodName = generatedType.Name.Replace("d__0", "");

                // TODO : generatedType.Name에서 문자열 맨 뒤에 출력되는 특정 문자열 "d__0"을 Substring 메서드 사용해서 삭제 구현 (2023.11.06 jbh)
                // 참고 URL - https://gent.tistory.com/502
                //string s = "d__0";
                //int delLen = s.Length;

                //string delStr = generatedType.Name.Substring(generatedType.Name.Length - delLen);
                //string methodName = generatedType.Name.Replace(delStr, "");   // Substring(generatedType.Name.Length - delLen);

                //fullMethodPath = generatedType.Namespace + " | " + methodName;

                fullMethodPath = "[" + pCurrentMethod.DeclaringType.FullName + " | " + pCurrentMethod.Name + "] : ";


                return fullMethodPath;
            }
            catch (Exception ex)
            {
                fullMethodPath = "[" + loggerMethod.DeclaringType.FullName + " | " + loggerMethod.Name + "] : ";
                Log.Error(fullMethodPath + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 jbh)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;

            }
        }

        #endregion GetMethodPath

        #region Sample

        //public class Logger : Stylet.Logging.ILogger
        //{
        //    #region 프로퍼티 

        //    /// <summary>
        //    /// 로그 객체 이름
        //    /// </summary>
        //    private readonly string Name;

        //    /// <summary>
        //    /// 로그 객체 이름 문자열
        //    /// </summary>
        //    private const string LoggerName = "CobimExplorer";

        //    /// <summary>
        //    /// 로그 객체
        //    /// </summary>
        //    public static Logger Log
        //    {
        //        get
        //        {
        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
        // 참고 URL - https://husk321.tistory.com/405
        //            // 싱글톤 패턴 
        //            if (_Log is null)
        //            {
        //                _Log = new Logger(LoggerName);
        //            }
        //            return _Log;
        //        }
        //        set => _Log = value;
        //    }
        //    private static Logger _Log;

        //    /// <summary>
        //    /// 로그 타입 - info
        //    /// </summary>
        //    public const string info = "Info";


        //    /// <summary>
        //    /// 로그 타입 - warn
        //    /// </summary>
        //    public const string warn = "Warn";

        //    /// <summary>
        //    /// 로그 타입 - error
        //    /// </summary>
        //    public const string error = "Error";


        //    #endregion 프로퍼티 

        //    #region 생성자

        //    public Logger(string loggerName)
        //    {
        //        // TODO
        //        this.Name = loggerName;
        //    }

        //    #endregion 생성자

        //    #region 기본 메소드

        //    public void Info(string format, params object[] args)
        //    {
        //        // TODO
        //        LogWrite(info, format, args.ToString());
        //    }

        //    public void Warn(string format, params object[] args)
        //    {
        //        // TODO
        //        LogWrite(warn, format);
        //    }

        //    public void Error(Exception exception, string message = null)
        //    {
        //        // TODO
        //        LogWrite(error, message);
        //    }

        //    #endregion 기본 메소드 

        //    #region LogWrite

        //    /// <summary>
        //    /// 로그 파일 생성 + 기록
        //    /// </summary>
        //    /// <param name="message"></param>
        //    private void LogWrite(string logType, string message, string args = null)
        //    {
        //        // string DirPath = Environment.CurrentDirectory + @"\Log";
        //        // TODO : 로그 파일 상위 디렉토리(폴더) 생성 위치("C:\\CobimExplorer\\Log") 문자열 string 객체 DirPath에 할당 (2023.10.6 jbh)
        //        // 참고 URL - https://lightgg.tistory.com/29
        //        string DirPath = "C:\\CobimExplorer\\Log";
        //        string FilePath = DirPath + "\\CobimExplorer_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
        //        string temp = string.Empty;

        //        DirectoryInfo di = new DirectoryInfo(DirPath);
        //        FileInfo fi = new FileInfo(FilePath);

        //        try
        //        {
        //            // 디렉터리(di)가 존재하지 않는 경우
        //            if(false == di.Exists) Directory.CreateDirectory(DirPath);   // 디렉터리 새로 생성

        //            // 로그 파일(fi)이 존재하지 않는 경우 
        //            if(false == fi.Exists)
        //            {
        //                // TODO : StreamWriter 클래스 객체 sw 인코딩 방식 Encoding.UTF8 설정 (한글 인코딩 때문) (2023.10.6 jbh)
        //                // 참고 URL - https://cloudedi.tistory.com/29
        //                // 참고 2 URL - https://aspdotnet.tistory.com/1043
        //                // using문 사용해서 로그파일 새로 생성
        //                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
        //                {
        //                    temp = string.Format("[{0}][{1}] {2}", logType, DateTime.Now, message);
        //                    sw.WriteLine(temp);
        //                    sw.Close();
        //                }
        //            }
        //            // 로그 파일(fi)이 존재하는 경우 
        //            else
        //            {
        //                // using문 사용해서 기존 로그파일에 로그 기록(문자열) 추가
        //                using (StreamWriter sw = File.AppendText(FilePath))
        //                {
        //                    temp = string.Format("[{0}] {1}", DateTime.Now, message);
        //                    sw.WriteLine(temp);
        //                    sw.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }

        //    #endregion LogWrite

        #endregion Sample
    }
}
