using Shiny.Jobs;

using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Microsoft.DataSync.Client
{
    public class SyncJob : IJob
    {
        public async Task Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
        }
    }
}
