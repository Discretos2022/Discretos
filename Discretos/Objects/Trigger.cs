using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Objects
{
    public class Trigger : ObjectV2
    {

        public TriggerAction action;


        public Trigger(Vector2 Position, TriggerAction action) : base(Position)
        {

            this.action = action;

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.trigger;

        }

        public override void Update(GameTime gameTime)
        {



        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            //if(Main.Debug)
                switch (action.trigger)
                {

                    case TriggerAction.TriggerType.snow:
                        spriteBatch.Draw(Main.Object[(int)objectID], Position, new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;

                    /*case CactuxVariante.little:
                        spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(-5, -47), new Rectangle(28, 0, 27, 47), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        break;*/

                }

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
        }

    }
}
