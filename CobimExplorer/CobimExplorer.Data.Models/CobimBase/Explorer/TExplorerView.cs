using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobimExplorer.Data.Models.CobimBase.Explorer
{
    // TODO : 필요시 TExplorerView.cs 클래스 파일 구현 예정 (2023.11.21 jbh)

    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    public class TExplorerView<TData> : TExplorerView where TData : TExplorerView<TData> { }

    // TODO : 일단 임시로 아래 처럼 구현해서 사용 추후 필요시 CDEModelBase 클래스 구현 예정 (2023.11.15 jbh)    
    public class TExplorerView : CDEModelBase<TExplorerView>
    {
        #region 프로퍼티 

        #endregion 프로퍼티
    }
}
