namespace Shiny.Applications;


public interface ICommandExceptionHandler
{
    Task OnException(Exception exception);
}