using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightSpot.Integrations.Infrastructure.BlobStorage;

public class SchoolFeedBlobService : BaseBlobService
{
    readonly string nameSchoolFeedContainer = Environment.GetEnvironmentVariable("SchoolFeedContainerName") ?? string.Empty;
}
