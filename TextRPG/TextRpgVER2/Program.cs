using System.Numerics;
using System.Xml.Linq;

namespace TextRpgVER2
{
    internal class Program
    {
        enum Job
        {
            전사 = 1,
            마법사,
            궁수
        }
        //플레이어 스탯 초기화
        class Player
        {
            //레벨, 이름, 직업, 공격력, 방어력, 체력, 골드
            public int level;
            public string name;
            public string job;
            public int attack;
            public int defense;
            public int hp;
            public int gold;

            //생성자
            public Player(string name, string job, int level, int attack, int defense, int hp, int gold)
            {
                this.name = name;
                this.job = job;
                this.level = level;
                this.attack = attack;
                this.defense = defense;
                this.hp = hp;
                this.gold = gold;
            }
        }
        static Player player = new Player("이름", "직업", 1, 10, 5, 100, 1500); //플레이어 객체 생성


        //아이템 클래스
        class Item
        {

            /*soldout이 true이면 판매 완료, false이면 판매 가능(Player 클래스의 gold와 비교)
            equipped가 true이면 장착 중, false이면 장착 안함
            장착표시, 아이템 이름, 가격, 공격력, 방어력, 설명, 판매여부, 착용여부
            구매가 완료되면 price를 구매완료로 변경*/

