using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using IronMacbeth.Model.ToBeRemoved;

namespace IronMacbeth.BFF
{
    public class Service : IService
    {
        private static readonly DataAdapter DataAdapter;
        private static readonly Dictionary<string,IServiceCallback> Callbacks;

        static Service()
        {
            //Represents a connection to a SQL Server database
            var connection = new SqlConnection("Data Source=localhost;Initial Catalog=IronMacbeth.BFF.Database;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
          
            DataAdapter = new DataAdapter(connection);

            Callbacks = new Dictionary<string, IServiceCallback>();
        }

        public User LogIn(string login, string password)
        {
            var bytes = GetAll(typeof(User).AssemblyQualifiedName);
            List<object> objects;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                objects = (List<object>)binaryFormatter.Deserialize(stream);
            }

            List<User> results = objects.Cast<User>().ToList();

            foreach (User user in results)
            {
                if (user.Login == login && user.Password == password)
                {
                    IServiceCallback registeredUser = OperationContext.Current.GetCallbackChannel<IServiceCallback>();

                    if (!Callbacks.Keys.Contains(login))
                    {
                        Callbacks.Add(login,registeredUser);
                    }

                    return user;
                }
            }

            return null;
        }

        public void LogOut(User user)
        {
            if (Callbacks.ContainsKey(user.Login))
            {
                Callbacks.Remove(user.Login);
            }
        }

        public byte[] GetAll(string typeName)
        {
            Type type = Type.GetType(typeName);

            var bytes = DataAdapter.GetAll(type);
            return bytes;
        }

        public void Insert(byte[] bytes, string typeName)
        {
            Type type = Type.GetType(typeName);

            if (type == typeof(Purchase))
            {
                Purchase purchase = DataAdapter.Deserialize(bytes) as Purchase;

                string imageName;
                string sellableName;
                DataAdapter.GetImageNameFromPurchase(purchase, out imageName,out sellableName);

                byte[] fileBytes = GetFile(imageName);

                Notification notification = new Notification
                {
                    Name = $"{purchase.FirstName} {purchase.SecondName}",
                    SellableName = sellableName,
                    BitmapBytes = fileBytes
                };

                IServiceCallback registeredUser = OperationContext.Current.GetCallbackChannel<IServiceCallback>();

                string login = DataAdapter.GetStoreOwnerName(purchase);

                if (Callbacks.ContainsKey(login) && Callbacks[login]!=registeredUser)
                {
                    try
                    {
                        Callbacks[login].NotifyNewPurchase(notification);
                    }
                    catch
                    {
                        Callbacks.Remove(login);
                    }
                }
            }

            DataAdapter.Insert(bytes, type);
        }

        public void Delete(byte[] item)
        {
            DataAdapter.Delete(item);
        }

        public void DeleteLink(byte[] item)
        {
            DataAdapter.DeleteLink(item);
        }

        public void Update(byte[] item)
        {
            DataAdapter.Update(item);
        }

        public void AddFile(byte[] file, out string fileName)
        {
            fileName = GetFileId();

            FileInfo fileInfo = new FileInfo($"Files\\{fileName}");

            using (var stream = fileInfo.Open(FileMode.CreateNew, FileAccess.Write))
            {
                stream.Write(file, 0, file.Length);
                stream.Close();
            }
        }

        private static string GetFileId()
        {
            string result;

            Directory.CreateDirectory("Files");

            FileInfo fileInfo = new FileInfo("Files\\idStorage");
            if (fileInfo.Exists)
            {
                int id;
                using (TextReader reader = fileInfo.OpenText())
                {
                    id = int.Parse(reader.ReadLine());
                }
                id++;
                result = id.ToString();
                using (TextWriter writer = fileInfo.CreateText())
                {
                    writer.Write(result);
                }
            }
            else
            {
                result = "1";
                using (TextWriter writer = fileInfo.CreateText())
                {
                    writer.Write(result);
                }
            }
            return result;
        }

        public byte[] GetFile(string fileName)
        {
            return File.ReadAllBytes($"Files\\{fileName}");
        }

        public bool Ping()
        {
            return true;
        }
    }
}