using Serilog;
using Stylet;
using StyletIoC;

using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

using CobimExplorer.Commands;
using CobimExplorer.Common.CobimBase;
using CobimExplorer.Interface.Tab;

namespace CobimExplorer.Services.Page
{
    // TODO : 아래 유튜브 영상의 ViewModelBase.cs 클래스를 응용해서 PageBase.cs 클래스 구현 (2023.12.04 minjae)
    // 참고 유튜브 영상 URL -  https://youtu.be/2FPFgW0xVB0 (2023.12.04 minjae)

    // TODO : 필요시 바로 아래 주석친 코드처럼 (PageBase -> PageBase<TData>) 수정 예정 (2023.11.15 minjae)
    // public abstract class PageBase<TData> : PageBase {}

    // public abstract class PageBase : TestPageBase // Screen// , ICommand
    // public abstract class PageBase<TData> : PageBase

    // TODO : 제네릭 클래스 PageBase<TData>에 속하는 프로퍼티 "SelectedData, InputParamData"가 null로 할당 안 하고 객체 생성(new()) 해주기 위해서 소스코드 "where TData : new()" 추가 (2024.01.05 minjae)
    // 참고 URL - https://learn.microsoft.com/ko-kr/dotnet/csharp/programming-guide/generics/generic-classes

    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    [HiddenVm]
    public abstract class PageBase<TData> : PageBase where TData : new()
    {
        #region 프로퍼티 

        //public event EventHandler CanExecuteChanged;

        // TODO : 데이터 타입 프로퍼티 "DataType" 구현 시 오류 발생
        //        오류 코드 "CS8370" - 오류 메시지 "'병합 할당' 기능(??= 연산)은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요."
        //        해당 오류 해결하기 위해 프로젝트 파일 "CobimExplorer"의 언어 버전 (기존) 7.3 -> (변경) 9.0  변경 완료 (2023.11.21 minjae)
        // '병합 할당' 기능 (??=) 연산 
        // null 병합 연산자 ??는 null이 아닌 경우 왼쪽 피연산자의 값을 반환한다. 그렇지 않으면 오른쪽 피연산자를 평가하고 그 결과를 반환한다.
        // 참고 URL - https://developstudy.tistory.com/69
        // 참고 2 URL - https://velog.io/@jinuku/C-%EB%B0%8F-.-%EC%97%B0%EC%82%B0%EC%9E%90
        /// <summary>
        /// 데이터 타입
        /// </summary>
        public Type DataType => _DataType ??= typeof(TData);
        private Type _DataType;

        // TODO : '병합 할당' 기능 (??=) 연산 대신 싱글톤 패턴으로 구현한 데이터 타입 프로퍼티 "DataType" 필요 시 사용 예정 (2023.12.04 minjae)
        /// <summary>
        /// 데이터 타입
        /// </summary>
        //public Type DataType
        //{
        //    get
        //    {
        //        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        // 싱글톤 패턴 
        //        if (_DataType is null)
        //        {
        //            _DataType = typeof(TData);
        //        }
        //        return _DataType;
        //    }
        //    set => _DataType = value;
        //}
        //private Type _DataType;

        public BindableCollection<TData> Datas { get; } = new BindableCollection<TData>();

        /// <summary>
        /// 입력 파라미터 데이터 모델 객체 (Rest API 포함)
        /// 체크아웃, 체크인, 다운로드, 작업취소, 파일열기 등등...
        /// </summary>
        public TData InputParamData { get => _InputParamData; set { SetAndNotify(ref _InputParamData, value); } }
        private TData _InputParamData = new();

        // TODO : 추후 필요시 프로퍼티 "SelectedData" 싱글톤 객체로 구현 예정 (2024.01.05 minjae)
        // public TData SelectedData { get => _SelectedData; set { SetAndNotify(ref _SelectedData, value); } }
        // private TData _SelectedData = new();

        // TODO : 필요시 프로퍼티 "SelectedDatas" 참고 (2024.10.29 minjae)
        // public BindableCollection<TData> SelectedDatas { get; } = new BindableCollection<TData>();

        #endregion 프로퍼티 

        #region 생성자

        public PageBase(IContainer container) : base(container)
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

