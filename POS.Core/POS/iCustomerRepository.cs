using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace POS.Core
{
    public interface iCustomerRepository
    {
        DataTable GetAll();

        bool Insert(CustomerModal obj);
        
        bool Update(CustomerModal obj);
        
        bool Delete(int CustomerId);

    }
}
