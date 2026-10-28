using Serilog;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Collections.Generic;

using CobimExplorer.Rest.Api.CobimBase.Explorer;

using CobimExplorer.Data.Models.CobimBase.Login;
using CobimExplorer.Data.Models.CobimBase.HubBox;
using CobimExplorer.Data.Models.CobimBase.Explorer;
using CobimUtil;

namespace CobimExplorer.Rest.Api.CobimBase.HubBox
{
    /// <summary>
    /// HubBox 파일 조회/변환 Rest API 클래스 
    /// </summary>
    public class HubBoxRestServer
    {
        #region GetWebViewerURLAsync

        /// <summary>
        /// GET - 2. HubBox 2. 파일 뷰어 요청 
        /// 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 출력할 때 해당 Dictionary 객체에 속한 Http URL 주소 문자열 사용 
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<Dictionary<string, object>> GetWebViewerURLAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                // HttpClient client 객체 가져오기 
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기  
            WebViewerPack webViewerPack = pInputParamData.GetWebViewerPack();   // 컨테이너에 속한 파일 웹뷰어로 출력시 필수 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록
        
            try
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.22 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : StringBuilder 클래스 객체 requestSb 생성 ->  웹뷰어 Http URL 주소 가져오기 요청 Rest API URL 주소에 문자열 추가 (2024.08.22 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
                StringBuilder requestSb = new StringBuilder(HubBoxHelper.requestWebViewerURL);
                requestSb.Append(webViewerPack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(webViewerPack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(webViewerPack.GetContainerId());
                requestSb.Append("/");
                requestSb.Append(webViewerPack.GetFileId());
        
                var requestWebViewerURL = requestSb.ToString();
        
                // TODO : HttpClient Get 방식으로 웹서버에 요청할 때, using 문 사용 (2024.08.22 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                // 웹서버가 내려간 상태(종료)인 경우 아래 HttpResponseMessage response에 값이 할당되지 않고 메서드 GetWebViewerURLAsync가 자동 종료됨.
                using HttpResponseMessage response = await client.GetAsync(requestWebViewerURL);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();
        
                // HubBox 웹뷰어 Http URL 주소 문자열로 가져오기 Http 통신 결과가 성공할 경우 
                if(HttpStatusCode.OK == response.StatusCode)
                {
                    // TODO : 필요시 30초 지날 때마다 파일 변환 상태 체크 비동기 메서드 "CheckTranslateStatusAsync" 사용 예정 (2024.08.30 minjae)
                    // _ = CheckTranslateStatusAsync(pClient, pWebViewerPack, pRequestHeaders);

                    // Http 통신 결과가 성공할 경우 아래에 웹서버로 부터 받은 결과값(json 데이터)을 Dictionary 변환
                    var webViewerJsonNode = JsonNode.Parse(resMsg);
                    var webViewerInfoDic = JsonSerializer.Deserialize<Dictionary<string, object>>(webViewerJsonNode);
        
                    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "HubBox 웹뷰어 정보 리스트 가져오기 성공");
        
                    return webViewerInfoDic;   // Dictionary 클래스 객체 webViewerInfoDic 리턴
                }
                // HubBox 웹뷰어 Http URL 주소 문자열로 가져오기 Http 통신 결과가 실패할 경우 
                else throw new Exception(resMsg);
            }
            catch(Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.22 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion GetWebViewerURLAsync

        #region CheckTranslateStatusAsync

        // TODO : 30초 지날 때마다 파일 변환 상태 체크 백그라운드 쓰레드 비동기 메서드 CheckTranslateStatusAsync 필요시 사용 예정 (2024.09.02 minjae)
        /// <summary>
        /// 30초 지날 때마다 파일 변환 상태 체크
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        private static async Task CheckTranslateStatusAsync(ExplorerView pInputParamData)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                // 1 단계 : 30초 대기
                await Task.Delay(TimeSpan.FromSeconds(30));

                // 2 단계 : 30초 직후 Rest API GET - 2. HubBox 웹뷰어 요청 및 정보 가져오기 재호출 처리 하여 파일 변환 상태 체크
                var resultWebViewerData = await GetWebViewerURLAsync(pInputParamData);

                // 파일 뷰어 요청 결과 데이터(resultWebViewerData) 값이 존재하고(resultWebViewerData.Count >= 0)
                
                if(resultWebViewerData.Count >= (int)EnumWebViewerUrn.EXISTENCE)
                {
                    HubBoxView.SetResponseData(new ResponseData(resultWebViewerData[HubBoxHelper.code].ToString(),
                                                                resultWebViewerData[HubBoxHelper.message].ToString(),
                                                                resultWebViewerData.ContainsKey(HubBoxHelper.data) ? resultWebViewerData[HubBoxHelper.data].ToString() : string.Empty));
                }
                else throw new Exception("파일 뷰어 요청 응답 데이터가 존재하지 않습니다.\r\n담당자에게 문의하세요.");
            }
            catch(Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.22 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion CheckTranslateStatusAsync

        #region PostTranslateAsync

        /// <summary>
        /// POST - 1. 컨테이너에 속한 특정 파일 변환 API 
        /// 인재 INC - Rest API 11. 컨테이너 체크인 + 파일 웹서버 업로드 (POST) 성공한 경우에만 해당 특정 파일 변환 API 호출 
        /// Rest API 메서드 파라미터 "ExplorerView pInputParamData"
        /// </summary>
        public static async Task<Dictionary<string, object>> PostTranslateAsync(ExplorerView pInputParamData)
        {
            HttpClient client = pInputParamData.GetHttpClient();                // HttpClient client 객체 가져오기 
            RequestHeader requestHeader = pInputParamData.GetRequestHeader();   // Http 서버 통신 요청 Header 값 가져오기  
            TranslatePack translatePack = pInputParamData.GetTranslatePack();   // HubBox 파일 변환 정보 값 가져오기 

            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록 

            try
            {
                Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 Rest API 시작");

                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.26 minjae)

                // HttpClient 클래스 객체 "client"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
                client.DefaultRequestHeaders.Clear();

                // 요청 헤더 (requestHeader) HttpClient 클래스 객체 "client"에 추가 
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.authorization, requestHeader.GetAuthorization());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, requestHeader.GetTenant());
                client.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.access, requestHeader.GetAccess());

