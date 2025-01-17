namespace YemekTarifiSitesi.Models
{
    public class Category
    {
        public string idCategory { get; set; }
        public string strCategory { get; set; }
        public string strCategoryThumb { get; set; }
        public string strCategoryDescription { get; set; }
    }

    public class CategoryResponse
    {
        public List<Category> categories { get; set; }
    }
}
