using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using CobimUtil.Helper;

namespace CobimUtil
{
    public class Logger
    {
        /// <summary>
        /// 로그 객체
        /// </summary>
        //public static Logger log
        //{
        //    get
        //    {
        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        // 참고 URL - https://husk321.tistory.com/405
        //        // 싱글톤 패턴 
        //        if (_log is null)
        //        {
        //            _log = new LoggerConfiguration()
        //            .MinimumLevel.Verbose()   // 로그 남기는 기준 ex) Verbose 이상 다 남겨야 함.
        //                                      // Verbose(0) => Debug => Information => Warning => Error => Fatal(5)
        //            .WriteTo.Console()        // 콘솔 파일에도 로그 출력
        //            .WriteTo.Debug()
        //            // 날짜 형식으로 로그 남김
        //            .WriteTo.File(
        //            $"Logs\\CobimUtil\\{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}_{DateTime.Now.ToString("yyyyMMdd")}.log",
        //            Serilog.Events.LogEventLevel.Verbose,
        //            // rollingInterval: RollingInterval.Day, // 로그 파일 생성시 로그파일 이름에 날짜가 중복되서 추가되므로 해당 "rollingInterval: RollingInterval.Day" 내용 주석 처리
        //            rollOnFileSizeLimit: true,   // 로그 파일 최대 사이즈 1GB  null 옵션 가능
        //            retainedFileCountLimit: 62,  // 로그 파일 갯수 62개(2달치 로그) 설정(2달 지난 오래된 로그는 삭제 처리) - 기본 31개 설정 가능, null 옵션 가능
        //            encoding: Encoding.UTF8
        //            )
        //            .CreateLogger();
        //        }
        //        return _log;
        //    }
        //    set => _log = value;
        //}
        //private static Logger _log;

        #region 프로퍼티 

        /// <summary>
        /// 로그 객체 이름
        /// </summary>
        // private readonly string Name;

        /// <summary>
        /// 로그 객체 이름 문자열
        /// </summary>
        // private const string LoggerName = "CobimExplorer";

        /// <summary>
        /// 로그 객체
        /// </summary>
        //public static Logger Log
        //{
        //get
        //{
        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        // 참고 URL - https://husk321.tistory.com/405
        //// 싱글톤 패턴 
        //if (_Log is null)
        //{
        //_Log = new Logger(LoggerName);
        //}
        //return _Log;
        //}
        //set => _Log = value;
        //}
        //private static Logger _Log;

        // TODO : 로그 타입 유형 문자열 (const string) 객체 다른 유형 필요시 추가 예정 (2023.10.11 minjae)
        public const string debugMessage = "디버그 - ";

        public const string errorMessage = "오류 - ";

        public const string warningMessage = "경고 - ";


        // TODO : 로그 파일 갯수 설정 변수 "retainedFileCountLimit"를 프로퍼티(Logger 클래스 객체의 인스턴스 변수)로 구현 안 하고
        //        StyletBootstrapper.cs -> 메서드 "OnStart" 안의 지역변수로 구현해서 static 메서드 "Logger.DeleteOldLogFiles(dirPath, retainedFileCountLimit)"의
        //        메서드 파라미터로 전달한 이유는
        //        static 메서드는 프로퍼티(클래스 객체의 인스턴스 변수) 사용 X + 지역변수(메서드 매개변수)사용 O
        //        하기 때문이다.
        // static 메서드 유튜브 참고 URL - https://youtu.be/Fl4TzjPKAMU?si=KUrhqTCO8jrNzicy
        /// <summary>
        /// 로그 파일 최대 누적 일자 
        /// </summary>
        //public const int retainedFileCountLimit = 62;
        // private const int retainedFileCountLimit = -7;

        /// <summary>
        /// 테스트용 - 로그 파일 최대 누적 일자 
        /// </summary>
        //public const int retainedFileCountLimit = 62;
        // private const int testFileCountLimit = -1;

        #endregion 프로퍼티 


        #region 생성자

        public Logger(string loggerName)
        {
            // TODO
            // this.Name = loggerName;
        }

