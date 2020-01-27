using Squirrel;
using System.Configuration;
using System.Linq;

namespace SquirrelTest
{
    public class ApplicationUpdateManager : IApplicationUpdateManager
    {
        public async void UpdateApplication()
        {
            bool applicationPendingUpdate = false;
            using (var updateManager = new UpdateManager(ConfigurationManager.AppSettings["UpdaterLocation"].ToString()))
            {
                var updateInfo = await updateManager.CheckForUpdate();

                if (updateInfo.ReleasesToApply.Any())
                {
                    await updateManager.DownloadReleases(updateInfo.ReleasesToApply);
                    await updateManager.ApplyReleases(updateInfo);
                    await updateManager.CreateUninstallerRegistryEntry();
                    applicationPendingUpdate = true;
                }
            }

            if (applicationPendingUpdate)
            {
                UpdateManager.RestartApp();
            }
        }
    }
}
