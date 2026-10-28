using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobimExplorer.Message;
using CobimExplorer.Services.Page;
using CobimExplorer.Interface.Page;

namespace CobimExplorer.ViewModels.Box
{
    // TODO : MessageBox 필요시 구현 예정(2023.07.26 jbh)
    public class CommonMsgBoxVM : PageBase, IPage
    {
        #region 프로퍼티 

        #endregion 프로퍼티 

        #region 생성자 

        public CommonMsgBoxVM(IEventAggregator events, IContainer container) : base(container)
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

        //public override void Execute(object parameter)
        //{
        //    // throw new NotImplementedException();
        //}

        #region Sample

        #endregion Sample
    }
}
