using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaVisualDemo
{
    public class Message
    {
        public string Content { get; set; }

        public Message(string content)
        {
            Content = content;
        }
    }
}
