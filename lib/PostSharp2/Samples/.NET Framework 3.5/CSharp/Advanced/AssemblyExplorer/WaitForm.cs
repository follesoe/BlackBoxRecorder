using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AssemblyExplorer
{
    public partial class WaitForm : Form
    {
        private Thread thread;
        private ParameterizedThreadStart action;
        private object parameter;

        public WaitForm()
        {
            InitializeComponent();
        }

        public WaitForm(ParameterizedThreadStart action, object parameter ) : this()
        {
            this.parameter = parameter;
            this.action = action;
            this.thread = new Thread(this.StartWorkerThread);
            this.thread.Start();
        }

        private void StartWorkerThread()
        {
            try
            {
                this.action(this.parameter);
            }
            finally
            {
                this.Invoke(new ThreadStart(this.Close));
            }

            
        }


        private void abortButton_Click(object sender, EventArgs e)
        {
            this.thread.Abort();
        }
    }
}