using System.ComponentModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CobimExplorer.Common.CobimBase;
using CobimNet.Extensions;

using Microsoft.Xaml.Behaviors;

namespace CobimExplorer.Behaviors
{
    // TODO :  메인 뷰(MainV.xaml) -> 화면 상단 타이틀 바 버튼("최소화", "최대화", "종료") 3가지
    //         각각의 Command 속성에 바인딩할 의존 프로퍼티 Behavior 클래스 "TitleBarButtonBehavior.cs"에 구현하기 (2023.11.15 jbh)
    // 참고 URL - https://kaki104.tistory.com/841

    // TODO : 메인 뷰(MainV.xaml) -> 화면 상단 타이틀 바 버튼 Click 이벤트 Behavior 클래스 "TitleBarButtonBehavior.cs"로 구현 예정 (2023.11.10 jbh)
    // 참고 URL - https://gapal.tistory.com/34
    public class TitleBarButtonBehavior : Behavior<Button>, INotifyPropertyChanged // AttachableForStyleBehavior<Button, TitleBarButtonBehavior>
    {

        #region OnAttached

        protected override void OnAttached()
        {
            base.OnAttached();
            var commandType = AssociatedObject.CommandParameter.ToString();

            switch (commandType)
            {
                // 최소화
                case CobimHelper.MinBtn:
                    AssociatedObject.Click -= MinBtn_Click;
                    AssociatedObject.Click += MinBtn_Click;
                    break;
                // 최대화
                case CobimHelper.MaxBtn:
                    AssociatedObject.Click -= MaxBtn_Click;
                    AssociatedObject.Click += MaxBtn_Click;
                    break;
                // 종료
                case CobimHelper.ExitBtn:
                    AssociatedObject.Click -= ExitBtn_Click;
                    AssociatedObject.Click += ExitBtn_Click;
                    break;
            }
        }

        #endregion OnAttached

        #region OnDetaching

        protected override void OnDetaching()
        {
            base.OnDetaching();
            var commandType = AssociatedObject.CommandParameter.ToString();

            switch (commandType)
            {
                // 최소화
                case CobimHelper.MinBtn:
                    AssociatedObject.Click -= MinBtn_Click;
                    break;
                // 최대화
                case CobimHelper.MaxBtn:
                    AssociatedObject.Click -= MaxBtn_Click;
                    break;
                // 종료
                case CobimHelper.ExitBtn:
                    AssociatedObject.Click -= ExitBtn_Click;
                    break;
            }
        }


        #endregion OnDetaching

        #region MinBtn_Click

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            // MinBtn.FindParent<Window>()!.WindowState = WindowState.Minimized;
            WinState = WindowState.Minimized;

            ParentWindow.WindowState = WinState;

            // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
            // 참고 URL - https://husk321.tistory.com/405
            if (MinCommand is not null)
            {
                MinCommand.Execute(e);
                e.Handled = true;
            }
        }

        #endregion MinBtn_Click

        #region MaxBtn_Click

