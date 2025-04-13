namespace Api.Models.v1
{
    public class EnquiryResponse
    {
        public Guid Id { get; set; }
        public Guid ConsumerId { get; set; }
        public Guid SubscriberId { get; set; }

        public string EnquiryTypeCode { get; set; }
        public DateTimeOffset RecordDate { get; set; }
    }
}