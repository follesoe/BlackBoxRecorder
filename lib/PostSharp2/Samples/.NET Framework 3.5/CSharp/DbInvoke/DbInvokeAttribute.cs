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

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace DbInvoke
{
    /// <summary>
    /// Custom attribute that allows to 'import' stored procedure by declaring <b>extern</b>
    /// methods, just like P-Invoke imports unmanaged APIes.
    /// </summary>
    /// <remarks>
    /// When applied on an external method, this custom attribute implements this method
    /// so that it calls a database stored procedure named after this method and having
    /// the same argument. Output parameters are supported.
    /// </remarks>
    [Serializable]
    public sealed class DbInvokeAttribute : MethodInterceptionAspect
    {
        /// <summary>
        /// Key of the connection string in the configuration file. Note that this
        /// field is serialized.
        /// </summary>
        private readonly string connectionStringKey;

        // ---- Transient fields, used at runtime at least -------------------------

        /// <summary>
        /// Connection factory. This is initialized at runtime from configuration settings.
        /// </summary>
        [NonSerialized] private DbProviderFactory dbProviderFactory;

        /// <summary>
        /// Database connection string. This is initialized at runtime from configuration settings.
        /// </summary>
        [NonSerialized] private string connectionString;

        /// <summary>
        /// Initializes a new instance of <see cref="DbInvokeAttribute"/>.
        /// </summary>
        /// <param name="connectionStringKey">Key of the connection string in the application
        /// configuration file.</param>
        public DbInvokeAttribute( string connectionStringKey )
        {
            this.connectionStringKey = connectionStringKey;
        }

        /// <summary>
        /// Method called at compile-time by the weaver just before the instance is serialized.
        /// </summary>
        /// <param name="method">Method on which this instance is applied.</param>
        /// <remarks>
        /// <para>Derived classes should implement this method if they want to compute some information
        /// at compile time. This information to be stored in member variables. It shall be
        /// serialized at compile time and deserialized at runtime.
        /// </para>
        /// <para>
        /// You cannot store and serialize the <paramref name="method"/> parameter because it is basically
        /// a runtime object. You shall receive the <see cref="MethodBase"/> at runtime by the
        /// <see cref="RuntimeInitialize"/> function.
        /// </para>
        /// </remarks>
        public override bool CompileTimeValidate( MethodBase method )
        {
            bool hasError = false;

            // Cannot be a constructor.
            MethodInfo methodInfo = method as MethodInfo;
            if ( methodInfo == null )
            {
                DbInvokeMessageSource.Instance.Write( SeverityType.Error, "DBI0001",
                                                      new object[] {method.DeclaringType.FullName} );
                return false;
            }

            // Should have void return type.
            if ( methodInfo.ReturnType != typeof(void) )
            {
                DbInvokeMessageSource.Instance.Write( SeverityType.Error, "DBI0002",
                                                      new object[] {method.ToString()} );
                hasError = true;
            }

            // All parameters should be mappable.
            foreach ( ParameterInfo parameter in methodInfo.GetParameters() )
            {
                Type parameterType = parameter.ParameterType;
                if ( parameterType.IsByRef )
                    parameterType = parameterType.GetElementType();

                if ( DbTypeMapping.GetPreferredMapping( parameterType ) == null )
                {
                    DbInvokeMessageSource.Instance.Write( SeverityType.Error, "DBI0003",
                                                          new object[]
                                                              {
                                                                  method.ToString(), parameter.ParameterType.FullName,
                                                                  parameter.Name
                                                              } );
                    hasError = true;
                }
            }

            return !hasError;
        }

        /// <summary>
        /// Method called at runtime just after the instance is deserialized.
        /// </summary>
        /// <param name="method">Method on which this instance is applied.</param>
        [DebuggerStepThrough]
        public override void RuntimeInitialize( MethodBase method )
        {
            ConnectionStringSettings connectionStringSettings =
                ConfigurationManager.ConnectionStrings[this.connectionStringKey];

            if ( connectionStringSettings == null )
            {
                throw new ConfigurationErrorsException("Invalid connection string name!");
            }

            this.dbProviderFactory = DbProviderFactories.GetFactory( connectionStringSettings.ProviderName );
            this.connectionString = connectionStringSettings.ConnectionString;
        }


        /// <summary>
        /// Method called instead of the body of the modified method. We put our own implementation
        /// here: calling the stored procedure that has the same name and signature as the
        /// called method.
        /// </summary>
        /// <param name="aspectArgs">Event arguments specifying which method is being
        /// executed, on which object instance and with which parameters.
        /// <remarks>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            // Get a connection.
            DbConnection connection = dbProviderFactory.CreateConnection();
            connection.ConnectionString = this.connectionString;

            // Get a command and set it up.
            DbCommand command = connection.CreateCommand();
            command.CommandText = args.Method.Name;
            command.CommandType = CommandType.StoredProcedure;

            // Add parameters.
            ParameterInfo[] methodParameters = args.Method.GetParameters();
            for ( int i = 0 ; i < methodParameters.Length ; i++ )
            {
                ParameterInfo methodParameter = methodParameters[i];

                // If the parameter is ByRef, get the element type.
                Type parameterType = methodParameter.ParameterType;
                if ( parameterType.IsByRef )
                    parameterType = parameterType.GetElementType();

                // Create and set up the parameter.
                DbParameter commandParameter = dbProviderFactory.CreateParameter();
                commandParameter.ParameterName = methodParameter.Name;
                if ( methodParameter.ParameterType.IsByRef )
                {
                    if ( methodParameter.IsIn && methodParameter.IsOut )
                    {
                        commandParameter.Direction = ParameterDirection.InputOutput;
                    }
                    else
                    {
                        commandParameter.Direction = ParameterDirection.Output;
                    }
                }
                else
                {
                    commandParameter.Direction = ParameterDirection.Input;
                }

                DbCompactType dbType = DbTypeMapping.GetPreferredMapping( parameterType ).DbCompactType;
                commandParameter.DbType = dbType.DbType;
                commandParameter.Size = dbType.Size == DbTypeMapping.FreeSize ? 1000 : dbType.Size;

                // If the parameter is input, set its value.
                if ( methodParameter.IsIn || methodParameter.Attributes == ParameterAttributes.None )
                {
                    commandParameter.Value = args.Arguments[i];
                }

                // Finally add the parameter to the command.
                command.Parameters.Add( commandParameter );
            }

            connection.Open();

            try
            {
                // Execute the command.
                command.ExecuteNonQuery();

                // Write back the output parameters.
                for ( int i = 0 ; i < methodParameters.Length ; i++ )
                {
                    ParameterInfo methodParameter = methodParameters[i];
                    if ( methodParameter.ParameterType.IsByRef )
                    {
                        args.Arguments[i] =
                            Convert.ChangeType( command.Parameters[i].Value,
                                                methodParameter.ParameterType.GetElementType() );
                    }
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}