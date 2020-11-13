using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrackingModels.Dtos;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources.Values;
using Microsoft.Practices.Unity;

namespace TrackingDesktop.Views.Windows
{
    public partial class CreateTerminalWindow : Window
    {
        #region Fields:
        CustomerDto _customer;
        #endregion

        #region Constructors:
        public CreateTerminalWindow(CustomerDto customer)
        {
            InitializeComponent();
            _customer = customer;
        }
        #endregion

        #region Event Handlers:
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblCustomerName.Text = $"{Strings.Customer}: {_customer.AspNetUser.FirstName} {_customer.AspNetUser.Surname} ({_customer.UserName})";
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ucTerminalEditor.Validate();

                var newTerminal = ucTerminalEditor.Terminal;
                newTerminal.CustomerID = _customer.ID;

                #region Call Api:
                using (var endpoint = App.Container.Resolve<ITerminalsEndpoint>())
                {
                    progress.IsBusy = true;
                    await endpoint.CreateAsync(newTerminal);
                    UxUtil.ShowMessage(Strings.SuccessfullyDone);
                    DialogResult = true;
                    Close();
                }
                #endregion

            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        #endregion
    }
}
