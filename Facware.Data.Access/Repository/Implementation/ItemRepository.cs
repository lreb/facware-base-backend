using System;
using System.Linq;
using Facware.Data.Access.Base.Base;
using Facware.Data.Access.Repository.Interface;
using Facware.Models;

namespace Facware.Data.Access.Repository.Implementation
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(FacwareDbContext context) : base(context)
        {
        }

        public IEquatable<Item> GetSpecialItems(int count)
        {
            return (IEquatable<Item>)_context.Items.OrderByDescending(x => x.Name).Take(count).ToList();
        }
    }
}
