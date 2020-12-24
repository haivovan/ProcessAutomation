using Gecko;
using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public interface IRegisterAccount
    {
        void startRegister(GeckoWebBrowser webBrowser, RegisterAccount form);
    }
}
