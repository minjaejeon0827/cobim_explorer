using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Test
{
    // TODO : TreeView DataType, ItemsSource 관련 클래스 Folder 구현 (2023.07.14 minjae)
    // 참고 URL - https://icodebroker.tistory.com/entry/CWPF-TreeViewItem-%ED%81%B4%EB%9E%98%EC%8A%A4-ItemsSource-%EC%86%8D%EC%84%B1%EC%9D%84-%EC%82%AC%EC%9A%A9%ED%95%B4-%ED%8A%B8%EB%A6%AC-%EB%B7%B0-%EB%A7%8C%EB%93%A4%EA%B8%B0#google_vignette

    /// <summary>
    /// 폴더
    /// </summary>
    public class Folder
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 디렉토리 정보
        /// </summary>
        private DirectoryInfo directoryInfo;

        /// <summary>
        /// 하위 폴더 컬렉션
        /// </summary>
        private ObservableCollection<Folder> subFolderCollection;

        /// <summary>
        /// 파일 정보 컬렉션
        /// </summary>
        private ObservableCollection<FileInfo> fileInfoCollection;

        #endregion Field

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 완전한 경로 - FullPath

        /// <summary>
        /// 완전한 경로
        /// </summary>
        public string FullPath
        {
            get
            {
                return this.directoryInfo.FullName;
            }

            set
            {
                if(true == Directory.Exists(value))
                {
                    this.directoryInfo = new DirectoryInfo(value);
                }
                else
                {
                    throw new ArgumentException("경로가 없습니다.", "FullPath");
                }
            }
        }

        #endregion 완전한 경로 - FullPath

        #region 명칭 - Name

        /// <summary>
        /// 명칭
        /// </summary>
        public string Name
        {
            get
            {
                return this.directoryInfo.Name;
            }
        }

        #endregion 명칭 - Name

        #region 하위 폴더 컬렉션 - SubFolderCollection

        /// <summary>
        /// 하위 폴더 컬렉션
        /// </summary>
        public ObservableCollection<Folder> SubFolderCollection
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                if (this.subFolderCollection is null)
                {
                    this.subFolderCollection = new ObservableCollection<Folder>();

                    // TODO : 오류 메시지 'c:\Documents and Settings' 경로에 대한 액세스가 거부되었습니다' 추후 확인 필요 (2023.07.26 minjae)
                    // 참고 URL - https://oracle.tistory.com/394
                    DirectoryInfo[] directoryInfoArray = this.directoryInfo.GetDirectories();

                    for (int i = 0; i < directoryInfoArray.Length; i++)
                    {
                        Folder folder = new Folder();

                        folder.FullPath = directoryInfoArray[i].FullName;

                        this.subFolderCollection.Add(folder);
                    }
                }

                return this.subFolderCollection;
            }
        }

        #endregion 하위 폴더 컬렉션 - SubFolderCollection

        #region 파일 정보 컬렉션 - FileInfoCollection

        /// <summary>
        /// 파일 정보 컬렉션
        /// </summary>
        public ObservableCollection<FileInfo> FileInfoCollection
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                if (this.fileInfoCollection is null)
                {
                    this.fileInfoCollection = new ObservableCollection<FileInfo>();

                    FileInfo[] fileInfoArray = this.directoryInfo.GetFiles();

                    for (int i = 0; i < fileInfoArray.Length; i++)
                    {
                        this.fileInfoCollection.Add(fileInfoArray[i]);
                    }
                }

                return this.fileInfoCollection;
            }
        }

        #endregion 파일 정보 컬렉션 - FileInfoCollection

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - Folder()

        /// <summary>
        /// 생성자
        /// </summary>
        public Folder()
        {
            FullPath = @"c:\";
        }

        #endregion 생성자 - Folder()
    }
}
