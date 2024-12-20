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
    public class Stone : ObjectV2
    {

        private StoneVariante variante;


        public Stone(Vector2 Position, StoneVariante variante) : base(Position)
        {

            this.variante = variante;

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.stone;

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            if(variante == StoneVariante.classique)
                spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(0, -23 + 1), new Rectangle(0, 0, 55, 23), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X, (int)Position.Y - 23, 55, 23);
        }

        public enum StoneVariante
        {

            classique = 0,

        };

    }
}