        private void MaxBtn_Click(object sender, RoutedEventArgs e)
        {
            // MaxBtn.FindParent<Window>()!.WindowState = WindowState.Maximized;
            // var window = MaxBtn.FindParent<Window>()!;

            // TODO : 삼항 연산자 사용해서 부모 최대화 버튼 상태(window.WindowState) 갱신 구현 (2023.11.13 jbh)
            // window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

            WinState = ParentWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

            ParentWindow.WindowState = WinState;

            // var titleBar = (TitleBarControl)AssociatedObject.Content;
            //var test = titleBar.FindParent<Window>()!;

            // 보통 크기 -> 최대화 진행 (Rectangle 사각형 2개 중첩)
            if (ParentWindow.WindowState == WindowState.Maximized)
            {
                // titleBar.mainTitle.BorderThickness = new Thickness(0);
                // titleBar.rectMax.Visibility = Visibility.Hidden;
                // titleBar.rectMin.Visibility = Visibility.Visible;

                // window.WindowState = WindowState.Normal;
                // this.mainTitle.BorderThickness = new Thickness(0);
                // this.mainTitle.Margin = new Thickness(7, 7, 7, 0);
                // this.mainTitle.Margin = new Thickness(0);
                //this.rectMax.Visibility = Visibility.Hidden;
                //this.rectMin.Visibility = Visibility.Visible;
            }
            // 최대화 -> 보통 크기로 축소 (Rectangle 사각형 1개)
            else if (ParentWindow.WindowState == WindowState.Normal)
            {
                // titleBar.mainTitle.BorderThickness = new Thickness(0);
                // titleBar.rectMax.Visibility = Visibility.Visible;
                // titleBar.rectMin.Visibility = Visibility.Hidden;

                // window.WindowState = WindowState.Maximized;
                // this.mainTitle.BorderThickness = new Thickness(0);
                // this.mainTitle.Margin = new Thickness(0);
                //this.rectMax.Visibility = Visibility.Visible;
                //this.rectMin.Visibility = Visibility.Hidden;
                //this.rectMax.Visibility = Visibility.Visible;
                //this.rectMin.Visibility = Visibility.Hidden;
            }

            // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
            // 참고 URL - https://husk321.tistory.com/405
            if (MaxCommand is not null)
            {
                MaxCommand.Execute(e);
                e.Handled = true;
            }
        }

        #endregion MaxBtn_Click

        #region ExitBtn_Click

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // ExitBtn.FindParent<Window>()!.Close();
            ParentWindow.Close();

            // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
            // 참고 URL - https://husk321.tistory.com/405
            if (ExitCommand is not null)
            {
                ExitCommand.Execute(e);
                e.Handled = true;
            }
        }

        #endregion ExitBtn_Click


        #region 프로퍼티 

        // TODO : DLL 파일 추가시 "System.Windows.dll" "System.Windows.Forms.dll" 2가지 dll 파일은 이름은 비슷하지만 서로 하는 기능이 다르고, 
        //        2가지 dll 파일을 모두 설치하면 충돌이 발생하여 아래의 소스코드가 오류가 발생하므로
        //        "System.Windows.dll" 파일은 프로젝트 파일 "CobimExplorer"에서 참조 제거 후 "System.Windows.Forms.dll" 파일만 참조 추가 진행 (2023.11.08 jbh)
        System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;

        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2023.07.17 jbh)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2023.07.17 jbh)
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
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if (_ParentWindow is null)
                {
                    _ParentWindow = ChildControl.FindParent<Window>()!;
                }
                return _ParentWindow;
            }

            set { _ParentWindow = value; }
        }
        private Window? _ParentWindow;


        /// <summary>
        /// 자식 컨트롤 - 타이틀 바 (싱글톤 패턴으로 구현)
        /// </summary>
        public DependencyObject ChildControl
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if (_ChildControl is null)
                {
                    _ChildControl = (DependencyObject)AssociatedObject;
                }
                return _ChildControl;
            }
            set { _ChildControl = value; }
        }
        private DependencyObject? _ChildControl;


        public UserControl TestControl
        {
            get
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                if (_TestControl is null)
                {
                    _TestControl = (UserControl)AssociatedObject.Content;
                }
                return _TestControl;
            }
            set { _TestControl = value; }
        }
        private UserControl? _TestControl;

