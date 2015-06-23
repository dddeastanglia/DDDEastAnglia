using System;

namespace DDDEastAnglia.Models
{
    public sealed class SponsorLogo
    {
        public byte[] Data { get { return data; } }
        private readonly byte[] data;

        public string ContentType { get { return contentType; } }
        private readonly string contentType;

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

            this.data = data;
            this.contentType = contentType;
        }
    }
}
