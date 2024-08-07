﻿using ANRCMS_MVVM.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ANRCMS_MVVM.ViewModel
{
    public class StaffViewModel : INotifyPropertyChanged
    {
        #region Staff Profile
        private Staff _loggedInStaff = null!;
        public Staff LoggedInStaff
        {
            get => _loggedInStaff;
            set
            {
                _loggedInStaff = value;
                OnPropertyChanged(nameof(LoggedInStaff));
                EditingStaff = new Staff
                {
                    StaffId = LoggedInStaff.StaffId,
                    StaffName = LoggedInStaff.StaffName,
                    StaffPhone = LoggedInStaff.StaffPhone,
                    Branch = LoggedInStaff.Branch,
                    Password = LoggedInStaff.Password
                };
            }
        }
        private Staff _editingStaff = null!;
        public Staff EditingStaff
        {
            get => _editingStaff;
            set
            {
                _editingStaff = value;
                OnPropertyChanged(nameof(EditingStaff));
            }
        }
        public List<Branch> Branches { get; set; }
        public StaffViewModel(Staff s)
        {
            LoggedInStaff = s;
            Branches = AnhnguyenclaypotDbContext.INSTANCE.Branches.ToList();
            OrderList = new ObservableCollection<Order>(AnhnguyenclaypotDbContext.INSTANCE.Orders
                                                                                          .Include(x => x.Customer)
                                                                                          .Where(x => x.BranchId == LoggedInStaff.BranchId)
                                                                                          .ToList());
        }

        public ICommand UpdateStaffCommand => new RelayCommand(execute => UpdateStaffProfile());

        private void UpdateStaffProfile()
        {
            if (MessageBox.Show("Cập nhật thông tin?", "", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (!CheckValidateStaffProfile(EditingStaff))
            {
                MessageBox.Show("Có lỗi xảy ra!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var staffToUpdate = AnhnguyenclaypotDbContext.INSTANCE.Staff.Find(EditingStaff.StaffId);
            if (staffToUpdate != null)
            {
                staffToUpdate.StaffName = EditingStaff.StaffName;
                staffToUpdate.StaffPhone = EditingStaff.StaffPhone;
                staffToUpdate.Branch = EditingStaff.Branch;
                staffToUpdate.Password = EditingStaff.Password;

                AnhnguyenclaypotDbContext.INSTANCE.Update(staffToUpdate);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
            }
        }
        private bool CheckValidateStaffProfile(Staff s)
        {
            if (s.StaffPhone == string.Empty || s.StaffName == string.Empty || s.Password == string.Empty)
            {
                return false;
            }
            if (AnhnguyenclaypotDbContext.INSTANCE.Staff.Any(x => x.StaffPhone == s.StaffPhone))
            {
                return false;
            }
            if (Regex.IsMatch(s.StaffPhone, @"^\d{10,11}$"))
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

        #region Orders
        public ObservableCollection<Order> OrderList { get; set; }
        private Order _selectedOrder = null!;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
        #endregion
    }
}
