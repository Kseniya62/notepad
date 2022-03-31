using System.Windows.Controls;
using System.Windows.Documents;

namespace notepad
{
    public static class Extension
    {
        public static void Clear(this RichTextBox richTextBox)
        {
            richTextBox.Document.Blocks.Clear();
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
        }
    }
}
