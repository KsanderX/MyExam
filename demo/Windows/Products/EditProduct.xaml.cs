using demo.Data;
using demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace demo.Windows.Products
{
    public partial class EditProduct : Window
    {
        private readonly string projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        DemoContext context;
        private Product product;
        private BitmapImage selectImage;
        private string? imageName = null;
        public EditProduct(Product product, DemoContext prevContext)
        {
            InitializeComponent();

            context = prevContext;
            this.product = product;
            Load();
        }

        private void Load()
        {

            if (product.ImagePath == null || product.ImagePath == "picture.png")
            {
                selectImage = new BitmapImage(new Uri(Path.Combine(projPath, "Images", "Defaults", "picture.png")));
            }
            else
            {
                selectImage = new BitmapImage(new Uri(Path.Combine(projPath, "Images",product.ImagePath)));
            }

            BoxImage.Source = selectImage;


            BoxCategory.DisplayMemberPath = "Name";
            BoxCategory.SelectedValuePath = "Id";
            BoxCategory.ItemsSource = context.Categories.ToList();
            BoxCategory.SelectedValue = product.CategoryId;

            BoxSupplier.DisplayMemberPath = "Name";
            BoxSupplier.SelectedValuePath = "Id";
            BoxSupplier.ItemsSource = context.Suppliers.ToList();
            BoxSupplier.SelectedValue = product.SupplierId;

            BoxManufacturer.DisplayMemberPath = "Name";
            BoxManufacturer.SelectedValuePath = "Id";
            BoxManufacturer.ItemsSource = context.Manufacturers.ToList();
            BoxManufacturer.SelectedValue = product.ManufacturerId;


            BoxName.Text = product.Name.Name;
            BoxDescription.Text = product.Description;
            BoxDiscount.Text = product.Discount.ToString();
            BoxPrice.Text = product.Price.ToString();
            BoxUnit.Text = product.Unit.ToString();
            BoxCount.Text = product.Count.ToString();
        }

        private void ButtonSaveProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BoxDescription.Text) ||
                string.IsNullOrWhiteSpace(BoxDiscount.Text) ||
                string.IsNullOrWhiteSpace(BoxCount.Text) ||
                string.IsNullOrWhiteSpace(BoxPrice.Text) ||
                string.IsNullOrWhiteSpace(BoxName.Text) ||
                string.IsNullOrWhiteSpace(BoxUnit.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            try
            {

                if (int.Parse(BoxDiscount.Text) < 0)
                {
                    MessageBox.Show("Скидка не может быть отрицательной");
                    return;
                }

                if (int.Parse(BoxDiscount.Text) > 100)
                {
                    MessageBox.Show("Скидка не может быть больше 100 процентов");
                    return;
                }

                if (int.Parse(BoxCount.Text) < 0)
                {
                    MessageBox.Show("Количество не может быть отрицательным значением");
                    return;
                }

                if (int.Parse(BoxPrice.Text) < 0)
                {
                    MessageBox.Show("Цена не может быть отрицательной");
                    return;
                }

                var name = context.ProductNames.FirstOrDefault(q => q.Name == BoxName.Text);
                if (name == null)
                {
                    context.ProductNames.Add(new ProductName() { Id = context.ProductNames.Max(q => q.Id) + 1, Name = BoxName.Text });
                    context.SaveChanges();
                    name = context.ProductNames.FirstOrDefault(q => q.Name == BoxName.Text);
                }

                product.Name = name;
                product.Category = context.Categories.FirstOrDefault(q => q.Name == BoxCategory.SelectedItem.ToString());
                product.Description = BoxDescription.Text;
                product.Manufacturer = context.Manufacturers.FirstOrDefault(q => q.Name == BoxManufacturer.SelectedItem.ToString());
                product.Supplier = context.Suppliers.FirstOrDefault(q => q.Name == BoxSupplier.SelectedItem.ToString());
                product.Price = int.Parse(BoxPrice.Text);
                product.Unit = BoxUnit.Text;
                product.Count = int.Parse(BoxCount.Text);
                product.Discount = int.Parse(BoxDiscount.Text);
                if (imageName != null)
                {
                    product.ImagePath = imageName;
                }

                context.SaveChanges();

                MessageBox.Show("Изменения сохранены!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неверный формат ввода {ex.Message}");
            }
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Вы вышли из окна редактирования товара", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = false;
        }

        private void ButtonLoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                Uri uri = new Uri(openFile.FileName);
                BitmapImage select = new BitmapImage(uri);

                if (select.Width > 400 || select.Height > 300)
                {
                    MessageBox.Show("Размеры изображения имеют неверный формат", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string folderPath = Path.Combine(projPath, "Images");

                string destFileName = Path.Combine(folderPath, openFile.SafeFileName);

                try
                {
                    File.Copy(openFile.FileName, destFileName, true);

                    selectImage = select;
                    imageName = openFile.SafeFileName;
                    BoxImage.Source = selectImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании файла: {ex.Message}");
                }
            }
        }
    }
}
