using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos.Master.Companies.zatca
{
    class CertificateRequest
    {
        public string CommonName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationUnitName { get; set; }
        public string CountryName { get; set; }
        public string SerialNumber { get; set; }
        public string OrganizationIdentifier { get; set; }
        public string InvoiceType { get; set; }
        public string Location { get; set; }
        public string BusinessCategory { get; set; }
        public string OTP { get; set; }
        //public Mode Mode { get; set; }
    }
}
