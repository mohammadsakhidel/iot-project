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
using TrackingUxLib.Code.API;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources.Values;
using Microsoft.Practices.Unity;

namespace TrackingDesktop.Views.Windows
{
    public partial class CreateCustomerWindow : Window
    {
        #region Constructors:
        public CreateCustomerWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers:
        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ucCustomerEditor.Validate();

                var newCustomer = ucCustomerEditor.Customer;
                newCustomer.AspNetUser.Creator = "admin";

                #region Call Api:
                using (var endpoint = App.Container.Resolve<ICustomersEndpoint>())
                {
                    progress.IsBusy = true;
                    await endpoint.CreateAsync(newCustomer);
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