    // TODO : 아래 abstract "PageBase"에 상속 받을 수 있는 부모 클래스(CobimScreen.cs) 또는
    //        인터페이스 파일(IPage, IDefaultFuncAbleVM<DefaultPageFunc>, IAppWinMsgEventArgs, IHandle<MenuBookMarkChangeMsg>)
    //        필요 시 구현하기 (2023.12.04 minjae)
    // TODO :  부모 클래스(CobimScreen.cs) 구현시 아래 부모 클래스 Screen의 위치를 (기존) PageBase.cs -> (변경) CobimScreen.cs 변경 해야함. (2023.12.04 minjae)
    [HiddenVm]
    public abstract class PageBase : CobimScreen, ITab  // Screen, IPage, IDefaultFuncAbleVM<DefaultPageFunc>, IAppWinMsgEventArgs, IHandle<MenuBookMarkChangeMsg> 
    {
        #region 프로퍼티 

        // TODO : 페이지 메뉴 정보 프로퍼티 "MenuInfo" 필요시 클래스 파일 "PageMenuInfo.cs" 구현 예정 (2023.12.04 minjae)
        /// <summary>
        /// 페이지 메뉴 정보 
        /// </summary>
        //public PageMenuInfo MenuInfo { get => _MenuInfo; set { SetAndNotify(ref _MenuInfo, value); } }
        //private PageMenuInfo _MenuInfo;

        // TODO : ListView 데이터 로우 갯수 프로퍼티 "DisplaySearchCount" 필요시 사용 예정 (2023.12.04 minjae)
        /// <summary>
        /// ListView 데이터 로우 갯수 
        /// </summary>
        public int DisplaySearchCount { get => _DisplaySearchCount; set { _DisplaySearchCount = value; NotifyOfPropertyChange(); NotifyOfPropertyChange(nameof(DisplaySearchCountDesc)); } }
        private int _DisplaySearchCount = 0;

        // TODO : ListView 데이터 로우 갯수 Description 프로퍼티 "DisplaySearchCountDesc" 필요시 사용 예정 (2023.12.04 minjae)
        /// <summary>
        /// ListView 데이터 로우 갯수 Description
        /// </summary>
        public string DisplaySearchCountDesc => $"[{DisplaySearchCount.ToString("#,##0")}] 건 조회";

        // TODO : 즐겨찾기 - 북마크(노란색 별표) 프로퍼티 "IsBookMark" 필요시 사용 예정 (2023.12.04 minjae)
        /// <summary>
        /// 즐겨찾기 - 북마크
        /// </summary>
        public bool IsBookMark { get => _IsBookMark; set { _IsBookMark = value; NotifyOfPropertyChange(); } }
        private bool _IsBookMark = false;

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

        // TODO :  필요시 아래 생성자에 주석 처리한 : base(container) 사용 예정 (2023.12.04 minjae)
        public PageBase(IContainer container) : base(container)
        {
            CloseCommand = new ButtonCommand(CobimHelper.VoidMethodType, CloseTab, CanExecuteMethod);
        }

        #endregion 생성자

        #region 기본 메소드

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
            // TODO : 메서드 "OnQueryDataHeaders" 필요시 아래 코드 구현 예정 (2023.12.04 minjae)
            //DataHeaders = OnQueryDataHeaders();
            //DisplayName = menuInfo.Name;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // TODO : 즐겨찾기 - 북마크 필요시 아래 코드 구현 예정 (2023.12.04 minjae)
            // 즐겨찾기 등록되어있는지 체크
            // IsBookMark = App.User.MenuBookMarkList.Where(x => x.cde_MenuCode.Equals(menuInfo.Code)).FirstOrDefault() is not null;
        }

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }


        // TODO : 메서드 OnDeactivate 필요시 사용 예정 (2023.12.04 minjae)
        // protected override void OnDeactivate()
        // {
        //     base.OnDeactivate();
        // }


        // TODO : 메서드 OnClose 필요시 사용 예정 (2023.07.04 minjae)
        //protected override void OnClose()
        //{
        //    base.OnClose();
        //    // EventAggregator.Unsubscribe((IHandle) this);
        //}


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

        #region CloseTab

        private void CloseTab(object obj)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion CloseTab

        #region OnQueryDataHeaders

        // TODO : 메서드 "OnQueryDataHeaders" 필요 시 구현 예정 (2023.11.21 minjae)
        // public virtual IReadOnlyDictionary<string, TableColumnInfo> OnQueryDataHeaders() => null;

        #endregion OnQueryDataHeaders

        #region RegBookMarkAsync

        // TODO : 즐겨찾기 - 북마크 등록 메서드 "RegBookMarkAsync" 필요시 구현 예정 (2023.12.04 minjae)
        /// <summary>
        /// 즐겨찾기 - 북마크 등록 
        /// </summary>
        public async Task RegBookMarkAsync()
        {
            try
            {

            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, CobimHelper.ErrorTitle);
            }
        }

        #endregion RegBookMarkAsync
    }

    #region Sample

    #endregion Sample
}
