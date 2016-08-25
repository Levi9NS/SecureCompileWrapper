namespace SecureCompileWrapperTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txCode = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.txUsedVariableTypes = new System.Windows.Forms.TextBox();
            this.txUsedMethodCallsAndDefinitions = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblUsedVariableTypes = new System.Windows.Forms.Label();
            this.lblUsedMethodDefinitionsAndCalls = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txCode
            // 
            this.txCode.Location = new System.Drawing.Point(12, 59);
            this.txCode.Multiline = true;
            this.txCode.Name = "txCode";
            this.txCode.Size = new System.Drawing.Size(445, 421);
            this.txCode.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 496);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(922, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExecute,
            this.btnClear});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(922, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnExecute
            // 
            this.btnExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExecute.Image = ((System.Drawing.Image)(resources.GetObject("btnExecute.Image")));
            this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(23, 22);
            this.btnExecute.Text = "Execute";
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(23, 22);
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // txUsedVariableTypes
            // 
            this.txUsedVariableTypes.Location = new System.Drawing.Point(482, 59);
            this.txUsedVariableTypes.Multiline = true;
            this.txUsedVariableTypes.Name = "txUsedVariableTypes";
            this.txUsedVariableTypes.ReadOnly = true;
            this.txUsedVariableTypes.Size = new System.Drawing.Size(412, 198);
            this.txUsedVariableTypes.TabIndex = 3;
            // 
            // txUsedMethodCallsAndDefinitions
            // 
            this.txUsedMethodCallsAndDefinitions.Location = new System.Drawing.Point(482, 282);
            this.txUsedMethodCallsAndDefinitions.Multiline = true;
            this.txUsedMethodCallsAndDefinitions.Name = "txUsedMethodCallsAndDefinitions";
            this.txUsedMethodCallsAndDefinitions.ReadOnly = true;
            this.txUsedMethodCallsAndDefinitions.Size = new System.Drawing.Size(412, 198);
            this.txUsedMethodCallsAndDefinitions.TabIndex = 4;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(12, 43);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(32, 13);
            this.lblCode.TabIndex = 5;
            this.lblCode.Text = "Code";
            // 
            // lblUsedVariableTypes
            // 
            this.lblUsedVariableTypes.AutoSize = true;
            this.lblUsedVariableTypes.Location = new System.Drawing.Point(479, 43);
            this.lblUsedVariableTypes.Name = "lblUsedVariableTypes";
            this.lblUsedVariableTypes.Size = new System.Drawing.Size(100, 13);
            this.lblUsedVariableTypes.TabIndex = 6;
            this.lblUsedVariableTypes.Text = "Used variable types";
            // 
            // lblUsedMethodDefinitionsAndCalls
            // 
            this.lblUsedMethodDefinitionsAndCalls.AutoSize = true;
            this.lblUsedMethodDefinitionsAndCalls.Location = new System.Drawing.Point(479, 266);
            this.lblUsedMethodDefinitionsAndCalls.Name = "lblUsedMethodDefinitionsAndCalls";
            this.lblUsedMethodDefinitionsAndCalls.Size = new System.Drawing.Size(165, 13);
            this.lblUsedMethodDefinitionsAndCalls.TabIndex = 7;
            this.lblUsedMethodDefinitionsAndCalls.Text = "Used method definitions and calls";
            this.lblUsedMethodDefinitionsAndCalls.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 518);
            this.Controls.Add(this.lblUsedMethodDefinitionsAndCalls);
            this.Controls.Add(this.lblUsedVariableTypes);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txUsedMethodCallsAndDefinitions);
            this.Controls.Add(this.txUsedVariableTypes);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.txCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SecureCompileWrapperTool";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txCode;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private System.Windows.Forms.TextBox txUsedVariableTypes;
        private System.Windows.Forms.TextBox txUsedMethodCallsAndDefinitions;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblUsedVariableTypes;
        private System.Windows.Forms.Label lblUsedMethodDefinitionsAndCalls;
        private System.Windows.Forms.ToolStripButton btnClear;
    }
}

