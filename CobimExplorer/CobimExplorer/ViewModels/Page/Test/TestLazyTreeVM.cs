using Stylet;
using StyletIoC;
using System;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using CobimExplorer.Utils;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Interface.Page;
using CobimExplorer.Services.Page;
using CobimExplorer.Models.Explorer;

using static CobimExplorer.Utils.ToggleImageUtils;

// TODO : LazyTreeView 구현 (2023.07.17 minjae)
// 유튜브 참고 URL
// WPF TreeView - LazyTreeNode(지연로드 트리뷰) 만들기 - 1
// 참고 URL - https://youtu.be/8mCzmlGRU4Y

// WPF TreeView - LazyTreeNode(지연로드 트리뷰) 만들기 - 2(완)
// 참고 URL - https://youtu.be/IId6XmwlqlQ

// 깃허브 소스코드
// 참고 URL - https://github.com/KaburiCoder/LazyTreeNode 

namespace CobimExplorer.ViewModels.Page.Test
{
    public class TestLazyTreeVM : PageBase, IPage
    {
        #region 프로퍼티 

        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2023.07.17 minjae)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2023.07.17 minjae)
        // 참고 URL - https://kukuta.tistory.com/359
        // C# Nullable 타입 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
        // C# Nullable 타입 예제 코드
#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고

        //public event PropertyChangedEventHandler PropertyChanged;    // nullable 전처리 중이지만 null 허용

