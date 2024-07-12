using ANRCMS_MVVM.Models;
using System.Windows.Controls;

namespace ANRCMS_MVVM
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class HomepageFoodMenu : Page
    {
        public List<Food> FoodData { get; set; }
        public HomepageFoodMenu()
        {
            InitializeComponent();
            DataContext = this;
            FoodData = AnhnguyenclaypotDbContext.INSTANCE.Foods.ToList();
        }
    }
}
