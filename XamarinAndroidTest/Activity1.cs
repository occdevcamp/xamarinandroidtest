using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.AspNet.SignalR.Client;

namespace XamarinAndroidTest
{
    [Activity(Label = "XamarinAndroidTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
            aLabel.Text = "Enter the text to chat here";

            var textBox = new EditText(this);

            var aButton = new Button(this);
            aButton.Text = "Say Hello";
            var label = new TextView(this);
            label.Text = "Chat Messages";

            var chatView = new TextView(this);

            layout.AddView(aLabel);
            layout.AddView(textBox);
            layout.AddView(aButton);
            layout.AddView(label);
            layout.AddView(chatView);
            SetContentView(layout);

            
            string HUB_URL = "http://129.67.34.227/ChatServer/signalr";
            string HUB_NAME = "chatHub";
            string SOMEONE_ELSE_IS_CHATTING_EVENT = "addNewMessageToPage";
            string IM_CHATTING_EVENT = "Send";

            HubConnection connection = new HubConnection(HUB_URL);
            IHubProxy hubProxy = connection.CreateHubProxy(HUB_NAME);
            hubProxy.On<string,string>(SOMEONE_ELSE_IS_CHATTING_EVENT, (p1,p2) => AppendChatMessage(chatView, ChatMessage.CreateFromWire(p1,p2)));
            connection.Start().Wait();

            aButton.Click += (sender, e) =>
            {
                hubProxy.Invoke<String[]>(IM_CHATTING_EVENT, GetChatMessage(textBox).ToWireFormat());
            };
        }

        ChatMessage GetChatMessage(TextView textBox)
        {
            return new ChatMessage("Android", textBox.Text);
        }

        void AppendChatMessage(TextView chatView, ChatMessage message)
        {
            chatView.Text += String.Format("{0}: {1}\n", message.name, message.message);
        }

    }
}

