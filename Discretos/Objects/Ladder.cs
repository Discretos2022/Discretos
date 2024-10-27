using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Objects
{
    public class Ladder : ObjectV2
    {

        public LadderVariante variante;
        public int ladderHeight;

        public Ladder(Vector2 Position, LadderVariante variante = LadderVariante.wood, int ladderHeight = 0) : base(Position)
        {

            this.variante = variante;
            this.ladderHeight = ladderHeight;

            this.Position.Y -= ladderHeight * 16;

            Init();

        }

        public override void Init()
        {
            if (variante == LadderVariante.wood) objectID = ObjectID.wood_ladder;
            if (variante == LadderVariante.wood_snow) objectID = ObjectID.wood_ladder_snow;
            hitbox.isEnabled = true;
        }

        public override void Update(GameTime gameTime)
        {
            


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            for (int i = 1; i <= ladderHeight; i++)
            {
                if (i == 1)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(2, -2), new Rectangle(0, 0, 18, 20), Color.White);
                else if (i == ladderHeight)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(2, 16 * (i - 1) - 2), new Rectangle(36, 0, 18, 20), Color.White);
                else
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(2, 16 * (i - 1) - 2), new Rectangle(18, 0, 18, 20), Color.White);
            }

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X + 4, (int)Position.Y, 14, 16 * ladderHeight);
        }


        public enum LadderVariante
        {

            wood = 1,
            wood_snow = 2,

        }

    }
}
