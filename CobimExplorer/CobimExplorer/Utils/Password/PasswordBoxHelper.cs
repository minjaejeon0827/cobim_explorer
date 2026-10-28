using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Drawing;

namespace CobimExplorer.Utils.Password
{
    // TODO : 로그인 뷰(LoginV.xaml) XAML 코드<PasswordBox> 영역 Hint(PlaceHolder와 같은 기능 - 반 투명 회색글자 "비밀번호를 입력하세요") 구현 (2023.08.23 minjae) 
    // TODO : PasswordBoxHelper.cs 에 의존 프로퍼티 IsMonitoring, PasswordLength 구현 (2023.08.23 minjae) 
    // 참고 URL - https://yimjang.tistory.com/entry/WPF-Textbox-Watermark-Passwordbox-%ED%8C%A8%EC%8A%A4%EC%9B%8C%EB%93%9C%EB%B0%95%EC%8A%A4%EC%97%90-%ED%9E%8C%ED%8A%B8-%EB%84%A3%EA%B8%B0 
    // 참고 2 URL - https://stackoverflow.com/questions/1607066/wpf-watermark-passwordbox-from-watermark-textbox 


    // TODO : Attached Property 기능 "PasswordBoxHelper.cs" 구현 (2023.08.23 minjae)
    // 유튜브 참고 URL - https://youtu.be/An7kwDYt3OQ
    public class PasswordBoxHelper
    {
        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
            // 참고 URL - https://husk321.tistory.com/405
            if (passwordBox is null) return;

            if ((bool)e.NewValue) passwordBox.PasswordChanged += PasswordChanged;

            else passwordBox.PasswordChanged -= PasswordChanged;

        }

        /// <summary>
        /// 콜백(CallBack) 메서드 OnBoundPasswordChanged - 이벤트 메서드 "PasswordChanged" 실행하는 기능 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
            // 참고 URL - https://husk321.tistory.com/405
            if (passwordBox is null) return;

            // 이벤트 메서드(핸들러) "PasswordChanged" 2번 태우기 (이벤트 메서드(핸들러) 중복 방지)
            // 참고 URL - https://mentum.tistory.com/602
            // 참고 2 URL - https://stackoverflow.com/questions/937181/c-sharp-pattern-to-prevent-an-event-handler-hooked-twice

            // 이벤트 메서드(핸들러) "PasswordChanged" 2번 태우는 이유? (이벤트 메서드(핸들러) 중복 방지)
            // 1. 메모리 효율적으로 관리 목적
            // 2. 이벤트 메서드(핸들러), Action(델리게이트 - delegate)은 꽤 많이 사용하는 편인데
            // 가끔씩 초기화 코드가 중복되면서 이벤트 메서드(핸들러)가 중복 호출되는 문제가 발생하기 쉽다. 
            // 그런데 이런 휴먼에러가 발생하면 이벤트 메서드(핸들러) 쪽은 이원화되어 있기 때문에 오류를 발견하기 어렵다.
            // 따라서 이번 오류를 해결하기 위해 언제나 초기화 되기 전에 제거(passwordBox.PasswordChanged -= PasswordChanged;) 부터 진행한다.
            passwordBox.PasswordChanged -= PasswordChanged;

            // TODO : 아래 passwordBox 객체의 프로퍼티 Password(passwordBox.Password)에 string 객체 newString 문자열 값을 할당 해주는 소스코드 필요시 사용 예정(2023.08.23)
            // PasswordBox에 비밀번호 Text가 새로 입력된 문자열(newString)을 passwordBox 객체의 프로퍼티 Password에 값 할당.
            //string newString = (string)e.NewValue ?? string.Empty;
            //if (newString != passwordBox.Password)
            //{
            //    passwordBox.Password = newString;
            //}

            // passwordBox.Password = (string)e.NewValue ?? string.Empty;

