using System.Threading.Tasks;
using CarSale.Persistence;

namespace CarSale.Core {
    public class UnitOfWork : IUnitOfWork {
        private readonly CarDbContext context;
        public UnitOfWork (CarDbContext context) {
            this.context = context;

        }
        public async Task CompleteAsync () {
            await context.SaveChangesAsync ();
        }
    }
}