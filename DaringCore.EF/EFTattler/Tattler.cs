using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace DaringCore.EF.EFTattler
{
    public class Tattler
    {
        private DbContext _context;
        private ITattleStorage _storage;

        public Tattler(DbContext context, ITattleStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public void TattleUpdate<T>(T dbObject, TattleReason reason, string user) where T : class
        {
            try
            {
                // Attempt to disconnect the object.
                ((IObjectContextAdapter)_context).ObjectContext.Detach(dbObject);
            }
            catch { } // No catch because failure simply meens it wasn't connected.

            var entry = _context.Entry(dbObject);
            entry.State = EntityState.Modified;
            Tattle(Shred(entry.OriginalValues), dbObject, reason, user);
        }

        public void TattleAdd<T>(T dbObject, TattleReason reason, string user) where T : class
        {
            var entry = _context.Entry(dbObject);
            entry.State = EntityState.Added;
            Tattle(new Dictionary<string, object>(), dbObject, reason, user);
        }

        public void TattleDelete<T>(T dbObject, TattleReason reason, string user) where T : class
        {
            try
            {
                // Attempt to disconnect the object.
                ((IObjectContextAdapter)_context).ObjectContext.Detach(dbObject);
            }
            catch { } // No catch because failure simply meens it wasn't connected.

            var entry = _context.Entry(dbObject);
            entry.State = EntityState.Deleted;
            Tattle(Shred(entry.OriginalValues), dbObject, reason, user);
        }

        public TattleModel GetLastTattle(Type type)
        {
            return _storage.GetLastTattle(type.Name);
        }

        public IEnumerable<TattleModel> GetTattles(Type type, int skip = 0, int take = 20)
        {
            return _storage.GetTattles(type.Name, skip, take);
        }

        private void Tattle<T>(Dictionary<string, object> original, T change, TattleReason reason, string userId)
        {
            if (original.GetHashCode() == change.GetHashCode())
                return;

            var settings = new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new SystemTypeResolver() };

            var tattle = new TattleModel()
            {
                ObjectName = change.GetType().Name,
                ChangeDate = DateTime.UtcNow,
                NewObject = Newtonsoft.Json.JsonConvert.SerializeObject(change, settings),
                OriginalObject = Newtonsoft.Json.JsonConvert.SerializeObject(original, settings),
                UserId = userId,
                Reason = reason
            };

            _storage.AddTattle(tattle);
        }

        private Dictionary<string, object> Shred(DbPropertyValues values)
        {
            return values.PropertyNames.ToDictionary(p => p, p => values[p]);
        }
    }
}
