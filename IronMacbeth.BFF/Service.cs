using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronMacbeth.Model.ToBeRemoved;
using Microsoft.EntityFrameworkCore;

namespace IronMacbeth.BFF
{
    public class Service : IService
    {

        #region Book
        public void CreateBook(Book book)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(book);

                dbContext.SaveChanges();
            }
        }

        public List<Book> GetAllBooks()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Books.ToList();
            }
        }

        public void UpdateBook(Book book)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(book);

                dbContext.SaveChanges();
            }
        }

        public void DeleteBook(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Books.Remove(new Book { Id = id });

                dbContext.SaveChanges();
            }
        }
        #endregion

        #region StoreBook

        public void CreateStoreBook(StoreBook storeBook)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeBook);

                dbContext.SaveChanges();
            }
        }

        public List<StoreBook> GetAllStoreBooks()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreBook.ToList();
            }
        }

        public void UpdateStoreBooks(StoreBook storeBook)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeBook);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreBook(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreBook.Remove(new StoreBook { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Memory

        public void CreateMemory(Memory memory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(memory);

                dbContext.SaveChanges();
            }
        }

        public List<Memory> GetAllMemories()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Memories.ToList();
            }
        }

        public void UpdateMemory(Memory memory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(memory);

                dbContext.SaveChanges();
            }
        }

        public void DeleteMemory(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Memories.Remove(new Memory { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Motherboard

        public void CreateMotherboard(Motherboard motherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(motherboard);

                dbContext.SaveChanges();
            }
        }

        public List<Motherboard> GetAllMotherboards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Motherboards.ToList();
            }
        }

        public void UpdateMotherboard(Motherboard motherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(motherboard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteMotherboard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Motherboards.Remove(new Motherboard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Processor

        public void CreateProcessor(Processor processor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(processor);

                dbContext.SaveChanges();
            }
        }

        public List<Processor> GetAllProcessors()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Processors.ToList();
            }
        }

        public void UpdateProcessor(Processor processor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(processor);

                dbContext.SaveChanges();
            }
        }

        public void DeleteProcessor(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Processors.Remove(new Processor { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Purchase

        public void CreatePurchase(Purchase purchase)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(purchase);

                dbContext.SaveChanges();
            }
        }

        public List<Purchase> GetAllPurchases()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Purchases.ToList();
            }
        }

        public void UpdatePurchase(Purchase purchase)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(purchase);

                dbContext.SaveChanges();
            }
        }

        public void DeletePurchase(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Purchases.Remove(new Purchase { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Store

        public void CreateStore(Store store)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(store);

                dbContext.SaveChanges();
            }
        }

        public List<Store> GetAllStores()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Stores.ToList();
            }
        }

        public void UpdateStore(Store store)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(store);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStore(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Stores.Remove(new Store { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreMemory

        public void CreateStoreMemory(StoreMemory storeMemory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeMemory);

                dbContext.SaveChanges();
            }
        }

        public List<StoreMemory> GetAllStoreMemories()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreMemories.ToList();
            }
        }

        public void UpdateStoreMemory(StoreMemory storeMemory)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeMemory);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreMemory(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreMemories.Remove(new StoreMemory { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreMotherboard

        public void CreateStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeMotherboard);

                dbContext.SaveChanges();
            }
        }

        public List<StoreMotherboard> GetAllStoreMotherboards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreMotherboards.ToList();
            }
        }

        public void UpdateStoreMotherboard(StoreMotherboard storeMotherboard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeMotherboard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreMotherboard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreMotherboards.Remove(new StoreMotherboard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreProcessor

        public void CreateStoreProcessor(StoreProcessor storeProcessor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeProcessor);

                dbContext.SaveChanges();
            }
        }

        public List<StoreProcessor> GetAllStoreProcessors()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreProcessors.ToList();
            }
        }

        public void UpdateStoreProcessor(StoreProcessor storeProcessor)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeProcessor);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreProcessor(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreProcessors.Remove(new StoreProcessor { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region StoreVideocard

        public void CreateStoreVideocard(StoreVideocard storeVideocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(storeVideocard);

                dbContext.SaveChanges();
            }
        }

        public List<StoreVideocard> GetAllStoreVideoCards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.StoreVideoCards.ToList();
            }
        }

        public void UpdateStoreVideocard(StoreVideocard storeVideocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(storeVideocard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteStoreVideocard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.StoreVideoCards.Remove(new StoreVideocard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region User

        public void Register(User user)
        {
            using (var dbContext = new DbContext())
            {
                // EntityFramework doesn't support adding entities without primary key
                dbContext.Database.ExecuteSqlRaw("INSERT [dbo].[User](Login, Password, AccessLevel) VALUES(@p0, @p1, 0)", user.Login, user.Password);

                dbContext.SaveChanges();
            }
        }

        public User LogIn(string login, string password)
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.Users.SingleOrDefault(x => x.Login == login && x.Password == password);
            }
        }

        #endregion

        #region Videocard

        public void CreateVideoCard(Videocard videocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Add(videocard);

                dbContext.SaveChanges();
            }
        }

        public List<Videocard> GetAllVideoCards()
        {
            using (var dbContext = new DbContext())
            {
                return dbContext.VideoCards.ToList();
            }
        }

        public void UpdateVideoCard(Videocard videocard)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.Update(videocard);

                dbContext.SaveChanges();
            }
        }

        public void DeleteVideoCard(int id)
        {
            using (var dbContext = new DbContext())
            {
                dbContext.VideoCards.Remove(new Videocard { Id = id });

                dbContext.SaveChanges();
            }
        }

        #endregion

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