namespace Tienda.Web.Data.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	public class Product : IEntity
	{
		public int Id { get; set; }
		[MaxLength(50)]
		[Required]
		public string Name { get; set; }
		//Aplicando formato para moneda {0:C2} currency 2 
		[Column(TypeName = "decimal(9,2)")]
		[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
		public decimal Price { get; set; }

		[Display(Name = "Image")]
		public string ImageUrl { get; set; }

		[Display(Name = "Last Purchase")]
		public DateTime? LastPurchase { get; set; }

		[Display(Name = "Last Sale")]
		public DateTime? LastSale { get; set; }

		[Display(Name = "Is Availabe?")]
		public bool IsAvailabe { get; set; }

		//Formato numerico de 2 
		[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
		public double Stock { get; set; }

		//Relacion uno a muchos (un usuario, muchos productos)
		public User User { get; set; }

		public string ImageFullPath {

			get
			{
				if (string.IsNullOrEmpty(this.ImageUrl))
				{
					return null;
				}

				return $"https://tiendita.azurewebsites.net{this.ImageUrl.Substring(1)}"; 
				
			}
		
		}

	}

}
