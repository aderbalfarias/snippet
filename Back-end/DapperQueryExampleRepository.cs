using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Example.Business.DataEntities;
using Example.Business.Interfaces.Repositories;
using Example.Infraestrutura.CrossCutting.Support.Extensions;
using Example.Infrastructure.Data.DapperConfig;
using Example.Infrastructure.Data.EntityConfig;

namespace Example.Infrastructure.Data.Repositories
{
    public class LogRepository : ILogRepository, IDisposable
    {
        #region Fields
        
        private readonly EntityContext _context = new EntityContext();
        private readonly SqlConnection _contextDapper = new DapperContext().SqlConnection();
        private readonly Log _log = new Log();

        #endregion

        #region Methods

        public void Add(Log entity)
        {
            _context.Log.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Log> Get()
        {
            return _contextDapper
                .Query<Log>("select * from Log")
                .ToList();
        }

        public IEnumerable<Log> Get(Pagination paginar, long? processoId = null, string key = null)
        {
            
            var resultList = _contextDapper.Query<Log, LogType, Usuario, Log>($@"
                    select l.Id, l.TelaId, l.Mensagem, l.Conteudo, l.DataCadastro, l.NomeClasse,
                        l.LogTypeId, lt.Id, lt.Descricao,
                        l.UsuarioId, u.Id, u.Nome, u.Login
                    from Log l
                    inner join LogType lt on lt.Id = l.LogTypeId                
                    left join Usuario u on u.Id = l.UsuarioId
                    where {_log.Where(processoId, key)} 
                    {paginar.GeneretePaginationSql(paginar, "l.Id")}", 
                    (l, lt, u) =>
                    {
                        l.LogType = lt;
                        l.Usuario = u;
                        return l; 
                    }, splitOn: "LogTypeId, UsuarioId")
                .AsQueryable();

            return resultList.ToList();
        }

        public int Count(long? processoId = null, string key = null)
        {
            return _contextDapper
                .Query<int>($@"select Count(l.Id)
                    from Log l
                    inner join LogType lt on lt.Id = l.LogTypeId                
                    left join Usuario u on u.Id = l.UsuarioId
                    where {_log.Where(processoId, key)}")
                .Single();
        }

        public void Dispose()
        {
            _contextDapper.Close();
            _context.Dispose();
        }

        #endregion
    }
}
