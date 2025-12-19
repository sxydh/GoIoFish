using GoIoFish.Services.Interfaces;
using System.Windows.Controls;

namespace GoIoFish.Services.Implementations
{
    public class FrameProvider : IFrameProvider
    {
        private Frame _frame;
        public void SetFrame(Frame frame) => _frame = frame;
        public Frame GetFrame() => _frame;
    }
}
