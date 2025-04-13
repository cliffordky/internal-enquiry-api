namespace Api.Models.v1
{
    public class EnquiryRequest
    {
        public Guid ConsumerId { get; set; }
        public Guid SubscriberId { get; set; }

        public string EnquiryTypeCode { get; set; }
        public DateTimeOffset RecordDate { get; set; }
    }
}