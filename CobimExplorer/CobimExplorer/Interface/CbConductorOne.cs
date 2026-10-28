using Stylet;
using StyletIoC;

using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using CobimExplorer.Services;

namespace CobimExplorer.Interface
{
    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    [HiddenVm]
    public class CbConductorOne<TItem> :
        Conductor<TItem>.Collection.OneActive,
        ICbVM,
        IScreen,
        IViewAware,
        IHaveDisplayName,
        IScreenState,
        IChild,
        IGuardClose,
        IRequestClose,
        IHandle
        where TItem : class
    {
        #region 프로퍼티 

        public IContainer Container { get; set; }

        [Inject]
        public IWindowManager WindowManager { get; set; }

        [Inject]
        public IEventAggregator EventAggregator { get; set; }

        // TODO : 로그인 정보 프로퍼티 "LogInPackInfo" 필요시 사용 예정 (2024.01.15 jbh)
        //[Inject]
        //public LogInPack LogInPackInfo { get; set; }

        #endregion 프로퍼티

        #region 생성자 

        public CbConductorOne(IContainer container)
        {
            this.Container = container;
            this.Container?.BuildUp((object)this);
        }

        #endregion 생성자

        #region 기본 메소드 

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            this.EventAggregator.Subscribe((IHandle)this);
            CommandManager.RequerySuggested -= new EventHandler(this.CommandManager_RequerySuggested);
            CommandManager.RequerySuggested += new EventHandler(this.CommandManager_RequerySuggested);
        }

        protected override void OnClose()
        {
            base.OnClose();
            CommandManager.RequerySuggested -= new EventHandler(this.CommandManager_RequerySuggested);
            this.EventAggregator?.Unsubscribe((IHandle)this);
            // this.SaveManagedStatus();
        }

        #endregion 기본 메소드

        #region CommandManager_RequerySuggested

        private void CommandManager_RequerySuggested(object sender, EventArgs e) => this.OnUserAnyAction();

        #endregion CommandManager_RequerySuggested

        #region OnUserAnyAction

        // TODO : 필요시 메서드 "OnUserAnyAction" 구현 예정 (2024.01.12 jbh)
        public virtual void OnUserAnyAction()
        {

        }

        #endregion OnUserAnyAction

        #region SaveManagedStatus

        // TODO : 필요시 메서드 "SaveManagedStatus" 구현 예정 (2024.01.12 jbh)
        // public void SaveManagedStatus() => this.App.Local.Status.SaveManagedStatus<CbConductorOne<TItem>>(this);

        #endregion SaveManagedStatus


        #region 인터페이스 ICbVM.cs 메소드 구현

        public new void NotifyOfPropertyChange([CallerMemberName] string name = "") => base.NotifyOfPropertyChange(name);

        public new void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property) => base.NotifyOfPropertyChange<TProperty>(property);

        public new bool SetAndNotify<T>(ref T field, T value, [CallerMemberName] string propertyName = "") => base.SetAndNotify<T>(ref field, value, propertyName);

        #endregion 인터페이스 ICbVM.cs 메소드 구현
    }
}
