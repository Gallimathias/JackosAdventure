using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackosAdventure.Simulation
{
    internal class Dialog
    {
        public string? Title { get;  }
        public string Text { get; }
        public List<DialogOption> Options { get;  }

        public Dialog(string text, string? title = null, params DialogOption[] options)
        {
            Title = title;
            Text = text;
            Options = new List<DialogOption>(options);
        }

        public class DialogOption
        {
            public string Text { get; }
            public Dialog? NextDialog { get; }

            public DialogOption(string text, Dialog? nextDialog = null)
            {
                Text = text;
                NextDialog = nextDialog;
            }
        }

    }
}
