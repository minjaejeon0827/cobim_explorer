using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CobimNet.Extensions;

namespace CobimExplorer.Controls
{
    // TODO : CobimExplorer 응용 프로그램 화면 상단 타이틀 바 비하인드 코드(TitleBarControl.xaml.cd) 구현(2023.11.09 minjae)  
    // 참고 URL - https://youtu.be/Q6VxSPlkcCg?si=dkC2ujAX3reUjQPk  

    /// <summary>
    /// TitleBarControl.xaml에 대한 상호 작용 논리
    /// </summary>
    //[ObservableObject]
    public partial class TitleBarControl : UserControl, INotifyPropertyChanged
    {
        #region 프로퍼티 

        // TODO : DLL 파일 추가시 "System.Windows.dll" "System.Windows.Forms.dll" 2가지 dll 파일은 이름은 비슷하지만 서로 하는 기능이 다르고, 
        //        2가지 dll 파일을 모두 설치하면 충돌이 발생하여 아래의 소스코드가 오류가 발생하므로
        //        "System.Windows.dll" 파일은 프로젝트 파일 "CobimExplorer"에서 참조 제거 후 "System.Windows.Forms.dll" 파일만 참조 추가 진행 (2023.11.08 minjae)
        System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;

        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2023.07.17 minjae)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2023.07.17 minjae)
        // 참고 URL - https://kukuta.tistory.com/359
        // C# Nullable 타입 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
        // C# Nullable 타입 예제 코드
#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고


        /// <summary>
        /// 부모 윈도우 - 싱글톤 패턴으로 구현 
        /// </summary>
        public Window ParentWindow
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                if (_ParentWindow is null)
                {
                    _ParentWindow = this.FindParent<Window>()!;
                }
                return _ParentWindow;
            }

            set { _ParentWindow = value; }
        }
        private Window? _ParentWindow;

#nullable disable  // nullable 전처리 비활성화

        // TODO : [ObservableObject] 사용 시 필요한 dll "CommunityToolkit.Mvvm" 설치 시 오류 발생
        //        또한 아래 프로퍼티 "ParentWindow"에 "SetProperty" 추가시 인식되지 않는 오류 추가 발생해서 
        //        INotifyPropertyChanged 인터페이스 상속 후 -> 아래처럼 "OnPropertyChanged" 이벤트 핸들러 사용 (2023.11.13 minjae)
        public WindowState WinState
        {
            get => _WinState;
            set
            {
                if (_WinState != value)
                {
                    _WinState = value;
                    OnPropertyChanged(nameof(WinState));
                }
            }
        }
        private WindowState _WinState;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion 프로퍼티

        #region 생성자

        public TitleBarControl()
        {
            InitializeComponent();

            // 이벤트 메서드(핸들러) "Click" 2번 태우기 (이벤트 메서드(핸들러) 중복 방지)
            // 참고 URL - https://mentum.tistory.com/602
            // 참고 2 URL - https://stackoverflow.com/questions/937181/c-sharp-pattern-to-prevent-an-event-handler-hooked-twice

            // 이벤트 메서드(핸들러) "Click" 2번 태우는 이유? (이벤트 메서드(핸들러) 중복 방지)
            // 1. 메모리 효율적으로 관리 목적
            // 2. 이벤트 메서드(핸들러), Action(델리게이트 - delegate)은 꽤 많이 사용하는 편인데
            // 가끔씩 초기화 코드가 중복되면서 이벤트 메서드(핸들러)가 중복 호출되는 문제가 발생하기 쉽다. 
            // 그런데 이런 휴먼에러가 발생하면 이벤트 메서드(핸들러) 쪽은 이원화되어 있기 때문에 오류를 발견하기 어렵다.
            // 따라서 이번 오류를 해결하기 위해 언제나 초기화 되기 전에
            // 제거(MinBtn.Click -= MinBtn_Click; / MaxBtn.Click -= MaxBtn_Click; / ExitBtn.Click -= ExitBtn_Click;)
            // 부터 진행한다.

            // 최소화 버튼 클릭 이벤트 가입
            MinBtn.Click -= MinBtn_Click;
            MinBtn.Click += MinBtn_Click;

            // 최대화 버튼 클릭 이벤트 가입
            MaxBtn.Click -= MaxBtn_Click;
            MaxBtn.Click += MaxBtn_Click;

            // 닫기 버튼 클릭 이벤트 가입 
            ExitBtn.Click -= ExitBtn_Click;
            ExitBtn.Click += ExitBtn_Click;
        }

        #endregion 생성자

        #region MinBtn_Click

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            // MinBtn.FindParent<Window>()!.WindowState = WindowState.Minimized;
            WinState = WindowState.Minimized;

            ParentWindow.WindowState = WinState;
        }

        #endregion MinBtn_Click

        #region MaxBtn_Click

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            // MaxBtn.FindParent<Window>()!.WindowState = WindowState.Maximized;
            // var window = MaxBtn.FindParent<Window>()!;

            // TODO : 삼항 연산자 사용해서 부모 최대화 버튼 상태(window.WindowState) 갱신 구현 (2023.11.13 minjae)
            // window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

            WinState = ParentWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

            ParentWindow.WindowState = WinState;

            // 보통 크기 -> 최대화 진행 (Rectangle 사각형 2개 중첩)
            if (ParentWindow.WindowState == WindowState.Maximized)
            {
                // window.WindowState = WindowState.Normal;
                this.mainTitle.BorderThickness = new Thickness(0);
                // this.mainTitle.Margin = new Thickness(7, 7, 7, 0);
                // this.mainTitle.Margin = new Thickness(0);
                this.rectMax.Visibility = Visibility.Hidden;
                this.rectMin.Visibility = Visibility.Visible;
            }
            // 최대화 -> 보통 크기로 축소 (Rectangle 사각형 1개)
            else if (ParentWindow.WindowState == WindowState.Normal)
            {
                // window.WindowState = WindowState.Maximized;
                this.mainTitle.BorderThickness = new Thickness(0);
                // this.mainTitle.Margin = new Thickness(0);
                //this.rectMax.Visibility = Visibility.Visible;
                //this.rectMin.Visibility = Visibility.Hidden;
                this.rectMax.Visibility = Visibility.Visible;
                this.rectMin.Visibility = Visibility.Hidden;
            }
        }

        #endregion MaxBtn_Click

        #region ExitBtn_Click

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // ExitBtn.FindParent<Window>()!.Close();
            ParentWindow.Close();
        }

        #endregion ExitBtn_Click
    }
}
