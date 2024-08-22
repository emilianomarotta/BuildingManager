using Domain;

namespace DTOs.Out
{
    public class CompanyDetailModel
    {
        public CompanyDetailModel(Company company)
        {
            this.Id = company.Id;
            this.Name = company.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            var otherCompanyDetail = obj as CompanyDetailModel;
            return Id == otherCompanyDetail.Id;
        }

    }
}
