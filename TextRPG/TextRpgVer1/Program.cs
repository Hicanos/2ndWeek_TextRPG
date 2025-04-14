namespace TextRpgVer1
{
    internal class Program
    {
        // 플레이어 스탯 초기화
        private static int lv = 1;
        private static int atk = 10;
        private static int def = 5;
        private static int hp = 100;
        private static int exp = 0;
        private static int gold = 1500;

        // Replace the problematic line with the following:
        private static string name;

        // Update the Main method to initialize the 'name' field:
        static void Main()
        {
            Console.WriteLine("게임을 시작합니다.");
            Console.WriteLine("플레이어 이름을 입력해주세요.");
            Console.Write("이름: ");
            name = Console.ReadLine(); // Assign the input to the static 'name' field
            Console.WriteLine(name + "님, 게임을 시작합니다.");
            Start();
        }

        static void Start()
        {
            Console.WriteLine($"스파르타 마을에 어서오세요,{name}님!");
            Console.WriteLine("여기서 할 수 있는 행동은 다음과 같습니다.");
            Console.WriteLine("1. 스테이터스");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 게임 종료");
            Console.WriteLine("원하는 행동을 선택해주세요.");
            string select = Console.ReadLine();
            switch (select)
            {
                case "1":
                    Staus();
                    break;
                case "2":
                    Inventory();
                    break;
                case "3":
                    Store();
                    break;
                case "4":
                    Console.WriteLine("게임을 종료합니다.");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Start();
                    break;
            }
        }

        static void Staus()
        {
            Console.WriteLine($"{name}님의 상태창입니다.");

            Console.WriteLine("==Status==");
            Console.WriteLine($"Level: {lv}");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Attack: {atk}");
            Console.WriteLine($"Defense: {def}");
            Console.WriteLine($"HP: {hp}");
            Console.WriteLine($"EXP: {exp}");
            Console.WriteLine($"GOLD: {gold}");

            Console.WriteLine("1. 인벤토리");
            Console.WriteLine("0. 나가기");
            string Statselect = Console.ReadLine();
            switch (Statselect)
            {
                case "1":
                    Inventory();
                    break;
                case "0":
                    Start();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Staus();
                    break;
            }
        }

        static void Inventory()
        {
            Console.WriteLine("==Inventory==");

            Console.WriteLine("1. 장비관리");
            Console.WriteLine("0. 나가기");
            string Iselect = Console.ReadLine();

            switch (Iselect)
            {
                case "1":
                    Equip();
                    break;
                case "0":
                    Start();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Inventory();
                    break;
            }

        }

        static void Equip()
        {
            Console.WriteLine("장착할 아이템을 선택해주세요.");
            Console.WriteLine("==Equip==");

            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");
            string Eselect = Console.ReadLine();
            switch (Eselect)
            {
                case "0":
                    Inventory();
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Equip();
                    break;
            }
        }

        static void Store()
        {
            Console.WriteLine("==Store==");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            string Sselect = Console.ReadLine();
            switch (Sselect)
            {
                case "1":
                    Buy();
                    break;
                case "2":
                    Sell();
                    break;
                case "0":
                    Start();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Store();
                    break;
            }
        }

        static void Buy()
        {
            Console.WriteLine($"{name}님, 어서오세요.");
            Console.WriteLine($"{name}님의 골드: {gold}G");

            Console.WriteLine("==Buy==");
            Console.WriteLine("구매할 아이템을 선택해주세요.");
            Console.WriteLine("1. 수련자 갑옷\t|방어력 +5│\t수련에 도움을 주는 갑옷입니다.\t| 1000G");
            Console.WriteLine("2. 무쇠갑옷\t|방어력 +9│\t무쇠로 만들어져 튼튼한 갑옷입니다.\t| 1800G");
            Console.WriteLine("3. 스파르타의 갑옷\t|방어력 +15│\t스파르타 전사들이 입던 갑옷입니다.\t| 3500G");
            Console.WriteLine("4. 낡은 검\t|공격력 +2│\t쉽게 볼 수 있는 낡은 검입니다.\t| 600G");
            Console.WriteLine("5. 청동 도끼\t|공격력 +5│\t청동으로 만들어진 도끼입니다.\t | 1500G");
            Console.WriteLine("6. 스파르타의 창\t|공격력 +7│\t스파르타 전사들이 사용하던 창입니다.\t 2000G");
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");

            Console.Write("입력:");
            string Bselect = Console.ReadLine();
            switch (Bselect)
            {
                case "0":
                    Store();
                    break;
                case "1":
                    if (gold >= 1000)
                    {
                        Console.WriteLine("수련자 갑옷을 구매했습니다.");
                        gold -= 1000;
                        def += 5;
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                    Store();
                    break;
                case "2":
                    if (gold >= 1800)
                    {
                        Console.WriteLine("무쇠갑옷을 구매했습니다.");
                        gold -= 1800;
                        def += 9;
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                    Store();
                    break;
                case "3":
                    if (gold >= 3500)
                    {
                        Console.WriteLine("스파르타의 갑옷을 구매했습니다.");
                        gold -= 3500;
                        def += 15;
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                    Store();
                    break;
                case "4":
                    if (gold >= 600)
                    {
                        Console.WriteLine("낡은 검을 구매했습니다.");
                        gold -= 600;
                        atk += 2;
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                    Store();
                    break;
                case "5":
                    if (gold >= 1500)
                    {
                        Console.WriteLine("청동 도끼를 구매했습니다.");
                        gold -= 1500;
                        atk += 5;
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                    Store();
                    break;
                case "6":
                    if (gold >= 2000)
                    {
                        Console.WriteLine("스파르타의 창을 구매했습니다.");
                        gold -= 2000;
                        atk += 7;
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                    Store();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Buy();
                    break;
            }
        }
        static void Sell()
        {
            Console.WriteLine("==Sell==");
            Console.WriteLine("판매할 아이템을 선택해주세요.");
            Console.WriteLine("0. 나가기");
            string Sellselect = Console.ReadLine();
            switch (Sellselect)
            {
                case "0":
                    Store();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Sell();
                    break;
            }
        }
    }
}
