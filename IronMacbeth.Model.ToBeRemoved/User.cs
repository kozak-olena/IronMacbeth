using System;
using System.Collections.Generic;
using System.Linq;

namespace IronMacbeth.Model.ToBeRemoved
{
    [Serializable]
    public class User:Base<User>
    {
        [Database]
        public string Login { get; set; }
        [Database]
        public string Password { get; set; }
        [Database]
        public int AccessLevel { get; set; }

        public List<Store> Stores
        {
            get { return Store.Items.Where(item => item.OwnerId == Login).ToList(); }
        }

        public override string DisplayString => $"User: Login: {Login}";
    }
}
