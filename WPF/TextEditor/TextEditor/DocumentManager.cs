
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;

namespace TextEditor
{
    class DocumentManager
    {
        private string _currentFile;
        private RichTextBox _textBox;

        public DocumentManager(RichTextBox textbox)
        {
            this._textBox = textbox;
        }

        public void NewDocument()
        {
            _currentFile = null;
            _textBox.Document = new FlowDocument();
        }

        public bool OpenDocument()
        {
            OpenFileDialog opn = new OpenFileDialog();
            if (opn.ShowDialog() == true)
            {
                _currentFile = opn.FileName;
                using (Stream strm = opn.OpenFile())
                {
                    TextRange txtRng = new TextRange(_textBox.Document.ContentStart, _textBox.Document.ContentEnd);
                    txtRng.Load(strm, DataFormats.Rtf);
                    return true;
                }
            }
            return false;
        }

        public bool SaveDocument()
        {
            if (string.IsNullOrEmpty(_currentFile)) return SaveAsDocument();

            using (FileStream fs = new FileStream(_currentFile, FileMode.Create))
            {
                TextRange rng = new TextRange(_textBox.Document.ContentStart, _textBox.Document.ContentEnd);
                rng.Save(fs, DataFormats.Rtf);
                
                return true;
            }
        }

        public bool SaveAsDocument()
        {
            SaveFileDialog sv = new SaveFileDialog();
            if (sv.ShowDialog()==true)
            {
                _currentFile = sv.FileName;

                return SaveDocument() ;
            }
            return false;
        }


        public void ApplyToSelection(DependencyProperty property, object value)
        {
            if (value != null)
            {
                _textBox.Selection.ApplyPropertyValue(property, value);
            }
        }

        public bool CanSaveDocument()
        {
            return !string.IsNullOrEmpty(_currentFile);
        }
    }
}
