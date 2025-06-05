using System;

namespace Arenda.ViewModels
{
    public class MessageViewModel
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsOwn { get; set; }
        public string BubbleColor => IsOwn ? "#DCF8C6" : "#F1F0F0";
    }
}