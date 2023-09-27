//---PROJECT STEPS---
/*
 * Note:    Write all methods to be fully encapsulated - working via parameters rather than any external/"global" variables.
 * ---
 * Write:   initialisations (with default variable values for now. They can be customisable later)
 * Write:   game loop skeleton
 * Write:   user input loop for player turns
 * ---
 * Study:   2D arrays
 * Study:   StringBuilder
 * Write:   method to display game board
 * ---
 * Test:    prints out empty game board
 * Test:    accepts valid user inputs for token insertions (checks: max, min, datatype, sanitised) 
 * Test:    inserted tokens go to correct space
 * Test:    game board display updates correctly after player turn
 * Test:    inserted tokens "stack" within columns
 * Test:    token inputs are rejected on columns that are full
 * Study:   user input sanitisation in C#
 * ---
 * Write:   backstop to check when board is full (results in stalemate)
 * Test:    game results in stalemate when board is full (TIP: could reduce dimensions of board to speed testing)
 * ---
 * Write:   skeleton method to check whether game has been won
 * Write:   analyse whether a token placement has won the game
 * Write:   set win conditions after a connect-4 has happened
 * Write:   (edit game board display to add column numbers!)
 * Test:    analysis algorithm can acknowledge partial connected lines, but the game is not won unless they add up to 4.
 * Test:    game can be won with vertical connect-4 located "higher up" in a column 
 * Test:    game can be won with vertical connect-4 located at the bottom of a column (to make sure algorithm doesn't check "out-of-bounds")
 * Test:    game can be won with horizontal connect-4 located across centre of board
 * Test:    game can be won with horizontal connect-4 located near edge of board (to make sure algorithm doesn't check "out-of-bounds")
 * Test:    game can be won with diagonal AL-BR connect-4 located across centre of board
 * Test:    game can be won with diagonal AL-BR connect-4 located near edge of board (to make sure algorithm doesn't check "out-of-bounds")
 * Test:    game can be won with diagonal BL-AR connect-4 located across centre of board
 * Test:    game can be won with diagonal BL-AR connect-4 located near edge of board (to make sure algorithm doesn't check "out-of-bounds")
 * Test:    other win conditions are not overridden by stalemate condition where the board is full
 * ---
 * Lookup:  is it possible to display multicoloured text on the SAME LINE in C# console apps? (colour-code player tokens, or highlight the "winning line")
 * Write:   alternative method to display game board with colour-coded player tokens. 
 * Write:   change all "Player 1"/"Player 2" printouts to be highlighted with their respective player colour.
 * Test:    player colours display correctly in: player turn instruction messages, game board display, game end result messages
 * ---
 * Write:   "do you want to play again? Y/N"
 * Test:    user input is validated for the above prompt
 * Test:    game can be restarted after finishing (TIP: could reduce dimensions of board to speed testing)
 * ---
 * Write:   coin flip to determine first player turn
 * Test:    coin flip works, is random, and affects first turn 
 * ---
 * Write:   user input loops for pre-match "configuration" step <----------------------------------------------------------------------------------------------------------------------(HERE)
 * Test:    accepts valid user inputs for config (checks: max, min, datatype, sanitised)
 * Test:    custom game board dimensions can be set
 * Test:    game board displays correctly with a variety of configs
 * ---
 * Write:   after finishing game: choice to restart with same config, or re-do config step
 * Test:    game can be restarted back to start of game loop, carrying over same config
 * Test:    game can be restarted back to start of config step, letting us set new config
 * ---
 * DONE?
 * ---
 * Study:   C# unit tests, regression tests (may or may not apply here)
 * Study:   C# GUI programs (could remake this program as a GUI version?)
 * OR:      move on to other grad scheme learning materials, e.g. PluralSight
 */


//---TESTS TO DO---
/*
 * check EVERY else-scenario and error scenario
 * user input validations: maximums, minimums, character types, (parameterisation?)
 * max rows + max columns
 */



//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



using System;
using System.Text;

namespace Connect_4
{
    class Program
    {
        //(No static/"global" variables here because I wanted the Main method to be portable.)
        
