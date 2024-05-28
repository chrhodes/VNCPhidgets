using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsExtensionMethods
{
    public static class TextBoxExtensionMethods
    {
        public static void AppendTextWithNewLine(this TextBox textBox, string text)
        {
            textBox.AppendText(text + Environment.NewLine);
        }
    }
}
