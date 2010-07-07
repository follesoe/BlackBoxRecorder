#region Released to Public Domain by Gael Fraiteur
/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/
#endregion

#region Using directives

using System.Reflection;
using System.Security.Permissions;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "PostSharp.Samples.Explorer" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "postsharp.org" )]
[assembly: AssemblyProduct( "PostSharp" )]
[assembly: AssemblyCopyright("Released to Public Domain by Gael Fraiteur")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture( "" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion( "1.0.*" )]
[assembly: PermissionSet(SecurityAction.RequestMinimum, Name = "FullTrust")]