using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Models
{
    public class KeyPair
    {
        public Guid KeyPairId { get; set; }
        public string EncryptedPrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
