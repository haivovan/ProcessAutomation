using System.Windows.Forms;

namespace ProcessAutomation.Main.PayIn
{
    public interface IRegisterAccount
    {
        void startRegister(WebBrowser webBrowser, RegisterAccount form);
    }
}
