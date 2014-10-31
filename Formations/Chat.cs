using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TomShane.Neoforce.Controls;

namespace Formations
{
    class Chat : IUpdateDraw
    {
        private Manager theManager;

        private bool isVisible;

        // GUI Stuff
        private Panel chatPanel;
        private ScrollBar chatScrollbar;
        private TextBox chatHistoryTextbox;
        private TextBox inputTextBox;
        private Button chatSendButton;

        /**
         * Constructor. Must pass in Neoforce manager.
         */
        public Chat() {}

        public void init(Manager manager)
        {
            theManager = manager;
            manager.SetSkin(new Skin(theManager, "Blue"));

            chatPanel = new Panel(theManager);
            chatScrollbar = new ScrollBar(theManager, Orientation.Vertical);
            chatHistoryTextbox = new TextBox(theManager);
            inputTextBox = new TextBox(theManager);
            chatSendButton = new Button(theManager);

            // Now initialize them all
            chatPanel.Init();
            chatScrollbar.Init();
            chatHistoryTextbox.Init();
            inputTextBox.Init();
            chatSendButton.Init();

            // Main chat panel
            chatPanel.SetSize(300, 430);
            chatPanel.SetPosition(800, 10);

            // Scrollbar
            chatScrollbar.SetSize(300, 400);
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

            // Add all the components to the gui manager
            chatScrollbar.Add(chatHistoryTextbox);
            chatPanel.Add(chatScrollbar);
            chatPanel.Add(inputTextBox);
            chatPanel.Add(chatSendButton);
        }

        public void show()
        {
            theManager.Add(chatPanel);
            isVisible = true;
        }

        public void hide()
        {
            theManager.Remove(chatPanel);
            isVisible = false;
        }

        public void toggle(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (this.isVisible)
            {
                hide();
            }
            else
            {
                show();
            }
        }

        public bool chatIsVisible()
        {
            return isVisible;
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
            chatHistoryTextbox.Text += "\n<You> " + inputTextBox.Text;
            inputTextBox.Text = "";
        }

        public void update()
        {
            //throw new NotImplementedException();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            
            //throw new NotImplementedException();
        }
    }
}
