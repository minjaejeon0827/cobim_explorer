using Serilog;

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CobimExplorer.Common.CobimBase;
using CobimExplorer.Common.LogManager;

// TODO : TabItem Header를 마우스 드래그 앤 드롭해서 위치 변경 (2024.07.23 jbh)  
// 참고 URL - https://stackoverflow.com/questions/10738161/is-it-possible-to-rearrange-tab-items-in-tab-control-in-wpf  
// 참고 2 URL - https://stackoverflow.com/questions/26174695/drag-and-drop-tabitems-in-wpf  

namespace CobimExplorer.Views.Page.Parents
{
    /// <summary>
    /// TabParentPageV.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TabParentPageV : UserControl
    {
        #region 프로퍼티

        /// <summary>
        /// MouseMove 이벤트 실행시 DragDrop할 TabItem
        /// </summary>
        private TabItem TabItem { get; set; }

        #endregion 프로퍼티

        #region 생성자

        public TabParentPageV()
        {
            InitializeComponent();
        }

        #endregion 생성자

        #region TabItem_PreviewMouseMove

        // TODO : PreviewMouseMove 이벤트 메서드 "TabItem_PreviewMouseMove" 실행시 
        //        소스파일 TabParentPageVM.cs
        //        -> TabItem 마우스 Drop 이벤트(TabItem 들끼리 위치 Swap 처리 가능) 메서드 TabItem_Drop 실행 되지만,
        //        -> TabItem 닫기 이벤트 메서드 OnTabCloseRequested 가 실행 되지 않는 오류가 발생하여,
        //        -> PreviewMouseMove 이벤트 메서드 "TabItem_PreviewMouseMove" 대신
        //        MouseMove 이벤트 메서드 "TabItem_MouseMove" 사용하는 것으로 수정 (2024.08.02 jbh)
        // 참고 URL - https://stackoverflow.com/questions/20899810/previewmousemove-vs-mousemove
        // 참고 2 URL - https://web.archive.org/web/20140225041901/http://www.csharptutorial.in/2012/10/understanding-event-handling-in-wpf.html#.UwwZyXbP1qY

        /// <summary>
        /// TabItem PreviewMouseMove 이벤트 
        /// </summary>
        // private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        // {
        //     var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

        //     try
        //     {
        //         TabItem = e.Source as TabItem;

        //         if (TabItem is null) return;

        //         if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
        //             DragDrop.DoDragDrop(TabItem, TabItem, DragDropEffects.All);
        //     }
        //     catch(Exception ex)
        //     {
        //         Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
        //         MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
        //     }
        // }

        #endregion TabItem_PreviewMouseMove

        #region TabItem_MouseMove

        /// <summary>
        /// TabItem MouseMove 이벤트 
        /// </summary>
        private void TabItem_MouseMove(object sender, MouseEventArgs e)
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                TabItem = e.Source as TabItem;

                if (TabItem is null) return;

                if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
                    DragDrop.DoDragDrop(TabItem, TabItem, DragDropEffects.All);
            }
            catch (Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion TabItem_MouseMove

        #region Sample

        #endregion Sample
    }
}
