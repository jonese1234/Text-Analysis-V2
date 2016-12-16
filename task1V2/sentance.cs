using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1V2
{
    public class sentence
    {
        // String for the sentance
        private string newSentence = "";

        // turns a string to the sentance
        public sentence(string sentenceContent)
        {
            this.newSentence = sentenceContent;

        }
        // This retruns the sentace as a string
        public string getSentence()
        {
            return this.newSentence;
        }
        // This allows the edting of the sentance
        public void setSentance(string sentenceContent)
        {
            this.newSentence = sentenceContent;
        }

    }
}

