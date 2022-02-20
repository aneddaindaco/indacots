using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Entities.Northwind;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Storage.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly ILogger<OrderRepository> _logger;
        protected readonly ApplicationDbContext _dbContext;
        private bool disposedValue;

        public OrderRepository(ILogger<OrderRepository> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Order> Create(Order order, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == order)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(order)}");
                throw new ArgumentException(nameof(order));
            }

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return order;
        }

        public async Task<Order> Delete(int id, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (id == 0)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(id)}");
                throw new ArgumentException(nameof(id));
            }

            Order order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id, cancellationToken);
            if (null != order)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return order;
        }

        public async Task<Order> DeleteDetached(Order order, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == order)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(order)}");
                throw new ArgumentException(nameof(order));
            }

            _dbContext.Entry(order).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return order;
        }

        public async Task<Order> Edit(Order order, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == order)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(order)}");
                throw new ArgumentException(nameof(order));
            }

            Order searchOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == order.OrderId, cancellationToken);
            if (null != searchOrder)
            {
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return order;
        }

        public async Task<Order> EditDetached(Order order, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == order)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(order)}");
                throw new ArgumentException(nameof(order));
            }

            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return order;
        }

        public async Task<Order> UpSert(Order order, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == order)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(order)}");
                throw new ArgumentException(nameof(order));
            }

            order = await this.Edit(order, cancellationToken);
            if (null == order)
                order = await this.Create(order, cancellationToken);

            return order;
        }

        public async Task<Order> Get(int id, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (id == 0)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(id)}");
                throw new ArgumentException(nameof(id));
            }

            return await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id, cancellationToken);

        }

        public async Task<ICollection<Order>> List(int skip, int take, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (take == 0)
                return await _dbContext.Orders.Skip(skip).ToListAsync(cancellationToken);
            else
                return await _dbContext.Orders.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public async Task<int> Count(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return await _dbContext.Orders.CountAsync(cancellationToken);
        }

        protected void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
