using Stylet;
using StyletIoC;

using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CobimExplorer.Interface
{
    public interface ICbVM : IScreen,
        IViewAware,
        IHaveDisplayName,
        IScreenState,
        IChild,
        IGuardClose,
        IRequestClose
    {
        IContainer Container { get; set; }

        IWindowManager WindowManager { get; set; }

        IEventAggregator EventAggregator { get; set; }

        void NotifyOfPropertyChange([CallerMemberName] string name = "");

        void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property);

        bool SetAndNotify<T>(ref T field, T value, [CallerMemberName] string propertyName = "");
    }
}
