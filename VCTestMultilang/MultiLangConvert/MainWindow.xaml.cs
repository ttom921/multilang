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

namespace MultiLangConvert
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.textblock_filename.Text = openFileDialog.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textblock_filename.Text)) return;
            try
            {
                var retdata = ConvertHelp.Instance.ReadExcelData(textblock_filename.Text);
                ConvertHelp.Instance.GenJsonFile(retdata.Item1, retdata.Item2);
                //轉mvc
                ConvertHelp.Instance.GenResfile(retdata.Item1, retdata.Item2);
                //轉core
                ConvertHelp.Instance.GenResfile(retdata.Item1, retdata.Item2, "SharedResource",true);
               
                
                MessageBox.Show("轉換成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
