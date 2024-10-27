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
    public class Spring : ObjectV2
    {

        private Animation BasicAnimation;


        public Spring(Vector2 Position) : base(Position)
        {

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.spring;

            BasicAnimation = new Animation(Main.Object[(int)objectID], 8, 1, 0.05f);
            BasicAnimation.Start();

        }

        public override void Update(GameTime gameTime)
        {

            BasicAnimation.Update(gameTime);

            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Actor player = Handler.playersV2[i];

                if (GetRectangle().Intersects(player.GetRectangle()))
                    if(BasicAnimation.GetFrame() >= 6)
                    {
                        BasicAnimation.Reset();
                        BasicAnimation.Start();
                    }

            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            BasicAnimation.Draw(spriteBatch, Position);
            if (BasicAnimation.GetFrame() == BasicAnimation.GetSourceRectangle().Count - 1)
                BasicAnimation.Stop();

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X + 1, (int)Position.Y + 6, 14, 10);
        }

    }
}
