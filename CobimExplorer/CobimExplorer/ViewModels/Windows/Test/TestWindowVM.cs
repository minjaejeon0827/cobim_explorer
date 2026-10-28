using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobimExplorer.Services.Page;

namespace CobimExplorer.ViewModels.Windows.Test
{
    public class TestWindowVM : PageBase
    {
        public TestWindowVM(IContainer container) : base(container)
        {

        }

        // Called by pressing the 'close' button
        public void Close()
        {
            this.RequestClose();
        }

        //public override void Execute(object parameter)
        //{
        //    // throw new NotImplementedException();
        //}
    }
}
