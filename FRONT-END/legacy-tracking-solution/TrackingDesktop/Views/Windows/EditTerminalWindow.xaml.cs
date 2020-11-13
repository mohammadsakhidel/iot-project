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
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources.Values;
using Microsoft.Practices.Unity;
using TrackingUxLib.Code.API.Interfaces;

namespace TrackingDesktop.Views.Windows
{
    public partial class EditTerminalWindow : Window
    {
        #region Fields:
        TerminalDto _terminalToEdit;
        #endregion

        #region Constructors:
        public EditTerminalWindow(TerminalDto terminalToEdit)
        {
            InitializeComponent();
            _terminalToEdit = terminalToEdit;
        }
        #endregion

        #region Event Handlers:
        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ucTerminalEditor.Validate();

                var terminalToUpdate = ucTerminalEditor.Terminal;

                #region Call Api:
                using (var endpoint = App.Container.Resolve<ITerminalsEndpoint>())
                {
                    progress.IsBusy = true;
                    await endpoint.UpdateAsync(terminalToUpdate);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblCustomerName.Text = $"{Strings.Customer}: {_terminalToEdit.Customer.FullName} ({_terminalToEdit.Customer.UserName})";
                ucTerminalEditor.Terminal = _terminalToEdit;
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        #endregion
    }
}
