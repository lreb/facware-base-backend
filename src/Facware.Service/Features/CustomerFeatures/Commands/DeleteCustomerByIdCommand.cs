// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Facware.Persistence;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;

// namespace Facware.Service.Features.CustomerFeatures.Commands
// {
//     public class DeleteCustomerByIdCommand : IRequest<int>
//     {
//         public int Id { get; set; }
//         public class DeleteCustomerByIdCommandHandler : IRequestHandler<DeleteCustomerByIdCommand, int>
//         {
//             private readonly IApplicationDbContext _context;
//             public DeleteCustomerByIdCommandHandler(IApplicationDbContext context)
//             {
//                 _context = context;
//             }
//             public async Task<int> Handle(DeleteCustomerByIdCommand request, CancellationToken cancellationToken)
//             {
//                 var customer = await _context.Customers.Where(a => a.Id == request.Id).FirstOrDefaultAsync();
//                 if (customer == null) return default;
//                 _context.Customers.Remove(customer);
//                 await _context.SaveChangesAsync();
//                 return customer.Id;
//             }
//         }
//     }
// }