            public string EquipItem { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public int GetPrice { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public string Description { get; set; }
            public bool SoldOut { get; set; }
            public bool Equipped { get; set; }
            //생성자
            public Item(string EqupItem, string name, string description, int price, int attack, int defense)
            {
                if (Equipped == true)
                {
                    EquipItem = "-[E]";
                }
                else
                {
                    EquipItem = "-[ ]";
                }
                Name = name;
                if (SoldOut == true)
                {
                    Price = "구매완료";
                }
                else
                {
                    Price = price.ToString()+"G";
                    GetPrice = price; //구매가
                }
                Attack = attack;
                Defense = defense;
                Description = description;
                SoldOut = false;
                Equipped = false;
            }

            //판매 메소드
            public void Buy()
            {
                if (SoldOut == false)
                {
                    Console.WriteLine($"{Name}을(를) 구매하였습니다.");
                    SoldOut = true;
                    player.gold -= GetPrice; //구매 후 골드 감소                    
                }
                else
                {
                    Console.WriteLine($"{Name}은(는) 이미 판매되었습니다.");                    
                }
                ItemPurchase(player); //구매 후 상점으로 이동
            }

            //구매 메소드
            public void Sell()
            {
                //판매할 수 있는 조건: SoldOut이 true이고 Equipped가 false일 때
                //Equipped가 true일 때: 장착 해제 후 판매 가능
                //판매가는 구매가의 80%로 설정
                if (SoldOut == true && Equipped == false)
                {
                    Console.WriteLine($"{Name}을(를) 판매하였습니다.");
                    Console.WriteLine($"{Name}의 판매가는 {GetPrice * 0.8}G입니다.");
                    player.gold += (int)(GetPrice * 0.8); //판매 후 골드 증가
                    SoldOut = false;
                }
                else if (SoldOut == true && Equipped == true)
                {
                    Console.WriteLine($"{Name}은(는) 장착 중입니다.");
                    Console.WriteLine(Name + "을(를) 장착 해제 하고 판매하시겠습니까? (Y/N)");
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "Y":
                            Console.WriteLine($"{Name}을(를) 장착 해제 하였습니다.");
                            Console.WriteLine($"{Name}을(를) 판매하였습니다.");
                            player.gold += (int)(GetPrice * 0.8);
                            Equipped = false;
                            SoldOut = false;
                            player.attack -= Attack; //플레이어의 공격력 감소
                            player.defense -= Defense; //플레이어의 방어력 감소
                            ItemSell(player); //판매 후 상점으로 이동
                            break;
                        case "N":
                            Console.WriteLine($"{Name}을(를) 장착 해제 하지 않았습니다.");
                            Console.WriteLine($"{Name}은(는) 판매되지 않았습니다.");
                            ItemSell(player);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            ItemSell(player);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"{Name}은(는) 판매할 수 없습니다.");
                }
                ItemSell(player); //판매 후 상점으로 이동

            }

            //장착 및 장착 해제 메소드
            public void Equip()
            {
                if (SoldOut && !Equipped)
                {
                    Console.WriteLine($"{Name}을(를) 장착하였습니다.");
                    //플레이어의 공격력과 방어력 증가
                    player.attack += Attack;
                    player.defense += Defense;
                    //장착된 아이템의 리스트 넘버링을 [E]로 출력

                    //아이템의 장착 여부를 true로 설정
                    Equipped = true;
                }
                else if (Equipped)
                {
                    Console.WriteLine($"{Name}의 장착을 해제하였습니다.");
                    Equipped = false;
                }
                Equipment(); //장착 후 인벤토리로 이동
            }

            //아이템 리스트 생성
            public static List<Item> itemList = new List<Item>()
            {
                new Item("-[ ]","수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, 0, 5),
                new Item("-[ ]","무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800, 0, 9),
                new Item("-[ ]","스파르타의 갑옷", "스파르타 전사들이 입던 갑옷입니다.", 3500, 0, 15),
                new Item("-[ ]","낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 600, 2, 0),
                new Item("-[ ]","청동 도끼", "청동으로 만들어진 도끼입니다.", 1500, 5, 0),
                new Item("-[ ]","스파르타의 창", "스파르타 전사들이 사용하던 창입니다.", 2000, 7, 0)
            };

            //구매한 아이템 리스트(SoldOut이 true인 아이템만 출력)
            public static void reciptItem()
            {
                foreach (var item in Item.itemList)
                {
                    if (item.SoldOut == true)
                    {
                        Console.WriteLine($"{item.EquipItem} {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t| 판매가격: {item.GetPrice*0.8}G");
                    }
                }
            }

        }

        static void Main(string[] args)
        {
            Console.Clear(); //콘솔창 초기화
                             //시작 화면
            Console.WriteLine("게임을 시작합니다.\n 플레이어의 이름을 입력해주세요.");
            Console.Write("이름: ");
            string name = Console.ReadLine(); //이름 입력
            Console.WriteLine("직업을 선택해주세요.\n 1. 전사\n 2. 마법사\n 3. 궁수");
            Console.Write("직업: ");
            //번호에 따라 맞는 직업을 job에 입력, enum 사용
            Job job = (Job)Enum.Parse(typeof(Job), Console.ReadLine());


            Console.WriteLine($"{name}님, 게임을 시작합니다.");
            player.name = name; //플레이어 이름 설정
            player.job = job.ToString(); //직업 설정//플레이어 직업 설정

            GameStart(player); //게임 시작
        }

        static void GameStart(Player player)
        {
            Console.WriteLine($"스파르타 마을에 어서오세요, {player.name}님!");
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
                    Status(player);
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
                    GameStart(player);
                    break;
            }
        }

        static void Status(Player player)
        {
            Console.Clear(); //콘솔창 초기화
            //플레이어 스탯 반영
            //장비 착용 시 공격력, 방어력 증가를 표시함 (공격력 7 장비를 장비시, 17 (+7)로 표시)

            Console.WriteLine($"{player.name}님의 상태창입니다.");
            Console.WriteLine("==Status==");
            Console.WriteLine($"이름: {player.name}");
            Console.WriteLine($"레벨: {player.level}");
            Console.WriteLine($"직업: {player.job}");
            Console.WriteLine($"공격력: {player.attack}+"); 
            Console.WriteLine($"방어력: {player.defense}+");
            Console.WriteLine($"체력: {player.hp}");
            Console.WriteLine($"골드: {player.gold}");

            Console.WriteLine("원하는 행동을 선택해주세요.");
            Console.WriteLine("1. 인벤토리");
            Console.WriteLine("0. 나가기");
            int select = int.Parse(Console.ReadLine());
            switch (select)
            {
                case 1:
                    Inventory();
                    break;
                case 0:
                    GameStart(player);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Status(player);
                    break;
            }

        }


        static void Inventory()
        {
            Console.Clear();
            Console.WriteLine("==Inventory==");

            //구매한 아이템 리스트 출력(SoldOut이 true인 아이템만 출력)
            Item.reciptItem();

            Console.WriteLine(" ");
            Console.WriteLine("원하는 행동을 선택해주세요.");

            

            Console.WriteLine("1. 장비관리");
            Console.WriteLine("0. 나가기");
            string Iselect = Console.ReadLine();

            switch (Iselect)
            {
                case "1":
                    Equipment();
                    break;
                case "0":
                    GameStart(player);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Inventory();
                    break;
            }
        }

        static void Equipment()
        {
            Console.Clear();
            //장비 관리 시 넘버링을 함께 확인
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
                    Equipment();
                    break;
            }
        }

        protected static void Store()
        {
            Console.Clear();
            Console.WriteLine("상점에 오신 것을 환영합니다.");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");

            Console.WriteLine("==Store==");

            Item.itemList.ForEach(item =>
            {
                Console.WriteLine($"{item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
            });

            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            string Sselect = Console.ReadLine();
            switch (Sselect)
            {
                case "1":
                    ItemPurchase(player);
                    break;
                case "2":
                    ItemSell(player);
                    break;
                case "0":
                    GameStart(player);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Store();
                    break;
            }
        }
        static void ItemPurchase(Player player)
        {
            Console.WriteLine($"{player.name}님, 어서오세요.");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");

            Console.WriteLine("==Buy==");
            Console.WriteLine("구매할 아이템을 선택해주세요.");

            int index = 1; //아이템 리스트 넘버링
            Item.itemList.ForEach(item =>
            {
                Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
            });

            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");

            Console.Write("입력:");

        }
        static void ItemSell(Player player)
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
                    ItemSell(player);
                    break;
            }
        }

    }
}