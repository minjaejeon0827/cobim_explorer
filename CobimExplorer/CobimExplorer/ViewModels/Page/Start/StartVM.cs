using Stylet;
using StyletIoC;
using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using CobimExplorer.Commands;
using System.Threading;
using CobimExplorer.ViewModels.Box;
using CobimExplorer.Services.Page;
using CobimExplorer.Interface.Page;
using CobimExplorer.Message;
using System.Collections.Generic;
using CobimExplorer.Common.Extensions;
using CobimExplorer.ViewModels.Message;
using CobimExplorer.Interface;

namespace CobimExplorer.ViewModels.Page.Start
{

    public class StartVM : PageBase, IPage
    {
        #region 프로퍼티 

        #region OnActivate

        // TODO : 아래 프로퍼티 "Sender" 필요시 참고 (2024.08.07 jbh)
        /// <summary>
        /// 이벤트 OnActivate 실행시 
        /// OpenPageMsg 클래스 객체에 전달할 객체 Sender 
        /// </summary>
        // private StartVM Sender { get; set; }

        #endregion OnActivate

        #region TEST

        public string TestName { get => _TestName; set { _TestName = value; NotifyOfPropertyChange(); } }
        private string _TestName = string.Empty;

        public bool IsTestOpenPopup { get => _IsTestOpenPopup; set => SetAndNotify(ref _IsTestOpenPopup, value); }
        private bool _IsTestOpenPopup;

        #endregion TEST

        #endregion 프로퍼티 

        // public StartViewModel TestViewModel { get; set; }

        // 참고 URL - https://velog.io/@esology/CWPF-MVVM-Command-Binding%EC%9D%84-%ED%86%B5%ED%95%B4-%EB%B2%84%ED%8A%BC-%EC%83%81%ED%83%9C-%EC%A0%9C%EC%96%B4%ED%95%98%EA%B8%B0
        //public WpfCommand CmdClick => Cmds.GetValueOrInsert(new WpfCommand()
        //{
        //    ExecuteAction = _ => DoSomething()
        //});

        public ICommand CmdClick { get; set; }

        // TODO : 필요시 프로퍼티 "Container" 사용 예정 (2024.01.04 jbh) 
        //public IContainer Container { get; set; }

        // TODO : 필요시 프로퍼티 "EventAggregator" 사용 예정 (2024.01.04 jbh) 
        //[Inject]
        //public IEventAggregator EventAggregator { get; set; }


        //public ICommand MsgCommand { get; set; }

        // TODO : RelayCommand 클래스 프로퍼티 구현 (2023.07.13 jbh)
        // 참고 URL - https://youtu.be/s7pt3EkDyq4
        // public RelayCommand MsgCommand => new RelayCommand(execute => DoSomething());

        // public RelayCommand MsgCommand { get; set; }

        #region 생성자

        //public StartViewModel() 
        //{
        //    // TODO: Button 컨트롤 Command Binding RelayCommand 구현 (2023.07.03 jbh)
        //    // 참고 URL - https://itpro.tistory.com/90
        //    CmdClick = new RelayCommand(DoSomething, CanDoSomething);
        //}


        //public StartViewModel(IContainer container) 
        //{
        //    TestViewModel = this;
        //}


        public StartVM(IEventAggregator events, IContainer container) : base(container) 
        {
            Container = container;

            // TODO : EventAggregator = events; 코드 추가 (2023.07.24 jbh)
            // 참고 URL - https://github.com/canton7/Stylet/wiki/3.-The-Taskbar
            EventAggregator = events;
            // MsgCommand = new RelayCommand(execute => DoSomething()); 
            // MsgCommand = new ViewModelCommand(ExecuteMethod, CanExecuteMethod);
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

            //var vm = Container.Get<StartVM>();
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

        //private void Timer_Tick(object sender, EventArgs e)
        //{

        //}

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
            //MessageBox.Show("StartViewModel called");
        }

        private bool CanDoSomething(object param)
        {
            return true;
        }

        //public override void Execute(object parameter)
        //{
        //    // throw new NotImplementedException();
        //}

        private void ExecuteMethod(object obj)
        {
            //MessageBox.Show("코드비하인드가 아닌 Command ExecuteMethod");
        }

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }

        #endregion TEST

        #region Sample

        #endregion Sample
    }
}
