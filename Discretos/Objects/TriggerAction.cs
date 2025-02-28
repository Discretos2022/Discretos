using Microsoft.Xna.Framework.Graphics;

namespace Plateform_2D_v9.Objects
{
    public abstract class TriggerAction
    {

        public Texture2D tex;
        public TriggerType trigger;


        public abstract void Action();


        public enum TriggerType
        {
            snow = 0,
        }

    }
}