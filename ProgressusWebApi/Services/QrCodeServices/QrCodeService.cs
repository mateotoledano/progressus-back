using ProgressusWebApi.Repositories.QrCodeRepositories;

namespace ProgressusWebApi.Services.QrCodeServices
{
	public class QrCodeService
	{
		private readonly QrCodeRepository _repository;

		public QrCodeService(QrCodeRepository repository)
		{
			_repository = repository;
		}

		public async Task AddQrCodeAsync(string payload)
		{
			await _repository.AddQrCodeAsync(payload);
		}

		public async Task<bool> VerifyQrCodeAsync(string payload)
		{
			return await _repository.ExistsAsync(payload);
		}
	}
}
