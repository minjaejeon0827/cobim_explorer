using System;
using System.Globalization;
using System.Windows.Data;

namespace CobimExplorer.Converters
{
    // TODO : 바인딩한 객체(프로퍼티)에 할당된 값이 True면 False, False면 True로 반대의 값으로 리턴되는 Converter 클래스 "InvertBooleanConverter.cs" 구현 (2024.09.03 minjae)
    // 참고 URL - https://codekiller.tistory.com/581
    // 참고 2 URL - https://devbong.tistory.com/41
    // 참고 3 URL - https://benstagram.tistory.com/203
    public class InvertBooleanConverter : IValueConverter
    {
        #region Convert

        /// <summary>
        /// 뷰모델(ViewModel) -> 뷰(View)로 
        /// 뷰모델(ViewModel)에 구현된 프로퍼티에 할당된 값을 
        /// True면 False, False면 True로 반대의 값으로 리턴
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 매개변수 value에 전달된 바인딩한 객체(프로퍼티)의 자료형이 bool 형인 경우 
            if(true == (value is bool))
            {
                return !(bool)value;   // 기존 매개변수 value 값과 반대의 값으로 리턴
            }

            return value;
        }

        #endregion Convert

        #region ConvertBack

        /// <summary>
        /// 뷰(View) -> 뷰모델(ViewModel)로 
        /// 뷰모델(ViewModel)에 구현된 프로퍼티에 할당된 값을 
        /// True면 False, False면 True로 반대의 값으로 리턴
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.09.04 minjae)
        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    // 매개변수 value에 전달된 바인딩한 객체(프로퍼티)의 자료형이 bool 형인 경우 
        //    if(true == (value is bool))
        //    {
        //        return !(bool)value;   // 기존 매개변수 value 값과 반대의 값으로 리턴
        //    }

        //    return value;
        //}

        #endregion ConvertBack

        #region Sample

        #endregion Sample
    }
}
