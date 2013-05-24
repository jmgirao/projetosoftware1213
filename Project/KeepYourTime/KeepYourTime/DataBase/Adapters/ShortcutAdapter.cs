
namespace KeepYourTime.DataBase.Adapters
{
    /// <summary>
    /// Adapter for Shortcuts Table
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public class ShortcutAdapter
    {
        public int ShortcutId { get; set; }
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public char ShortcutKey { get; set; }
        public long TaskId { get; set; }

    }
}
