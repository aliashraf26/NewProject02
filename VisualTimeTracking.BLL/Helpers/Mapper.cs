
using Microsoft.Data.SqlClient;

namespace VisualTimeTracking.BL.Helpers
{
    public static class Mapper
    {
        public static void MapSql<T>(ref T obj, SqlDataReader reader)
        {
            var properties = obj.GetType().GetProperties();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string fieldName = reader.GetName(i);

                    if (properties.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        var prop = properties.FirstOrDefault(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase));
                        var value = reader.GetValue(i);
                        var val = value == null ? "" : Convert.ChangeType(value.ToString(), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                        prop.SetValue(obj, val);
                    }
                }
            }
        }
        public static List<T> ExecuteQueryWithMapping<T>(SqlConnection connection, string query, params SqlParameter[] parameters) where T : new()
        {
            List<T> resultList = new List<T>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T obj = new T();
                        MapSql(ref obj, reader); // Using the provided MapSql<T> method
                        resultList.Add(obj);
                    }
                }
            }

            return resultList;
        }

    }
}
