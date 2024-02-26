using System;

class GemHunters
{
    static void Main()
    {
        Game gemHuntersGame = new Game();
        gemHuntersGame.Start();
    }
}
class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        if (direction == 'U')
            Position.Y -= 1;
        else if (direction == 'D')
            Position.Y += 1;
        else if (direction == 'L')
            Position.X -= 1;
        else if (direction == 'R')
            Position.X += 1;
    }
}

class Cell
{
    public string Occupant { get; set; }

    public Cell()
    {
        Occupant = "-";
    }
}

class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell();
            }
        }

        //players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        //gems
        Random random = new Random();
        for (int i = 0; i < 8; i++)
        {
            int x, y;
            do
            {
                x = random.Next(0, 6);
                y = random.Next(0, 6);
            } while (Grid[y, x].Occupant != "-");
            Grid[y, x].Occupant = "G";
        }

        
        for (int i = 0; i < 6; i++)
        {
            int x, y;
            do
            {
                x = random.Next(0, 6);
                y = random.Next(0, 6);
            } while (Grid[y, x].Occupant != "-");
            Grid[y, x].Occupant = "O";
        }
    }

    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        int x = player.Position.X;
        int y = player.Position.Y;

        if (direction == 'U' && y > 0 && Grid[y - 1, x].Occupant != "O")
            return true;
        else if (direction == 'D' && y < 5 && Grid[y + 1, x].Occupant != "O")
            return true;
        else if (direction == 'L' && x > 0 && Grid[y, x - 1].Occupant != "O")
            return true;
        else if (direction == 'R' && x < 5 && Grid[y, x + 1].Occupant != "O")
            return true;

        return false;
    }

    public void CollectGem(Player player)
    {
        int x = player.Position.X;
        int y = player.Position.Y;

        if (Grid[y, x].Occupant == "G")
        {
            player.GemCount += 1;
            Grid[y, x].Occupant = "-";
        }
    }
}

class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; set; }
    public int TotalTurns { get; set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            Board.Display();
            TakeTurn();
            SwitchTurn();
        }

        Board.Display();
        AnnounceWinner();
    }

    public void TakeTurn()
    {
        Console.WriteLine($"\n{CurrentTurn.Name}'s turn:");
        Console.Write("Enter U, D, L, R: ");
        char direction = char.ToUpper(Console.ReadKey().KeyChar);

        if (Board.IsValidMove(CurrentTurn, direction))
        {
            CurrentTurn.Move(direction);
            Board.CollectGem(CurrentTurn);
            TotalTurns += 1;
        }
        else
        {
            Console.WriteLine("\nInvalid move. Try again.");
        }
    }

    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    public bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    public void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine($"\n{Player1.Name} won");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine($"\n{Player2.Name} won");
        }
        else
        {
            Console.WriteLine("\nIt's a Draw!");
        }
    }
}


