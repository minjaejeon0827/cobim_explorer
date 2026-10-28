using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Rest.Api.CobimBase.Explorer
{
    public class FolderHelper
    {
        #region 프로퍼티 

        /// <summary>
        /// 부모 폴더 아이디
        /// </summary>
        public const string parentFolderId = "parentFolderId";

        /// <summary>
        /// 폴더 그룹 아이디
        /// </summary>
        public const string folderGroupId = "folderGroupId";

        /// <summary>
        /// 폴더 아이디
        /// </summary>
        public const string folderId = "folderId";
        
        /// <summary>
        /// 폴더 이름 
        /// </summary>
        public const string folderName = "folderName";

        /// <summary>
        /// 폴더 레벨 (상위 폴더 및 하위 폴더 분류 기준)
        /// </summary>
        public const string levelNo = "levelNo";

        /// <summary>
        /// 루트 디렉토리(최상위 폴더) 레벨 (팀 하위 최상위 폴더 의미)
        /// </summary>
        // 참고 URL - https://ko.wikipedia.org/wiki/%EB%A3%A8%ED%8A%B8_%EB%94%94%EB%A0%89%ED%86%A0%EB%A6%AC
        public const string rootFolder = "0";

        /// <summary>
        /// 루트 디렉토리 바로 밑의 서브 폴더 레벨 
        /// </summary>
        public const string level1SubFolder = "1";

        // TODO : 4. 폴더 목록 조회 const string 변수 "requestFolderURL" 필요시 수정 예정 (2023.09.20 minjae)
        /// <summary>
        /// 4. 폴더 목록 조회 
        /// 요청 URL
        /// </summary>
        // public const string requestFolderURL = "https://cobim.io/api/folder/list/jc-constrct/inc-001-jc-constrct-t001";
        public const string requestFolderURL = "https://cobim.io/api/explorer/folder/list/";

        // TODO : 필요시 ExplorerVM.cs -> 비동기 메서드 FillFolders 위치를 FolderHelper.cs 로 변경 예정 (2023.11.30 minjae)
        /// <summary>
        /// 루트 폴더 밑의 서브 폴더 추가 메서드 
        /// </summary>
        //public static async Task FillFolders(FolderView parentFolder, List<Dictionary<string, object>> folderList)
        //{

        //}

        #endregion 프로퍼티

        #region Sample

        #endregion Sample
    }
}
