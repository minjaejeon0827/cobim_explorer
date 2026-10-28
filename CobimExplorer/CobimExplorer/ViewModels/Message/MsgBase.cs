using CobimExplorer.Interface;

namespace CobimExplorer.ViewModels.Message
{
    public class MsgBase
    {
        #region 프로퍼티 

        public ICbVM Sender { get; set; }

        #endregion 프로퍼티 

        #region 생성자

        public MsgBase(ICbVM sender)
        { 
            this.Sender = sender;
        }

        #endregion 생성자 

        #region Sample

        #endregion Sample
    }
}