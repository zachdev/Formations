using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Formations
{
    class GameLobby
    {
        private static GameLobby gameLobbyInstance;
        private Person person;
        private Formations formation;
        private Manager uiManager;
        private Chat lobbyChat;
        private bool _challengeAccepted = false;
        private ListBox playerlist;
        private Button challengeButton;

        public bool IsChallengeAccepted
        {
            get
            {
                return _challengeAccepted;
            }
            private set
            {
                _challengeAccepted = value;
            }
        }
        private GameLobby()
        {
            
        }
        public static GameLobby getInstance()
        {
            if (gameLobbyInstance == null)
            {
                gameLobbyInstance = new GameLobby();
            }
            return gameLobbyInstance;
        }
        public void init(Formations formation, Manager uiManager, Person person)
        {
            this.formation = formation;
            this.uiManager = uiManager;
            this.person = person;

            lobbyChat = new Chat();
            lobbyChat.init(uiManager);

            playerlist = new ListBox(uiManager);
            playerlist.SetPosition(620, 8);
            playerlist.SetSize(200, 400);
            playerlist.Text = "Josh";
            playerlist.Items.AddRange(new Person[] {
            new Person("Josh", "Password"),
            new Person("Dan", "Password")});
            playerlist.Init();

            challengeButton = new Button(uiManager);
            challengeButton.SetPosition(517, 326);
            challengeButton.SetSize(93, 28);
            challengeButton.Text = "Challenge";
            challengeButton.BackColor = Color.Indigo; 
            challengeButton.Click += challengeButton_Click;

            uiManager.Add(challengeButton);
            uiManager.Add(playerlist);

        }

        void challengeButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            Person temp = (Person)playerlist.Items.ElementAt<object>(playerlist.ItemIndex);
        }
        public void connectToServer()
        {
            //connects back to the server with already logged in Person
        }
        public void disconnectFromServer()
        {
            //disConnects from server when challenge is accepted
        }
    }
}
