using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
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

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly List<OrderProductDto> _orderProducts = new List<OrderProductDto>();

        private readonly IServiceProvider _serviceProvider;

        public OrderWindow(
            IServiceProvider serviceProvider,
            int customerId,
            int? orderId = null)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();

            LoadData(customerId, orderId);

            txtOrderDate.Text = DateTime.Now.ToString(Thread.CurrentThread.CurrentCulture);

        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadData(int customerId, int? orderId)
        {
            var customer = await _serviceProvider.GetRequiredService<ICustomerService>()
                .GetByIdAsync(customerId, CancellationToken.None);

            txtCustomerId.Text = customer.Name;

            if (!orderId.HasValue)
            {
                var products = _serviceProvider.GetRequiredService<ICustomerService>()
                    .GetProducts(customerId)
                    .Select(x => new OrderProductDto
                    {
                        CustomerProductId = x.Id.Value,
                        ProductName = x.ProductName,
                        ProductPrice = x.ProductPrice.Value
                    });

                _orderProducts.AddRange(products);

                DataGridOrderProducts.ItemsSource = _orderProducts;
            }
        }

        private void CellValue_OnChange(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == nameof(OrderProductDto.ActualWeight))
            {

                var textBox = e.EditingElement as TextBox;
                if (decimal.TryParse(textBox.Text, out var value))
                {
                    var orderProductDto = e.Row.DataContext as OrderProductDto;

                    var orderProduct = _orderProducts.FirstOrDefault(x => x.CustomerProductId == orderProductDto.CustomerProductId);

                    if (orderProduct == null)
                    {
                        return;
                    }

                    orderProduct.TotalPrice = orderProduct.ProductPrice * value;

                    //var grid = (DataGrid)sender;
                    //grid.CommitEdit(DataGridEditingUnit.Row, true);

                    //DataGridOrderProducts.CancelEdit();
                    DataGridOrderProducts.Items.Refresh();
                }
            }
        }
    }
}
