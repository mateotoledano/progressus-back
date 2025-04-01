using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Models.CodigoQRModels;

namespace ProgressusWebApi.Repositories.QrCodeRepositories
{
	public class QrCodeRepository
	{
		private readonly ProgressusDataContext _context;

		public QrCodeRepository(ProgressusDataContext context) {
			_context = context;
		}

		public async Task AddQrCodeAsync(string payload)
		{
			var qrCode = new CodigoQR { Payload = payload };
			_context.CodigosQR.Add(qrCode);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsAsync(string payload)
		{
			return await _context.CodigosQR.AnyAsync(q => q.Payload == payload);
		}
	}
}
