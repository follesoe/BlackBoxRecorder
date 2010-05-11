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

using Microsoft.Win32;

namespace AssemblyExplorer
{
    internal static class UserConfiguration
    {
        private const string registryKey =
            @"SOFTWARE\postsharp.org\PostSharp Explorer\Loaded Assemblies";

        public static string[] GetLoadedAssemblies()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey( registryKey, false );

            if ( key != null )
            {
                return key.GetValueNames();
            }
            else
            {
                return new string[0];
            }
        }

        public static void AddAssembly( string path )
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey( registryKey, true );
            if ( key == null )
            {
                key = Registry.CurrentUser.CreateSubKey( registryKey );
            }
            key.SetValue( path, "", RegistryValueKind.String );
        }

        public static void RemoveAssembly( string path )
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey( registryKey, true );

            if ( key != null )
            {
                key.DeleteValue( path, true );
            }
        }
    }
}