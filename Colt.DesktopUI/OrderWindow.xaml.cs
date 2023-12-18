using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly List<OrderProductDto> _orderProducts = new List<OrderProductDto>();

        private readonly MainWindow _mainWindow;
        private readonly CustomerWindow _customerWindow;
        private readonly IOrderService _orderService;
        private readonly IServiceProvider _serviceProvider;
        private OrderDto _order;

        public OrderWindow(
            MainWindow mainWindow,
            CustomerWindow customerWindow,
            IServiceProvider serviceProvider,
            int customerId,
            int? orderId = null)
        {
            _mainWindow = mainWindow;
            _customerWindow = customerWindow;
            _serviceProvider = serviceProvider;
            _orderService = _serviceProvider.GetRequiredService<IOrderService>();

            InitializeComponent();

            LoadData(customerId, orderId);
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            _order.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);

            if (_order.Id.HasValue)
            {
                await _orderService.UpdateAsync(_order, CancellationToken.None);
            }
            else
            {
                await _orderService.CreateAsync(_order, CancellationToken.None);
            }

            if (_customerWindow != null)
            {
                await _customerWindow.PopulateCustomerOrdersAsync(_order.CustomerId);
            }

            if (_mainWindow != null)
            {
                await _mainWindow.PopulateOrdersGrids();
            }

            Close();
        }

        private async void LoadData(int customerId, int? orderId)
        {
            var customer = await _serviceProvider.GetRequiredService<ICustomerService>()
                .GetByIdAsync(customerId, CancellationToken.None);

            txtCustomerId.Text = customerId.ToString();
            txtCustomerName.Text = customer.Name;

            if (!orderId.HasValue)
            {
                var products = (await _serviceProvider.GetRequiredService<ICustomerService>()
                    .GetProductsAsync(customerId, CancellationToken.None))
                    .Select(x => new OrderProductDto
                    {
                        CustomerProductId = x.Id.Value,
                        ProductName = x.ProductName,
                        ProductPrice = x.ProductPrice.Value
                    });

                _orderProducts.AddRange(products);

                _order = new OrderDto
                {
                    CustomerId = customerId,
                    DeliveryDate = DateTime.Now,
                    OrderDate = DateTime.Now,
                    Products = _orderProducts
                };

                txtOrderDate.Text = _order.OrderDate.ToString(Thread.CurrentThread.CurrentCulture);
            }
            else
            {
                _order = await _orderService.GetByIdAsync(orderId.Value, CancellationToken.None);

                txtId.Text = orderId.ToString();
                txtOrderTotalPrice.Text = _order.TotalPrice.HasValue ? _order.TotalPrice.Value.ToString("N0") : string.Empty;
                txtOrderDate.Text = _order.OrderDate.ToString(Thread.CurrentThread.CurrentCulture);
                txtDeliveryDate.Text = _order.DeliveryDate.ToString(Thread.CurrentThread.CurrentCulture);
                txtOrderStatus.Text = _order.Status.ToString();

                _orderProducts.AddRange(_order.Products);    
            }

            DataGridOrderProducts.ItemsSource = _orderProducts;
        }

        private void CellValue_OnEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.SortMemberPath.Equals("ActualWeight"))
            {
                var item = (OrderProductDto)e.Row.Item;
                var textBox = e.EditingElement as TextBox;
                var value = !string.IsNullOrEmpty(textBox.Text) ? decimal.Parse(textBox.Text) : 0;

                item.TotalPrice = item.ProductPrice * value;

                DataGridOrderProducts.CurrentItem = item;

                if (!e.Row.IsEditing)
                {
                    DataGridOrderProducts.Items.Refresh();

                    _order.TotalPrice = _orderProducts
                        .Where(x => x.TotalPrice.HasValue)
                        .Sum(x => x.TotalPrice);

                    txtOrderTotalPrice.Text = _order.TotalPrice.Value.ToString("N0", Thread.CurrentThread.CurrentCulture);
                }
            }
        }
    }
}
