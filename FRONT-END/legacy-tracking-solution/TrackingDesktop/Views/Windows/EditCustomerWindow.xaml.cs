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
using TrackingUtils.Objects.Exceptions;
using TrackingUxLib.Code.API;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources.Values;
using Microsoft.Practices.Unity;

namespace TrackingDesktop.Views.Windows
{
    public partial class EditCustomerWindow : Window
    {
        #region Fields:
        CustomerDto _customerToEdit;
        #endregion

        #region Constructors:
        public EditCustomerWindow(CustomerDto customerToEdit)
        {
            InitializeComponent();
            _customerToEdit = customerToEdit;
        }
        #endregion

        #region Event Handlers:
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_customerToEdit != null)
                {
                    ucCustomerEditor.Customer = _customerToEdit;
                }
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
                ucCustomerEditor.Validate();

                var customerToEdit = ucCustomerEditor.Customer;

                #region Call Api:
                using (var endpoint = App.Container.Resolve<ICustomersEndpoint>())
                {
                    progress.IsBusy = true;
                    await endpoint.UpdateAsync(customerToEdit);
                    UxUtil.ShowMessage(Strings.SuccessfullyDone);
                    DialogResult = true;
                    Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        #endregion
    }
}
