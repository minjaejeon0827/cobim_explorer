using Stylet;
using CobimExplorer.Interface.Page;

namespace CobimExplorer.Services.Page
{
    public interface IPageParent
    {
        void AddPage(IPage page);

        void ClearPage();

        IObservableCollection<IPage> Items { get; }
    }
}
