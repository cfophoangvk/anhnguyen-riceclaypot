using ANRCMS_MVVM.Models;

namespace ANRCMS_MVVM.ViewModel
{
    public class HomepageViewModel
    {
        public List<Food> FoodData { get; set; }
        public HomepageViewModel()
        { 
            FoodData = AnhnguyenclaypotDbContext.INSTANCE.Foods.ToList();
        }
    }
}
