using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Oiski.Common.SQL
{
    /// <summary>
    /// A basic class used for internal construction of <see cref="IMySQLObject"/>s
    /// </summary>
    public class SQLObject : IMySQLObject
    {

        /// <summary>
        /// The list of row fields for this instance
        /// </summary>
        public IReadOnlyDictionary<string, object> Fields { get; set; }
        
        /// <summary>
        /// The unique ID that represents this object in the database
        /// </summary>
        public KeyValuePair<string, int> RowID { get; set; }

        public object this[ string _columnName ]
        {
            get
            {
                return Fields[ _columnName ];
            }
        }

        public override string ToString ()
        {
            string fieldsString = string.Empty;

            List<KeyValuePair<string, object>> fields = Fields.ToList ();

            for ( int i = 0; i < fields.Count; i++ )
            {
                fieldsString += $"[{fields[i].Key}][{fields[i].Value}]";

                if ( i != fields.Count - 1 )
                {
                    fieldsString += ",";
                }
            }

            return $"ID: {RowID} - {fieldsString}";
        }

        public SqlParameter[] BuildParameters ()
        {
            SqlParameter[] parameters = new SqlParameter[ Fields.Count ];

            int index = 0;
            foreach ( KeyValuePair<string, object> field in Fields )
            {
                parameters[ index ] = new SqlParameter ($"@{field.Key}", field.Value);
                    index++;
            }

            return parameters;
        }

        public SqlParameter GetIDAsParameter ()
        {
            return new SqlParameter ($"@{RowID.Key}", RowID.Value);
        }
    }
}
