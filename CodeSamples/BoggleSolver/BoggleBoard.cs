using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleSolver
{
    public class BoggleBoard
    {
        public char[,] Board;
        public int BoardWidth;
        public int BoardHeight;
        public BoggleDictionary BoggleDictionary;

        public int Bottom
        {
            get
            {
                return BoardHeight-1;
            }
        }
        public int Top
        {
            get
            {
                return 0;
            }
        }
        public int Left
        {
            get
            {
                return 0;
            }
        }
        public int Right
        {
            get
            {
                return BoardWidth - 1;
            }
        }

        public BoggleBoard(char[,] board, int boardWidth, int boardHeight, BoggleDictionary boggleDictionary)
        {
            Board = board;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            BoggleDictionary = boggleDictionary;
        }

        public List<string> Solve()
        {
            Validate();

            List<string> foundWords = new List<string>();

            bool[,] used = new bool[BoardHeight,BoardWidth];
            for (int row = 0; row < BoardHeight; row++)
            {
                for(int col = 0; col < BoardWidth; col++)
                {
                    foundWords.AddRange(FindWords(string.Empty, row, col, BoggleDictionary.RootNode, used));
                }
            }

            return foundWords.Distinct().ToList();
        }

        private IEnumerable<string> FindWords(string baseWord, int row, int col, BoggleDictionaryNode parentDictionaryNode, bool[,] used)
        {
            List<string> foundWords = new List<string>();
            
            //check here just in case...
            //hopefully, we checked before calling this method in the first place
            if (!parentDictionaryNode.HasChildren)
            {
                return foundWords;
            }

            string word = baseWord + Board[row, col];
            used[row, col] = true;

            BoggleDictionaryNode childDictionaryNode;
            if(parentDictionaryNode.ChildNodes.TryGetValue(word.ToLower(), out childDictionaryNode))
            {
                //Boggle words must be at least 3 characters
                if(childDictionaryNode.IsWord && word.Length >= 3)
                {
                    foundWords.Add(word);
                }

                //don't bother looking any deeper if there are no more children
                if(childDictionaryNode.HasChildren)
                {
                    //try to move in each direction
                    //N
                    if(row > Top && !used[row-1,col])
                    {
                        foundWords.AddRange(FindWords(word, row - 1, col, childDictionaryNode, used));
                    }
                    //NE
                    if(row > Top && col < Right && !used[row-1,col+1])
                    {
                        foundWords.AddRange(FindWords(word, row - 1, col + 1, childDictionaryNode, used));
                    }
                    //E
                    if(col < Right && !used[row,col+1])
                    {
                        foundWords.AddRange(FindWords(word, row, col + 1, childDictionaryNode, used));
                    }
                    //SE
                    if(row < Bottom && col < Right && !used[row+1,col+1])
                    {
                        foundWords.AddRange(FindWords(word, row + 1, col + 1, childDictionaryNode, used));
                    }
                    //S
                    if(row < Bottom && !used[row+1,col])
                    {
                        foundWords.AddRange(FindWords(word, row + 1, col, childDictionaryNode, used));
                    }
                    //SW
                    if(row < Bottom && col > Left && !used[row+1,col-1])
                    {
                        foundWords.AddRange(FindWords(word, row + 1, col - 1, childDictionaryNode, used));
                    }
                    //W
                    if(col > Left && !used[row,col-1])
                    {
                        foundWords.AddRange(FindWords(word, row, col - 1, childDictionaryNode, used));
                    }
                    //NW
                    if(row > Top && col > Left && !used[row-1,col-1])
                    {
                        foundWords.AddRange(FindWords(word, row - 1, col - 1, childDictionaryNode, used));
                    }
                }
            }

            //reset the used flag before returning
            used[row, col] = false;
            return foundWords;
        }

        private void Validate()
        {
            //make sure we have a valid board
            if(BoardWidth < 1 || BoardHeight < 1)
            {
                throw new InvalidBoardException("BoardWidth and BoardHeight must be greater than 0.");
            }

            //make sure the array is at least large enough on both dimensions
            if(Board.GetLength(0) < BoardHeight || Board.GetLength(1) < BoardWidth)
            {
                throw new InvalidBoardException("Board[,] allocation is not large enough for the specified dimensions.");
            }

            //make sure the board only contains letters
            for(int row = 0; row < BoardHeight; row++)
            {
                for(int col =0; col < BoardWidth; col++)
                {
                    if(!Char.IsLetter(Board[row,col]))
                    {
                        throw new InvalidBoardException("The Board can only contain letters.");
                    }
                }
            }

            //make sure the dictionary is not null and at has something in it
            if(BoggleDictionary == null)
            {
                throw new InvalidDictionaryException("BoggleDictionary is null.");
            }
            if(BoggleDictionary.RootNode == null || !BoggleDictionary.RootNode.HasChildren)
            {
                throw new InvalidDictionaryException("BoggleDictionary is empty.");
            }
        }


    }
}
