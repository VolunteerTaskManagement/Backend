using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGuide.Application.CQRS.MediaFiles
{
    public class StreamImgDTO
    {
        public StreamImgDTO(Stream stream, string contentType)
        {
            Stream = stream;
            ContentType = contentType;
        }
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
    }
}
