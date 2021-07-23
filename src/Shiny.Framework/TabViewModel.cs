using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Prism;
using ReactiveUI;


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


        bool active;
        public bool IsActive
        {
            get => this.active;
            set
            {
                if (this.active != value)
                {
                    this.active = value;
                    this.RaisePropertyChanged();
                    this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        public event EventHandler? IsActiveChanged;
    }
}
