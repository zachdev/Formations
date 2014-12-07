﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Formations
{
    class GameLobby : IUpdateDraw
    {
        private static readonly int WINDOW_WIDTH = 300;
        private static readonly int WINDOW_HEIGHT = 430;

        private ConnectionManager connectionManager;
        private static GameLobby gameLobbyInstance;
        private Person _person;
        private Formations formation;
        private Manager uiManager;
        private bool _challengeAccepted = false;

        // GUI Stuff
        private Panel chatPanel;
        private ScrollBar chatScrollbar;
        public TextBox chatHistoryTextbox;
        private TextBox inputTextBox;
        public ListBox playerlist;
        private Button chatSendButton;
        private Button challengeButton;
        private int count = 1;

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
        public Person person 
        { 
            get 
            { 
                return _person; 
            }
            private set 
            {
                _person = value; 
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

            playerlist = new ListBox(uiManager);
            playerlist.SetPosition(1100, 8);
            playerlist.SetSize(100, 200);
            
            playerlist.Init();

            challengeButton = new Button(uiManager);
            challengeButton.SetPosition(1100, 220);
            challengeButton.SetSize(100, 28);
            challengeButton.Text = "Challenge";
            challengeButton.BackColor = Color.Indigo; 
            challengeButton.Click += challengeButton_Click;



            chatPanel = new Panel(uiManager);
            chatScrollbar = new ScrollBar(uiManager, Orientation.Vertical);
            chatHistoryTextbox = new TextBox(uiManager);
            inputTextBox = new TextBox(uiManager);
            chatSendButton = new Button(uiManager);



            // Main chat panel
            chatPanel.SetSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            //chatPanel.SetPosition(800, 10);

            // Scrollbar
            chatScrollbar.SetSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            chatScrollbar.SetPosition(0, 0);

            // History text area
            chatHistoryTextbox.SetSize(300, 400);
            chatHistoryTextbox.SetPosition(0, 0);
            chatHistoryTextbox.WordWrap = true;
            chatHistoryTextbox.Mode = TextBoxMode.Multiline;
            chatHistoryTextbox.ReadOnly = true;
            chatHistoryTextbox.CaretVisible = false;
            chatHistoryTextbox.TextColor = Color.Azure;

            // Input box
            inputTextBox.SetSize(370, 30);
            inputTextBox.SetPosition(0, 400);
            inputTextBox.KeyDown += new KeyEventHandler(this.handleTextInput);

            // Send button
            chatSendButton.SetSize(80, 30);
            chatSendButton.SetPosition(220, 400);
            chatSendButton.Text = "Send";
            chatSendButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.sendMessage);

            // Now initialize them all
            chatPanel.Init();
            chatScrollbar.Init();
            chatHistoryTextbox.Init();
            inputTextBox.Init();
            chatSendButton.Init();

            // Add all the components to the gui manager
            chatScrollbar.Add(chatHistoryTextbox);
            chatPanel.Add(chatScrollbar);
            chatPanel.Add(inputTextBox);
            chatPanel.Add(chatSendButton);

            uiManager.Add(chatPanel);
            uiManager.Add(challengeButton);
            uiManager.Add(playerlist);

            connectionManager = ConnectionManager.getInstance();
            connectionManager.setUpChat(chatHistoryTextbox);

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
        public void updatePlayersList(Person newPerson)
        {
            if (!playerlist.Items.Contains(newPerson))
            {
                playerlist.Items.Add(newPerson);

            }

            
        }
        private void handleTextInput(object sender, TomShane.Neoforce.Controls.KeyEventArgs e)
        {
            if (e.Key == Keys.Enter)
            {
                this.sendMessage(sender, e);
            }
        }

        private void sendMessage(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            connectionManager.sendMessage(inputTextBox.Text);
            inputTextBox.Text = "";
        }

        public void connectToServer()
        {
            //connects back to the server with already logged in Person when game is over
        }
        public void disconnectFromServer()
        {
            //disConnects from server when challenge is accepted
        }


        public void update()
        {
            if (count >= 1000)
            {
                playerlist.Items.Clear();
                count = 1;
            }
            if (count % 250 == 0)
            {
                connectionManager.sendPerson(person);
            }
            count++;
        }

        public void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
