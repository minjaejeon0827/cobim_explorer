using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Runtime.InteropServices;

using CobimExplorer.Common.CobimBase;
using CobimExplorer.Data.Models.CobimBase.Main;

namespace CobimExplorer.Views.Windows.Test
{
    // TODO : 탐색기 응용 프로그램 메인 화면 뷰(MainV.xaml) 상단 윈도우 타이틀 바에 연동되는 이벤트 메서드들을 Behind Code(MainV.xaml.cs) 소스파일에 직접 구현 (2023.11.09 minjae) 
    // 참고 URL - https://gigong.tistory.com/99  
    // 참고 2 URL - https://itpro.tistory.com/88

    // TODO : 아래 Behind Code(MainV.xaml.cs) 소스파일에 직접 구현된 이벤트 메서드들을 Behavior 클래스로 만들어서 MainV.xaml에 연동할 수 있도록 구현하기 (2023.11.09 minjae)

    /// <summary>
    /// TestMainV.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestMainV : Window
    {
        #region 프로퍼티 

        // TODO : 필요시 프로퍼티 "startPos", "screens" 사용 예정 (2023.12.11 minjae)
        // private Point startPos;

        // TODO : DLL 파일 추가시 "System.Windows.dll" "System.Windows.Forms.dll" 2가지 dll 파일은 이름은 비슷하지만 서로 하는 기능이 다르고, 
        //        2가지 dll 파일을 모두 설치하면 충돌이 발생하여 아래의 소스코드가 오류가 발생하므로
        //        "System.Windows.dll" 파일은 프로젝트 파일 "CobimExplorer"에서 참조 제거 후 "System.Windows.Forms.dll" 파일만 참조 추가 진행 (2023.11.08 minjae)
        // System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;

        #endregion 프로퍼티

        #region 생성자

        public TestMainV()
        {
            // TODO : 오류 코드 "cs0103" 오류 메시지 'initializecomponent' 이름이 현재 컨텍스트에 없습니다. 해결 (2023.11.14 minjae)
            // 참고 URL - https://blog.naver.com/rtrue/220882338518
            InitializeComponent();


            // 이벤트 메서드(핸들러) "Window_LocationChanged" 2번 태우기 (이벤트 메서드(핸들러) 중복 방지)
            // 참고 URL - https://mentum.tistory.com/602
            // 참고 2 URL - https://stackoverflow.com/questions/937181/c-sharp-pattern-to-prevent-an-event-handler-hooked-twice

            // 이벤트 메서드(핸들러) "Window_LocationChanged" 2번 태우는 이유? (이벤트 메서드(핸들러) 중복 방지)
            // 1. 메모리 효율적으로 관리 목적
            // 2. 이벤트 메서드(핸들러), Action(델리게이트 - delegate)은 꽤 많이 사용하는 편인데
            // 가끔씩 초기화 코드가 중복되면서 이벤트 메서드(핸들러)가 중복 호출되는 문제가 발생하기 쉽다. 
            // 그런데 이런 휴먼에러가 발생하면 이벤트 메서드(핸들러) 쪽은 이원화되어 있기 때문에 오류를 발견하기 어렵다.
            // 따라서 이번 오류를 해결하기 위해 언제나 초기화 되기 전에
            // 제거(this.LocationChanged -= Window_LocationChanged;)
            // 부터 진행한다.
            // this.LocationChanged -= Window_LocationChanged;
            // this.LocationChanged += Window_LocationChanged;

            // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
            // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
            // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
            StateChanged -= Window_StateChanged;
            StateChanged += Window_StateChanged;


            // this.DataContext = new MainVM();   // View(MainV)와 ViewModel(MainVM) 연결, 참고 URL - https://github.com/canton7/Stylet/wiki/ViewModel-First
        }

        #endregion 생성자

        #region CanExecute

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion CanExecute

        #region Window_StateChanged

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// CobimExplorer 응용 프로그램 화면 창 최대/최소화 메서드 
        /// </summary>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            //if (WindowState == WindowState.Maximized)
            //{
            //    mainBorder.BorderThickness = new Thickness(8);
            //    RestoreButton.Visibility = Visibility.Visible;
            //    MaximizeButton.Visibility = Visibility.Collapsed;

            //    // TODO : 아래 if 조건절 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 오류 발생시 추후 로직 수정 예정 (2023.12.14 minjae)
            //    // 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 
            //    MainView.CommonData.BtnClickType = string.Empty;
            //    MainView.CommonData.BtnClickType = CobimHelper.IconBtnType;
            //}
            //else
            //{
            //    mainBorder.BorderThickness = new Thickness(0);
            //    RestoreButton.Visibility = Visibility.Collapsed;
            //    MaximizeButton.Visibility = Visibility.Visible;
            //}

            
            switch(WindowState)
            {
                //윈도우 작업 표시줄에서 CobimExplorer 응용 프로그램 아이콘을 클릭해서 화면 활성화 하기 전 CobimExplorer 화면 크기 상태 최대일 경우 
                case WindowState.Maximized:
                    mainBorder.BorderThickness = new Thickness(8);
                    RestoreButton.Visibility = Visibility.Visible;
                    MaximizeButton.Visibility = Visibility.Collapsed;

                    // TODO : 아래 if 조건절 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 오류 발생시 추후 로직 수정 예정 (2023.12.14 minjae)
                    // 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 
                    MainView.CommonData.BtnClickType = string.Empty;
                    MainView.CommonData.BtnClickType = CobimHelper.IconBtnType;
                    break;

                // 윈도우 작업 표시줄에서 CobimExplorer 응용 프로그램 아이콘을 클릭해서 화면 활성화 하기 전 CobimExplorer 화면 크기 상태 최대일 경우 
                case WindowState.Normal:
                    mainBorder.BorderThickness = new Thickness(0);
                    RestoreButton.Visibility = Visibility.Collapsed;
                    MaximizeButton.Visibility = Visibility.Visible;

                    // TODO : 아래 if 조건절 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 오류 발생시 추후 로직 수정 예정 (2023.12.14 minjae)
                    // 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 
                    MainView.CommonData.BtnClickType = string.Empty;
                    MainView.CommonData.BtnClickType = CobimHelper.IconBtnType;

                    break;

                case WindowState.Minimized:
                    mainBorder.BorderThickness = new Thickness(0);
                    RestoreButton.Visibility = Visibility.Collapsed;
                    MaximizeButton.Visibility = Visibility.Visible;

                    break;
            }
        }

