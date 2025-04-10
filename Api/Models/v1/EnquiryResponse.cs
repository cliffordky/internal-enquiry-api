namespace Api.Models.v1
{
    public class EnquiryResponse
    {
        public Guid Id { get; set; }
        public Guid ConsumerId { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberContact { get; set; }
        public int EnquiryTypeId { get; set; }
        public DateTimeOffset RecordDate { get; set; }
    }
}