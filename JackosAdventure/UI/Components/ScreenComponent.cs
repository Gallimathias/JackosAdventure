using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackosAdventure.UI.Controls;
using JackosAdventure.UI.Screens;

namespace JackosAdventure.UI.Components
{
    internal class ScreenComponent : ScreenGameComponent
    {
        private MenuScreen? menu;

        public ScreenComponent(Game game) : base(game)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            menu = new MenuScreen(this);
            NavigateTo(menu);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
