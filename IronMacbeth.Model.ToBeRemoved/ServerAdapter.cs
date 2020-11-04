using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;

namespace IronMacbeth.Model.ToBeRemoved
{
    public class ServerAdapter
    {
        public static IService Proxy;

        public ServerAdapter(IService proxy)
        {
            Proxy = proxy;
        }

        public void AddTextFile(string text, out string fileName)
        {
            byte[] bytes = Serialize(text);

            Proxy.AddFile(bytes, out fileName);
        }

        public void AddImageFile(byte[] image, out string fileName)
        {
            Proxy.AddFile(image, out fileName);
        }

        public void Insert<T>(T item)
        {
            var interfaceTypes = typeof(T).GetInterfaces();
            if (interfaceTypes.Contains(typeof(IDisplayable)))
            {
                IDisplayable displayable = item as IDisplayable;

                if (displayable.BitmapImage != null)
                {
                    string fileName;

                    Bitmap bitmap = BitmapImageToBitmap(displayable.BitmapImage);

                    byte[] image = Serialize(bitmap);

                    AddImageFile(image, out fileName);

                    displayable.ImageName = fileName;
                }
            }
            if (interfaceTypes.Contains(typeof(IDescribable)))
            {
                IDescribable describable = item as IDescribable;

                if (describable.Description != null)
                {
                    string fileName;

                    AddTextFile(describable.Description, out fileName);

                    describable.DescriptionName = fileName;
                }
            }

            byte[] bytes = Serialize(item);

            Proxy.Insert(bytes, item.GetType().AssemblyQualifiedName);
        }

        public List<T> GetAll<T>()
        {
            var bytes = Proxy.GetAll(typeof(T).AssemblyQualifiedName);
            List<object> objects;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                objects = (List<object>)binaryFormatter.Deserialize(stream);
            }

            List<T> results = objects.Cast<T>().ToList();

            var intefaceTypes = typeof(T).GetInterfaces();
            if (intefaceTypes.Contains(typeof(IDisplayable)))
            {
                PropertyInfo nameProperty = typeof(IDisplayable).GetProperty("ImageName");
                PropertyInfo imageProperty = typeof(IDisplayable).GetProperty("BitmapImage");

                foreach (T result in results)
                {
                    imageProperty.SetValue(
                        result,
                        GetImage((string)nameProperty.GetValue(result))
                    );
                }
            }

            if (intefaceTypes.Contains(typeof(IDescribable)))
            {
                PropertyInfo nameProperty = typeof(IDescribable).GetProperty("DescriptionName");
                PropertyInfo decriptionProperty = typeof(IDescribable).GetProperty("Description");

                foreach (T result in results)
                {
                    decriptionProperty.SetValue(
                        result,
                        GetStringFileContent((string)nameProperty.GetValue(result))
                    );
                }
            }

            return results;
        }

        public void Delete<T>(T item)
        {
            byte[] bytes = Serialize(item);

            Proxy.Delete(bytes);
        }

        public void DeleteLink<T>(T item)
        {
            byte[] bytes = Serialize(item);

            Proxy.DeleteLink(bytes);
        }

        public void Update<T>(T item)
        {
            var intefaceTypes = typeof(T).GetInterfaces();
            if (intefaceTypes.Contains(typeof(IDisplayable)))
            {
                IDisplayable displayable = item as IDisplayable;

                PropertyInfo nameProperty = typeof(IDisplayable).GetProperty("ImageName");
                PropertyInfo imageProperty = typeof(IDisplayable).GetProperty("BitmapImage");

                if (string.IsNullOrWhiteSpace(nameProperty.GetValue(item) as string) &&
                    imageProperty.GetValue(item) != null)
                {
                    string fileName;

                    Bitmap bitmap = BitmapImageToBitmap(displayable.BitmapImage);

                    byte[] image = Serialize(bitmap);

                    AddImageFile(image, out fileName);

                    displayable.ImageName = fileName;
                }
            }

            if (intefaceTypes.Contains(typeof(IDescribable)))
            {
                PropertyInfo nameProperty = typeof(IDescribable).GetProperty("DescriptionName");
                PropertyInfo decriptionProperty = typeof(IDescribable).GetProperty("Description");

                IDescribable describable = item as IDescribable;

                if (string.IsNullOrWhiteSpace(nameProperty.GetValue(item) as string) &&
                    decriptionProperty.GetValue(item) != null)
                {
                    string fileName;

                    AddTextFile(describable.Description, out fileName);

                    describable.DescriptionName = fileName;
                }
            }

            byte[] bytes = Serialize(item);

            Proxy.Update(bytes);
        }

        private static byte[] Serialize(object item)
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

        private string GetStringFileContent(string fileName)
        {
            byte[] bytes = Proxy.GetFile(fileName);

            return Deserialize(bytes) as string;
        }

        public static BitmapImage GetImage(string fileName)
        {
            byte[] bytes = Proxy.GetFile(fileName);

            Bitmap bitmap = Deserialize(bytes) as Bitmap;

            return BitmapToBitmapImage(bitmap);
        }

        public void InsertStoreSellable(StoreSellable storeSellable)
        {
            Type type = storeSellable.Sellable.GetType();
            Assembly assembly = type.Assembly;

            Type linkType = assembly.GetType("IronMacbeth.Model.ToBeRemoved.Store" + type.Name);

            ISellableLink sellableLink = Activator.CreateInstance(linkType) as ISellableLink;

            sellableLink.StoreId = storeSellable.StoreId;
            sellableLink.SellableId = storeSellable.SellableId;

            Insert(sellableLink);
        }

        public static object Deserialize(byte[] bytes)
        {
            object item;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                item = binaryFormatter.Deserialize(stream);
            }
            return item;
        }

        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }
            return bitmap;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bitmapImage;

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}