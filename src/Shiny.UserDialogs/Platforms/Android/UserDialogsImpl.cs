using System;
using System.Reactive;


namespace Shiny.UserDialogs
{
    public class UserDialogsImpl : IUserDialogs
    {
        readonly IPlatform platform;
        public UserDialogsImpl(IPlatform platform)
            => this.platform = platform;


        public IObservable<string?> ActionSheet(ActionSheetOptions options)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Alert(AlertOptions options)
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> Confirm(ConfirmOptions options)
        {
            throw new NotImplementedException();
        }

        public IObservable<PromptResult> Prompt(PromptOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

//using System;
//using Acr.UserDialogs.Builders;
//using Acr.UserDialogs.Fragments;
//using Acr.UserDialogs.Infrastructure;
//using Android.App;
//using Android.Text;
//using Android.Views;
//using Android.Widget;
//using Android.Text.Style;
//using AndroidHUD;
//#if ANDROIDX
//using AndroidX.AppCompat.App;
//using Google.Android.Material.Snackbar;
//#else
//using Android.Support.V7.App;
//using Android.Support.Design.Widget;
//#endif

        //public override IDisposable Alert(AlertConfig config)
        //{
        //    var activity = this.TopActivityFunc();
        //    if (activity is AppCompatActivity act)
        //        return this.ShowDialog<AlertAppCompatDialogFragment, AlertConfig>(act, config);

        //    return this.Show(activity, () => new AlertBuilder().Build(activity, config));
        //}


        //public override IDisposable ActionSheet(ActionSheetConfig config)
        //{
        //    var activity = this.TopActivityFunc();
        //    if (activity is AppCompatActivity act)
        //    {
        //        if (config.UseBottomSheet)
        //            return this.ShowDialog<Fragments.BottomSheetDialogFragment, ActionSheetConfig>(act, config);

        //        return this.ShowDialog<ActionSheetAppCompatDialogFragment, ActionSheetConfig>(act, config);
        //    }

        //    return this.Show(activity, () => new ActionSheetBuilder().Build(activity, config));
        //}


        //public override IDisposable Confirm(ConfirmConfig config)
        //{
        //    var activity = this.TopActivityFunc();
        //    if (activity is AppCompatActivity act)
        //        return this.ShowDialog<ConfirmAppCompatDialogFragment, ConfirmConfig>(act, config);

        //    return this.Show(activity, () => new ConfirmBuilder().Build(activity, config));
        //}


        //public override IDisposable DatePrompt(DatePromptConfig config)
        //{
        //    var activity = this.TopActivityFunc();
        //    if (activity is AppCompatActivity act)
        //        return this.ShowDialog<DateAppCompatDialogFragment, DatePromptConfig>(act, config);

        //    return this.Show(activity, () => DatePromptBuilder.Build(activity, config));
        //}



        //public override IDisposable Prompt(PromptConfig config)
        //{
        //    var activity = this.TopActivityFunc();
        //    if (activity is AppCompatActivity act)
        //        return this.ShowDialog<PromptAppCompatDialogFragment, PromptConfig>(act, config);

        //    return this.Show(activity, () => new PromptBuilder().Build(activity, config));
        //}


        //public override IDisposable TimePrompt(TimePromptConfig config)
        //{
        //    var activity = this.TopActivityFunc();
        //    if (activity is AppCompatActivity act)
        //        return this.ShowDialog<TimeAppCompatDialogFragment, TimePromptConfig>(act, config);

        //    return this.Show(activity, () => TimePromptBuilder.Build(activity, config));
        //}



        //protected virtual string ToHex(System.Drawing.Color color)
        //{
        //    var red = (int)(color.R * 255);
        //    var green = (int)(color.G * 255);
        //    var blue = (int)(color.B * 255);
        //    //var alpha = (int)(color.A * 255);
        //    //var hex = String.Format($"#{red:X2}{green:X2}{blue:X2}{alpha:X2}");
        //    var hex = String.Format($"#{red:X2}{green:X2}{blue:X2}");
        //    return hex;
        //}

//        protected virtual IDisposable Show(Activity activity, Func<Dialog> dialogBuilder)
//        {
//            Dialog dialog = null;
//            activity.SafeRunOnUi(() =>
//            {
//                dialog = dialogBuilder();
//                dialog.Show();
//            });
//            return new DisposableAction(() =>
//                activity.SafeRunOnUi(dialog.Dismiss)
//            );
//        }


//        protected virtual IDisposable ShowDialog<TFragment, TConfig>(AppCompatActivity activity, TConfig config) where TFragment : AbstractAppCompatDialogFragment<TConfig> where TConfig : class, new()
//        {
//            var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
//            activity.SafeRunOnUi(() =>
//            {
//                frag.Config = config;
//                frag.Show(activity.SupportFragmentManager, FragmentTag);
//            });
//            return new DisposableAction(() =>
//                activity.SafeRunOnUi(frag.Dismiss)
//            );
//        }

//        #endregion
//    }
//}