using System;

namespace CobimExplorer.Rest.Api.CobimBase.Login
{
    public class LoginHelper
    {
        #region 프로퍼티 

        /// <summary>
        /// 1. 로그인
        /// 요청 파라미터 - inputTenantId
        /// </summary>
        public const string inputTenantId = "inc-001";

        /// <summary>
        /// 1. 로그인
        /// 요청 URL - requestLoginURL
        /// </summary>
        public const string requestLoginURL = "https://cobim.io/api/account/user/authorize";

        /// <summary>
        /// 1. 로그인
        /// 요청 Header - Content-Type	
        /// </summary>
        public const string contentType = "application/json";

        /// <summary>
        /// 1. 로그인 응답 결과(상태) - status
        /// </summary>
        public const string status = "status";

        /// <summary>
        /// 1. 로그인 토큰 만료 에러 메시지 - A003
        /// </summary>
        public const string A003 = "A003";

        #endregion 프로퍼티 

        #region Sample

        #endregion Sample
    }
}
