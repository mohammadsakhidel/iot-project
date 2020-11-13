using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TrackingModels.Dtos;
using TrackingUtils.Constants;
using TrackingUtils.Enums;
using TrackingUtils.Objects;
using TrackingUtils.Objects.Exceptions;
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources;
using TrackingUxLib.Resources.Values;

namespace TrackingDesktop.Views.UserControls
{
    public partial class TerminalEditor : UserControl
    {
        #region Constructors:
        public TerminalEditor()
        {
            InitializeComponent();
        }
        #endregion

        #region Fields:
        bool _isEditing = false;
        TerminalDto _model;
        #endregion

        #region Props:
        public TerminalDto Terminal
        {
            get
            {
                UpdateModel();
                return _model;
            }
            set
            {
                _model = value;
                _isEditing = true;
                UpdateForm();
            }
        }
        #endregion

        #region Event Handlers:
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbManufacturer.ItemsSource = Collections.Manufacturers;
                cmbManufacturer.SelectedIndex = 0;

                var products = ProductBag.Products;
                products.ForEach(p => p.DisplayName = ResourceManager.GetValue(p.DisplayNameResourceID));
                cmbProduct.ItemsSource = products;

                tbDeviceID.Focus();
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private void cmbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var product = cmbProduct.SelectedItem as Product;
                if (product != null)
                {
                    product.Models.ForEach(m => m.DisplayName = ResourceManager.GetValue(m.DisplayNameResourceID));
                    cmbModel.ItemsSource = product.Models;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private void cmbModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var model = cmbModel.SelectedItem as ProductModel;
                if (model != null)
                {
                    model.Variants.ForEach(v => v.DisplayName = ResourceManager.GetValue(v.DisplayNameResourceID));
                    cmbVariant.ItemsSource = model.Variants;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private void cmbVariant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var product = cmbProduct.SelectedItem as Product;
                var model = cmbModel.SelectedItem as ProductModel;
                var variant = cmbVariant.SelectedItem as ProductModelVariant;
                if (product != null && model != null && variant != null)
                {
                    var displayName = String.Format("{0} - {1} - {2}",
                        ResourceManager.GetValue(product.DisplayNameResourceID),
                        ResourceManager.GetValue(model.DisplayNameResourceID),
                        ResourceManager.GetValue(variant.DisplayNameResourceID));
                    tbDisplayName.Text = displayName;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        #endregion

        #region Methods:
        void UpdateModel()
        {
            if (_model == null)
                _model = new TerminalDto();

            _model.ManufacturerID = cmbManufacturer.SelectedValue.ToString();
            _model.DeviceID = tbDeviceID.Text;
            _model.ID = $"{_model.ManufacturerID}_{_model.DeviceID}";
            _model.Product = (cmbProduct.SelectedItem as Product)?.Name;
            _model.Model = (cmbModel.SelectedItem as ProductModel)?.Name;
            _model.Variant = (cmbVariant.SelectedItem as ProductModelVariant)?.Name;
            _model.DisplayName = tbDisplayName.Text;
            _model.IMEI = tbIMEI.Text;
            _model.ServerAddress = tbServerAddress.Text;
            _model.ServerPort = Convert.ToInt32(tbServerPort.Text);
            _model.Statements = tbStatements.Text;
        }
        void UpdateForm()
        {
            if (_model != null)
            {
                cmbManufacturer.SelectedValue = _model.ManufacturerID;
                tbDeviceID.Text = _model.DeviceID;
                cmbProduct.SelectedValue = _model.Product;
                cmbModel.SelectedValue = _model.Model;
                cmbVariant.SelectedValue = _model.Variant;
                tbDisplayName.Text = _model.DisplayName;
                tbIMEI.Text = _model.IMEI;
                tbServerAddress.Text = _model.ServerAddress;
                tbServerPort.Text = _model.ServerPort.ToString();
                tbStatements.Text = _model.Statements;
                if (_isEditing)
                {
                    cmbManufacturer.IsEnabled = false;
                    tbDeviceID.IsEnabled = false;
                }
            }
        }
        public void Validate()
        {
            var message = "";

            var requiredFieldsFilled =
                cmbManufacturer.SelectedItem != null && !string.IsNullOrEmpty(tbDeviceID.Text)
                && cmbProduct.SelectedItem != null && cmbModel.SelectedItem != null
                && cmbVariant.SelectedItem != null && !string.IsNullOrEmpty(tbIMEI.Text)
                && !string.IsNullOrEmpty(tbServerAddress.Text) && !string.IsNullOrEmpty(tbServerPort.Text)
                && !string.IsNullOrEmpty(tbDisplayName.Text);

            if (!requiredFieldsFilled)
                message += (!string.IsNullOrEmpty(message)
                    ? $"{Environment.NewLine}{Strings.InputRequiredFields}"
                    : Strings.InputRequiredFields);

            var rgxNumber = new Regex(Patterns.number);
            if (!string.IsNullOrEmpty(tbDeviceID.Text) && !rgxNumber.IsMatch(tbDeviceID.Text))
                message += (!string.IsNullOrEmpty(message)
                    ? $"{Environment.NewLine}{Strings.InvalidTerminalID}"
                    : Strings.InvalidTerminalID);

            if (!string.IsNullOrEmpty(tbServerPort.Text) && !rgxNumber.IsMatch(tbServerPort.Text))
                message += (!string.IsNullOrEmpty(message)
                    ? $"{Environment.NewLine}{Strings.InvalidPortNumber}"
                    : Strings.InvalidPortNumber);

            if (!string.IsNullOrEmpty(message))
                throw new ValidationException(message);
        }
        #endregion
    }
}