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
    public class CheckPoint : ObjectV2
    {

        private Animation BasicAnimation;
        private Animation FlagAnimation;

        public bool hited;

        public int number;


        public CheckPoint(Vector2 Position, int number) : base(Position)
        {

            this.number = number;

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.checkPoint;

            hitbox.isEnabled = true;

            BasicAnimation = new Animation(Main.Object[(int)objectID], 4, 3, 0.1f, 1);
            BasicAnimation.Start();

            FlagAnimation = new Animation(Main.Object[(int)objectID], 4, 3, 0.1f, 2);
            FlagAnimation.Start();

        }

        public override void Update(GameTime gameTime)
        {

            if (hited)
            {
                if (FlagAnimation.GetFrame() != FlagAnimation.GetSourceRectangle().Count - 1)
                    FlagAnimation.Update(gameTime);
                else
                { FlagAnimation.Stop(); BasicAnimation.Update(gameTime); }
            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            
            if (hited)
            {
                if (FlagAnimation.GetFrame() != FlagAnimation.GetSourceRectangle().Count - 1)
                    FlagAnimation.Draw(spriteBatch, Position + new Vector2(-2, 0));
                else
                    BasicAnimation.Draw(spriteBatch, Position + new Vector2(-2, 0));
            }
            else
                spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(-2, 0), new Rectangle(0, 64, 32, 32), Color.White);

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
        }

    }
}
