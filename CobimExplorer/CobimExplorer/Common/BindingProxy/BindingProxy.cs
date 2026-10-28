using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CobimExplorer.Common.BindingProxy
{
    // TODO : 탐색시 화면(ExplorerV.xaml) 우측 상단 ListView 영역 StackPanel 영역 안 컨텍스트 메뉴 "테스트", "로그인", "로그아웃" 체크시 뷰모델에 구현한 이벤트 메서드가 실행할 수 있도록 BindingProxy 구현(2023.11.14 minjae)  -->
    // 참고 URL - https://stackoverflow.com/questions/3583507/wpf-binding-a-contextmenu-to-an-mvvm-command  -->
    // 참고 2 URL - https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/  -->

    // TODO : BindingProxy.cs 구현 (2023.11.14 minjae)
    // 참고 URL - https://codekiller.tistory.com/575
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // TODO : 의존 프로퍼티 구현 (2023.11.14 minjae)
        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc.
        // 참고 URL - https://afsdzvcx123.tistory.com/entry/8%EC%9E%A5-WPF-%EC%9D%98%EC%A1%B4%ED%94%84%EB%A1%9C%ED%8D%BC%ED%8B%B0DependencyProperty-%EB%9E%80
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
