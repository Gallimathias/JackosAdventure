using System;
using System.IO;
using System.Windows.Forms;

namespace JackosAdventure.MapEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            foreach (var type in editor.Map.TileTypes)
            {
                selectionItemBox.Items.Add(type);
            }

            selectionItemBox.SelectedIndexChanged += SelectionChanged;
        }

        private void SelectionChanged(object? sender, EventArgs e)
        {
            editor.CurrentTypeSelection = (string)selectionItemBox.SelectedItem;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "graveyard.map";
            dialog.Title = "Save map";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using var fileStream = File.OpenWrite(dialog.FileName);
                using var writer = new BinaryWriter(fileStream);
                editor.Map.Serialize(writer);
            }
        }
    }
}
