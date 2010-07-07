using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace ContactManager.Aspects
{
    [Serializable]
    [ProvideAspectRole("UI")]
    public sealed class StatusTextAttribute : OnMethodBoundaryAspect
    {
        private readonly string text;

        public StatusTextAttribute(string text)
        {
            this.text = text;
        }


        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = MainWindow.Instance.SetStatusText(text);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            MainWindow.Instance.SetStatusText((string) args.MethodExecutionTag);
        }
    }
}
