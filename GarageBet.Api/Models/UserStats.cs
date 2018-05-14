using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class UserStats
    {
        public UserModel User { get; set; }

        public int Won { get; set; }

        public int Result { get; set; }

        public int Lost { get; set; }

        public int Position { get; set; }
    }
}
