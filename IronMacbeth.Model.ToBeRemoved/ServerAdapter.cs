using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

        #region Memory

        public void CreateMemory(Memory memory)
        {
            CreateIDisplayable(memory);
            CreateIDescribable(memory);

            Proxy.CreateMemory(memory);
        }

        public List<Memory> GetAllMemories()
        {
            var memories = Proxy.GetAllMemories();

            foreach (var memory in memories)
            {
                memory.BitmapImage = GetImage(memory.ImageName);
                memory.Description = GetStringFileContent(memory.DescriptionName);
            }

            return memories;
        }

        public void UpdateMemory(Memory memory)
        {
            UpdateIDisplayable(memory);
            UpdateIDescribable(memory);

            Proxy.UpdateMemory(memory);
        }

        public void DeleteMemory(int id)
        {
            Proxy.DeleteMemory(id);
        }

        #endregion

        #region Motherboard

        public void CreateMotherboard(Motherboard motherboard)
        {
            CreateIDisplayable(motherboard);
            CreateIDescribable(motherboard);

            Proxy.CreateMotherboard(motherboard);
        }

        public List<Motherboard> GetAllMotherboards()
        {
            var motherboards = Proxy.GetAllMotherboards();

            foreach (var motherboard in motherboards)
            {
                motherboard.BitmapImage = GetImage(motherboard.ImageName);
                motherboard.Description = GetStringFileContent(motherboard.DescriptionName);
            }

            return motherboards;
        }

        public void UpdateMotherboard(Motherboard motherboard)
        {
            UpdateIDisplayable(motherboard);
            UpdateIDescribable(motherboard);

            Proxy.UpdateMotherboard(motherboard);
        }

        public void DeleteMotherboard(int id)
        {
            Proxy.DeleteMotherboard(id);
        }

        #endregion

        #region Processor

        public void CreateProcessor(Processor processor)
        {
            CreateIDisplayable(processor);
            CreateIDescribable(processor);

            Proxy.CreateProcessor(processor);
        }

        public List<Processor> GetAllProcessors()
        {
            var processors = Proxy.GetAllProcessors();

            foreach (var processor in processors)
            {
                processor.BitmapImage = GetImage(processor.ImageName);
                processor.Description = GetStringFileContent(processor.DescriptionName);
            }

            return processors;
        }

        public void UpdateProcessor(Processor processor)
        {
            UpdateIDisplayable(processor);
            UpdateIDescribable(processor);

            Proxy.UpdateProcessor(processor);
        }

        public void DeleteProcessor(int id)
        {
            Proxy.DeleteProcessor(id);
        }

        #endregion

        #region Purchase

        public void CreatePurchase(Purchase purchase)
        {
            Proxy.CreatePurchase(purchase);
        }

        public List<Purchase> GetAllPurchases()
        {
            var purchases = Proxy.GetAllPurchases();

            return purchases;
        }

        public void UpdatePurchase(Purchase purchase)
        {
            Proxy.UpdatePurchase(purchase);
        }

        public void DeletePurchase(int id)
        {
            Proxy.DeletePurchase(id);
        }

        public Store GetStoreFromPurchase(Purchase purchase)
        {
            ISellableLink sellableLink;

            if (purchase.LinkName == typeof(StoreMemory).FullName)
            {
                sellableLink = GetAllStoreMemories().Find(x => x.Id == purchase.LinkId);
            }
            else if (purchase.LinkName == typeof(StoreMotherboard).FullName)
            {
                sellableLink = GetAllStoreMotherboards().Find(x => x.Id == purchase.LinkId);
            }
            else if (purchase.LinkName == typeof(StoreProcessor).FullName)
            {
                sellableLink = GetAllStoreProcessors().Find(x => x.Id == purchase.LinkId);
            }
            else if (purchase.LinkName == typeof(StoreVideocard).FullName)
            {
                sellableLink = GetAllStoreVideoCards().Find(x => x.Id == purchase.LinkId);
            }
            else
            {
                throw new NotSupportedException($"ISellableLink of type '{purchase.LinkName}' is not supported.");
            }

            var result = GetAllStores().Find(item => item.Id == sellableLink.StoreId);

            return result;
        }

        #endregion

        #region Store

        public void CreateStore(Store store)
        {
            CreateIDisplayable(store);

            Proxy.CreateStore(store);
        }

        public List<Store> GetAllStores()
        {
            var stores = Proxy.GetAllStores();

            foreach (var store in stores)
            {
                store.BitmapImage = GetImage(store.ImageName);
            }

            return stores;
        }

        public void UpdateStore(Store store)
        {
            UpdateIDisplayable(store);

            Proxy.UpdateStore(store);
        }

        public void DeleteStore(int id)
        {
            Proxy.DeleteStore(id);
        }

        public List<ISellableLink> GetStoreSellableLinks(int storeId)
        {
            List<ISellableLink> sellableLinks = new List<ISellableLink>();

            sellableLinks.AddRange(GetAllStoreProcessors().Where(item => item.StoreId == storeId));
            sellableLinks.AddRange(GetAllStoreVideoCards().Where(item => item.StoreId == storeId));
            sellableLinks.AddRange(GetAllStoreMotherboards().Where(item => item.StoreId == storeId));
            sellableLinks.AddRange(GetAllStoreMemories().Where(item => item.StoreId == storeId));

            return sellableLinks;
        }

        public List<ISellable> GetStoreSellables(int storeId)
        {
            List<ISellable> sellables = new List<ISellable>();

            sellables.AddRange(GetAllStoreProcessors().
                    Where(item => item.StoreId == storeId).
                    Select(item => GetProcessorFromStoreProcessor(item)).ToList());

            sellables.AddRange(GetAllStoreVideoCards().
                    Where(item => item.StoreId == storeId).
                    Select(item => GetVideoCardFromStoreVideoCard(item)).ToList());

            sellables.AddRange(GetAllStoreMotherboards().
                    Where(item => item.MotherboardId == storeId).
                    Select(item => GetMotherboardFromStoreMotherboard(item)).ToList());

            sellables.AddRange(GetAllStoreMemories().
                    Where(item => item.MemoryId == storeId).
                    Select(item => GetMemoryFromStoreMemory(item)).ToList());

            return sellables;
        }

        #endregion

        #region StoreMemory

        public void CreateStoreMemory(StoreMemory storeMemory)
        {
            Proxy.CreateStoreMemory(storeMemory);
        }

        public List<StoreMemory> GetAllStoreMemories()
        {
            var result = Proxy.GetAllStoreMemories();

            return result;
        }

        public void UpdateStoreMemory(StoreMemory storeMemory)
        {
            Proxy.UpdateStoreMemory(storeMemory);
        }

        public void DeleteStoreMemory(int id)
        {
            Proxy.DeleteStoreMemory(id);
        }

        public Memory GetMemoryFromStoreMemory(StoreMemory storeMemory)
        {
            return GetAllMemories().Find(item => item.Id == storeMemory.MemoryId);
        }

        #endregion

        #region StoreMotherboard

        public void CreateStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            Proxy.CreateStoreMotherboard(storeMotherboard);
        }

        public List<StoreMotherboard> GetAllStoreMotherboards()
        {
            var result = Proxy.GetAllStoreMotherboards();

            return result;
        }

        public void UpdateStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            Proxy.UpdateStoreMotherboard(storeMotherboard);
        }

        public void DeleteStoreMotherboard(int id)
        {
            Proxy.DeleteStoreMotherboard(id);
        }

        public Motherboard GetMotherboardFromStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            return GetAllMotherboards().Find(item => item.Id == storeMotherboard.MotherboardId);
        }

        #endregion

        #region StoreProcessor

        public void CreateStoreProcessor(StoreProcessor storeProcessor)
        {
            Proxy.CreateStoreProcessor(storeProcessor);
        }

        public List<StoreProcessor> GetAllStoreProcessors()
        {
            var result = Proxy.GetAllStoreProcessors();

            return result;
        }

        public void UpdateStoreProcessor(StoreProcessor storeProcessor)
        {
            Proxy.UpdateStoreProcessor(storeProcessor);
        }

        public void DeleteStoreProcessor(int id)
        {
            Proxy.DeleteStoreProcessor(id);
        }

        public Processor GetProcessorFromStoreProcessor(StoreProcessor storeProcessor)
        {
            return GetAllProcessors().Find(item => item.Id == storeProcessor.ProcessorId);
        }

        #endregion

        #region StoreVideocard

        public void CreateStoreVideocard(StoreVideocard storeVideocard)
        {
            Proxy.CreateStoreVideocard(storeVideocard);
        }

        public List<StoreVideocard> GetAllStoreVideoCards()
        {
            var result = Proxy.GetAllStoreVideoCards();

            return result;
        }

        public void UpdateStoreVideocard(StoreVideocard storeVideocard)
        {
            Proxy.UpdateStoreVideocard(storeVideocard);
        }

        public void DeleteStoreVideocard(int id)
        {
            Proxy.DeleteStoreVideocard(id);
        }

        public Videocard GetVideoCardFromStoreVideoCard(StoreVideocard storeVideocard)
        {
            return GetAllVideoCards().Find(item => item.Id == storeVideocard.VideocardId);
        }

        #endregion

        #region User

        public void Register(User user)
        {
            Proxy.Register(user);
        }

        public User LogIn(string login, string password)
        {
            var result = Proxy.LogIn(login, password);

            return result;
        }

        public List<Store> GetUserStores(User user)
        {
            return GetAllStores().Where(item => item.OwnerId == user.Login).ToList();
        }

        #endregion

        #region Videocard

        public void CreateVideoCard(Videocard videocard)
        {
            CreateIDisplayable(videocard);
            CreateIDescribable(videocard);

            Proxy.CreateVideoCard(videocard);
        }

        public List<Videocard> GetAllVideoCards()
        {
            var videoCards = Proxy.GetAllVideoCards();

            foreach (var videoCart in videoCards)
            {
                videoCart.BitmapImage = GetImage(videoCart.ImageName);
                videoCart.Description = GetStringFileContent(videoCart.DescriptionName);
            }

            return videoCards;
        }

        public void UpdateVideoCard(Videocard videocard)
        {
            UpdateIDisplayable(videocard);
            UpdateIDescribable(videocard);

            Proxy.UpdateVideoCard(videocard);
        }

        public void DeleteVideoCard(int id)
        {
            Proxy.DeleteVideoCard(id);
        }

        #endregion

        public void AddTextFile(string text, out string fileName)
        {
            byte[] bytes = Serialize(text);

            Proxy.AddFile(bytes, out fileName);
        }

        public void AddImageFile(byte[] image, out string fileName)
        {
            Proxy.AddFile(image, out fileName);
        }

        private void CreateIDisplayable(IDisplayable displayable)
        {
            if (displayable.BitmapImage != null)
            {
                string fileName;

                Bitmap bitmap = BitmapImageToBitmap(displayable.BitmapImage);

                byte[] image = Serialize(bitmap);

                AddImageFile(image, out fileName);

                displayable.ImageName = fileName;
            }
        }

        private void CreateIDescribable(IDescribable describable)
        {
            if (describable.Description != null)
            {
                string fileName;

                AddTextFile(describable.Description, out fileName);

                describable.DescriptionName = fileName;
            }
        }

        private void UpdateIDisplayable(IDisplayable displayable)
        {
            if (string.IsNullOrWhiteSpace(displayable.ImageName) && displayable.BitmapImage != null)
            {
                Bitmap bitmap = BitmapImageToBitmap(displayable.BitmapImage);

                byte[] image = Serialize(bitmap);

                AddImageFile(image, out var fileName);

                displayable.ImageName = fileName;
            }
        }

        private void UpdateIDescribable(IDescribable describable)
        {
            if (string.IsNullOrWhiteSpace(describable.DescriptionName) && describable.Description != null)
            {
                AddTextFile(describable.Description, out var fileName);

                describable.DescriptionName = fileName;
            }
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