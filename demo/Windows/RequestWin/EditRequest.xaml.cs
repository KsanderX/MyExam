using System.Windows;
using System.Xml.Linq;
using demo.Data;
using demo.Models;
using Microsoft.EntityFrameworkCore;

namespace demo.Windows.RequestWin
{
    public partial class EditRequest : Window
    {
        private DemoContext context;
        private Order order;
        public EditRequest(Order order)
        {
            InitializeComponent();
            context = new DemoContext();
            PanelOrder.DataContext = order;
            this.order = order;
            BoxStatus.ItemsSource = context.Statuses.ToList();
            BoxStatus.DisplayMemberPath = "Status1";
            BoxStatus.SelectedValuePath = "Id";
            BoxStatus.SelectedValue= order.StatusId;

        }

        private void Button_save(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxDateDelivery.Text) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text))
            {
                try
                {

                    order.OrderDate = DateTime.Parse(BoxDateOrder.Text);
                    order.DeliveryDate = DateTime.Parse(BoxDateDelivery.Text);
                    order.Code = double.Parse(BoxArc.Text);                    
                    var name = context.PickupPoints.FirstOrDefault(q => q.Adress== BoxDelivary.Text);
                    if (name == null)
                    {
                        context.PickupPoints.Add(new PickupPoint() { Id = context.PickupPoints.Max(q => q.Id) + 1, Adress= BoxDelivary.Text });
                        context.SaveChanges();
                        
                    }

                    order.PickupPoint = context.PickupPoints.FirstOrDefault(q => q.Adress == BoxDelivary.Text);                    
                    order.Status = BoxStatus.SelectedItem as Status;

                    context.Entry(order).State = EntityState.Modified;
                    MessageBox.Show("Изменения сохранены!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                    context.SaveChanges();

                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Заполните обязательные поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);               
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вышли из окна редактирования заявки", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = false;
        }
    }
}
