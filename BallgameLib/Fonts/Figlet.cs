/*
Figlet Font related code taken from the wonderful Colorful.Console project
https://github.com/tomakita/Colorful.Console
 */
/*The MIT License (MIT)

Copyright (c) 2015 Tom Akita

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
//using System.Drawing;

namespace Ballgame
{
    public class Figlet
    {
        private readonly FigletFont font;

        public Figlet()
        {
            this.font = FigletFont.Default;
        }

        public Figlet(FigletFont font)
        {
            if (font == null) { throw new ArgumentNullException(nameof(font)); }

            this.font = font;
        }

        public StyledString ToAscii(string value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            if (Encoding.UTF8.GetByteCount(value) != value.Length) { throw new ArgumentException("String contains non-ascii characters"); }

            StringBuilder stringBuilder = new StringBuilder();

            int stringWidth = GetStringWidth(font, value);
            char[,] characterGeometry = new char[font.Height + 1, stringWidth];
            int[,] characterIndexGeometry = new int[font.Height + 1, stringWidth];
            //Color[,] colorGeometry = new Color[font.Height + 1, stringWidth];

            for (int line = 1; line <= font.Height; line++)
            {
                int runningWidthTotal = 0;

                for (int c = 0; c < value.Length; c++) 
                {
                    char character = value[c];
                    string fragment = GetCharacter(this.font, character, line);

                    stringBuilder.Append(fragment);
                    CalculateCharacterGeometries(fragment, c, runningWidthTotal, line, characterGeometry, characterIndexGeometry);

                    runningWidthTotal += fragment.Length;
                }

                stringBuilder.AppendLine();
            }

            StyledString styledString = new StyledString(value, stringBuilder.ToString());
            styledString.CharacterGeometry = characterGeometry;
            styledString.CharacterIndexGeometry = characterIndexGeometry;
            //styledString.ColorGeometry = colorGeometry;

            return styledString;
        }

        private static void CalculateCharacterGeometries(string fragment, int characterIndex, int runningWidthTotal, int line, char[,] charGeometry, int[,] indexGeometry)
        {
            for (int i = runningWidthTotal; i < runningWidthTotal + fragment.Length; i++)
            {
                charGeometry[line, i] = fragment[i - runningWidthTotal];
                indexGeometry[line, i] = characterIndex;
            }
        }

        private static int GetStringWidth(FigletFont font, string value)
        {
            List<int> charWidths = new List<int>();
            foreach (var character in value)
            {
                int charWidth = 0;
                for (int line = 1; line <= font.Height; line++)
                {
                    string figletCharacter = GetCharacter(font, character, line);

                    charWidth = figletCharacter.Length > charWidth ? figletCharacter.Length : charWidth;
                }

                charWidths.Add(charWidth);
            }

            return charWidths.Sum();
        }

        private static string GetCharacter(FigletFont font, char character, int line)
        {
            var start = font.CommentLines + ((Convert.ToInt32(character) - 32) * font.Height);
            var result = font.Lines[start + line];
            var lineEnding = result[result.Length - 1];
            result = Regex.Replace(result, @"\" + lineEnding + "{1,2}$", string.Empty);

            if (font.Kerning > 0)
            {
                result += new string(' ', font.Kerning);
            }

            return result.Replace(font.HardBlank, " ");
        }
    }
}