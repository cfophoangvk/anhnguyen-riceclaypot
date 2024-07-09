using ANRCMS_MVVM.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ANRCMS_MVVM.ViewModel
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private Customer _loggedInCustomer = null!;
        public Customer LoggedInCustomer
        {
            get => _loggedInCustomer;
            set
            {
                _loggedInCustomer = value;
                OnPropertyChanged(nameof(LoggedInCustomer));
                EditingCustomer = new Customer
                {
                    CustomerId = _loggedInCustomer.CustomerId,
                    CustomerName = _loggedInCustomer.CustomerName,
                    CustomerPhone = _loggedInCustomer.CustomerPhone,
                    Address = _loggedInCustomer.Address,
                    Password = _loggedInCustomer.Password
                };
            }
        }
        private Customer _editingCustomer = null!;
        public Customer EditingCustomer
        {
            get => _editingCustomer;
            set
            {
                _editingCustomer = value;
                OnPropertyChanged(nameof(EditingCustomer));
            }
        }
        public CustomerViewModel(Customer c)
        {
            LoggedInCustomer = c;
        }

        private void UpdateCustomerProfile()
        {
            if (MessageBox.Show("Cập nhật thông tin?", "", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (!CheckValidate(EditingCustomer))
            {
                MessageBox.Show("Có lỗi xảy ra!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var customerToUpdate = AnhnguyenclaypotDbContext.INSTANCE.Customers.Find(EditingCustomer.CustomerId);
            if (customerToUpdate != null)
            {
                customerToUpdate.CustomerName = EditingCustomer.CustomerName;
                customerToUpdate.CustomerPhone = EditingCustomer.CustomerPhone;
                customerToUpdate.Address = EditingCustomer.Address;
                customerToUpdate.Password = EditingCustomer.Password;

                AnhnguyenclaypotDbContext.INSTANCE.Update(customerToUpdate);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
            }
        }
        private bool CheckValidate(Customer c)
        {
            if (c.CustomerPhone == string.Empty || c.CustomerName == string.Empty || c.Password == string.Empty)
            {
                return false;
            }
            if (Regex.IsMatch(c.CustomerPhone, @"^\d{10,11}$"))
            {
                return true;
            }
            return false;
        }

        public ICommand UpdateCustomerCommand => new RelayCommand(execute => UpdateCustomerProfile());

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}