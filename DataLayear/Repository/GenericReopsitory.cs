using MarketPlace.DataLayer.Context;
using MarketPlace.DataLayer.Entities.Commen;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Repository
{
   public class GenericReopsitory<Tentity>: IGenericRepository<Tentity> where Tentity :BaseEntity
    {
        private readonly MarketDBContext _Context;

        private readonly DbSet<Tentity> _dbset;

        public GenericReopsitory(MarketDBContext Context)
        {
            _Context = Context;
            this._dbset = Context.Set<Tentity>();
        }

  
        public IQueryable<Tentity> GetQuery()
        {
            return  _dbset.AsQueryable();
        }

        public async Task AddEntity(Tentity tentity)
        {
            tentity.CreateDate = DateTime.Now;
            tentity.LastUpdateDate = tentity.CreateDate;
            await _dbset.AddAsync(tentity);
        }

        public async Task AddRangeEntities(List<Tentity> entities)
        {
            foreach (var entity in entities)
            {
                await AddEntity(entity);
            }
        }

        public async Task<Tentity> GetEntityById(long entityId)
        {
            return await _dbset.SingleOrDefaultAsync(s => s.Id == entityId);
        }

        public void EditEntity(Tentity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            _dbset.Update(entity);
        }

        public void DeleteEntity(Tentity entity)
        {
            entity.IsDelete = true;

            EditEntity(entity);
        }

        public async Task DeleteEntity(long entityId)
        {
            Tentity entity = await GetEntityById(entityId);
            if (entity != null) DeleteEntity(entity);
        }

        public void DeletePermanent(Tentity entity)
        {
            _dbset.Remove(entity);
        }

        public void DeletePermanentEntities(List<Tentity> entities)
        {
            _Context.RemoveRange(entities);
        }

        public void ISDeletePermanentEntities(List<Tentity> entities)
        {
            foreach (var item in entities)
            { 
                 DeleteEntity(item);
            }

        }
        public async Task DeletePermanent(long entityId)
        {
            Tentity entity = await GetEntityById(entityId);
            if (entity != null) DeletePermanent(entity);
        }

        public async Task SaveChanges()
        {
            await _Context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_Context != null)
            {
                await _Context.DisposeAsync();
            }
        }

   
    } 
}
