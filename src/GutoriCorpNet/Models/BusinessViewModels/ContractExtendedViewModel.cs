using GutoriCorp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Models.BusinessViewModels
{
    public class ContractExtendedViewModel : ContractViewModel
    {
        public OwnerViewModel Lessor { get; set; }

        public DriverViewModel Lessee { get; set; }

        public DriverLicenseViewModel DriverLicense { get; set; }

        public DriverLicenseViewModel TlcLicense { get; set; }

        public VehicleViewModel Vehicle { get; set; }
    }
}
