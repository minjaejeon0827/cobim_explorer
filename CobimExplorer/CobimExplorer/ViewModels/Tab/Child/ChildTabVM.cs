using Stylet;
using StyletIoC;

using System.Windows.Controls;

using CobimExplorer.Interface.Page;
using CobimExplorer.ViewModels.Message;
using CobimExplorer.Services.Page;

namespace CobimExplorer.ViewModels.Tab.Child
{
    // TODO : 리본 버튼 클릭시 출력되는 화면을 탭 페이지로 출력 + 버튼 "닫기" 클릭시 탭 페이지 화면 종료 되도록 구현 (2024.01.10 jbh)  
    // 참고 URL - https://youtu.be/rFhmTccRUpQ?si=dNYuEcN6487nTeYt
    public class ChildTabVM : PageBase   // , IPage
    {
        #region 프로퍼티 

        #region UserControl

        // TODO : UserControl 클래스 프로퍼티 "Page"의 경우 프로퍼티 안에 속한 값을
        //        변경할 수 있는 필드(Content, DataContext)가 존재하므로 싱글톤 구현 대상 제외 (2023.11.08 jbh)
        // 참고 URL - https://minchul-son.tistory.com/317
        /// <summary>
        /// 탐색기 화면에서 출력하는 화면 뷰 + 뷰모델
        /// </summary>
        public UserControl Page { get => _Page; set { _Page = value; NotifyOfPropertyChange(); } }
        private UserControl _Page = new UserControl();

        #endregion UserControl

        // TODO : 프로퍼티 "TestExplorerV" 필요시 참고 (2024.08.05 jbh) 
        /// <summary>
        /// UserControl 뷰 TestExplorerV (싱글톤 객체)
        /// </summary>
        //[Inject]
        //public TestExplorerV TestExplorerV { get => _TestExplorerV; set { _TestExplorerV = value; NotifyOfPropertyChange(); } }
        //private TestExplorerV _TestExplorerV = new TestExplorerV();

        #endregion 프로퍼티 

        #region 생성자 

        // TODO : 주의사항 - TabControl(TabParentPageV.xaml) 화면 뷰 안에 존재하는<TabControl> 영역에 속성 SelectedItem= "{Binding ActiveItem}" 사용 안 함 (2024.09.12 jbh)  
        //                   왜냐하면 Dynamic TabItem("ExplorerVM", "HubBoxVM") 클릭시 TabControl(TabParentPageV.xaml) 화면 뷰 BorderBrush 색상이 Red로 바뀌는 오류가 발생하기 때문이다.  
        //                   SelectedItem="{Binding ActiveItem}" 사용 안 하는 대신 ChildTabV.xaml 에서 아래처럼<ContentControl> 영역에서 ActiveItem 바인딩 처리 하고  
        //                   <ContentControl s:View.Model="{Binding ActiveItem}" />  ChildTabVM.cs 생성자 호출시 아래처럼 ActiveItem 값이 null일 때, 새로 값 할당 해줌  
        //                   this.ActiveItem = pActiveVM;  

        // public ChildTabVM(IEventAggregator events, IContainer container, object obj, IPage pActiveVM) : base(container)
        public ChildTabVM(IEventAggregator events, IContainer container, string pVMName, string pTabItemName, IPage pActiveVM) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;

            Page.Content = null;
            Page.DataContext = null;

            VMName = pVMName;
            TabItemName = pTabItemName;

            // Page = null;

            // VMName  = obj.ToString();
            // VMName = pVMName;
            // TabItemName = VMName + " " + DateTime.Now.ToString();

            // var tabParent = this.Items.Where(it => string.Equals(it.GetType().Name, vmName)).FirstOrDefault();

            if(pActiveVM is not null)
            {
                // var vm = this;

                // "TabParentPageVM" 뷰모델과 연동된 초기화면 뷰(TabParentPageV)를 열어준다.
                // this.ActivateItem(pActiveVM);
                // Page.Content = TestExplorerV;
                Page.DataContext = (object)pActiveVM;
                this.ActiveItem = pActiveVM;
            }
        }

        #endregion 생성자

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 jbh)
            // InitTreeViewText();

            // var vm = Container.Get<DateTabVM>();

            // string VMName = vm.GetViewModelName();

            // var msg = new ChangeViewModelMsg(this, VMName);
            // EventAggregator.PublishOnUIThread(msg);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        #endregion 기본 메소드

        #region Sample

        #endregion Sample
    }
}
