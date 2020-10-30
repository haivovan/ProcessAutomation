using ProcessAutomation.Main.PayIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessAutomation.Main
{
    public partial class RegisterAccount : Form
    {
        IRegisterAccount _account;
        public RegisterAccount(IRegisterAccount account)
        {
            _account = account;
            InitializeComponent();
        }

        public void StartRegister(RegisterAccount form) {
            _account.startRegister(registerAccountBrowser, form);
        }
    }
}
