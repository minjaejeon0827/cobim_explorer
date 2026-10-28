using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CobimExplorer.Utils
{
    // TODO : 드라이브 노드 하위에 폴더나 파일이 존재하는지를 체크하기 위해 유틸리티 구현 (2023.07.18 minjae)
    // 참고 URL - https://youtu.be/IId6XmwlqlQ
    public static class DirectoryUtils
    {
        /// <summary>
        /// 숨겨진 폴더 또는 권한이 있는 폴더 확인
        /// </summary>
        /// <param name="fsi"></param>
        /// <returns></returns>
        private static bool IsValidDirectoriesOrFiles(FileSystemInfo fsi)
        {
            try
            {
                // 숨겨진 폴더(isHidden = true)를 확인합니다. 
                bool isHidden = (fsi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                // 권한이 있는 폴더(isSystem = true)를 확인합니다.
                bool isSystem = (fsi.Attributes & FileAttributes.System) == FileAttributes.System;

                return !(isHidden || isSystem);   // 숨겨진 폴더(isHidden = true) 또는 권한이 있는 폴더(isSystem = true)는 보이지 않게(return false;) 반환 
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        /// <summary>
        /// 해당 노드의 하위로 디렉토리 또는 파일이 존재 하는지에 대한 여부 확인 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryOrFileExists(string path)
        {
            IEnumerable<DirectoryInfo> directories = GetDirectories(path);
            if (directories.Any()) return true;

            IEnumerable<FileInfo> files = GetFiles(path);
            if (files.Any()) return true;

            return false;
        }

        /// <summary>
        /// 하위 디렉토리 배열 반환 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<DirectoryInfo> GetDirectories(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if(true == di.Exists)
            {
                return di.GetDirectories("*", SearchOption.TopDirectoryOnly).Where(IsValidDirectoriesOrFiles);
            }
            else
            {
                return Enumerable.Empty<DirectoryInfo>();
            }
        }

        /// <summary>
        /// 하위 파일 배열 반환 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if(true == di.Exists)
            {
                return di.GetFiles("*", SearchOption.TopDirectoryOnly).Where(IsValidDirectoriesOrFiles);
            }
            else
            {
                return Enumerable.Empty<FileInfo>();
            }
        }
    }
}
