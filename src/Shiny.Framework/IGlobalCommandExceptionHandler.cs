using System;
using System.Threading.Tasks;

namespace Shiny;


public interface IGlobalCommandExceptionHandler
{
    Task OnError(Exception exception);
}