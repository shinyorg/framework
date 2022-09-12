using System;
using System.Windows.Input;
using ReactiveUI;

namespace Shiny;


public class CommandItem : ReactiveObject
{
    string? imageUri;
    public string? ImageUri
    {
        get => this.imageUri;
        set => this.RaiseAndSetIfChanged(ref this.imageUri, value);
    }


    string? text;
    public string? Text
    {
        get => this.text;
        set => this.RaiseAndSetIfChanged(ref this.text, value);
    }


    string? detail;
    public string? Detail
    {
        get => this.detail;
        set => this.RaiseAndSetIfChanged(ref this.detail, value);
    }


    public ICommand? PrimaryCommand { get; set; }
    public ICommand? SecondaryCommand { get; set; }
    public object? Data { get; set; }
}

