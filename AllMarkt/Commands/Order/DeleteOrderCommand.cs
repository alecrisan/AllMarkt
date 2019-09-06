using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.Order
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler : AsyncRequestHandler<DeleteOrderCommand>
    {
        private readonly AllMarktContext _allmarktContext;

        public DeleteOrderCommandHandler(AllMarktContext allMarktContext)
        {
            _allmarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _allmarktContext.Orders.Include(o=>o.OrderItems).FirstOrDefaultAsync(
                x => x.Id == request.Id, cancellationToken);

            if (order != null)
            {
                _allmarktContext.Orders.Remove(order);
                await _allmarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
