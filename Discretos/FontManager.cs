using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9
{
    public static class FontManager
    {

        public static SpriteFont UltimateFont = null;
        public static Texture2D SuperFont;

        public static SpriteFont ScoreFont = null;
        public static Texture2D ScoreF;

        private static List<Rectangle> glyphRect = new List<Rectangle>();
        private static List<Rectangle> croppingList = new List<Rectangle>();
        private static List<char> charList = new List<char>();
        private static List<Vector3> Vector3List = new List<Vector3>();


        public static SpriteFont InitFont(Font font, Texture2D tex)
        {

            switch (font)
            {

                case Font.ScoreFont:

                    ScoreF = tex;

                    #region Font Constructor

                    glyphRect.Add(new Rectangle(0, 0, 5, 8));    //0
                    glyphRect.Add(new Rectangle(6, 0, 5, 8));    //1
                    glyphRect.Add(new Rectangle(12, 0, 5, 8));   //2
                    glyphRect.Add(new Rectangle(18, 0, 5, 8));   //3
                    glyphRect.Add(new Rectangle(24, 0, 5, 8));   //4
                    glyphRect.Add(new Rectangle(30, 0, 5, 8));   //5
                    glyphRect.Add(new Rectangle(36, 0, 5, 8));   //6
                    glyphRect.Add(new Rectangle(42, 0, 5, 8));   //7
                    glyphRect.Add(new Rectangle(48, 0, 5, 8));   //8
                    glyphRect.Add(new Rectangle(54, 0, 5, 8));   //9
                    
                    glyphRect.Add(new Rectangle(0, 0, 5, 8));    //...


                    charList.Add('0');
                    charList.Add('1');
                    charList.Add('2');
                    charList.Add('3');
                    charList.Add('4');
                    charList.Add('5');
                    charList.Add('6');
                    charList.Add('7');
                    charList.Add('8');
                    charList.Add('9');

                    charList.Add('§');

                    int numberCaractere = charList.Count;


                    /// NE CHANGE RIEN
                    for (int i = 0; i < numberCaractere; i++)
                        croppingList.Add(new Rectangle(0, 0, 16, 16));


                    Vector3List.Add(new Vector3(0, -4, 0));//0
                    Vector3List.Add(new Vector3(0, -4, 0));//1
                    Vector3List.Add(new Vector3(0, -4, 0));//2
                    Vector3List.Add(new Vector3(0, -4, 0));//3
                    Vector3List.Add(new Vector3(0, -4, 0));//4
                    Vector3List.Add(new Vector3(0, -4, 0));//5
                    Vector3List.Add(new Vector3(0, -4, 0));//6
                    Vector3List.Add(new Vector3(0, -4, 0));//7
                    Vector3List.Add(new Vector3(0, -4, 0));//8
                    Vector3List.Add(new Vector3(0, -4, 0));//9


                    Vector3List.Add(new Vector3(0, -4, 0));//...
                    /// NE CHANGE RIEN


                    ScoreFont = new SpriteFont(ScoreF, glyphRect, croppingList, charList, 1, 12f, Vector3List, '§');


                    for (int i = 0; i < numberCaractere; i++)
                    {
                        glyphRect.Remove(glyphRect[0]);
                        charList.Remove(charList[0]);
                        croppingList.Remove(croppingList[0]);
                        Vector3List.Remove(Vector3List[0]);
                    }

                    #endregion

                    return ScoreFont;

                    
            }

            return null;

        }


        public enum Font
        {

            UltimateFont = 1,
            ScoreFont = 2,

        } 


    }



}
