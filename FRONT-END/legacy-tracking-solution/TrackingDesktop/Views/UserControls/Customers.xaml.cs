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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrackingDesktop.Views.Windows;
using TrackingModels.Dtos;
using TrackingUxLib.Code.API;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUxLib.Code.Utils;
using Microsoft.Practices.Unity;
using TrackingUxLib.Resources.Values;

namespace TrackingDesktop.Views.UserControls
{
    public partial class Customers : UserControl
    {
        #region Constructors:
        public Customers()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers:
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadRecords();
            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new CreateCustomerWindow();
                var res = window.ShowDialog();
                if (res.HasValue && res.Value)
                {
                    await LoadRecords();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var entityToEdit = datagrid.SelectedItem as CustomerDto;
                if (entityToEdit != null)
                {
                    var window = new EditCustomerWindow(entityToEdit);
                    var res = window.ShowDialog();
                    if (res.HasValue && res.Value)
                    {
                        await LoadRecords();
                    }
                }
            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var entityToDelete = datagrid.SelectedItem as CustomerDto;
                if (entityToDelete != null)
                {
                    var question = UxUtil.ShowQuestion(Strings.AreYouSure);
                    if (question == MessageBoxResult.Yes)
                    {
                        progress.IsBusy = true;
                        using (var endpoint = App.Container.Resolve<ICustomersEndpoint>())
                        {
                            await endpoint.DeleteAsync(entityToDelete.ID);
                            await LoadRecords();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        private void btnCreateTerminal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = datagrid.SelectedItem as CustomerDto;
                if (customer != null)
                {
                    var window = new CreateTerminalWindow(customer);
                    var res = window.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        #endregion

        #region Methods:
        private async Task LoadRecords()
        {
            progress.IsBusy = true;
            using (var endpoint = App.Container.Resolve<ICustomersEndpoint>())
            {
                var customers = await endpoint.GetLatestsAsync();
                datagrid.ItemsSource = customers;
                progress.IsBusy = false;
            }
        }
        #endregion
    }
}
