using System.Collections.Generic;
using System.Drawing;

namespace ConsoleWordle
{
    internal class Program
    {

        enum LetterStatus
        {
            Correct,
            WrongPlace,
            Incorrect,
        }


        static void Main(string[] args)
        {
            Random _rng = new Random();

            List<String> wordList = LoadWords();

            String mysteryWord = wordList[_rng.Next(wordList.Count)];
            string currentWord = "";
            int wordGuessCount = 0;
            int totalGuessCount = 0;
            int wordCount = 1;
            float avgGuessCount = 0;


            List<String> AIWordList = LoadWords();
            LetterStatus[] AIStatusTracker;
            



            while (wordCount < 10)  //change this to 100 when ready to flex your AI. 10 is just for testing.
            {
                //**AI Note: Leave this alone. 
                wordGuessCount++;



                //AI START
                //I believe most of your AI code could go here. The AI here needs to intelligently choose the next word to guess. Use AIWordList to help you. 
                currentWord = GetWord("What is your guess: ");
                



                //AI FINISH



                //**AI Note: Leave this alone. It checks to see if the word you guessed is a valid word.
                if (wordList.Contains(currentWord) == false)
                {
                    Console.WriteLine("Not a valid word, try again.");
                    wordGuessCount--;
                    continue;
                }
                

                //**AI Note: You can use AIStatusTracker to "see" what's right and what's wrong in your guessed word before checking your next word.
                AIStatusTracker = CheckWord(mysteryWord, currentWord);
                

                //**AI Note: Leave this alone. It tells you when you found the word.
                if (mysteryWord.Equals(currentWord))
                {
                    totalGuessCount += wordGuessCount;
                    avgGuessCount = (float)totalGuessCount / wordCount;
                    Console.WriteLine("Congrats, you got it in " + wordGuessCount + "! Avg guess count = " + avgGuessCount + " Resetting the mystery word.\n");
                    mysteryWord = wordList[_rng.Next(wordList.Count)];
                    AIWordList = LoadWords();
                    wordCount++;
                    wordGuessCount = 0;
                }


            }

        }


        //Check the guessed word against the myster word.
        //Don't change this without asking me first.
        static LetterStatus[] CheckWord(String word, String guess)
        {
            char[] wordChars = word.ToCharArray();
            char[] guessChars = guess.ToCharArray();

            LetterStatus[] statusTracker = new LetterStatus[5];
            statusTracker[0] = LetterStatus.Incorrect;
            statusTracker[1] = LetterStatus.Incorrect;
            statusTracker[2] = LetterStatus.Incorrect;
            statusTracker[3] = LetterStatus.Incorrect;
            statusTracker[4] = LetterStatus.Incorrect;

            List<char> letterCounter = word.ToList();
 
            //green letters - right letter, right place
            for (int i = 0; i < guessChars.Length; i++) 
            {
                for (int j = 0; j < wordChars.Length; j++ )
                {
                    if (guessChars[i] == wordChars[j] && i == j)
                    { 
                        statusTracker[i] = LetterStatus.Correct;
                        letterCounter.Remove(guessChars[i]);
                        break; 
                    }
                }

            }

            //yellow letters - right letter wrong place
            for (int i = 0; i < guessChars.Length; i++)
            {
                for (int j = 0; j < wordChars.Length; j++)
                {
                    if (statusTracker[i] == LetterStatus.Correct)
                        break;

                    if (guessChars[i] == wordChars[j] && i != j && statusTracker[j] != LetterStatus.Correct)
                    {
                        //count occurences of that letter, only flag yellow if flags < actual occurences
                        if (letterCounter.Contains(guessChars[i]))
                        {
                            statusTracker[i] = LetterStatus.WrongPlace;
                            letterCounter.Remove(guessChars[i]);
                        }
                    }
                }

            }

            for (int i = 0; i < guessChars.Length; i++)
            {
                if(statusTracker[i] == LetterStatus.Correct)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (statusTracker[i] == LetterStatus.WrongPlace)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(guessChars[i]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();

            return statusTracker;
        }


        //This method loads the words from file. Don't change
        //this without asking me first.
        static List<string> LoadWords()
        {
            List<string> wordList = new List<string>();
            string[] lines = File.ReadAllLines("../../../words.txt");
            foreach (string line in lines)
            {
                if (line.Length == 5)
                {
                    wordList.Add(line.ToLower());
                }
            }
            return wordList;
        }



        //This is the original humamn player GetWord method.
        //You can write an AI version, if it's helpful for
        //you.
        static string GetWord(String prompt)
        {
            Console.Write(prompt);
            string inputWord = Console.ReadLine();
            while (inputWord.Length != 5)
            {
                Console.Write("Word must be 5 letters. Try again:");
                inputWord = Console.ReadLine();
            }
            return inputWord.ToLower();
        }
    }
}
