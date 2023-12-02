using MovieTracker.Infrastructure.Data;
using MovieTracker.Infrastructure.Interfaces;
using MovieTracker.Models;

namespace MovieTracker.Infrastructure.Repo;

public class UserRepository : Repository<AppUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

}