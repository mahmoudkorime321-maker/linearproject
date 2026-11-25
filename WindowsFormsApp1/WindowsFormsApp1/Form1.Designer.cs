using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlTop;
        private Label lblRows;
        private TextBox txtRows;
        private Label lblCols;
        private TextBox txtCols;
        private ComboBox cmbMode;
        private Button btnGenerate;
        private Button btnClear;

        private Panel pnlMatrixContainer;

        private Panel pnlBottom;
        private Label lblStepDescription;
        private Button btnPrev;
        private Button btnSolve;
        private Button btnNext;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new Panel();
            this.lblRows = new Label();
            this.txtRows = new TextBox();
            this.lblCols = new Label();
            this.txtCols = new TextBox();
            this.cmbMode = new ComboBox();
            this.btnGenerate = new Button();
            this.btnClear = new Button();
            this.pnlMatrixContainer = new Panel();
            this.pnlBottom = new Panel();
            this.lblStepDescription = new Label();
            this.btnPrev = new Button();
            this.btnSolve = new Button();
            this.btnNext = new Button();

            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Dock = DockStyle.Top;
            this.pnlTop.Height = 65;
            this.pnlTop.BackColor = Color.WhiteSmoke;
            this.pnlTop.Controls.Add(this.lblRows);
            this.pnlTop.Controls.Add(this.txtRows);
            this.pnlTop.Controls.Add(this.lblCols);
            this.pnlTop.Controls.Add(this.txtCols);
            this.pnlTop.Controls.Add(this.cmbMode);
            this.pnlTop.Controls.Add(this.btnGenerate);
            this.pnlTop.Controls.Add(this.btnClear);

            // lblRows
            this.lblRows.Text = "Rows (m):";
            this.lblRows.AutoSize = true;
            this.lblRows.Font = new Font("Segoe UI", 10F);
            this.lblRows.Location = new Point(20, 22);

            // txtRows
            this.txtRows.Size = new Size(45, 25);
            this.txtRows.Font = new Font("Segoe UI", 10F);
            this.txtRows.TextAlign = HorizontalAlignment.Center;
            this.txtRows.Text = "3";
            this.txtRows.Location = new Point(92, 19);

            // lblCols
            this.lblCols.Text = "Cols (n):";
            this.lblCols.AutoSize = true;
            this.lblCols.Font = new Font("Segoe UI", 10F);
            this.lblCols.Location = new Point(150, 22);

            // txtCols
            this.txtCols.Size = new Size(45, 25);
            this.txtCols.Font = new Font("Segoe UI", 10F);
            this.txtCols.TextAlign = HorizontalAlignment.Center;
            this.txtCols.Text = "4";
            this.txtCols.Location = new Point(212, 19);

            // cmbMode
            this.cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMode.Font = new Font("Segoe UI", 10F);
            this.cmbMode.Items.AddRange(new object[] { "RREF", "REF" });
            this.cmbMode.SelectedIndex = 0;
            this.cmbMode.Size = new Size(80, 25);
            this.cmbMode.Location = new Point(275, 19);

            // btnGenerate
            this.btnGenerate.Text = "Generate Matrix";
            this.btnGenerate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnGenerate.Size = new Size(135, 32);
            this.btnGenerate.Location = new Point(375, 16);
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);

            // btnClear
            this.btnClear.Text = "Clear";
            this.btnClear.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnClear.BackColor = Color.MistyRose;
            this.btnClear.Size = new Size(85, 32);
            this.btnClear.Location = new Point(525, 16);
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);

            // pnlMatrixContainer
            this.pnlMatrixContainer.Dock = DockStyle.Fill;
            this.pnlMatrixContainer.AutoScroll = true;
            this.pnlMatrixContainer.BackColor = Color.White;

            // pnlBottom
            this.pnlBottom.Dock = DockStyle.Bottom;
            this.pnlBottom.Height = 105;
            this.pnlBottom.BackColor = Color.WhiteSmoke;
            this.pnlBottom.Controls.Add(this.lblStepDescription);
            this.pnlBottom.Controls.Add(this.btnPrev);
            this.pnlBottom.Controls.Add(this.btnSolve);
            this.pnlBottom.Controls.Add(this.btnNext);

            // lblStepDescription
            this.lblStepDescription.Text = "Enter dimensions, generate the matrix, fill values, and click 'Solve'.";
            this.lblStepDescription.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblStepDescription.ForeColor = Color.DarkBlue;
            this.lblStepDescription.TextAlign = ContentAlignment.MiddleCenter;
            this.lblStepDescription.Dock = DockStyle.Top;
            this.lblStepDescription.Height = 42;

            // btnPrev
            this.btnPrev.Text = "◀ Previous";
            this.btnPrev.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnPrev.Size = new Size(110, 34);
            this.btnPrev.Location = new Point(185, 52);
            this.btnPrev.Click += new System.EventHandler(this.BtnPrev_Click);

            // btnSolve
            this.btnSolve.Text = "Solve";
            this.btnSolve.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSolve.BackColor = Color.LightGreen;
            this.btnSolve.Size = new Size(120, 34);
            this.btnSolve.Location = new Point(315, 52);
            this.btnSolve.Click += new System.EventHandler(this.BtnSolve_Click);

            // btnNext
            this.btnNext.Text = "Next ▶";
            this.btnNext.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnNext.Size = new Size(110, 34);
            this.btnNext.Location = new Point(455, 52);
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);

            // Form1
            this.Text = "Gaussian Elimination Step-by-Step (WinForms)";
            this.ClientSize = new Size(760, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.pnlMatrixContainer);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}