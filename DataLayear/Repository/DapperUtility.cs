using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Repository
{
    public class DapperUtility
    {
        public SqlConnection GetConnection()
        {
            return new SqlConnection("Data Source=.;Initial Catalog=mohamm16_mp;Integrated Security=True");
        }
    }
}
