using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Plateform_2D_v9.Core;

namespace Plateform_2D_v9.Objects
{
    class LiquidDrop : ObjectV2
    {

        private Animation BasicAnimation;
        private bool onGround = false;

        private LiquidDropGenerator.LiquidType type;

        public LiquidDrop(Vector2 Position, LiquidDropGenerator.LiquidType type) : base(Position)
        {

            this.type = type;

            Init();
        }

        public override void Init()
        {

            objectID = ObjectID.liquid_drop;

            if(type == LiquidDropGenerator.LiquidType.Water)
                BasicAnimation = new Animation(Main.Object[(int)objectID], 9, 4, 0.025f, 2);
            else if(type == LiquidDropGenerator.LiquidType.Lava)
                BasicAnimation = new Animation(Main.Object[(int)objectID], 9, 4, 0.025f * 2, 4);

            Velocity = new Vector2(0, 3);

        }

        public override void Update(GameTime gameTime)
        {

            if (BasicAnimation.GetFrame() == BasicAnimation.GetSourceRectangle().Count - 1)
                Handler.RemoveActor(this);

            if(onGround)
                BasicAnimation.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (!onGround)
            {
                if (type == LiquidDropGenerator.LiquidType.Water)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position, new Rectangle(77, 0, 11, 8), Color.White);
                else if (type == LiquidDropGenerator.LiquidType.Lava)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position, new Rectangle(77, 16, 11, 8), Color.White);

            }
            else
                BasicAnimation.Draw(spriteBatch, Position);

        }

        public override void UpdateHitbox()
        {
            hitbox = new Hitbox((int)Position.X, (int)Position.Y, 11, 8);
            hitbox.isEnabled = true;
        }


        public override void DownDisplacement(GameTime gameTime)
        {

            Position.Y += Velocity.Y;
            Velocity.Y += 0.1f;

            if (Velocity.Y > 6)
                Velocity.Y = 6;

            UpdateHitbox();

        }


        public override void DownStaticCollisionAction()
        {

            if (!onGround)
            {
                BasicAnimation.Reset();
                BasicAnimation.Start();
                onGround = true;
            }

        }
    }
}
