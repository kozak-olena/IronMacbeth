using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.BFF
{
    internal class DataAdapter
    {
        public readonly SqlConnection Connection;

        public DataAdapter(SqlConnection connection)
        {
            Connection = connection;
        }

        public void Insert(byte[] bytes, Type type)
        {
            Object item = Deserialize(bytes);

            String tableName = type.Name.ToLower();

            String queryTable = $"INSERT INTO dbo.[{tableName}] (";

            String queryProperties = string.Empty;

            var properties = type.GetProperties().Where(property => Attribute.IsDefined(property, typeof(DatabaseAttribute)) && property.Name != "Id").ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                queryProperties += properties[i].Name.ToLower();
                if (i != properties.Count - 1)
                {
                    queryProperties += ", ";
                }
            }

            String queryValues = ") VALUES (";

            for (int i = 0; i < properties.Count; i++)
            {
                queryValues += "'" + properties[i].GetValue(item, null) + "'";
                if (i != properties.Count - 1)
                {
                    queryValues += ", ";
                }
            }

            SqlCommand command = new SqlCommand(
                queryTable + queryProperties + queryValues + ");",
                Connection
            );

            Connection.Open();

            command.ExecuteNonQuery();

            Connection.Close();
        }

        public byte[] GetAll(Type type)
        {
            Connection.Open();

            List<object> objects = new List<object>();

            String tableName = type.Name.ToLower();

            SqlCommand command = new SqlCommand($"SELECT * FROM [dbo].[{tableName}]", Connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Object obj = Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties().Where(property => Attribute.IsDefined(property, typeof(DatabaseAttribute))).ToList())
                    {
                        item.SetValue(obj, reader[item.Name.ToLower()]);
                    }
                    objects.Add(obj);
                }
            }

            byte[] bytes = Serialize(objects);

            Connection.Close();

            return bytes;
        }
        public List<T> GetAll<T>()
        {
            Connection.Open();

            List<T> objects = new List<T>();

            Type type = typeof(T);

            String tableName = type.Name.ToLower();

            SqlCommand command = new SqlCommand($"SELECT * FROM [dbo].[{tableName}]", Connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Object obj = Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties().Where(property => Attribute.IsDefined(property, typeof(DatabaseAttribute))).ToList())
                    {
                        item.SetValue(obj, reader[item.Name.ToLower()]);
                    }
                    objects.Add((T)obj);
                }
            }

            Connection.Close();

            return objects;
        }

        public void Update(byte[] bytes)
        {
            Object item = Deserialize(bytes);

            Type type = item.GetType();

            String tableName = type.Name.ToLower();

            String queryTable = $"UPDATE [dbo].[{tableName}] SET ";

            String queryValues = string.Empty;

            var properties = type.GetProperties().Where(property => Attribute.IsDefined(property, typeof(DatabaseAttribute)) && property.Name != "Id").ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                queryValues +=
                    properties[i].Name.ToLower() + "='" +
                    properties[i].GetValue(item) + "'";

                if (i != properties.Count - 1)
                {
                    queryValues += ", ";
                }
            }

            string queryWhere = $" WHERE id='{type.GetProperty("Id").GetValue(item)}';";

            SqlCommand command = new SqlCommand(
                queryTable + queryValues + queryWhere,
                Connection
            );


            Connection.Open();

            command.ExecuteNonQuery();

            Connection.Close();
        }

        public void Delete(byte[] bytes)
        {
            Object item = Deserialize(bytes);

            string tableName = item.GetType().Name;
            string value = item.GetType().GetProperty("Id").GetValue(item).ToString();

            SqlCommand command = new SqlCommand(
                $"DELETE FROM [dbo].[{tableName}] WHERE id= '{value}';",
                Connection
            );

            Connection.Open();

            command.ExecuteNonQuery();

            Connection.Close();
        }

        public void DeleteLink(byte[] bytes)
        {
            object item = Deserialize(bytes);
            Type type = item.GetType();

            string tableName = type.Name.ToLower();

            string queryTable = $"DELETE FROM [dbo].[{tableName}] ";

            string queryWhere = "WHERE ";

            var properties = type.GetProperties().Where(property => Attribute.IsDefined(property, typeof(DatabaseAttribute))).ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                queryWhere += properties[i].Name.ToLower() + "='" + properties[i].GetValue(item) + "'";

                if (i != properties.Count - 1)
                {
                    queryWhere += " AND ";
                }
            }


            SqlCommand command = new SqlCommand(
                queryTable + queryWhere + ";",
                Connection
            );

            Connection.Open();

            command.ExecuteNonQuery();

            Connection.Close();
        }

        public void GetImageNameFromPurchase(Purchase purchase, out string imagename, out string model)
        {
            string[] splittedString = purchase.LinkName.Split(new[] { "Store" }, StringSplitOptions.None);
            string sellableName = splittedString.Last().ToLower();

            SqlCommand command = new SqlCommand(
                $"select imagename, model From {sellableName} where id=(SELECT distinct {sellableName}id FROM store{sellableName} where id={purchase.LinkId})",
                Connection
            );

            Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                imagename = (string) reader["imagename"];
                model = (string) reader["model"];
            }

            Connection.Close();
        }

        public string GetStoreOwnerName(Purchase purchase)
        {
            string[] splittedString = purchase.LinkName.Split(new[] { "Store" }, StringSplitOptions.None);
            string sellableName = splittedString.Last().ToLower();

            SqlCommand command = new SqlCommand(
                $"select ownerid from store where id = (select storeid from store{sellableName} where id = {purchase.LinkId})",
                Connection
            );

            Connection.Open();

            string name;

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                name = (string)reader["ownerid"];
            }

            Connection.Close();

            return name;
        }

        public int NextId(string tableName)
        {
            SqlCommand command = new SqlCommand(
                $"SELECT AUTO_INCREMENT FROM information_schema.tables WHERE table_name = '{tableName.ToLower()}' AND table_schema = 'database';",
                Connection
            );

            int id = -1;

            Connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = int.Parse(reader["AUTO_INCREMENT"].ToString());
                }
            }

            Connection.Close();

            return id;
        }

        public static object Deserialize(byte[] bytes)
        {
            Object item;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                item = binaryFormatter.Deserialize(stream);
            }
            return item;
        }

        public static byte[] Serialize(object item)
        {
            byte[] bytes;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, item);
                bytes = stream.ToArray();
            }

            return bytes;
        }
    }
}