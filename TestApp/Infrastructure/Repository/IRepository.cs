using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApp.Infrastructure.Repository;

public interface IRepository<T>
{
    public Task<List<T>> ListAsync();
}