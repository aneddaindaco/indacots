using IndacoProject.Corso.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Core
{
    public interface IMessageRepository: IDisposable
    {
        Task<int> Count(CancellationToken cancellationToken);
        Task<Message> Create(Message message, CancellationToken cancellationToken);
        Task<Message> Delete(string id, CancellationToken cancellationToken);
        Task<Message> DeleteDetached(Message message, CancellationToken cancellationToken);
        Task<Message> Edit(Message message, CancellationToken cancellationToken);
        Task<Message> Edit(string MessageId, Message Message, CancellationToken token);
        Task<Message> EditDetached(Message message, CancellationToken cancellationToken);
        Task<Message> Get(string id, CancellationToken cancellationToken);
        Task<ICollection<Message>> List(int skip, int take, CancellationToken cancellationToken);
        Task<List<Message>> ListFailed(int take, int skip, int attempts, CancellationToken token);
        Task<List<Message>> ListUnsent(int? attempts, CancellationToken token);
        Task<Message> UpSert(Message message, CancellationToken cancellationToken);
    }
}