namespace IDataAccess
{
    public interface IUserRepository<T> where T : class
    {
        T FindByToken(Guid token);
        T FindByEmailAndPassword(string email, string password);
    }
}
