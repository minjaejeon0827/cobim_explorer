using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Rest.Api.CobimBase.Explorer
{
    public class ProjectHelper
    {
        #region 프로퍼티

        /// <summary>
        /// 2. 나의 프로젝트(+ 팀목록) 목록 조회 (Get)
        /// 요청 URL
        /// </summary>
        public const string requestProjectTeamURL = "https://cobim.io/api/explorer/project/my/list";

        // TODO : 테스트 프로젝트 아이디 "bmcb" 변경 (2023.09.12 jbh)
        /// <summary>
        /// 4. 폴더 목록 조회 
        /// 요청 파라미터 - 프로젝트 아이디
        /// </summary>
        //public const string testProjectId = "jc-constrct";
        public const string testProjectId = "bmcb";

        // TODO : 테스트 팀 아이디 "inc-001-bmcb-t005" 변경 (2023.09.12 jbh)
        /// <summary>
        /// 4. 폴더 목록 조회 
        /// 요청 파라미터 - 팀 아이디
        /// </summary>
        public const string testTeamId = "inc-001-bmcb-t005";

        // TODO : 4. 폴더 목록 조회 const string 변수 "requestFolderURL" -> FolderHelper.cs 소스파일로 이동 (2023.09.20 jbh)
        /// <summary>
        /// 4. 폴더 목록 조회 
        /// 요청 URL
        /// </summary>
        // public const string requestFolderURL = "https://cobim.io/api/folder/list/jc-constrct/inc-001-jc-constrct-t001";
        // public const string requestFolderURL = "https://cobim.io/api/folder/list/";

        /// <summary>
        /// 프로젝트 정보
        /// </summary>
        public const string projectEntity = "projectEntity";

        /// <summary>
        /// 프로젝트 아이디
        /// </summary>
        public const string projectId = "projectId";

        /// <summary>
        /// 프로젝트 이름
        /// </summary>
        public const string projectName = "projectName";

        /// <summary>
        /// Http 통신 결과 값 
        /// </summary>
        public const string resultData = "resultData";

        /// <summary>
        /// 프로젝트 + 팀 정보
        /// </summary>
        public const string projectWithTeamInfos = "projectWithTeamInfos";

        #endregion 프로퍼티

        #region Sample

        #endregion Sample
    }
}
