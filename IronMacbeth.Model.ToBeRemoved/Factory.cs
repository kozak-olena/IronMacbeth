using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.Model.ToBeRemoved
{
    abstract class Factory
    {
        abstract public Creator Create(string s);
    }

    class CreateStoreClass : Factory
    {
        public override Creator Create(String s)
        {
            if (s == "Store")
            {
                return new StoreSomething();
            }
            else throw new NotImplementedException("Couldn't create instance of Store");
        }
    }

    class CreateRentClass : Factory
    {
        public override Creator Create(string s)
        {
            if (s == "Rent")
            {
                return new RentSomething();
            }
            else throw new NotImplementedException("Couldn't create instance of Rent");
        }
    }

    abstract class Creator
    {
        public int Id { get; set; }

        public int ThingId { get; set; }

        public int UserId { get; set; }   //user, who rented book

        private int _productPrice;
        public int ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
               // Modified = true;
            }
        }
       // public new bool Modified { get; set; }
    }

    class StoreSomething : Creator
    {
        public int StoreId { get; set; }
    }

    class RentSomething : Creator
    {
        public int RentId { get; set; }
    }


}

