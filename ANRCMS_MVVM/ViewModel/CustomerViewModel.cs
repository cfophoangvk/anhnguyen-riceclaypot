﻿using ANRCMS_MVVM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ANRCMS_MVVM.ViewModel
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        public CustomerViewModel(Customer c)
        {
            LoggedInCustomer = c;
        }
        #region Customer Profile
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
        public ICommand UpdateCustomerCommand => new RelayCommand(execute => UpdateCustomerProfile());
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
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Order
        public ObservableCollection<OrderDetail> OrderCart { get; set; } = new ObservableCollection<OrderDetail>();
        public List<Food> FoodData { get; set; } = AnhnguyenclaypotDbContext.INSTANCE.Foods.ToList();
        private int _totalPrice = 0;
        public int TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        public ICommand AddToCartCommand => new RelayCommand(AddToCart);
        public ICommand DeleteFromCartCommand => new RelayCommand(DeleteFromCart, id => OrderCart.Where(x => x.FoodId == (int)id).FirstOrDefault() != null);
        public ICommand ConfirmCommand => new RelayCommand(execute => Confirm(), canExecute => (OrderCart.Count > 0) && (SelectedBranch != null));

        private void AddToCart(object parameter)
        {
            int id = (int)parameter;
            var order = OrderCart.Where(x => x.FoodId == id).FirstOrDefault();
            if (order == null)
            {
                OrderDetail item = new OrderDetail
                {
                    FoodId = id,
                    Quantity = 1,
                    Food = AnhnguyenclaypotDbContext.INSTANCE.Foods.First(x => x.FoodId == id)
                };
                OrderCart.Add(item);
            }
            else
            {
                int index = OrderCart.IndexOf(order);
                OrderDetail item = new OrderDetail
                {
                    FoodId = id,
                    Quantity = order.Quantity + 1,
                    Food = AnhnguyenclaypotDbContext.INSTANCE.Foods.First(x => x.FoodId == id)
                };
                OrderCart[index] = item;
            }
            TotalPrice = CalculateTotalAmount();
        }
        private void DeleteFromCart(object parameter)
        {
            int id = (int)parameter;
            var order = OrderCart.Where(x => x.FoodId == id).FirstOrDefault();
            if (order != null)
            {
                int index = OrderCart.IndexOf(order);
                if (order.Quantity > 1)
                {
                    OrderDetail item = new OrderDetail
                    {
                        FoodId = id,
                        Quantity = order.Quantity - 1,
                        Food = AnhnguyenclaypotDbContext.INSTANCE.Foods.First(x => x.FoodId == id)
                    };
                    OrderCart[index] = item;
                }
                else
                {
                    OrderCart.Remove(order);
                }
                TotalPrice = CalculateTotalAmount();
            }
        }

        private int CalculateTotalAmount()
        {
            int total = 0;
            foreach (var item in OrderCart)
            {
                total += item.TotalAmount;
            }
            return total;
        }

        private void Confirm()
        {
            if (MessageBox.Show("Bạn có muốn đặt hàng?\n(LƯU Ý: Bạn không thể hoàn tác đơn hàng!)","",MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                AnhnguyenclaypotDbContext.INSTANCE.Orders.Add(new Order
                {
                    CustomerId = LoggedInCustomer.CustomerId,
                    BranchId = SelectedBranch.BranchId,
                    Status = 1,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    TotalPrice = TotalPrice
                });
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();

                int orderId = AnhnguyenclaypotDbContext.INSTANCE.Orders.OrderByDescending(x => x.OrderId).First().OrderId;
                foreach (var item in OrderCart)
                {
                    item.OrderId = orderId;
                }
                AnhnguyenclaypotDbContext.INSTANCE.AddRange(OrderCart);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                OrderCart.Clear();
                MessageBox.Show("Đặt hàng thành công!");
            }
        }
        #endregion

        #region Branch
        public List<Branch> Branches { get; set; } = AnhnguyenclaypotDbContext.INSTANCE.Branches.ToList();
        private Branch _selectedBranch = null!;
        public Branch SelectedBranch
        {
            get => _selectedBranch;
            set
            {
                _selectedBranch = value;
                OnPropertyChanged(nameof(SelectedBranch));
            }
        }
        #endregion

        #region Order Details

        #endregion
    }
}