#nullable disable  // nullable 전처리 비활성화

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 minjae) 
        // public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 minjae) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        #region OnActivate

        // TODO : 아래 프로퍼티 "Sender" 필요시 참고 (2024.08.07 minjae)
        /// <summary>
        /// 이벤트 OnActivate 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        // private TestLazyTreeVM Sender { get; set; }

        #endregion OnActivate

        public ObservableCollection<LazyTreeNode> PathNodes 
        { 
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                // 싱글톤 패턴 
                if (_pathNodes is null)
                {
                    _pathNodes = new ObservableCollection<LazyTreeNode>();
                }
                return _pathNodes;
            }
            set => _pathNodes = value;
        }

        private ObservableCollection<LazyTreeNode> _pathNodes;

        /// <summary>
        /// 하위 폴더 Collection
        /// </summary>
        public ObservableCollection<LazyTreeNode> SubFolderCollection
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                // 싱글톤 패턴 
                if (_subFolderCollection is null)
                {
                    _subFolderCollection = new ObservableCollection<LazyTreeNode>();


                }

                return _subFolderCollection;
            }
            set => _subFolderCollection = value;
        }

        private ObservableCollection<LazyTreeNode> _subFolderCollection;


        /// <summary>
        /// 파일 정보 Collection
        /// </summary>
        public ObservableCollection<LazyTreeNode> FileInfoCollection
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                // 싱글톤 패턴 
                if (_fileInfoCollection is null)
                {
                    _fileInfoCollection = new ObservableCollection<LazyTreeNode>();


                }

                return _fileInfoCollection;
            }
            set => _fileInfoCollection = value;
        }

        private ObservableCollection<LazyTreeNode> _fileInfoCollection;

        #endregion 프로퍼티 

        #region 생성자 

        public TestLazyTreeVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 minjae)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;
        }

        #endregion 생성자 

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            // PathNodes에 LazyTreeNode 아이템 2개 추가 
            //PathNodes.Add(CreateNode("1", "홍길동"));
            //PathNodes.Add(CreateNode("2", "김이박"));

            //string strJson = @"[
            //    { ""Key"": ""1"", ""Text"": ""홍길동"" },
            //    { ""Key"": ""2"", ""Text"": ""김이박"" },
            // ]";

            // TODO : Json 데이터 -> ObservableCollection Convert 처리 구현 (2023.07.18 minjae)
            //string Json = @"[
            //    {""Key"": ""1"", ""Text"": ""홍길동"" },
            //    {""Key"": ""2"", ""Text"": ""김이박"" },
            // ]";


            // TODO : JObject.Parse 실행시 오류 메시지 ": 'Error reading JObject from JsonReader. Current JsonReader item is not an object: StartArray. Path '', line 1, position 1.'" 해결 (2023.07.17 minjae)
            // 참고 URL - https://blog.naver.com/chandong83/222771357703
            // JObject jsonObject = JObject.Parse(strJson);
            //JArray jarray = JArray.Parse(Json);

            //foreach (var item in jarray.Children())
            //{
            //    JObject jObject = JObject.Parse(item.ToString());

            //    string key = jObject.First.First.ToString();
            //    string text = jObject.Last.Last.ToString();

            //    PathNodes.Add(CreateNode(key, text));
            //}

            // ObservableCollection<LazyTreeNode> TestPathNodes = JsonConvert.DeserializeObject<ObservableCollection<LazyTreeNode>>(json);
            // PathNodes = JsonConvert.DeserializeObject<ObservableCollection<LazyTreeNode>>(json);


            //foreach (var TestPathNode in TestPathNodes)
            //{
            //    PathNodes.Add(CreateNode(TestPathNode.Key, TestPathNode.Text));
            //}

            // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 minjae)
            // InitTreeViewText();
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // TODO : 싱글톤 패턴 사용해서 프로퍼티 PathNodes Collection에서 객체 생성하도록 로직 보완 (2023.07.26 minjae)
            // Collection PathNodes 객체 생성 
            // PathNodes = new ObservableCollection<LazyTreeNode>();

            InitLazyTreeNode();     // TreeView에 바인딩된 Collection 객체 초기화

            AddDriveNodes();  // 드라이브 노드 생성 (탐색기 기능)

            // TODO : 아래 테스트 코드 필요시 참고 (2024.08.07 minjae)
            //Sender = this;

            //var vm = Container.Get<TestLazyTreeVM>();
            //string VMName = vm.GetViewModelName();

            // TODO : 필요시 var msg = new ChangeViewModelMsg(this, VMName); 사용 예정 (2024.08.06 minjae)
            //var msg = new ChangeViewModelMsg(this, VMName);
            //var msg = new OpenPageMsg((ICbVM)Sender, VMName);
            //EventAggregator.PublishOnUIThread(msg);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        #endregion 기본 메소드 

        #region InitLazyTreeNode

        /// <summary>
        /// TreeView에 바인딩된 Collection 객체 초기화
        /// </summary>
        private void InitLazyTreeNode()
        {
            PathNodes.Clear();
            SubFolderCollection.Clear();
            FileInfoCollection.Clear();
        }

        #endregion InitLazyTreeNode

        #region CreateNode

        /// <summary>
        /// LazyTreeNode 아이템(노드) 생성해주는 메서드 "CreateNode"
        /// </summary>
        /// <param name="key">생성한 노드(아이템 - var node)의 전체 디렉토리(또는 파일) 경로</param>
        /// <param name="text">디렉토리(또는 파일) 이름</param>
        /// <param name="explorerType">드라이브, 디렉토리, 파일 구볋해주는 열거형(enum) 구조체</param>
        public LazyTreeNode CreateNode(string key, string text, ExplorerType explorerType)
        {
            var images = ToggleImageUtils.GetExplorers(explorerType);
            var node = new LazyTreeNode { Path = key, FileName = text };

            // node가 펼쳐졌을 경우(화면에 출력되는 부모노드 (예) "홍길동" 또는 "김이박"을 더블클릭한 경우) 이벤트 "Node_OnExpanded" 추가 
            node.OnExpanded += Node_OnExpanded;

            node.OpenedImage = images.opendedImage; // node가 오픈 되었을 때 이미지
            node.ClosedImage = images.closedImage;  // node가 닫혔을 때 이미지

            // 하위 디렉토리 또는 파일 존재 여부 체크 
            if (true == DirectoryUtils.IsDirectoryOrFileExists(key))
            {
                node.AddDummyNode();  // 만약 하위 디렉토리 또는 파일 존재하는 경우 더미노드 생성 
            }

            return node;
        }

        #endregion CreateNode

        #region Node_OnExpanded

        /// <summary>
        /// 더미노드가 펼쳐졌을 경우(화면에 출력되는 부모노드 "홍길동" 또는 "김이박"을 더블클릭한 경우) 실행되는 이벤트 메서드 "Node_OnExpanded" (지연 로드(로딩) 기능 이벤트 메서드)
        /// </summary>
        /// <param name="obj"></param>
        private void Node_OnExpanded(LazyTreeNode node)
        {
            // TODO : 오류 메시지 "'C:\PerfLogs\Admin' 경로에 대한 액세스가 거부되었습니다." 로직 보완 (2023.07.26 minjae)
            // try ~ catch ~ finally
            // 참고 URL - https://hello-bryan.tistory.com/21
            try
            {
                SubFolderCollection.Clear();    // 하위 폴더 Collection 초기화
                FileInfoCollection.Clear();     // 파일 정보 Collection 초기화

                // else if (node.OpenedImage.UriSource.Equals(GetBitmapImageByFileName("file.png").UriSource))     FileInfoCollection.Clear();     // 파일 정보 Collection 초기화

                // 하위 디렉토리 추가 
                // 생성한 더미노드(node)의 전체 경로(node.Key)의 하위 폴더(di)들을 반복문으로 돌려서 방문 
                foreach (var di in DirectoryUtils.GetDirectories(node.Path))
                {
                    // TODO : 오류 메시지 "System.UnauthorizedAccessException: ''C:\PerfLogs\Admin' 경로에 대한 액세스가 거부되었습니다.'" 추후 보완 예정 (2023.07.19 minjae)


                    // 생성한 더미노드(node)의 자식 디렉토리(Children) 경로, 이름, 이미지 추가 - CreateNode(di.FullName, di.Name, ExplorerType.Directory)
                    node.Children.Add(CreateNode(di.FullName, di.Name, ExplorerType.Directory));
                    SubFolderCollection.Add(CreateNode(di.FullName, di.Name, ExplorerType.Directory));     // 하위 폴더 Collection 데이터 추가 
                }

                // 하위 파일 추가 
                // 생성한 더미노드(node)의 전체 경로(node.Key)의 하위 파일(fi)들을 반복문으로 돌려서 방문 
                foreach (var fi in DirectoryUtils.GetFiles(node.Path))
                {
                    // 생성한 더미노드(node)의 하위 파일(Children) 경로, 이름, 이미지 추가 - CreateNode(fi.FullName, fi.Name, ExplorerType.File)
                    node.Children.Add(CreateNode(fi.FullName, fi.Name, ExplorerType.File));
                    FileInfoCollection.Add(CreateNode(fi.FullName, fi.Name, ExplorerType.File));           // 파일 정보 Collection 데이터 추가 
                }



                // TODO: 테스트 코드 부모 노드(parent) 밑에 자식 노드(Children) "가나다" 추가 구현 (2023.07.18 minjae)
                // Collection Children Nullable(물음표 ?) 삭제 (경고 메시지가 출력되기 때문)
                // 어차피 _children 값이 null 이여도 싱글톤 패턴으로 인해 _children 값은 null이다.
                // node.Children.Add(CreateNode("3", "가나다"));

                // 조건을 줘서(switch - case) node의 자식 노드(Children) 만들기 
                //switch (node.Key)
                //{
                //    case "1":
                //        // Key "1"의 값인 "홍길동"의 자식 노드로 "홍길동의 자식" 추가 
                //        // "홍길동의 자식" 노드를 계속 클릭해도 그 밑에 자식 노드가 생기지 않는다. (switch - case문에서 case "3": 가 존재하지 않기 때문)
                //        node.Children.Add(CreateNode("3", "홍길동의 자식"));
                //        break;
                //    case "2":
                //        // Key "2"의 값인 "김이박"의 자식 노드로 "김이박의 자식" 추가 
                //        // "김이박의 자식" 노드를 계속 클릭해도 그 밑에 자식 노드가 생기지 않는다. (switch - case문에서 case "4": 가 존재하지 않기 때문)
                //        node.Children.Add(CreateNode("4", "김이박의 자식"));
                //        break;

                //}
            }
            catch (Exception ex)
            {
                // 메시지 박스 구현 참고 URL - https://gigong.tistory.com/52
                // 오류 메시지 출력
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
            finally
            {
                // TODO : 필요시 추가 예정 (2023.07.26 minjae)
            }
        }

        #endregion Node_OnExpanded

        /// <summary>
        /// 하위 폴더 컬렉션 생성 
        /// </summary>
        //private void CreateSubFolderCollection(ObservableCollection<LazyTreeNode> children)
        //{

        //}

        #region AddDriveNodes

        /// <summary>
        /// 드라이브 노드 생성 (탐색기 기능)
        /// </summary>
        private void AddDriveNodes()
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                // 드라이브 경로, 이름, 이미지 추가 - CreateNode(driveInfo.Name, driveInfo.Name, ExplorerType.Drive)
                PathNodes.Add(CreateNode(driveInfo.Name, driveInfo.Name, ExplorerType.Drive));
            }
        }

        #endregion AddDriveNodes

        #region Sample

        #endregion Sample
    }
}
