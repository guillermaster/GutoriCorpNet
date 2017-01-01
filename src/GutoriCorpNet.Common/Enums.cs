using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GutoriCorpNet.Common
{
    public class Enums
    {
        public enum GeneralCatalog
        {
            ContractType = 1,
            GeneralStatus = 2,
            ContractFrequency = 3,
            ContractLateFeeType = 4,
            VehicleBodyHull = 5,
            Colors = 6,
            Fuels = 7,
            Types = 8,
            PersonType = 9
        }

        public enum GeneralStatus
        {
            Active = 3,
            Inactive = 4
        }

        public enum PaymentFrequency
        {
            Weekly = 5,
            Monthly = 6
        }

        public enum PersonType
        {
            NaturalPerson = 23,
            BusinessPerson = 24
        }

        public enum DriverLicenseType
        {
            DriverLicense = 25,
            TlcLicense = 26
        }
    }
}
