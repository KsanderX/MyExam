using demo.Data;
using demo.Models;
using System.Windows;

namespace demo.Windows
{
    public partial class Authorization : Window
    {
        private DemoContext context;
        public Authorization()
        {
            InitializeComponent();
            context = new DemoContext();
        }

        private void Button_authorization(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(BoxLogin.Text) && !string.IsNullOrWhiteSpace(BoxPassword.Text))
            {
                User user = context.Users.FirstOrDefault(q => q.Login == BoxLogin.Text && q.Password == BoxPassword.Text);
            
                if (user != null)
                {
                    user.RoleNavigation = context.Roles.FirstOrDefault(q => q.Id == user.Role);
                    Main main = new Main(user);
                    MessageBox.Show("Вы успешно авторизовались!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_authorization_gouest(object sender, RoutedEventArgs e)
        {
            Main main = new Main();
            MessageBox.Show("Вы вошли как гость!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            main.Show();
            this.Close();
        }
    }
}
