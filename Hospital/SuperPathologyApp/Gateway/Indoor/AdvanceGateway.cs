using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Indoor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Gateway.Indoor
{
    class AdvanceGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(AdmissionModel mAdm, string comeFrom)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                mAdm.AdvanceAmtTrNo = Hlp.GetAutoIncrementVal("TrNo", "tb_in_ADVANCE_COLLECTION", 6, ConLab, _trans);

                var mdl = Hlp.GetRegAndBedId(mAdm.AdmId, ConLab, _trans);
                int advanceCollId = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Advance Collection'", "Id", _trans, ConLab));

                Hlp.InsertIntoPatientLedger(mAdm.AdvanceAmtTrNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, mAdm.AdmId, mdl.Bed.BedId, advanceCollId, "Advance Collection", 0, 1, 0, 0, mAdm.AdvanceAmt, 0, 0,0, 0, 0, ConLab, _trans);
                if (mAdm.AdvanceAmt > 0)
                {
                    Hlp.InsertIntoAdvanceColl(mAdm.AdvanceAmtTrNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, mAdm.AdmId, mdl.Bed.BedId, mAdm.AdvanceAmt, ConLab, _trans);
                }


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
      
        
    }
}