        //Main method
        static void Main(string[] args)
        {
            bool userWantsToPlay = true; //this remains True until a user requests to exit the program

            while (userWantsToPlay)
            {
                //Variable declarations
                Random randomNumGen = new Random();
                int totalRows;
                int totalColumns;
                int winningLineCount;
                int[,] gameBoard;  //In each space: 0 = empty, 1 = player1, 2 = player2    //IDEA:(states could be stored as enumeration? Probably no reason for this though)====================
                bool gameComplete;
                int turnCount;
                int currentPlayer; //1 = player1, 2 = player2
                int fullColumns;
                int targetRow;
                int targetColumn;

                //User Configuration step
                totalRows = 6;
                totalColumns = 7;
                winningLineCount = 4;
                //TO DO: Make these customisable <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //TO DO: enforce minimum and maximum values e.g. 4-20 each?

                //Game initilisation step
                gameBoard = new int[totalRows, totalColumns]; //spaces will be initiliased to 0 by default         
                gameComplete = false;
                turnCount = 0;
                fullColumns = 0;
                targetRow = 0;
                targetColumn = 0;

                //Decide which player goes first
                currentPlayer = randomNumGen.Next(1,3);
                switch (currentPlayer)
                {
                    case 1:
                        Console.Write("Result of coin toss: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Player 1");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" will go first.\n");
                        Console.WriteLine("Press any key to begin.");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Write("Result of coin toss: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Player 2");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" will go first.\n");
                        Console.WriteLine("Press any key to begin.");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine($"ERROR: Random number generator returned the following value: {currentPlayer}");
                        Console.WriteLine("Press any key to exit the program.");
                        Console.ReadKey();
                        Environment.Exit(0);
                        break;
                }
                
                //Game loop
                while (gameComplete == false)
                {
                    turnCount++;

                    //Clear console and Display game board
                    Console.Clear();
                    Console.WriteLine($"Game board at the start of turn {turnCount}:");
                    Console.WriteLine("");
                    //Console.WriteLine(DisplayGameBoard(gameBoard));
                    DisplayGameBoardColour(gameBoard);
                    Console.WriteLine("");

                    //Solicit player's token placement
                    switch (currentPlayer)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Player 1");
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Player 2");
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("UNKNOWN PLAYER");
                            break;
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($": please enter a column number (1-{totalColumns}) to place your token into.\n");
                    //Console.WriteLine($"Player {currentPlayer}: please enter a column number (1-{totalColumns}) to place your token into.");

                    bool tokenPlaced = false;
                    bool validInput;
                    while (tokenPlaced == false)
                    {
                        targetRow = 0;
                        targetColumn = 0;
                        validInput = false;
                        string inputString = Console.ReadLine();

                        //validate user input by checking it's an integer within the available range of column numbers
                        if (int.TryParse(inputString, out targetColumn))
                        {
                            if (targetColumn >= 1 && targetColumn <= totalColumns)
                            {
                                validInput = true;
                            }
                            else //input is not a valid column
                            {
                                Console.WriteLine("Invalid input (too high or too low) - Please try another.");
                            }
                        }
                        else //input is not an integer
                        {
                            Console.WriteLine("Invalid input (not an integer) - Please try another.");
                        }

                        if (validInput)
                        {
                            //look for open space in the chosen column, and place token in the lowest one available                          
                            //traverse column upwards from bottom to top 
                            for (targetRow = totalRows; targetRow > 0; targetRow--)
                            {
                                //check whether current space is empty
                                if (gameBoard[(targetRow - 1), (targetColumn - 1)] == 0) //(deduct 1 from the target row/column values so they match array indexing)
                                {
                                    //place the player's token
                                    gameBoard[(targetRow - 1), (targetColumn - 1)] = currentPlayer;
                                    tokenPlaced = true;
                                    //With token now placed, check whether the player has won
                                    if (turnCount >= ((2 * winningLineCount) - 1)) //(because it's not possible for either player to win on any earlier turn)
                                    {
                                        gameComplete = hasPlayerWon(gameBoard, (targetRow - 1), (targetColumn - 1), currentPlayer, winningLineCount);
                                    }
                                    break; //for-loop terminates here "for(targetRow = totalRows; targetRow > 0; targetRow--)"
                                }
                            }
                            //if we get to this point and tokenPlaced is still false, then it means the column was full
                            if (tokenPlaced == false)
                            {
                                Console.WriteLine("Invalid input (column is full) - Please try another.");
                            }
                        }
                    } //end of player input loop "while (tokenPlaced == false)"
                    Console.WriteLine(""); //add blank line after input is recieved

                    //If the game still hasn't ended ...
                    if (gameComplete == false)
                    {
                        //Work out whose turn it is next
                        if (currentPlayer == 1)
                            currentPlayer = 2;
                        else
                            currentPlayer = 1;

                        //if the last-placed token filled the top space of its column: mark the column as full.
                        if ((tokenPlaced == true) && (targetRow == 1))
                            fullColumns++;

                        //If gameBoard has no more empty spaces left, end game with stalemate condition      
                        if (fullColumns == totalColumns)
                        {
                            currentPlayer = 0;
                            gameComplete = true;
                        }
                    }
                } //end of game loop "while (gameComplete == false)"

