using Stylet;
using StyletIoC;

using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using CobimExplorer.Interface;
using CobimExplorer.Interface.Page;

namespace CobimExplorer.Services
{
    // TODO : 제네릭 클래스 CobimScreen<TData>에 속하는 프로퍼티 "SelectedData, InputParamData"가 null로 할당 안 하고 객체 생성(new()) 해주기 위해서 소스코드 "where TData : new()" 추가 (2024.01.05 minjae)
    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/programming-guide/generics/generic-classes

    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx

    // TODO : 추후 "CobimScreen" 클래스 구현 예정 (2023.12.14 minjae)
    [HiddenVm]
    //public abstract class CobimScreen<TData> : CobimScreen
    public abstract class CobimScreen<TData> : CobimScreen where TData : new()
    {
        #region 프로퍼티

        /// <summary>
        /// Rest API 입력 파라미터 데이터 객체 
        /// 로그인 등등...
        /// </summary>
        public TData InputParamData { get => _InputParamData; set { SetAndNotify(ref _InputParamData, value); } }
        private TData _InputParamData = new();


        #endregion 프로퍼티 

        #region 생성자

        public CobimScreen(IContainer container) : base(container)
        {

        }

        #endregion 생성자
    }

    // TODO : 추후 "CobimScreen" 클래스 구현 예정 (2023.12.14 minjae)
    // TODO : 참고 사항 Conductor<IPage>.Collection.OneActive의 부모 클래스는 Screen 클래스 인 것으로 확인 (2024.01.04 minjae)
    [HiddenVm]
    public abstract class CobimScreen : Conductor<IPage>.Collection.OneActive, ICbVM, IHandle, IDisposable
    {
        #region 프로퍼티 

        public IContainer Container { get; set; }

        [Inject]
        public IWindowManager WindowManager { get; set; }

        [Inject]
        public IEventAggregator EventAggregator { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        // TODO : 필요시 아래 생성자에 주석 처리한 : base(container) 사용 예정 (2023.12.04 minjae)
        public CobimScreen(IContainer container) //: base(container)
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
        }

        protected override void OnClose()
        {
            base.OnClose();
            this.EventAggregator?.Unsubscribe((IHandle)this);
        }

        // TODO : 화면 뷰(V.xaml) 리소스 해제 메서드 "Dispose" 필요시 구현 예정 (2024.09.25 minjae)
        // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/standard/garbage-collection/implementing-dispose
        // 참고 2 URL - https://just-my-blog.tistory.com/55
        // 참고 3 URL - https://huiyu.tistory.com/entry/C-%ED%91%9C%EC%A4%80-Dispose-%ED%8C%A8%ED%84%B4
        public void Dispose()
        {

        }

        public new void NotifyOfPropertyChange([CallerMemberName] string name = "") => base.NotifyOfPropertyChange(name);

        public new void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property) => base.NotifyOfPropertyChange<TProperty>(property);

        public new bool SetAndNotify<T>(ref T field, T value, [CallerMemberName] string propertyName = "") => base.SetAndNotify<T>(ref field, value, propertyName);

        #endregion 기본 메소드
    }
}
