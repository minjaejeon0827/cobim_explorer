using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using CobimExplorer.Common.Extensions;
using CobimExplorer.Interface.Page;
using CobimExplorer.Message;
using CobimExplorer.Services.Page;
using CobimExplorer.ViewModels.Message;
using CobimExplorer.Interface;

namespace CobimExplorer.ViewModels.Page.Test
{
    public class TestExplorerVM : PageBase, IPage
    {
        #region 프로퍼티 

        // TODO : 테스트 코드 추후 삭제 예정 (2023.07.14 jbh)
        //public TreeView GroupView = new TreeView();

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        //public BindableCollection<TreeViewItem> DepthDatas { get; } = new BindableCollection<TreeViewItem>();

        //TODO : 아래 BindableCollection 구현 예시 참고해서 추후 BindableCollection 구현 예정 (2023.07.26 jbh)
        //public BindableCollection<ProgMenuView> DepthDatas { get; } = new BindableCollection<ProgMenuView>();

        #region OnActivate

        // TODO : 아래 프로퍼티 "Sender" 필요시 참고 (2024.08.07 jbh)
        /// <summary>
        /// 이벤트 OnActivate 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        // private TestExplorerVM Sender { get; set; }

        #endregion OnActivate

        #endregion 프로퍼티

        #region 생성자

        public TestExplorerVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;
            // TODO : 테스트 코드 추후 삭제 예정 (2023.07.14 jbh)
            //InitTreeViewText();
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

            // TODO : 아래 테스트 코드 필요시 참고 (2024.08.07 jbh)
            // Sender = this;

            // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 jbh)
            // InitTreeViewText();

            //var vm = Container.Get<TestExplorerVM>();
            //string VMName = vm.GetViewModelName();

            // TODO : 필요시 var msg = new ChangeViewModelMsg(this, VMName); 사용 예정 (2024.08.06 jbh)
            //var msg = new ChangeViewModelMsg(this, VMName);
            //var msg = new OpenPageMsg((ICbVM)Sender, VMName);
            //EventAggregator.PublishOnUIThread(msg);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        #endregion 기본 메소드

        #region InitTreeViewText

        // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 jbh)
        // TreeView 참고 URL - http://inasie.tistory.com/19 
        // TreeView 참고 2 URL - https://insurang.tistory.com/383 
        /// <summary>
        /// TreeView 데이터 출력 
        /// </summary>
        //private void InitTreeViewText()
        //{
        //    GroupView.Items.Clear();    // Collection 초기화

        //    // foreach (string str in Directory.GetDirectories(""))   // 특정폴더
        //    foreach (string str in Directory.GetLogicalDrives())   // 루트폴더
        //    {
        //        try
        //        {
        //            TreeViewItem item = new TreeViewItem();
        //            item.Header = str;
        //            item.Tag = str;
        //            item.Expanded += new RoutedEventHandler(item_Expanded);   // 노드 확장시 추가

        //            GroupView.Items.Add(item);


        //            GetSubDirectories(item);


        //        }

        //        catch (Exception ex)
        //        {
        //            // MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);   // 접근 거부 폴더로 인해 주석처리
        //        }
        //    }
        //}

        #endregion InitTreeViewText

        #region GetSubDirectories (서브 디렉토리)

        // TODO : TreeView 데이터 출력 테스트 코드 추후 삭제 예정 (2023.07.14 jbh)
        /// <summary>
        /// 서브 디렉토리
        /// </summary>
        /// <param name="itemParent"></param>
        //private void GetSubDirectories(TreeViewItem itemParent)
        //{
        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
        // 참고 URL - https://husk321.tistory.com/405
        //    if (itemParent is null) return;
        //    if (itemParent.Items.Count != 0) return;

        //    try
        //    {
        //        string strPath = itemParent.Tag as string;
        //        DirectoryInfo dInfoParent = new DirectoryInfo(strPath);
        //        foreach (DirectoryInfo dInfo in dInfoParent.GetDirectories())
        //        {
        //            TreeViewItem item = new TreeViewItem();
        //            item.Header = dInfo.Name;
        //            item.Tag = dInfo.FullName;
        //            item.Expanded += new RoutedEventHandler(item_Expanded);
        //            itemParent.Items.Add(item);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        // MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);   // 접근 거부 폴더로 인해 주석처리
        //    }
        //}

        #endregion GetSubDirectories (서브 디렉토리)

        #region item_Expanded

        /// <summary>
        /// 트리확장시 내용 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void item_Expanded(object sender, RoutedEventArgs e)
        //{
        //    TreeViewItem itemParent = sender as TreeViewItem;
              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
              // 참고 URL - https://husk321.tistory.com/405
        //    if (itemParent is null) return;
        //    if (itemParent.Items.Count == 0) return;
        //    foreach (TreeViewItem item in itemParent.Items)
        //    {
        //        GetSubDirectories(item);
        //    }
        //}

        #endregion item_Expanded

        #region Sample

        #endregion Sample
    }
}
