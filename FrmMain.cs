using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoLoginForm1
{
    public partial class FrmMain : Form
    {
        private string _username;
        public FrmMain(string username)
        {
            InitializeComponent();
            _username = username;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            label1.Text=$"Chao mung thay den voi nhom chung e,{_username}";
        }
    }
}
