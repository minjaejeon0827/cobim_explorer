using Stylet;
using StyletIoC;

using CobimExplorer.Interface.Page;
using CobimExplorer.Services.Page;

namespace CobimExplorer.ViewModels.Windows.LoadingWork
{
    // TODO : 로그인, 체크아웃, 체크인, 다운로드 시 처리 중 화면 뷰모델(CommonLoadingWorkVM.cs) 구현 (2024.01.03 jbh)  
    // 참고 URL - https://standard-stereotype.tistory.com/13  
    // 참고 2 URL - https://rhkdrmfh.tistory.com/135  
    // 참고 3 URL - https://www.sysnet.pe.kr/3/0/5107?pageno=0#google_vignette  
    // TODO : 필요시 "CobimExplorer.Data.Models 데이터 모델 프로젝트 파일 -> 폴더 CobimBase -> 폴더 CommonLoadingWork" 생성 및 "CommonLoadingWorkView.cs" 생성하기 (2024.01.03 jbh)
    // public class CommonLoadingWorkVM : PageBase<CommonLoadingWorkView>, IPageBase          
    public class CommonLoadingWorkVM : PageBase, IPage
    {
        #region 프로퍼티 

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        // TODO : 필요시 프로퍼티 "WindowManager" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IWindowManager WindowManager { get; set; }

        /// <summary>
        /// 화면 출력 메세지 
        /// </summary>
        public string Message { get => _Message; set { _Message = value; NotifyOfPropertyChange(); } }
        private string _Message = string.Empty;

        #endregion 프로퍼티

        #region 생성자 

        public CommonLoadingWorkVM(IEventAggregator events, IContainer container) : base(container)
        {
            // TODO : 아래 주석친 코드 필요시 사용 예정 (2024.01.03 jbh)
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;
        }

        #endregion 생성자

        #region 기본 메소드 

        protected override void OnActivate()
        {
            base.OnActivate();
            EventAggregator.Subscribe(this);
            // TODO : 필요시 "CommonProgressStateMsg.cs" 클래스 구현 및 아래 주석친 코드 사용 예정 (2024.01.03 jbh)
            // EventAggregator.PublishOnUIThread(new CommonProgressStateMsg(this, CommonProgressState.Open));
        }

        protected override void OnDeactivate()
        {
            EventAggregator.Unsubscribe(this);
            // TODO : 필요시 "CommonProgressStateMsg.cs" 클래스 구현 및 아래 주석친 코드 사용 예정 (2024.01.03 jbh)
            // EventAggregator.PublishOnUIThread(new CommonProgressStateMsg(this, CommonProgressState.Close));
            base.OnDeactivate();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        #endregion 기본 메소드 

        #region SetMessage

        // TODO : 메서드 "SetMessage"에 존재하는 메서드 파라미터 "IsAutoNewLine" 메세지 멀티 라인 기능 필요시 사용 예정 (2024.01.03 jbh)
        /// <summary>
        /// 화면 출력 메세지 셋팅 
        /// </summary>
        public CommonLoadingWorkVM SetMessage(string pMessage, bool IsAutoNewLine = false)
        {
            this.Message = pMessage;
            return this;
        }

        #endregion SetMessage

        #region ShowDialog

        // TODO : 메서드 "ShowDialog" 필요시 사용 예정 (2024.01.04 jbh)
        //public void ShowDialog()
        //{
        //    WindowManager.ShowDialog(this);
        //}

        #endregion ShowDialog

        #region ShowWindow

        public void ShowWindow()
        {
            WindowManager.ShowWindow(this);
        }

        #endregion ShowWindow

        #region Sample

        #endregion Sample
    }
}
