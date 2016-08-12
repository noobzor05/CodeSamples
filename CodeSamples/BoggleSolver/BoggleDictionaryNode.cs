using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleSolver
{
    //this is not technically thread-safe, but for our use, we will only be adding words
    //on a single thread.  it is safe to do concurrent reads/lookups in the dictionaries as long
    //as we are not adding anything at the same time
    class BoggleDictionaryNode
    {
        public string BaseString { get; private set; }
        public int BaseLength
        {
            get
            {
                return (BaseString == null ? 0 : BaseString.Length);
            }
        }
        public bool IsWord { get; private set; }
        public bool HasChildren
        { 
            get
            {
                return (_ChildNodes != null && _ChildNodes.Any());
            }
        }

        private Dictionary<string, BoggleDictionaryNode> _ChildNodes;
        public Dictionary<string, BoggleDictionaryNode> ChildNodes
        {
            get
            {
                if(_ChildNodes == null)
                {
                    //lazy-init so that we don't allocate the dictionary unless we actually need it
                    _ChildNodes = new Dictionary<string, BoggleDictionaryNode>();
                }
                return _ChildNodes;
            }
        }

        public BoggleDictionaryNode ParentNode { get; internal set; }

        private BoggleDictionaryNode()
        {
        }

        public BoggleDictionaryNode(string baseString)
        {
            BaseString = baseString;
        }

        //returning the node representing the word being added could help in
        //optimizing the load of the initial dictionary if we know that the
        //words are being loaded in alphabetical order
        public BoggleDictionaryNode AddWord(string word)
        {
            if(word == null)
            {
                //word cannot be represented
                return null;
            }

            if(!word.StartsWith(this.BaseString))
            {
                //word is not represent by this node and cannot be a child of this node
                //so, try the parent node, if there is one
                if(ParentNode != null)
                {
                    return ParentNode.AddWord(word);
                }

                //or just give up
                return null;
            }

            if (BaseString == word)
            {
                //word is represented by this node
                IsWord = true;
                return this;
            }

            string newBaseString = word.Substring(0, BaseLength + 1);
            
            BoggleDictionaryNode newChild;
            if(!ChildNodes.TryGetValue(newBaseString, out newChild))
            {
                newChild = new BoggleDictionaryNode(newBaseString);
                newChild.ParentNode = this;
                ChildNodes.Add(newBaseString, newChild);
            }

            return newChild.AddWord(word);
        }
    }
}
