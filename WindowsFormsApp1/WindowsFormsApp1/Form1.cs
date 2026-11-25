using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private TextBox[,] _matrixCells;
        private Label _colArrowLabel;
        private List<MatrixStep> _steps;
        private int _currentStepIndex = 0;
        private bool _isUpdatingUI = false;

        private const int CellWidth = 65;
        private const int CellHeight = 35;
        private const int CellGap = 10;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRows.Text, out int m) && int.TryParse(txtCols.Text, out int n) && m > 0 && n > 0)
            {
                BuildMatrixGrid(m, n);
                _steps = null;
                lblStepDescription.Text = "Matrix generated. Enter numbers and click 'Solve'.";
            }
            else
            {
                MessageBox.Show("Please enter valid positive integers for dimensions.", "Input Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BuildMatrixGrid(int rows, int cols)
        {
            pnlMatrixContainer.Controls.Clear();
            _matrixCells = new TextBox[rows, cols];

            int totalWidth = cols * (CellWidth + CellGap) - CellGap;
            int totalHeight = rows * (CellHeight + CellGap) - CellGap;

            int startX = Math.Max(25, (pnlMatrixContainer.ClientSize.Width - totalWidth) / 2);
            int startY = Math.Max(25, (pnlMatrixContainer.ClientSize.Height - totalHeight - 35) / 2);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    TextBox tb = new TextBox
                    {
                        Width = CellWidth,
                        Height = CellHeight,
                        Font = new Font("Segoe UI", 12F),
                        TextAlign = HorizontalAlignment.Center,
                        Location = new Point(startX + j * (CellWidth + CellGap), startY + i * (CellHeight + CellGap)),
                        BackColor = Color.White,
                        Tag = new Point(i, j)
                    };

                    tb.TextChanged += CellInput_TextChanged;
                    tb.KeyDown += CellInput_KeyDown;
                    pnlMatrixContainer.Controls.Add(tb);
                    _matrixCells[i, j] = tb;
                }
            }

            _colArrowLabel = new Label
            {
                Text = "▲ Pivot",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = CellWidth,
                Height = 25,
                Visible = false
            };
            pnlMatrixContainer.Controls.Add(_colArrowLabel);

            if (rows > 0 && cols > 0)
                _matrixCells[0, 0].Focus();
        }

        private void CellInput_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;

            if (sender is TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text) || tb.Text == "-" || tb.Text == ".")
                {
                    tb.BackColor = Color.White;
                    return;
                }

                tb.BackColor = double.TryParse(tb.Text, out _) ? Color.Honeydew : Color.MistyRose;
            }
        }

        private void CellInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && sender is TextBox tb && tb.Tag is Point pos)
            {
                e.SuppressKeyPress = true;

                int rows = _matrixCells.GetLength(0);
                int cols = _matrixCells.GetLength(1);

                int nextCol = pos.Y + 1;
                int nextRow = pos.X;

                if (nextCol >= cols)
                {
                    nextCol = 0;
                    nextRow++;
                }

                if (nextRow < rows)
                {
                    _matrixCells[nextRow, nextCol].Focus();
                    _matrixCells[nextRow, nextCol].SelectAll();
                }
                else
                {
                    btnSolve.Focus();
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (_matrixCells == null) return;

            _isUpdatingUI = true;
            try
            {
                int rows = _matrixCells.GetLength(0);
                int cols = _matrixCells.GetLength(1);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        _matrixCells[i, j].Text = string.Empty;
                        _matrixCells[i, j].BackColor = Color.White;
                    }
                }

                if (_colArrowLabel != null)
                    _colArrowLabel.Visible = false;

                _steps = null;
                _currentStepIndex = 0;
                lblStepDescription.Text = "Matrix cleared. Enter new numbers and click 'Solve'.";

                if (rows > 0 && cols > 0)
                    _matrixCells[0, 0].Focus();
            }
            finally
            {
                _isUpdatingUI = false;
            }
        }

        private void BtnSolve_Click(object sender, EventArgs e)
        {
            if (_matrixCells == null)
            {
                MessageBox.Show("Please generate a matrix first.", "No Matrix",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rows = _matrixCells.GetLength(0);
            int cols = _matrixCells.GetLength(1);
            double[,] matrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!double.TryParse(_matrixCells[i, j].Text, out double val))
                    {
                        _matrixCells[i, j].BackColor = Color.MistyRose;
                        _matrixCells[i, j].Focus();
                        MessageBox.Show($"Invalid number at Row {i + 1}, Column {j + 1}. Please fix it and try again.",
                            "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    matrix[i, j] = val;
                }
            }

            bool toRref = cmbMode.SelectedItem?.ToString() == "RREF";
            _steps = GaussianSolver.Solve(matrix, toRref);
            _currentStepIndex = 0;
            DisplayCurrentStep();
        }

        private void DisplayCurrentStep()
        {
            if (_steps == null || _steps.Count == 0 || _matrixCells == null) return;

            var step = _steps[_currentStepIndex];
            lblStepDescription.Text = $"Step {_currentStepIndex + 1} of {_steps.Count}: {step.Description}";

            int rows = _matrixCells.GetLength(0);
            int cols = _matrixCells.GetLength(1);

            _isUpdatingUI = true;
            try
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        TextBox tb = _matrixCells[i, j];
                        tb.Text = step.State[i, j].ToString("0.##");

                        bool isHighlightedRow = step.HighlightRow.HasValue && step.HighlightRow.Value == i;
                        bool isHighlightedCol = step.HighlightCol.HasValue && step.HighlightCol.Value == j;

                        if (isHighlightedRow && isHighlightedCol)
                            tb.BackColor = Color.LightSkyBlue;
                        else if (isHighlightedRow)
                            tb.BackColor = Color.LightYellow;
                        else if (isHighlightedCol)
                            tb.BackColor = Color.AliceBlue;
                        else
                            tb.BackColor = Color.White;
                    }
                }

                if (step.HighlightCol.HasValue && step.HighlightCol.Value < cols)
                {
                    int col = step.HighlightCol.Value;
                    TextBox bottomCell = _matrixCells[rows - 1, col];
                    _colArrowLabel.Location = new Point(bottomCell.Left, bottomCell.Bottom + 6);
                    _colArrowLabel.Visible = true;
                }
                else
                {
                    _colArrowLabel.Visible = false;
                }
            }
            finally
            {
                _isUpdatingUI = false;
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_steps != null && _currentStepIndex < _steps.Count - 1)
            {
                _currentStepIndex++;
                DisplayCurrentStep();
            }
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (_steps != null && _currentStepIndex > 0)
            {
                _currentStepIndex--;
                DisplayCurrentStep();
            }
        }
    }
}