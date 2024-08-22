using Domain;

namespace DTOs.Out
{
    public class OwnerDetailModel
    {
        public OwnerDetailModel(Owner owner)
        {
            this.Id = owner.Id;
            this.Email = owner.Email;
            this.Name = owner.Name;
            this.LastName = owner.LastName;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        
        public string Name { get; set; }
        
        public string LastName { get; set; }

        public override bool Equals(object? obj)
        {
            var otherOwnerDetail = obj as OwnerDetailModel;
            return Id == otherOwnerDetail.Id;
        }

    }
}
