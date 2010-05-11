using System;
using System.Data;
using System.Data.Common;

namespace ContactManager.Entities
{
    public static class Extensions
    {
        public static DbParameter AddParameter( this DbCommand command, string name, DbType dbType, object value )
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = dbType;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add( parameter );
            return parameter;
        }
    }
}