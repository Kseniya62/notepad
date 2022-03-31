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
using System.Windows.Shapes;

namespace notepad
{
    /// <summary>
    /// Логика взаимодействия для GoWindow.xaml
    /// </summary>
    public partial class GoWindow : Window
    {
        private int _rowCount;
        public GoWindow(int rowCount)
        {
            InitializeComponent();
            _rowCount = rowCount;
            RowNumber = -1;
        }
        public int RowNumber { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRowNumber.Text, out int rowNumber) || rowNumber < 1 || rowNumber > _rowCount)
            {
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RowNumber = rowNumber;
            this.Close();
        }
    }
}
