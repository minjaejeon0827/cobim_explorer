using Stylet;
using StyletIoC;

using System.Windows;

using CobimExplorer.Interface.Page;
using CobimExplorer.Services.Page;

using CobimExplorer.Common.Extensions;
using CobimExplorer.ViewModels.Message;
using CobimExplorer.Interface;

namespace CobimExplorer.ViewModels.Page.Test
{
    public class TestVM : PageBase, IPage
    {
        #region 프로퍼티 

        #region TEST

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        // public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh)
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }

        #endregion TEST

        #region OnActivate

        // TODO : 아래 프로퍼티 "Sender" 필요시 참고 (2024.08.07 jbh)
        /// <summary>
        /// 이벤트 OnActivate 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        // private TestVM Sender { get; set; }

        #endregion OnActivate

        #endregion 프로퍼티 


        #region 생성자

        public TestVM(IEventAggregator events, IContainer container) : base(container)
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;
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
            //Sender = this;

            //var vm = Container.Get<TestVM>();
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

        #region TEST

        public void ShowMessageBox(object param)
        {
            // Debug.WriteLine("DoSomething called");
            //MessageBox.Show("StartViewModel called");
            //new CommonMsgBoxVM(container).Set("다운로드 실패", ex.Message).ShowDialog();
            // EventAggregator.PublishOnUIThread(msg);
        }


        public void DoSomething(object param)
        {
            // Debug.WriteLine("DoSomething called");
            //MessageBox.Show("TestViewModel called");
        }

        private bool CanDoSomething(object param)
        {
            return true;
        }

        //public override void Execute(object parameter)
        //{
        //    // throw new NotImplementedException();
        //}

        #endregion TEST

        #region Sample

        #endregion Sample
    }
}
