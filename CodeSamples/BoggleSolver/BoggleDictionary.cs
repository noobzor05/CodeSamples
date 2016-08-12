using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BoggleSolver
{
    public class BoggleDictionary
    {
        internal BoggleDictionaryNode RootNode { get; set; }
        
        public BoggleDictionary()
        {
            RootNode = new BoggleDictionaryNode(string.Empty);
        }

        //the isAlphabeticOrder flag could help optimize the load by reducing the number
        //of nodes searched if the words loaded are already in alphabetic order
        public void Load(string filePath, bool isAlphabeticOrder)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("filePath cannot be empty.", "filePath");
            }

            //the dictionary is finite and not all that big
            //it shouldn't be an issue to read the whole file into memory
            //if we expected large files, or a theoretically infinite number of words,
            //then we should do this differently
            Load(File.ReadAllLines(filePath), isAlphabeticOrder);
        }

        public void Load(string[] words, bool isAlphabetic)
        {
            if(words == null || words.Length < 1)
            {
                return;
            }

            BoggleDictionaryNode currentNode = RootNode;
            foreach(string word in words)
            {
                //lower-case everything
                BoggleDictionaryNode wordNode = currentNode.AddWord(word.ToLower());

                //if we know that the words are being loaded in alphabetic order,
                //it is likely that a large proportion of the words will either be
                //children of the last word's node or a sibling of a close ancestor
                //node.  because the load will start trying to load into the current node
                //and go up or down the hierarchy until it find the right spot, we are
                //better off starting out as close to where the word is going to live as possible.
                //this is counter-productive, though, if the words are in random order, 
                //because in that case we would usually go all the way back up to the root
                //node anyway.
                if(isAlphabetic)
                {
                    //if for some reason the word couldn't be loaded, just go back to the root
                    currentNode = (wordNode ?? RootNode);
                }
            }
        }
    }
}
