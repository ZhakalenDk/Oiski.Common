using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Oiski.Common.SQL;

namespace Oiski.Common.SQL
{
    /// <summary>
    /// Represents a SQL Connection to a database specified by the <see cref="this[string]"/>.
    /// </summary>
    public class SQLConnectionUtility
    {
        protected SQLConnectionUtility ()
        {
            var builder = new ConfigurationBuilder ().SetBasePath (Directory.GetCurrentDirectory ()).AddJsonFile ("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build ();
        }

        public List<IMySQLObject> ExecuteStoredProcedure ( string v, string cONNECTIONID, object p )
        {
            throw new NotImplementedException ();
        }

        /// <summary>s
        /// The instance of the connection
        /// </summary>s
        public static SQLConnectionUtility Connection
        {
            get
            {
                if ( connection == null )
                {
                    connection = new SQLConnectionUtility ();
                }

                return connection;
            }
        }
        private static SQLConnectionUtility connection;

        public IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// Retrieves the connection to a specefic <paramref name="_connectionStringName"/> database
        /// </summary>
        /// <param name="_connectionStringName">The name that represents the desired connection string</param>
        /// <returns></returns>
        public SqlConnection this[ string _connectionStringName ]
        {
            get
            {
                return new SqlConnection (Configuration.GetConnectionString (_connectionStringName));
            }
        }

        /// <summary>
        /// Connects to a database and executes a stored procedure.
        /// <br/>
        /// If any objects are returned form the database, the execution will open a reader and store all objects in a list of type <see cref="IMySQLObject"/> then return it.
        /// <br/>
        /// If no objects are returned from the database, the execution will return null.
        /// </summary>
        /// <param name="_procedure">The procedure to execute.</param>
        /// <param name="_connectionStringName">The ID that refers to the connection string that's collected from <see cref="ConfigurationManager.ConnectionStrings"/>.</param>
        /// <param name="_paramters">The paramters that should be passed to the command when executing.
        /// <br/>
        /// <strong>Note:</strong> Parameters must be passed in the same order as the <i>Stored Procedure </i> recieves them.
        ///     <list type="table">
        ///         <listheader><strong>Example:</strong></listheader>
        ///         <item>
        ///         The following <i>Stored Procedure</i> takes in parameters in order:
        ///             <list type="number">
        ///             <item>
        ///             <strong>@FirstName</strong>
        ///             </item>
        ///             <item>
        ///             <strong>@LastName</strong>
        ///             </item>
        ///             </list>
        ///         </item>
        ///         
        ///         <item>So the array of <see cref="SqlParameter"/>s should be ordered in the following way:
        ///         <br/>
        ///         <strong>_parameters[0]</strong> = new <see cref="SqlParameter"/> ("<strong>@FirstName</strong>", "<i>Hans</i>")
        ///         <br/>
        ///         <strong>_parameters[1]</strong> = new <see cref="SqlParameter"/> ("<strong>@LastName</strong>", "<i>Von Frankfurt</i>")
        ///         </item>
        ///     </list>
        ///     
        /// Else the program might throw and <see cref="InvalidCastException"/>.
        /// </param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public List<IMySQLObject> ExecuteStoredProcedure ( string _procedure, string _connectionStringName, SqlParameter[] _paramters = null )
        {
            List<IMySQLObject> objects = null;
            SqlConnection con = this[ _connectionStringName ];

            if ( con == null )
            {
                throw new NullReferenceException ($"Connectin with ID: [{_connectionStringName}] returned null. Did you forget to include the ConnectionString in the configuration file?");
            }

            con.Open ();

            //  Creates a command ready to recieve a stored procedure
            SqlCommand command = new SqlCommand (_procedure, con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            if ( _paramters != null )
            {
                for ( int i = 0; i < _paramters.Length; i++ )
                {
                    command.Parameters.Add (_paramters[ i ]);
                }

            }

            SqlDataReader reader = command.ExecuteReader ();

            SQLObject obj = null;

            //  Goes through any potential objects returned from the database
            while ( reader.Read () )
            {
                //  Create a list if there's not already one created
                if ( obj == null )
                {
                    objects = new List<IMySQLObject> ();
                }

                obj = new SQLObject ();
                Dictionary<string, object> fields = new Dictionary<string, object> ();

                //  Goes through each field of a given object from the database
                for ( int i = 0; i < reader.FieldCount; i++ )
                {
                    if ( i == 0 )
                    {
                        obj.RowID = new KeyValuePair<string, int> (reader.GetName (i), reader.GetInt32 (i));
                    }
                    else
                    {
                        fields.Add (reader.GetName (i), reader.GetValue (i));
                    }
                }

                obj.Fields = fields;
                objects.Add (obj);
            }

            reader.Close ();

            con.Close ();

            return objects;
        }
    }
}