using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobimNet;

namespace CobimExplorer.Data.Models.CobimBase.Main
{
    // TODO : 필요시 MainView.cs 소스파일 수정 예정 (2023.12.14 jbh)
    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    public class MainView<TData> : MainView where TData : MainView<TData> { }

    /// <summary>
    /// CobimExplorer
    /// 메인 뷰모델(MainVM) 데이터 모델
    /// </summary>
    public class MainView : TMainView<MainView>
    {
        #region 프로퍼티 

        /// <summary>
        /// CobimExplorer 응용 프로그램 공통(공용) 데이터 
        /// </summary>
        public static MainView CommonData
        {
            get => MainView._CommonData ?? (MainView._CommonData = new MainView());
            set
            {
                MainView._CommonData = value;
                BindableBase.StaticChanged(nameof(CommonData));
            }
        }
        private static MainView _CommonData;

        // TODO : 필요시 리본 버튼 클릭 여부 프로퍼티 "IsRibbonBtnClick" 수정 예정 (2023.12.14 jbh)
        /// <summary>
        /// 리본 버튼 클릭 여부 
        /// </summary>
        // public bool IsRibbonBtnClick { get; set; }

        /// <summary>
        /// 버튼 클릭한 타입 (CobimExplorer 아이콘, 리본 버튼 등등...)
        /// </summary>
        public string BtnClickType { get; set; }

        #endregion 프로퍼티 
    }
}