                // TODO : TranslatePack 클래스 객체 "translatePack" string 클래스 json 문자열(content) 구현 (2024.10.11 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
                // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc-001\"}";
                string requestData = JsonSerializer.Serialize(translatePack);

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.08.26 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
                using var jsonContent = new StringContent(
                                                requestData,
                                                Encoding.UTF8
                                            );

                // TODO : StringBuilder 클래스 객체 requestSb 생성 -> POST - 3. 컨테이너에 속한 특정 파일 변환 Rest API URL 주소에 문자열 추가 (2024.08.26 minjae)
                // StringBuilder vs String 비교 
                // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

                StringBuilder requestSb = new StringBuilder(HubBoxHelper.requestTranslateURL);
                requestSb.Append(translatePack.GetProjectId());
                requestSb.Append("/");
                requestSb.Append(translatePack.GetTeamId());
                requestSb.Append("/");
                requestSb.Append(translatePack.GetContainerId());
                requestSb.Append("/");
                requestSb.Append(translatePack.GetFileId());

                var requestTranslateURL = requestSb.ToString();

                // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.08.26 minjae)
                // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient   
                using HttpResponseMessage response = await client.PostAsync(requestTranslateURL, jsonContent);

                // TODO : 필요시 토큰 만료된 경우 요청 헤더(requestHeader) 재발급 토큰(RefreshToken) 추가 및 HttpClient 클래스 객체 "client"에 추가 구현 예정 (2024.11.12 minjae)

                string resMsg = await response.Content.ReadAsStringAsync();

                // 웹서버로 부터 받은 결과값(json 데이터)을 Dictionary 변환
                var translateJsonNode = JsonNode.Parse(resMsg);
                var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(translateJsonNode);

                Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 Rest API 종료");