        #endregion 생성자

        #region 기본 메소드 

        public static void Verbose(MethodBase currentMethod, string format, params object[] args)
        {
            // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae)
            // LogWrite(LogEventLevel.Verbose.ToString(), currentMethod, format, args.ToString());
        }

        public static void Debug(MethodBase currentMethod, string format, params object[] args)
        {
            // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae) 
            // LogWrite(LogEventLevel.Debug.ToString(), currentMethod, format, args.ToString());
        }

        //public static void Information(string MethodPath, string format, params object[] args)
        //{
        //    // TODO : 
        //    LogWrite(LogEventLevel.Information.ToString(), format, args.ToString());
        //}

        public static void Information(MethodBase currentMethod, string format, params object[] args)
        {
            // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae)
            // LogWrite(LogEventLevel.Information.ToString(), currentMethod, format, args.ToString());
        }

        public static void Warning(MethodBase currentMethod, string format, params object[] args)
        {
            // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae)
            // LogWrite(LogEventLevel.Warning.ToString(), currentMethod, format, args.ToString());
        }

        // TODO : static 메서드 "Error" (메서드 파라미터 Exception exception 포함) 필요시 사용 예정 (2023.10.11 minjae)
        //public static void Error(Exception exception, MethodBase currentMethod, string message = null)
        //{
        //    // TODO : 
        //    LogWrite(LogEventLevel.Error.ToString(), currentMethod, message);
        //}

        public static void Error(MethodBase currentMethod, string message = null)
        {
            // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae)
            // LogWrite(LogEventLevel.Error.ToString(), currentMethod, message);
        }

        // TODO : static 메서드 "Fatal" (메서드 파라미터 Exception exception 포함) 필요시 사용 예정 (2023.10.11 minjae)
        //public static void Fatal(Exception exception, MethodBase currentMethod, string message = null)
        //{
        //    // TODO : 
        //    LogWrite(LogEventLevel.Fatal.ToString(), currentMethod, message);
        //}

        public static void Fatal(MethodBase currentMethod, string message = null)
        {
            // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae)
            // LogWrite(LogEventLevel.Fatal.ToString(), currentMethod, message);
        }

        #endregion 기본 메소드

        #region LogWrite