                //End of Game step
                Console.Clear();
                Console.WriteLine($"Final state of game board:");
                Console.WriteLine("");
                //Console.WriteLine(DisplayGameBoard(gameBoard));
                DisplayGameBoardColour(gameBoard);
                Console.WriteLine("");
                //report final result of game based on value of currentPlayer:  0 = stalemate, 1 = player1, 2 = player2
                switch (currentPlayer)
                {
                    case 0:
                        Console.WriteLine($"Game ended in stalemate after {turnCount} turns.");
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Player 1");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write($" has won the game after {turnCount} turns.\n");
                        //Console.WriteLine($"Player {currentPlayer} has won the game after {turnCount} turns.");
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Player 2");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write($" has won the game after {turnCount} turns.\n");
                        //Console.WriteLine($"Player {currentPlayer} has won the game after {turnCount} turns.");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Game ended with unknown result. (Probably an error!)");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
                Console.WriteLine("");

                //ask user whether they want to either replay game or quit
                Console.WriteLine("Do you want to play again? Y/N");
                bool validResponse = false;
                while (validResponse == false)
                {
                    string inputString = Console.ReadLine();
                    switch (inputString)
                    {
                        case "Y":
                        case "y":
                            Console.Clear();
                            //program will now repeat because "userWantsToPlay" is still True.
                            validResponse = true;
                            break;
                        case "N":
                        case "n":
                            Console.Clear();
                            userWantsToPlay = false; //program will now terminate.
                            validResponse = true;
                            break;
                        default:
                            Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                            break;
                    }
                }
                //TO DO: if yes: do you want to re-use previous rows/columns and players? <-- may require dividing all loops up into methods! <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            } //end of "while (userWantsToPlay)"
           
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        } //end of Main method



        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        //Method to display game board. (Colourless version that uses StringBuilder)
        static string DisplayGameBoard(int[,] boardArray)
        {
             /* 
             * Example string output for a 6x7 game board:
             * 
             *    1   2   3   4   5   6   7
             *  |   |   |   |   |   |   |   |
             *  |---+---+---+---+---+---+---|
             *  |   |   |   |   |   |   |   |
             *  |---+---+---+---+---+---+---|
             *  |   |   |   |   | 1 |   |   |
             *  |---+---+---+---+---+---+---|
             *  |   |   |   | 2 | 2 |   |   |
             *  |---+---+---+---+---+---+---|
             *  |   |   | 2 | 1 | 1 |   |   |
             *  |---+---+---+---+---+---+---|
             *  |   | 2 | 1 | 1 | 2 | 1 |   |
             *  |===========================|
             */

            //Work out number of rows and columns from the provided 'boardArray' parameter
            int rows    = boardArray.GetLength(0);
            int columns = boardArray.GetLength(1);

            //Calculate CharLimit for String Builder
            int charLimit = ( (rows*2) * ((columns*4)+3) ); //(allows enough characters for '\n' line breaks)

            //Create a StringBuilder object to hold the string representation of the game board display
            StringBuilder boardBuilder = new StringBuilder(charLimit);

            //First, print the column number displays
            boardBuilder.Append(" ");
            //for each column in the game board...
            for (int c = 0; c < columns; c++)
            {
                boardBuilder.Append($"  {c+1} ");
            }
            //end of line and new line
            boardBuilder.Append("\n");

            //Now to print the remainder of the game board...
            //for each row in the game board...
            for (int r = 0; r < rows; r++) 
            {
                boardBuilder.Append(" "); //add a little whitespace to distance each row from the left edge of the console window

                //for each column in the game board...
                for (int c = 0; c < columns; c++)
                {
                    //print content of a line that has player token spaces in it
                    if (boardArray[r,c] == 0)
                        boardBuilder.Append("|   "); //Print whitespace instead of zeroes
                    else
                        boardBuilder.Append($"| {boardArray[r,c]} ");
                }
                //end of line and new line
                boardBuilder.Append("|\n");

                //If not at the bottom row yet, add a divider line + line break
                if((r + 1) < rows)
                {
                    //start of divider line
                    boardBuilder.Append(" |---");
                    //for each column in the game board...
                    for (int c = 1; c < columns; c++) //(starts at the 2nd column because we already covered the 1st)
                    {
                        boardBuilder.Append("+---");
                    }
                    //end of divider line and new line
                    boardBuilder.Append("|\n");
                }
                else //we're at the bottom row, so add the final line
                {
                    //start of final line
                    boardBuilder.Append(" |===");
                    //for each column in the game board...
                    for (int c = 1; c < columns; c++) //(starts at the 2nd column because we already covered the 1st)
                    {
                        boardBuilder.Append("====");
                    }
                    //end of final line
                    boardBuilder.Append("|");
                }
            } //end of row traversal

            return boardBuilder.ToString();
        } //end of DisplayGameBoard method



        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        //Method to display game board. (Coloured version that prints directly to console)
        static void DisplayGameBoardColour(int[,] boardArray)
        {
            /* 
            * Example string output for a 6x7 game board:
            * 
            *    1   2   3   4   5   6   7
            *  |   |   |   |   |   |   |   |
            *  |---+---+---+---+---+---+---|
            *  |   |   |   |   |   |   |   |
            *  |---+---+---+---+---+---+---|
            *  |   |   |   |   | 1 |   |   |
            *  |---+---+---+---+---+---+---|
            *  |   |   |   | 2 | 2 |   |   |
            *  |---+---+---+---+---+---+---|
            *  |   |   | 2 | 1 | 1 |   |   |
            *  |---+---+---+---+---+---+---|
            *  |   | 2 | 1 | 1 | 2 | 1 |   |
            *  |===========================|
            */

            //Work out number of rows and columns from the provided 'boardArray' parameter
            int rows = boardArray.GetLength(0);
            int columns = boardArray.GetLength(1);

            //First, print the column number displays
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" ");
            //for each column in the game board...
            for (int c = 0; c < columns; c++)
            {
                Console.Write($"  {c + 1} ");
            }
            //end of line and new line
            Console.Write("\n");

