using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                itemSelectionBox.Items.Add(type);
            }

            itemSelectionBox.SelectedIndexChanged += SelectionChanged;
        }

        private void SelectionChanged(object? sender, EventArgs e)
        {
            editor.CurrentTypeSelection = (string)itemSelectionBox.SelectedItem;
        }
    }
}
