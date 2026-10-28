using Serilog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Reflection;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using CobimExplorer.Data.Models.CobimBase.Login;
using CobimExplorer.Data.Models.CobimBase.Explorer;
using CobimUtil;

namespace CobimExplorer.Rest.Api.CobimBase.Explorer
{
    /// <summary>
    /// 성과품(CDE) 조회 Rest API 클래스 
    /// </summary>
    // TODO : 클래스 "BindableBase" 필요시 "ExplorerRestServer"의 부모 클래스로 상속 예정 (2023.12.28 minjae)
    public class ExplorerRestServer // : BindableBase
    {
        #region GetProjectTeamListAsync

        // TODO : 프로젝트 + 팀목록 조회 Get 방식 구현 (2023.08.29 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 2. 나의 프로젝트(+ 팀목록) 목록 조회 (Get)
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<List<Dictionary<string, object>>> GetProjectTeamListAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                // HttpClient client 객체 가져오기 
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기

            var currentMethod = MethodBase.GetCurrentMethod();                  // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.08.30 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(ProjectHelper.requestProjectTeamURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 2. 나의 프로젝트 목록 조회 (Get) Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    /// <summary>
                    /// TODO : JSON 데이터(resMsg) -> Dictionary<string, object> 변환(Convert) 구현 및 var jtmp에 데이터 할당 (2023.08.30 minjae)
                    /// (할당 받은 데이터는 조사식 -> 키워드 "jtmp" 입력 및 엔터 -> ResultView 에서 확인 가능 - var 자료형 이여서 IEnumerable<JsonNode> 형태이기 때문) (2023.08.30 minjae)
                    /// 참고 URL - https://stackoverflow.com/questions/1207731/how-can-i-deserialize-json-to-a-simple-dictionarystring-string-in-asp-net
                    /// using System.Text.Json
                    /// 참고 URL - https://www.csharpstudy.com/Data/Json-SystemTextJson.aspx
                    /// 참고 2 URL - https://dhddl.tistory.com/250
                    /// JsonNode 클래스 - 변경 가능한 JSON 문서 내의 단일 노드를 나타내는 기본 클래스를 의미
                    /// 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode?view=net-7.0
                    /// JSON 이란? - 데이터를 표현하는 문자열 
                    /// 파싱이란? - 다른 형식으로 저장된 데이터를 원하는 형식의 데이터로 변환하는 것을 말한다.
                    /// JSON 파싱 - JSON 형식의 문자열을 객체로 변환하는 것
                    /// 참고 URL - https://velog.io/@lzhxxn/JSON-Parsing%ED%8C%8C%EC%8B%B1%EC%9D%B4%EB%9E%80
                    /// </summary>


                    // TODO : JSON 데이터(resMsg) -> Dictionary<string, object> 변환(Convert) 구현 및 var jtmp에 데이터 할당 (2023.08.30 minjae)
                    //        할당 받은 데이터 var jtmp는 조사식 -> 키워드 "jtmp" 입력 및 엔터(또는 해당 var jdic 클릭 및 조사식으로 드래그) -> Results View 에서 확인 가능 - var 자료형 이여서 IEnumerable<JsonNode> 형태이기 때문 (2023.08.30 minjae)
                    // TODO : JSON 데이터(resMsg) 를 JsonObject.Parse(resMsg) 사용해서 "resultData" 항목만 파싱(Json Object(객체) 변환) -> 파싱한 Json Object(객체)안에 존재하는 데이터들을 JsonArray (JSON 배열) 변환
                    //        -> 해당 JsonArray (JSON 배열)에서 항목(Object) "projectEntity" 추출 -> 추출한 JsonArray 데이터를 var jtmp로 할당 하도록 구현 (2023.08.30 minjae)
                    // var jtmp = (JsonObject.Parse(resMsg)[ProjectHelper.resultData] as JsonArray).Select(x => x[ProjectHelper.projectEntity]);

                    // TODO : 테스트 JSON 데이터(resMsg) 구조 변경으로 인하여 "projectWithTeamInfos" 영역 안 데이터 "projectEntity", "teamEntity" 추출 구현 (2023.09.12 minjae)
                    var jtmp = (JsonObject.Parse(resMsg)[ProjectHelper.projectWithTeamInfos] as JsonArray).Select(x => x);

                    var projectTeamList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "2. 나의 프로젝트 목록 조회 성공");

