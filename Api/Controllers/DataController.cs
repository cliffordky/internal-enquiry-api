using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Coravel.Cache.Interfaces;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IDocumentStore _store;
        private readonly ICache _cache;

        public DataController(ILogger<DataController> logger, IDocumentStore store, ICache cache)
        {
            _logger = logger;
            _store = store;
            _cache = cache;
        }

        [TranslateResultToActionResult]
        [HttpPost("enquiry")]
        public async Task<Result<Models.v1.EnquiryResponse>> AddEnquiry(Models.v1.EnquiryRequest request)
        {
            try
            {
				string hash = Core.Encryption.Hash.GetHashString(request.ConsumerId.ToString() +
                    request.SubscriberId.ToString() +
                    request.EnquiryTypeCode+
                    request.RecordDate);

				await using var session = _store.LightweightSession();
				var existing = await session.Query<Core.Models.Enquiry>().SingleOrDefaultAsync(x => x.Hash == hash);
				if (existing != null)
				{
					return Result<Models.v1.EnquiryResponse>.Error("Enquiry already exists");
				}


				var enquiry = new Core.Models.Enquiry(
                        Guid.NewGuid(),
                        request.ConsumerId,
                        request.SubscriberId,
                        request.EnquiryTypeCode.ToString(),
                        request.RecordDate,
                        hash
                    );

                session.Store(enquiry);
                await session.SaveChangesAsync();

                return Result<Models.v1.EnquiryResponse>.Success(new Models.v1.EnquiryResponse
                {
                    Id = enquiry.Id,
                    ConsumerId = enquiry.ConsumerId,
                    SubscriberId = enquiry.SubscriberId,
                    EnquiryTypeCode = enquiry.EnquiryTypeCode,
                    RecordDate = enquiry.RecordDate
                });
            }
            catch (Exception Ex)
            {
                return Result<Models.v1.EnquiryResponse>.Error(Ex.Message);
            }
        }

        [TranslateResultToActionResult]
        [HttpGet("enquiries")]
        public async Task<Result<List<Models.v1.EnquiryResponse>>> GetEnquiriesForConsumer(Guid ConsumerId)
        {
            try
            {
                await using var session = _store.LightweightSession();
                var enquiries = await session.Query<Core.Models.Enquiry>().Where(x => x.ConsumerId == ConsumerId).ToListAsync();

                return Result<List<Models.v1.EnquiryResponse>>.Success(enquiries.Select(
                    x => new Models.v1.EnquiryResponse
                    {
                        Id = x.Id,
                        ConsumerId = x.ConsumerId,
                        SubscriberId = x.SubscriberId,
                        EnquiryTypeCode = x.EnquiryTypeCode,
                        RecordDate = x.RecordDate
                    }).ToList());
            }
            catch (Exception Ex)
            {
                return Result<List<Models.v1.EnquiryResponse>>.Error(Ex.Message);
            }
        }
    }
}