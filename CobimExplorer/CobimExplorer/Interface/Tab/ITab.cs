using System;
using System.Windows.Input;

using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimNet;

namespace CobimExplorer.Interface.Tab
{
    // TODO : 리본 버튼 클릭시 출력되는 화면을 탭 페이지로 출력 + 버튼 "닫기" 클릭시 탭 페이지 화면 종료 되도록 구현 (2024.01.10 jbh)  
    // 참고 URL - https://youtu.be/rFhmTccRUpQ?si=dNYuEcN6487nTeYt
    public interface ITab
    {
        string Name { get; }

        /// <summary>
        /// 리본버튼 클릭시 화면의 TabItem으로 활성화 될 뷰모델 이름
        /// </summary>
        string VMName { get; set; }

        /// <summary>
        /// TabItem Header 문자열 
        /// </summary>
        string TabItemName { get; set; }

        /// <summary>
        /// 탭 페이지 닫기 Command
        /// </summary>
        ICommand CloseCommand { get; set; }

        event EventHandler CloseRequested;
    }

    public class Tab : BindableBase, ITab
    {
        #region 프로퍼티 

        public string Name { get; set; }

        /// <summary>
        /// 리본버튼 클릭시 화면의 TabItem으로 활성화 될 뷰모델 이름
        /// </summary>
        public string VMName { get; set; }

        /// <summary>
        /// TabItem Header 문자열 
        /// </summary>
        public string TabItemName { get; set; }

        public ICommand CloseCommand { get; set; }

        public event EventHandler CloseRequested;

        #endregion 프로퍼티 

        #region 생성자 

        public Tab()
        {
            CloseCommand = new ButtonCommand(CobimHelper.VoidMethodType, CloseTab, CanExecuteMethod);
        }

        #endregion 생성자

        #region 기본 메소드 

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }

        #endregion 기본 메소드

        #region CloseTab

        private void CloseTab(object obj)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion CloseTab
    }
}