                return resultData;   // Dictionary 클래스 객체 resultData 리턴
            }
            catch (Exception ex)
            {
                // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.10.11 minjae)
                Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

        #endregion PostTranslateAsync

        #region Sample

        // TODO : GET - 2. HubBox 2. 파일 뷰어 요청 비동기 메서드 "GetWebViewerURLAsync" 필요시 참고 (2024.10.30 minjae)
        /// <summary>
        /// GET - 2. HubBox 2. 파일 뷰어 요청 
        /// 특정 컨테이너에 속한 파일 변환해서 웹뷰어로 출력할 때 해당 Dictionary 객체에 속한 Http URL 주소 문자열 사용 
        /// </summary>
        //public static async Task<Dictionary<string, object>> GetWebViewerURLAsync(HttpClient pClient, WebViewerPack pWebViewerPack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.22 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 ->  웹뷰어 Http URL 주소 가져오기 요청 Rest API URL 주소에 문자열 추가 (2024.08.22 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4
        //        StringBuilder requestSb = new StringBuilder(HubBoxHelper.requestWebViewerURL);
        //        requestSb.Append(pWebViewerPack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pWebViewerPack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pWebViewerPack.containerId);
        //        requestSb.Append("/");
        //        requestSb.Append(pWebViewerPack.fileId);

        //        var requestWebViewerURL = requestSb.ToString();

        //        // TODO : HttpClient Get 방식으로 웹서버에 요청할 때, using 문 사용 (2024.08.22 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        // 웹서버가 내려간 상태(종료)인 경우 아래 HttpResponseMessage response에 값이 할당되지 않고 메서드 GetWebViewerURLAsync가 자동 종료됨.
        //        using HttpResponseMessage response = await pClient.GetAsync(requestWebViewerURL);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // HubBox 웹뷰어 Http URL 주소 문자열로 가져오기 Http 통신 결과가 성공할 경우 
        //        if (HttpStatusCode.OK == response.StatusCode)
        //        {
        //            // TODO : 30초 지날 때마다 파일 변환 상태 체크 비동기 메서드 "CheckTranslateStatusAsync" 필요시 사용 예정 (2024.08.30 minjae)
        //            // _ = CheckTranslateStatusAsync(pClient, pWebViewerPack, pRequestHeaders);

        //            // Http 통신 결과가 성공할 경우 아래에 웹서버로 부터 받은 결과값(json 데이터)을 Dictionary 변환
        //            var webViewerJsonNode = JsonNode.Parse(resMsg);
        //            var webViewerInfoDic = JsonSerializer.Deserialize<Dictionary<string, object>>(webViewerJsonNode);

        //            Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "HubBox 웹뷰어 정보 리스트 가져오기 성공");

        //            return webViewerInfoDic;   // Dictionary 클래스 객체 webViewerInfoDic 리턴
        //        }
        //        // HubBox 웹뷰어 Http URL 주소 문자열로 가져오기 Http 통신 결과가 실패할 경우 
        //        else throw new Exception(resMsg);
        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.22 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        // TODO : 30초 지날 때마다 파일 변환 상태 체크 백그라운드 쓰레드 비동기 메서드 CheckTranslateStatusAsync 필요시 참고 (2024.10.30 minjae)
        // TODO : 30초 지날 때마다 파일 변환 상태 체크 백그라운드 쓰레드 비동기 메서드 CheckTranslateStatusAsync 필요시 사용 예정 (2024.09.02 minjae)
        /// <summary>
        /// 30초 지날 때마다 파일 변환 상태 체크
        /// Rest API 메서드 파라미터 "HttpClient pClient, WebViewerPack pWebViewerPack, RequestHeader pRequestHeaders"
        /// </summary>
        //private static async Task CheckTranslateStatusAsync(HttpClient pClient, WebViewerPack pWebViewerPack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //    try
        //    {
        //        // 1 단계 : 30초 대기
        //        await Task.Delay(TimeSpan.FromSeconds(30));

        //        // 2 단계 : 30초 직후 Rest API GET - 2. HubBox 웹뷰어 요청 및 정보 가져오기 재호출 처리 하여 파일 변환 상태 체크
        //        var resultWebViewerData = await GetWebViewerURLAsync(pClient, pWebViewerPack, pRequestHeaders);

        //        // 파일 뷰어 요청 결과 데이터(resultWebViewerData) 값이 존재하고(resultWebViewerData.Count >= 0)

        //        if (resultWebViewerData.Count >= (int)EnumWebViewerUrn.EXISTENCE)
        //        {
        //            //HubBoxView.ResponseDatas.Code = resultWebViewerData.ContainsKey(HubBoxHelper.code) ? resultWebViewerData[HubBoxHelper.code].ToString() : resultWebViewerData[HubBoxHelper.statusCode].ToString();
        //            HubBoxView.ResponseDatas.Code = resultWebViewerData[HubBoxHelper.code].ToString();
        //            HubBoxView.ResponseDatas.Message = resultWebViewerData[HubBoxHelper.message].ToString();
        //            HubBoxView.ResponseDatas.Data = resultWebViewerData.ContainsKey(HubBoxHelper.data) ? resultWebViewerData[HubBoxHelper.data].ToString() : string.Empty;
        //        }
        //        else throw new Exception("파일 뷰어 요청 응답 데이터가 존재하지 않습니다.\r\n담당자에게 문의하세요.");
        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : 2024년 10월 COBIM 시연회 종료후 Http 통신 Get 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 로그 기록 및 Rest API 메서드 종료 기능 구현 예정 (2024.08.22 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        // TODO : 필요시 아래 주석친 POST - 1. 컨테이너에 속한 특정 파일 변환 API 비동기 메서드 "PostTranslateAsync" 참고 (2024.10.11 minjae)
        /// <summary>
        /// POST - 1. 컨테이너에 속한 특정 파일 변환 API 
        /// 인재 INC - Rest API 11. 컨테이너 체크인 + 파일 웹서버 업로드 (POST) 성공한 경우에만 해당 특정 파일 변환 API 호출 
        /// </summary>
        //public static async Task<Dictionary<string, object>> PostTranslateAsync(HttpClient pClient, TranslatePack pTranslatePack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록 

        //    try
        //    {
        //        Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 Rest API 시작");

        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.08.26 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 ("로그인 토큰 값 - Authorization", "테넌트 아이디 - Tenant") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);


        //        // TODO : TranslatePack 클래스 객체 "pTranslatePack" string 클래스 json 문자열(content) 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
        //        // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc-001\"}";
        //        string requestData = JsonSerializer.Serialize(pTranslatePack);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.08.26 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using var jsonContent = new StringContent(
        //                                        requestData,
        //                                        Encoding.UTF8
        //                                    );

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> POST - 3. 컨테이너에 속한 특정 파일 변환 Rest API URL 주소에 문자열 추가 (2024.08.26 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        StringBuilder requestSb = new StringBuilder(HubBoxHelper.requestTranslateURL);
        //        requestSb.Append(pTranslatePack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pTranslatePack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pTranslatePack.containerId);
        //        requestSb.Append("/");
        //        requestSb.Append(pTranslatePack.fileId);

        //        var requestTranslateURL = requestSb.ToString();

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.08.26 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient   
        //        using HttpResponseMessage response = await pClient.PostAsync(requestTranslateURL, jsonContent);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        // 웹서버로 부터 받은 결과값(json 데이터)을 Dictionary 변환
        //        var translateJsonNode = JsonNode.Parse(resMsg);
        //        var resultData = JsonSerializer.Deserialize<Dictionary<string, object>>(translateJsonNode);

        //        Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 Rest API 종료");

        //        return resultData;   // Dictionary 클래스 객체 resultData 리턴
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        /// <summary>
        /// POST - 1. 컨테이너에 속한 특정 파일 변환 API 
        /// 인재 INC - Rest API 11. 컨테이너 체크인 + 업로드 (POST) 성공한 경우에만 해당 특정 파일 변환 API 호출 
        /// </summary>
        //public static async Task<Dictionary<string, object>> PostTranslateAsync(HttpClient pClient, TranslatePack pTranslatePack, RequestHeader pRequestHeaders)
        //{
        //    var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록 

        //    try
        //    {
        //        Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 Rest API 시작");

        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2024.08.26 minjae)

        //        // HttpClient 클래스 객체 "pClient"의 요청헤더(DefaultRequestHeaders)에 데이터 추가 하기 전 초기화
        //        pClient.DefaultRequestHeaders.Clear();

        //        // 요청 헤더 (메서드 파라미터 "로그인 토큰 값 - authorization", "테넌트 아이디 - pInputTenantId") 메서드 파라미터 HttpClient 클래스 객체 "pClient"에 추가 
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(LoginHelper.authorization, pRequestHeaders.Authorization);
        //        pClient.DefaultRequestHeaders.TryAddWithoutValidation(ContainerHelper.tenant, pRequestHeaders.Tenant);


        //        // TODO : TranslatePack 클래스 객체 "pTranslatePack" string 클래스 json 문자열(content) 구현 (2023.12.20 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-8-0
        //        // json 문자열(하드코딩) 생성 코드 예시 -> (예) string requestData = "{\"id\":\"\",\"password\":\"\",\"inputTenantId\":\"inc-001\"}";
        //        string requestData = JsonSerializer.Serialize(pTranslatePack);

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.08.26 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient
        //        using var jsonContent = new StringContent(
        //                                        requestData,
        //                                        Encoding.UTF8
        //                                    );

        //        // TODO : StringBuilder 클래스 객체 requestSb 생성 -> POST - 3. 컨테이너에 속한 특정 파일 변환 Rest API URL 주소에 문자열 추가 (2024.08.26 minjae)
        //        // StringBuilder vs String 비교 
        //        // 참고 URL - https://afsdzvcx123.tistory.com/entry/C-%ED%99%9C%EC%9A%A9-String-StringBuilder-%EC%84%B1%EB%8A%A5-%EC%B0%A8%EC%9D%B4

        //        StringBuilder requestSb = new StringBuilder(HubBoxHelper.requestTranslateURL);
        //        requestSb.Append(pTranslatePack.projectId);
        //        requestSb.Append("/");
        //        requestSb.Append(pTranslatePack.teamId);
        //        requestSb.Append("/");
        //        requestSb.Append(pTranslatePack.containerId);
        //        requestSb.Append("/");
        //        requestSb.Append(pTranslatePack.fileId);

        //        var requestTranslateURL = requestSb.ToString();

        //        Debug.WriteLine($"{requestTranslateURL}");

        //        // TODO : HttpClient Post 방식으로 서버에 요청할 때, using 문 사용 (2024.08.26 minjae)
        //        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/fundamentals/networking/http/httpclient   
        //        using HttpResponseMessage response = await pClient.PostAsync(requestTranslateURL, jsonContent);

        //        string resMsg = await response.Content.ReadAsStringAsync();

        //        Console.WriteLine($"{resMsg}\n");


        //        // 웹서버로 부터 받은 결과값(json 데이터)을 Dictionary 변환
        //        var translateJsonNode = JsonNode.Parse(resMsg);
        //        var translateInfoDic = JsonSerializer.Deserialize<Dictionary<string, object>>(translateJsonNode);

        //        Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 Rest API 종료");

        //        return translateInfoDic;   // Dictionary 클래스 객체 translateInfoDic 리턴


        //        // TODO : 아래 주석친 코드 필요시 참고 (2024.08.26 minjae)
        //        // POST - 3. 컨테이너에 속한 특정 파일 변환 API Http 통신 결과가 성공(또는 파일 변환 중)할 경우 
        //        //if(HttpStatusCode.OK == response.StatusCode
        //        //   || HttpStatusCode.Accepted == response.StatusCode)
        //        //{
        //        //    // Http 통신 결과가 성공(또는 파일 변환 중)할 경우 웹서버로 부터 받은 결과값(json 데이터)을 Dictionary 변환 
        //        //    var translateJsonNode = JsonNode.Parse(resMsg);
        //        //    var translateInfoDic = JsonSerializer.Deserialize<Dictionary<string, object>>(translateJsonNode);



        //        //    Log.Logger.Information(Logger.GetMethodPath(currentMethod) + "3. 컨테이너에 속한 특정 파일 변환 성공");

        //        //    return translateInfoDic;   // Dictionary 클래스 객체 translateInfoDic 리턴
        //        //}
        //        //// POST - 3. 컨테이너에 속한 특정 파일 변환 API Http 통신 결과가 실패할 경우 
        //        //else
        //        //    throw new Exception(resMsg);


        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO : Http 통신 Post 방식 작업 시 서버로 부터 결과값 리턴되지 않고 시간이 길어지면 Time Out 처리 되면서 오류 메시지 출력 기능 구현 예정 (2023.12.01 minjae)
        //        // Log.Information(LogHelper.GetMethodPath(currentMethod) + ex.Message);
        //        Log.Logger.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);

        //        throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
        //    }
        //}

        #endregion Sample
    }
}
