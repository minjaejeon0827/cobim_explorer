using System.Windows;
using System.Windows.Media;

namespace CobimNet.Extensions
{
    // TODO : CobimExplorer 응용 프로그램 화면 상단 타이틀 바 비하인드 코드(TitleBarControl.xaml.cs)에 사용할 클래스 "FindParentExtension.cs" 구현(2023.11.09 minjae)  
    // 참고 URL - https://youtu.be/Q6VxSPlkcCg?si=dkC2ujAX3reUjQPk  

    public static class FindParentExtension
    {
        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2023.07.17 minjae)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2023.07.17 minjae)
        // 참고 URL - https://kukuta.tistory.com/359
        // C# Nullable 타입 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
        // C# Nullable 타입 예제 코드
#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고

        // 제네릭 타입 제약 문법 where T 
        // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
        public static T? FindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            return FindParent<T>(child, null);
        }


        /// <summary>
        /// 부모 윈도우 화면 찾기 - 형식 매개변수는 DependencyObject만 들어오도록 구현 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        // 제네릭 타입 제약 문법 where T 
        // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
        public static T? FindParent<T>(this DependencyObject child, string? parentName)
        where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
            // 참고 URL - https://husk321.tistory.com/405
            if (parent is null)
            {
                return null;
            }

            var frameworkElement = (FrameworkElement)parent;

            // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
            // 참고 URL - https://husk321.tistory.com/405
            // (부모 이름(parentName) 값이 null 또는  frameworkElement.Name == parentName)
            // && frameworkElement is T
            if ((parentName is null || frameworkElement.Name == parentName) && frameworkElement is T)
            {
                return (T)parent;
            }
            else
            {
                // 재귀함수 "FindParent" 호출 
                return FindParent<T>(parent, parentName);
            }
        }

#nullable disable  // nullable 전처리 비활성화
    }
}
