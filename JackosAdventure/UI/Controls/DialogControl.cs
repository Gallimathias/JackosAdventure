using engenious;
using engenious.UI;
using engenious.UI.Controls;
using JackosAdventure.Simulation;
using JackosAdventure.UI.Components;
using System.Linq;

namespace JackosAdventure.UI.Controls
{
    internal class DialogControl : Control
    {
        private Dialog? currentDialog;
        private Dialog? CurrentDialog
        {
            get => currentDialog;
            set
            {
                if (value == currentDialog)
                    return;
                if (value == null)
                {
                    Visible = false;
                }
                else
                {
                    if (value.Title != null)
                        titleLabel.Text = value.Title;
                    textLabel.Text = value.Text;

                    dialogList.Items.Clear();
                    foreach (var o in value.Options)
                        dialogList.Items.Add(o);

                    dialogList.SelectedItem = dialogList.Items.FirstOrDefault();
                }
                currentDialog = value;
            }
        }

        private readonly Label titleLabel;
        private readonly Label textLabel;
        private readonly Listbox<Dialog.DialogOption> dialogList;

        public DialogControl(ScreenComponent manager, string style = "") : base(manager, style)
        {
            Visible = false;

            var grid = new Grid(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            grid.Rows.Add(new RowDefinition() { Height = 1, ResizeMode = ResizeMode.Parts });
            grid.Columns.Add(new ColumnDefinition() { Width = 5, ResizeMode = ResizeMode.Parts });
            grid.Columns.Add(new ColumnDefinition() { Width = 1, ResizeMode = ResizeMode.Parts });

            var leftGrid = new Grid(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = Border.All(5),
                Padding = Border.All(5),
            };
            leftGrid.Rows.Add(new RowDefinition() { ResizeMode = ResizeMode.Fixed,Height=40, MinHeight = 40});
            leftGrid.Rows.Add(new RowDefinition() { ResizeMode = ResizeMode.Auto, Height = 20 });
            leftGrid.Columns.Add(new ColumnDefinition() { ResizeMode = ResizeMode.FitParts, Width = 1 });
            titleLabel = new Label(manager) { HorizontalAlignment = HorizontalAlignment.Stretch, TextColor = Color.White};
            textLabel = new Label(manager) { HorizontalAlignment = HorizontalAlignment.Stretch, TextColor = Color.White };
            leftGrid.AddControl(titleLabel, 0, 0);
            leftGrid.AddControl(textLabel, 0, 1);

            grid.AddControl(leftGrid, 0, 0);

            dialogList = new Listbox<Dialog.DialogOption>(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = SolidColorBrush.Transparent,
                DrawFocusFrame = false,
                SelectedItemBrush = SolidColorBrush.SaddleBrown,
            };
            dialogList.TemplateGenerator = (d) =>
            {
                return new Label(manager) { Text = d!.Text, TextColor = Color.White };
            };
            dialogList.KeyDown += DialogList_KeyDown;


            grid.AddControl(dialogList, 1, 0);


            Children.Add(grid);
        }

        public override void SetActualSize(Point available)
        {
            base.SetActualSize(available);
            if (dialogList.Focused != TreeState.Active)
                dialogList.Focus();
        }
        private void DialogList_KeyDown(Control sender, KeyEventArgs args)
        {
            if (dialogList.SelectedItem == null)
                return;
            if (args.Key == engenious.Input.Keys.Enter)
            {
                CurrentDialog = dialogList.SelectedItem.NextDialog;
            }
        }

        public void Show(Dialog dialog)
        {
            Visible = true;
            CurrentDialog = dialog;
        }
        public void Close()
        {
            Visible = false;
            CurrentDialog = null;
        }
    }
}
