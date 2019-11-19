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
using System.Windows.Interop;
using System.Windows.Forms;

namespace Login_Registration
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

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Password;
            string phone = phoneBox.Text;
            string email = emailBox.Text;

            Register.register(username, password, phone, email);
        }

        private void showBrowser()
        {
            ImageExplorer.Form1 form1 = new ImageExplorer.Form1();
            WindowInteropHelper wih = new WindowInteropHelper(this);
            wih.Owner = form1.Handle;
            form1.Show();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox_login.Text;
            string password = passwordBox_login.Password;

            if (Login.login(username, password))
            {
                showBrowser();
                this.Hide();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var reg = Login.readRegistry();
            if (reg != null)
            {
                if (Login.verifyToken((string)reg))
                {
                    showBrowser();
                    this.Hide();
                }
            }
        }
    }
}
