using ReactiveUI;

using System;
using System.Collections.Generic;


namespace Shiny
{
    public interface IValidationBinding : IReactiveObject, IDisposable
    {
        IReadOnlyDictionary<string, string> Errors { get; }
        IReadOnlyDictionary<string, bool> Touched { get;}
    }
}
