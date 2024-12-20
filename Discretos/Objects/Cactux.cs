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
    public class Cactux : ObjectV2
    {

        private CactuxVariante variante;


        public Cactux(Vector2 Position, CactuxVariante variante) : base(Position)
        {

            this.variante = variante;

            Init();
            
        }

        public override void Init()
        {

            objectID = ObjectID.cactux;

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);

            switch (variante)
            {

                case CactuxVariante.classique:
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(-5, -47), new Rectangle(0, 0, 27, 47), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    break;

                case CactuxVariante.little:
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(-5, -47), new Rectangle(28, 0, 27, 47), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    break;

            }

        }

        public override void UpdateHitbox()
        {
            switch (variante)
            {
                case CactuxVariante.classique:
                    hitbox.rectangle = new Rectangle((int)Position.X - 5, (int)Position.Y - 47, 27, 47);
                    break;

                case CactuxVariante.little:
                    hitbox.rectangle = new Rectangle((int)Position.X - 5, (int)Position.Y - 47, 27, 47);
                    break;

            }
            
        }

        public enum CactuxVariante
        {

            classique = 0,
            little = 1,

        };

    }
}
