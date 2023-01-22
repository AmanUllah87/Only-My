using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Pharmacy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 

namespace SuperPathologyApp.Gateway
{
    class ItemGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(ItemModel mdl)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_ph_ITEM", "Id=" + mdl.Id + "", _trans, ConLab) ? "Update tb_ph_ITEM SET GroupId=@GroupId, SuppId=@SuppId, Name=@Name, GenericName=@GenericName, SalePrice=@SalePrice, WholeSalePrice=@WholeSalePrice, ReOrderQty=@ReOrderQty, IsDiscItem=@IsDiscItem,    LastUpdateDtls=@LastUpdateDtls WHERE Id=@Id" : @"INSERT INTO tb_ph_ITEM(GroupId, SuppId, Name, GenericName, SalePrice, WholeSalePrice, ReOrderQty, IsDiscItem,  PostedBy, LastUpdateDtls, PcName, IpAddress) VALUES (@GroupId, @SuppId, @Name, @GenericName, @SalePrice, @WholeSalePrice, @ReOrderQty, @IsDiscItem,   @PostedBy, @LastUpdateDtls, @PcName, @IpAddress)";
                var cmd = new SqlCommand(query, ConLab,_trans);
                cmd.Parameters.Clear();
                //, , , , , , , , , , , , 
                
                
                cmd.Parameters.AddWithValue("@Id", mdl.Id);
                cmd.Parameters.AddWithValue("@GroupId", mdl.Group.Id);
                cmd.Parameters.AddWithValue("@SuppId", mdl.Supplier.Id);
                cmd.Parameters.AddWithValue("@Name", mdl.Name);

                cmd.Parameters.AddWithValue("@GenericName", mdl.GenericName);
                cmd.Parameters.AddWithValue("@SalePrice", mdl.SalePrice);
                cmd.Parameters.AddWithValue("@WholeSalePrice", mdl.WholeSalePrice);
                cmd.Parameters.AddWithValue("@ReOrderQty", mdl.ReOrderQty);
                cmd.Parameters.AddWithValue("@IsDiscItem", mdl.IsDiscItem);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@LastUpdateDtls", Hlp.UserName+"-"+DateTime.Now.ToString()+"-"+Hlp.IpAddress()+"-"+Environment.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());


                cmd.ExecuteNonQuery();

                _trans.Commit();
                ConLab.Close();
                
                return SaveSuccessMessage;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    ConLab.Close();
                }
                return exception.Message;
            }
        }

        internal DataTable GetItemList(int id, string searchString,int suppId)
        {
            try
            {
                string cond = "",suppCond="";
                if (searchString != "0")
                {
                    cond = "AND (convert(varchar,Id)+Name+GenericName+SuppName) like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                if (suppId != 0)
                {
                    suppCond = "AND SuppId=" + suppId+ "";
                }




                string query = @"SELECT * FROM V_ph_ITEM_LIST WHERE 1=1 "+ suppCond +" " + cond + "";

                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;
            }
            catch (Exception )
            {

                if (ConLab.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    ConLab.Close();
                }
                throw;
            }
        }
        
      
        
     


        
        
    }
}
