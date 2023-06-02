using Colt.Application.Commands;
using Colt.Application.Commands.Products;
using Colt.Application.Common.Models;
using Colt.Application.Queries;
using Colt.Domain.Entities;
using MediatR;
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
    /// Interaction logic for CreateProduct.xaml
    /// </summary>
    public partial class CreateProduct : Window
    {
        private readonly MainWindow _mainWindow;
        private readonly IMediator _mediator;

        public CreateProduct(
            IMediator mediator,
            MainWindow mainWindow,
            ProductDto product = null)
        {
            _mediator = mediator;
            _mainWindow = mainWindow;

            InitializeComponent();

            if (product != null)
            {
                txtId.Text = product.Id?.ToString();
                txtName.Text = product.Name;
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (!txtId.Text.IsNullOrEmpty())
            {
                await _mediator.Send(new UpdateProductCommand
                {
                    Id = int.Parse(txtId.Text),
                    Name = txtName.Text
                }, CancellationToken.None);
            }
            else if (!txtName.Text.IsNullOrEmpty())
            {
                await _mediator.Send(new CreateProductCommand
                {
                    Name = txtName.Text
                }, CancellationToken.None);
            }

            await _mainWindow.PopulateGrids();

            Close();
        }
    }
}
