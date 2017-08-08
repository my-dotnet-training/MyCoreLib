
using System.Collections.Generic;

namespace MyCoreLib.BaseNet.Rest.Model
{
    public interface IRestRepository<TModel>
    {
        IEnumerable<TModel> GetAll();
        TModel Get(int id);
        TModel Add(TModel item);
        void Remove(int id);
        bool Update(TModel item);
    }
}
