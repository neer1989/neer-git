using MinorityDashboard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MinorityDashboard.Data
{
   public  class CommonRepository
    {

        public List<T> GetData<T>() where T : class
        {
            List<T> item = new List<T>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                item = entities.Set<T>().ToList();
            }
            return item;
        }

        public List<T> GetData<T>(Expression<Func<T, bool>> filterexp) where T : class
        {
            List<T> item = new List<T>();
            using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
            {
                item = entities.Set<T>().Where(filterexp).ToList();
            }
            return item;
        }

        public T SaveData<T>(T obj) where T : class
        {
           
            try
            {
                using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
                {
                    var dta = entities.Set<T>().Add(obj);
                    entities.SaveChanges();
                    return obj;
                }
            }
            catch(Exception ex)
            {
                return obj;
            }
           
        }

        public int DeleteData<T>(T obj) where T : class
        {
            try
            {
                using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
                {
                    var dta = entities.Set<T>().Remove(obj);
                    entities.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public T UpdateData<T>(T obj, Expression<Func<T, bool>> filterexp) where T : class
        {
            List<T> item = new List<T>();
            try
            {
                using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
                {
                    var dta = entities.Set<T>().FirstOrDefault<T>(filterexp);  //= obj;
                    dta = obj;
                    entities.SaveChanges();
                    return dta;
                }
            }
            catch (Exception ex)
            {
                return obj;
            }
        }

        //public List<T> GetData<T>(Expression<Func<T,bool>> filterexp) where T : class
        //{
        //    List<T> item = new List<T>();
        //    using (MinorityDasboard_DBEntities entities = new MinorityDasboard_DBEntities())
        //    {
        //        item = entities.Set<T>().Where<T>(filterexp).ToList();
        //    }
        //    return item;
        //}
    }
}
