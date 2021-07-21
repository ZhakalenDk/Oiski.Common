using System.Collections.Generic;
using System.Data.SqlClient;

namespace Oiski.Common.SQL
{
    /// <summary>
    /// Represents a de-serialized SQL-tablerow with all its fields
    /// </summary>
    public interface IMySQLObject
    {
        /// <summary>
        /// The list of fields for this object
        /// </summary>
        IReadOnlyDictionary<string, object> Fields { get; }
        /// <summary>
        /// The unique ID that represents this object in the database
        /// </summary>
        KeyValuePair<string, int> RowID { get; }

        object this[ string _columnName ] { get; }

        SqlParameter GetIDAsParameter ();

        SqlParameter[] BuildParameters ();
    }
}
