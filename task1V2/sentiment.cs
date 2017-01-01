using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1V2
{
    public class sentiment
    {
        // Array of pos and neg words
        string[] positiveWords;
        string[] negativeWords;

        List<string> posWords;
        List<string> negWords;

        public sentiment()
        {
            // Reads files and stores each line in the array
            positiveWords = System.IO.File.ReadAllLines("positive-words.txt");
            negativeWords = System.IO.File.ReadAllLines("negative-words.txt");
        }

        // Adds each word to their respective list
        public void initiate()
        {
            posWords = new List<string>();
            negWords = new List<string>();

            foreach(string word in positiveWords)
            {
                posWords.Add(word);
            }

            foreach(string word in negativeWords)
            {
                negWords.Add(word);
            }
        }

        // Returns a float which is the sentement requires imput of a list of sentances
        public float analyseSentences(List<sentence> sentences)
        {
            float posWordCount = 0;
            float negWordCount = 0;
            float neutWordCount = 0;
            float totalWordCount = 0;

            // For each sentance it adds each word to a list
            foreach(sentence s in sentences)
            {
                string[] words = s.getSentence().Split(' ');

                // For each word it then compairs them to the lists and checks if pos or neg or neutral
                foreach(string word in words)
                {
                    totalWordCount++;
                    if (posWords.Contains(word)) { posWordCount++; }
                    else if (negWords.Contains(word)) { negWordCount++; }
                    else { neutWordCount++; }
                }

            }

            Console.WriteLine("\nPositive Words: {0}, Negative Words: {1}, Neutral Words: {2}, Total Words: {3}", posWordCount.ToString(), negWordCount.ToString(), neutWordCount.ToString(), totalWordCount.ToString());

            // Calculation to return the persentage positive of the text
            float perPos = posWordCount / totalWordCount;
            float perNeg = negWordCount / totalWordCount;

            //Console.WriteLine("PerPos: {0}, perNeg: {1}", perPos.ToString("n3"), perNeg.ToString("n3"));

            float accPerPos = perPos - perNeg;
            //Console.WriteLine("accPerPos: {0}", accPerPos.ToString("n3"));
            float totalPerPos = ((accPerPos * 0.5f) + 0.5f) * 100f;

            //Console.WriteLine("totalPerPos: {0}", totalPerPos.ToString("n3"));

            return totalPerPos;
        }



    }
}
