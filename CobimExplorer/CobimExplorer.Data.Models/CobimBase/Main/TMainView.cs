using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Data.Models.CobimBase.Main
{
    // TODO : 필요시 TMainView.cs 클래스 파일 구현 예정 (2023.11.21 jbh)
    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    public class TMainView<TData> : TMainView where TData : TMainView<TData> { }

    public class TMainView : CDEModelBase<TMainView>
    {
        #region 프로퍼티 

        #endregion 프로퍼티
    }
}
