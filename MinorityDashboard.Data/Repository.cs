namespace MinorityDashboard.Data
{
    using MinorityDashboard.DataModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The Repository Implementation.
    /// </summary>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region ---- Fields ----


        private readonly IUnitOfWork _currentUoW;

        #endregion

        #region ---- Constructor ----


        public Repository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("UoW");
            }

            this._currentUoW = unitOfWork;
        }

        #endregion

        #region ---- Repository Implementation ----

        private List<T> GetData<T>() where T : class
        {
            List<T> item = new List<T>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                item = entities.Set<T>().ToList();
            }
            return item;
        }



        public virtual void Add(TEntity entity)
        {
            this._currentUoW.CreateSet<TEntity>().Add(entity);
        }
        public virtual void Remove(TEntity entity)
        {
            var oSet = this._currentUoW.CreateSet<TEntity>();
            oSet.Attach(entity);
            oSet.Remove(entity);
        }
        public virtual void Update(TEntity entity)
        {
            this._currentUoW.SetModified(entity);
        }
        public virtual TEntity Get(int id)
        {
            return this._currentUoW.CreateSet<TEntity>().Find(id);
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            return this._currentUoW.CreateSet<TEntity>().AsEnumerable();
        }

        public virtual IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selectBuilder)
        {
            return this._currentUoW.CreateSet<TEntity>().Select(selectBuilder).AsEnumerable();
        }

        public virtual IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selectBuilder, Expression<Func<TEntity, bool>> predicate)
        {
            return this._currentUoW.CreateSet<TEntity>().Where(predicate).Select(selectBuilder).AsEnumerable();
        }


        public virtual Tuple<IEnumerable<TResult>, int> GetAllAndCount<TResult>(Expression<Func<TEntity, TResult>> selectBuilder, Expression<Func<TEntity, bool>> predicate)
        {
            var oSet = this._currentUoW.CreateSet<TEntity>();

            var filtered = oSet.Where(predicate).Select(selectBuilder).AsEnumerable();
            var count = oSet.Where(predicate).Select(selectBuilder).Count();

            return new Tuple<IEnumerable<TResult>, int>(filtered, count);
        }

     
       

        #endregion
    }
}
