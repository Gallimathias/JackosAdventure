using JackosAdventure.UI.Components;
using JackosAdventure.UI.Controls;
using engenious;
using engenious.Graphics;
using engenious.UI.Controls;
using engenious.UI;

namespace JackosAdventure.UI.Screens
{
    internal class GameScreen : Screen
    {
        public GameScreen(ScreenComponent screenComponent) : base(screenComponent)
        {
            screenComponent.Game.IsMouseVisible = false;
            Controls.Add(new GameControl(screenComponent));
        }
        
    }
}