#nullable disable  // nullable 전처리 비활성화

        // TODO : [ObservableObject] 사용 시 필요한 dll "CommunityToolkit.Mvvm" 설치 시 오류 발생
        //        또한 아래 프로퍼티 "ParentWindow"에 "SetProperty" 추가시 인식되지 않는 오류 추가 발생해서 
        //        INotifyPropertyChanged 인터페이스 상속 후 -> 아래처럼 "OnPropertyChanged" 이벤트 핸들러 사용 (2023.11.13 jbh)
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
            // PropertyChanged 가 null이 아니면 Invoke를 호출한다
            // 참고 URL - https://m.cafe.daum.net/aspdotnet/6TQG/3149
            // 참고 2 URL - https://hwigyeom.ntils.com/3
            // 참고 3 URL - https://cafe.daum.net/aspdotnet/WJSu/65
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region 의존 프로퍼티

        // Using a DependencyProperty as the backing store for BoundPassword.  This enables animation, styling, binding, etc...
        // 로그인 화면 뷰(LoginV.xaml) -> 비밀번호 <PasswordBox> 영역에 프로퍼티 (utilsPassword:PasswordBoxHelper.UseOnPropertyChanged="True") 추가 
        // -> 프로그램 실행 -> 로그인 화면 뷰(LoginV.xaml)에서 비밀번호 텍스트 문자열 입력시(빈 문자열("" 또는 " ")도 포함)
        // -> TextBoxHelper.cs 소스파일 -> 의존 프로퍼티 "BoundPasswordProperty" -> 메서드 " DependencyProperty.RegisterAttached" 실행
        // -> 해당 메서드 파라미터 "new PropertyMetadata("<Default>", OnUseOnPropertyChanged)" 안에 존재하는 콜백(CallBack) 메서드 "OnUseOnPropertyChanged" 실행 

        // 메서드 "DependencyProperty.RegisterAttached" 파라미터 new PropertyMetadata("<Default>")로 입력하는 이유?
        // "<Default>"로 입력해야 비밀번호로 들어오는 문자열이 만약 빈 문자열("" 또는 " ")로 들어오더라도
        // 콜백(CallBack) 메서드 OnBoundPasswordChanged가 실행되고 -> 이벤트 메서드 "TextBox_TextChanged" 또한 실행되게 하기 위해서 이다.
        // public static readonly DependencyProperty BoundPasswordProperty =
        //     DependencyProperty.RegisterAttached("BoundPassword", typeof(string),
        //       typeof(PasswordBoxHelper), new PropertyMetadata("<Default>", OnBoundPasswordChanged));


        // TODO : 버튼 "최소화" 의존 프로퍼티 Behavior 클래스 "TitleBarButtonBehavior.cs"에 구현하기 (2023.11.15 jbh)
        // 참고 URL - https://kaki104.tistory.com/841
        public static readonly DependencyProperty MinCommandProperty =
            DependencyProperty.Register(nameof(MinCommand), typeof(ICommand), typeof(TitleBarButtonBehavior), new PropertyMetadata(null));

        // TODO : 버튼 "최대화" 의존 프로퍼티 Behavior 클래스 "TitleBarButtonBehavior.cs"에 구현하기 (2023.11.15 jbh)
        // 참고 URL - https://kaki104.tistory.com/841
        public static readonly DependencyProperty MaxCommandProperty =
            DependencyProperty.Register(nameof(MaxCommand), typeof(ICommand), typeof(TitleBarButtonBehavior), new PropertyMetadata(null));


        // TODO : 버튼 "종료" 의존 프로퍼티 Behavior 클래스 "TitleBarButtonBehavior.cs"에 구현하기 (2023.11.15 jbh)
        // 참고 URL - https://kaki104.tistory.com/841
        public static readonly DependencyProperty ExitCommandProperty =
            DependencyProperty.Register(nameof(ExitCommand), typeof(ICommand), typeof(TitleBarButtonBehavior), new PropertyMetadata(null));



        #endregion 의존 프로퍼티 

        #region Command

        /// <summary>
        /// 최소화 Command
        /// </summary>
        public ICommand MinCommand
        {
            get { return (ICommand)GetValue(MinCommandProperty); }
            set { SetValue(MinCommandProperty, value); }
        }

        /// <summary>
        /// 최대화 Command
        /// </summary>
        public ICommand MaxCommand
        {
            get { return (ICommand)GetValue(MaxCommandProperty); }
            set { SetValue(MaxCommandProperty, value); }
        }

        /// <summary>
        /// 종료 Command
        /// </summary>
        public ICommand ExitCommand
        {
            get { return (ICommand)GetValue(ExitCommandProperty); }
            set { SetValue(ExitCommandProperty, value); }
        }

        #endregion Command

        #endregion 프로퍼티 
    }
}
