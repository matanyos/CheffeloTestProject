using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase database;

        public UserRepository(MockDatabase database)
        {
            this.database = database;
        }

        public void Create(User obj)
        {
            database.Users.Add(obj.UserId, obj);
        }

        public User Read(Guid key)
        {
            if (database.Users.TryGetValue(key, out var user))
            {
                return user;
            }
            throw new KeyNotFoundException($"User with ID {key} is not found");
        }

        public void Update(User obj)
        {
            if (database.Users.ContainsKey(obj.UserId))
            {
                database.Users[obj.UserId] = obj;
            }
            else
            {
                throw new KeyNotFoundException($"User with ID {obj.UserId} is not found");
            }
        }

        public void Remove(User obj)
        {
            database.Users.Remove(obj.UserId);
        }
    }
}
