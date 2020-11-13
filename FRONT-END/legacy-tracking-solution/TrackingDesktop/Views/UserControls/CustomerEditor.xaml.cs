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
using TrackingUtils.Objects.Exceptions;
using TrackingUtils.Utils;
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources;
using TrackingUxLib.Resources.Values;

namespace TrackingDesktop.Views.UserControls
{
    public partial class CustomerEditor : UserControl
    {
        #region Constructors:
        public CustomerEditor()
        {
            InitializeComponent();
        }
        #endregion

        #region Fields:
        bool _isEditing = false;
        CustomerDto _model = null;
        #endregion

        #region Props:
        public CustomerDto Customer
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
                var provinces = CityUtil.GetProvinces();
                cmbProvince.ItemsSource = provinces;
                cmbMarketer.ItemsSource = Collections.Marketers;
                cmbInitialValidity.ItemsSource = ResourceManager.EnumToDic<InitialValidity>();
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private void cmbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbProvince.SelectedItem != null)
                {
                    var provinceId = Convert.ToInt32(cmbProvince.SelectedValue);
                    var cities = CityUtil.GetProvinceCities(provinceId);
                    cmbCity.ItemsSource = cities;
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
            {
                _model = new CustomerDto();
                _model.AspNetUser = new AspNetUserDto();
            }

            _model.AspNetUser.FirstName = tbFirstName.Text;
            _model.AspNetUser.Surname = tbSurname.Text;
            _model.AspNetUser.PhoneNumber = tbPhoneNumber.Text;
            _model.AspNetUser.Email = tbEmail.Text;
            _model.AspNetUser.UserName = tbUserName.Text;
            _model.UserName = tbUserName.Text;
            _model.Password = tbPassword.Password;
            _model.AspNetUser.Gender = (tbMale.IsChecked.HasValue && tbMale.IsChecked.Value ? true
                : (tbFemale.IsChecked.HasValue && tbFemale.IsChecked.Value ? false : (bool?)null));
            _model.Province = (cmbProvince.SelectedItem != null ? cmbProvince.SelectedValue.ToString() : null);
            _model.City = (cmbCity.SelectedItem != null ? cmbCity.SelectedValue.ToString() : null);
            _model.Address = tbAddress.Text;
            _model.Marketer = cmbMarketer.SelectedValue.ToString();
            _model.AspNetUser.IsActive = tbIsActive.IsChecked.HasValue && tbIsActive.IsChecked.Value;
            if (!_isEditing)
            {
                var initialValidity = EnumUtil.GetEnum<InitialValidity>(cmbInitialValidity.SelectedValue.ToString());
                var validityMonths = (int)initialValidity;
                _model.AspNetUser.AccountExpirationDate = DateTime.Now.AddMonths(validityMonths);
            }
        }
        void UpdateForm()
        {
            if (_model != null)
            {
                if (_model.AspNetUser != null)
                {
                    tbFirstName.Text = _model.AspNetUser.FirstName;
                    tbSurname.Text = _model.AspNetUser.Surname;
                    tbPhoneNumber.Text = _model.AspNetUser.PhoneNumber;
                    tbEmail.Text = _model.AspNetUser.Email;
                    tbUserName.Text = _model.AspNetUser.UserName;
                    tbMale.IsChecked = _model.AspNetUser.Gender.HasValue && _model.AspNetUser.Gender.Value;
                    tbFemale.IsChecked = _model.AspNetUser.Gender.HasValue && !_model.AspNetUser.Gender.Value;
                    cmbProvince.SelectedValue = _model.Province;
                    cmbCity.SelectedValue = _model.City;
                    tbAddress.Text = _model.Address;
                    tbIsActive.IsChecked = _model.AspNetUser.IsActive;
                    cmbMarketer.SelectedValue = _model.Marketer;
                    if (_isEditing)
                    {
                        cmbInitialValidity.IsEnabled = false;
                    }
                }


            }
        }
        public void Validate()
        {
            var message = "";

            var requiredFieldsFilled =
                !string.IsNullOrEmpty(tbFirstName.Text) && !string.IsNullOrEmpty(tbSurname.Text)
                && !string.IsNullOrEmpty(tbUserName.Text) && !string.IsNullOrEmpty(tbPhoneNumber.Text)
                && !string.IsNullOrEmpty(tbAddress.Text) && cmbMarketer.SelectedItem != null;

            if (!_isEditing)
            {
                requiredFieldsFilled = requiredFieldsFilled &&
                    !string.IsNullOrEmpty(tbPassword.Password) && cmbInitialValidity.SelectedItem != null;
            }

            if (!requiredFieldsFilled)
                message += (!string.IsNullOrEmpty(message) 
                    ? $"{Environment.NewLine}{Strings.InputRequiredFields}" 
                    : Strings.InputRequiredFields);

            if (!string.IsNullOrEmpty(tbPassword.Password)
                && (tbPassword.Password != tbPasswordConfirm.Password))
                message += (!string.IsNullOrEmpty(message)
                    ? $"{Environment.NewLine}{Strings.InvalidPasswordConfirm}"
                    : Strings.InvalidPasswordConfirm);

            if (!string.IsNullOrEmpty(tbPhoneNumber.Text) && !(new Regex(Patterns.phonenumber)).IsMatch(tbPhoneNumber.Text))
                message += (!string.IsNullOrEmpty(message)
                    ? $"{Environment.NewLine}{Strings.InvalidPhoneNumber}"
                    : Strings.InvalidPhoneNumber);

            if (!string.IsNullOrEmpty(tbEmail.Text) && !RamancoLibrary.Utilities.TextUtils.IsEmailAddress(tbEmail.Text))
                message += (!string.IsNullOrEmpty(message)
                    ? $"{Environment.NewLine}{Strings.InvalidEmailAddress}"
                    : Strings.InvalidEmailAddress);

            if (!string.IsNullOrEmpty(message))
                throw new ValidationException(message);
        }
        #endregion
    }
}
