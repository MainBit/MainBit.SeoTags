//using Orchard.Environment;
//using Orchard.Tasks;
//using System.Web.Routing;

//namespace MainBit.Common.Routing
//{
//    public class LowercaseRouting : IOrchardShellEvents, IBackgroundTask
//    {
//        private readonly RouteCollection _routeCollection;

//        public LowercaseRouting(
//            RouteCollection routeCollection)
//        {
//            _routeCollection = routeCollection;
//        }
//        void IOrchardShellEvents.Activated()
//        {
//            _routeCollection.LowercaseUrls = true;
//        }

//        void IOrchardShellEvents.Terminating()
//        {
//        }

//        void IBackgroundTask.Sweep()
//        {
//            _routeCollection.LowercaseUrls = true;
//        }
//    }
//}