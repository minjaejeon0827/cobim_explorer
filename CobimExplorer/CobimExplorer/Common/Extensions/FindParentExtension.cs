using Serilog;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

using CobimExplorer.Common.LogManager;

namespace CobimExplorer.Common.Extensions
{
    // TODO : CobimExplorer 응용 프로그램 화면 부모 탭 화면 비하인드 코드(TabParentPageV.xaml.cd)에 사용할 클래스 "FindParentExtension.cs" 구현 (2024.08.01 minjae)  
    // 참고 URL - https://youtu.be/Q6VxSPlkcCg?si=dkC2ujAX3reUjQPk  

    public static class FindParentExtension
    {
        // TODO: 오류 코드 "CS8370" 오류 메시지 "'nullable 참조 형식' 기능은 C# 7.3에서 사용할 수 없습니다. 8.0 이상의 언어 버전을 사용하세요." .Net 8.0 이상 언어 버전 설치 (2024.08.01 minjae)
        // 참고 URL - https://drehzr.tistory.com/872
        // TODO : 경고 메시지 코드 "CS8632" 경고 메시지 "c# 경고 CS8632 nullable 참조 형식에 대한 주석은 코드에서 '#nullable' 주석 컨텍스트 내에만 사용되어야 합니다." 로직 보완 (2024.08.01 minjae)
        // 참고 URL - https://kukuta.tistory.com/359
        // C# Nullable 타입 참고 URL - https://www.csharpstudy.com/CSharp/CSharp-nullable.aspx
        // C# Nullable 타입 예제 코드
#nullable enable   // 참조 타입 변수라도 null을 대입하면 경고

        // 제네릭 타입 제약 문법 where T 
        // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
        //public static T? FindTabItemParent<T>(this DependencyObject child)
        //    where T : DependencyObject
        //{
        //    //return FindTabItemParent<T>(child, null);
        //    return FindTabItemParent<T>(child);
        //}


        /// <summary>
        /// TabItem의 부모 찾기 - 형식 매개변수는 DependencyObject만 들어오도록 구현 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        //public static T? FindTabItemParent<T>(this DependencyObject child, string? parentName)

        // 제네릭 타입 제약 문법 where T 
        // 참고 URL - http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
        public static T? FindTabItemParent<T>(this DependencyObject child)
        where T : DependencyObject
        {
            var currentMethod = MethodBase.GetCurrentMethod();   // 로그 기록시 현재 실행 중인 메서드 위치 기록

            try
            {
                var parentObject = VisualTreeHelper.GetParent(child);
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2024.08.01 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                if (parentObject is null) return null;

                //var frameworkElement = (FrameworkElement)parent;

                // 부모 객체가 T 타입인지 확인합니다.
                T parent = parentObject as T;

                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2024.08.01 minjae)
                // 참고 URL - https://husk321.tistory.com/405
                // (부모 이름(parentName) 값이 null 또는  frameworkElement.Name == parentName)
                // && frameworkElement is T
                //if ((parentName is null || frameworkElement.Name == parentName) && frameworkElement is T)
                if (parent is not null) return parent;

                else return FindTabItemParent<T>(parentObject);   // 재귀함수 "FindTabItemParent" 호출 
            }
            catch(Exception ex)
            {
                Log.Error(Logger.GetMethodPath(currentMethod) + Logger.errorMessage + ex.Message);
                // TODO : 필요 없으면 throw; 문 주석 처리 예정 (2024.08.08 minjae)
                throw;   // 오류 발생시 상위 호출자 예외처리 전달 throw
            }
        }

#nullable disable  // nullable 전처리 비활성화

        #region Sample

        #endregion Sample
    }
}
