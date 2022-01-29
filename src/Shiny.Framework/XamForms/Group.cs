using System.Collections.ObjectModel;


namespace Shiny.XamForms
{
    public class Group<T> : ObservableCollection<T>
    {
        public Group(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }


        public string Name { get; }
        public string ShortName { get; }
    }
}