        /// <summary>
        /// CobimExplorer 응용 프로그램 화면 창 최대/최소화 메서드 
        /// </summary>
        //private void Window_StateChanged(object sender, EventArgs e)
        //{
        //    if (this.WindowState == WindowState.Maximized)
        //    {
        //        this.mainBorder.BorderThickness = new Thickness(0);
        //        // this.mainBorder.Margin = new Thickness(7, 7, 7, 0);
        //        this.mainBorder.Margin = new Thickness(0);
        //        this.rectMax.Visibility = Visibility.Hidden;
        //        this.rectMin.Visibility = Visibility.Visible;
        //    }
        //    else if (this.WindowState == WindowState.Minimized)
        //    {
        //        this.mainBorder.BorderThickness = new Thickness(1);
        //        this.mainBorder.Margin = new Thickness(0);
        //        this.rectMax.Visibility = Visibility.Visible;
        //        this.rectMin.Visibility = Visibility.Hidden;
        //    }
        //}

        #endregion Window_StateChanged

        #region MinimizeApp

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// 화면 최소화
        /// </summary>
        private void MinimizeApp(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        #endregion MinimizeApp

        #region MaximizeApp

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// 화면 최대화
        /// </summary>
        private void MaximizeApp(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        #endregion MaximizeApp

        #region RestoreApp

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// 화면 축소
        /// </summary>
        private void RestoreApp(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        #endregion RestoreApp

        #region CloseApp

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// 종료 버튼 
        /// </summary>
        private void CloseApp(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        #endregion CloseApp

        #region Window_LocationChanged

        // TODO : CobimExplorer 응용 프로그램 화면 최대화시 화면 아래 작업 표시줄 보이도록 이벤트 메서드 "Window_LocationChanged" 구현 (2023.11.13 minjae)
        // 참고 URL -  https://gigong.tistory.com/102
        /// <summary>
        /// CobimExplorer 응용 프로그램 화면 최대화시 화면 아래 작업 표시줄 보이기 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Window_LocationChanged(object sender, EventArgs e)
        //{
        //    int sum = 0; 
        //    foreach (var item in screens) 
        //    { 
        //        sum += item.WorkingArea.Width; 
        //        if (sum >= this.Left + this.Width / 2) 
        //        { 
        //            this.MaxHeight = item.WorkingArea.Height; 
        //            break;
        //        }
        //    }
        //}

        #endregion Window_LocationChanged

        #region System_MouseDown

        //private void System_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //this.DragMove();
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        if (e.ClickCount >= 2)
        //        {
        //            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        //        }
        //        else
        //        {
        //            startPos = e.GetPosition(null);
        //        }
        //    }
        //    else if (e.ChangedButton == MouseButton.Right)
        //    {
        //        var pos = PointToScreen(e.GetPosition(this));
        //        IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        //        IntPtr hMenu = GetSystemMenu(hWnd, false);
        //        int cmd = TrackPopupMenu(hMenu, 0x100, (int)pos.X, (int)pos.Y, 0, hWnd, IntPtr.Zero);
        //        if (cmd > 0) SendMessage(hWnd, 0x112, (IntPtr)cmd, IntPtr.Zero);
        //    }
        //}

        #endregion System_MouseDown

        #region DllImport

        //[DllImport("PresentationCore.dll")]
        //static extern Point PointToScreen(Point point);

        //[DllImport("user32.dll")]
        //static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        //[DllImport("user32.dll")]
        //static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        //[DllImport("user32.dll")]
        //static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        #endregion DllImport

        #region System_MouseMove

        //private void System_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        if (this.WindowState == WindowState.Maximized && Math.Abs(startPos.Y - e.GetPosition(null).Y) > 2)
        //        {
        //            var point = PointToScreen(e.GetPosition(null));

        //            this.WindowState = WindowState.Normal;

        //            this.Left = point.X - this.ActualWidth / 2;
        //            this.Top = point.Y - mainBorder.ActualHeight / 2;
        //        }
        //        DragMove();
        //    }
        //}

        #endregion System_MouseMove

        #region MimimizeApp

        /// <summary>
        /// 화면 최소화
        /// </summary>
        //private void MinimizeApp(object sender, RoutedEventArgs e)
        //{
        //    this.WindowState = WindowState.Minimized;
        //}

        #endregion MimimizeApp

        #region MaximizeApp

        /// <summary>
        /// 화면 최대화
        /// </summary>
        //private void MaximizeApp(object sender, RoutedEventArgs e)
        //{
        //    this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        //}

        #endregion MaximizeApp

        #region ExitApp

        /// <summary>
        /// 종료 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ExitApp(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}

        #endregion ExitApp

        //private void PluScroll_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        //{
        //    e.Handled = true;
        //}
    }
}
