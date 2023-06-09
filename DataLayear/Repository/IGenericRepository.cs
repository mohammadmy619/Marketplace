using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Repository
{
   public interface IGenericRepository<Tentity> :IAsyncDisposable where Tentity:BaseEntity
    {
        IQueryable<Tentity> GetQuery();
        Task AddEntity(Tentity tentity);
        Task AddRangeEntities(List<Tentity> entities);
        Task<Tentity> GetEntityById(long entityId);
        void EditEntity(Tentity entity);
        void DeleteEntity(Tentity entity);
        Task DeleteEntity(long entityId);
        void DeletePermanent(Tentity entity);
        void DeletePermanentEntities(List<Tentity> entities);
        void ISDeletePermanentEntities(List<Tentity> entities);

        Task DeletePermanent(long entityId);
        Task SaveChanges();
      
    }
}