            //Now to print the remainder of the game board...
            Console.ForegroundColor = ConsoleColor.Blue;
            //for each row in the game board...
            for (int r = 0; r < rows; r++)
            {
                Console.Write(" "); //add a little whitespace to distance each row from the left edge of the console window

                //for each column in the game board...
                for (int c = 0; c < columns; c++)
                {
                    //print content of a line that has player token spaces in it
                    Console.Write("|");
                    switch (boardArray[r, c]) //Switch statement here allows us to display play token's in their respective colours.
                    {
                        case 0:
                            Console.Write("   "); //print whitespace instead of zeroes
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" 1 ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(" 2 ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" ? "); //this would only appear in an error scenario
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                    }
                }
                //end of line and new line
                Console.Write("|\n");

                //If not at the bottom row yet, add a divider line + line break
                if ((r + 1) < rows)
                {
                    //start of divider line
                    Console.Write(" |---");
                    //for each column in the game board...
                    for (int c = 1; c < columns; c++) //(starts at the 2nd column because we already covered the 1st)
                    {
                        Console.Write("+---");
                    }
                    //end of divider line and new line
                    Console.Write("|\n");
                }
                else //we're at the bottom row, so add the final line
                {
                    //start of final line
                    Console.Write(" |===");
                    //for each column in the game board...
                    for (int c = 1; c < columns; c++) //(starts at the 2nd column because we already covered the 1st)
                    {
                        Console.Write("====");
                    }
                    //end of final line
                    Console.Write("|\n");
                }
            } //end of row traversal

