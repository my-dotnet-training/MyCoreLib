using System;
using System.Collections.Generic;

namespace MyCoreLib.BaseNet.Rest.Model
{
    public abstract class BaseRestRepository<TModel> : IRestRepository<TModel>
        where TModel : IRestModel
    {
        internal List<TModel> _models = new List<TModel>();

        public virtual IEnumerable<TModel> GetAll()
        {
            return _models;
        }
        public virtual TModel Add(TModel item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _models.Add(item);
            return item;
        }

        public virtual TModel Get(int id)
        {
            return this._models.Find(p => p.Id == id);
        }
        public virtual void Remove(int id)
        {
            this._models.RemoveAll(p => p.Id == id);
        }

        public virtual bool Update(TModel item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int index = _models.FindIndex(p => p.Id == item.Id);
            if (index == -1)
                return false;

            _models.RemoveAt(index);
            _models.Add(item);
            return true;
        }
    }
}
