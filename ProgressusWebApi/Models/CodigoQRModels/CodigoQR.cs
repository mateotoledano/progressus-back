using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgressusWebApi.Models.CodigoQRModels
{
	[Table("CodigoQR")]
	public class CodigoQR
	{
		[Key]
		public string Payload { get; set; } = string.Empty;
	}
}