using System;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Librarian.WinForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }


        private void buttonOk_Click( object sender, EventArgs e )
        {
            try
            {
                if ( !ClientSession.OpenSession( this.textBoxLogin.Text, this.textBoxPassword.Text ) )
                {
                    MessageBox.Show( this, "Invalid login or password.", "Error", MessageBoxButtons.OK,
                                     MessageBoxIcon.Error );
                    this.textBoxLogin.Focus();
                    return;
                }
            }
            catch ( RemotingException exc )
            {
                MessageBox.Show( this, exc.Message, "Cannot connect to the server.", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error );
                return;
            }

            try
            {
                ClientSession.Current.SetCurrentCashbox( this.textBoxCashboxId.Text );
            }
            catch ( ArgumentException )
            {
                MessageBox.Show( this, "Invalid cashbox identifier.", "Error", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error );
                this.textBoxCashboxId.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}