            passwordBox.PasswordChanged += PasswordChanged;
        }

        /// <summary>
        /// PasswordChanged 이벤트 메서드 
        /// PasswordBox에 비밀번호 Text가 입력될 때마다 바인딩된 프로퍼티(속성)에 입력된 비밀번호 문자열을 계속적으로 셋팅 해주는 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
            // 참고 URL - https://husk321.tistory.com/405
            if (passwordBox is null) return;

            //PasswordText = passwordBox.Password;

            SetBoundPassword(passwordBox, passwordBox.Password);
            SetPasswordLength(passwordBox, passwordBox.Password.Length);
        }


        

        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxHelper), new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxHelper), new UIPropertyMetadata(0));

        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }


        // Using a DependencyProperty as the backing store for BoundPassword.  This enables animation, styling, binding, etc...
        // 로그인 화면 뷰(LoginV.xaml) -> 비밀번호 <PasswordBox> 영역에 프로퍼티 (utilsPassword:PasswordBoxHelper.UseOnPropertyChanged="True") 추가 
        // -> 프로그램 실행 -> 로그인 화면 뷰(LoginV.xaml)에서 비밀번호 텍스트 문자열 입력시(빈 문자열("" 또는 " ")도 포함)
        // -> TextBoxHelper.cs 소스파일 -> 의존 프로퍼티 "BoundPasswordProperty" -> 메서드 " DependencyProperty.RegisterAttached" 실행
        // -> 해당 메서드 파라미터 "new PropertyMetadata("<Default>", OnUseOnPropertyChanged)" 안에 존재하는 콜백(CallBack) 메서드 "OnUseOnPropertyChanged" 실행 

        // 메서드 "DependencyProperty.RegisterAttached" 파라미터 new PropertyMetadata("<Default>")로 입력하는 이유?
        // "<Default>"로 입력해야 비밀번호로 들어오는 문자열이 만약 빈 문자열("" 또는 " ")로 들어오더라도
        // 콜백(CallBack) 메서드 OnBoundPasswordChanged가 실행되고 -> 이벤트 메서드 "TextBox_TextChanged" 또한 실행되게 하기 위해서 이다.
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string),
              typeof(PasswordBoxHelper), new PropertyMetadata("<Default>", OnBoundPasswordChanged));

        //public static readonly DependencyProperty WaterMarkTextProperty =
        //    DependencyProperty.RegisterAttached("WaterMarkPassword", typeof(string),
        //      typeof(PasswordBoxHelper), new PropertyMetadata(OnWaterMarkPassword));

        //public static readonly DependencyProperty WaterMarkTextColorProperty =
        //    DependencyProperty.RegisterAttached("WaterMarkPasswordColor", typeof(Brush),
        //      typeof(PasswordBoxHelper), new UIPropertyMetadata(Brushes.Gray));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }
    }


    // TODO : Password (비밀번호) 바인딩 처리해주는 클래스 "PasswordHelper" 필요시 사용 예정 (2023.08.23 minjae)
    // 참고 URL - https://opallios7.gitbooks.io/wpf-tutorials/content/wpf-passwordbox-control.html
    // 참고 2 URL - http://blog.functionalfun.net/2008/06/wpf-passwordbox-and-data-binding.html
    //public static class PasswordHelper
    //{
    //    public static readonly DependencyProperty BoundPassword =
    //      DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

    //    public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
    //        "BindPassword", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, OnBindPasswordChanged));

    //    private static readonly DependencyProperty UpdatingPassword =
    //        DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false));

    //    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        PasswordBox box = d as PasswordBox;

    //        // only handle this event when the property is attached to a PasswordBox
    //        // and when the BindPassword attached property has been set to true
              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
              // 참고 URL - https://husk321.tistory.com/405
    //        if (d is null || !GetBindPassword(d))
    //        {
    //            return;
    //        }

    //        // avoid recursive updating by ignoring the box's changed event
    //        box.PasswordChanged -= HandlePasswordChanged;

    //        string newPassword = (string)e.NewValue;

    //        if (!GetUpdatingPassword(box))
    //        {
    //            box.Password = newPassword;
    //        }

    //        box.PasswordChanged += HandlePasswordChanged;
    //    }

    //    private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    //    {
    //        // when the BindPassword attached property is set on a PasswordBox,
    //        // start listening to its PasswordChanged event

    //        PasswordBox box = dp as PasswordBox;

              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
              // 참고 URL - https://husk321.tistory.com/405
    //        if (box is null)
    //        {
    //            return;
    //        }

    //        bool wasBound = (bool)(e.OldValue);
    //        bool needToBind = (bool)(e.NewValue);

    //        if (wasBound)
    //        {
    //            box.PasswordChanged -= HandlePasswordChanged;
    //        }

    //        if (needToBind)
    //        {
    //            box.PasswordChanged += HandlePasswordChanged;
    //        }
    //    }

    //    private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
    //    {
    //        PasswordBox box = sender as PasswordBox;

    //        // set a flag to indicate that we're updating the password
    //        SetUpdatingPassword(box, true);
    //        // push the new password into the BoundPassword property
    //        SetBoundPassword(box, box.Password);
    //        SetUpdatingPassword(box, false);
    //    }

    //    public static void SetBindPassword(DependencyObject dp, bool value)
    //    {
    //        dp.SetValue(BindPassword, value);
    //    }

    //    public static bool GetBindPassword(DependencyObject dp)
    //    {
    //        return (bool)dp.GetValue(BindPassword);
    //    }

    //    public static string GetBoundPassword(DependencyObject dp)
    //    {
    //        return (string)dp.GetValue(BoundPassword);
    //    }

    //    public static void SetBoundPassword(DependencyObject dp, string value)
    //    {
    //        dp.SetValue(BoundPassword, value);
    //    }

    //    private static bool GetUpdatingPassword(DependencyObject dp)
    //    {
    //        return (bool)dp.GetValue(UpdatingPassword);
    //    }

    //    private static void SetUpdatingPassword(DependencyObject dp, bool value)
    //    {
    //        dp.SetValue(UpdatingPassword, value);
    //    }
    //}

    //public static class PasswordHelper
    //{
    //    public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

    //    public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

    //    public static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

    //    public static void SetAttach(DependencyObject dp, bool value)
    //    {
    //        dp.SetValue(AttachProperty, value);
    //    }

    //    public static bool GetAttach(DependencyObject dp)
    //    {
    //        return (bool)dp.GetValue(AttachProperty);
    //    }

    //    public static string GetPassword(DependencyObject dp)
    //    {
    //        return (string)dp.GetValue(PasswordProperty);
    //    }

    //    public static void SetPassword(DependencyObject dp, string value)
    //    {
    //        dp.SetValue(PasswordProperty, value);
    //    }

    //    private static bool GetIsUpdating(DependencyObject dp)
    //    {
    //        return (bool)dp.GetValue(IsUpdatingProperty);
    //    }

    //    private static void SetIsUpdating(DependencyObject dp, bool value)
    //    {
    //        dp.SetValue(IsUpdatingProperty, value);
    //    }

    //    private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        PasswordBox passwordBox = sender as PasswordBox;
    //        passwordBox.PasswordChanged -= PasswordChanged;

    //        if (!(bool)GetIsUpdating(passwordBox))
    //        {
    //            passwordBox.Password = (string)e.NewValue;
    //        }
    //        passwordBox.PasswordChanged += PasswordChanged;
    //    }

    //    private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        PasswordBox passwordBox = sender as PasswordBox;

              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
              // 참고 URL - https://husk321.tistory.com/405
    //        if (passwordBox is null)
    //            return;

    //        if ((bool)e.OldValue)
    //        {
    //            passwordBox.PasswordChanged -= PasswordChanged;
    //        }

    //        if ((bool)e.NewValue)
    //        {
    //            passwordBox.PasswordChanged += PasswordChanged;
    //        }
    //    }

    //    private static void PasswordChanged(object sender, RoutedEventArgs e)
    //    {
    //        PasswordBox passwordBox = sender as PasswordBox;
    //        SetIsUpdating(passwordBox, true);
    //        SetPassword(passwordBox, passwordBox.Password);
    //        SetIsUpdating(passwordBox, false);
    //    }
    //}
}
