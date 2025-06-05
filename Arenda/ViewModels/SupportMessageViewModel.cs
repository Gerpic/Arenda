using System;
using System.Windows;

namespace Arenda.ViewModels
{
    public class SupportMessageViewModel
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool IsFromSupport { get; set; }
        public HorizontalAlignment Alignment { get; set; }
        public string BubbleColor { get; set; }
    }
}