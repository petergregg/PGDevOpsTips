using PGDevOpsTips.Domain.Interfaces;

namespace PGDevOpsTips.Domain.Models
{
    public class Shortcut : IShortcut
    {
        public string Key { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
    }
}
