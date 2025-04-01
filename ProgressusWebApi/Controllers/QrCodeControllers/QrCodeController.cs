using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.QrCodeDtos;
using ProgressusWebApi.Services.QrCodeServices;

namespace ProgressusWebApi.Controllers.QrCodeControllers
{
	[ApiController]
	[Route("api/qr")]
	public class QrCodeController : ControllerBase
	{
		private readonly QrCodeService _qrCodeService;

		public QrCodeController(QrCodeService qrCodeService)
		{
			_qrCodeService = qrCodeService;
		}

		[HttpPost("agregarCodigo")]
		public async Task<IActionResult> AddQrCode([FromQuery] string payload)
		{
			await _qrCodeService.AddQrCodeAsync(payload);
			return Ok(new { message = "QR Code added successfully" });
		}

		[HttpGet("verificarCodigo")]
		public async Task<IActionResult> VerifyQrCode([FromQuery] string payload)
		{
			bool exists = await _qrCodeService.VerifyQrCodeAsync(payload);
			return Ok(new { exists });
		}
	}
}
