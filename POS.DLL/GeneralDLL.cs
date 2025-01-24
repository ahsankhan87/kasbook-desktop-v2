using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DLL
{
    public class GeneralDLL
    {
        private SqlDataAdapter da;
        SqlCommand cmd = new SqlCommand();
        //private DataTable dt = new DataTable();

        public DataTable GetRecord(string keyword,string table)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable(); 
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        da = new SqlDataAdapter("SELECT "+keyword+" FROM "+table+"",cn);
                        da.Fill(dt);
                        
                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }


        public List<string> GetProductsList()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                List<string> listRange = new List<string>();  
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        //da = new SqlDataAdapter("SELECT " + keyword + " FROM " + table + "", cn);
                        //da.Fill(dt);
                        cmd.Connection = cn;
                        cmd.CommandText = "SELECT TOP 500 id,code,name,item_type,barcode,qty,avg_cost,unit_price,location_code,description,date_created FROM pos_products ORDER BY id desc";
                        
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {

                                listRange.Add(rdr["id"].ToString());
                                listRange.Add(rdr["code"].ToString());
                                listRange.Add(rdr["name"].ToString());
                                listRange.Add(rdr["item_type"].ToString());
                                listRange.Add(rdr["barcode"].ToString());
                                listRange.Add(rdr["qty"].ToString());
                                listRange.Add(rdr["avg_cost"].ToString());
                                listRange.Add(rdr["unit_price"].ToString());
                                listRange.Add(rdr["location_code"].ToString());
                                listRange.Add(rdr["description"].ToString());
                                listRange.Add(rdr["date_created"].ToString());
                                
                            }
                            rdr.Close();
                        }
                        
                    }
                    return listRange;

                    
                }
                catch
                {
                    throw;
                }
            }
        }
        
    }
}
