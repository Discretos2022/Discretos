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
    public class Torch : ObjectV2
    {

        private Animation BasicAnimation;


        public Torch(Vector2 Position) : base(Position)
        {

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.torch;

            light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2 - 2), 1f, 60f, Color.White); // 50f
            LightManager.lights.Add(light);
            BasicAnimation = new Animation(Main.Object[(int)objectID], 4, 1, 0.1f);
            BasicAnimation.Start();

        }

        public override void Update(GameTime gameTime)
        {

            light.Position = new Vector2(GetRectangle().X + GetRectangle().Width / 2, GetRectangle().Y + GetRectangle().Height / 2);
            light.Radius = 60 + Random.Shared.Next(-4, 4);

            BasicAnimation.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            BasicAnimation.Draw(spriteBatch, Position);

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X + 3, (int)Position.Y, 10, 16);
        }

    }
}
