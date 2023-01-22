using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperPathologyApp.UI
{
    public class KeyMessageFilter : IMessageFilter
    {
        private Form m_target = null;

        public KeyMessageFilter(Form targetForm)
        {
            m_target = targetForm;
        }

        private const int WM_KEYDOWN = 0x0100;

        private const int WM_KEYUP = 0x0101;


        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                //Note this ensures Enter is only filtered if in the 
                // DataGridViewTextBoxEditingControl and Shift is not also  pressed.
                if (m_target.ActiveControl != null &&
                    m_target.ActiveControl is DataGridViewTextBoxEditingControl &&
                    (Keys)m.WParam == Keys.Enter &&
                    (Control.ModifierKeys & Keys.Shift) != Keys.Shift)
                {
                    return true;
                }

            }

            return false;
        }
    }
}
