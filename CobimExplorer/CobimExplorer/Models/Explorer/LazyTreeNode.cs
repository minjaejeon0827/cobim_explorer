using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace CobimExplorer.Models.Explorer
{
    // TODO : LazyTreeView 구현 (2023.07.17 jbh)
    // 유튜브 참고 URL
    // WPF TreeView - LazyTreeNode(지연로드 트리뷰) 만들기 - 1
    // 참고 URL - https://youtu.be/8mCzmlGRU4Y

    // WPF TreeView - LazyTreeNode(지연로드 트리뷰) 만들기 - 2(완)
    // 참고 URL - https://youtu.be/IId6XmwlqlQ

    public class LazyTreeNode
    {
        /// <summary>
        /// 자식노드(Children)의 부모노드(Parent)가 누구인지 할당
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _children_Collection(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Collection에 아이템이 추가가 되었을 경우 
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // Collection에 추가된 아이템을 반복문으로 하나씩 순회하면서 해당 각각의 아이템들의 부모 노드(Parent) 설정 
                foreach (LazyTreeNode node in e.NewItems!)
                {
                    node.Parent = this;  // 이 노드(아이템)의 부모는 자기 자신(LazyTreeNode 클래스)이라는 것을 알려줌(할당)
                }
            }
        }

        /// <summary>
        /// 생성한 노드(아이템 - var node)의 전체 디렉토리(또는 파일) 경로
        /// </summary>
        //public string Key { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 디렉토리(또는 파일) 이름
        /// </summary>
        //public string Text { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2023.07.17 jbh)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2023.07.17 jbh)
        // 참고 URL - https://kukuta.tistory.com/359
        // C# Nullable 타입 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고

        /// <summary>
        /// 노드가 펼쳐졌을 때 이벤트 OnExpanded
        /// 이 이벤트를 발생시킬 때 인자값으로는 자기 자신(LazyTreeNode 클래스)을 넘겨준다.
        /// </summary>
        public event Action<LazyTreeNode>? OnExpanded;  // nullable 전처리 중이지만 null 허용

        /// <summary>
        /// 태그
        /// </summary>
        public object? Tag { get; set; }   // nullable 전처리 중이지만 null 허용

        /// <summary>
        /// 비트맵 이미지 타입 (Opened)
        /// </summary>
        public BitmapImage? OpenedImage { get; set; }

        /// <summary>
        /// 비트맵 이미지 타입 (Closed)
        /// </summary>
        public BitmapImage? ClosedImage { get; set; }

        /// <summary>
        /// 부모 노드 
        /// 최상위 노드일 경우 Parent(부모 노드)가 존재하지 않을 수 있다. (null)
        /// </summary>
        public LazyTreeNode? Parent { get; set; }    // nullable 전처리 중이지만 null 허용

        /// <summary>
        /// 자식 노드 
        /// 자식 노드의 경우 Parent(부모 노드)가 반드시 존재한다. 
        /// </summary>
        public ObservableCollection<LazyTreeNode> Children        // nullable 전처리 중이지만 null 허용
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                // 싱글톤 패턴 
                if (_children is null)
                {
                    _children = new ObservableCollection<LazyTreeNode>();
                    _children.CollectionChanged += _children_Collection; // _children 객체가 새로 생성될 때, 부모 노드(Parent)가 누구인지 알려주기 위해 _children 객체의 아이템 변경될 때 이벤트 "_children_Collection" 가입 

                }
                return _children;
            }
            set => _children = value;
            // set { _children = value; NotifyOfPropertyChange(); }
        }

        private ObservableCollection<LazyTreeNode>? _children;     // nullable 전처리 중이지만 null 허용

        /// <summary>
        /// 노드가 펼쳐졌을 때 이벤트 OnExpanded를 발생시킬 속성(프로퍼티) IsExpanded
        /// 속성(프로퍼티) IsExpanded는 나중에 <TreeView> 영역의 Item(아이템)의 IsExpanded 속성((예)Item.IsExpanded)과 바인딩 될 속성이다.
        /// MainV.xaml 에서 <TreeView> 영역의 아이템(<Style TargetType="{x:Type TreeViewItem}">) 영역에서
        /// IsExpanded 속성((예) Property = "IsExpanded")에 LazyTreeNode.cs -> 속성(프로퍼티) IsExpanded 를 바인딩 처리.
        /// (<Setter Property="IsExpanded" Value="{Binding IsExpanded, Mode=TwoWay}"/>)
        /// 아이템 TreeViewItem에는 LazyTreeNode.cs의 IsExpanded 속성을 바인딩 처리
        /// </summary>
        public bool IsExpanded                // nullable 전처리 중이지만 null 허용
        {
            get => _isExpanded;

            set
            {
                _isExpanded = value;
                // TreeView 아이템이 펼쳐졌을 경우 
                if (_isExpanded)
                {
                    Children.Clear();         // 해당 노드의 자식노드로 생성된 더미노드 초기화(삭제) 처리 
                    OnExpanded?.Invoke(this); // OnExpanded 이벤트 호출(Invoke)
                }
            }
        }

        private bool _isExpanded;             // nullable 전처리 중이지만 null 허용


#nullable disable  // nullable 전처리 비활성화

        #region AddDummyNode

        /// <summary>
        /// 더미노드 생성 
        /// </summary>
        public void AddDummyNode()
        {
            Children.Add(new LazyTreeNode());
        }

        #endregion AddDummyNode
    }
}
