using Colt.Application.Commands;
using Colt.Application.Commands.Products;
using Colt.Application.Common.Models;
using Colt.Application.Common.Services;
using Colt.Application.Interfaces;
using Colt.Application.Queries;
using Colt.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Colt.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMediator _mediator;
        private readonly ICustomerService _customerService;

        public MainWindow(
            IMediator mediator,
            ICustomerService customerService)
        {
            _mediator = mediator;
            _customerService = customerService;

            InitializeComponent();
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateProductsGrids();
            await PopulateCustomersGrids();
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

        }

        private void ButtonEditCustomer_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteCustomer_OnClick(object sender, RoutedEventArgs e)
        {

        }

        public async Task PopulateCustomersGrids()
        {
            DataGridCustomers.ItemsSource = await _customerService.GetAsync(CancellationToken.None);
        }

        #endregion
    }
}
