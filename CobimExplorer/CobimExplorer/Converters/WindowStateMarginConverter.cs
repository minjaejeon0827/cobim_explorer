using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CobimExplorer.Converters
{
    public class WindowStateMarginConverter : IValueConverter
    {
        #region Convert

        /// <summary>
        /// 뷰모델(ViewModel) -> 뷰(View)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WindowState state = (WindowState)value;

            if (state == WindowState.Normal)
            {
                return new Thickness(0);    // WindowState.Normal인 경우 두 께 변함 없어야 함. (Margin이 존재하지 않음.)
            }
            else
            {
                var param = (string)parameter;
                var right = param == "Exit" ? 7 : 0;
                return new Thickness(0, 7, right, 0);
            }
        }

        #endregion Convert

        #region ConvertBack

        /// <summary>
        /// 뷰(View) -> 뷰모델(ViewModel)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion ConvertBack

        #region Sample

        #endregion Sample
    }
}
