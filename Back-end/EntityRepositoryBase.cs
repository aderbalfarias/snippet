using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Example.Infrastructure.Data.Repositories
{
    public class EntityRepositoryBase<TEntity> : IEntityRepositoryBase<TEntity> where TEntity : class
    {
        #region Fields

        //ReSharper disable once InconsistentNaming
        protected readonly EntityContext _context = new EntityContext();
        private readonly ILogRepository _logRepository = new LogRepository();

        #endregion
        
        #region Methods

        public void Add(TEntity objModel, long userId)
        {
            _context.Set<TEntity>().Add(objModel);
            _context.SaveChanges();

            _logRepository.Add(new Log().GeneratedForEntity(userId, objModel, LogTypeResource.Insert, true));
        }

        /// <summary>
        /// Retorna valor da propriedade informada, por exemplo a Pk
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="userId">Código do usuário</param>
        /// <param name="property">propriedade para retorno, deve conter na objModel</param>
        /// <returns></returns>
        public object Add(TEntity objModel, long userId, string property)
        {
            _context.Set<TEntity>().Add(objModel);
            _context.SaveChanges();

            object result = objModel.GetType()
                .GetProperty(property).GetValue(objModel);

            _logRepository.Add(new Log().GeneratedForEntity(userId, objModel, LogTypeResource.Insert, true));

            return result;
        }

        public void AddRange(IList<TEntity> objModel, long userId)
        {
            _context.Set<TEntity>().AddRange(objModel);
            _context.SaveChanges();

            _logRepository.Add(new Log().GeneratedForEntity(userId, objModel, LogTypeResource.Insert, true));
        }

        public TEntity GetId(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetIdAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public int Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public void Update(TEntity objModel, long userId, bool openContextInRequest = false, bool referenceCircular = false)
        {
            if (openContextInRequest)
            {
                _context.Set<TEntity>().AddOrUpdate(objModel);
            }
            else
            {
                _context.Entry(objModel).State = EntityState.Modified;
            }

            _context.SaveChanges();
            _logRepository.Add(new Log().GeneratedForEntity(userId, objModel, LogTypeResource.Update, referenceCircular));
        }

        /// <summary>
        /// Update de objetos com filhos
        /// </summary>
        /// <param name="objModel"></param>
        /// <param name="userId"></param>
        /// <param name="collectionList"></param>
        /// <param name="objList"></param>
        public void UpdateWithCollection(TEntity objModel, long userId, IEnumerable<Type> collectionList, IEnumerable<Type> objList)
        {
            if (collectionList != null)
                foreach (var collection in collectionList)
                {
                    foreach (var item in objModel.GetType().GetProperties().Where(w => w.PropertyType == collection))
                    {
                        foreach (var obj in (ICollection) item.GetValue(objModel))
                        {
                            var state = (long) obj.GetType()
                                .GetProperty("Id").GetValue(obj) == 0
                                    ? EntityState.Added
                                    : EntityState.Modified;

                            _context.Entry(obj).State = state;
                            //Todo: Não pode abrir um novo contexto após submit
                        }
                    }
                }

            if (objList != null)
                foreach (var obj in objList)
                {
                    foreach (var item in objModel.GetType().GetProperties().Where(w => w.PropertyType == obj))
                    {
                        var objItem = item.GetValue(objModel);

                        var state = (long)objItem.GetType()
                            .GetProperty("Id").GetValue(objItem) == 0
                                ? EntityState.Added
                                : EntityState.Modified;

                        _context.Entry(objItem).State = state;
                        //Todo: Não pode abrir um novo contexto após submit
                    }
                }

            _context.Set<TEntity>().AddOrUpdate(objModel);
            _context.SaveChanges();
            
            _logRepository.Add(new Log().GeneratedForEntity(userId, objModel, LogTypeResource.Update, true));
        }

        public void Remove(TEntity objModel, long userId)
        {
            _context.Set<TEntity>().Remove(objModel);
            _context.SaveChanges();

            _logRepository.Add(new Log().GeneratedForEntity(userId, objModel, LogTypeResource.Delete));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
