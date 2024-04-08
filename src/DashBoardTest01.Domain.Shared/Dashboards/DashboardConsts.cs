namespace DashBoardTest01.Dashboards
{
    public static class DashboardConsts
    {
        private const string DefaultSorting = "{0}Description asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Dashboard." : string.Empty);
        }

    }
}