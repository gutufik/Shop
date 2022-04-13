using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public partial class ShopMyasnikovEntities
    {
        private static ShopMyasnikovEntities _context;
        public static ShopMyasnikovEntities GetContext()
        {
            if (_context == null)
                _context = new ShopMyasnikovEntities();
            return _context;
        }
    }
}
