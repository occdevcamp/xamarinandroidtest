using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.AspNet.SignalR.Client;
using OxfordCC.DevCamp2013.ChatCore;
using Android.Views.InputMethods;

namespace OxfordCC.DevCamp2013.AndroidChatUI
{
    [Activity(Label = "XamarinAndroidTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// URL of the Hub server.
        /// </summary>
        private const string HUB_URL = "http://129.67.34.227/ChatServer/signalr";

        /// <summary>
        /// The name of the Chat Hub
        /// </summary>
        /// <remarks>
        /// This is the name specified by the HubName attribute, 
        /// defaulting to the camelCase version of the hub class name
        /// </remarks>
        private const string HUB_NAME = "chatHub"; 

        /// <summary>
        /// The name of the method the hub uses to broadcast chat messages
        /// </summary>
        /// <remarks>
        /// This is the name of the method called on the Clients object
        /// </remarks>
        private const string SOMEONE_ELSE_IS_CHATTING_METHOD = "addNewMessageToPage";
        /// <summary>
        /// The name of the method the hub listens for to receive new chat messages
        /// </summary>
        /// <remarks>
        /// This is the name specified by the HubMethodName attribute, 
        /// defaulting to the name of the C# method
        /// </remarks>
        private const string I_AM_CHATTING_METHOD = "Send";


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            #region Configure Activity Controls
            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var chatMessagesLabel = new TextView(this);
            chatMessagesLabel.Text = "Chat Messages";

            /// <summary>
            /// The text view in which the chat messages are displayed
            /// </summary>
            var chatTextView = new TextView(this);


            var chatInputLabel = new TextView(this);
            chatInputLabel.Text = "Enter the text to chat here";

            /// <summary>
            /// The text view which is the chat message to be broadcast
            /// </summary>
            var chatInput = new EditText(this);

            /// <summary>
            /// The button that sends the chat message
            /// </summary>
            var chatButton = new Button(this);
            chatButton.Text = "Chat";

            layout.AddView(chatInputLabel);
            layout.AddView(chatInput);
            layout.AddView(chatButton);
            layout.AddView(chatMessagesLabel);
            layout.AddView(chatTextView);
            SetContentView(layout);
            #endregion

            #region Configure the SignalR bindings
            //Create the Hub Proxy connection
            HubConnection connection = new HubConnection(HUB_URL);
            IHubProxy hubProxy = connection.CreateHubProxy(HUB_NAME);
            
            //Receive chat messages
            hubProxy.On<string, string>(
                SOMEONE_ELSE_IS_CHATTING_METHOD,
                (p1, p2) =>
                {
                    //Need to ensure the changes are in the UI thread, 
                    //since SignalR can fire the events from a different thread
                    RunOnUiThread(() => 
                        chatTextView.Append(String.Format("{0}: {1}\n", p1, p2))
                    );
                });

            //Send out chat messages
            chatButton.Click += (sender, e) =>
            {
                hubProxy.Invoke<String[]>(
                    I_AM_CHATTING_METHOD, 
                    new String[] { "Android", chatInput.Text });
                chatInput.Text = "";
            };

            //Start the link with the hub
            connection.Start().Wait();
            #endregion
        }
    }
}

