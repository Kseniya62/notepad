using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isSaved = true;
        private string _pathToFile;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void NewPage_Click(object sender, RoutedEventArgs e)
        {
            //Если файл не сохранён, предлагаем сохранить
            if (_isSaved == false)
            {
                if (!SaveQuestion()) return;
            }
            textbox.Clear();
            _pathToFile = null;
            _isSaved = true;
            Title = "Блокнот";
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //Если файл не сохранён, предлагаем сохранить
            if (_isSaved == false)
            {
                if (!SaveQuestion()) return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf";
            dialog.DefaultExt = ".txt";
            if (dialog.ShowDialog() != true) return;

            //Сохраняем путь к файлу и его имя в переменных
            _pathToFile = dialog.FileName;
            _isSaved = true;

            //Помещаем в заголовок программы имя файла
            Title = dialog.SafeFileName + " - Блокнот";

            //Вывод текста из файла
            TextRange doc = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd);
            using (FileStream fs = new FileStream(_pathToFile, FileMode.Open))
            {
                if (_pathToFile.Remove(0, _pathToFile.LastIndexOf('.')) == ".rtf")
                    doc.Load(fs, DataFormats.Rtf);
                if (_pathToFile.Remove(0, _pathToFile.LastIndexOf('.')) == ".txt")
                    doc.Load(fs, DataFormats.Text);
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void SaveHow_Click(object sender, RoutedEventArgs e)
        {
            bool success = SaveDialog();
            if (success) Save();
        }
        private void RichTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd).Text;
            Typeface typeface = new Typeface(textbox.FontFamily, textbox.FontStyle, textbox.FontWeight, textbox.FontStretch);
            FormattedText ft = new FormattedText(text, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, typeface, textbox.FontSize, Brushes.Black);
            textbox.Document.PageWidth = ft.Width + 12;
        }
        private void Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _isSaved = false;
        }
        private bool SaveQuestion()
        {
            MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Внимание", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Cancel) return false;
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
            return true;
        }
        private void Save()
        {
            if (_pathToFile == null)
            {
                if (!SaveDialog()) return;
            }
            _isSaved = true;

            TextRange doc = new TextRange(textbox.Document.ContentStart, textbox.Document.ContentEnd);
            using (FileStream fs = File.Create(_pathToFile))
            {
                if (_pathToFile.Remove(0, _pathToFile.LastIndexOf('.')) == ".rtf")
                    doc.Save(fs, DataFormats.Rtf);
                else if (_pathToFile.Remove(0, _pathToFile.LastIndexOf('.')) == ".txt")
                    doc.Save(fs, DataFormats.Text);
                else
                    doc.Save(fs, DataFormats.Xaml);
            }
        }
        private bool SaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (dialog.ShowDialog() != true) return false;
            _pathToFile = dialog.FileName;
            this.Title = dialog.SafeFileName + " - Блокнот";
            return true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Если файл не сохранён, предлагаем сохранить
            if (_isSaved == false)
            {
                if (!SaveQuestion())
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            textbox.Copy();
        }
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            textbox.Cut();
        }
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            textbox.Paste();
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            textbox.Undo();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Блокнот\n\nРазработчик - Митрохина Ксения ИСП-31\n\n Вариант 6 Очистить текст. Поиск нужной строки.", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            string text = textbox.GetText();//получаем текст из текстового поля
            var lines = text.Split("\r\n");//разделяем на строки
            int countRow = lines.Length - 1;//получаем количество строк
            GoWindow window = new GoWindow(countRow);
            window.Owner = this;
            window.ShowDialog();
            if (window.RowNumber == -1) return;
            textbox.CaretPosition = textbox.Document.ContentStart;//переводим каретку  в начало 
            //переводим каретку вниз на определенное количество строк
            textbox.CaretPosition = textbox.CaretPosition.GetLineStartPosition(window.RowNumber - 1);
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            textbox.Clear(); 
        }
    }
}
