using Domain;

namespace DTOs.Out
{
    public class StaffDetailModel
    {
        public StaffDetailModel(Staff staff)
        {
            Id = staff.Id;
            Email = staff.Email;
            Name = staff.Name;
            LastName = staff.LastName;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public override bool Equals(object? obj)
        {
            var otherStaffDetailModel = obj as StaffDetailModel;
            return Id == otherStaffDetailModel.Id;
        }
    }
}
