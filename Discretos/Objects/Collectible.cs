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
    public class Collectible : ObjectV2
    {

        private Animation BasicAnimation;
        private CollectibleType collectibleType;

        private Vector2 NumAnimation;
        private float numX = 0;
        private float numY = 0.1f;


        public Collectible(Vector2 Position, CollectibleType type) : base(Position)
        {

            collectibleType = type;

            Init();
            
        }

        public override void Init()
        {

            if (collectibleType == CollectibleType.coin) objectID = ObjectID.coin;
            if (collectibleType == CollectibleType.life) objectID = ObjectID.core;

            if(collectibleType == CollectibleType.coin) 
            {
                BasicAnimation = new Animation(Main.Object[(int)objectID], 4, 1, 0.1f);
                BasicAnimation.Start();
                light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 20f, Color.White);
                LightManager.lights.Add(light);
            }
            else if (collectibleType == CollectibleType.life)
            {
                BasicAnimation = new Animation(Main.Object[(int)objectID], 4, 1, 0.1f);
                BasicAnimation.Start();
                light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 20f, new Color(255, 100, 100));
                LightManager.lights.Add(light);
            }

        }

        public override void Update(GameTime gameTime)
        {

            BasicAnimation.Update(gameTime);


            if (light != null)
            {

                if (collectibleType == CollectibleType.life)
                {
                    //light.Radius = (float)(Math.Abs(Math.Cos(BasicAnimation.GetFrame() / 2) * 20 + 5));
                    light.Radius = (float)(Math.Cos(Math.Abs(NumAnimation.Y / 2)) * 20 + 15);
                    light.Position = new Vector2(GetRectangle().X + GetRectangle().Width / 2, GetRectangle().Y + GetRectangle().Height / 2) + NumAnimation;

                }
                else
                    light.Position = new Vector2(GetRectangle().X + GetRectangle().Width / 2, GetRectangle().Y + GetRectangle().Height / 2);

            }



            NumAnimation.X += numX;
            NumAnimation.Y += numY;

            if (NumAnimation.Y > 3 || NumAnimation.Y < -3)
                numY *= -1;

            if (NumAnimation.X > 0.8f || NumAnimation.X < -0.8f)
                numY *= -1;

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            if (collectibleType == CollectibleType.coin)
                BasicAnimation.Draw(spriteBatch, Position);
            else if (collectibleType == CollectibleType.life)
                BasicAnimation.Draw(spriteBatch, Position + NumAnimation);

        }

        public override void UpdateHitbox()
        {
            switch (objectID)
            {
                case ObjectID.coin:
                    hitbox.rectangle = new Rectangle((int)Position.X + 3, (int)Position.Y + 1, 10, 14);
                    break;
                case ObjectID.core:
                    hitbox.rectangle = new Rectangle((int)Position.X + 1, (int)Position.Y, 13, 14);
                    break;
            }

        }


        public enum CollectibleType
        {

            coin = 1,
            life = 2,

        }

    }
}