        // TODO : 로그 파일 생성 + 기록 메서드 "LogWrite" 필요시 참고 (2024.07.18 minjae)
        /// <summary>
        /// 로그 파일 생성 + 기록
        /// </summary>
        /// <param name="message"></param>
        private static void LogWrite(string logType, MethodBase currentMethod, string message, string args = null)
        {
            // TODO : 현재 실행 중인 프로그램의 파일 경로 로그 파일 생성하기(필요시 수정 예정) (2023.10.10 minjae)
            // 참고 URL - https://ghostweb.tistory.com/1149
            //string path = Assembly.GetExecutingAssembly().Location;
            //string dirPath = Path.GetDirectoryName(path);
            //string filePath = $"{dirPath}\\Logs\\{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}_{DateTime.Now.ToString("yyyyMMdd")}.log";
            // string filePath = $"{dirPath}\\Logs\\{"TestLog"}_{DateTime.Now.ToString("yyyyMMdd")}.log";
            // string DirPath = Environment.CurrentDirectory + @"\Log";
            // TODO : 로그 파일 상위 디렉토리(폴더) 생성 위치("C:\\CobimExplorer\\Log") 문자열 string 객체 DirPath에 할당 (2023.10.6 minjae)
            // 참고 URL - https://lightgg.tistory.com/29
            // string DirPath = "C:\\CobimExplorer\\Log";

            // TODO : 생성할 로그 파일의 상위 폴더를 생성하기 위해 상위 폴더명을 로그 기록이 실행되는 프로젝트 파일명을 "projectFileName"에 문자열로 할당 (2023.10.11 minjae) 
            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.reflection.assembly?view=net-7.0
            string projectFileName = currentMethod.ReflectedType.Assembly.GetName().Name;

            // TODO : C# 문자열 보간 기능 사용해서 로그파일 생성할 상위 폴더 경로 문자열로 변환 (2023.10.11 minjae)
            // 참고 URL - https://gaeunhan.tistory.com/61

            // TODO : 오류 메시지 "예외 발생: 'System.IO.DirectoryNotFoundException'(mscorlib.dll)" 해결 하기 위해 아래 코드 주석처리 및
            //        디렉토리 경로 변수(dirPath)에 할당할 "내문서(Environment.SpecialFolder.MyDocuments)" 변경 구현 (2024.07.18 minjae) 
            // string dirPath = $"D:\\bhjeon\\COBIM-Explorer\\CobimExplorer\\CobimExplorer\\bin\\x64\\Debug\\Logs\\{projectFileName}";

            string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\CobimExplorer\\Logs";
            // string filePath = dirPath + $"\\{projectFileName}_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string filePath = dirPath + $"\\CobimExplorer_" + DateTime.Today.ToString("yyyyMMdd") + ".log";

            int logFileCountLimit = UtilHelper.LogFileCountLimit;   // 로그 파일 최대 누적 일자 (30일)

            string temp = string.Empty;

            DirectoryInfo di = new DirectoryInfo(dirPath);
            FileInfo fi = new FileInfo(filePath);

            try
            {
                // 기간 지난 로그 파일 삭제 - logFileCountLimit)에 할당된 값(기간) 기준
                DeleteOldLogFiles(logFileCountLimit, dirPath);

                // 로그 파일(fi)이 존재하지 않는 경우 
                if (false == fi.Exists)
                {
                    // TODO : StreamWriter 클래스 객체 sw 인코딩 방식 Encoding.UTF8 설정 (한글 인코딩 때문) (2023.10.6 minjae)
                    // 참고 URL - https://cloudedi.tistory.com/29
                    // 참고 2 URL - https://aspdotnet.tistory.com/1043
                    // using문 사용해서 로그파일 새로 생성
                    using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
                    {
                        temp = string.Format("{0} [{1}] [{2}] : {3}", DateTime.Now, logType, GetMethodPath(currentMethod), message);
                        sw.WriteLine(temp);
                        // TODO : 필요시 textWriter.Close(); 메서드 호출 예정 (2024.02.27 minjae)
                        // sw.Close();
                    }
                }
                // 로그 파일(fi)이 존재하는 경우 
                else
                {
                    // using문 사용해서 기존 로그파일에 로그 기록(문자열) 추가
                    using (StreamWriter sw = File.AppendText(filePath))
                    // using(StreamWriter sw = new StreamWriter(filePath))
                    {
                        temp = string.Format("{0} [{1}] [{2}] : {3}", DateTime.Now, logType, GetMethodPath(currentMethod), message);
                        sw.WriteLine(temp);
                        // TODO : 필요시 textWriter.Close(); 메서드 호출 예정 (2024.02.27 minjae)
                        // sw.Close();
                    }   // 여기서 Dispose (리소스 해제) 처리 
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion LogWrite

        #region DeleteOldLogFiles

        // TODO : 기간 지난 로그 파일 삭제 ((new LoggerConfiguration() -> .WriteTo.File(retainedFileCountLimit)에 할당된 값(기간) 기준)) 처리 메서드 "" 구현 (2024.02.02 minjae)
        // 참고 URL - https://chat.openai.com/c/8791804a-55ca-4bf1-b54e-1a34a68f39b5
        /// <summary>
        /// 기간 지난 로그 파일 삭제
        /// </summary>
        /// <param name="pDirPath"></param>
        /// <param name=""></param>
        //public static void DeleteOldLogFiles(string pDirPath, int pRetainedFileCountLimit)
        public static void DeleteOldLogFiles(int pLogFileCountLimit, string pLogDirPath)
        {
            string logDirPath = string.Empty;   // 로그 파일이 저장된 디렉토리 경로 

            int daysToKeep = 0;                 // 로그 파일 삭제할 기간 설정 (예: 30일 이전 파일 삭제 : -30으로 음수로 값 할당 처리 필요)

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2024.01.22 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                logDirPath = pLogDirPath;                                      // 1. 로그 파일이 저장된 디렉토리 경로 할당

                daysToKeep = (-1) * pLogFileCountLimit;                        // 2. 로그 파일 삭제할 기간 설정 (로그파일 삭제하려면 음수로 값 할당 처리 필요)

                DirectoryInfo directory = new DirectoryInfo(logDirPath);

                // 3. 디렉터리(directory)가 존재 여부 확인
                if (false == directory.Exists) Directory.CreateDirectory(logDirPath);   // 디렉터리 새로 생성


                var fileList = directory.GetFiles().ToList();               // 4. 해당 디렉터리(di) 안에 로그 파일 존재 여부 확인 

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
                // 로그 파일 생성 날짜가 금일 날짜 보다 30일 이전(DateTime.Now.AddDays(-30))이면 로그 파일 삭제
                // 참고 URL - https://bigenergy.tistory.com/entry/C-%ED%83%80%EC%9D%B4%EB%A8%B8%EB%A5%BC-%EC%9D%B4%EC%9A%A9%ED%95%9C-%EC%98%A4%EB%9E%98%EB%90%9C-%ED%8C%8C%EC%9D%BC-%EC%82%AD%EC%A0%9C-%EB%A1%9C%EA%B7%B8-%EC%82%AD%EC%A0%9C
                // 참고 2 URL - https://medialink.tistory.com/44
                //fileList.FindAll(file => DateTime.Compare(file.CreationTime, DateTime.Now.AddDays(testFileCountLimit)) < 0)
                //        .ForEach(file => { File.Delete(file.FullName); });

                // var list = fileList.FindAll(file => DateTime.Compare(file.CreationTime, DateTime.Now.AddDays(testFileCountLimit)) < 0).ToList();

                // list.ForEach(file => { if (Path.GetExtension(file.FullName) == ".log") File.Delete(file.FullName); });

                // TODO : 아래 소스코드 실행시 당일 날짜(DateTime.Now) 30일 전일(-30 daysToKeep) 이전 로그파일 찾아서 오래된 로그 파일 삭제 처리 구현 (2023.10.16 minjae) 
                fileList.FindAll(file => file.CreationTime.CompareTo(DateTime.Now.AddDays(daysToKeep)) < 0)
                        .ForEach(file => {
                            if (Path.GetExtension(file.FullName) == ".log") File.Delete(file.FullName);
                        });

                //fileList.Where(file => file.CreationTime.CompareTo(DateTime.Now.AddDays(testFileCountLimit)) < 0)
                //        .ToList()
                //        .ForEach(file => {
                //            if (Path.GetExtension(file.FullName) == ".log") File.Delete(file.FullName);
                //        });

                //fileList.FindAll(file => DateTime.Compare(file.CreationTime, DateTime.Now.AddDays(testFileCountLimit)) < 0)
                //        .ForEach(file => {
                //                            if (Path.GetExtension(file.FullName) == ".log") File.Delete(file.FullName);
                //                         });

                //foreach (FileInfo f in fileList)
                //{
                //    if (Path.GetExtension(f.FullName) == ".log")
                //    {
                //        DateTime file_time = f.CreationTime;
                //        if (DateTime.Compare(file_time, DateTime.Now.AddDays(testFileCountLimit)) < 0)
                //        {
                //            File.Delete(f.FullName);
                //        }
                //    }
                //}

                // TODO : 위의 소스코드 "fileList.FindAll(file => DateTime.Compare(file.CreationTime, DateTime.Now.AddDays(testFileCountLimit)) < 0).ForEach(file => { File.Delete(file.FullName); });"
                //        테스트 진행 후 테스트 결과 오류 발생시 바로 아래 주석 처리된 소스코드로 날짜 지난 로그 파일이 삭제되는지 확인 (2023.10.11 minjae)
                // var oldLogFileList = fileList.FindAll(file => DateTime.Compare(file.CreationTime, DateTime.Now.AddDays(testFileCountLimit)) < 0).ToList();

                // 날짜 지난 로그 파일 존재하는 경우 
                //if (oldLogFileList.Count > 0)
                //{
                //    // 날짜 지난 로그 파일이 존재하고
                //    // 로그 파일 생성 날짜가 금일 날짜 보다 7일 이전(DateTime.Now.AddDays(-7))이면 로그 파일 삭제
                //    // 참고 URL - https://bigenergy.tistory.com/entry/C-%ED%83%80%EC%9D%B4%EB%A8%B8%EB%A5%BC-%EC%9D%B4%EC%9A%A9%ED%95%9C-%EC%98%A4%EB%9E%98%EB%90%9C-%ED%8C%8C%EC%9D%BC-%EC%82%AD%EC%A0%9C-%EB%A1%9C%EA%B7%B8-%EC%82%AD%EC%A0%9C
                //    // 참고 2 URL - https://medialink.tistory.com/44

                //    oldLogFileList.ForEach(file => { File.Delete(file.FullName); });
                //}
            }
            catch (Exception ex)
            {
                Error(currentMethod, ex.Message);
                Console.WriteLine($"오류 발생: {ex.Message}");
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2024.01.24 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion DeleteOldLogFiles

        #region GetMethodPath

        /// <summary>
        /// 로그 실행한 Namespace 및 메서드(비동기 메서드 포함) 경로 추출
        /// </summary>
        public static string GetMethodPath(MethodBase pCurrentMethod)
        {
            var fullMethodPath = string.Empty;

            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            // 참고 URL - https://slaner.tistory.com/73
            // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
            // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            var currentUtilMethod = MethodBase.GetCurrentMethod();

            try
            {
                // TODO : 로그 실행한 Namespace 및 메서드(비동기 메서드 포함) 경로 추출하여 문자열로 변환 (2023.10.11 minjae)
                // 참고 URL - https://stackoverflow.com/questions/2968352/using-system-reflection-to-get-a-methods-full-name
                // var generatedType = currentMethod.DeclaringType;

                // TODO : "generatedType", "originalType" Null Exception 발생한 원인 파악하기 (2023.10.20 minjae)
                // 참고 URL - https://stackoverflow.com/questions/22809141/why-does-resharper-show-a-system-nullreferenceexception-warning-in-this-case
                //var originalType = generatedType.DeclaringType;
                //var foundMethod = originalType.GetMethods(
                //      BindingFlags.Instance | BindingFlags.Static
                //    | BindingFlags.Public | BindingFlags.NonPublic
                //    | BindingFlags.DeclaredOnly)
                //    .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                //string test =  foundMethod.DeclaringType.Name + "." + foundMethod.Name;
                // string test = foundMethod.DeclaringType.FullName + "." + foundMethod.Name;

                // fullMethodPath = "[" + currentMethod.ReflectedType.ReflectedType.FullName.Replace("+", " | ").Replace("d__77", "") + "] : ";
                // fullMethodPath = currentMethod.DeclaringType.FullName + " | " + currentMethod.Name;

                // TODO : generatedType.Name에서 출력되는 특정 문자열 "d__0"을 Replace 메서드 사용해서 삭제 구현 (2023.10.20 minjae)
                // 참고 URL - https://miss-flower31.tistory.com/entry/C-%ED%8A%B9%EC%A0%95%EB%AC%B8%EC%9E%90%EC%97%B4-%EC%82%AD%EC%A0%9C
                // string methodName = generatedType.Name.Replace("d__0", "");

                // TODO : generatedType.Name에서 문자열 맨 뒤에 출력되는 특정 문자열 "d__0"을 Substring 메서드 사용해서 삭제 구현 (2023.10.20 minjae)
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
                // var methodPath = currentUtilMethod.DeclaringType.FullName;
                // Error(methodPath, e, ex.Message);
                Error(currentUtilMethod, ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
                
            }
        }

        #endregion GetMethodPath

        #region Sample

        #endregion Sample
    }
}
