using System;

namespace DDDEastAnglia.Models
{
    public sealed class SponsorLogo
    {
        public byte[] Data { get; }

        public string ContentType { get; }

        public SponsorLogo(byte[] data, string contentType)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }

            Data = data;
            ContentType = contentType;
        }
    }
}
