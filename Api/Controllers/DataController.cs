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
                var enquiry = new Core.Models.Enquiry(
                        Guid.NewGuid(),
                        request.ConsumerId,
                        request.SubscriberName,
                        request.SubscriberContact,
                        request.EnquiryTypeId.ToString(),
                        request.RecordDate
                    );

                await using var session = _store.LightweightSession();
                session.Store(enquiry);
                await session.SaveChangesAsync();

                return Result<Models.v1.EnquiryResponse>.Success(new Models.v1.EnquiryResponse
                {
                    Id = enquiry.Id,
                    ConsumerId = enquiry.ConsumerId,
                    SubscriberName = enquiry.SubscriberName,
                    SubscriberContact = enquiry.SubscriberContact,
                    EnquiryTypeId = Int32.Parse(enquiry.EnquiryTypeId),
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
                        SubscriberName = x.SubscriberName,
                        SubscriberContact = x.SubscriberContact,
                        EnquiryTypeId = Int32.Parse(x.EnquiryTypeId),
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