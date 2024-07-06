using ANRCMS_MVVM.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ANRCMS_MVVM.ViewModel
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        //Declaration
        public ObservableCollection<Customer> CustomerList { get; set; } = null!;
        public ObservableCollection<Staff> StaffList { get; set; } = null!;
        public ObservableCollection<Branch> BranchList { get; set; } = null!;
        private Customer _selectedCustomer = null!;
        private Customer _editingCustomer = new Customer();
        private Staff _selectedStaff = null!;
        private Staff _editingStaff = new Staff();
        private Branch _selectedBranch = null!;
        private Branch _editingBranch = new Branch();

        public AdminViewModel()
        {
            LoadCustomer();
            LoadStaff();
            LoadBranch();
        }

        //Selected item
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                if (value != null)
                {
                    EditingCustomer = new Customer
                    {
                        CustomerId = value.CustomerId,
                        CustomerName = value.CustomerName,
                        CustomerPhone = value.CustomerPhone,
                        Address = value.Address
                    };
                }
            }
        }
        public Customer EditingCustomer
        {
            get => _editingCustomer;
            set
            {
                _editingCustomer = value;
                OnPropertyChanged(nameof(EditingCustomer));
            }
        }
        public Staff SelectedStaff
        {
            get => _selectedStaff;
            set
            {
                _selectedStaff = value;
                OnPropertyChanged(nameof(SelectedStaff));
                if (value != null)
                {
                    EditingStaff = new Staff
                    {
                        StaffId = value.StaffId,
                        StaffName = value.StaffName,
                        StaffPhone = value.StaffPhone,
                        Branch = value.Branch
                    };
                }
            }
        }
        public Staff EditingStaff
        {
            get => _editingStaff;
            set
            {
                _editingStaff = value;
                OnPropertyChanged(nameof(EditingStaff));
            }
        }
        public Branch SelectedBranch
        {
            get => _selectedBranch;
            set
            {
                _selectedBranch = value;
                OnPropertyChanged(nameof(SelectedBranch));
                if (value != null)
                {
                    EditingBranch = new Branch
                    {
                        BranchId = value.BranchId,
                        BranchName = value.BranchName
                    };
                }
            }
        }
        public Branch EditingBranch
        {
            get => _editingBranch;
            set
            {
                _editingBranch = value;
                OnPropertyChanged(nameof(EditingBranch));
            }
        }

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Command
        public ICommand EmptyCustomerCommand => new RelayCommand(execute => EmptyCustomerValues());
        public ICommand AddCustomerCommand => new RelayCommand(execute => AddCustomer(), canExecute => EditingCustomer.CustomerId == 0);
        public ICommand UpdateCustomerCommand => new RelayCommand(execute => UpdateCustomer(), canExecute => EditingCustomer.CustomerId != 0);
        public ICommand DeleteCustomerCommand => new RelayCommand(execute => RemoveCustomer(), canExecute => EditingCustomer.CustomerId != 0);
        public ICommand EmptyStaffCommand => new RelayCommand(execute => EmptyStaffValues());
        public ICommand AddStaffCommand => new RelayCommand(execute => AddStaff(), canExecute => EditingStaff.StaffId == 0);
        public ICommand UpdateStaffCommand => new RelayCommand(execute => UpdateStaff(), canExecute => EditingStaff.StaffId != 0);
        public ICommand DeleteStaffCommand => new RelayCommand(execute => RemoveStaff(), canExecute => EditingStaff.StaffId != 0);
        public ICommand EmptyBranchCommand => new RelayCommand(execute => EmptyBranchValues());
        public ICommand AddBranchCommand => new RelayCommand(execute => AddBranch(), canExecute => EditingBranch.BranchId == 0);
        public ICommand UpdateBranchCommand => new RelayCommand(execute => UpdateBranch(), canExecute => EditingBranch.BranchId != 0);
        public ICommand DeleteBranchCommand => new RelayCommand(execute => RemoveBranch(), canExecute => EditingBranch.BranchId != 0);

        //Operation
        #region Customer
        private void LoadCustomer()
        {
            if (CustomerList == null)
            {
                CustomerList = new ObservableCollection<Customer>(AnhnguyenclaypotDbContext.INSTANCE.Customers.ToList());
            }
        }
        private void AddCustomer()
        {
            if (MessageBox.Show("Thêm khách hàng?", "Thêm khách hàng", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Customer c = new Customer
            {
                CustomerName = EditingCustomer.CustomerName,
                CustomerPhone = EditingCustomer.CustomerPhone,
                Address = EditingCustomer.Address,
                Password = "123@"
            };
            if (CheckCustomerValidation(c))
            {
                AnhnguyenclaypotDbContext.INSTANCE.Customers.Add(c);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                CustomerList.Add(c);
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void UpdateCustomer()
        {
            if (MessageBox.Show("Cập nhật khách hàng?", "Cập nhật khách hàng", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            var c = AnhnguyenclaypotDbContext.INSTANCE.Customers.Find(EditingCustomer.CustomerId);
            if (c != null)
            {
                c.CustomerName = EditingCustomer.CustomerName;
                c.CustomerPhone = EditingCustomer.CustomerPhone;
                c.Address = EditingCustomer.Address;
                if (!CheckCustomerValidation(c))
                {
                    MessageBox.Show("Có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                AnhnguyenclaypotDbContext.INSTANCE.Customers.Update(c);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                Customer customerToUpdate = new Customer
                {
                    CustomerId = EditingCustomer.CustomerId,
                    CustomerName = EditingCustomer.CustomerName,
                    CustomerPhone = EditingCustomer.CustomerPhone,
                    Address = EditingCustomer.Address,
                };
                CustomerList[CustomerList.IndexOf(c)] = customerToUpdate;
            }
        }
        private void RemoveCustomer()
        {
            if (MessageBox.Show("Xóa khách hàng?", "Xóa khách hàng", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Customer? c = AnhnguyenclaypotDbContext.INSTANCE.Customers.Find(EditingCustomer.CustomerId);
            if (c != null)
            {
                AnhnguyenclaypotDbContext.INSTANCE.Customers.Remove(c);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                CustomerList.Remove(c);
                EmptyCustomerValues();
            }
            else
            {
                MessageBox.Show("Không xóa được!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void EmptyCustomerValues()
        {
            EditingCustomer.CustomerId = 0;
            EditingCustomer.CustomerName = string.Empty;
            EditingCustomer.CustomerPhone = string.Empty;
            EditingCustomer.Address = string.Empty;
            OnPropertyChanged(nameof(EditingCustomer));
        }
        private bool CheckCustomerValidation(Customer c)
        {
            if (c.Address != null)
            {
                if (c.Address.Length > 255)
                {
                    return false;
                }
            }
            if (c.CustomerPhone == null || c.CustomerName == null)
            {
                return false;
            }
            if (Regex.IsMatch(c.CustomerPhone, @"^\d{10,11}$") && (c.CustomerName.Length > 0 && c.CustomerName.Length <= 100))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Staff
        private void LoadStaff()
        {
            if (StaffList == null)
            {
                StaffList = new ObservableCollection<Staff>(AnhnguyenclaypotDbContext.INSTANCE.Staff.Include(x => x.Branch).ToList());
            }
        }
        private void AddStaff()
        {
            if (MessageBox.Show("Thêm nhân viên?", "Thêm nhân viên", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Staff s = new Staff
            {
                StaffName = EditingStaff.StaffName,
                StaffPhone = EditingStaff.StaffPhone,
                Branch = EditingStaff.Branch,
                Password = "123@"
            };
            if (CheckStaffValidation(s))
            {
                AnhnguyenclaypotDbContext.INSTANCE.Staff.Add(s);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                StaffList.Add(s);
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void UpdateStaff()
        {
            if (MessageBox.Show("Cập nhật nhân viên?", "Cập nhật nhân viên", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Staff? s = AnhnguyenclaypotDbContext.INSTANCE.Staff.Find(EditingStaff.StaffId);
            if (s != null)
            {
                s.StaffName = EditingStaff.StaffName;
                s.StaffPhone = EditingStaff.StaffPhone;
                s.Branch = EditingStaff.Branch;
                if (!CheckStaffValidation(s))
                {
                    MessageBox.Show("Có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                AnhnguyenclaypotDbContext.INSTANCE.Staff.Update(s);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                Staff staffToUpdate = new Staff
                {
                    StaffId = EditingStaff.StaffId,
                    StaffName = EditingStaff.StaffName,
                    StaffPhone = EditingStaff.StaffPhone,
                    Branch = EditingStaff.Branch
                };
                StaffList[StaffList.IndexOf(s)] = staffToUpdate;
            }
        }
        private void RemoveStaff()
        {
            if (MessageBox.Show("Xóa nhân viên?", "Xóa nhân viên", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Staff? s = AnhnguyenclaypotDbContext.INSTANCE.Staff.Find(EditingStaff.StaffId);
            if (s != null)
            {
                AnhnguyenclaypotDbContext.INSTANCE.Staff.Remove(s);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                StaffList.Remove(s);
                EmptyStaffValues();
            }
            else
            {
                MessageBox.Show("Không xóa được!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void EmptyStaffValues()
        {
            EditingStaff.StaffId = 0;
            EditingStaff.StaffName = string.Empty;
            EditingStaff.StaffPhone = string.Empty;
            EditingStaff.Branch = null!;
            OnPropertyChanged(nameof(EditingStaff));
        }
        private bool CheckStaffValidation(Staff s)
        {
            if (s.Branch == null)
            {
                return false;
            }
            if (s.StaffName == null || s.StaffPhone == null)
            {
                return false;
            }
            if (Regex.IsMatch(s.StaffPhone, @"^\d{10,11}$") && (s.StaffName.Length > 0 && s.StaffName.Length <= 100))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Branch
        private void LoadBranch()
        {
            if (BranchList == null)
            {
                BranchList = new ObservableCollection<Branch>(AnhnguyenclaypotDbContext.INSTANCE.Branches.ToList());
            }
        }
        private void AddBranch()
        {
            if (MessageBox.Show("Thêm chi nhánh mới?", "Thêm chi nhánh", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Branch b = new Branch
            {
                BranchName = EditingBranch.BranchName
            };
            if (b.BranchName != null && b.BranchName.Length > 0 && b.BranchName.Length <= 255)
            {
                AnhnguyenclaypotDbContext.INSTANCE.Branches.Add(b);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                BranchList.Add(b);
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void UpdateBranch()
        {
            if (MessageBox.Show("Cập nhật chi nhánh?", "Cập nhật chi nhánh", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Branch? b = AnhnguyenclaypotDbContext.INSTANCE.Branches.Find(EditingBranch.BranchId);
            if (b != null)
            {
                b.BranchName = EditingBranch.BranchName;
                if (b.BranchName != null && b.BranchName.Length > 0 && b.BranchName.Length <= 255)
                {
                    AnhnguyenclaypotDbContext.INSTANCE.Branches.Update(b);
                    AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                    Branch branchToUpdate = new Branch
                    {
                        BranchName = EditingBranch.BranchName
                    };
                    BranchList[BranchList.IndexOf(b)] = branchToUpdate;
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
        private void RemoveBranch()
        {
            if (MessageBox.Show("Xóa chi nhánh?", "Xóa chi nhánh", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            Branch? b = AnhnguyenclaypotDbContext.INSTANCE.Branches.Find(EditingBranch.BranchId);
            if (b != null)
            {
                AnhnguyenclaypotDbContext.INSTANCE.Branches.Remove(b);
                AnhnguyenclaypotDbContext.INSTANCE.SaveChanges();
                BranchList.Remove(b);
                EmptyBranchValues();
            }
            else
            {
                MessageBox.Show("Không xóa được!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void EmptyBranchValues()
        {
            EditingBranch.BranchId = 0;
            EditingBranch.BranchName = string.Empty;
            OnPropertyChanged(nameof(EditingBranch));
        }
        #endregion
    }
}
