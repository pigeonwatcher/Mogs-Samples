#if ANDROID
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Mogs
{
    public partial class Game1 : Game
    {
        private DeviceLocationService deviceLocationService;

        private void SetPlatformSettings()
        {
            Debug.WriteLine("-- ANDROID VERSION --");

            deviceLocationService = new DeviceLocationService();
            deviceLocationService.Initialise();

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 2184; // Minus navbar
            graphics.ApplyChanges();

            Grid.Initialise(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }
    }
}
#endif

