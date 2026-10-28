using CobimExplorer.Interface;
using CobimExplorer.Interface.Page;

namespace CobimExplorer.ViewModels.Message
{
    public class OpenPageMsg : MsgBase
    {
        #region 프로퍼티 

        /// <summary>
        /// 탐색기 응용 프로그램 TabItem에 출력할 화면 정보
        /// </summary>
        public IPage Page { get; set; }

        // TODO : 즐겨찾기 기능 필요시 추가 예정 (2024.01.10 minjae)
        // public bool IsMenuBookMarkSelected { get; set; }

        //public int Lv4MenuBookMarkCode { get; set; }

        /// <summary>
        /// 탐색기 응용 프로그램 TabItem에 출력할 화면 뷰모델 이름 
        /// </summary>
        public string VMName { get; set; }

        // TODO : 필요시 프로퍼티 "Items" 사용 예정 (2024.08.06 minjae)
        //public ObservableCollection<IPage> Items
        //{
        //    get
        //    {
        //        // TODO : if 조건절에 == null 보다 빠른 is null 연산자 사용 (2023.11.24 minjae)
        //        // 참고 URL - https://husk321.tistory.com/405
        //        // 싱글톤 패턴 
        //        if (_Items is null)
        //        {
        //            _Items = new ObservableCollection<IPage>();
        //        }

        //        return _Items;
        //    }
        //    set => _Items = value;
        //}

        //private ObservableCollection<IPage> _Items;

        // TODO : 필요시 프로퍼티 "obj" 사용 예정 (2024.08.06 minjae)
        //public object obj { get; set; }

        #endregion 프로퍼티 

        #region 생성자 

        public OpenPageMsg(ICbVM sender) : base(sender)
        {

        }

        // TODO : 추후 필요시 아래 생성자 OpenPageMsg(ICbVM sender, string VMName) 참조해서 사용 예정 (2024.08.08 minjae)
        //public OpenPageMsg(ICbVM sender, string ViewModelName) : base(sender)
        public OpenPageMsg(ICbVM sender, string VMName) : base(sender)
        {
            this.Page = (IPage)sender;
            // obj = o;
            this.VMName = VMName;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }
}