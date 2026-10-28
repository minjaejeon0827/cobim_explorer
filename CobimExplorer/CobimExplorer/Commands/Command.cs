using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CobimExplorer.Commands
{
    // TODO : Command 클래스 구현시 참고 URL - https://blog.naver.com/PostView.naver?blogId=goldrushing&logNo=221243250136 (2023.07.12 jbh)
    public abstract class Command : ICommand
    {
        #region 프로퍼티 

        public event EventHandler CanExecuteChanged;

        #endregion 프로퍼티 

        #region 생성자 

        public Command()
        {

        }

        #endregion 생성자 


        #region 기본 메소드 

        /// <summary>
        /// Command 활성화 여부 제어 (활성화 - true / 비활성화 - false)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter) { 
            // 
        }

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        #endregion 기본 메소드 
    }
}
