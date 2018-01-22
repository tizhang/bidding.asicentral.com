using System.Collections.Generic;
using System.Web.Optimization;

namespace Bidding.Web.Helpers
{
    public class AngularPartialsBundle : Bundle
    {
        public AngularPartialsBundle(Dictionary<string, string> modulePath, string virtualPath)
            : base(virtualPath, new IBundleTransform[] { new PartialsTransform(modulePath) })
        {

        }
    }
}
