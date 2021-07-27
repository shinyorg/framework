using System;
using System.Reactive;

using UIKit;

namespace Shiny.UserDialogs
{
    public class UserDialogsImpl : IUserDialogs
    {
        readonly IPlatform platform;
        public UserDialogsImpl(IPlatform platform)
            => this.platform = platform;


        public IObservable<string?> ActionSheet(ActionSheetOptions options)
        {
            //var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            //alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke()));
            //return alert;


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


//        public override IDisposable ActionSheet(ActionSheetConfig config) => this.Present(() => this.CreateNativeActionSheet(config));


//        public override IDisposable Confirm(ConfirmConfig config) => this.Present(() =>
//        {
//            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
//            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnAction?.Invoke(false)));
//            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke(true)));
//            return dlg;
//        });

//        public override IDisposable Prompt(PromptConfig config) => this.Present(() =>
//        {
//            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
//            UITextField txt = null;

//            if (config.IsCancellable)
//            {
//                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
//                    config.OnAction?.Invoke(new PromptResult(false, txt.Text)
//                )));
//            }

//            var btnOk = UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
//                config.OnAction?.Invoke(new PromptResult(true, txt.Text)
//            ));
//            dlg.AddAction(btnOk);

//            dlg.AddTextField(x =>
//            {
//                txt = x;
//                this.SetInputType(txt, config.InputType);
//                txt.Placeholder = config.Placeholder ?? String.Empty;
//                txt.Text = config.Text ?? String.Empty;
//                txt.AutocorrectionType = (UITextAutocorrectionType)config.AutoCorrectionConfig;

//                if (config.MaxLength != null)
//                {
//                    txt.ShouldChangeCharacters = (field, replacePosition, replacement) =>
//                    {
//                        var updatedText = new StringBuilder(field.Text);
//                        updatedText.Remove((int)replacePosition.Location, (int)replacePosition.Length);
//                        updatedText.Insert((int)replacePosition.Location, replacement);
//                        return updatedText.ToString().Length <= config.MaxLength.Value;
//                    };
//                }

//                if (config.OnTextChanged != null)
//                {
//                    txt.AddTarget((sender, e) => ValidatePrompt(txt, btnOk, config), UIControlEvent.EditingChanged);
//                    ValidatePrompt(txt, btnOk, config);
//                }
//            });
//            return dlg;
//        });


//        static void ValidatePrompt(UITextField txt, UIAlertAction btn, PromptConfig config)
//        {
//            var args = new PromptTextChangedArgs { Value = txt.Text };
//            config.OnTextChanged(args);
//            btn.Enabled = args.IsValid;
//            if (!txt.Text.Equals(args.Value))
//                txt.Text = args.Value;
//        }


        //protected virtual UIAlertController CreateNativeActionSheet(ActionSheetConfig config)
        //{
        //    var sheet = UIAlertController.Create(config.Title, config.Message, UIAlertControllerStyle.ActionSheet);

        //    config
        //        .Options
        //        .ToList()
        //        .ForEach(x => this.AddActionSheetOption(x, sheet, UIAlertActionStyle.Default, config.ItemIcon));

        //    if (config.Destructive != null)
        //        this.AddActionSheetOption(config.Destructive, sheet, UIAlertActionStyle.Destructive, config.ItemIcon);

        //    if (config.Cancel != null)
        //        this.AddActionSheetOption(config.Cancel, sheet, UIAlertActionStyle.Cancel, config.ItemIcon);

        //    return sheet;
        //}

        //protected virtual void AddActionSheetOption(ActionSheetOption opt, UIAlertController controller, UIAlertActionStyle style, string imageName)
        //{
        //    var alertAction = UIAlertAction.Create(opt.Text, style, x => opt.Action?.Invoke());

        //    if (opt.ItemIcon == null && imageName != null)
        //        opt.ItemIcon = imageName;

        //    if (opt.ItemIcon != null)
        //    {
        //        var icon = UIImage.FromBundle(opt.ItemIcon);
        //        alertAction.SetValueForKey(icon, new NSString("image"));
        //    }
        //    controller.AddAction(alertAction);
        //}

        //        protected virtual IDisposable Present(Func<UIAlertController> alertFunc)
        //        {
        //            UIAlertController alert = null;
        //            var app = UIApplication.SharedApplication;
        //            app.SafeInvokeOnMainThread(() =>
        //            {
        //                alert = alertFunc();
        //                var top = this.viewControllerFunc();
        //                if (alert.PreferredStyle == UIAlertControllerStyle.ActionSheet && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
        //                {
        //                    var x = top.View.Bounds.Width / 2;
        //                    var y = top.View.Bounds.Bottom;
        //                    var rect = new CGRect(x, y, 0, 0);
        //#if __IOS__
        //                    alert.PopoverPresentationController.SourceView = top.View;
        //                    alert.PopoverPresentationController.SourceRect = rect;
        //                    alert.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
        //#endif
        //                }
        //                top.PresentViewController(alert, true, null);
        //            });
        //            return new DisposableAction(() => app.SafeInvokeOnMainThread(() => alert.DismissViewController(true, null)));
        //        }


        //protected virtual IDisposable Present(UIViewController controller)
        //{
        //    var app = UIApplication.SharedApplication;
        //    var top = this.viewControllerFunc();

        //    app.SafeInvokeOnMainThread(() => top.PresentViewController(controller, true, null));
        //    return new DisposableAction(() => app.SafeInvokeOnMainThread(() => controller.DismissViewController(true, null)));
        //}


        //protected virtual void SetInputType(UITextField txt, InputType inputType)
        //{
        //    switch (inputType)
        //    {
        //        case InputType.DecimalNumber:
        //            txt.KeyboardType = UIKeyboardType.DecimalPad;
        //            break;

        //        case InputType.Email:
        //            txt.KeyboardType = UIKeyboardType.EmailAddress;
        //            break;

        //        case InputType.Name:
        //            break;

        //        case InputType.Number:
        //            txt.KeyboardType = UIKeyboardType.NumberPad;
        //            break;

        //        case InputType.NumericPassword:
        //            txt.SecureTextEntry = true;
        //            txt.KeyboardType = UIKeyboardType.NumberPad;
        //            break;

        //        case InputType.Password:
        //            txt.SecureTextEntry = true;
        //            break;

        //        case InputType.Phone:
        //            txt.KeyboardType = UIKeyboardType.PhonePad;
        //            break;

        //        case InputType.Url:
        //            txt.KeyboardType = UIKeyboardType.Url;
        //            break;
        //    }
        //}

        //protected iOSPickerStyle GetPickerStyle(IiOSStyleDialogConfig config)
        //{
        //    //var iOSConfig = (config as IiOSStyleDialogConfig);
        //    if (config == null)
        //        return iOSPickerStyle.Auto;

        //    if (!config.iOSPickerStyle.HasValue)
        //        return iOSPickerStyle.Auto;

        //    return config.iOSPickerStyle.Value;
        //}
    }
}