                    return projectTeamList;
                }
                // Rest API 2. 나의 프로젝트 목록 조회 (Get) Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
            }
            catch(Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.08.30 minjae)
                // Logger.Error(currentMethod, Logger.errorMessage + ex.Message);
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion GetProjectTeamListAsync

        #region GetExplorerTeamUserListAsync

        // TODO : 3. 프로젝트 상세조회 Get 방식 필요시 구현 예정 (2023.09.12 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 3. 프로젝트 상세조회 (Get)
        /// Rest API 메서드 파라미터 "HttpClient pClient, string projectId, string teamId, string authorization"
        /// </summary>
        // TODO : 메서드 "GetExplorerTeamUserListAsync" 리턴 자료형 Task<Dictionary<string, object>>으로 구현 예정 (2023.09.12 minjae)
        //public static async Task<List<Dictionary<string, object>>> GetExplorerTeamUserListAsync(HttpClient pClient, ProjectHelper.ProjectPack pProjectPack, RequestHeader pRequestHeaders)
        //{
        // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        // 참고 URL - https://slaner.tistory.com/73

        // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        // var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        var teamUserList = ;
        //        return teamUserList;
        //    }
        //    catch (Exception e)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.12 minjae)
        //        Logger.Error(currentMethod, Logger.errorMessage + ex.Message);
        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.6 minjae)
        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }

        //}

        #endregion GetExplorerTeamUserListAsync

        #region GetFolderListAsync

        // TODO : 4. 폴더 목록 조회 Get 방식 구현 (2023.09.01 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 4. 폴더 목록 조회 (Get)
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<List<Dictionary<string, object>>> GetFolderListAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                // HttpClient client 객체 가져오기 
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기
            ContainerPack containerPack = pInputParamData.GetContainerPack();   // 컨테이너 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.04 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
                // Http Get 방식으로 호출해서 각각의 프로젝트 및 팀 아이디 하위의 폴더 + 파일 목록 JSON 데이터 가져오기 (2023.09.06 minjae)
                // 테스트용 URL 주소 - requestFolderURL = "https://cobim.io/api/explorer/folder/list/jc-constrct/inc-001-jc-constrct-t001";

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 4. 폴더 목록 조회 URL 주소에 문자열 추가 (2023.11.22 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

                StringBuilder requestSb = new StringBuilder(FolderHelper.requestFolderURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());

                var requestFolderURL = requestSb.ToString();

                // string requestTestFolderURL = ProjectHelper.requestFolderURL + "project-09-01-aa" + "/" + "inc-001-project-09-01-aa-t002";


                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(requestFolderURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                // 테스트용 Http 통신 (Get) 방식 코드 필요시 사용 예정 (2023.09.06 minjae)
                // HttpResponseMessage response = await pClient.GetAsync(requestTestFolderURL);

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 4. 폴더 목록 조회 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    JsonArray resultData = JsonObject.Parse(resMsg)[ProjectHelper.resultData] as JsonArray;

                    List<Dictionary<string, object>> folderList = new List<Dictionary<string, object>>();   // 폴더 리스트 객체 생성하기 

                    // resultData 값이 존재하는 경우 
                    if(resultData is not null)
                    {
                        // TODO : 테스트 코드 필요시 사용 예정 (2023.09.04 minjae)
                        // var values = JsonSerializer.Deserialize<Dictionary<string, object>>(resMsg)[ProjectHelper.resultData].ToString();

                        // TODO : JSON 데이터(resMsg) -> Dictionary<string, object> 변환(Convert) 구현 및 var jtmp에 데이터 할당 (2023.08.30 minjae)
                        //        할당 받은 데이터 var jtmp는 조사식 -> 키워드 "jtmp" 입력 및 엔터(또는 해당 var jdic 클릭 및 조사식으로 드래그) -> Results View 에서 확인 가능 - var 자료형 이여서 IEnumerable<JsonNode> 형태이기 때문 (2023.09.04 minjae)
                        // TODO : JSON 데이터(resMsg) 를 JsonObject.Parse(resMsg) 사용해서 "resultData" 항목만 파싱(Json Object(객체) 변환) -> 파싱한 Json Object(객체)안에 존재하는 데이터들을 JsonArray (JSON 배열) 변환
                        //var jtmp = (JsonObject.Parse(resMsg)[ProjectHelper.resultData] as JsonArray).Select(jsonNode => jsonNode);
                        var jtmp = resultData.Select(jsonNode => jsonNode);

                        // var jtmp = (JsonObject.Parse(resMsg) as JsonArray).Select(x => x);

                        // TODO : jtmp -> Select 메서드 사용 -> Dictionary<string, object>>(x) 형태로 Deserialize -> Deserialize해서 나온 Dictionary 객체를 List로 변환 및 해당 List에 속한 데이터를 var jsonList에 할당 (2023.08.31 minjae) 
                        //        할당 받은 데이터 var jsonList 조사식 -> 키워드 "jsonList" 입력 및 엔터(또는 해당 var jsonList 클릭 및 조사식으로 드래그) 확인 가능
                        //var jsonList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

                        //return jsonList;

                        // TODO : 폴더 리스트에 속한 폴더 데이터 Dictionary 객체의 Key "levelNo"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2023.09.12 minjae)
                        // 참고 URL - https://codechacha.com/ko/csharp-sort-list/
                        // 참고 2 URL - https://developer-talk.tistory.com/694
                        // 참고 3 URL - https://developer-talk.tistory.com/210
                        folderList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => Int32.Parse(x[FolderHelper.levelNo].ToString())).ToList();
                    }

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "4. 폴더 목록 조회 성공");

                    return folderList;
                }
                // Rest API 4. 폴더 목록 조회 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
                
            }
            catch(Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.04 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO :오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion GetFolderListAsync

        #region GetTeamMemberListAsync

        // TODO : 5. 팀 멤버 목록 조회 - 개발 대상 제외 (2023.11.28 minjae)

        #endregion GetTeamMemberListAsync

        #region GetContainerListAsync

        // TODO : HttpClient 클래스 사용 Http Get 방식 통신 자료 참고 (2023.11.28 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 6. 컨테이너 목록 조회 (Get)  
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<List<Dictionary<string, object>>> GetContainerListAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                     // HttpClient client 객체 가져오기 
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();        // Http 서버 통신 요청 Header 값 가져오기 
            ContainerPack containerPack = pInputParamData.GetContainerPack();        // 컨테이너 정보 값 가져오기  

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
                // Http Get 방식으로 호출해서 각각의 프로젝트 및 팀 아이디 하위의 폴더 + 파일 목록 JSON 데이터 가져오기 (2023.09.06 minjae)
                // 테스트용 URL 주소 - requestFolderURL = "https://cobim.io/api/explorer/folder/list/jc-constrct/inc-001-jc-constrct-t001";
                // string requestTestFolderURL = ProjectHelper.requestFolderURL + "project-09-01-aa" + "/" + "inc-001-project-09-01-aa-t002";

                // TODO : StringBuilder 클래스 객체 requestSb 생성 ->  6. 컨테이너 목록 조회 (파일 포함) Rest API URL 주소에 문자열 추가 (2023.11.22 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                StringBuilder requestSb = new StringBuilder(ContainerHelper.requestContainerURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetSelectedFolderId());

                //var requestFileURL = requestSb.ToString();
                var requestContainerURL = requestSb.ToString();

                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(requestContainerURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 6. 컨테이너 목록 (파일) 리스트 조회 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.containerList] as JsonArray).Select(x => x);

                    // TODO : 컨테이너 리스트에 속한 컨테이너 데이터 Dictionary 객체의 Key "containerId"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2024.08.22 minjae)
                    // 참고 URL   - https://codechacha.com/ko/csharp-sort-list/
                    // 참고 2 URL - https://developer-talk.tistory.com/694
                    // 참고 3 URL - https://developer-talk.tistory.com/210
                    // var containerList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

                    var containerList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => x[ContainerHelper.containerId].ToString()).ToList();

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "6. 컨테이너 목록 조회 성공");

                    return containerList;
                }
                // Rest API 6. 컨테이너 목록 (파일) 리스트 조회 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
            }
            catch(Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion GetContainerListAsync

        #region GetFileDownLoadAsync

        // TODO : Rest API 파일 다운로드 (Get) 메서드 "GetFileDownLoadAsync" 구현 (2023.10.25 minjae)
        // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        /// <summary>
        /// 7. 파일 다운로드 (Get)
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task GetFileDownLoadAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                     // HttpClient client 객체 가져오기 
            string commonDirectoryPath = pInputParamData.GetCommonDirectoryPath();   // 폴더 (디렉토리) 공통 경로 값 가져오기 
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();        // Http 서버 통신 요청 Header 값 가져오기 
            ContainerPack containerPack = pInputParamData.GetContainerPack();        // 컨테이너 정보 값 가져오기 
            FilePack filePack = pInputParamData.GetFilePack();                       // 다운로드할 파일 정보 값 가져오기 

            string downloadFilePath = string.Empty;        // 다운로드 받으려고 하는 최종 파일 경로 (파일 전체 경로 + 이름 중복일 때 변경된 파일 이름)

            // TODO : C# 문자열 보간 기능 사용해서 로그파일 생성할 상위 폴더 경로 문자열로 변환 (2024.10.07 minjae)
            // 참고 URL - https://gaeunhan.tistory.com/61
            // TODO : 다른 PC에서도 파일 다운로드 기능이 실행될 수 있도록 다운로드 받을 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2024.10.07 minjae)
            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
            // 파일 다운로드 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimDockableDialogs\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002" - (SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) 사용)
            // TODO : 메서드 Assembly.GetCallingAssembly() 사용하여 비동기 메서드 "GetFileDownLoadAsync" 호출한 어셈블리 (CobimDockableDialogs) 문자열 가져올 때 실패한 원인(문자열 "System.Private.CoreLib" 으로 리턴) 파악 예정 (2024.10.07 minjae)
            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.reflection.assembly.getcallingassembly?view=net-8.0
            // string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetCallingAssembly().GetName().Name}\\{containerPack.GetProjectId()}\\{containerPack.GetTeamId()}\\{containerPack.GetContainerId()}";
            // string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\CobimExplorer\\{containerPack.GetProjectId()}\\{containerPack.GetTeamId()}\\{containerPack.GetContainerId()}";
            string dirPath = $"{commonDirectoryPath}\\{containerPack.GetProjectId()}\\{containerPack.GetTeamId()}\\{containerPack.GetContainerId()}";
            string filePath = dirPath + $"\\{filePack.GetFileName()}";

            DirectoryInfo di = new DirectoryInfo(dirPath);

            FileInfo fi = new FileInfo(filePath);

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.10.07 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 7. 파일 다운로드 (파일 다운로드 Rest API) URL 주소에 문자열 추가 (2024.10.07 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

                // Http 통신(Get) 테스트용 URL 주소 - requestFileDownLoadURL = "https://cobim.io/api/explorer/file/container/download/jc-constrct/inc-001-jc-constrct-t001";
                StringBuilder requestSb = new StringBuilder(FileHelper.requestFileDownLoadURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetContainerId());
                requestSb.Append("/");
                requestSb.Append(filePack.GetFileId());

                var requestFileDownLoadURL = requestSb.ToString();

                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2024.10.07 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(requestFileDownLoadURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 7. 파일 다운로드 Http 통신 결과가 성공할 경우 
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    var ms = await response.Content.ReadAsStreamAsync();

                    // 디렉터리(di)가 존재하지 않는 경우
                    if (false == di.Exists) Directory.CreateDirectory(dirPath);   // 디렉터리 새로 생성

                    // 새로 다운로드 받으려는 파일 전체 경로 - dirPath + 파일이름 - fi.Name 구하기
                    downloadFilePath = FileHelper.FileDownLoadPath(dirPath, containerPack, fi.Name);


                    // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2024.10.07 minjae)
                    //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
                    //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
                    // 참고 URL - https://hongjinhyeon.tistory.com/92
                    // TODO : httpClient 객체 client 사용 URL을 이용한 파일 다운로드 구현 (2024.10.07 minjae)
                    // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
                    // 참고 2 URL - https://stackoverflow.com/questions/45711428/download-file-with-webclient-or-httpclient
                    using (var fs = File.Create(downloadFilePath))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        await ms.CopyToAsync(fs);
                        await ms.FlushAsync();
                        await fs.FlushAsync();

                        // fs.Close();
                    }

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "7. 파일 다운로드 성공");

                    return;
                }

                // Rest API 7. 파일 다운로드 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
            }
            catch(Exception ex)
            {
                // TODO : Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.10.07 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion GetFileDownLoadAsync

        #region GetLinkedContainerListAsync

        // TODO : 아래 Rest API 메서드 "GetLinkedContainerListAsync" -> ExplorerVM.cs 비동기 메서드 "CheckOutAsync" 안에서 사용 (2023.11.28 minjae)
        /// <summary>
        /// 8. (참조/연관) 컨테이너 목록 조회 (Get) - 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너)
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<List<Dictionary<string, object>>> GetLinkedContainerListAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                     // HttpClient client 객체 가져오기 
                                                                                     // string commonDirectoryPath = pInputParamData.GetCommonDirectoryPath();   // 폴더 (디렉토리) 공통 경로 값 가져오기
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();        // Http 서버 통신 요청 Header 값 가져오기 
            ContainerPack containerPack = pInputParamData.GetContainerPack();        // 컨테이너 정보 값 가져오기 
            FilePack filePack = pInputParamData.GetFilePack();                       // 다운로드할 파일 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.10.08 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // TODO : 로그인 JWT 토큰 값 들어있는 string 클래스 객체 "authorization"를 bearer 토큰으로 변환하기 위해 문자열 "bearer " 추가해서
                // HttpClient 요청 헤더 client.DefaultRequestHeaders에 추가(TryAddWithoutValidation) (2024.10.08 minjae)
                // 추후 인재 INC와 소통 진행 -> JWT 토큰 또는 bearer 토큰 둘 중에 하나로 통일 예정 
                // 최종 결과 - client.DefaultRequestHeaders.Clear(); 사용하면 bearer 토큰 쓸 필요 없이 JWT 토큰만으로 사용 가능

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 8. 연관 컨테이너 목록 조회 URL 주소에 문자열 추가 (2024.10.08 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                StringBuilder requestSb = new StringBuilder(ContainerHelper.requestLinkedContainerURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetContainerId());

                var requestLinkedNodeInfoURL = requestSb.ToString();

                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2024.10.08 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(requestLinkedNodeInfoURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 8. 연관 컨테이너 목록 조회 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData] as JsonArray).Select(x => x);

                    // TODO : 연관 컨테이너 리스트에 속한 컨테이너 데이터 Dictionary 객체의 Key "containerId"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2024.10.08 minjae)
                    // 참고 URL   - https://codechacha.com/ko/csharp-sort-list/
                    // 참고 2 URL - https://developer-talk.tistory.com/694
                    // 참고 3 URL - https://developer-talk.tistory.com/210
                    var linkedNodeInfoList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => x[ContainerHelper.containerId].ToString()).ToList();

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "8. 연관 컨테이너 목록 조회 성공");

                    return linkedNodeInfoList;
                }

                // Rest API 8. 연관 컨테이너 목록 조회 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);

            }
            catch (Exception ex)
            {
                // TODO : Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.10.08 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw 
            }
        }

        #endregion GetLinkedContainerListAsync

        #region GetDetailListAsync

        /// <summary>
        /// 9. 컨테이너 상세 조회 (Get) - 컨테이너 정보, 파일정보 + 메타데이터 정보 + 마일스톤 정보
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<List<Dictionary<string, List<Dictionary<string, object>>>>> GetDetailListAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                // HttpClient client 객체 가져오기
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기 
            ContainerPack containerPack = pInputParamData.GetContainerPack();   // 컨테이너 정보 값 가져오기 

            // 컨테이너 정보, 파일정보 + 메타데이터 정보 + 마일스톤 정보를 같이 담을 2차원 리스트 total_Info_List
            List<Dictionary<string, List<Dictionary<string, object>>>> total_Info_List = new List<Dictionary<string, List<Dictionary<string, object>>>>();

            List<Dictionary<string, object>> linkedNodeInfoList = new List<Dictionary<string, object>>();    // 연관된 부모 컨테이너 정보 리스트
            var linkedNodeInfoDic = new Dictionary<string, List<Dictionary<string, object>>>();              // 연관된 부모 컨테이너 정보 Dictionary

            List<Dictionary<string, object>> containerDetailList = new List<Dictionary<string, object>>();   // 컨테이너 정보, 파일정보 리스트 
            var containerDetailDic = new Dictionary<string, List<Dictionary<string, object>>>();             // 컨테이너 정보, 파일정보 Dictionary 

            List<Dictionary<string, object>> metaDataList = new List<Dictionary<string, object>>();          // 메타데이터 정보 리스트 
            var metaDataDic = new Dictionary<string, List<Dictionary<string, object>>>();                    // 메타데이터 정보 Dictionary

            List<Dictionary<string, object>> taskDetailList = new List<Dictionary<string, object>>();        // 마일스톤 업무 리스트
            var taskDetailDic = new Dictionary<string, List<Dictionary<string, object>>>();                  // 마일스톤 업무 Dictionary

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                // TODO : 로그인 JWT 토큰 값 들어있는 string 클래스 객체 "authorization"를 bearer 토큰으로 변환하기 위해 문자열 "bearer " 추가해서
                // HttpClient 요청 헤더 client.DefaultRequestHeaders에 추가(TryAddWithoutValidation) (2023.10.12 minjae)
                // 추후 인재 INC와 소통 진행 -> JWT 토큰 또는 bearer 토큰 둘 중에 하나로 통일 예정 
                // 최종 결과 - client.DefaultRequestHeaders.Clear(); 사용하면 bearer 토큰 쓸 필요 없이 JWT 토큰만으로 사용 가능
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : 서버에 Http 통신(Get) 진행시 아래 URL 주소(ContainerHelper.requestContainerURL)에
                // 프로젝트 아이디, 팀 아이디, 컨테이너 아이디를 "pContainerPack" 메서드 파라미터로 넘겨 받아서 
                // Http Get 방식으로 호출해서 컨테이너 상세 조회 목록 JSON 데이터 가져오기 (2023.09.06 minjae)

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 9. 컨테이너 상세 조회 Rest API URL 주소에 문자열 추가 (2023.11.22 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                StringBuilder requestSb = new StringBuilder(ContainerHelper.requestDetailURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetContainerId());

                var requestDetailURL = requestSb.ToString();

                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(requestDetailURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
                //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2023.10.12 minjae)
                //        참고 URL - https://cano721.tistory.com/216
                //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
                //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 9. 컨테이너 상세 조회 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    // 연관된 부모 컨테이너 정보 리스트
                    var jlinkedNodeInfo = (JsonObject.Parse(resMsg)[ContainerHelper.linkedNodeInfo] as JsonArray);
                    // var jlinkedNodeInfoTmp = (JsonObject.Parse(resMsg)[ContainerHelper.linkedNodeInfo] as JsonArray).Select(x => x);
                    // TODO : 삼항 연산자 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.12.11 minjae)
                    // 참고 URL - https://husk321.tistory.com/405
                    // var jlinkedNodeInfoTmp = jlinkedNodeInfo is null ? null : jlinkedNodeInfo.Select(x => x);

                    // TODO : is null 연산자 대신 null 체크 연산자(?) 사용 (2023.12.12 minjae)
                    // 참고 URL - https://m.cafe.daum.net/aspdotnet/6TQG/3149
                    // 참고 2 URL - https://hwigyeom.ntils.com/3
                    // 참고 3 URL - https://cafe.daum.net/aspdotnet/WJSu/65
                    var jlinkedNodeInfoTmp = jlinkedNodeInfo?.Select(x => x);

                    // TODO : jdetailTmp null Exception이 발생하는 원인 파악하기 (2023.10.12 minjae)
                    // 컨테이너 정보, 파일정보
                    var jdetailNode = (JsonObject.Parse(resMsg)[ContainerHelper.detail]);
                    var jdetailTmp = JsonSerializer.Deserialize<Dictionary<string, object>>(jdetailNode);

                    // 메타데이터 정보
                    var jmetaDataTmp = (JsonObject.Parse(resMsg)[ContainerHelper.metaDataList] as JsonArray).Select(x => x);

                    // 마일스톤 업무
                    var jtaskDetailNode = (JsonObject.Parse(resMsg)[ContainerHelper.taskDetail]);
                    var jtaskDetailTmp = JsonSerializer.Deserialize<Dictionary<string, object>>(jtaskDetailNode);

                    // TODO : containerDetailList null Exception이 발생하는 원인 파악하기 (2023.10.12 minjae)
                    containerDetailList.Add(jdetailTmp);

                    // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 minjae)
                    // 참고 URL - https://husk321.tistory.com/405
                    // 연관 컨테이너 정보가 존재하는 경우에만 jlinkedNodeInfoTmp.Select 사용해서 "linkedNodeInfoList"에 데이터 할당
                    //if (jlinkedNodeInfoTmp is not null) linkedNodeInfoList.Add(jlinkedNodeInfoTmp);
                    if (jlinkedNodeInfoTmp is not null) linkedNodeInfoList = jlinkedNodeInfoTmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();
                    metaDataList = jmetaDataTmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();
                    taskDetailList.Add(jtaskDetailTmp);

                    linkedNodeInfoDic.Add(ContainerHelper.linkedNodeInfo, linkedNodeInfoList);
                    containerDetailDic.Add(ContainerHelper.detail, containerDetailList);
                    metaDataDic.Add(ContainerHelper.metaDataList, metaDataList);
                    taskDetailDic.Add(ContainerHelper.taskDetail, taskDetailList);

                    total_Info_List.Add(linkedNodeInfoDic);       // 연관된 부모 컨테이너 정보 리스트에 추가
                    total_Info_List.Add(containerDetailDic);      // 컨테이너 정보, 파일정보 total_Info_List에 추가
                    total_Info_List.Add(metaDataDic);             // 메타데이터 정보 total_Info_List에 추가
                    total_Info_List.Add(taskDetailDic);           // 마일스톤 정보 total_Info_List에 추가

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "9. 컨테이너 상세 조회 성공");

                    return total_Info_List;
                }
                // Rest API 9. 컨테이너 상세 조회 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
            }
            catch(Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion GetDetailListAsync

        #region GetCheckOutAsync

        /// <summary>
        /// 10. 컨테이너 체크아웃 + (파일)다운로드 (Get)
        /// /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task GetCheckOutAsync(ExplorerView pInputParamData)
        {
            // string createUserId  = string.Empty;     // 컨테이너 생성자
            // string workUserId    = string.Empty;     // 컨테이너 작업중인 사용자 아이디
            // string readYn        = string.Empty;     // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
            // string statusCode    = string.Empty;     // 컨테이너 검토상태 ("comstt10": 미진행, "comstt20": 진행, "comstt30": 완료)

            HttpClient client = pInputParamData.GetHttpClient();                 // HttpClient client 객체 가져오기 
            string commonDirectoryPath = pInputParamData.GetCommonDirectoryPath();   // 폴더 (디렉토리) 공통 경로 값 가져오기
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기 
            ContainerPack containerPack = pInputParamData.GetContainerPack();    // 컨테이너 정보 값 가져오기 
            FilePack filePack = pInputParamData.GetFilePack();                   // 다운로드할 파일 정보 값 가져오기 

            string downloadFilePath = string.Empty;     // 다운로드 받으려고 하는 최종 파일 경로 (파일 전체 경로 + 이름 중복일 때 변경된 파일 이름)

            // TODO : 다른 PC에서도 파일 다운로드 기능이 실행될 수 있도록 다운로드 받을 파일 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2024.10.08 minjae)
            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
            // TODO : 메서드 Assembly.GetCallingAssembly() 사용하여 비동기 메서드 "GetCheckOutAsync" 호출한 어셈블리 (CobimDockableDialogs) 문자열 가져올 때 실패한 원인(문자열 "System.Private.CoreLib" 으로 리턴) 파악 예정 (2024.10.08 minjae)
            // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.reflection.assembly.getcallingassembly?view=net-8.0
            // string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetCallingAssembly().GetName().Name}\\{containerPack.GetProjectId()}\\{containerPack.GetTeamId()}\\{containerPack.GetContainerId()}";
            // string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\CobimExplorer\\{containerPack.GetProjectId()}\\{containerPack.GetTeamId()}\\{containerPack.GetContainerId()}";
            string dirPath = $"{commonDirectoryPath}\\{containerPack.GetProjectId()}\\{containerPack.GetTeamId()}\\{containerPack.GetContainerId()}";
            string filePath = dirPath + $"\\{filePack.GetFileName()}";
            DirectoryInfo di = new DirectoryInfo(dirPath);
            FileInfo fi = new FileInfo(filePath);

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.10.08 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 10. 컨테이너 체크아웃 + (파일)다운로드 Rest API URL 주소에 문자열 추가 (2024.10.08 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

                // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
                // Http Get 방식으로 호출해서 10. 컨테이너 체크아웃 + (파일)다운로드 Rest API 테스트 진행 (2024.10.08 minjae)
                // 테스트용 URL 주소 - requestCheckOutURL = "https://cobim.io/api/explorer/container/checkout/jc-constrct/inc-001-jc-constrct-t001/jc-constrct-ijinc-sptl_902-func_010-006";
                StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCheckOutURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetContainerId());

                var requestCheckOutURL = requestSb.ToString();

                // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2024.10.08 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using HttpResponseMessage response = await client.GetAsync(requestCheckOutURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 10. 컨테이너 체크아웃 + (파일)다운로드 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    var ms = await response.Content.ReadAsStreamAsync();

                    // 디렉터리(di)가 존재하지 않는 경우
                    if (false == di.Exists) Directory.CreateDirectory(dirPath);   // 디렉터리 새로 생성

                    // 새로 다운로드 받으려는 파일 전체 경로 - dirPath + 파일이름 - fi.Name 구하기
                    downloadFilePath = FileHelper.FileDownLoadPath(dirPath, containerPack, fi.Name);

                    // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2024.10.08 minjae)
                    //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
                    //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
                    // 참고 URL - https://hongjinhyeon.tistory.com/92
                    // TODO : HttpClient 클래스 객체 client 사용 URL을 이용한 파일 다운로드 구현 (2024.10.08 minjae)
                    // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
                    // 참고 2 URL - https://stackoverflow.com/questions/45711428/download-file-with-webclient-or-httpclient

                    using (FileStream fs = File.Create(downloadFilePath))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        await ms.CopyToAsync(fs);
                        await ms.FlushAsync();
                        await fs.FlushAsync();

                        // fs.Close();
                    }

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "10. 컨테이너 체크아웃 + (파일)다운로드 성공");

                    return;
                }

                // Rest API 10. 컨테이너 체크아웃 + (파일)다운로드 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);

            }
            catch (Exception ex)
            {
                // TODO : Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.10.08 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw 
            }
        }

        #endregion GetCheckOutAsync

        #region PostCheckInAsync

        /// <summary>
        /// 11. 컨테이너 체크인 + 업로드 (POST)
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<Dictionary<string, object>> PostCheckInAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                // HttpClient client 객체 가져오기 
            // string commonDirectoryPath = pInputParamData.GetCommonDirectoryPath();   // 폴더 (디렉토리) 공통 경로 값 가져오기
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기 
            ContainerPack containerPack = pInputParamData.GetContainerPack();   // 컨테이너 정보 값 가져오기 
            FilePack filePack = pInputParamData.GetFilePack();                  // 컨테이너 체크인 + 파일 웹서버 업로드 할 파일 정보 값 가져오기
            CheckInPack checkInPack = pInputParamData.GetCheckInPack();         // 컨테이너 체크인 + 파일 웹서버 업로드 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.10.11 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.10.11 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent" 구현 (2024.10.11 minjae)
                // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient

                // TODO : 자바 스프링(Java Spring) 오류 메시지 "org.springframework.validation.BindException" 출력 원인 파악 (2024.10.11 minjae)
                // 참고 URL - https://jy-note.tistory.com/7

                // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2024.10.11 minjae)
                // 참고 URL - https://brokul.dev/sending-files-and-additional-data-using-httpclient-in-net-core
                var strCnt = new StreamContent(filePack.GetUploadFiles());
                var fCnt = new ByteArrayContent(strCnt.ReadAsByteArrayAsync().Result);
                fCnt.Headers.ContentType = MediaTypeHeaderValue.Parse(ContainerHelper.formDataContentType);

                // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2024.10.11 minjae)
                // 참고 URL - https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
                using var formContent = new MultipartFormDataContent
                { 
                    // 요청 데이터 Body(form-data) 문자열 
                    { new StringContent(checkInPack.GetFolderId(), Encoding.UTF8), ContainerHelper.folderId },
                    { new StringContent(filePack.GetFileName(), Encoding.UTF8), ContainerHelper.fileName },
                    { new StringContent(filePack.GetFileSize(), Encoding.UTF8), ContainerHelper.fileSize },
                    { new StringContent(checkInPack.GetComment(),  Encoding.UTF8), ContainerHelper.comment  },
                    // 요청 데이터 Body(form-data) 이미지 파일 (uploadFiles)
                    { fCnt, ContainerHelper.uploadFiles, filePack.GetFileName() }
                };

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 11. 컨테이너 체크인 + 업로드 (POST) Rest API URL 주소에 문자열 추가 (2024.10.11 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

                // TODO : 서버에 Http 통신(POST) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀 아이디 "inc-001-jc-constrct-t001"
                //        컨테이너 아이디 "jc-constrct-ijinc-sptl_902-func_010-006" 메서드 파라미터로 넘겨 받아서 
                //        Http POST 방식으로 호출해서 11. 컨테이너 체크인 + 업로드 (POST) Rest API 테스트 진행 (2024.10.11 minjae)
                // 테스트용 URL 주소 - requestCheckOutURL = "https://cobim.io/api/explorer/container/checkin/jc-constrct/inc-001-jc-constrct-t001/jc-constrct-ijinc-sptl_902-func_010-006";
                StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCheckInURL);
                requestSb.Append(containerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(containerPack.GetContainerId());

                var requestCheckInURL = requestSb.ToString();

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.10.11 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent"구현 (2024.10.11 minjae)
                // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient
                using HttpResponseMessage response = await client.PostAsync(requestCheckInURL, formContent);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
                //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2024.10.11 minjae)
                //        참고 URL - https://cano721.tistory.com/216
                //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
                //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
                string resMsg = await response.Content.ReadAsStringAsync();

                // Rest API 11. 컨테이너 체크인 + 업로드 Http 통신 결과가 성공할 경우 
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    // TODO : 서버로 부터 받은 json 데이터에 속한 "resultData" 영역이 리스트가 아니기 때문에 아래처럼 (JsonObject.Parse(resMsg) 사용 -> JsonNode(JsonObject 배열) 형식으로 변환 후 JSON 문자열 변환(ToJsonString()) (2024.10.11 minjae)
                    // 참고 URL - https://stackoverflow.com/questions/76035924/c-sharp-list-field-names-in-a-jsonobject-or-jsonnode
                    // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode.tojsonstring?view=net-8.0
                    var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).ToJsonString();

                    // TODO : JsonString 객체 jtmp를 Dictionary<string, object> 클래스 객체 "resultData"로 변환할 수 있도록 JsonSerializer.Deserialize 진행 (2024.10.11 minjae)
                    // 참고 URL - https://stackoverflow.com/questions/70111672/read-from-json-file-into-ienumerable-using-system-text-json
                    var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(jtmp);

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "11. 컨테이너 체크인 + 업로드 성공");

                    return resultData;
                }
                // Rest API 11. 컨테이너 체크인 + 업로드 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
            }
            catch (Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.10.11 minjae)

                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion PostCheckInAsync

        #region GetCurrentUserInfoAsync

        // TODO : 12. 현재 로그인 사용자 정보 조회 - 개발 대상 제외 (2023.11.28 minjae)

        #endregion GetCurrentUserInfoAsync

        #region GetProjectSettingInfoAsync

        // TODO : 13. 프로젝트 셋팅 정보 조회(메타데이터, 컨테이너 코드 등) - 개발 대상 제외 (2023.11.28 minjae)

        #endregion GetProjectSettingInfoAsync

        #region PorsContainerFileUpLoadAsync

        // TODO : 14. 컨테이너 생성(파일 업로드) (1) 파일 업로드 - 개발 대상 제외(웹에서만 사용) (2023.11.28 minjae)

        #endregion PorsContainerFileUpLoadAsync

        #region PorsContainerFileCreateAsync

        // TODO : 14. 컨테이너 생성(파일 업로드) (2) 컨테이너 생성 - 개발 대상 제외(웹에서만 사용) (2023.11.28 minjae)

        #endregion PorsContainerFileCreateAsync


        #region PostFileUpLoadAsync

        /// <summary>
        /// 15. 컨테이너 변경(파일 변경) - (1) 파일 업로드 (POST)
        /// </summary>
        //PostFileUpLoadAsync()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        #endregion PostFileUpLoadAsync

        #region PostContainerUpdateAsync

        /// <summary>
        /// 15. 컨테이너 변경(파일 변경) - (2) 컨테이너 변경 (POST)
        /// </summary>
        //PostContainerUpdateAsync()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //      throw;
        //    }
        //}

        #endregion PostContainerUpdateAsync

        #region GetTaskDetailListAsync

        // TODO : ComboxBox - 마일스톤의 경우 추후 인재 INC 측으로 마일스톤 Rest API 요청 후 개발 진행 (2023.10.20 minjae)
        /// <summary>
        /// 16. 마일스톤 목록 조회 (Get)
        /// </summary>
        // public static async Task<List<Dictionary<string, object>>> GetTaskDetailListAsync(HttpClient pClient, ProjectHelper.ProjectPack pProjectPack, string pFolderId, string authorization)
        public static async Task<object> GetTaskDetailListAsync()
        {
            // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
            var currentMethod = MethodBase.GetCurrentMethod();

            try
            {
                object obj = null;

                return obj;

            }
            catch (Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion GetTaskDetailListAsync

        #region PostCancelWorkAsync

        // TODO : ExplorerView.cs 소스파일에 속한
        //        컨테이너 작업 취소 정보 클래스 "CancelWorkPack"안에 속하는 프로퍼티 "projectId", "teamId", "containerId" 의 접근 제한자를 
        //        private로 설정하면 오류 발생하여 접근 제한자를 public으로 수정함. (2024.11.04 minjae)
        //        오류 발생 위치 - ExplorerRestServer.cs -> 비동기 메서드 "PostCancelWorkAsync" 
        //        -> 메서드 "JsonSerializer.Serialize" 호출하여 json 문자열(content)을 제대로 리턴 받지 못하고 오류 발생함. 

        // TODO : ExplorerView.cs 소스파일에 속한
        //        컨테이너 작업 취소 정보 클래스 "CancelWorkPack"안에 속하는 프로퍼티 "projectId", "teamId", "containerId" 프로퍼티 이름 
        //        변경하면 오류 발생하여 수정 안 하고 그대로 유지함.(2024.11.04 minjae)
        //        오류 발생 위치 - ExplorerRestServer.cs -> 비동기 메서드 "PostCancelWorkAsync" 
        //        서버로 부터 제대로 리턴 받지 못하고 오류 발생함. 

        /// <summary>
        /// 17. 컨테이너 작업 취소 (Post)
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<Dictionary<string, object>> PostCancelWorkAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                   // HttpClient client 객체 가져오기
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();      // Http 서버 통신 요청 Header 값 가져오기
            CancelWorkPack cancelWorkPack = pInputParamData.GetCancelWorkPack();   // 컨테이너 작업 취소 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : 컨테이너 작업 취소 정보 CancelWorkPack 클래스 객체 "cancelWorkPack" string 클래스 json 문자열(content) 구현 (2023.12.20 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
                // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc-001\"}";
                string requestData = JsonSerializer.Serialize(cancelWorkPack);

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                // TODO : 메서드 "StringContent"에 메서드 파라미터 Encoding 클래스 객체 "Encoding.UTF8" 제거 (2023.12.20 minjae)
                // TODO : 오류 메시지 "{"status":"org.springframework.web.HttpMediaTypeNotSupportedException: Content type 'text/plain;charset=utf-8' not supported","message":"Content type 'text/plain;charset=utf-8' not supported","error":"org.springframework.web.HttpMediaTypeNotSupportedException"}" 원인 파악 하기 (2023.12.20 minjae)
                //        오류 메시지 원인은 Content type을 추가하지 않을 경우 자동 생성되는 Content type이 "text/plain"이라서 발생한 오류이다.
                //        하여 아래처럼  Content type을 "LoginHelper.contentType(== "application/json")" 변경해야 한다.
                // 참고 URL - https://velog.io/@ds257873/Content-type-textplaincharsetUTF-8-not-supported
                using var jsonContent = new StringContent(
                                                requestData,
                                                Encoding.UTF8,
                                                ContainerHelper.contentType
                                            );

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 17. 컨테이너 작업 취소 (POST) Rest API URL 주소에 문자열 추가 (2023.12.20 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

                // TODO : 서버에 Http 통신(POST) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀 아이디 "inc-001-jc-constrct-t001"
                //        메서드 파라미터로 넘겨 받아서 Http POST 방식으로 호출 및
                //        17. 컨테이너 작업 취소 (POST) Rest API 테스트 진행 (2023.12.20 minjae)
                // 테스트용 URL 주소 - requestCancelWorkURL = "https://cobim.io/api/explorer/container/work/cancel/jc-constrct/inc-001-jc-constrct-t001";
                StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCancelWorkURL);
                requestSb.Append(cancelWorkPack.projectId);
                requestSb.Append("/");
                requestSb.Append(cancelWorkPack.teamId);

                var requestCancelWorkURL = requestSb.ToString();

                Debug.WriteLine($"{requestCancelWorkURL}");

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.20 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient   
                using HttpResponseMessage response = await client.PostAsync(requestCancelWorkURL, jsonContent);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
                //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2023.12.20 minjae)
                //        참고 URL - https://cano721.tistory.com/216
                //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
                //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
                // TODO : 오류 메시지 "{"status":"org.springframework.web.HttpMediaTypeNotSupportedException: Content type 'text/plain;charset=utf-8' not supported","message":"Content type 'text/plain;charset=utf-8' not supported","error":"org.springframework.web.HttpMediaTypeNotSupportedException"}" 원인 파악 후 해결 진행 예정 (2023.12.20 minjae)
                string resMsg = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"{resMsg}\n");

                // Rest API 17. 컨테이너 작업 취소 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    // var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).Select(x => x);
                    // TODO : 서버로 부터 받은 resMsg 데이터에 속한 "resultData" 영역이 리스트가 아니기 때문에 아래처럼 (JsonObject.Parse(resMsg) 사용 -> JsonNode(JsonObject 배열) 형식으로 변환 후 JSON 문자열 변환(ToJsonString()) (2023.12.20 minjae)
                    // 참고 URL - https://stackoverflow.com/questions/76035924/c-sharp-list-field-names-in-a-jsonobject-or-jsonnode
                    // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode.tojsonstring?view=net-8.0
                    var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).ToJsonString();

                    // TODO : JsonString 객체 jtmp를 Dictionary<string, object> 클래스 객체 "resultData"로 변환할 수 있도록 JsonSerializer.Deserialize 진행 (2023.12.20 minjae)
                    // 참고 URL - https://stackoverflow.com/questions/70111672/read-from-json-file-into-ienumerable-using-system-text-json
                    var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(jtmp);

                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "17. 컨테이너 작업 취소 성공");

                    return resultData;
                }
                // Rest API 17. 컨테이너 작업 취소 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
                
            }
            catch(Exception ex)
            {
                // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.20 minjae)
                // Log.Information(LogHelper.GetMethodPath(currentMethod) + ex.Message);
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
                // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
                throw;
            }
        }

        #endregion PostCancelWorkAsync

        #region Sample

        // TODO : 2. 나의 프로젝트(+ 팀목록) 목록 조회 (Get) 비동기 메서드 "GetProjectTeamListAsync" 필요시 참고 (2024.10.29 minjae)
        // TODO : 프로젝트 + 팀목록 조회 Get 방식 구현 (2023.08.29 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 2. 나의 프로젝트(+ 팀목록) 목록 조회 (Get)
        /// Rest API 메서드 파라미터 "HttpClient pClient, string authorization"
        /// </summary>
        // TODO : 메서드 "GetExplorerProjectTeamListAsync" 리턴 자료형 Task<Dictionary<string, object>>으로 구현 예정 (2023.08.30 minjae)
        // public static async Task<string> GetExplorerProjectTeamListAsync(HttpClient pClient, string authorization)
        //public static async Task<List<Dictionary<string, object>>> GetProjectTeamListAsync(HttpClient pClient, RequestHeader pRequestHeaders)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    // 참고 URL - https://slaner.tistory.com/73
        //    // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        //    // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.08.30 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(ProjectHelper.requestProjectTeamURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 2. 나의 프로젝트 목록 조회 (Get) Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            /// <summary>
        //            /// TODO : JSON 데이터(resMsg) -> Dictionary<string, object> 변환(Convert) 구현 및 var jtmp에 데이터 할당 (2023.08.30 minjae)
        //            /// (할당 받은 데이터는 조사식 -> 키워드 "jtmp" 입력 및 엔터 -> ResultView 에서 확인 가능 - var 자료형 이여서 IEnumerable<JsonNode> 형태이기 때문) (2023.08.30 minjae)
        //            /// 참고 URL - https://stackoverflow.com/questions/1207731/how-can-i-deserialize-json-to-a-simple-dictionarystring-string-in-asp-net
        //            /// using System.Text.Json
        //            /// 참고 URL - https://www.csharpstudy.com/Data/Json-SystemTextJson.aspx
        //            /// 참고 2 URL - https://dhddl.tistory.com/250
        //            /// JsonNode 클래스 - 변경 가능한 JSON 문서 내의 단일 노드를 나타내는 기본 클래스를 의미
        //            /// 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode?view=net-7.0
        //            /// JSON 이란? - 데이터를 표현하는 문자열 
        //            /// 파싱이란? - 다른 형식으로 저장된 데이터를 원하는 형식의 데이터로 변환하는 것을 말한다.
        //            /// JSON 파싱 - JSON 형식의 문자열을 객체로 변환하는 것
        //            /// 참고 URL - https://velog.io/@lzhxxn/JSON-Parsing%ED%8C%8C%EC%8B%B1%EC%9D%B4%EB%9E%80
        //            /// </summary>


        //            // TODO : JSON 데이터(resMsg) -> Dictionary<string, object> 변환(Convert) 구현 및 var jtmp에 데이터 할당 (2023.08.30 minjae)
        //            //        할당 받은 데이터 var jtmp는 조사식 -> 키워드 "jtmp" 입력 및 엔터(또는 해당 var jdic 클릭 및 조사식으로 드래그) -> Results View 에서 확인 가능 - var 자료형 이여서 IEnumerable<JsonNode> 형태이기 때문 (2023.08.30 minjae)
        //            // TODO : JSON 데이터(resMsg) 를 JsonObject.Parse(resMsg) 사용해서 "resultData" 항목만 파싱(Json Object(객체) 변환) -> 파싱한 Json Object(객체)안에 존재하는 데이터들을 JsonArray (JSON 배열) 변환
        //            //        -> 해당 JsonArray (JSON 배열)에서 항목(Object) "projectEntity" 추출 -> 추출한 JsonArray 데이터를 var jtmp로 할당 하도록 구현 (2023.08.30 minjae)
        //            // var jtmp = (JsonObject.Parse(resMsg)[ProjectHelper.resultData] as JsonArray).Select(x => x[ProjectHelper.projectEntity]);

        //            // TODO : 테스트 JSON 데이터(resMsg) 구조 변경으로 인하여 "projectWithTeamInfos" 영역 안 데이터 "projectEntity", "teamEntity" 추출 구현 (2023.09.12 minjae)
        //            var jtmp = (JsonObject.Parse(resMsg)[ProjectHelper.projectWithTeamInfos] as JsonArray).Select(x => x);

        //            // JsonNode -> json 데이터 형변환 구현 (2023.08.31 minjae)
        //            // string Testjson = JsonSerializer.Serialize(jtmp);

        //            // TODO : jtmp -> Select 메서드 사용 -> Dictionary<string, object>>(x) 형태로 Deserialize -> Deserialize해서 나온 Dictionary 객체를 List로 변환 및 해당 List에 속한 데이터를 var jdic에 할당 (2023.08.31 minjae) 
        //            //        할당 받은 데이터 var jdic는 조사식 -> 키워드 "jdic" 입력 및 엔터(또는 해당 var jdic 클릭 및 조사식으로 드래그) 확인 가능
        //            //var jdic = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

        //            //return jdic;

        //            var projectTeamList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "2. 나의 프로젝트 목록 조회 성공");

        //            return projectTeamList;
        //        }
        //        // Rest API 2. 나의 프로젝트 목록 조회 (Get) Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);


        //        // TODO : 테스트 코드 필요시 사용 예정 (List<Dictionary<string, object>> 객체 jdic -> json 데이터 형변환 구현) (2023.08.31 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/9110724/serializing-a-list-to-json
        //        // string jsonDic = JsonSerializer.Serialize(jdic);

        //        //var pNameList = jtmp.Select(x => x["projectName"].ToString()).ToList();

        //        // TODO : 테스트 코드 필요시 사용 예정 (2023.09.04 minjae)
        //        // var values = JsonSerializer.Deserialize<Dictionary<string, object>>(resMsg)[ProjectHelper.resultData].ToString();

        //        // TODO : 테스트 코드 필요시 사용 예정 (2023.09.04 minjae)
        //        // var temp = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(values);

        //        //var a = temp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x["projectEntity"].ToString()))
        //        //    .Select(x => x["projectName"].ToString()).ToList();
        //        //    ;

        //        // TODO : 테스트 코드 필요시 사용 예정 (JSON 데이터 중 "projectEntity" 항목 List 데이터를 var a에 할당) (2023.08.30 minjae)
        //        // var a = temp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x["projectEntity"].ToString())).ToList();

        //        //var temp2 = JsonSerializer.Deserialize<Dictionary<string, object>>(temp[0].ToString())["projectName"];

        //        //return values;
        //        //return jdic;
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.08.30 minjae)
        //        // Logger.Error(currentMethod, Logger.errorMessage + ex.Message);
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 4. 폴더 목록 조회 (Get) 비동기 메서드 "GetFolderListAsync" 필요시 참고 (2024.10.29 minjae)
        // TODO : 4. 폴더 목록 조회 Get 방식 구현 (2023.09.01 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 4. 폴더 목록 조회 (Get)
        /// Rest API 메서드 파라미터 "HttpClient pClient, string projectId, string teamId, string authorization"
        /// </summary>
        // TODO : 메서드 "GetExplorerFolderListAsync" 리턴 자료형 Task<Dictionary<string, object>>으로 구현 예정 (2023.08.30 minjae)
        // public static async Task<string> GetExplorerProjectListAsync(HttpClient pClient, string authorization)
        //public static async Task<List<Dictionary<string, object>>> GetFolderListAsync(HttpClient pClient, ProjectPack pProjectPack, RequestHeader pRequestHeaders)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    // 참고 URL - https://slaner.tistory.com/73
        //    // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        //    // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.04 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 각각의 프로젝트 및 팀 아이디 하위의 폴더 + 파일 목록 JSON 데이터 가져오기 (2023.09.06 minjae)
        //        // 테스트용 URL 주소 - requestFolderURL = "https://cobim.io/api/explorer/folder/list/jc-constrct/inc-001-jc-constrct-t001";

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 4. 폴더 목록 조회 URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        StringBuilder requestSb = new StringBuilder(FolderHelper.requestFolderURL);
        //        requestSb.Append(pProjectPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pProjectPack.teamId);

        //        var requestFolderURL = requestSb.ToString();

        //        // string requestTestFolderURL = ProjectHelper.requestFolderURL + "project-09-01-aa" + "/" + "inc-001-project-09-01-aa-t002";


        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestFolderURL);

        //        // 테스트용 Http 통신 (Get) 방식 코드 필요시 사용 예정 (2023.09.06 minjae)
        //        // HttpResponseMessage response = await pClient.GetAsync(requestTestFolderURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 4. 폴더 목록 조회 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            JsonArray resultData = JsonObject.Parse(resMsg)[ProjectHelper.resultData] as JsonArray;

        //            List<Dictionary<string, object>> folderList = new List<Dictionary<string, object>>();   // 폴더 리스트 객체 생성하기 

        //            // resultData 값이 존재하는 경우 
        //            if (resultData is not null)
        //            {
        //                // TODO : 테스트 코드 필요시 사용 예정 (2023.09.04 minjae)
        //                // var values = JsonSerializer.Deserialize<Dictionary<string, object>>(resMsg)[ProjectHelper.resultData].ToString();

        //                // TODO : JSON 데이터(resMsg) -> Dictionary<string, object> 변환(Convert) 구현 및 var jtmp에 데이터 할당 (2023.08.30 minjae)
        //                //        할당 받은 데이터 var jtmp는 조사식 -> 키워드 "jtmp" 입력 및 엔터(또는 해당 var jdic 클릭 및 조사식으로 드래그) -> Results View 에서 확인 가능 - var 자료형 이여서 IEnumerable<JsonNode> 형태이기 때문 (2023.09.04 minjae)
        //                // TODO : JSON 데이터(resMsg) 를 JsonObject.Parse(resMsg) 사용해서 "resultData" 항목만 파싱(Json Object(객체) 변환) -> 파싱한 Json Object(객체)안에 존재하는 데이터들을 JsonArray (JSON 배열) 변환
        //                //var jtmp = (JsonObject.Parse(resMsg)[ProjectHelper.resultData] as JsonArray).Select(jsonNode => jsonNode);
        //                var jtmp = resultData.Select(jsonNode => jsonNode);

        //                // var jtmp = (JsonObject.Parse(resMsg) as JsonArray).Select(x => x);

        //                // TODO : jtmp -> Select 메서드 사용 -> Dictionary<string, object>>(x) 형태로 Deserialize -> Deserialize해서 나온 Dictionary 객체를 List로 변환 및 해당 List에 속한 데이터를 var jsonList에 할당 (2023.08.31 minjae) 
        //                //        할당 받은 데이터 var jsonList 조사식 -> 키워드 "jsonList" 입력 및 엔터(또는 해당 var jsonList 클릭 및 조사식으로 드래그) 확인 가능
        //                //var jsonList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

        //                //return jsonList;

        //                // TODO : 폴더 리스트에 속한 폴더 데이터 Dictionary 객체의 Key "levelNo"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2023.09.12 minjae)
        //                // 참고 URL - https://codechacha.com/ko/csharp-sort-list/
        //                // 참고 2 URL - https://developer-talk.tistory.com/694
        //                // 참고 3 URL - https://developer-talk.tistory.com/210
        //                folderList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => Int32.Parse(x[FolderHelper.levelNo].ToString())).ToList();
        //            }

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "4. 폴더 목록 조회 성공");

        //            return folderList;
        //        }
        //        // Rest API 4. 폴더 목록 조회 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);

        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.04 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO :오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 6. 컨테이너 목록 조회 (Get)   비동기 메서드 "GetContainerListAsync" 필요시 참고 (2024.10.29 minjae)
        // TODO : HttpClient 클래스 사용 Http Get 방식 통신 자료 참고 (2023.11.28 minjae)
        // 참고 URL - https://thebook.io/006890/1017/
        /// <summary>
        /// 6. 컨테이너 목록 조회 (Get)  
        /// Rest API 메서드 파라미터 "HttpClient pClient, string projectId, string teamId, string authorization"
        /// </summary>
        // TODO : 메서드 "GetExplorerContainerListAsync" 리턴 자료형 Task<Dictionary<string, object>>으로 구현 예정 (2023.08.30 minjae)
        // public static async Task<string> GetExplorerProjectListAsync(HttpClient pClient, string authorization)
        //public static async Task<List<Dictionary<string, object>>> GetContainerListAsync(HttpClient pClient, ContainerPack pContainerPack, string pFolderId, RequestHeader pRequestHeaders)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    // 참고 URL - https://slaner.tistory.com/73
        //    // 참고 2 URL - https://stackoverflow.com/questions/4132810/how-can-i-get-a-method-name-with-the-namespace-class-name
        //    // 참고 3 URL - https://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 각각의 프로젝트 및 팀 아이디 하위의 폴더 + 파일 목록 JSON 데이터 가져오기 (2023.09.06 minjae)
        //        // 테스트용 URL 주소 - requestFolderURL = "https://cobim.io/api/explorer/folder/list/jc-constrct/inc-001-jc-constrct-t001";
        //        // string requestTestFolderURL = ProjectHelper.requestFolderURL + "project-09-01-aa" + "/" + "inc-001-project-09-01-aa-t002";

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 ->  6. 컨테이너 목록 조회 (파일 포함) Rest API URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestContainerURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pFolderId);

        //        //var requestFileURL = requestSb.ToString();
        //        var requestContainerURL = requestSb.ToString();

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestContainerURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 6. 컨테이너 목록 (파일) 리스트 조회 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.containerList] as JsonArray).Select(x => x);

        //            // TODO : 컨테이너 리스트에 속한 컨테이너 데이터 Dictionary 객체의 Key "containerId"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2024.08.22 minjae)
        //            // 참고 URL   - https://codechacha.com/ko/csharp-sort-list/
        //            // 참고 2 URL - https://developer-talk.tistory.com/694
        //            // 참고 3 URL - https://developer-talk.tistory.com/210
        //            // var containerList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();

        //            var containerList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => x[ContainerHelper.containerId].ToString()).ToList();

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "6. 컨테이너 목록 조회 성공");

        //            return containerList;
        //        }
        //        // Rest API 6. 컨테이너 목록 (파일) 리스트 조회 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);
        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 7. 파일 다운로드 (Get) 비동기 메서드 "GetFileDownLoadAsync" 필요시 참고 (2024.10.29 minjae)
        // TODO : Rest API 파일 다운로드 (Get) 메서드 "GetFileDownLoadAsync" 구현 (2023.10.25 minjae)
        // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        /// <summary>
        /// 7. 파일 다운로드 (Get)
        /// </summary>
        //public static async Task GetFileDownLoadAsync(HttpClient pClient, ContainerPack pContainerPack, FilePack pFilePack, RequestHeader pRequestHeaders)
        //{
        //    string downloadFilePath = string.Empty;        // 다운로드 받으려고 하는 최종 파일 경로 (파일 전체 경로 + 이름 중복일 때 변경된 파일 이름)

        //    // TODO : C# 문자열 보간 기능 사용해서 로그파일 생성할 상위 폴더 경로 문자열로 변환 (2023.10.11 minjae)
        //    // 참고 URL - https://gaeunhan.tistory.com/61
        //    // TODO : 다른 PC에서도 파일 다운로드 기능이 실행될 수 있도록 다운로드 받을 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.10.26 minjae)
        //    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //    // 파일 다운로드 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002" - (SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) 사용)
        //    string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetEntryAssembly().GetName().Name}\\{pContainerPack.projectId}\\{pContainerPack.teamId}\\{pContainerPack.containerId}";
        //    string filePath = dirPath + $"\\{pFilePack.fileName}";

        //    DirectoryInfo di = new DirectoryInfo(dirPath);

        //    FileInfo fi = new FileInfo(filePath);

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);


        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 7. 파일 다운로드 (파일 다운로드 Rest API) URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 파일 다운로드 테스트 진행 (2023.10.25 minjae)
        //        // 테스트용 URL 주소 - requestFileDownLoadURL = "https://cobim.io/api/explorer/file/container/download/jc-constrct/inc-001-jc-constrct-t001";
        //        StringBuilder requestSb = new StringBuilder(FileHelper.requestFileDownLoadURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);
        //        requestSb.Append("/");
        //        requestSb.Append(pFilePack.fileId);

        //        var requestFileDownLoadURL = requestSb.ToString();

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 파일 다운로드 테스트 진행 (2023.10.25 minjae)
        //        // 테스트용 URL 주소 - requestFileDownLoadURL = "https://cobim.io/api/explorer/file/container/download/jc-constrct/inc-001-jc-constrct-t001";
        //        // string requestFileDownLoadURL = FileHelper.requestFileDownLoadURL + pContainerPack.projectId + "/" + pContainerPack.teamId + "/" + pContainerPack.containerId + "/" + pFilePack.fileId;

        //        Debug.WriteLine($"{requestFileDownLoadURL}");

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestFileDownLoadURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 7. 파일 다운로드 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var ms = await response.Content.ReadAsStreamAsync();

        //            // 디렉터리(di)가 존재하지 않는 경우
        //            if (false == di.Exists) Directory.CreateDirectory(dirPath);   // 디렉터리 새로 생성

        //            // 새로 다운로드 받으려는 파일 전체 경로 - dirPath + 파일이름 - fi.Name 구하기
        //            downloadFilePath = FileHelper.FileDownLoadPath(dirPath, pContainerPack, fi.Name);


        //            // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2023.10.25 minjae)
        //            //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
        //            //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
        //            // 참고 URL - https://hongjinhyeon.tistory.com/92
        //            // TODO : httpClient 객체 pClient 사용 URL을 이용한 파일 다운로드 구현 (2023.10.26 minjae)
        //            // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        //            // 참고 2 URL - https://stackoverflow.com/questions/45711428/download-file-with-webclient-or-httpclient
        //            using (var fs = File.Create(downloadFilePath))
        //            {
        //                ms.Seek(0, SeekOrigin.Begin);
        //                await ms.CopyToAsync(fs);
        //                await ms.FlushAsync();
        //                await fs.FlushAsync();

        //                // fs.Close();
        //            }

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "7. 파일 다운로드 성공");

        //            return;
        //        }

        //        // Rest API 7. 파일 다운로드 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);

        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        // TODO : 7. 파일 다운로드 (Get) 비동기 메서드 "GetFileDownLoadAsync" 필요시 참고 (2024.10.07 minjae)
        // TODO : Rest API 파일 다운로드 (Get) 메서드 "GetFileDownLoadAsync" 구현 (2023.10.25 minjae)
        // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        /// <summary>
        /// 7. 파일 다운로드 (Get)
        /// </summary>
        //public static async Task GetFileDownLoadAsync(HttpClient pClient, ContainerPack pContainerPack, FilePack pFilePack, RequestHeader pRequestHeaders)
        //{
        //    // string fileName       = string.Empty;        // 다운로드 받으려고 하는 파일 이름
        //    string downloadFilePath = string.Empty;        // 다운로드 받으려고 하는 최종 파일 경로 (파일 전체 경로 + 이름 중복일 때 변경된 파일 이름)

        //    // TODO : C# 문자열 보간 기능 사용해서 로그파일 생성할 상위 폴더 경로 문자열로 변환 (2023.10.11 minjae)
        //    // 참고 URL - https://gaeunhan.tistory.com/61
        //    // string dirPath = $"D:\\bhjeon\\COBIM-Explorer\\CobimExplorer\\CobimExplorer\\bin\\x64\\Debug\\Logs\\{projectFileName}";
        //    // Assembly.GetEntryAssembly().GetName().Name은 프로젝트 파일 "CobimExplorer"를 의미 - C:\\Users\\bhjeon\\Documents
        //    // string dirPath = $"C:\\Users\\bhjeon\\Documents\\{Assembly.GetEntryAssembly().GetName().Name}\\{pContainerPack.projectId}\\{pContainerPack.teamId}\\{pContainerPack.containerId}";

        //    // TODO : 다른 PC에서도 파일 다운로드 기능이 실행될 수 있도록 다운로드 받을 파일 전체 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.10.26 minjae)
        //    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //    // 파일 다운로드 전체 경로 예시 - "C:\Users\bhjeon\Documents\COBIM\CobimExplorer\bmcb\inc-001-bmcb-t005\bmcb-imbu-func_016-sptl_901-002" - (SpecialFolder 시스템 디렉토리(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) 사용)
        //    string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetEntryAssembly().GetName().Name}\\{pContainerPack.projectId}\\{pContainerPack.teamId}\\{pContainerPack.containerId}";
        //    string filePath = dirPath + $"\\{pFilePack.fileName}";

        //    DirectoryInfo di = new DirectoryInfo(dirPath);

        //    FileInfo fi = new FileInfo(filePath);

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);


        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 7. 파일 다운로드 (파일 다운로드 Rest API) URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 파일 다운로드 테스트 진행 (2023.10.25 minjae)
        //        // 테스트용 URL 주소 - requestFileDownLoadURL = "https://cobim.io/api/explorer/file/container/download/jc-constrct/inc-001-jc-constrct-t001";
        //        StringBuilder requestSb = new StringBuilder(FileHelper.requestFileDownLoadURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);
        //        requestSb.Append("/");
        //        requestSb.Append(pFilePack.fileId);

        //        var requestFileDownLoadURL = requestSb.ToString();

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 파일 다운로드 테스트 진행 (2023.10.25 minjae)
        //        // 테스트용 URL 주소 - requestFileDownLoadURL = "https://cobim.io/api/explorer/file/container/download/jc-constrct/inc-001-jc-constrct-t001";
        //        // string requestFileDownLoadURL = FileHelper.requestFileDownLoadURL + pContainerPack.projectId + "/" + pContainerPack.teamId + "/" + pContainerPack.containerId + "/" + pFilePack.fileId;

        //        Debug.WriteLine($"{requestFileDownLoadURL}");

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestFileDownLoadURL);

        //        // Rest API 7. 파일 다운로드 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var ms = await response.Content.ReadAsStreamAsync();

        //            // 디렉터리(di)가 존재하지 않는 경우
        //            if (false == di.Exists) Directory.CreateDirectory(dirPath);   // 디렉터리 새로 생성

        //            // 새로 다운로드 받으려는 파일 전체 경로 - dirPath + 파일이름 - fi.Name 구하기
        //            downloadFilePath = FileHelper.FileDownLoadPath(dirPath, pContainerPack, fi.Name);


        //            // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2023.10.25 minjae)
        //            //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
        //            //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
        //            // 참고 URL - https://hongjinhyeon.tistory.com/92
        //            // TODO : httpClient 객체 pClient 사용 URL을 이용한 파일 다운로드 구현 (2023.10.26 minjae)
        //            // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        //            // 참고 2 URL - https://stackoverflow.com/questions/45711428/download-file-with-webclient-or-httpclient
        //            using (var fs = File.Create(downloadFilePath))
        //            {
        //                ms.Seek(0, SeekOrigin.Begin);
        //                await ms.CopyToAsync(fs);
        //                await ms.FlushAsync();
        //                await fs.FlushAsync();

        //                // fs.Close();
        //            }

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "7. 파일 다운로드 성공");

        //            return;
        //        }

        //        // Rest API 7. 파일 다운로드 Http 통신 결과가 실패할 경우 
        //        else
        //        {
        //            string resMsg = await response.Content.ReadAsStringAsync();
        //            throw new Exception(resMsg);
        //        }



        //        // TODO : 아래 테스트 코드 필요시 참고 (2023.10.25 minjae)
        //        // 이미지 파일(fi)이 존재하지 않는 경우 
        //        //if(false == fi.Exists)
        //        //{
        //        //    // TODO : 아래 테스트 코드 2줄은 필요시 참고 (2023.10.25 minjae)
        //        //    //        단, 아래 테스트 코드 2줄 사용은 가능하나 다운로드 받는 파일의 용량을 크게 다운 받으므로 메모리 부족이 발생할 수 있다.
        //        //    //var cotent = await response.Content.ReadAsByteArrayAsync();
        //        //    //File.WriteAllBytes(fi.FullName, cotent);


        //        //    var ms = await response.Content.ReadAsStreamAsync();

        //        //    // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2023.10.25 minjae)
        //        //    //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
        //        //    //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
        //        //    // 참고 URL - https://hongjinhyeon.tistory.com/92
        //        //    using (var fs = File.Create(fi.FullName))
        //        //    {
        //        //        ms.Seek(0, SeekOrigin.Begin);
        //        //        await ms.CopyToAsync(fs);
        //        //        await ms.FlushAsync();
        //        //        await fs.FlushAsync();

        //        //        // fs.Close();
        //        //    }
        //        //}

        //        //// 이미지 파일(fi)이 존재하는 경우 
        //        //// 다운로드 받을 파일명이 중복일 때, 기존 파일명 + N 으로 생성하도록 구현 (2023.10.25 minjae)
        //        //// 참고 URL - https://pseudo-code.tistory.com/166
        //        //else if (true == fi.Exists 
        //        //         && (fi.FullName.Equals(pFilePack.fileName))
        //        //         && pFilePack.fileName.Length > 0)
        //        //{
        //        //    var ms = await response.Content.ReadAsStreamAsync();

        //        //    using ()
        //        //    {

        //        //    }
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //    // return;
        //}

        // TODO : 8. 연관 컨테이너 목록 조회 (Get) 비동기 메서드 "GetLinkedContainerListAsync" 필요시 참고 (2024.10.29 minjae)
        /// <summary>
        /// 8. 연관 컨테이너 목록 조회 (Get) - 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너)
        /// Rest API 메서드 파라미터 "HttpClient pClient, string projectId, string teamId, string containerId, string authorization, string pInputTenantId"
        /// </summary>
        //public static async Task<List<Dictionary<string, object>>> GetLinkedContainerListAsync(HttpClient pClient, ContainerPack pContainerPack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        // TODO : 로그인 JWT 토큰 값 들어있는 string 클래스 객체 "authorization"를 bearer 토큰으로 변환하기 위해 문자열 "bearer " 추가해서
        //        // HttpClient 요청 헤더 pClient.DefaultRequestHeaders에 추가(TryAddWithoutValidation) (2023.10.30 minjae)
        //        // 추후 인재 INC와 소통 진행 -> JWT 토큰 또는 bearer 토큰 둘 중에 하나로 통일 예정 
        //        // 최종 결과 - pClient.DefaultRequestHeaders.Clear(); 사용하면 bearer 토큰 쓸 필요 없이 JWT 토큰만으로 사용 가능
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);

        //        // TryAddWithoutValidation 메서드 사용 - 요청 헤더 (메서드 파라미터 "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 8. 연관 컨테이너 목록 조회 URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestLinkedContainerURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestLinkedNodeInfoURL = requestSb.ToString();

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestLinkedNodeInfoURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 8. 연관 컨테이너 목록 조회 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData] as JsonArray).Select(x => x);


        //            // TODO : 연관 컨테이너 리스트에 속한 컨테이너 데이터 Dictionary 객체의 Key "containerId"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2023.12.15 minjae)
        //            // 참고 URL   - https://codechacha.com/ko/csharp-sort-list/
        //            // 참고 2 URL - https://developer-talk.tistory.com/694
        //            // 참고 3 URL - https://developer-talk.tistory.com/210
        //            var linkedNodeInfoList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => x[ContainerHelper.containerId].ToString()).ToList();

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "8. 연관 컨테이너 목록 조회 성공");

        //            return linkedNodeInfoList;
        //        }

        //        // Rest API 8. 연관 컨테이너 목록 조회 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);

        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw 
        //    }
        //}


        // TODO : 8. 연관 컨테이너 목록 조회 (Get) - 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너)
        //        비동기 메서드 "GetLinkedContainerListAsync" 필요시 참고 (2024.10.08 minjae)
        // TODO : 아래 Rest API 메서드 "GetLinkedContainerListAsync" -> ExplorerVM.cs 비동기 메서드 "CheckOutAsync" 안에서 사용 (2023.11.28 minjae)
        /// <summary>
        /// 8. 연관 컨테이너 목록 조회 (Get) - 연관 컨테이너 정보(참조관계 - 부모/자식 컨테이너)
        /// Rest API 메서드 파라미터 "HttpClient pClient, string projectId, string teamId, string containerId, string authorization, string pInputTenantId"
        /// </summary>
        //public static async Task<List<Dictionary<string, object>>> GetLinkedContainerListAsync(HttpClient pClient, ContainerPack pContainerPack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        // TODO : 로그인 JWT 토큰 값 들어있는 string 클래스 객체 "authorization"를 bearer 토큰으로 변환하기 위해 문자열 "bearer " 추가해서
        //        // HttpClient 요청 헤더 pClient.DefaultRequestHeaders에 추가(TryAddWithoutValidation) (2023.10.30 minjae)
        //        // 추후 인재 INC와 소통 진행 -> JWT 토큰 또는 bearer 토큰 둘 중에 하나로 통일 예정 
        //        // 최종 결과 - pClient.DefaultRequestHeaders.Clear(); 사용하면 bearer 토큰 쓸 필요 없이 JWT 토큰만으로 사용 가능
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);

        //        // TryAddWithoutValidation 메서드 사용 - 요청 헤더 (메서드 파라미터 "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);

        //        // TODO : 서버에 Http 통신(Get) 진행시 아래 URL 주소(ContainerHelper.requestLinkedNodeInfoURL)에
        //        // 프로젝트 아이디, 팀 아이디, 컨테이너 아이디를 "pContainerPack" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 컨테이너 상세 조회 목록 JSON 데이터 가져오기 (2023.10.30 minjae)

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 8. 연관 컨테이너 목록 조회 URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestLinkedContainerURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestLinkedNodeInfoURL = requestSb.ToString();

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestLinkedNodeInfoURL);


        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 8. 연관 컨테이너 목록 조회 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData] as JsonArray).Select(x => x);


        //            // TODO : 연관 컨테이너 리스트에 속한 컨테이너 데이터 Dictionary 객체의 Key "containerId"의 값(Value)을 기준으로 오름차순(OrderBy) 정렬하여 리스트 구현 (2023.12.15 minjae)
        //            // 참고 URL   - https://codechacha.com/ko/csharp-sort-list/
        //            // 참고 2 URL - https://developer-talk.tistory.com/694
        //            // 참고 3 URL - https://developer-talk.tistory.com/210
        //            // var folderList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => Int32.Parse(x[FolderHelper.levelNo].ToString())).ToList();

        //            // var linkedNodeInfoList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();
        //            var linkedNodeInfoList = jtmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).OrderBy(x => x[ContainerHelper.containerId].ToString()).ToList();

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "8. 연관 컨테이너 목록 조회 성공");

        //            return linkedNodeInfoList;
        //        }
        //        // Rest API 8. 연관 컨테이너 목록 조회 Http 통신 결과가 실패할 경우 
        //        else
        //            throw new Exception(resMsg);


        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   //  오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        // TODO : 9. 컨테이너 상세 조회 (Get) 비동기 메서드 "GetDetailListAsync" 필요시 참고 (2024.10.30 minjae)
        /// <summary>
        /// 9. 컨테이너 상세 조회 (Get) - 컨테이너 정보, 파일정보 + 메타데이터 정보 + 마일스톤 정보
        /// Rest API 메서드 파라미터 "HttpClient pClient, string projectId, string teamId, string containerId, string authorization, string pInputTenantId"
        /// </summary>
        //public static async Task<List<Dictionary<string, List<Dictionary<string, object>>>>> GetDetailListAsync(HttpClient pClient, ContainerPack pContainerPack, RequestHeader pRequestHeaders)
        //{
        //    // TODO : 로그 기록시 현재 실행 중인 메서드 위치 기록하기 (2023.10.10 minjae)
        //    var currentMethod = MethodBase.GetCurrentMethod();

        //    // 컨테이너 정보, 파일정보 + 메타데이터 정보 + 마일스톤 정보를 같이 담을 2차원 리스트 객체 "total_Info_List" 생성
        //    List<Dictionary<string, List<Dictionary<string, object>>>> total_Info_List = new List<Dictionary<string, List<Dictionary<string, object>>>>();

        //    // 연관된 부모 컨테이너 정보 리스트
        //    List<Dictionary<string, object>> linkedNodeInfoList = new List<Dictionary<string, object>>();

        //    // 연관된 부모 컨테이너 정보 Dictionary 객체 "linkedInfoDic" 생성
        //    var linkedNodeInfoDic = new Dictionary<string, List<Dictionary<string, object>>>();

        //    // 컨테이너 정보, 파일정보 리스트 객체 "containerDetailList" 생성
        //    List<Dictionary<string, object>> containerDetailList = new List<Dictionary<string, object>>();

        //    // 컨테이너 정보, 파일정보 Dictionary 객체 "containerDetailDic" 생성
        //    var containerDetailDic = new Dictionary<string, List<Dictionary<string, object>>>();


        //    // 메타데이터 정보 리스트 객체 "metaDataList" 생성
        //    List<Dictionary<string, object>> metaDataList = new List<Dictionary<string, object>>();

        //    // 메타데이터 정보 Dictionary 객체 "metaDataDic" 생성
        //    var metaDataDic = new Dictionary<string, List<Dictionary<string, object>>>();


        //    // 마일스톤 업무 리스트 객체 "taskDetailList" 생성
        //    List<Dictionary<string, object>> taskDetailList = new List<Dictionary<string, object>>();

        //    // 마일스톤 업무 Dictionary 객체 "taskDetailDic" 생성
        //    var taskDetailDic = new Dictionary<string, List<Dictionary<string, object>>>();

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        // TODO : 로그인 JWT 토큰 값 들어있는 string 클래스 객체 "authorization"를 bearer 토큰으로 변환하기 위해 문자열 "bearer " 추가해서
        //        // HttpClient 요청 헤더 pClient.DefaultRequestHeaders에 추가(TryAddWithoutValidation) (2023.10.12 minjae)
        //        // 추후 인재 INC와 소통 진행 -> JWT 토큰 또는 bearer 토큰 둘 중에 하나로 통일 예정 
        //        // 최종 결과 - pClient.DefaultRequestHeaders.Clear(); 사용하면 bearer 토큰 쓸 필요 없이 JWT 토큰만으로 사용 가능
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        // pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, authorization);

        //        // TryAddWithoutValidation 메서드 사용 - 요청 헤더 (메서드 파라미터 "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        // pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pInputTenantId);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);


        //        // Add 메서드 사용 - 요청 헤더 (메서드 파라미터 "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가
        //        // 참고 URL - https://vmpo.tistory.com/91
        //        // pClient.DefaultRequestHeaders.Add(ContainerHelper.tenant, pInputTenantId);


        //        // TODO : 서버에 Http 통신(Get) 진행시 아래 URL 주소(ContainerHelper.requestContainerURL)에
        //        // 프로젝트 아이디, 팀 아이디, 컨테이너 아이디를 "pContainerPack" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 컨테이너 상세 조회 목록 JSON 데이터 가져오기 (2023.09.06 minjae)

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 9. 컨테이너 상세 조회 Rest API URL 주소에 문자열 추가 (2023.11.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestDetailURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestDetailURL = requestSb.ToString();

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestDetailURL);

        //        // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
        //        //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2023.10.12 minjae)
        //        //        참고 URL - https://cano721.tistory.com/216
        //        //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
        //        //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 9. 컨테이너 상세 조회 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            // 연관된 부모 컨테이너 정보 리스트
        //            var jlinkedNodeInfo = (JsonObject.Parse(resMsg)[ContainerHelper.linkedNodeInfo] as JsonArray);
        //            // var jlinkedNodeInfoTmp = (JsonObject.Parse(resMsg)[ContainerHelper.linkedNodeInfo] as JsonArray).Select(x => x);
        //            // TODO : 삼항 연산자 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.12.11 minjae)
        //            // 참고 URL - https://husk321.tistory.com/405
        //            // var jlinkedNodeInfoTmp = jlinkedNodeInfo is null ? null : jlinkedNodeInfo.Select(x => x);

        //            // TODO : is null 연산자 대신 null 체크 연산자(?) 사용 (2023.12.12 minjae)
        //            // 참고 URL - https://m.cafe.daum.net/aspdotnet/6TQG/3149
        //            // 참고 2 URL - https://hwigyeom.ntils.com/3
        //            // 참고 3 URL - https://cafe.daum.net/aspdotnet/WJSu/65
        //            var jlinkedNodeInfoTmp = jlinkedNodeInfo?.Select(x => x);

        //            // TODO : jdetailTmp null Exception이 발생하는 원인 파악하기 (2023.10.12 minjae)
        //            // 컨테이너 정보, 파일정보
        //            var jdetailNode = (JsonObject.Parse(resMsg)[ContainerHelper.detail]);
        //            var jdetailTmp = JsonSerializer.Deserialize<Dictionary<string, object>>(jdetailNode);

        //            // 메타데이터 정보
        //            var jmetaDataTmp = (JsonObject.Parse(resMsg)[ContainerHelper.metaDataList] as JsonArray).Select(x => x);

        //            // 마일스톤 업무
        //            var jtaskDetailNode = (JsonObject.Parse(resMsg)[ContainerHelper.taskDetail]);
        //            var jtaskDetailTmp = JsonSerializer.Deserialize<Dictionary<string, object>>(jtaskDetailNode);

        //            // TODO : containerDetailList null Exception이 발생하는 원인 파악하기 (2023.10.12 minjae)
        //            containerDetailList.Add(jdetailTmp);

        //            // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 minjae)
        //            // 참고 URL - https://husk321.tistory.com/405
        //            // 연관 컨테이너 정보가 존재하는 경우에만 jlinkedNodeInfoTmp.Select 사용해서 "linkedNodeInfoList"에 데이터 할당
        //            //if (jlinkedNodeInfoTmp is not null) linkedNodeInfoList.Add(jlinkedNodeInfoTmp);
        //            if (jlinkedNodeInfoTmp is not null) linkedNodeInfoList = jlinkedNodeInfoTmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();
        //            metaDataList = jmetaDataTmp.Select(x => JsonSerializer.Deserialize<Dictionary<string, object>>(x)).ToList();
        //            taskDetailList.Add(jtaskDetailTmp);

        //            linkedNodeInfoDic.Add(ContainerHelper.linkedNodeInfo, linkedNodeInfoList);
        //            containerDetailDic.Add(ContainerHelper.detail, containerDetailList);
        //            metaDataDic.Add(ContainerHelper.metaDataList, metaDataList);
        //            taskDetailDic.Add(ContainerHelper.taskDetail, taskDetailList);

        //            total_Info_List.Add(linkedNodeInfoDic);       // 연관된 부모 컨테이너 정보 리스트에 추가
        //            total_Info_List.Add(containerDetailDic);      // 컨테이너 정보, 파일정보 total_Info_List에 추가
        //            total_Info_List.Add(metaDataDic);             // 메타데이터 정보 total_Info_List에 추가
        //            total_Info_List.Add(taskDetailDic);           // 마일스톤 정보 total_Info_List에 추가

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "9. 컨테이너 상세 조회 성공");

        //            return total_Info_List;
        //        }
        //        // Rest API 9. 컨테이너 상세 조회 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);

        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.09.20 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 10. 컨테이너 체크아웃 + (파일)다운로드 (Get) 비동기 메서드 "GetCheckOutAsync" 필요시 참고 (2024.10.08 minjae)
        /// <summary>
        /// 10. 컨테이너 체크아웃 + (파일)다운로드 (Get)
        /// </summary>
        //public static async Task GetCheckOutAsync(HttpClient pClient, ContainerPack pContainerPack, FilePack pFilePack, RequestHeader pRequestHeaders)
        //{
        //    // string createUserId  = string.Empty;     // 컨테이너 생성자
        //    // string workUserId    = string.Empty;     // 컨테이너 작업중인 사용자 아이디
        //    // string readYn        = string.Empty;     // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
        //    // string statusCode    = string.Empty;     // 컨테이너 검토상태 ("comstt10": 미진행, "comstt20": 진행, "comstt30": 완료)

        //    string downloadFilePath = string.Empty;     // 다운로드 받으려고 하는 최종 파일 경로 (파일 전체 경로 + 이름 중복일 때 변경된 파일 이름)

        //    // TODO : 다른 PC에서도 파일 다운로드 기능이 실행될 수 있도록 다운로드 받을 파일 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.11.22 minjae)
        //    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //    string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetEntryAssembly().GetName().Name}\\{pContainerPack.projectId}\\{pContainerPack.teamId}\\{pContainerPack.containerId}";
        //    string filePath = dirPath + $"\\{pFilePack.fileName}";
        //    DirectoryInfo di = new DirectoryInfo(dirPath);
        //    FileInfo fi = new FileInfo(filePath);

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.11.30 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 10. 컨테이너 체크아웃 + (파일)다운로드 Rest API URL 주소에 문자열 추가 (2023.11.29 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 10. 컨테이너 체크아웃 + (파일)다운로드 Rest API 테스트 진행 (2023.11.29 minjae)
        //        // 테스트용 URL 주소 - requestCheckOutURL = "https://cobim.io/api/explorer/container/checkout/jc-constrct/inc-001-jc-constrct-t001/jc-constrct-ijinc-sptl_902-func_010-006";
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCheckOutURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestCheckOutURL = requestSb.ToString();

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestCheckOutURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 10. 컨테이너 체크아웃 + (파일)다운로드 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var ms = await response.Content.ReadAsStreamAsync();

        //            // 디렉터리(di)가 존재하지 않는 경우
        //            if (false == di.Exists) Directory.CreateDirectory(dirPath);   // 디렉터리 새로 생성

        //            // 새로 다운로드 받으려는 파일 전체 경로 - dirPath + 파일이름 - fi.Name 구하기
        //            downloadFilePath = FileHelper.FileDownLoadPath(dirPath, pContainerPack, fi.Name);

        //            // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2023.10.25 minjae)
        //            //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
        //            //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
        //            // 참고 URL - https://hongjinhyeon.tistory.com/92
        //            // TODO : HttpClient 클래스 객체 pClient 사용 URL을 이용한 파일 다운로드 구현 (2023.10.26 minjae)
        //            // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        //            // 참고 2 URL - https://stackoverflow.com/questions/45711428/download-file-with-webclient-or-httpclient

        //            using (FileStream fs = File.Create(downloadFilePath))
        //            {
        //                ms.Seek(0, SeekOrigin.Begin);
        //                await ms.CopyToAsync(fs);
        //                await ms.FlushAsync();
        //                await fs.FlushAsync();

        //                // fs.Close();
        //            }

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "10. 컨테이너 체크아웃 + (파일)다운로드 성공");

        //            return;
        //        }

        //        // Rest API 10. 컨테이너 체크아웃 + (파일)다운로드 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);

        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.11.30 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        /// <summary>
        /// 10. 컨테이너 체크아웃 + (파일)다운로드 (Get)
        /// </summary>
        // public static async Task<bool> GetCheckOutAsync(HttpClient pClient, ContainerPack pContainerPack, FilePack pFilePack, List<string> pContainerIdList, List<ContainerView> pFileList, string authorization, string pInputTenantId)
        // public static async Task GetCheckOutAsync(HttpClient pClient, ContainerPack pContainerPack, string pContainerId, string authorization, string pInputTenantId)
        //public static async Task GetCheckOutAsync(HttpClient pClient, ContainerPack pContainerPack, FilePack pFilePack, RequestHeader pRequestHeaders)
        //{
        //    // string createUserId     = string.Empty;     // 컨테이너 생성자
        //    // string workUserId       = string.Empty;     // 컨테이너 작업중인 사용자 아이디
        //    // string readYn           = string.Empty;     // 컨테이너 잠금 상태 ( "Y": 잠금 ON, "N": 잠금 OFF)
        //    // string statusCode       = string.Empty;     // 컨테이너 검토상태 ("comstt10": 미진행, "comstt20": 진행, "comstt30": 완료)

        //    string downloadFilePath = string.Empty;     // 다운로드 받으려고 하는 최종 파일 경로 (파일 전체 경로 + 이름 중복일 때 변경된 파일 이름)

        //    // TODO : 다른 PC에서도 파일 다운로드 기능이 실행될 수 있도록 다운로드 받을 파일 경로 "dirPath"의 루트 디렉토리 값을 내문서 폴더 (Environment.SpecialFolder.MyDocuments)로 설정 (2023.11.22 minjae)
        //    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.environment.specialfolder?view=net-7.0
        //    string dirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\COBIM\\{Assembly.GetEntryAssembly().GetName().Name}\\{pContainerPack.projectId}\\{pContainerPack.teamId}\\{pContainerPack.containerId}";

        //    string filePath = dirPath + $"\\{pFilePack.fileName}";

        //    DirectoryInfo di = new DirectoryInfo(dirPath);

        //    FileInfo fi = new FileInfo(filePath);

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 테스트 코드 - 강제로 오류 발생하도록 Exception 생성 코드 필요시 사용 예정 (2023.11.27 minjae)
        //        // 참고 URL - https://morm.tistory.com/187
        //        // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
        //        // throw new Exception("테스트");

        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.11.30 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 10. 컨테이너 체크아웃 + (파일)다운로드 Rest API URL 주소에 문자열 추가 (2023.11.29 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(Get) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀아이디를 "inc-001-jc-constrct-t001" 메서드 파라미터로 넘겨 받아서 
        //        // Http Get 방식으로 호출해서 10. 컨테이너 체크아웃 + (파일)다운로드 Rest API 테스트 진행 (2023.11.29 minjae)
        //        // 테스트용 URL 주소 - requestCheckOutURL = "https://cobim.io/api/explorer/container/checkout/jc-constrct/inc-001-jc-constrct-t001/jc-constrct-ijinc-sptl_902-func_010-006";
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCheckOutURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestCheckOutURL = requestSb.ToString();

        //        Debug.WriteLine($"{requestCheckOutURL}");

        //        // TODO : HttpClient Get 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using HttpResponseMessage response = await pClient.GetAsync(requestCheckOutURL);

        //        // Rest API 10. 컨테이너 체크아웃 + (파일)다운로드 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            var ms = await response.Content.ReadAsStreamAsync();

        //            // 디렉터리(di)가 존재하지 않는 경우
        //            if (false == di.Exists) Directory.CreateDirectory(dirPath);   // 디렉터리 새로 생성

        //            // 새로 다운로드 받으려는 파일 전체 경로 - dirPath + 파일이름 - fi.Name 구하기
        //            downloadFilePath = FileHelper.FileDownLoadPath(dirPath, pContainerPack, fi.Name);

        //            // TODO : 해당 using 문장을 벗어나면 FileStream 클래스 객체 fs Dipose 처리 (리소스 해제 및 자원 반납) 구현 (2023.10.25 minjae)
        //            //        아래 소스코드를 using문으로 구현 안하고 실행시 파일을 생성되나 FileStream 클래스 객체 fs가 close 처리가 자동으로 되지 않아서 
        //            //        다운로드 받은 이미지 파일(.jpg, .png 등등...)을 열어도 손상된 상태라고 하며 이미지를 볼 수 없으니 주의해야 한다.
        //            // 참고 URL - https://hongjinhyeon.tistory.com/92
        //            // TODO : HttpClient 클래스 객체 pClient 사용 URL을 이용한 파일 다운로드 구현 (2023.10.26 minjae)
        //            // 참고 URL - https://asecurity.dev/entry/NET-Download-file-from-HttpClientWebClient
        //            // 참고 2 URL - https://stackoverflow.com/questions/45711428/download-file-with-webclient-or-httpclient

        //            using (FileStream fs = File.Create(downloadFilePath))
        //            {
        //                ms.Seek(0, SeekOrigin.Begin);
        //                await ms.CopyToAsync(fs);
        //                await ms.FlushAsync();
        //                await fs.FlushAsync();

        //                // fs.Close();
        //            }

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "10. 컨테이너 체크아웃 + (파일)다운로드 성공");

        //            // TODO : 아래 주석 친 테스트 코드 필요시 사용 예정 (2023.12.13 minjae)
        //            //File.WriteAllBytes(downloadFilePath, await response.Content.ReadAsByteArrayAsync());

        //            return;
        //        }
        //        // Rest API 10. 컨테이너 체크아웃 + (파일)다운로드 Http 통신 결과가 실패할 경우 
        //        else
        //        {
        //            string resMsg = await response.Content.ReadAsStringAsync();
        //            throw new Exception(resMsg);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2023.11.30 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        // TODO : 11. 컨테이너 체크인 + 업로드 (POST) 비동기 메서드 "PostCheckInAsync" 필요시 참고 (2024.10.11 minjae)
        /// <summary>
        /// 11. 컨테이너 체크인 + 업로드 (POST)
        /// </summary>
        //public static async Task<Dictionary<string, object>> PostCheckInAsync(HttpClient pClient, ContainerPack pContainerPack, CheckInPack pCheckInPack, RequestHeader pRequestHeaders)
        ////public static async Task<Dictionary<string, object>> PostCheckInAsync(HttpClient pClient, ContainerPack pContainerPack, CheckInPack pCheckInPack, string pFilePath, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 ("로그인 토큰 값 - Authorization", "테넌트 아이디 - Tenant") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent" 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient

        //        // TODO : 자바 스프링(Java Spring) 오류 메시지 "org.springframework.validation.BindException" 출력 원인 파악 (2023.12.06 minjae)
        //        // 참고 URL - https://jy-note.tistory.com/7

        //        // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://brokul.dev/sending-files-and-additional-data-using-httpclient-in-net-core
        //        var strCnt = new StreamContent(pCheckInPack.uploadFiles);
        //        var fCnt = new ByteArrayContent(strCnt.ReadAsByteArrayAsync().Result);
        //        fCnt.Headers.ContentType = MediaTypeHeaderValue.Parse(ContainerHelper.formDataContentType);

        //        // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
        //        using var formContent = new MultipartFormDataContent
        //        { 
        //            // 요청 데이터 Body(form-data) 문자열 
        //            { new StringContent(pCheckInPack.folderId, Encoding.UTF8), ContainerHelper.folderId },
        //            { new StringContent(pCheckInPack.fileName, Encoding.UTF8), ContainerHelper.fileName },
        //            { new StringContent(pCheckInPack.fileSize, Encoding.UTF8), ContainerHelper.fileSize },
        //            { new StringContent(pCheckInPack.comment,  Encoding.UTF8), ContainerHelper.comment  },
        //            // 요청 데이터 Body(form-data) 이미지 파일 (uploadFiles)
        //            { fCnt, ContainerHelper.uploadFiles, pCheckInPack.fileName }
        //        };

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 11. 컨테이너 체크인 + 업로드 (POST) Rest API URL 주소에 문자열 추가 (2023.12.01 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(POST) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀 아이디 "inc-001-jc-constrct-t001"
        //        //        컨테이너 아이디 "jc-constrct-ijinc-sptl_902-func_010-006" 메서드 파라미터로 넘겨 받아서 
        //        //        Http POST 방식으로 호출해서 11. 컨테이너 체크인 + 업로드 (POST) Rest API 테스트 진행 (2023.12.01 minjae)
        //        // 테스트용 URL 주소 - requestCheckOutURL = "https://cobim.io/api/explorer/container/checkin/jc-constrct/inc-001-jc-constrct-t001/jc-constrct-ijinc-sptl_902-func_010-006";
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCheckInURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestCheckInURL = requestSb.ToString();

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent"구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient
        //        using HttpResponseMessage response = await pClient.PostAsync(requestCheckInURL, formContent);

        //        // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
        //        //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2023.10.12 minjae)
        //        //        참고 URL - https://cano721.tistory.com/216
        //        //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
        //        //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // Rest API 11. 컨테이너 체크인 + 업로드 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            // TODO : 서버로 부터 받은 json 데이터에 속한 "resultData" 영역이 리스트가 아니기 때문에 아래처럼 (JsonObject.Parse(resMsg) 사용 -> JsonNode(JsonObject 배열) 형식으로 변환 후 JSON 문자열 변환(ToJsonString()) (2023.12.07 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/76035924/c-sharp-list-field-names-in-a-jsonobject-or-jsonnode
        //            // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode.tojsonstring?view=net-8.0
        //            var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).ToJsonString();

        //            // TODO : JsonString 객체 jtmp를 Dictionary<string, object> 클래스 객체 "resultData"로 변환할 수 있도록 JsonSerializer.Deserialize 진행 (2023.12.07 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/70111672/read-from-json-file-into-ienumerable-using-system-text-json
        //            var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(jtmp);

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "11. 컨테이너 체크인 + 업로드 성공");

        //            return resultData;
        //        }
        //        // Rest API 11. 컨테이너 체크인 + 업로드 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        /// <summary>
        /// 11. 컨테이너 체크인 + 업로드 (POST)
        /// </summary>
        //public static async Task<Dictionary<string, object>> PostCheckInAsync(HttpClient pClient, ContainerPack pContainerPack, CheckInPack pCheckInPack, string pFilePath, RequestHeader pRequestHeaders)
        //{
        //    // var resultDataList = new List<Dictionary<string, object>>();  // resultData를 담는 테스트용 리스트 객체 "resultDataList"

        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent" 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient
        //        //var formContent = new FormUrlEncodedContent(new[]
        //        //{
        //        //    new KeyValuePair<string, string>(ContainerHelper.folderId,    pCheckInPack.folderId),
        //        //    // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2023.12.06 minjae)
        //        //    // 참고 URL - https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
        //        //    new KeyValuePair<string, string>(ContainerHelper.uploadFiles, pCheckInPack.uploadFiles),
        //        //    new KeyValuePair<string, string>(ContainerHelper.fileName,    pCheckInPack.fileName),
        //        //    new KeyValuePair<string, string>(ContainerHelper.fileSize,    pCheckInPack.fileSize),
        //        //    new KeyValuePair<string, string>(ContainerHelper.comment,     pCheckInPack.comment)
        //        //});

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent" 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient

        //        // TODO : 자바 스프링(Java Spring) 오류 메시지 "org.springframework.validation.BindException" 출력 원인 파악 (2023.12.06 minjae)
        //        // 참고 URL - https://jy-note.tistory.com/7

        //        // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
        //        //using var formContent = new MultipartFormDataContent 
        //        //{
        //        //    // 요청 데이터 Body(form-data) 문자열 
        //        //    { new StringContent(pCheckInPack.folderId), ContainerHelper.folderId },
        //        //    { new StringContent(pCheckInPack.fileName), ContainerHelper.fileName },
        //        //    { new StringContent(pCheckInPack.fileSize), ContainerHelper.fileSize },
        //        //    { new StringContent(pCheckInPack.comment),  ContainerHelper.comment  },
        //        //    // 요청 데이터 Body(form-data) 이미지 파일 (uploadFiles)
        //        //    { new StreamContent(new MemoryStream(pUploadFileStream)), ContainerHelper.uploadFiles, pCheckInPack.uploadFiles },
        //        //};

        //        // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://brokul.dev/sending-files-and-additional-data-using-httpclient-in-net-core
        //        // using var upLoadFileStream = File.OpenRead(pFilePath);
        //        var strCnt = new StreamContent(pCheckInPack.uploadFiles);
        //        var fCnt = new ByteArrayContent(strCnt.ReadAsByteArrayAsync().Result);
        //        fCnt.Headers.ContentType = MediaTypeHeaderValue.Parse(ContainerHelper.formDataContentType);

        //        // TODO : HttpClient Post 방식으로 요청 데이터 (form-data) 중 파일 업로드 형식(upload file form) 구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
        //        using var formContent = new MultipartFormDataContent
        //        { 
        //            // 요청 데이터 Body(form-data) 문자열 
        //            { new StringContent(pCheckInPack.folderId, Encoding.UTF8), ContainerHelper.folderId },
        //            { new StringContent(pCheckInPack.fileName, Encoding.UTF8), ContainerHelper.fileName },
        //            { new StringContent(pCheckInPack.fileSize, Encoding.UTF8), ContainerHelper.fileSize },
        //            { new StringContent(pCheckInPack.comment,  Encoding.UTF8), ContainerHelper.comment  },
        //            // 요청 데이터 Body(form-data) 이미지 파일 (uploadFiles)
        //            { fCnt, ContainerHelper.uploadFiles, pCheckInPack.fileName }
        //        };


        //        //var formContent = new MultipartFormDataContent
        //        //{
        //        //    {new StringContent(pCheckInPack.folderId), ContainerHelper.folderId },
        //        //    {new StringContent(pCheckInPack.fileName), ContainerHelper.fileName },
        //        //    {new StringContent(pCheckInPack.comment),  ContainerHelper.comment  }
        //        //};

        //        //var fs = File.OpenRead(pFilePath);
        //        //var strCnt = new StreamContent(fs);

        //        //var fCnt = new ByteArrayContent(strCnt.ReadAsByteArrayAsync().Result);
        //        //fCnt.Headers.ContentType = MediaTypeHeaderValue.Parse(ContainerHelper.contentType);

        //        // formContent.Add(fCnt, ContainerHelper.uploadFiles, pCheckInPack.fileName);
        //        // formContent.Add(new StringContent(fs.Length.ToString()), ContainerHelper.fileSize);


        //        // TODO : FilePack 클래스 객체 "pFilePack" string 클래스 json 문자열(content) 구현 (2023.12.01 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
        //        // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"folderId\":\"719c1e362f0f42c0840dc848a96d2e9d\",\"uploadFiles\":\"hohoho.xlsx\",\"fileName\":\"hohoho.xlsx\",\"fileSize\":\"8799\"}";
        //        // string requestData = JsonSerializer.Serialize(pFilePack);
        //        // string requestData = JsonSerializer.Serialize(pCheckInPack);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        //using var jsonContent = new StringContent(
        //        //                        requestData,
        //        //                        Encoding.UTF8
        //        //                      );



        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 11. 컨테이너 체크인 + 업로드 (POST) Rest API URL 주소에 문자열 추가 (2023.12.01 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(POST) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀 아이디 "inc-001-jc-constrct-t001"
        //        //        컨테이너 아이디 "jc-constrct-ijinc-sptl_902-func_010-006" 메서드 파라미터로 넘겨 받아서 
        //        //        Http POST 방식으로 호출해서 11. 컨테이너 체크인 + 업로드 (POST) Rest API 테스트 진행 (2023.12.01 minjae)
        //        // 테스트용 URL 주소 - requestCheckOutURL = "https://cobim.io/api/explorer/container/checkin/jc-constrct/inc-001-jc-constrct-t001/jc-constrct-ijinc-sptl_902-func_010-006";
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCheckInURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.containerId);

        //        var requestCheckInURL = requestSb.ToString();

        //        Debug.WriteLine($"{requestCheckInURL}");

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        // using HttpResponseMessage response = await pClient.PostAsync(requestCheckInURL, jsonContent);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, 요청 데이터 (form-data) 형식(Dictionary 방식)으로 객체 "var formContent"구현 (2023.12.06 minjae)
        //        // 참고 URL - https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient
        //        using HttpResponseMessage response = await pClient.PostAsync(requestCheckInURL, formContent);

        //        // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
        //        //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2023.10.12 minjae)
        //        //        참고 URL - https://cano721.tistory.com/216
        //        //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
        //        //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        Console.WriteLine($"{resMsg}\n");

        //        // Rest API 11. 컨테이너 체크인 + 업로드 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            // var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).Select(x => x);
        //            // TODO : 서버로 부터 받은 json 데이터에 속한 "resultData" 영역이 리스트가 아니기 때문에 아래처럼 (JsonObject.Parse(resMsg) 사용 -> JsonNode(JsonObject 배열) 형식으로 변환 후 JSON 문자열 변환(ToJsonString())(2023.12.07 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/76035924/c-sharp-list-field-names-in-a-jsonobject-or-jsonnode
        //            // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode.tojsonstring?view=net-8.0
        //            var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).ToJsonString();

        //            // TODO : JsonString 객체 jtmp를 Dictionary<string, object> 클래스 객체 "resultData"로 변환할 수 있도록 JsonSerializer.Deserialize 진행 (2023.12.07 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/70111672/read-from-json-file-into-ienumerable-using-system-text-json
        //            var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(jtmp);

        //            // resultDataList.Add(resultData);

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "11. 컨테이너 체크인 + 업로드 성공");

        //            return resultData;
        //        }
        //        // Rest API 11. 컨테이너 체크인 + 업로드 Http 통신 결과가 실패할 경우 
        //        else
        //            throw new Exception(resMsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)
        //        // Log.Information(LogHelper.GetMethodPath(currentMethod) + ex.Message);
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.10.6 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        // TODO : 17. 컨테이너 작업 취소 (Post) 비동기 메서드 "PostCancelWorkAsync" 필요시 참고 (2024.10.11 minjae)
        /// <summary>
        /// 17. 컨테이너 작업 취소 (Post)
        /// </summary>
        //public static async Task<Dictionary<string, object>> PostCancelWorkAsync(HttpClient pClient, ContainerPack pContainerPack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);


        //        // TODO : ContainerPack 클래스 객체 "pContainerPack" string 클래스 json 문자열(content) 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
        //        // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc-001\"}";
        //        string requestData = JsonSerializer.Serialize(pContainerPack);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.06 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        // TODO : 메서드 "StringContent"에 메서드 파라미터 Encoding 클래스 객체 "Encoding.UTF8" 제거 (2023.12.20 minjae)
        //        // TODO : 오류 메시지 "{"status":"org.springframework.web.HttpMediaTypeNotSupportedException: Content type 'text/plain;charset=utf-8' not supported","message":"Content type 'text/plain;charset=utf-8' not supported","error":"org.springframework.web.HttpMediaTypeNotSupportedException"}" 원인 파악 하기 (2023.12.20 minjae)
        //        //        오류 메시지 원인은 Content type을 추가하지 않을 경우 자동 생성되는 Content type이 "text/plain"이라서 발생한 오류이다.
        //        //        하여 아래처럼  Content type을 "LoginHelper.contentType(== "application/json")" 변경해야 한다.
        //        // 참고 URL - https://velog.io/@ds257873/Content-type-textplaincharsetUTF-8-not-supported
        //        using var jsonContent = new StringContent(
        //                                        requestData,
        //                                        Encoding.UTF8,
        //                                        ContainerHelper.contentType
        //                                    );

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> 17. 컨테이너 작업 취소 (POST) Rest API URL 주소에 문자열 추가 (2023.12.20 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        // TODO : 서버에 Http 통신(POST) 테스트 진행시 아래 URL 주소에 테스트 프로젝트 아이디 "jc-constrct", 팀 아이디 "inc-001-jc-constrct-t001"
        //        //        메서드 파라미터로 넘겨 받아서 Http POST 방식으로 호출 및
        //        //        17. 컨테이너 작업 취소 (POST) Rest API 테스트 진행 (2023.12.20 minjae)
        //        // 테스트용 URL 주소 - requestCancelWorkURL = "https://cobim.io/api/explorer/container/work/cancel/jc-constrct/inc-001-jc-constrct-t001";
        //        StringBuilder requestSb = new StringBuilder(ContainerHelper.requestCancelWorkURL);
        //        requestSb.Append(pContainerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pContainerPack.teamId);

        //        var requestCancelWorkURL = requestSb.ToString();

        //        Debug.WriteLine($"{requestCancelWorkURL}");

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2023.12.20 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient   
        //        using HttpResponseMessage response = await pClient.PostAsync(requestCancelWorkURL, jsonContent);

        //        // TODO : 인재 INC에서 개발한 서버측에서 전달 받은 오류 메시지 "{"status":"io.jsonwebtoken.MalformedJwtException: JWT strings must contain exactly 2 period characters. Found: 10","message":"JWT strings must contain exactly 2 period characters. Found: 10","error":"io.jsonwebtoken.MalformedJwtException"}"
        //        //        해당 오류 메시지의 원인은 JWT 토큰을 파싱하는 과정에서 발생한 오류로 확인. 해당 오류 원인 파악 후 로직 수정 예정 (2023.12.20 minjae)
        //        //        참고 URL - https://cano721.tistory.com/216
        //        //        참고 2 URL - https://stackoverflow.com/questions/53949137/jwt-strings-must-contain-exactly-2-period-characters-found-0
        //        //        참고 3 URL - https://velog.io/@jiu_lee/Project-%EB%B9%84%EB%B0%80%EB%B2%88%ED%98%B8-%EC%B0%BE%EA%B8%B0-with-JWT
        //        // TODO : 오류 메시지 "{"status":"org.springframework.web.HttpMediaTypeNotSupportedException: Content type 'text/plain;charset=utf-8' not supported","message":"Content type 'text/plain;charset=utf-8' not supported","error":"org.springframework.web.HttpMediaTypeNotSupportedException"}" 원인 파악 후 해결 진행 예정 (2023.12.20 minjae)
        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        Console.WriteLine($"{resMsg}\n");

        //        // Rest API 17. 컨테이너 작업 취소 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            // var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).Select(x => x);
        //            // TODO : 서버로 부터 받은 resMsg 데이터에 속한 "resultData" 영역이 리스트가 아니기 때문에 아래처럼 (JsonObject.Parse(resMsg) 사용 -> JsonNode(JsonObject 배열) 형식으로 변환 후 JSON 문자열 변환(ToJsonString()) (2023.12.20 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/76035924/c-sharp-list-field-names-in-a-jsonobject-or-jsonnode
        //            // 참고 2 URL - https://learn.microsoft.com/ko-kr/dotnet/api/system.text.json.nodes.jsonnode.tojsonstring?view=net-8.0
        //            var jtmp = (JsonObject.Parse(resMsg)[ContainerHelper.resultData]).ToJsonString();

        //            // TODO : JsonString 객체 jtmp를 Dictionary<string, object> 클래스 객체 "resultData"로 변환할 수 있도록 JsonSerializer.Deserialize 진행 (2023.12.20 minjae)
        //            // 참고 URL - https://stackoverflow.com/questions/70111672/read-from-json-file-into-ienumerable-using-system-text-json
        //            var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(jtmp);

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "17. 컨테이너 작업 취소 성공");

        //            return resultData;
        //        }
        //        // Rest API 17. 컨테이너 작업 취소 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);

        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.20 minjae)
        //        // Log.Information(LogHelper.GetMethodPath(currentMethod) + ex.Message);
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        // TODO : 오류 발생시 상위 호출자 예외처리 전달 throw 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://devlog.jwgo.kr/2009/11/27/thrownthrowex/
        //        throw;
        //    }
        //}

        #endregion Sample
    }
}
