using System.IO;
using DevExpress.DashboardWeb;

namespace ASPNETCoreDashboardAngular {
    public class CustomDashboardFileStorage : DashboardFileStorage {
        public CustomDashboardFileStorage(string workingDirectory)
            : base(workingDirectory) {
        }
        public void DeleteDashboard(string dashboardID) {
            var dashboardPath = base.ResolveFileName(dashboardID);
            if (File.Exists(dashboardPath))
                File.Delete(dashboardPath);
        }
    }
}