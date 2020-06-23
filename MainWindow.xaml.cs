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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;


namespace Filmdatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        } 
        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            if ((Username.Text == "Admin") && (PasswordBox.Password == "Password"))
            {
                this.Hide();
                Main mainwin = new Main();
                mainwin.Show();
            }
            else
            {
                MessageBox.Show("Incorrect User or Password");
                Username.Clear(); PasswordBox.Clear();
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
