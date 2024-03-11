#if DESKTOP
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Mogs
{
    public partial class Game1 : Game
    {
        private void SetPlatformSettings()
        {
            Debug.WriteLine("-- DESKTOP VERSION --");

            graphics.PreferredBackBufferWidth = 486; // 486
            graphics.PreferredBackBufferHeight = 864; // 864
            graphics.ApplyChanges();

            Grid.Initialise(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }
    }
}
#endif
