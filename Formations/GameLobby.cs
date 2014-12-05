using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Formations
{
    class GameLobby
    {
        private Formations formation;
        private Manager uiManager;
       private bool isLoggedIn = false;
        private Button challengeButton;
        public GameLobby(Manager uiManager)
        {
            this.uiManager = uiManager;
        }

        public void init(Formations formation)
        {
            this.formation = formation;


        }

    }
}
