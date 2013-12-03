using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamarinAndroidTest
{
    struct ChatMessage
    {
        public static ChatMessage CreateFromWire(String[] array) {
            return new ChatMessage(array[0], array[1]);
        }
        public static ChatMessage CreateFromWire(string p0, string p1)
        {
            return new ChatMessage(p0,p1);
        }

        public ChatMessage(string name, string message)
        {
            this.name = name;
            this.message = message;
        }

        public String[] ToWireFormat()
        {
            var array = new String[2];
            array[0] = name;
            array[1] = message;
            return array;
        }

        public string name;
        public string message;
    }
}
