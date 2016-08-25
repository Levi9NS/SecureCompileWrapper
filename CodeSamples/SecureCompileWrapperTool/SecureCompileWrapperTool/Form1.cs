using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureCompileWrapperTool
{
    public partial class Form1 : Form
    {
        private CodeAnalyzer _codeAnalyzer;
        private List<string> _usedVariableTypes;
        private List<string> _usedMethodDefinitionsAndCalls;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            this.ClearAnalysisResult();

            if (string.IsNullOrEmpty(txCode.Text))
            {
                MessageBox.Show("There is no code to analyze.", "Warning");
            }
            else
            {
                try
                {
                    _codeAnalyzer = new CodeAnalyzer(txCode.Text);
                    if(_codeAnalyzer != null)
                    {
                        _usedVariableTypes = _codeAnalyzer.GetUsedVariableTypes();
                        _usedMethodDefinitionsAndCalls = _codeAnalyzer.GetNamesForDefinedOrCalledMethods();
                        this.WriteAnalysisResultToTextBox(txUsedVariableTypes, _usedVariableTypes);
                        this.WriteAnalysisResultToTextBox(txUsedMethodCallsAndDefinitions, _usedMethodDefinitionsAndCalls);
                    }
                    else
                    {
                        Console.WriteLine("Code contained errors, please correct them and try again.");
                    }

                    MessageBox.Show("Analysis compleated.","Notification");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ClearAnalysisResult()
        {
            txUsedVariableTypes.Text = null;
            txUsedMethodCallsAndDefinitions.Text = null;
        }

        private void ClearAllTextBoxes()
        {
            ClearAnalysisResult();
            txCode.Text = null;
        }

        private void WriteAnalysisResultToTextBox(TextBox textBox, List<string> analysisResult)
        {
            if(analysisResult != null && analysisResult.Count > 0)
            {
                foreach(string analysisResultItem in analysisResult.Distinct())
                {
                    textBox.Text += analysisResultItem + "\r\n";
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.ClearAllTextBoxes();
        }
    }
}
