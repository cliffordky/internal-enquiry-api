namespace Api.Models.v1
{
    public class EnquiryRequest
    {
        public Guid ConsumerId { get; set; }
        public Guid SubscriberId { get; set; }

        public int EnquiryTypeId { get; set; }
        public DateTimeOffset RecordDate { get; set; }
    }
}