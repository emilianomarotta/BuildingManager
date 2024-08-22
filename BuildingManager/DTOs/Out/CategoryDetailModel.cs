using Domain;

namespace DTOs.Out
{
    public class CategoryDetailModel
    {
        public CategoryDetailModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            var otherCategoryDetail = obj as CategoryDetailModel;
            return Name == otherCategoryDetail.Name;
        }

    }
}
