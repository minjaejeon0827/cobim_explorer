using Stylet;
using StyletIoC;

using CobimExplorer.Interface.Page;
using CobimExplorer.Services.Page;
using CobimExplorer.Rest.Api.CobimBase.HubBox;
using CobimExplorer.Data.Models.CobimBase.HubBox;

namespace CobimExplorer.ViewModels.Page.Project.BIM360.HubBox
{
    // TODO : TabItem으로 출력된 [2]번 HubBox 웹사이트 관리자(Admin-1) 화면 종료시
    // 구글 크롬 전용 <CefSharp> 화면
    // 뷰(HubBoxV.xaml)
    // 뷰모델(HubBoxVM.cs)에서
    // 사용하고 있는 메모리를 정리하는 로직 구현하기 (2024.08.22 minjae)

    // 참고 URL - https://stackoverflow.com/questions/48398628/c-sharp-cefsharp-memory-management
    // 참고 2 URL - https://blog.naver.com/iconms1/222033637762
    // 참고 3 URL - https://github.com/cefsharp/CefSharp/blob/master/CefSharp.WinForms.Example/BrowserForm.cs#L137

    // TODO : 구글 크롬 기반 웹브라우저 화면을 WPF에서 출력할 수 있도록 CefSharp(구글 크롬 웹브라우저 컨트롤) 구현 (2024.08.22 minjae) 
    // 참고 URL - https://blog.naver.com/codeknight2/222210163649  
    // 참고 2 URL - https://github.com/cefsharp/CefSharp/wiki/Quick-Start  
    // 유튜브 참고 URL - https://youtu.be/si8wemHOMYA?si=FiL-r-_5MoF1EwU_
    public class HubBoxVM : PageBase<HubBoxView>, IPage
    {
        #region 프로퍼티

        /// <summary>
        /// HubBox ACC 파일 변환 목록 화면 조회 Rest API 주소
        /// </summary>
        public string WebConvertListURL { get => _WebConvertListURL; set { _WebConvertListURL = value; NotifyOfPropertyChange(); } }
        private string _WebConvertListURL;

        #endregion 프로퍼티

        #region 생성자

        public HubBoxVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.08.22 minjae)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;

            this._WebConvertListURL = HubBoxHelper.requestWebConvertListURL;   // HubBox ACC 파일 변환 목록 화면 조회 Rest API 주소
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

            // TODO : 아래 테스트 코드 필요시 참고 (2024.08.22 minjae)
            // Sender = this;

            // var vm = Container.Get<HubBoxVM>();
            // string VMName = vm.GetViewModelName();

            // TODO : 필요시 var msg = new ChangeViewModelMsg(this, VMName); 사용 예정 (2024.08.22 minjae)
            // var msg = new ChangeViewModelMsg(this, VMName);
            // var msg = new OpenPageMsg((ICbVM)Sender, VMName);
            // EventAggregator.PublishOnUIThread(msg);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        #endregion 기본 메소드

        #region Sample

        // ** 구글 크롬 전용 <CefSharp> 참고 자료
        // 참고 URL - https://blog.naver.com/codeknight2/222210163649

        // 참고 2 URL - https://github.com/cefsharp/CefSharp/wiki/Quick-Start

        #endregion Sample
    }
}
