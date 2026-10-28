using CobimExplorer.Interface.Page;
using CobimExplorer.Interface;

namespace CobimExplorer.ViewModels.Message
{
    public class OpenedPageMsg : MsgBase
    {
        public OpenedPageMsg(ICbVM sender) : base(sender)
        {

        }

        public IPage Page { get; set; }

        // TODO : 데이터 조회 건수 프로퍼티 "DisplaySearchCount" 필요시 구현 예정 (2024.01.10 minjae)
        // public int DisplaySearchCount { get; set; }

        // TODO : 타이틀 관련 프로퍼티 "DisplayTitle" 필요시 구현 예정 (2024.01.10 minjae)
        // public string DisplayTitle { get; set; }
    }
}