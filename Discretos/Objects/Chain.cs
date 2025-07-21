using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Objects
{
    public class Chain : ObjectV2
    {

        public DownType downType;
        public int chainHeight;

        public Chain(Vector2 Position, DownType _downType = DownType.freeLink, int chainHeight = 0) : base(Position)
        {

            this.downType = _downType;
            this.chainHeight = chainHeight;

            Init();

        }

        public override void Init()
        {
            objectID = ObjectID.chain;
            hitbox.isEnabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            for (int i = 0; i < chainHeight; i++)
            {
                if (i == 0)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position, new Rectangle(16, 0, 16, 16), Color.White);
                else if (i == chainHeight - 1)
                {
                    if (downType == DownType.freeLink)
                        spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(0, i * 16), new Rectangle(48, 0, 16, 16), Color.White);
                    else if(downType == DownType.fixedLink)
                        spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(0, i * 16), new Rectangle(32, 0, 16, 16), Color.White);
                }
                else
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(0, i * 16), new Rectangle(0, 0, 16, 20), Color.White);
            }

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X + 4, (int)Position.Y, 8, 16 * chainHeight);
        }


        public enum DownType
        {

            freeLink = 1,
            fixedLink = 2,

        }

    }
}
