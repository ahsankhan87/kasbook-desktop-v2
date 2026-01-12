using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos.Master.Companies.zatca
{
    public class ZatcaSignatureData
    {
        public string SignatureValueBase64 { get; set; }
        public string PublicKeyBase64 { get; set; }
        public string CertificateSignatureBase64 { get; set; }
    }

}
