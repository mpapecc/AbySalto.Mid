using System.Text.Json.Serialization;
using AbySalto.Mid.Application.JsonConverters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AbySalto.Mid.Application.Models
{
    [JsonConverter(typeof(IdConverter))]
    public class Id
    {
        [BindNever]
        public int RawId { get; set; }

        [BindRequired]
        public string EncryptedId { get; set; }
        public Id(string encryptedId)
        {
            this.EncryptedId = encryptedId;
            this.RawId = Decrypt(encryptedId);
        }

        public Id(int rawId)
        {
            this.RawId = rawId;
            this.EncryptedId = Encrypt(rawId);
        }

        public static string Encrypt(int id)
        {
            return Convert.ToBase64String(BitConverter.GetBytes(id));
        }

        public static int Decrypt(string encryptedId)
        {
            var bytes = Convert.FromBase64String(encryptedId);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
