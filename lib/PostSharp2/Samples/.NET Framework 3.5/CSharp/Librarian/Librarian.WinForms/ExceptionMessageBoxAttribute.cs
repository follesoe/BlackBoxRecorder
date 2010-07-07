using System;
using System.Reflection;
using System.Windows.Forms;
using Librarian.Framework;
using PostSharp.Aspects;

namespace Librarian.WinForms
{
    [Serializable]
    public sealed class ExceptionMessageBoxAttribute : OnExceptionAspect
    {
        public override Type GetExceptionType( MethodBase method )
        {
            return typeof(BusinessException);
        }

        public override void OnException( MethodExecutionArgs eventArgs )
        {
            MessageBox.Show( eventArgs.Instance as Form,
                             eventArgs.Exception.Message,
                             "Business Error",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error );
            eventArgs.FlowBehavior = FlowBehavior.Continue;
        }
    }
}