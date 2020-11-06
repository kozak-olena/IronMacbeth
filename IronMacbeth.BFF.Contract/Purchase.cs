using System;

namespace IronMacbeth.BFF.Contract
{
    public class Purchase
    {
        public int Id { get; set; }

        public int LinkId { get; set; }

        public int Number { get; set; }

        public string LinkName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Email { get; set; }

        public string Date { get; set; }

        public string IsRead { get; set; }
    }
}