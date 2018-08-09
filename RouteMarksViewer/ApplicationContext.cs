using System.Data.Entity;

namespace RouteMarksViewer
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Models.Mark> Marks { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<Models.Map> Maps { get; set; }
        public DbSet<Models.RouteScheduler> RouteSchedulers { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.UserRole> UserRoles { get; set; }
        public DbSet<Models.Log> Logs { get; set; }
        public DbSet<Models.LogType> LogTypes { get; set; }

        public DbSet GetDbSet<T>()
        {
            System.Type type = typeof(T);
            if (type == typeof(Models.Mark))
            {
                return Marks;
            }
            if (type == typeof(Models.Route))
            {
                return Routes;
            }
            if (type == typeof(Models.Map))
            {
                return Maps;
            }
            if (type == typeof(Models.RouteScheduler))
            {
                return RouteSchedulers;
            }
            if (type == typeof(Models.User))
            {
                return Users;
            }
            if (type == typeof(Models.UserRole))
            {
                return UserRoles;
            }
            if (type == typeof(Models.Log))
            {
                return Logs;
            }
            if (type == typeof(Models.LogType))
            {
                return LogTypes;
            }
            return null;
        }
    }
}
