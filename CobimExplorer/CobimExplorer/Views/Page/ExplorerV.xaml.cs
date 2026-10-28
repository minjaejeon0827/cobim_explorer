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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CobimExplorer.Views.Pages
{
    /// <summary>
    /// ExplorerV.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExplorerV : UserControl
    {
        public ExplorerV()
        {
            InitializeComponent();
        }


        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("테스트 진행 중...");
        }


        // TODO : 마우스 오른쪽 클릭 이벤트 메서드 "Btn_MouseRightButtonDown" 구현 (필요시 사용) (2023.11.02 minjae)
        // 참고 URL - https://yeko90.tistory.com/entry/c-wpf-%EB%A7%88%EC%9A%B0%EC%8A%A4-%EC%9D%B4%EB%B2%A4%ED%8A%B8%EB%8D%94%EB%B8%94%ED%81%B4%EB%A6%AD-%EC%98%A4%EB%A5%B8%EC%AA%BD%ED%81%B4%EB%A6%AD-%EC%99%BC%EC%AA%BD-%ED%81%B4%EB%A6%AD
        // 참고 2 URL - https://poorman.tistory.com/263
        //private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    // MessageBox.Show("마우스 오른쪽 클릭 : 향후 체크인/체크아웃 항목 구현 예정");
        //}
    }
}
