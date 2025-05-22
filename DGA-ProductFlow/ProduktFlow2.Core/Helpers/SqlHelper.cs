using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProduktFlow2.Core.Helpers
{
    public static class SqlHelper
    {
        /// <summary>
        /// Creates and opens a SQL connection with the given connection string.
        /// </summary>
        public static SqlConnection CreateConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Creates a SQL command for a stored procedure.
        /// </summary>
        public static SqlCommand CreateStoredProcedureCommand(string procedureName, SqlConnection connection)
        {
            return new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        /// <summary>
        /// Adds a parameter with a specific SQL type to the command, handling null values.
        /// </summary>
        public static void AddParameter(SqlCommand command, string name, object value)
        {
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }


        /// <summary>
        /// Adds a parameter using inferred SQL type and handles null values.
        /// </summary>
        public static void AddSimpleParameter(SqlCommand command, string name, object value)
        {
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Adds an output parameter to a command and returns the created parameter.
        /// </summary>
        public static SqlParameter AddOutputParameter(SqlCommand command, string name, SqlDbType type)
        {
            var param = new SqlParameter(name, type)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(param);
            return param;
        }
    }
}
