using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace CobimExplorer.Behaviors
{
    /// <summary>
    /// 스타일에서 Behavior을 추가해서 사용할 수 있도록 지원합니다.
    /// 참고 URL - https://kaki104.tistory.com/841 
    /// </summary>
    /// <remarks>https://stackoverflow.com/questions/1647815/how-to-add-a-blend-behavior-in-a-style-setter</remarks>
    /// <typeparam name="TComponent"></typeparam>
    // 제네릭 타입 제약 문법 where T 
    // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    public class AttachableForStyleBehavior<TComponent, TBehavior> : Behavior<TComponent>
            where TComponent : DependencyObject
            where TBehavior : AttachableForStyleBehavior<TComponent, TBehavior>, new()
    {
        #region 생성자

        public AttachableForStyleBehavior()
        {

        }

        #endregion 생성자

        #region IsEnabledForStyle

        public static DependencyProperty IsEnabledForStyleProperty =
            DependencyProperty.RegisterAttached(nameof(IsEnabledForStyle), typeof(bool),
            typeof(AttachableForStyleBehavior<TComponent, TBehavior>), new FrameworkPropertyMetadata(false, OnIsEnabledForStyleChanged));

        public bool IsEnabledForStyle
        {
            get => (bool)GetValue(IsEnabledForStyleProperty);
            set => SetValue(IsEnabledForStyleProperty, value);
        }

        private static void OnIsEnabledForStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = d as UIElement;

            // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 minjae)
            // 참고 URL - https://husk321.tistory.com/405
            if (uie is not null)
            {
                BehaviorCollection behColl = Interaction.GetBehaviors(uie);
                TBehavior existingBehavior = behColl.FirstOrDefault(b => b.GetType() ==
                      typeof(TBehavior)) as TBehavior;

                // TODO : if 조건절에 != null 보다 빠른 is not null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                if ((bool)e.NewValue == false && existingBehavior is not null)
                {
                    behColl.Remove(existingBehavior);
                }
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                else if ((bool)e.NewValue == true && existingBehavior is null)
                {
                    behColl.Add(new TBehavior());
                }
            }
        }

        #endregion IsEnabledForStyle
    }
}
