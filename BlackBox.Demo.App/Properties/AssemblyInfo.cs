using System.Reflection;
using System.Runtime.InteropServices;
using BlackBox.Recorder;

[assembly: AssemblyTitle("BlackBox.Demo.App")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("BlackBoxRecorder")]
[assembly: AssemblyProduct("BlackBox.Demo.App")]
[assembly: AssemblyCopyright("Copyright © Jonas Follesø, Marcus Almgren 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: Guid("ca24eb81-9bc6-4646-81e1-cceb08836b21")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: RecordingAttribute(
            AttributeTargetAssemblies = "BlackBox.Demo.App",
            AttributeTargetTypes = "BlackBox.Demo.App.AssemblyAttribute.BL.*")]

[assembly: DependencyAttribute(
            AttributeTargetAssemblies = "BlackBox.Demo.App",
            AttributeTargetTypes = "BlackBox.Demo.App.AssemblyAttribute.DAL.*")]

[assembly: DependencyAttribute(
            AttributeTargetAssemblies = "mscorlib",
            AttributeTargetTypes = "System.Random")]