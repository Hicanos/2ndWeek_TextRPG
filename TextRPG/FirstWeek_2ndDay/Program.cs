using System.Linq;
using System.Threading;

namespace FirstWeek_2ndDay
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("원하는 게임을 선택하세요.");
            Console.WriteLine("1. 스네이크 게임");
            Console.WriteLine("2. 블랙잭 게임");
            Console.WriteLine("3. 종료");
            Console.Write("선택: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    SnakeGame();
                    break;
                case "2":
                    BlackJack();
                    break;
                case "3":
                    Console.WriteLine("게임을 종료합니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Main(args);
                    break;
            }
        }


        // 방향을 나타내는 열거형
        public enum Direction { Up, Down, Left, Right }

        // 위치를 나타내는 구조체
        public struct Position
        {
            public int X;
            public int Y;

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            // 위치 비교
            public override bool Equals(object obj)
            {
                if (obj is Position)
                {
                    Position p = (Position)obj;
                    return X == p.X && Y == p.Y;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return X * 31 + Y;
            }
        }

        // 뱀 클래스
        public class Snake
        {
            public List<Position> Body { get; private set; }
            public Direction CurrentDirection { get; set; }
            public int Score { get; private set; }

            public Snake(int initialLength, Position startPos)
            {
                Body = new List<Position>();
                for (int i = 0; i < initialLength; i++)
                {
                    Body.Add(new Position(startPos.X - i, startPos.Y));
                }
                CurrentDirection = Direction.Right;
                Score = 0;
            }

            // 뱀 이동
            public void Move()
            {
                Position head = Body.First();
                Position newHead = head;
                switch (CurrentDirection)
                {
                    case Direction.Up:
                        newHead = new Position(head.X, head.Y - 1);
                        break;
                    case Direction.Down:
                        newHead = new Position(head.X, head.Y + 1);
                        break;
                    case Direction.Left:
                        newHead = new Position(head.X - 1, head.Y);
                        break;
                    case Direction.Right:
                        newHead = new Position(head.X + 1, head.Y);
                        break;
                }
                Body.Insert(0, newHead);
                Body.RemoveAt(Body.Count - 1);
            }

            // 먹이를 먹었을 때
            public void EatFood()
            {
                Position tail = Body.Last();
                Body.Add(tail); // 꼬리를 복제하여 길이 증가
                Score += 10;
            }

            // 충돌 검사
            public bool CheckCollision(int boardWidth, int boardHeight)
            {
                Position head = Body.First();
                // 벽과 충돌 검사
                if (head.X < 0 || head.X >= boardWidth || head.Y < 0 || head.Y >= boardHeight)
                    return true;
                // 자기 자신과 충돌 검사 (LINQ 사용)
                return Body.Skip(1).Any(pos => pos.Equals(head));
            }
        }

        // 먹이 클래스
        public class Food
        {
            public Position Position { get; private set; }
            private int boardWidth;
            private int boardHeight;
            private Random rand;

            public Food(int boardWidth, int boardHeight)
            {
                this.boardWidth = boardWidth;
                this.boardHeight = boardHeight;
                rand = new Random();
                Position = new Position();
            }

            // 새로운 먹이 생성
            public void GenerateFood(List<Position> snakeBody)
            {
                do
                {
                    Position = new Position(rand.Next(0, boardWidth), rand.Next(0, boardHeight));
                } while (snakeBody.Any(pos => pos.Equals(Position))); // 뱀의 몸과 겹치지 않게
            }
        }

        // 게임 클래스
        public class Game
        {
            private const int BoardWidth = 15;
            private const int BoardHeight = 15;
            private const int InitialSnakeLength = 3;
            private Snake snake;
            private Food food;
            private bool isGameOver;
            private Thread inputThread;

            public void SnakeGameStart()
            {
                Console.CursorVisible = false;
                Initialize();
                inputThread = new Thread(HandleInput);
                inputThread.Start();
                GameLoop();
            }

            // 초기화
            private void Initialize()
            {
                Position startPos = new Position(BoardWidth / 2, BoardHeight / 2);
                snake = new Snake(InitialSnakeLength, startPos);
                food = new Food(BoardWidth, BoardHeight);
                food.GenerateFood(snake.Body);
                isGameOver = false;
            }

            // 게임 루프
            private void GameLoop()
            {
                while (!isGameOver)
                {
                    snake.Move();
                    if (snake.CheckCollision(BoardWidth, BoardHeight))
                    {
                        isGameOver = true;
                        break;
                    }

                    // 먹이를 먹었는지 확인
                    if (snake.Body.First().Equals(food.Position))
                    {
                        snake.EatFood();
                        food.GenerateFood(snake.Body);
                    }

                    Render();
                    Thread.Sleep(200); // 게임 속도 조절
                }

                Console.Clear();
                Console.SetCursorPosition(BoardWidth / 2 - 5, BoardHeight / 2);
                Console.Write($"Game Over! Score: {snake.Score}");
                Console.SetCursorPosition(0, BoardHeight + 2);

                Console.WriteLine("1. 다시하기");
                Console.WriteLine("2. 게임선택");
                Console.WriteLine("3. 종료");
                Console.Write("선택: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        SnakeGameStart();
                        break;
                    case "2":
                        Main(new string[0]);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }

            // 렌더링
            private void Render()
            {
                Console.SetCursorPosition(0, 0);
                for (int y = 0; y < BoardHeight; y++)
                {
                    for (int x = 0; x < BoardWidth; x++)
                    {
                        Position currentPos = new Position(x, y);
                        if (snake.Body.First().Equals(currentPos))
                            Console.Write("■ "); // 뱀의 머리
                        else if (snake.Body.Skip(1).Any(pos => pos.Equals(currentPos)))
                            Console.Write("■ "); // 뱀의 몸통
                        else if (food.Position.Equals(currentPos))
                            Console.Write("● "); // 먹이
                        else
                            Console.Write("□ "); // 빈 공간
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"Score: {snake.Score}");
            }

            // 입력 처리
            private void HandleInput()
            {
                while (!isGameOver)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.W:
                                if (snake.CurrentDirection != Direction.Down)
                                    snake.CurrentDirection = Direction.Up;
                                break;
                            case ConsoleKey.S:
                                if (snake.CurrentDirection != Direction.Up)
                                    snake.CurrentDirection = Direction.Down;
                                break;
                            case ConsoleKey.A:
                                if (snake.CurrentDirection != Direction.Right)
                                    snake.CurrentDirection = Direction.Left;
                                break;
                            case ConsoleKey.D:
                                if (snake.CurrentDirection != Direction.Left)
                                    snake.CurrentDirection = Direction.Right;
                                break;
                            case ConsoleKey.Escape:
                                isGameOver = true;
                                break;
                        }
                    }
                    Thread.Sleep(50);
                }
            }
        }

        static void SnakeGame()
        {
            Console.Clear();
            Console.WriteLine("스네이크 게임을 시작합니다.");
            Console.WriteLine("  ");
            Console.WriteLine("===========게임규칙===========");
            Console.WriteLine("1. 스네이크는 방향키로 이동합니다.");
            Console.WriteLine("2. 먹이를 먹으면 길어집니다.");
            Console.WriteLine("3. 벽에 부딪히면 게임 오버입니다.");
            Console.WriteLine("4. 점수는 먹이를 먹을 때마다 10점씩 올라갑니다.");
            Console.WriteLine("5. 게임 오버 시 최종 점수를 보여줍니다.");
            Console.WriteLine("게임을 종료하려면 ESC 키를 누르세요.");
            Console.WriteLine("게임을 시작하려면 아무 키나 누르세요.");
            Console.ReadKey();
            Console.Clear();

            Game game = new Game();
            game.SnakeGameStart();
        }


        static void BlackJack()
        {
            Console.Clear();
            Console.WriteLine("블랙잭 게임을 시작합니다.");
            Console.WriteLine("  ");
            Console.WriteLine("===========게임규칙===========");
            Console.WriteLine("1. 카드의 숫자 합이 21에 가까워야 합니다.");
            Console.WriteLine("2. A는 1 또는 11로 계산할 수 있습니다.");
            Console.WriteLine("3. 10, J, Q, K는 10으로 계산합니다.");
            Console.WriteLine("4. 21을 초과하면 버스트입니다.");
            Console.WriteLine("5. 딜러와 플레이어의 카드 합을 비교하여 승패를 결정합니다.");
            Console.WriteLine("게임을 종료하려면 ESC 키를 누르세요.");
            Console.WriteLine("게임을 시작하려면 아무 키나 누르세요.");
            Console.ReadKey();
            Console.Clear();

        }

        static void BlackJackGameStart()
        {
            
        }

    }
}
