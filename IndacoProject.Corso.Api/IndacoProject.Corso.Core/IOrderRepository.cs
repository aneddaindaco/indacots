using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Entities.Northwind;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Core
{
    public interface IOrderRepository : IDisposable
    {
        Task<int> Count(CancellationToken cancellationToken);
        Task<Order> Create(Order order, CancellationToken cancellationToken);
        Task<Order> Delete(int id, CancellationToken cancellationToken);
        Task<Order> DeleteDetached(Order order, CancellationToken cancellationToken);
        Task<Order> Edit(Order order, CancellationToken cancellationToken);
        Task<Order> EditDetached(Order order, CancellationToken cancellationToken);
        Task<Order> Get(int id, CancellationToken cancellationToken);
        Task<ICollection<Order>> List(int skip, int take, CancellationToken cancellationToken);
        Task<Order> UpSert(Order order, CancellationToken cancellationToken);
    }
}