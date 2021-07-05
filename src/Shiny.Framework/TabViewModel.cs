using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Prism;


namespace Shiny
{
    public abstract class TabViewModel : BaseViewModel, IActiveAware
    {
        protected TabViewModel()
        {
            this.WhenActiveChanged()
                .Subscribe(active =>
                {
                    if (!active)
                        this.Deactivate();

                    this.OnActiveChanged(active);
                })
                .DisposeWith(this.DestroyWith);
        }


        protected virtual void OnActiveChanged(bool active) {}


        protected IObservable<bool> WhenActiveChanged() => Observable.Create<bool>(ob =>
        {
            var handler = new EventHandler((sender, args) => ob.OnNext(this.IsActive));
            this.IsActiveChanged += handler;
            return () => this.IsActiveChanged -= handler;
        });


        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;
    }
}
