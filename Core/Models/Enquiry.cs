using Core.Encryption;
using Newtonsoft.Json;

namespace Core.Models
{
    public record Enquiry(Guid Id, Guid ConsumerId, Guid SubscriberId, string EnquiryTypeId, DateTimeOffset RecordDate)
          : IHasEncryptionKey
    {
        [JsonIgnore]
        public string EncryptionKey => Id.ToString();
    }
}