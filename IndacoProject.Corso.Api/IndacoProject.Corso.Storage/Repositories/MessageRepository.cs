using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
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
    public class MessageRepository : IMessageRepository
    {
        protected readonly ILogger<MessageRepository> _logger;
        protected readonly ApplicationDbContext _dbContext;
        private bool disposedValue;

        public MessageRepository(ILogger<MessageRepository> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Message> Create(Message message, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == message)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(message)}");
                throw new ArgumentException(nameof(message));
            }
            Guid guid = Guid.NewGuid();
            if (string.IsNullOrEmpty(message.Id))
                message.Id = guid.ToString();

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return message;
        }

        public async Task<Message> Delete(string id, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(id)}");
                throw new ArgumentException(nameof(id));
            }

            Message message = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (null != message)
            {
                _dbContext.Messages.Remove(message);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return message;
        }

        public async Task<Message> DeleteDetached(Message message, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == message)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(message)}");
                throw new ArgumentException(nameof(message));
            }

            _dbContext.Entry(message).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return message;
        }

        public async Task<Message> Edit(Message message, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == message)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(message)}");
                throw new ArgumentException(nameof(message));
            }

            Message searchOrder = await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (null != searchOrder)
            {
                _dbContext.Messages.Update(message);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return message;
        }

        public async Task<Message> EditDetached(Message message, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == message)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(message)}");
                throw new ArgumentException(nameof(message));
            }

            _dbContext.Entry(message).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return message;
        }

        public async Task<Message> Edit(string MessageId, Message Message, CancellationToken token)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(MessageId))
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(MessageId)}");
                throw new ArgumentException(nameof(MessageId));
            }
            if (!(Message != null && Message.Id == MessageId))
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(Message)}");
                throw new ArgumentException(nameof(Message));
            }
            _dbContext.Entry(Message).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync(token);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Messages.Any(s => s.Id == MessageId))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw;
                }
            }
            return Message;
        }



        public async Task<Message> UpSert(Message message, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (null == message)
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(message)}");
                throw new ArgumentException(nameof(message));
            }

            message = await this.Edit(message, cancellationToken);
            if (null == message)
                message = await this.Create(message, cancellationToken);

            return message;
        }

        public async Task<Message> Get(string id, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError($"{this.GetType().Name}::InvalidArgument {nameof(id)}");
                throw new ArgumentException(nameof(id));
            }

            return await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        }

        public async Task<ICollection<Message>> List(int skip, int take, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (take == 0)
                return await _dbContext.Messages.Skip(skip).ToListAsync(cancellationToken);
            else
                return await _dbContext.Messages.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public async Task<int> Count(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return await _dbContext.Messages.CountAsync(cancellationToken);
        }

        protected void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public async Task<List<Message>> ListUnsent(int? attempts, CancellationToken token)
        {
            ThrowIfDisposed();
            return await _dbContext.Messages
                .Where(o => o.Sent == false && (attempts.HasValue ? o.Attempts <= attempts.Value : true))
                .ToListAsync(token);
        }

        public async Task<List<Message>> ListFailed(int take, int skip, int attempts, CancellationToken token)
        {
            ThrowIfDisposed();
            return await _dbContext.Messages
                .Where(o => o.Sent == false && o.Attempts > attempts)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                // TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire l'override del finalizzatore
                // TODO: impostare campi di grandi dimensioni su Null
                disposedValue = true;
            }
        }

        // // TODO: eseguire l'override del finalizzatore solo se 'Dispose(bool disposing)' contiene codice per liberare risorse non gestite
        // ~OrderRepository()
        // {
        //     // Non modificare questo codice. Inserire il codice di pulizia nel metodo 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Non modificare questo codice. Inserire il codice di pulizia nel metodo 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
