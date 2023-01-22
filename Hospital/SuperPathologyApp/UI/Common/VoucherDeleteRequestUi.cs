using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper ;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.UI
{
    public partial class VoucherDeleteRequestUi : Form
    {
        VoucherDeleteRequestGateway _gt = new VoucherDeleteRequestGateway();
        public VoucherDeleteRequestUi()
        {
            InitializeComponent();
        }

        


   
       
       

        private void frmDoctorSetup_Activated(object sender, EventArgs e)
        {
        }

     
        public string textinfo = "";
 

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_Doctor", "Id='" + projectComboBox.SelectedValue + "'")==false)
            {
                MessageBox.Show(@"Invalid Docotor Name Please Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var lists = new List<SubProjectModel>();

            for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                //string result = dataGridView2.Rows[i].Cells[3].Value == null ? "" : dataGridView2.Rows[i].Cells[3].Value.ToString();
                    lists.Add(new SubProjectModel()
                    {
                        SubProjectId = Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value),
                        HnrAmt = Convert.ToDouble(dataGridView2.Rows[i].Cells[2].Value),
                        Type = dataGridView2.Rows[i].Cells[3].Value.ToString(),
                    });
            }

            var mdl = new DoctorModel
            {
                DrId =Convert.ToInt32(projectComboBox.SelectedValue),
                SubProject = lists,

            };

            string msg =  _gt.Save(mdl);
            if (msg == "Saved Success")
            {
                projectComboBox.SelectedIndex = 0;
              
                MessageBox.Show(msg, @"Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       
      

        private void button3_Click(object sender, EventArgs e)
        {
        }

   

       
       

  
       
      

        private void GetProjectList(int drId,DateTime dtDateFrom,DateTime dtDateTo)
        {
            dataGridView2.Rows.Clear();
            var aList=new List<BillModel>(); 
            
            switch (drId)
            {
                case 0://Diagnosis Invoice
                    aList = _gt.GetRequestBillList(dtDateFrom,dtDateTo,projectComboBox.Text);
                    break;
            }

            foreach (var mdl in aList)
            {
                dataGridView2.Rows.Add(mdl.BillId, mdl.BillNo, mdl.BillDate.ToString("dd-MMM-yyyy"), mdl.PatientName,mdl.TotalAmt,mdl.BillStatus,mdl.PostedBy,mdl.DeliveryTimeAmPm);
            }

            Hlp.GridColor(dataGridView2);
          
        }

    

     

        private void VoucherDeleteRequestUi_Load(object sender, EventArgs e)
        {
            projectComboBox.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetProjectList(Convert.ToInt32(projectComboBox.SelectedValue),dtDateFrom.Value,dtDateTo.Value);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells[5].Value = true;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells[5].Value = false;
            }
        }

        private SqlTransaction _trans;
        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {

            _gt.ConLab.Open();
            _trans = _gt.ConLab.BeginTransaction();

            for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                int checkedValue = Convert.ToInt32(dataGridView2.Rows[i].Cells[5].Value);
                string masterId = dataGridView2.Rows[i].Cells[0].Value.ToString();
                string trNo = dataGridView2.Rows[i].Cells[1].Value.ToString();
                DateTime trDate = DateTime.Now;
                try
                {
                    trDate = Convert.ToDateTime(dataGridView2.Rows[i].Cells[2].Value.ToString());

                }
                catch (Exception)
                {
                    ;
                }
                
                string pendingStatus = dataGridView2.Rows[i].Cells[7].Value.ToString();
                
                if (checkedValue == 1)
                {
                    switch (projectComboBox.SelectedIndex)
                    {
                        case 0://Bill
                            if (pendingStatus=="")
                            {
                                _gt.DeleteInsertLab("DELETE FROM tb_BILL_MASTER WHERE Id=" + masterId + "", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_BILL_DETAIL WHERE MasterId=" + masterId + "", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE MasterId=" + masterId + "", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='"+ Hlp.IpAddress() +"' WHERE MasterId=" + masterId + " AND ModuleName='Bill'", _trans, _gt.ConLab);
                                
                            }
                            break;
                        case 1://Due-Collection
                            if (pendingStatus == "")
                            {
                                _gt.DeleteInsertLab("DELETE FROM tb_DUE_COLL WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE BillNo='" + trNo + "' AND ModuleName='Due-Coll'", _trans, _gt.ConLab);

                            }
                            break;

                        case 2://Voucher
                            if (pendingStatus == "")
                            {
                                _gt.DeleteInsertLab("DELETE FROM tb_ac_EXPENSE_ENTRY WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE BillNo='" + trNo + "' AND ModuleName='Voucher'", _trans, _gt.ConLab);
                            }
                            break;
                        case 3://In-Bill
                            if (pendingStatus == "")
                            {
                                masterId =_gt.FncReturnFielValueLab("tb_IN_BILL_MASTER","BillDate='" + trDate.ToString("yyyy-MM-dd") + "' ANd BillNo='" + trNo + "'", "Id",_trans,_gt.ConLab);
                                string admId = _gt.FncReturnFielValueLab("tb_IN_BILL_MASTER", "Id='" + masterId + "'", "AdmId", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_IN_BILL_MASTER WHERE Id=" + masterId + "", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_in_BILL_DETAIL WHERE MasterId=" + masterId + "", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_in_PATIENT_LEDGER WHERE TrDate='" + trDate.ToString("yyyy-MM-dd") + "' ANd TrNo='" + trNo + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("Update tb_in_ADMISSION SET ReleaseStatus=0 WHERE Id='"+ admId +"'", _trans, _gt.ConLab);
                                
                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE MasterId=" + masterId + " AND ModuleName='In-Bill'", _trans, _gt.ConLab);

                            }
                            break;

                        case 4://In-Due_coll
                            if (pendingStatus == "")
                            {
                                _gt.DeleteInsertLab("DELETE FROM tb_DUE_COLL WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_in_PATIENT_LEDGER WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);

                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE BillNo='" + trNo + "' AND ModuleName='In-DueColl'", _trans, _gt.ConLab);
                            }
                            break;
                        case 5://In-Advance
                            if (pendingStatus == "")
                            {
                                _gt.DeleteInsertLab("DELETE FROM tb_in_ADVANCE_COLLECTION WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_in_PATIENT_LEDGER WHERE TrNo='" + trNo + "'", _trans, _gt.ConLab);

                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE BillNo='" + trNo + "' AND ModuleName='In-Advance'", _trans, _gt.ConLab);
                            }
                            break;
                        case 6://Pharmacy-Purchase
                            if (pendingStatus == "")
                            {
                                masterId = _gt.FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "BillDate='" + trDate.ToString("yyyy-MM-dd") + "' ANd BillNo='" + trNo + "'", "Id", _trans, _gt.ConLab);

                                _gt.DeleteInsertLab("DELETE FROM tb_ph_PURCHASE_MASTER WHERE Id='" + masterId + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_ph_PURCHASE_DETAIL WHERE MasterId='" + masterId + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_ph_PURCHASE_LEDGER WHERE MasterId='" + masterId + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_ph_STOCK_LEDGER WHERE MasterId='" + masterId + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE BillNo='" + trNo + "' AND ModuleName='Pharmacy-Purchase'", _trans, _gt.ConLab);
                            }
                            break;
                        case 7://In-Admission
                            if (pendingStatus == "")
                            {

                                int bedId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_IN_ADMISSION","Id='"+ masterId +"'","BedId",_trans,_gt.ConLab));
                                _gt.DeleteInsertLab("DELETE FROM tb_IN_ADMISSION WHERE Id='" + masterId + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_in_PATIENT_LEDGER WHERE AdmId='" + masterId + "'", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("DELETE FROM tb_in_ADVANCE_COLLECTION WHERE AdmId='" + masterId + "'", _trans, _gt.ConLab);

                                _gt.DeleteInsertLab("Update tb_in_BED SET BookStatus=0 WHERE Id=" + bedId + "", _trans, _gt.ConLab);
                                _gt.DeleteInsertLab("UPDATE DEL_RECORD_OF_BILL_DELETE SET AuthorizedDateTime='" + DateTime.Now + "',AuthorizedBy='" + Hlp.UserName + "',Status='Complete',PcName='" + Environment.UserName + "',IpAddress='" + Hlp.IpAddress() + "' WHERE MasterId=" + masterId + " AND ModuleName='In-Admission'", _trans, _gt.ConLab);

                            
                            }
                            break;




                    }
                    
                }
            }
            MessageBox.Show(@"Authorized Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _trans.Commit();
            _gt.ConLab.Close();
        }
    }
}
