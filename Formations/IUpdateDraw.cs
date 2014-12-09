﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    [Serializable]
    public interface IUpdateDraw
    {
        void update();
        void draw(SpriteBatch spriteBatch);
    }
}
