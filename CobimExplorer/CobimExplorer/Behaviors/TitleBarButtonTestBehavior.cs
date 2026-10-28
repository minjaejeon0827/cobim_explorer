using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CobimExplorer.Behaviors
{
    public class TitleBarButtonTestBehavior : AttachableForStyleBehavior<Button, TitleBarButtonTestBehavior>
    {
        // private readonly IDynamicResource _dr;

        public TitleBarButtonTestBehavior()
        {
            // Debug.WriteLine("Create TextBoxExBehavior");
            if (App.IsInDesignMode())
            {
                return;
            }
            // _dr = App.Current.Services.GetService(typeof(IDynamicResource)) as IDynamicResource;
        }

        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
            // 참고 URL - https://husk321.tistory.com/405
            //if (_dr is null) return;
            ////OnAttached가 UI Thread에서 실행되지 않기 때문에 Dispatcher 이용해서 출력
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            //    () =>
            //    {
            //        var text = _dr["wrd_Hello"];
            //        AssociatedObject.Text = text;
            //    });
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("AssociatedObject_GotFocus in TextBoxExBehavior");
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }
    }
}
