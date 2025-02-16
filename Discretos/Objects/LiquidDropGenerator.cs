using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Objects
{
    class LiquidDropGenerator : ObjectV2
    {

        private Animation BasicAnimation;

        private int timer;
        private int baseTimer;

        private LiquidType type;

        public LiquidDropGenerator(Vector2 Position, int timer = 240, LiquidType type = LiquidType.Water) : base(Position)
        {


            this.timer = Random.Shared.Next(0, timer) * 2;
            this.baseTimer = timer * 2;
            this.type = type;

            Init();
        }

        public override void Init()
        {

            objectID = ObjectID.liquid_drop;

            if(type == LiquidType.Water)
                BasicAnimation = new Animation(Main.Object[(int)objectID], 9, 4, 0.05f); // 0.05f
            else if(type == LiquidType.Lava)
                BasicAnimation = new Animation(Main.Object[(int)objectID], 9, 4, 0.05f * 2, 3); // 0.05f

            BasicAnimation.Stop();

        }

        public override void Update(GameTime gameTime)
        {
            BasicAnimation.Update(gameTime);


            if(BasicAnimation.GetFrame() == BasicAnimation.GetSourceRectangle().Count - 1)
            {
                BasicAnimation.Stop();
                BasicAnimation.Reset();
                Handler.actors.Add(new LiquidDrop(Position, type));
            }


            if(timer == 0)
            {
                BasicAnimation.Reset();
                BasicAnimation.Start();
                timer = baseTimer;
            }
            timer--;

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            BasicAnimation.Draw(spriteBatch, Position);

        }

        public override void UpdateHitbox()
        {

        }

        public enum LiquidType
        {
            Water,
            Lava,
        };

    }
}
