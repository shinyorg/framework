﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace Shiny.Framework.Tests;


public class ValidationBindingTests
{
    // TODO: test localizationmanager
    // TODO: test localize
    // TODO: test blank message

    public void Test()
    {
        var services = new ServiceCollection();
        //services.AddGlobalCommandExceptionAction();        
        //services.AddConnectivity();
        //services.AddScoped<BaseServices>();

        var sp = services.BuildServiceProvider();
        var vm = sp.GetRequiredService<StandardBaseViewModel>();

    }
}


public class StandardBaseViewModel : BaseViewModel
{
    public StandardBaseViewModel(BaseServices services) : base(services) {}


    [Reactive]
    [Required(AllowEmptyStrings = false)]
    [StringLength(5, MinimumLength = 3)]
    public string String { get; set; }
}


public class LocalizedBaseViewModel : BaseViewModel
{
    public LocalizedBaseViewModel(BaseServices services) : base(services) {}


    [Reactive]
    [Required(AllowEmptyStrings = false, ErrorMessage = "localize:ErrorString")]
    public string String { get; set; }
}