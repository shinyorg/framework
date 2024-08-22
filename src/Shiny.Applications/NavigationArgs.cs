namespace Shiny.Applications;

public enum NavigationType
{
    Back,
    Forward
}

public record NavigationArgs(
    NavigationType Type,
    IDictionary<string, string> Arguments
);