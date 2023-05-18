using TrackSpense.BL;
using TrackSpense.BL.Contracts;
using TrackSpense.BL.Services;

namespace TrackSpense.Server
{
    public class Startup
    {
        public Startup(IServiceCollection service, IConfiguration configuration)
        {
            BLConfig config = new BLConfig(service, configuration);
        }
    }
}
