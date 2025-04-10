using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Encryption;
using Newtonsoft.Json;

namespace Core.Models
{
    public record Enquiry(Guid Id, Guid ConsumerId, string SubscriberName, string SubscriberContact, string EnquiryTypeId, DateTimeOffset RecordDate)
          : IHasEncryptionKey
    {
        [JsonIgnore]
        public string EncryptionKey => Id.ToString();
    }
}