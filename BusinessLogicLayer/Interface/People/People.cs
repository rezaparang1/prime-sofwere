using BusinessEntity.DTO.People;

namespace BusinessLogicLayer.Interface.People
{
    public interface IPeopleService
    {
        Task<List<PeopleComboDto>> GetPeopleForComboAsync();
        Task<List<BusinessEntity.People.People>> Search(string? firstName = null,
            string? lastName = null, string? phone = null, string? address = null,
            int? groupPeople = null, bool? business = null, bool? user = null,
            bool? employee = null, bool? investor = null);

        Task<IEnumerable<BusinessEntity.People.People>> GetAll();
        Task<BusinessEntity.People.People?> GetById(int id);
        Task<BusinessEntity.People.People?> GetPeopleById(string? idPeople);
        Task<Result> Create(BusinessEntity.People.People person, int userId);
        Task<Result> Update(BusinessEntity.People.People person, int userId);
        Task<Result> Delete(int id, int userId);
    }
}
