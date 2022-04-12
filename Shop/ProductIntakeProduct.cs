using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Core;

namespace Shop
{
    public partial class IntakeProduct : ProductIntakeProduct
    {
        public ComboBox cbProduct { get; set; }
        public decimal Sum => Count * PriceUnit;
        //public override decimal PriceUnit { get;  }
    }
}
