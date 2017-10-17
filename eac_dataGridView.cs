/* eac_dataGridView, SQL & C# Framework for windows forms DataGridView
    Emmanuel Cardenaz - 2017-10-01
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace EAC_Framework
{
    class eac_dataGridView
    {
        /**************Controls**************/
        DataGridView gridview;
        eac_sqlConnector sqlConn;
        /**************Names**************/
        string tableName;
        /**************FLAGS**************/
        bool in_newRow = false; //For new row added by user
        bool enable_autoUpdate = true; //SQL AutoUpdate, 
        bool enable_autoInsert = true; //SQL for Autoinsert


        public eac_dataGridView(ref DataGridView gridviews)
        {
            enable_autoUpdate = true;
            gridview = gridviews; // Usig pointer for edit in form //For event CellClick
            gridview.CellClick += new System.Windows.
                Forms.DataGridViewCellEventHandler(onClick);
            gridview.CellEndEdit += new System.Windows.Forms.
                DataGridViewCellEventHandler(CellEndEditUpdateGrid);
            gridview.CellValueChanged += //It is for Paste from clipboard
                new System.Windows.Forms.DataGridViewCellEventHandler(CellEndEditUpdateGrid); 
            gridview.KeyDown += new System.Windows.Forms.
                KeyEventHandler(copyPaste_KeyDown);
            gridview.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(NewRowAddedByUser);
        }

        public void fillGridFromSqlSelect(string QuerySelect, ref eac_sqlConnector sqlConnector)
        {
            sqlConn = sqlConnector;
            gridview.DataSource = null; 
            gridview.Rows.Clear();
            DataTable result = sqlConn.Select(QuerySelect);
            gridview.DataSource = result;
            tableName = result.TableName;
        }

        private void onClick(object sender, DataGridViewCellEventArgs e)
        {
            //   MessageBox.Show("Works here");
        }

        private void CellEndEditUpdateGrid(object sender, DataGridViewCellEventArgs e)
        {
            string queryUpdate = "UPDATE " + tableName;
            if (enable_autoUpdate)
            {
                int idx = gridview.CurrentCell.ColumnIndex;
                DataGridViewRow actualRow = gridview.Rows[gridview.CurrentCell.RowIndex];
                List<string> sqlFields = new List<string>();
                queryUpdate += " SET [" + gridview.Columns[idx].Name + "] = '" +
                    actualRow.Cells[idx].Value.ToString() + "' WHERE ";

                for (int i = 0; i < gridview.Columns.Count; i++)
                {
                    if (i != idx)
                    {
                        sqlFields.Add(" [" + gridview.Columns[i].Name + "] = '" +
                            actualRow.Cells[i].Value.ToString() + "'");
                    }
                }
                queryUpdate += string.Join(" AND", sqlFields.ToArray());
                sqlConn.Update(queryUpdate);
                //MessageBox.Show(queryUpdate);
            }
        }

        private void AutoInsert()
        {
           if (enable_autoInsert)
            {
             //CODE HERE FOR INSERTS>>>> 
            }
        }

        private void NewRowAddedByUser(object sender, DataGridViewRowEventArgs e)
        {
            MessageBox.Show("NEW ROW ADDED");
            in_newRow = true;
        }

        private void CopyToClipboard(object sender)
        {
            DataObject dataObj = gridview.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void PasteClipboard(object sender)
        {   
            //This code (Copy Paste) is no my own.  But i no remember the Blog, i will reference this... 
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');

                int iRow = gridview.CurrentCell.RowIndex;
                int iCol = gridview.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                if (iRow + lines.Length > gridview.Rows.Count - 1)
                {
                    bool bFlag = false;
                    foreach (string sEmpty in lines)
                    {
                        if (sEmpty == "")
                        {
                            bFlag = true;
                        }
                    }
                    int iNewRows = iRow + lines.Length - gridview.Rows.Count;
                    if (iNewRows > 0)
                    {
                        if (bFlag)
                            gridview.Rows.Add(iNewRows);
                        else
                            gridview.Rows.Add(iNewRows + 1);
                    }
                    else
                        gridview.Rows.Add(iNewRows + 1);
                }
                foreach (string line in lines)
                {
                    if (iRow < gridview.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < gridview.ColumnCount)
                            {
                                oCell = gridview[iCol + i, iRow];
                                oCell.Value = Convert.ChangeType(sCells[i].Replace("\r", ""), oCell.ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        iRow++;
                    }
                    else
                    {
                        break;
                    }
                }
                Clipboard.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }
        private void copyPaste_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            CopyToClipboard(sender);
                            break;

                        case Keys.V:
                            PasteClipboard(sender);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Copy/paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
