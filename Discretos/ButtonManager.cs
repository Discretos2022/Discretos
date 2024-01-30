using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    static class ButtonManager
    {

        public static List<ButtonV3> mainMenuButtons;

        public static List<ButtonV3> settingsButtons;
        public static List<ButtonV3> generalSettingsButtons;
        public static List<ButtonV3> videoSettingsButtons;
        public static List<ButtonV3> audioSettingsButtons;
        public static List<ButtonV3> controlsSettingsButtons;


        public static void InitButtonManager()
        {
            mainMenuButtons = new List<ButtonV3>();

            settingsButtons = new List<ButtonV3>();
            generalSettingsButtons = new List<ButtonV3>();
            videoSettingsButtons = new List<ButtonV3>();
            audioSettingsButtons = new List<ButtonV3>();
            controlsSettingsButtons = new List<ButtonV3>();

        }

    }
}
