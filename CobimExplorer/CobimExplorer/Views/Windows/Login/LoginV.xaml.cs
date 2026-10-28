using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CobimExplorer.Views.Windows.Login
{
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginV : Window
    {
        public LoginV()
        {
            InitializeComponent();
        }

        // TODO : 추후에 마우스 드래그 이벤트 메서드 "OnMouseLeftButtonDown"를 로그인 뷰모델 (LoginVM.cs)에 구현하기 (2023.12.07 jbh)
        /// <summary>
        /// 마우스 드래그 이벤트
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        
    }
}
