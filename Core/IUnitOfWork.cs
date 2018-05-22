using System.Threading.Tasks;

namespace CarSale.Core
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}