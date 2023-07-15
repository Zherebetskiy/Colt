using Colt.Application.Commands.Products;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly List<CustomerProductDto> _customerProducts = new List<CustomerProductDto>();

        private readonly IServiceProvider _serviceProvider;
        private readonly IProductService _productService;
        private readonly MainWindow _mainWindow;
        private CustomerDto _customerDto;

        public CustomerWindow(
            IServiceProvider serviceProvider,
            IProductService productService,
            MainWindow mainWindow,
            CustomerDto customerDto = null)
        {
            InitializeComponent();

            _productService = productService;
            _serviceProvider = serviceProvider;
            _mainWindow = mainWindow;
            _customerDto = customerDto;

            if (customerDto != null)
            {
                txtId.Text = customerDto.Id?.ToString();
                txtName.Text = customerDto.Name;

                PopulateCustomerProducts(customerDto.Id.Value);
            }
            else
            {
                DataGridCustomerOrders.Visibility = Visibility.Hidden;
                addOrderButton.Visibility = Visibility.Hidden;
            }

            Task.FromResult(PopulateProductsDropdown(customerDto.Id));

            DataGridCustomerProducts.ItemsSource = _customerProducts;
        }

        private void Product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBoxProduct_OnCheck(object sender, RoutedEventArgs e)
        {
            var productDto = ((FrameworkElement)sender).DataContext as ProductDropdownDto;

            _customerProducts.Add(new CustomerProductDto
            {
                ProductId = productDto.Id,
                ProductName = productDto.Name
            });

            DataGridCustomerProducts.Items.Refresh();
        }

        private void CheckBoxProduct_OnUncheck(object sender, RoutedEventArgs e)
        {
            var productDto = ((FrameworkElement)sender).DataContext as ProductDropdownDto;

            var productToDelete = _customerProducts.FirstOrDefault(x => x.ProductId == productDto.Id);

            _customerProducts.Remove(productToDelete);

            DataGridCustomerProducts.Items.Refresh();
        }

        private void ButtonCreateCustomerOrder_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEditCustomerOrder_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteCustomerOrder_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (!txtId.Text.IsNullOrEmpty())
            {
                _customerDto.Name = txtName.Text;
                _customerDto.Products = _customerProducts;

                await _serviceProvider.GetRequiredService<ICustomerService>()
                    .UpdateAsync(_customerDto, CancellationToken.None);
            }
            else
            {
                _customerDto = new CustomerDto
                {
                    Name = txtName.Text,
                    Products = _customerProducts
                };

                await _serviceProvider.GetRequiredService<ICustomerService>()
                    .CreateAsync(_customerDto, CancellationToken.None);
            }

            await _mainWindow.PopulateCustomersGrids();

            Close();
        }

        private async Task PopulateProductsDropdown(int? customerId)
        {
            var products = (await _productService.GetAsync(CancellationToken.None))
                    .Select(x => new ProductDropdownDto
                    {
                        Name = x.Name,
                        Id = x.Id.Value
                    })
                    .ToList();

            if (customerId.HasValue)
            {
                products.ForEach(x => x.IsSelected = _customerProducts.Any(p => p.ProductId == x.Id));
            }

            ProductsBox.ItemsSource = products;
        }

        private void PopulateCustomerProducts(int id)
        {
            var products = _serviceProvider.GetRequiredService<ICustomerService>()
                .GetProducts(id);

            _customerProducts.AddRange(products);
        }
    }
}