            Console.ForegroundColor = ConsoleColor.Gray;
        } //end of DisplayGameBoardColour method



        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        //Method to check whether the game has been won
        static bool hasPlayerWon(int[,] boardArray, int chosenRowIndex, int chosenColIndex, int currentPlayer , int targetCount)
        {
            int currentCount = 0;
            int i;

            //Work out maximum indices of rows and columns from the provided 'boardArray' parameter
            int maxRowIndex = (boardArray.GetLength(0) - 1);
            int maxColIndex = (boardArray.GetLength(1) - 1);

            //check vertical           
            if ((maxRowIndex - chosenRowIndex) >= (targetCount - 1)) //first check whether there are enough spaces underneath to fit a winning line
            {             
                currentCount = 1; //set currentCount to starting value
                while (currentCount < targetCount)
                {
                    //if the next token beneath is a matching token, increment currentCount and advance further down. Else if it's an opposing token then stop advancing.
                    if (boardArray[(chosenRowIndex + currentCount), chosenColIndex] == currentPlayer)
                        currentCount++;
                    else
                        break;
                }
            } //end of vertical check

            //check horizontal
            if (currentCount < targetCount)
            {
                //the previous check did not find a winning currentCount, hence reset it so that we can try a different check     
                currentCount = 1;          
                //check spaces to the left. If we find opposing token or edge of game board, go to check spaces right instead.
                i = 1;
                while ((currentCount < targetCount) && ((chosenColIndex - i) >= 0))
                {
                    if (boardArray[chosenRowIndex, (chosenColIndex - i)] == currentPlayer)
                    {
                        i++;
                        currentCount++;
                    }
                    else
                        break;
                }
                //now check spaces to the right. If we find opposing token or edge of game board, stop checking.
                i = 1;
                while ((currentCount < targetCount) && ((chosenColIndex + i) <= maxColIndex))
                {
                    if (boardArray[chosenRowIndex, (chosenColIndex + i)] == currentPlayer)
                    {
                        i++;
                        currentCount++;
                    }
                    else
                        break;
                }
            } //end of horizontal check

            //check diagonal AL-BR
            if (currentCount < targetCount)
            {
                //the previous check did not find a winning currentCount, hence reset it so that we can try a different check
                currentCount = 1;
                //check spaces above-left. If we find opposing token or edge of game board, go to check spaces below-right instead.
                i = 1;
                while ((currentCount < targetCount) && ((chosenRowIndex - i) >= 0) && ((chosenColIndex - i) >= 0))
                {
                    if (boardArray[(chosenRowIndex - i), (chosenColIndex - i)] == currentPlayer)
                    {
                        i++;
                        currentCount++;
                    }
                    else
                        break;
                }
                //now check spaces below-right. If we find opposing token or edge of game board, stop checking.
                i = 1;
                while ((currentCount < targetCount) && ((chosenRowIndex + i) <= maxRowIndex) && ((chosenColIndex + i) <= maxColIndex))
                {
                    if (boardArray[(chosenRowIndex + i), (chosenColIndex + i)] == currentPlayer)
                    {
                        i++;
                        currentCount++;
                    }
                    else
                        break;
                }
            } //end of diagonal AL-BR check

            //check diagonal BL-AR
            if (currentCount < targetCount)
            {
                //the previous check did not find a winning currentCount, hence reset it so that we can try a different check
                currentCount = 1;
                //check spaces below-left. If we find opposing token or edge of game board, go to check spaces above-right instead.
                i = 1;
                while ((currentCount < targetCount) && ((chosenRowIndex + i) <= maxRowIndex) && ((chosenColIndex - i) >= 0))
                {
                    if (boardArray[(chosenRowIndex + i), (chosenColIndex - i)] == currentPlayer)
                    {
                        i++;
                        currentCount++;
                    }
                    else
                        break;
                }
                //now check spaces above-right. If we find opposing token or edge of game board, stop checking.
                i = 1;
                while ((currentCount < targetCount) && ((chosenRowIndex - i) >= 0) && ((chosenColIndex + i) <= maxColIndex))
                {
                    if (boardArray[(chosenRowIndex - i), (chosenColIndex + i)] == currentPlayer)
                    {
                        i++;
                        currentCount++;
                    }
                    else
                        break;
                }
            } //end of diagonal BL-AR check

            //If we found a sequence of matching tokens equal to (or over) targetCount, the player wins. Otherwise the game can continue.
            return (currentCount >= targetCount);
        }
    }
}
