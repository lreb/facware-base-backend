using System;
using Facware.Data.Access.Base.Base;
using Facware.Models;

namespace Facware.Data.Access.Repository.Interface
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        IEquatable<Item> GetSpecialItems(int count);
    }
}
