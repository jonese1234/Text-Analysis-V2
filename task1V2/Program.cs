using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task1V2
{
    class Program
    {
        
        // Main function that runs the program.
        static void Main(string[] args)
        {
            bool moreInput = true;
            while (moreInput)
            {
                int choice = getUserChoice();

                // Selects which option to run.
                switch (choice)
                {
                    case 1:
                        option1();
                        break;
                    case 2:
                        option2();
                        break;
                }

                // Allows for more text analisis with out closing program.
                bool correctImput = false;
                while (!correctImput)
                {
                    Console.WriteLine("Do you want to do more text analasis? (y/n): ");
                    string imput = Console.ReadLine();

                    if (imput == "y" || imput == "Y")
                    {
                        moreInput = true;
                        correctImput = true;
                        Console.Clear();
                    }
                    else if (imput == "n" || imput == "N")
                    {
                        moreInput = false;
                        correctImput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid imput try again.");
                    }
                }
            }
        }


        // Gets the user choice as 1 or 2, which corresponds to via keyboard or text file respectively
        private static int getUserChoice()
        {

            bool goodImput = false;
            int option = 0;

            while (!goodImput)
            {
                // Trys to get an int value for the option
                try
                {
                    Console.WriteLine("1. Do you want to enter the text via the keyboard?");
                    Console.WriteLine("2. Do you want to read in the text from a file?");
                    option = Convert.ToInt32(Console.ReadLine());

                    if (option == 1 || option == 2)
                    {
                        goodImput = true;
                    }
                    // IF its not 1 or 2
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Bad Imput Try again");
                    }
                }
                // Catch for when not an int
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Bad Imput Try again");
                }
            }

            // Returns the option typed
            return option;
        }

        // Option 1, Imput via keyboard
        public static void option1()
        {
            // clears console and explains what to do
            Console.Clear();
            Console.WriteLine("Option 1 Selected! Enter your sentences 1 by 1, Press enter to add sentance.");
            Console.WriteLine("The last sentence must end with a '*' :");

            bool lastSentance = false;

            // Creates a new list of the class sentance
            List<sentence> sentanceList = new List<sentence>();

            // While not the last sentence
            while (!lastSentance)
            {
                // Creates a new sentance and reads from what is typed into the console
                sentence newSentance = new sentence(Console.ReadLine());

                // Checks if the sentance is empty
                if (newSentance.getSentence().Length == 0)
                {
                    Console.WriteLine("That Sentance was Empty, Sentance has been ignored.");
                }

                else
                {
                    // Checks if the last charecter is a *
                    if (newSentance.getSentence().Substring(newSentance.getSentence().Length - 1) == "*")
                    {
                        lastSentance = true;
                        // this changes the sentence to the sentence with out the *
                        newSentance.setSentance(newSentance.getSentence().Remove(newSentance.getSentence().Length - 1, 1));

                    }

                    // Adds the sentence to the list
                    sentanceList.Add(newSentance);
                }
            }
            Console.WriteLine("\nAll sentances:\n");
            foreach (sentence s in sentanceList)
            {
                Console.WriteLine(s.getSentence());
            }
            Console.WriteLine("");

            // Runs both the text analasis and sentiment analysis
            listAnalasis(sentanceList);
            analysisSentiment(sentanceList);
            Console.ReadLine();


        }

        public static void option2()
        {
            // Clears console and sets up a list of the class sentance
            Console.Clear();
            Console.WriteLine("Option 2 Selected!");
            List<sentence> sentanceList = new List<sentence>();

            
            bool validFileName = false;
            // runs if the file name is valid
            while (!validFileName)
            {

                try
                {
                    // Allows user to enter the text file to read, i.e "test.txt"
                    Console.Write("Enter Text file to read (i.e \"test.txt\"): ");
                    string fileName = Console.ReadLine();

                    // Reads each line of the text file and stores it in an array called textLines
                    string[] textLines = System.IO.File.ReadAllLines(fileName);

                    // For each line in the textlines it splits them in to individual sentences 
                    foreach (string line in textLines)
                    {
                        // make sures the line contains something
                        if (line.Length > 0)
                        {

                            string[] split = Regex.Split(line, @"(?<=[\.!\?])\s+");
                            // adds each sentance to the list
                            foreach (string accLine in split) { sentanceList.Add(new sentence(accLine)); }
                        }
                    }
                    Console.WriteLine("\nAll sentances:\n");
                    foreach (sentence s in sentanceList)
                    {
                        Console.WriteLine(s.getSentence());
                    }
                    Console.WriteLine("");

                    // Does longword analasis and text analasis and sentiment analasis
                    Console.WriteLine("\tRESULTS");
                    listAnalasis(sentanceList);
                    longwordAnalasis(sentanceList, fileName);
                    analysisSentiment(sentanceList);
                    validFileName = true;
                    Console.ReadLine();

                }
                // will repeat if file name is not correct
                catch { Console.WriteLine("This is not a valid file name, try again."); }
            }

            
        }

        // The sentiment analysis section
        public static void analysisSentiment(List<sentence> sentanceList)
        {
            // Creats a new sentament analyser
            sentiment analyser = new sentiment();
            // Initalises the loading of the arrays/list of positive and negative words
            analyser.initiate();
            // returns the score as a %
            float score = analyser.analyseSentences(sentanceList);
            // Prints the score to 3dp
            Console.WriteLine("The Sentiment of the text was determined as {0}% positive.", score.ToString("n3"));

        }

        // Longword analasis requires the list of sentances and the filename
        public static void longwordAnalasis(List<sentence> sentanceList, string fileName)
        {
            // Defines the length of words
            const int longWordLength = 7;

            // String list of long words
            List<string> longWords = new List<string>();

            // For each sentance in the sentance list it splits at a space, fullstop, ect.
            foreach (sentence sentance in sentanceList)
            {
                var result = sentance.getSentence().Split(' ', '.', ',', '"', '/').Where(x => x.Length >= longWordLength);
                foreach (string word in result)
                {
                    // If the word is not already in the list then add it, and make all the letters lowercase
                    if (!longWords.Contains(word.ToLower()))
                        longWords.Add(word.ToLower());
                }
            }

            // Sorts alphabeticaly
            longWords.Sort();
            // This makes the filename is the filename with out the .txt (i.e "test.txt" -> "test")
            fileName = fileName.Substring(0, fileName.LastIndexOf("."));
            Console.WriteLine("\nLong words: ");
            foreach(string word in longWords)
            {
                Console.WriteLine("\t{0}", word);
            }
            // Writes all the long words in the list to the filename + _longWords one per line (i.e "test" -> "test_longWords.txt")
            System.IO.File.WriteAllLines(fileName + "_longWords.txt", longWords);
        }


        // This Analasis returns the sentance count, word count, Uppercase count, lowercase count, vowel count, constanct count and the frequence of the letters.
        public static void listAnalasis(List<sentence> sentanceList)
        {
            // Setting Count variables
            int sentanceCount = sentanceList.Count();
            int wordCount = 0;
            int uppercaseCount = 0;
            int lowercaseCount = 0;
            int vowelCount = 0;
            int consonantCount = 0;
            // Dictonary for letter count (Letter, Count), i.e (d, 6)
            Dictionary<char, int> letterCount = new Dictionary<char, int>();


            for (int i = 0; i < sentanceCount; i++)
            {
                // Gets sentance from list
                string currentSentance = sentanceList[i].getSentence();
                string sentatanceUpper = currentSentance.ToUpper();

                // An array of the vowels
                char[] vowel = { 'a', 'e', 'i', 'o', 'u' };

                // for each letter in the Sentance ( Uses the upper to make all the same )
                foreach (var alphabet in sentatanceUpper)
                {
                    // Checks if it is a letter
                    if (Char.IsLetter(alphabet))
                    {
                        // If letter is in the letterCount dictonary already then add 1 to the count for it.
                        if (letterCount.ContainsKey(alphabet))
                            letterCount[alphabet] = letterCount[alphabet] + 1;
                        // If its not then add it to dictionary
                        else
                            letterCount.Add(alphabet, 1);
                    }
                }

                // For each sentance do
                for (int x = 0; x < currentSentance.Length; x++)
                {
                    // add to word count if there is a space or its the last on the sentance.
                    //Console.WriteLine(currentSentance[x]);
                    if (currentSentance[x] == ' ' || x == (currentSentance.Length - 1))
                    {
                        wordCount++;
                    }
                    // Check if letter
                    else if (Char.IsLetter(currentSentance[x]))
                    {
                        // If lower add to lowercase count
                        if (Char.IsLower(currentSentance[x]))
                        {
                            lowercaseCount++;
                        }
                        // If upper then add to uppercase Count
                        else
                        {
                            uppercaseCount++;
                        }
                        // if letter is in the vowel array then add to vowel count
                        if (vowel.Contains(currentSentance.ToLower()[x]))
                        {
                            vowelCount++;
                        }
                        // If not then add to consonant count
                        else
                        {
                            consonantCount++;
                        }

                    }
                }
            }

            // Prints all the results
            Console.WriteLine("Sentance Count: {0}", sentanceCount);
            Console.WriteLine("Word Count: {0}", wordCount);
            Console.WriteLine("Vowel Count: {0}", vowelCount);
            Console.WriteLine("Consonant Count: {0}", consonantCount);
            Console.WriteLine("Uppercase Count: {0}", uppercaseCount);
            Console.WriteLine("Lowercase Count: {0}", lowercaseCount);

            Console.WriteLine("\nLetter Frequency:");
            // Sorts by decending order
            letterCount = letterCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (var item in letterCount)
                Console.WriteLine("\tThe letter '" + item.Key + "' appeared " + item.Value + " times.");
        }
    }
}

