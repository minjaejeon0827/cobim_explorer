using System;
using System.Windows;
using System.Windows.Input;

namespace CobimExplorer.Views
{
    // TODO : 탐색기 응용 프로그램 메인 화면 뷰(MainV.xaml) 상단 윈도우 타이틀 바에 연동되는 이벤트 메서드들을 Behind Code(MainV.xaml.cs) 소스파일에 직접 구현 (2023.11.09 minjae) 
    // 참고 URL - https://gigong.tistory.com/99  
    // 참고 2 URL - https://itpro.tistory.com/88

    // TODO : 아래 Behind Code(MainV.xaml.cs) 소스파일에 직접 구현된 이벤트 메서드들을 Behavior 클래스로 만들어서 MainV.xaml에 연동할 수 있도록 구현하기 (2023.11.09 minjae)

    /// <summary>
    /// MainV.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainV : Window
    {
        #region 프로퍼티 

        #endregion 프로퍼티 

        #region 생성자 

        public MainV()
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
        }

        #endregion 생성자 

        #region Window_StateChanged

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// CobimExplorer 응용 프로그램 화면 창 최대/원래대로/최소화 메서드 
        /// </summary>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                // 윈도우 작업 표시줄에서 최대화 버튼(WindowState.Maximized) 클릭해서 CobimExplorer 화면 크기 최대로 확대하는 경우  
                case WindowState.Maximized:
                    mainBorder.BorderThickness = new Thickness(8);
                    RestoreButton.Visibility = Visibility.Visible;
                    MaximizeButton.Visibility = Visibility.Collapsed;

                    // TODO : 아래 if 조건절 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 오류 발생시 추후 로직 수정 예정 (2023.12.14 minjae)
                    // 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 
                    // MainView.CommonData.BtnClickType = string.Empty;
                    // MainView.CommonData.BtnClickType = CobimHelper.IconBtnType;

                    break;

                // 윈도우 작업 표시줄에서 원래대로 버튼(WindowState.Normal) 클릭해서 CobimExplorer 화면 크기 원래대로 복구하는 경우 
                case WindowState.Normal:
                    mainBorder.BorderThickness = new Thickness(0);
                    RestoreButton.Visibility = Visibility.Collapsed;
                    MaximizeButton.Visibility = Visibility.Visible;

                    // TODO : 아래 if 조건절 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 오류 발생시 추후 로직 수정 예정 (2023.12.14 minjae)
                    // 윈도우 작업 표시줄 하단 CobimExplorer 아이콘을 클릭한 경우 
                    // MainView.CommonData.BtnClickType = string.Empty;
                    // MainView.CommonData.BtnClickType = CobimHelper.IconBtnType;

                    break;

                // 윈도우 작업 표시줄에서 최소화 버튼(WindowState.Minimized) 클릭해서 CobimExplorer 화면을 아래로 내리는 경우 
                case WindowState.Minimized:
                    mainBorder.BorderThickness = new Thickness(0);
                    RestoreButton.Visibility = Visibility.Collapsed;
                    MaximizeButton.Visibility = Visibility.Visible;

                    break;
            }
        }

        #endregion Window_StateChanged

        #region CanExecute

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion CanExecute

        #region MinimizeApp

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// 화면 아래로 내리기(축소)
        /// </summary>
        private void MinimizeApp(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        #endregion MinimizeApp

        #region RestoreApp

        // TODO : 타이틀 바 이벤트 메서드 구현(2023.12.11 minjae) 
        // 참고 URL - https://stackoverflow.com/questions/1283006/changing-wpf-title-bar-background-color  
        // 참고 2 URL - https://www.codeproject.com/Articles/5255192/Use-WindowChrome-to-Customize-the-Title-Bar-in-WPF  
        /// <summary>
        /// 화면 원래 크기로 변환
        /// </summary>
        private void RestoreApp(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        #endregion RestoreApp


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

        #region Sample

        #endregion Sample
    }
}
