using System.Windows.Controls;

namespace GoIoFish.Services.Interfaces
{
    public interface IFrameProvider
    {

        void SetFrame(Frame frame);
        Frame GetFrame();

    }
}
