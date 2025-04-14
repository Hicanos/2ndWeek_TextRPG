namespace SecondProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("플레이할 게임을 선택하세요");
            Console.WriteLine("1. 숫자 맞추기 게임");
            Console.WriteLine("2. 틱택토 게임");
            Console.WriteLine("3. 구구단 판");
            Console.WriteLine("4. 종료");
            int game = int.Parse(Console.ReadLine());
            switch (game)
            {
                case 1:
                    MatchNumber();
                    break;
                case 2:
                    TicTacToe();
                    break;
                case 3:
                    TimesTable();
                    break;
                case 4:
                    Console.WriteLine("게임을 종료합니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }

        }

        static void MatchNumber()
        {
            //컴퓨터 수 지정
            int ComNumber = new Random().Next(1, 101);
            int count = 10;

            Console.WriteLine("맞춰야 할 수가 정해졌습니다!");
            Console.WriteLine(count + "번의 기회가 있습니다.");
            Console.WriteLine("1~100 사이의 숫자를 입력하세요.");
            
            do 
            {
                bool isNumber = int.TryParse(Console.ReadLine(), out int number);

                if (!isNumber)
                {
                    Console.WriteLine("숫자를 입력하세요.");
                    continue;
                }
                else if (number < 1 || number > 100)
                {
                    Console.WriteLine("1~100 사이의 숫자를 입력하세요.");
                    continue;
                }
                else if (number == ComNumber)
                {
                    Console.WriteLine("정답입니다!");
                    break;
                }
                else if (number > ComNumber)
                {
                    Console.WriteLine("입력한 숫자가 더 큽니다.");
                }
                else
                {
                    Console.WriteLine("입력한 숫자가 더 작습니다.");
                }

                count--;
                Console.WriteLine($"남은 기회: {count}");

            } while (count > 0);

            if (count == 0)
            {
                Console.WriteLine("기회를 모두 소진했습니다.");
                Console.WriteLine($"정답은 {ComNumber}입니다.");
            }
        }

        static void TicTacToe()
        {
            char[] board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char player1 = 'X';
            char player2 = 'O';
            int moveCount = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("틱택토 게임");
                Console.WriteLine($"{board[0]} | {board[1]} | {board[2]}");
                Console.WriteLine("--|---|--");
                Console.WriteLine($"{board[3]} | {board[4]} | {board[5]}");
                Console.WriteLine("--|---|--");
                Console.WriteLine($"{board[6]} | {board[7]} | {board[8]}");
                char player = (moveCount % 2 == 0) ? player1 : player2; //플레이어 결정

                Console.WriteLine($"플레이어 {player}, 1~9 사이의 숫자를 입력하세요.");

                int move = int.Parse(Console.ReadLine()) - 1;

                // 입력값이 유효한지 확인
                // 1~9 사이의 숫자이고, 해당 위치에 이미 X 또는 O가 없는지 확인

                if (move < 0 || move > 8 || board[move] == player1 || board[move] == player2)
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                    continue;
                }
                // 보드에 플레이어의 기호를 놓기
                board[move] = player;
                moveCount++;
                // 승리 조건 확인
                if ((board[0] == player && board[1] == player && board[2] == player) ||
                    (board[3] == player && board[4] == player && board[5] == player) ||
                    (board[6] == player && board[7] == player && board[8] == player) ||
                    (board[0] == player && board[3] == player && board[6] == player) ||
                    (board[1] == player && board[4] == player && board[7] == player) ||
                    (board[2] == player && board[5] == player && board[8] == player) ||
                    (board[0] == player && board[4] == player && board[8] == player) ||
                    (board[2] == player && board[4] == player && board[6] == player))
                {
                    Console.Clear();
                    Console.WriteLine("틱택토 게임");
                    Console.WriteLine($"{board[0]} | {board[1]} | {board[2]}");
                    Console.WriteLine("--|---|--");
                    Console.WriteLine($"{board[3]} | {board[4]} | {board[5]}");
                    Console.WriteLine("--|---|--");
                    Console.WriteLine($"{board[6]} | {board[7]} | {board[8]}");
                    Console.WriteLine($"플레이어 {player}가 승리했습니다!");
                }
                else if (moveCount == 9) //위에서 이기지 않았고, 무승부
                {
                    Console.Clear();
                    Console.WriteLine("틱택토 게임");
                    Console.WriteLine($"{board[0]} | {board[1]} | {board[2]}");
                    Console.WriteLine("--|---|--");
                    Console.WriteLine($"{board[3]} | {board[4]} | {board[5]}");
                    Console.WriteLine("--|---|--");
                    Console.WriteLine($"{board[6]} | {board[7]} | {board[8]}");
                    Console.WriteLine("무승부입니다!");
                    break;
                }

            }
        }

        static void TimesTable()
        {
            Console.WriteLine("2단부터 9단까지의 구구단 판입니다.");

            for (int i = 1; i <= 9; i++)
            {
                
                for (int j = 2; j <= 9; j++)
                {
                    Console.Write(j+"x"+i+"="+(i * j)+ "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
