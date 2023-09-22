using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml.Linq;

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly List<OrderProductDto> _orderProducts = new List<OrderProductDto>();

        private readonly CustomerWindow _customerWindow;
        private readonly IOrderService _orderService;
        private readonly IServiceProvider _serviceProvider;
        private readonly int _customerId;

        public OrderWindow(
            CustomerWindow customerWindow,
            IServiceProvider serviceProvider,
            int customerId,
            int? orderId = null)
        {
            _customerWindow = customerWindow;
            _serviceProvider = serviceProvider;
            _orderService = _serviceProvider.GetRequiredService<IOrderService>();

            _customerId = customerId;
            InitializeComponent();

            LoadData(customerId, orderId);
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var orderDto = new OrderDto
            {
                Id = !string.IsNullOrEmpty(txtId.Text) ? int.Parse(txtId.Text) : null,
                CustomerId = int.Parse(txtCustomerId.Text),
                DeliveryDate = DateTime.Parse(txtDeliveryDate.Text),
                OrderDate = DateTime.Parse(txtOrderDate.Text),
                Products = _orderProducts
            };

            if (orderDto.Id.HasValue)
            {
                await _orderService.UpdateAsync(orderDto, CancellationToken.None);
            }
            else
            {
                await _orderService.CreateAsync(orderDto, CancellationToken.None);
            }

            _customerWindow.PopulateCustomerOrders(_customerId);

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
                txtOrderDate.Text = DateTime.Now.ToString(Thread.CurrentThread.CurrentCulture);

                var products = _serviceProvider.GetRequiredService<ICustomerService>()
                    .GetProducts(customerId)
                    .Select(x => new OrderProductDto
                    {
                        CustomerProductId = x.Id.Value,
                        ProductName = x.ProductName,
                        ProductPrice = x.ProductPrice.Value
                    });

                _orderProducts.AddRange(products);
            }
            else
            {
                var order = await _orderService.GetByIdAsync(orderId.Value, CancellationToken.None);

                txtId.Text = orderId.ToString();
                txtOrderDate.Text = order.OrderDate.ToString(Thread.CurrentThread.CurrentCulture);
                txtDeliveryDate.Text = order.DeliveryDate.ToString(Thread.CurrentThread.CurrentCulture);

                _orderProducts.AddRange(order.Products);    
            }

            DataGridOrderProducts.ItemsSource = _orderProducts;
        }

        private void ButtonCalculate_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var orderProduct in _orderProducts.Where(x => x.ActualWeight.HasValue))
            {
                orderProduct.TotalPrice = (decimal)orderProduct.ActualWeight * orderProduct.ProductPrice;
            }

            DataGridOrderProducts.Items.Refresh();

        }

        private void CellValue_OnEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.SortMemberPath.Equals("ActualWeight"))
            {
                var item = (OrderProductDto)e.Row.Item;
                var textBox = e.EditingElement as TextBox;
                var value = decimal.Parse(textBox.Text);

                item.TotalPrice = item.ProductPrice * value;

                DataGridOrderProducts.CurrentItem = item;

                if (!e.Row.IsEditing)
                {
                    DataGridOrderProducts.Items.Refresh();
                }
            }
        }
    }
}
