using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class MouseListener
    {
        private MouseState oldState;
        private Game1 game;
        public MouseListener(MouseState mouseState, Game1 game)
        {
            oldState = mouseState;
            this.game = game;
        }
        public void update(MouseState newState)
        {
            if(oldState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
            {
                game.mousePressed(newState);
            }
            else if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released)
            {
                game.mouseReleased(newState);
            }
 
            oldState = newState; // this reassigns the old state so that it is ready for next time
        }


    }
}
