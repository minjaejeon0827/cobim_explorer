using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobimExplorer.Interface.Page;

namespace CobimExplorer.Message
{
    public class ChangeViewModelMsg 
    {
        #region 프로퍼티 

        public string VMName { get; set; }

        public ObservableCollection<IPage> Items 
        {
            get 
            {
                // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 jbh)
                // 참고 URL - https://husk321.tistory.com/405
                // 싱글톤 패턴 
                if (_Items is null)
                {
                    _Items = new ObservableCollection<IPage>();
                }

                return _Items;
            }
            set => _Items = value;
        }

        private ObservableCollection<IPage> _Items;


        public object obj { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        public ChangeViewModelMsg(object o, string ViewModelName)
        {
            this.Items.Add((IPage)o);
            obj = o;
            VMName = ViewModelName;
        }

        #endregion 생성자 
    }
}
