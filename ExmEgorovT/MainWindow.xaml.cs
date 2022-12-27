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
using System.Windows.Threading;

namespace ExmEgorovT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AcceptBtn.IsEnabled = false;
            CodeRefreshBtn.IsEnabled = false;
        }

        string code;
        static Random random = new Random();
        DispatcherTimer timer = new DispatcherTimer();

        private void CodeRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            code = random.Next(10000000, 99999999).ToString();
            if (MessageBox.Show(code,"КOД", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                CodeBx.IsEnabled = true;
                timer.Interval = TimeSpan.FromSeconds(10);
                timer.Tick += new EventHandler(timerTick);
                timer.Start();
            }
            
        }

        private void timerTick(object sender, EventArgs e)
        {
            CodeBx.IsEnabled = false;
            code = null;
            timer.Stop();
        }

        private void NumberBx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new ExmEntities())
                {
                    var number = db.Users.AsNoTracking().FirstOrDefault(u => u.Number == NumberBx.Text);
                    if (number != null)
                        NumberBx.IsEnabled = false;
                    else
                        MessageBox.Show("Неверный номер");
                }
            }
        }

        private void PasswordBx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new ExmEntities())
                {
                    var password = db.Users.AsNoTracking().FirstOrDefault(u => u.Number == NumberBx.Text & u.Password == PasswordBx.Password);
                    if (password != null)
                    {
                        PasswordBx.IsEnabled = false;
                        CodeRefreshBtn.IsEnabled = true;
                    }
                    else
                        MessageBox.Show("Неверный Пароль");

                }
            }
        }

        private void CodeBx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (code == CodeBx.Text)
                {
                    CodeBx.IsEnabled = false;
                    AcceptBtn.IsEnabled = true;
                } else
                {
                    MessageBox.Show("Неверный код");
                }

            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ExmEntities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Number == NumberBx.Text & u.Password == PasswordBx.Password);
                if (user.Acc == true)
                    MessageBox.Show("Добро пожаловать, администратор");
                else
                    MessageBox.Show("Добро пожаловать, пользователь");
            }
        }
    }
}
