using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Users
{
    public class UserRepositoryAsync(AppDbContext context) : GenericRepository<User>(context), IUserRepositoryAsync
    {

        public async Task<bool> IsExistsEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(user => user.Email == email);

            return user != null;

        }

        public async Task<bool> IsExistsName(string name)
        {
            var user = await context.Users.FirstOrDefaultAsync(user => user.Name == name);

            return user != null;
        }

        public async Task UpdateUserName(string name, int id)
        {
            var user = await GetById(id);
            user!.Name = name;
            await Update(user);
        }
    }
}