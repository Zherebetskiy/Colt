using Colt.Application.Commands.Products;
using Colt.Application.Common.Models;
using Colt.Application.Interfaces;
using Colt.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMediator _mediator;
        private readonly IProductService _productService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOrderService _orderService;

        public MainWindow(
            IMediator mediator,
            IProductService productService,
            IServiceProvider serviceProvider,
            IOrderService orderService)
        {
            _mediator = mediator;
            _productService = productService;
            _orderService = orderService;
            _serviceProvider = serviceProvider;

            InitializeComponent();
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateProductsGrids();
            await PopulateCustomersGrids();
            await PopulateOrdersGrids();
        }

        #region prducts

        public async Task PopulateProductsGrids()
        {
            DataGridProducts.ItemsSource = await _mediator.Send(new GetProductsQuery(), CancellationToken.None);
        }

        private void ButtonEditProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var productDto = ((FrameworkElement)sender).DataContext as ProductDto;

            var createProductWindow = new CreateProduct(_mediator, this, productDto);
            createProductWindow.ShowDialog();
        }

        private async void ButtonDeleteProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var productDto = ((FrameworkElement)sender).DataContext as ProductDto;

            await _mediator.Send(new DeleteProductCommand
            {
                Id = productDto.Id ?? 0
            }, CancellationToken.None);

            await PopulateProductsGrids();
        }

        private void ButtonCreateProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var createProductWindow = new CreateProduct(_mediator, this);
            createProductWindow.ShowDialog();
        }

        #endregion

        #region customers

        private void ButtonCreateCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var customerWindow = new CustomerWindow(
                _serviceProvider,
                _productService,
                this);

            customerWindow.ShowDialog();
        }

        private void ButtonEditCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var customerDto = ((FrameworkElement)sender).DataContext as CustomerDto;

            var customerWindow = new CustomerWindow(
                _serviceProvider,
                _productService,
                this,
                customerDto);

            customerWindow.ShowDialog();
        }

        private async void ButtonDeleteCustomer_OnClick(object sender, RoutedEventArgs e)
        {
            var customer = ((FrameworkElement)sender).DataContext as CustomerDto;

            await _serviceProvider.GetRequiredService<ICustomerService>()
                .DeleteAsync(customer.Id ?? default, CancellationToken.None);

            await PopulateCustomersGrids();
        }

        public async Task PopulateCustomersGrids()
        {
            DataGridCustomers.ItemsSource = await _serviceProvider.GetRequiredService<ICustomerService>()
                .GetAsync(CancellationToken.None);

            DataGridCustomers.Items.Refresh();
        }

        #endregion

        #region

        private void ButtonEditOrder_OnClick(object sender, RoutedEventArgs e)
        {
            var orderDto = ((FrameworkElement)sender).DataContext as OrderDto;

            var customerWindow = new OrderWindow(
                this,
                null,
                _serviceProvider,
                orderDto.CustomerId,
                orderDto.Id);

            customerWindow.ShowDialog();
        }

        private async void ButtonDeleteOrder_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete order?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        var order = ((FrameworkElement)sender).DataContext as OrderDto;

                        await _orderService.DeleteAsync(order.Id ?? default, CancellationToken.None);

                        await PopulateOrdersGrids();

                        break;
                    }
                case MessageBoxResult.No:
                    break;
            }
        }

        public async Task PopulateOrdersGrids()
        {
            var orders = await _orderService.GetAsync(CancellationToken.None);

            await Dispatcher.BeginInvoke(() =>
            {
                DataGridOrders.ItemsSource = orders;

                DataGridOrders.Items.Refresh();
            });
        }

        private void ButtonPrintOrder_OnClick(object sender, RoutedEventArgs e)
        {
            var orderDto = ((FrameworkElement)sender).DataContext as OrderDto;

            var documentService = _serviceProvider.GetRequiredService<IDocumentService>();

            Task.Run(async () =>
            {
                var documentName = await documentService.CreateInvoiceAsync(orderDto.Id.Value);

                await Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show($"Invoice {documentName} successfully created.", "Document", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            });

        }

        #endregion
    }
}
