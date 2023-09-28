using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly List<CustomerProductDto> _customerProducts = new List<CustomerProductDto>();
        private List<OrderDto> _customerOrders = new List<OrderDto>();
        private List<ProductDropdownDto> _customerDropdowns = new List<ProductDropdownDto>();

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

            DataGridCustomerProducts.ItemsSource = _customerProducts;

            Task.Run(() => PopulateProductsDropdownAsync());

            if (customerDto != null)
            {
                txtId.Text = customerDto.Id?.ToString();
                txtName.Text = customerDto.Name;

                Task.Run(() => PopulateCustomerProductsAsync(customerDto.Id.Value));
                Task.Run(() => PopulateCustomerOrdersAsync(customerDto.Id.Value));
            }
            else
            {
                DataGridCustomerOrders.Visibility = Visibility.Hidden;
                addOrderButton.Visibility = Visibility.Hidden;
            }
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
            var orderWindow = new OrderWindow(null, this, _serviceProvider, _customerDto.Id.Value);

            orderWindow.ShowDialog();
        }

        private void ButtonEditCustomerOrder_OnClick(object sender, RoutedEventArgs e)
        {
            var orderDto = ((FrameworkElement)sender).DataContext as OrderDto;

            var editOrderWindow = new OrderWindow(null, this, _serviceProvider, _customerDto.Id.Value, orderDto.Id.Value);
            editOrderWindow.ShowDialog();
        }

        private void ButtonDeleteCustomerOrder_OnClick(object sender, RoutedEventArgs e)
        {
            var order = ((FrameworkElement)sender).DataContext as OrderDto;

            Task.Run(() => DeleteCustomerOrderAsync(order?.Id ?? default));
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
                _customerDto.Orders = _customerOrders;

                await _serviceProvider.GetRequiredService<ICustomerService>()
                    .UpdateAsync(_customerDto, CancellationToken.None);
            }
            else
            {
                _customerDto = new CustomerDto
                {
                    Name = txtName.Text,
                    Products = _customerProducts,
                    Orders = _customerOrders
                };

                await _serviceProvider.GetRequiredService<ICustomerService>()
                    .CreateAsync(_customerDto, CancellationToken.None);
            }

            await _mainWindow.PopulateCustomersGrids();

            Close();
        }

        private async Task PopulateProductsDropdownAsync()
        {
            _customerDropdowns = (await _productService.GetAsync(CancellationToken.None))
                    .Select(x => new ProductDropdownDto
                    {
                        Name = x.Name,
                        Id = x.Id.Value
                    })
                    .ToList();

            await Dispatcher.BeginInvoke(() =>
            {
                ProductsBox.ItemsSource = _customerDropdowns;
            });
        }

        private async Task PopulateCustomerProductsAsync(int id)
        {
            var products = await _serviceProvider.GetRequiredService<ICustomerService>()
                .GetProductsAsync(id, CancellationToken.None);

            _customerProducts.AddRange(products);

            await Dispatcher.BeginInvoke(() =>
            {
                _customerDropdowns.ForEach(x => x.IsSelected = _customerProducts.Any(p => p.ProductId == x.Id));

                ProductsBox.Items.Refresh();

                DataGridCustomerProducts.Items.Refresh();
            });
        }

        public async Task PopulateCustomerOrdersAsync(int id)
        {
            var orders = await _serviceProvider.GetRequiredService<IOrderService>()
                .GetByCustomerIdAsync(id, CancellationToken.None);

            _customerOrders = orders;

            await Dispatcher.BeginInvoke(() =>
            {
                DataGridCustomerOrders.ItemsSource = _customerOrders;

                DataGridCustomerOrders.Items.Refresh();
            });

            if (_mainWindow != null)
            {
                await _mainWindow.PopulateOrdersGrids();
            }
        }

        private async Task DeleteCustomerOrderAsync(int orderId)
        {
            await _serviceProvider.GetRequiredService<IOrderService>()
                .DeleteAsync(orderId, CancellationToken.None);

            await PopulateCustomerOrdersAsync(_customerDto.Id.Value);
        }
    }
}
