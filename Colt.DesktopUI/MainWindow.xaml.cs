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

        public MainWindow(IMediator mediator)
        {
            _mediator = mediator;

            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateGrids();
        }

        private void ButtonEditProduct_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteProduct_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private async Task PopulateGrids()
        {
            DataGridProducts.ItemsSource = await _mediator.Send(new GetProductsQuery(), CancellationToken.None);
        }
    }
}
