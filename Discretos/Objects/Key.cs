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
    public class Key : ObjectV2
    {

        private Animation BasicAnimation;

        private Vector2 NumAnimation;
        private float numX = 0.1f;
        private float numY = 0.1f;

        public int trigger;

        public new bool isCollected;


        public Key(Vector2 Position, int trigger) : base(Position)
        {

            this.trigger = trigger;

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.gold_key;

            BasicAnimation = new Animation(Main.Object[(int)objectID], 18, 1, 0.05f);
            BasicAnimation.Start();

            NumAnimation.X = Util.NextFloat(-0.8f, 0.8f);
            NumAnimation.Y = Util.NextFloat(-3, 3);

        }

        public override void Update(GameTime gameTime)
        {

            BasicAnimation.Update(gameTime);

            NumAnimation.Y += numX;
            NumAnimation.X += numY;

            if (NumAnimation.Y > 3 || NumAnimation.Y < -3)
                numX *= -1;

            if (NumAnimation.X > 0.8f || NumAnimation.X < -0.8f)
                numY *= -1;


            Position += Velocity;

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            if (!isCollected)
                BasicAnimation.Draw(spriteBatch, Position + new Vector2(0, NumAnimation.Y));
            else
                BasicAnimation.Draw(spriteBatch, Position + new Vector2(NumAnimation.X / 4, NumAnimation.Y));

        }

        public override void UpdateHitbox()
        {

            hitbox.rectangle = new Rectangle((int)Position.X + 5, (int)Position.Y, 7, 15);

        }

    }
}
