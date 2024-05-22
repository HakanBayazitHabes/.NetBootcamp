using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Users
{
    public interface IUserRepositoryAsync : IGenericRepository<User>
    {
        Task<bool> IsExistsName(string name);

        Task<bool> IsExistsEmail(string email);

        Task UpdateUserName(string name, int id);
    }
}