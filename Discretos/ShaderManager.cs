using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    static class ShaderManager
    {

        public static void Update(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    //Screen.BackgroundShader = Render.ShaderEffect.None;
                    Screen.LevelShader = Render.ShaderEffect.None;
                    Main.LevelPlaying = 0;
                    break;

                case GameState.Settings:
                    Screen.BackgroundShader = Render.ShaderEffect.None;
                    Screen.LevelShader = Render.ShaderEffect.None;
                    Main.LevelPlaying = 0;
                    break;

                case GameState.Playing:
                    switch (Main.LevelPlaying)
                    {

                        case 3:
                            Screen.BackgroundShader = Render.ShaderEffect.LightMaskBackground;
                            Screen.LevelShader = Render.ShaderEffect.LightMaskLevel;
                            break;

                        case 4:
                            Screen.BackgroundShader = Render.ShaderEffect.HeatDistortion;
                            Screen.LevelShader = Render.ShaderEffect.HeatDistortion;
                            break;

                        default:
                            Screen.BackgroundShader = Render.ShaderEffect.None;
                            Screen.LevelShader = Render.ShaderEffect.None;
                            break;

                    }
                    break;



            }
        }

    }
}
