using Microsoft.CAP.ICEM.ApiClient;
using Microsoft.CAP.ICEM.Entities;
using Microsoft.CAP.PAM.ICEM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CAP.ICEM.Business.Interface
{
   public interface IPAMRequestService
    {
        public Task<ApiResponse<PAMResponse>> Create(PAMRequest pAMRequest);
    }
}
