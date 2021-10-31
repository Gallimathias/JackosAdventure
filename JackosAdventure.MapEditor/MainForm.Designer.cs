namespace JackosAdventure.MapEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.editor = new JackosAdventure.MapEditor.Editor();
            this.itemSelectionBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // editor
            // 
            this.editor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.editor.Location = new System.Drawing.Point(12, 12);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(1027, 918);
            this.editor.TabIndex = 0;
            // 
            // itemSelectionBox
            // 
            this.itemSelectionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemSelectionBox.FormattingEnabled = true;
            this.itemSelectionBox.ItemHeight = 20;
            this.itemSelectionBox.Location = new System.Drawing.Point(1037, 12);
            this.itemSelectionBox.Name = "itemSelectionBox";
            this.itemSelectionBox.Size = new System.Drawing.Size(150, 904);
            this.itemSelectionBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 942);
            this.Controls.Add(this.itemSelectionBox);
            this.Controls.Add(this.editor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Jackos Adventure - MapEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private Editor editor;
        private System.Windows.Forms.ListBox itemSelectionBox;
    }
}
