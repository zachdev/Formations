using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Formations
{
    public interface IGame : IUpdateDraw, IMouseListener 
    {
        void init(Manager uiManager, GraphicsDevice graphicsDevice, string gameName, bool isHost);
        void update();
        void draw(SpriteBatch spriteBatch);

    }
}
