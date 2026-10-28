using CobimExplorer.Data.Models.CobimBase.Explorer;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Services.Part
{
    // TODO : 탐색기 화면(ExplorerV.xaml), 뷰모델(ExplorerVM.cs)에서 체크인 Part 화면 구현할 때, PartBase 클래스 구현하기 (2023.11.30 minjae)

    // TODO : 제네릭 클래스 PartBase<TData>에 속하는 프로퍼티 "SelectedData"가 null로 할당 안 하고 객체 생성(new()) 해주기 위해서 소스코드 "where TData : new()" 추가 (2024.01.05 minjae)
    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/programming-guide/generics/generic-classes
    //public abstract class PartBase<TData> : PartBase

    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    [HiddenVm]
    public abstract class PartBase<TData> : PartBase where TData : new()
    {
        #region 프로퍼티 

        // TODO : 데이터 타입 프로퍼티 "DataType" 구현 시 오류 발생
        //        오류 코드 "CS8370" - 오류 메시지 "'병합 할당' 기능(??= 연산)은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요."
        //        해당 오류 해결하기 위해 프로젝트 파일 "CobimExplorer"의 언어 버전 (기존) 7.3 -> (변경) 9.0  변경 완료 (20023.11.21 minjae)
        // '병합 할당' 기능 (??=) 연산 
        // null 병합 연산자 ??는 null이 아닌 경우 왼쪽 피연산자의 값을 반환한다. 그렇지 않으면 오른쪽 피연산자를 평가하고 그 결과를 반환한다.
        // 참고 URL - https://developstudy.tistory.com/69
        // 참고 2 URL - https://velog.io/@jinuku/C-%EB%B0%8F-.-%EC%97%B0%EC%82%B0%EC%9E%90
        /// <summary>
        /// 데이터 타입
        /// </summary>
        public Type DataType => _DataType ??= typeof(TData);
        private Type _DataType;

        // TODO : '병합 할당' 기능 (??=) 연산 대신 is not null 연산으로 구현한 데이터 타입 프로퍼티 "DataType" 필요 시 사용 예정 (2023.12.04 minjae)
        //public Type DataType
        //{
        //    get
        //    {
        //        Type dataType = this._DataType;
        //        // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.12.04 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        return (object)dataType is not null ? dataType : (this._DataType = typeof(TData));
        //    }
        //}
        //private Type _DataType;

        // TODO : BindableCollection<TData> 객체 Datas 필요시 사용 예정 (2024.01.05 minjae)
        // public BindableCollection<TData> Datas { get; } = new BindableCollection<TData>();

        // TODO : 필요시 Part - Rest API 입력 파라미터 데이터 모델 객체 프로퍼티 "InputParamData" 사용 예정 (2024.10.31 minjae)
        /// <summary>
        /// Part - Rest API 입력 파라미터 데이터 모델 객체 
        /// Part - 체크인 팝업화면 등등...
        ///// </summary>
        // public TData InputParamData { get => _InputParamData; set { SetAndNotify(ref _InputParamData, value); } }
        // private TData _InputParamData = new();


        //public TData SelectedData { get => _SelectedData; set { SetAndNotify(ref _SelectedData, value); } }
        //private TData _SelectedData;

        // TODO : 추후 필요시 프로퍼티 "SelectedData" 싱글톤 객체로 구현 예정 (2024.01.05 minjae)
        // public TData SelectedData { get => _SelectedData; set { SetAndNotify(ref _SelectedData, value); } }
        // private TData _SelectedData = new();

        // TODO : BindableCollection<TData> 객체 SelectedDatas 필요시 사용 예정 (2024.01.05 minjae)
        // public BindableCollection<TData> SelectedDatas { get; } = new BindableCollection<TData>();

        #endregion 프로퍼티

        #region 생성자 

        public PartBase(IContainer container) : base(container)

        {

        }

        #endregion 생성자

        #region OnQueryDataHeaders

        // TODO : 메서드 "OnQueryDataHeaders" 필요시 구현 예정 (2023.11.21 minjae)
        //public override IReadOnlyDictionary<string, TableColumnInfo> OnQueryDataHeaders()
        //{
        //    return App.Sys?.TableColumns?.GetDictionary<TData>();
        //}

        #endregion OnQueryDataHeaders    
    }

    // TODO : 아래 abstract "PartBase"에 상속 받을 수 있는 부모 클래스(CobimScreen.cs) 또는
    //        인터페이스 파일(CobimScreen, IPart, IHaveWorkDataSet, IDefaultFuncAbleVM<DefaultPartFunc>, IAppWinMsgEventArgs)
    //        필요 시 구현하기 (2023.12.04 minjae)
    // TODO :  부모 클래스(CobimScreen.cs) 구현시 아래 부모 클래스 Screen의 위치를 (기존) PartBase.cs -> (변경) CobimScreen.cs 변경 해야함. (2023.12.04 minjae)
    [HiddenVm]
    public abstract class PartBase : CobimScreen  // CobimScreen, Screen, IPart, IHaveWorkDataSet, IDefaultFuncAbleVM<DefaultPartFunc>, IAppWinMsgEventArgs
    {
        #region 프로퍼티 

        // TODO : Part 페이지 메뉴 정보 프로퍼티 "MenuInfo" 필요시 클래스 파일 "PartMenuInfo.cs" 구현 예정 (2023.12.04 minjae)
        /// <summary>
        /// Part 페이지 메뉴 정보 
        /// </summary>
        //public PartMenuInfo MenuInfo { get => _MenuInfo; set { SetAndNotify(ref _MenuInfo, value, nameof(MenuInfo)); } }
        //private PartMenuInfo _MenuInfo;


        #endregion 프로퍼티

        #region 생성자

        // TODO :  부모 클래스(CobimScreen.cs) 구현시 아래 생성자에 주석 처리한 : base(container) 사용 예정 (2023.12.04 minjae)
        public PartBase(IContainer container) : base(container)
        {

        }

        #endregion 생성자

        #region 기본 메소드

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            // TODO : 메서드 "OnQueryDataHeaders" 필요시 아래 코드 구현 예정 (2023.12.04 minjae)
            //DataHeaders = OnQueryDataHeaders();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        // TODO : 메서드 OnDeactivate 필요시 사용 예정 (2023.12.04 minjae)
        // protected override void OnDeactivate()
        // {
        //     base.OnDeactivate();
        // }


        // TODO : 메서드 OnClose 필요시 사용 예정 (2023.07.04 minjae)
        // protected override void OnClose()
        // {
        //     base.OnClose();
        //     // EventAggregator.Unsubscribe();
        // }


        // TODO : 메서드 OnViewLoaded 필요시 사용 예정 (2023.12.04 minjae)
        // protected override void OnViewLoaded()
        // {
        //     base.OnViewLoaded();
        // }


        // TODO : 메서드 CanExecute 필요시 사용 예정 (2023.12.04 minjae)
        //public bool CanExecute(object parameter)
        //{
        //    return true;
        //}


        // TODO : 메서드 Execute 필요시 사용 예정 (2023.12.04 minjae)
        //public abstract void Execute(object parameter);


        // TODO : 메서드 OnCanExecutedChanged 필요 시 사용 예정 (2023.12.04 minjae)
        //protected void OnCanExecutedChanged()
        //{
        //    CanExecuteChanged?.Invoke(this, new EventArgs());
        //}

        #endregion 기본 메소드 

        #region OnQueryDataHeaders

        // TODO : 메서드 "OnQueryDataHeaders" 필요 시 구현 예정 (2023.11.21 minjae)
        // public virtual IReadOnlyDictionary<string, TableColumnInfo> OnQueryDataHeaders() => null;

        #endregion OnQueryDataHeaders

        #region OnQueryCanDefaultFunc

        // TODO : 메서드 "OnQueryCanDefaultFunc" 필요 시 구현 예정 (2023.11.21 minjae)
        //public virtual bool OnQueryCanDefaultFunc(DefaultPartFunc funcType) => true;

        #endregion OnQueryCanDefaultFunc

        #region OnQueryDefaultFunc

        // TODO : 메서드 "OnQueryCanDefaultFunc" 필요 시 구현 예정 (2023.11.21 minjae)
        //public virtual void OnQueryDefaultFunc(DefaultPartFunc funcType)
        //{
        //}

        #endregion OnQueryDefaultFunc


    }

    #region Sample

    #endregion Sample
}
