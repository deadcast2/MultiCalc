using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MultiCalc
{
    public partial class Form1 : Form
    {
        private TextBox _FocusedTextBox;

        private Dictionary<Keys, string> _ValidKeys;

        public Form1()
        {
            InitializeComponent();

            _ValidKeys = new Dictionary<Keys, string>()
            {
                { Keys.D0, "0" },
                { Keys.D1, "1" },
                { Keys.D2, "2" },
                { Keys.D3, "3" },
                { Keys.D4, "4" },
                { Keys.D5, "5" },
                { Keys.D6, "6" },
                { Keys.D7, "7" },
                { Keys.D8, "8" },
                { Keys.D9, "9" },
                { Keys.NumPad0, "0" },
                { Keys.NumPad1, "1" },
                { Keys.NumPad2, "2" },
                { Keys.NumPad3, "3" },
                { Keys.NumPad4, "4" },
                { Keys.NumPad5, "5" },
                { Keys.NumPad6, "6" },
                { Keys.NumPad7, "7" },
                { Keys.NumPad8, "8" },
                { Keys.NumPad9, "9" },
                { Keys.Oemplus, "+" },
                { Keys.Add, "+" },
                { Keys.OemMinus, "-" },
                { Keys.Subtract, "-" },
                { Keys.Divide, "/" },
                { Keys.OemQuestion, "/" },
                { Keys.Multiply, "*" },
                { Keys.Enter, "" },
                { Keys.Back, "" },
                { Keys.Delete, "" },
                { Keys.OemPeriod, "." },
                { Keys.Decimal, "." }
            };
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (_FocusedTextBox != null && sender is Button button)
            {
                if (button.Text == buttonEquals.Text)
                {
                    HandleKeyEvent(Keys.Enter, false);
                }
                else if (button.Text == buttonDelete.Text)
                {
                    HandleKeyEvent(Keys.Back, false);
                }
                else if (button.Text == buttonClear.Text)
                {
                    _FocusedTextBox.Text = string.Empty;
                }
                else
                {
                    _FocusedTextBox.Text += button.Text;
                }
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyEvent(e.KeyCode, e.Shift);

            e.SuppressKeyPress = true;
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _FocusedTextBox = textBox;
            }
        }

        private void button_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            HandleKeyEvent(e.KeyCode, e.Shift);
        }

        private void HandleKeyEvent(Keys keyCode, bool shift)
        {
            if (_FocusedTextBox == null || !_ValidKeys.ContainsKey(keyCode))
                return;

            if (keyCode == Keys.Enter)
            {
                try
                {
                    var result = new DataTable().Compute(_FocusedTextBox.Text, "");

                    if (IsValid(result))
                    {
                        _FocusedTextBox.Text = result.ToString();
                    }
                }
                catch { }
            }
            else if (keyCode == Keys.Back || keyCode == Keys.Delete)
            {
                if (_FocusedTextBox.Text.Length > 0)
                {
                    _FocusedTextBox.Text = _FocusedTextBox.Text.Substring(keyCode == Keys.Back ? 0 : 1, _FocusedTextBox.Text.Length - 1);
                }
            }
            else if (keyCode == Keys.D8 && shift)
            {
                _FocusedTextBox.Text += buttonMultiply.Text;
            }
            else
            {
                _FocusedTextBox.Text += _ValidKeys[keyCode];
            }
        }

        private bool IsValid(object result)
        {
            if (result is double _d && double.IsInfinity(_d))
                return false;

            return true;
        }
    }
}
