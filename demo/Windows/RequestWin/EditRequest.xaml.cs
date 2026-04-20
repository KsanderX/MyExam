using demo.Data;
using demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

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


            BoxStatus.DisplayMemberPath = "Status1";
            BoxStatus.SelectedValuePath = "Id";
            BoxStatus.ItemsSource = context.Statuses.ToList();
            BoxStatus.SelectedValue = order.StatusId;

            BoxPoint.DisplayMemberPath = "Adress";
            BoxPoint.SelectedValuePath = "Id";
            BoxPoint.ItemsSource = context.PickupPoints.ToList();
            BoxPoint.SelectedValue = order.PickupPointId;

        }

        private void Button_save(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxDateDelivery.Text) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text))
            {
                try
                {

                    order.OrderDate = DateTime.Parse(BoxDateOrder.Text);
                    order.DeliveryDate = DateTime.Parse(BoxDateDelivery.Text);
                    order.Code = double.Parse(BoxArc.Text);
                    order.PickupPoint = (PickupPoint)BoxPoint.SelectedItem;
                    order.Status = BoxStatus.SelectedItem as Status;
                    
                    context.Entry(order).State = EntityState.Modified;
                    context.SaveChanges();

                    MessageBox.Show("Заказ успешно отредактирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
