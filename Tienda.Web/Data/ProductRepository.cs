namespace Tienda.Web.Data
{
	using Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly DataContext context;

		public ProductRepository(DataContext context) : base(context)
		{
			this.context = context;
		}

		public IQueryable GetAllWithUsers()
		{
			return this.context.Products.Include(x=>x.User);
		}
	}

}
