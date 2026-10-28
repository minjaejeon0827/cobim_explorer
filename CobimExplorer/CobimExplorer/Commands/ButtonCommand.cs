using System;
using System.Threading.Tasks;
using System.Windows.Input;

using CobimExplorer.Common.CobimBase;

namespace CobimExplorer.Commands
{
    // TODO: RelayCommand 추후 로직 변경 예정 (2023.07.03 jbh)
    // 참고 URL - https://www.codeproject.com/Tips/813345/Basic-MVVM-and-ICommand-Usage-Example
    // 참고 2 URL - https://youtu.be/s7pt3EkDyq4
    // 참고 3 URL - https://blog.naver.com/PostView.naver?blogId=goldrushing&logNo=221243250136 
    // Command 참고 URL - https://blog.naver.com/goldrushing/221243250136
    public class ButtonCommand : ICommand
    {
        #region 프로퍼티 

        // TODO : 반환형 void 메소드 타입 프로퍼티 "AsyncMethodType" 필요시 사용 예정(2023.10.18 jbh)
        /// <summary>
        /// 반환형 void 메소드 타입
        /// </summary>
        // public const string MethodType = "void";

        /// <summary>
        /// 비동기 메소드 타입
        /// </summary>
        // public const string AsyncMethodType = "async";

        /// <summary>
        /// 메소드 타입
        /// </summary>
        public string _methodType { get; set; }

        // TODO : 비동기 메소드 + 반환 타입이 void인 메소드 둘 다 구별해서 실행해야할 경우 추후 Action<object> _executeMethod; 사용 예정 (2023.10.11 jbh)
        // Action은 반환 타입이 void인 메소드를 위해 특별히 설계된 제네릭 델리게이트 의미
        // 참고 URL - https://blog.joe-brothers.com/csharp-delegate-func-action/
        Action<object> _executeVoidMethod;            // 반환 타입 void 메소드

        Func<object, Task> _executeAsyncMethod;   // 비동기 메소드

        Func<object, bool> _canexecuteMethod;

        // TODO : 추후 필요 시 이벤트 핸들러 "CanExecuteChanged" 사용 예정(2023.08.24 jbh)
        public event EventHandler CanExecuteChanged;

        #endregion 프로퍼티

        #region 생성자

        // TODO : 비동기 메소드 + 반환 타입이 void인 메소드 둘 다 구별해서 실행해야할 경우 추후 아래  반환 타입이 void인 메소드와 바인딩하는 "ButtonCommand" 생성자 사용 예정 (2023.10.11 jbh)
        // 반환 타입이 void인 메소드와 바인딩하는 "ButtonCommand" 생성자
        public ButtonCommand(string voidMethodType, Action<object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this._methodType        = voidMethodType;
            this._executeVoidMethod = executeMethod;
            this._canexecuteMethod  = canexecuteMethod;
        }

        // 비동기 메소드(async)와 바인딩하는 "ButtonCommand" 생성자
        public ButtonCommand(string asyncMethodType, Func<object, Task> executeAsyncMethod, Func<object, bool> canexecuteMethod)
        {
            this._methodType         = asyncMethodType;
            this._executeAsyncMethod = executeAsyncMethod;
            this._canexecuteMethod   = canexecuteMethod;
        }

        #endregion 생성자

        #region 기본 메소드

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // 비동기 메소드인 경우
            // _executeAsyncMethod(parameter);

            // 비동기 메소드인 경우 
            if (_methodType.Equals(CobimHelper.AsyncMethodType)) _executeAsyncMethod(parameter);

            // 반환 타입이 void인 메소드인 경우
            else _executeVoidMethod(parameter);

            // TODO : 비동기 메소드 + 반환 타입이 void인 메소드 둘 다 구별해서 실행해야할 경우 추후 로직 구현 예정 (2023.10.11 jbh)
            //var methodType = parameter.ToString();
            //// 비동기 메소드인 경우 
            //if (methodType.Equals(asyncMethod)) _executeAsyncMethod(parameter);
            //// 반환 타입이 void인 메소드인 경우
            //else _executeMethod(parameter);
        }

        #endregion 기본 메소드

        // TODO : 아래 소스코드 필요시 사용 예정 (2023.08.24 jbh)
        //private Action<object> execute;
        //private Func<object, bool> canExecute;

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value;  }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        //public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        //{
        //    this.execute = execute;
        //    this.canExecute = canExecute;
        //}

        /// <summary>
        /// RelayCommand 활성화 여부 제어 (활성화 - true / 비활성화 - false)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        //public bool CanExecute(object parameter)
        //{
              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
              // 참고 URL - https://husk321.tistory.com/405
        //    return canExecute is null || canExecute(parameter);
        //}

        //public void Execute(object parameter)
        //{
        //    execute(parameter);
        //}
    }


    //public class RelayCommand : ICommand
    //{
    //    private Action<object> execute;

    //    private Predicate<object> canExecute;

    //    private event EventHandler CanExecuteChangedInternal;

    //    public RelayCommand(Action<object> execute)
    //        : this(execute, DefaultCanExecute)
    //    {
    //    }

    //    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    //    {
              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
              // 참고 URL - https://husk321.tistory.com/405
    //        if (execute is null)
    //        {
    //            throw new ArgumentNullException("execute");
    //        }

              // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
              // 참고 URL - https://husk321.tistory.com/405
    //        if (canExecute is null)
    //        {
    //            throw new ArgumentNullException("canExecute");
    //        }

    //        this.execute = execute;
    //        this.canExecute = canExecute;
    //    }

    //    public event EventHandler CanExecuteChanged
    //    {
    //        add
    //        {
    //            CommandManager.RequerySuggested += value;
    //            this.CanExecuteChangedInternal += value;
    //        }

    //        remove
    //        {
    //            CommandManager.RequerySuggested -= value;
    //            this.CanExecuteChangedInternal -= value;
    //        }
    //    }

    //    public bool CanExecute(object parameter)
    //    {
    // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
    // 참고 URL - https://husk321.tistory.com/405
    //        return this.canExecute is not null && this.canExecute(parameter);
    //    }

    //    public void Execute(object parameter)
    //    {
    //        this.execute(parameter);
    //    }

    //    public void OnCanExecuteChanged()
    //    {
    //        EventHandler handler = this.CanExecuteChangedInternal;
    // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 jbh)
    // 참고 URL - https://husk321.tistory.com/405
    //        if (handler is not null)
    //        {
    //            //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
    //            handler.Invoke(this, EventArgs.Empty);
    //        }
    //    }

    //    public void Destroy()
    //    {
    //        this.canExecute = _ => false;
    //        this.execute = _ => { return; };
    //    }

    //    private static bool DefaultCanExecute(object parameter)
    //    {
    //        return true;
    //    }
    //}
}
