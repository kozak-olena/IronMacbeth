using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMacbeth.BFF.Contract
{
   public class ReadingRoomOrder
    {
        public int Id { get; set; }

        public string UserLogin { get; set; }

        public int BookId { get; set; }

        public int ArticleId { get; set; }

        public int PeriodicalId { get; set; }

        public int NewspaperId { get; set; }

        public int ThesesID { get; set; }
    }
}
