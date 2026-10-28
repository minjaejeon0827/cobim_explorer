using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobimExplorer.Interface.Page;
using CobimExplorer.Interface;

namespace CobimExplorer.Services.Page
{
    // TODO : 클래스 ParentOnePageBase에 인터페이스 IPage를 직접 상속하면
    //        TabParentPageVM.cs -> 생성자 "TabParentPageVM"가 무한 호출되며,
    //        CobimScreen.cs -> 생성자 "CobimScreen" 실행시 오류 메시지 "System.StackOverflowException" 출력된다.
    //        결과적으로 해당 오류 메시지로 인하여 
    //        MainVM.cs -> 기본 메소드 "OnInitialActivate" -> 로그인 화면 출력 메서드 "OpenLogin"가 실행 안 되면서
    //        탐색기 응용 프로그램이 정상적으로 실행 안 되고 종료되는 오류 발생!!
    //        하여 해당 오류를 해결하기 위해서 클래스 ParentOnePageBase에 인터페이스 IPage를 직접 상속 안하고
    //        CbConductorOne<IPage> 제네릭 타입 형식으로 클래스 상속 받도록 구현 (2024.08.07 jbh)
    [HiddenVm]
    public class ParentOnePageBase : CbConductorOne<IPage>, IPageParent // IParentPage, IPage
    {
        #region 프로퍼티 

        #endregion 프로퍼티 

        #region 생성자 

        public ParentOnePageBase(IContainer container) : base(container)
        {

        }

        #endregion 생성자 

        #region AddPage

        /// <summary>
        /// TabItem 추가 
        /// </summary>
        public void AddPage(IPage page)
        {
            this.ActivateItem(page);
        }

        #endregion AddPage

        #region ClearPage

        /// <summary>
        /// TabItem 제거 
        /// </summary>
        public void ClearPage()
        {
            foreach (IPage page in this.Items.ToArray<IPage>())
            {
                if (page is IRequestClose requestClose)
                {
                    bool? dialogResult = new bool?();
                    requestClose.RequestClose(dialogResult);
                }
            }
        }

        #endregion ClearPage

        #region OnInitialActivate

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }

        #endregion OnInitialActivate

        #region OnClose

        protected override void OnClose()
        {
            base.OnClose();
            //foreach (IPage page in this.Items.ToArray<IPage>())
            //{
            //    if (page is IRequestClose requestClose)
            //    {
            //        bool? dialogResult = new bool?();
            //        requestClose.RequestClose(dialogResult);
            //    }
            //}
        }

        #endregion OnClose

        #region Sample

        #endregion Sample
    }
}
