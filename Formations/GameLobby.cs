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
        private ConnectionManager connectionManager;
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

            connectionManager = ConnectionManager.getInstance();


            lobbyChat = new Chat();
            lobbyChat.init(uiManager);

            playerlist = new ListBox(uiManager);
            playerlist.SetPosition(1100, 8);
            playerlist.SetSize(100, 200);
            playerlist.Items.AddRange(new Person[] {
            new Person("Josh", "Password"),
            new Person("Dan", "Password")});
            playerlist.Init();

            challengeButton = new Button(uiManager);
            challengeButton.SetPosition(1100, 220);
            challengeButton.SetSize(100, 28);
            challengeButton.Text = "Challenge";
            challengeButton.BackColor = Color.Indigo; 
            challengeButton.Click += challengeButton_Click;

            uiManager.Add(challengeButton);
            uiManager.Add(playerlist);

        }

        void challengeButton_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            connectionManager.sendPerson(formation.person);
           // Person temp = (Person)playerlist.Items.ElementAt<object>(playerlist.ItemIndex);//grabs the person that was selected
//sendChallengeRequest(temp);
            //lobbyChat.toggle(sender, e);
        }
        public void sendChallengeRequest(Person person)
        {
            //connectionManager.sendMessage(person);
            uiManager.Remove(playerlist);
            uiManager.Remove(challengeButton);
            this.IsChallengeAccepted = true;
            formation.challengePerson();
        }
        public void updatePlayersList(Person[] persons)
        {
            playerlist.Items.Clear();
            playerlist.Items.AddRange(persons);
        }
        public void connectToServer()
        {
            //connects back to the server with already logged in Person when game is over
        }
        public void disconnectFromServer()
        {
            //disConnects from server when challenge is accepted
        }

    }
}
