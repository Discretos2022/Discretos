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

        private static int numberCaractere = 0;


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


                    ScoreFont = new SpriteFont(ScoreF, glyphRect, croppingList, charList, 1, 11f, Vector3List, '§');


                    for (int i = 0; i < numberCaractere; i++)
                    {
                        glyphRect.Remove(glyphRect[0]);
                        charList.Remove(charList[0]);
                        croppingList.Remove(croppingList[0]);
                        Vector3List.Remove(Vector3List[0]);
                    }

                    #endregion

                    return ScoreFont;


                case Font.UltimateFont:

                    SuperFont = tex;

                    #region Font Constructor

                    glyphRect.Add(new Rectangle(600, 0, 9, 16));   //SPACE
                    glyphRect.Add(new Rectangle(533, 0, 1, 16));   //!
                    glyphRect.Add(new Rectangle(567, 0, 7, 16));   //*
                    glyphRect.Add(new Rectangle(575, 0, 7, 16));   //+
                    glyphRect.Add(new Rectangle(585, 0, 7, 16));   //-
                    glyphRect.Add(new Rectangle(583, 0, 1, 16));   //.

                    glyphRect.Add(new Rectangle(456, 0, 6, 16));   //0
                    glyphRect.Add(new Rectangle(463, 0, 5, 16));   //1
                    glyphRect.Add(new Rectangle(469, 0, 7, 16));   //2
                    glyphRect.Add(new Rectangle(477, 0, 6, 16));   //3
                    glyphRect.Add(new Rectangle(484, 0, 7, 16));   //4
                    glyphRect.Add(new Rectangle(492, 0, 7, 16));   //5
                    glyphRect.Add(new Rectangle(500, 0, 7, 16));   //6
                    glyphRect.Add(new Rectangle(508, 0, 7, 16));   //7
                    glyphRect.Add(new Rectangle(516, 0, 6, 16));   //8
                    glyphRect.Add(new Rectangle(523, 0, 6, 16));   //9

                    glyphRect.Add(new Rectangle(530, 0, 2, 16));   //:
                    glyphRect.Add(new Rectangle(535, 0, 7, 16));   //?

                    glyphRect.Add(new Rectangle(0, 0, 11, 16));    //A
                    glyphRect.Add(new Rectangle(12, 0, 9, 16));    //B
                    glyphRect.Add(new Rectangle(22, 0, 11, 16));   //C
                    glyphRect.Add(new Rectangle(34, 0, 10, 16));   //D
                    glyphRect.Add(new Rectangle(45, 0, 9, 16));    //E
                    glyphRect.Add(new Rectangle(55, 0, 9, 16));    //F
                    glyphRect.Add(new Rectangle(65, 0, 11, 16));   //G
                    glyphRect.Add(new Rectangle(77, 0, 9, 16));    //H
                    glyphRect.Add(new Rectangle(87, 0, 7, 16));    //I
                    glyphRect.Add(new Rectangle(95, 0, 10, 16));   //J
                    glyphRect.Add(new Rectangle(106, 0, 9, 16));   //K
                    glyphRect.Add(new Rectangle(116, 0, 8, 16));   //L
                    glyphRect.Add(new Rectangle(125, 0, 11, 16));  //M
                    glyphRect.Add(new Rectangle(137, 0, 9, 16));   //N
                    glyphRect.Add(new Rectangle(147, 0, 9, 16));   //O
                    glyphRect.Add(new Rectangle(157, 0, 9, 16));   //P
                    glyphRect.Add(new Rectangle(167, 0, 9, 16));   //Q
                    glyphRect.Add(new Rectangle(177, 0, 9, 16));   //R
                    glyphRect.Add(new Rectangle(187, 0, 9, 16));   //S
                    glyphRect.Add(new Rectangle(197, 0, 9, 16));   //T
                    glyphRect.Add(new Rectangle(207, 0, 9, 16));   //U
                    glyphRect.Add(new Rectangle(217, 0, 11, 16));  //V
                    glyphRect.Add(new Rectangle(229, 0, 15, 16));  //W
                    glyphRect.Add(new Rectangle(245, 0, 10, 16));  //X
                    glyphRect.Add(new Rectangle(256, 0, 11, 16));  //Y
                    glyphRect.Add(new Rectangle(268, 0, 9, 16));   //Z

                    glyphRect.Add(new Rectangle(278, 0, 6, 16));   //a
                    glyphRect.Add(new Rectangle(285, 0, 5, 16));   //b
                    glyphRect.Add(new Rectangle(291, 0, 6, 16));   //c
                    glyphRect.Add(new Rectangle(298, 0, 5, 16));   //d
                    glyphRect.Add(new Rectangle(304, 0, 5, 16));   //e
                    glyphRect.Add(new Rectangle(310, 0, 5, 16));   //f
                    glyphRect.Add(new Rectangle(316, 0, 6, 16));   //g
                    glyphRect.Add(new Rectangle(323, 0, 5, 16));   //h
                    glyphRect.Add(new Rectangle(329, 0, 5, 16));   //i
                    glyphRect.Add(new Rectangle(335, 0, 5, 16));   //j
                    glyphRect.Add(new Rectangle(341, 0, 5, 16));   //k
                    glyphRect.Add(new Rectangle(347, 0, 5, 16));   //l
                    glyphRect.Add(new Rectangle(352, 0, 7, 16));   //m
                    glyphRect.Add(new Rectangle(360, 0, 5, 16));   //n
                    glyphRect.Add(new Rectangle(366, 0, 7, 16));   //o
                    glyphRect.Add(new Rectangle(374, 0, 5, 16));   //p
                    glyphRect.Add(new Rectangle(380, 0, 7, 16));   //q
                    glyphRect.Add(new Rectangle(388, 0, 5, 16));   //r
                    glyphRect.Add(new Rectangle(394, 0, 6, 16));   //s
                    glyphRect.Add(new Rectangle(401, 0, 5, 16));   //t
                    glyphRect.Add(new Rectangle(407, 0, 7, 16));   //u
                    glyphRect.Add(new Rectangle(414, 0, 7, 16));   //v
                    glyphRect.Add(new Rectangle(422, 0, 11, 16));  //w
                    glyphRect.Add(new Rectangle(434, 0, 6, 16));   //x
                    glyphRect.Add(new Rectangle(441, 0, 7, 16));   //y
                    glyphRect.Add(new Rectangle(449, 0, 6, 16));   //z


                    glyphRect.Add(new Rectangle(600, 0, 8, 16));   //...


                    charList.Add(' ');
                    charList.Add('!');
                    charList.Add('*');
                    charList.Add('+');
                    charList.Add('-');
                    charList.Add('.');

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

                    charList.Add(':');
                    charList.Add('?');

                    charList.Add('A');
                    charList.Add('B');
                    charList.Add('C');
                    charList.Add('D');
                    charList.Add('E');
                    charList.Add('F');
                    charList.Add('G');
                    charList.Add('H');
                    charList.Add('I');
                    charList.Add('J');
                    charList.Add('K');
                    charList.Add('L');
                    charList.Add('M');
                    charList.Add('N');
                    charList.Add('O');
                    charList.Add('P');
                    charList.Add('Q');
                    charList.Add('R');
                    charList.Add('S');
                    charList.Add('T');
                    charList.Add('U');
                    charList.Add('V');
                    charList.Add('W');
                    charList.Add('X');
                    charList.Add('Y');
                    charList.Add('Z');

                    charList.Add('a');
                    charList.Add('b');
                    charList.Add('c');
                    charList.Add('d');
                    charList.Add('e');
                    charList.Add('f');
                    charList.Add('g');
                    charList.Add('h');
                    charList.Add('i');
                    charList.Add('j');
                    charList.Add('k');
                    charList.Add('l');
                    charList.Add('m');
                    charList.Add('n');
                    charList.Add('o');
                    charList.Add('p');
                    charList.Add('q');
                    charList.Add('r');
                    charList.Add('s');
                    charList.Add('t');
                    charList.Add('u');
                    charList.Add('v');
                    charList.Add('w');
                    charList.Add('x');
                    charList.Add('y');
                    charList.Add('z');

                    charList.Add('§');

                    numberCaractere = charList.Count;


                    /// NE CHANGE RIEN
                    for (int i = 0; i < numberCaractere; i++)
                        croppingList.Add(new Rectangle(0, 0, 16, 16));

                    Vector3List.Add(new Vector3(0, -4, 0));//SPACE
                    Vector3List.Add(new Vector3(0, -8, 0));//!
                    Vector3List.Add(new Vector3(0, -2, 0));//*
                    Vector3List.Add(new Vector3(0, -2, 0));//+
                    Vector3List.Add(new Vector3(0, -2, 0));//-
                    Vector3List.Add(new Vector3(0, -8, 0));//.

                    Vector3List.Add(new Vector3(0, -3, 0));//0
                    Vector3List.Add(new Vector3(0, -4, 0));//1
                    Vector3List.Add(new Vector3(0, -2, 0));//2
                    Vector3List.Add(new Vector3(0, -3, 0));//3
                    Vector3List.Add(new Vector3(0, -2, 0));//4
                    Vector3List.Add(new Vector3(0, -2, 0));//5
                    Vector3List.Add(new Vector3(0, -2, 0));//6
                    Vector3List.Add(new Vector3(0, -2, 0));//7
                    Vector3List.Add(new Vector3(0, -3, 0));//8
                    Vector3List.Add(new Vector3(0, -3, 0));//9

                    Vector3List.Add(new Vector3(0, -7, 0));//:
                    Vector3List.Add(new Vector3(0, -3, 0));//?

                    Vector3List.Add(new Vector3(0, 2, 0));//A
                    Vector3List.Add(new Vector3(0, 0, 0));//B
                    Vector3List.Add(new Vector3(0, 2, 0));//C
                    Vector3List.Add(new Vector3(0, 1, 0));//D
                    Vector3List.Add(new Vector3(0, 0, 0));//E
                    Vector3List.Add(new Vector3(0, 0, 0));//F
                    Vector3List.Add(new Vector3(0, 2, 0));//G
                    Vector3List.Add(new Vector3(0, 0, 0));//H
                    Vector3List.Add(new Vector3(0, -2, 0));//I
                    Vector3List.Add(new Vector3(0, 1, 0));//J
                    Vector3List.Add(new Vector3(0, 0, 0));//K
                    Vector3List.Add(new Vector3(0, -1, 0));//L
                    Vector3List.Add(new Vector3(0, 2, 0));//M
                    Vector3List.Add(new Vector3(0, 0, 0));//N
                    Vector3List.Add(new Vector3(0, 0, 0));//O
                    Vector3List.Add(new Vector3(0, 0, 0));//P
                    Vector3List.Add(new Vector3(0, 0, 0));//Q
                    Vector3List.Add(new Vector3(0, 0, 0));//R
                    Vector3List.Add(new Vector3(0, 0, 0));//S
                    Vector3List.Add(new Vector3(0, 0, 0));//T
                    Vector3List.Add(new Vector3(0, 0, 0));//U
                    Vector3List.Add(new Vector3(0, 2, 0));//V
                    Vector3List.Add(new Vector3(0, 6, 0));//W
                    Vector3List.Add(new Vector3(0, 1, 0));//X
                    Vector3List.Add(new Vector3(0, 2, 0));//Y
                    Vector3List.Add(new Vector3(0, 0, 0));//Z

                    Vector3List.Add(new Vector3(0, -3, 0));//a
                    Vector3List.Add(new Vector3(0, -4, 0));//b
                    Vector3List.Add(new Vector3(0, -3, 0));//c
                    Vector3List.Add(new Vector3(0, -4, 0));//d
                    Vector3List.Add(new Vector3(0, -4, 0));//e
                    Vector3List.Add(new Vector3(0, -4, 0));//f
                    Vector3List.Add(new Vector3(0, -3, 0));//g
                    Vector3List.Add(new Vector3(0, -4, 0));//h
                    Vector3List.Add(new Vector3(0, -4, 0));//i
                    Vector3List.Add(new Vector3(0, -4, 0));//j
                    Vector3List.Add(new Vector3(0, -4, 0));//k
                    Vector3List.Add(new Vector3(0, -5, 0));//l
                    Vector3List.Add(new Vector3(0, -2, 0));//m
                    Vector3List.Add(new Vector3(0, -4, 0));//n
                    Vector3List.Add(new Vector3(0, -2, 0));//o
                    Vector3List.Add(new Vector3(0, -4, 0));//p
                    Vector3List.Add(new Vector3(0, -2, 0));//q
                    Vector3List.Add(new Vector3(0, -4, 0));//r
                    Vector3List.Add(new Vector3(0, -3, 0));//s
                    Vector3List.Add(new Vector3(0, -4, 0));//t
                    Vector3List.Add(new Vector3(0, -3, 0));//u
                    Vector3List.Add(new Vector3(0, -2, 0));//v
                    Vector3List.Add(new Vector3(0, 2, 0));//w
                    Vector3List.Add(new Vector3(0, -3, 0));//x
                    Vector3List.Add(new Vector3(0, -2, 0));//y
                    Vector3List.Add(new Vector3(0, -3, 0));//z


                    Vector3List.Add(new Vector3(0, 0, 0));//...
                    /// NE CHANGE RIEN


                    UltimateFont = new SpriteFont(SuperFont, glyphRect, croppingList, charList, 0, 12f, Vector3List, '§');


                    for (int i = 0; i < numberCaractere; i++)
                    {
                        glyphRect.Remove(glyphRect[0]);
                        charList.Remove(charList[0]);
                        croppingList.Remove(croppingList[0]);
                        Vector3List.Remove(Vector3List[0]);
                    }

                    #endregion

                    return UltimateFont;


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
