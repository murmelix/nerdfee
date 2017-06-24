using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nerdfee.Models
{
    public class ArticleImage
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public short OrderBy { get; set; }
        public byte[] Data { get; set; }
    }
}
