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
                    EquipItem = "[E]";
                }
                else
                {
                    EquipItem = "[ ]";
                }
                Name = name;
                if (SoldOut == true)
                {
                    Price = "구매완료";
                }
                else
                {
                    Price = price.ToString() + "G";
                    GetPrice = price; //구매가
                }
                Attack = attack;
                Defense = defense;
                Description = description;
                SoldOut = false;
                Equipped = false;
            }

            //구매 메서드
            public void Buy()
            {
                if (SoldOut == false)
                {
                    if (player.gold >= GetPrice)
                    {
                        Console.WriteLine("상점 주인: 크하하, 어때, 마음에 드나?");
                        Console.WriteLine($"{Name}을(를) 구매하였습니다.");
                        player.gold -= GetPrice; //구매 후 골드 감소
                        SoldOut = true; //판매 완료
                        Price = "구매완료"; //구매가 완료로 변경
                        //아이템을 구매하면 reciptItem에 add됨
                        reciptItem.Add(this); //구매한 아이템 리스트에 추가

                        Console.WriteLine("상점 주인: 가방에 넣어줄까, 아니면 입고 갈래?");

                        Console.WriteLine($"{Name}을(를) 장착하시겠습니까? (Y/N)");
                        string answer = Console.ReadLine();
                        switch (answer)
                        {
                            case "Y":
                                Console.WriteLine($"{Name}을(를) 장착하였습니다.");
                                //플레이어의 공격력과 방어력 증가
                                player.attack += Attack;
                                player.defense += Defense;

                                //장착된 아이템의 리스트 넘버링을 [E]로 출력
                                EquipItem = "[E]"; //장착 후 아이템 리스트에서 [ ] 제거

                                //아이템의 장착 여부를 true로 설정
                                Equipped = true;
                                break;
                            case "N":
                                Console.WriteLine($"{Name}을(를) 장착하지 않았습니다.");
                                break;
                            default:
                                Console.WriteLine("잘못된 입력입니다.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("상점 주인: 이런, 돈이 부족한 건 아닌가?");
                        Console.WriteLine($"{Name}을(를) 구매할 수 없습니다. 골드가 부족합니다.");
                    }

                }
                else
                {
                    Console.WriteLine("상점 주인: 그 물건은 이미 팔렸어. 다른 걸 찾아봐.");
                    Console.WriteLine($"{Name}은(는) 이미 판매되었습니다.");
                }
                ItemPurchase(player); //구매 후 상점으로 이동
            }

            //판매 메소드
            public void Sell()
            {
                //판매할 수 있는 조건: SoldOut이 true이고 Equipped가 false일 때
                //Equipped가 true일 때: 장착 해제 후 판매 가능
                //판매가는 구매가의 80%로 설정
                if (SoldOut == true && Equipped == false)
                {
                    Console.WriteLine($"{Name}을(를) 판매하였습니다.");
                    player.gold += (int)(GetPrice * 0.8); //판매 후 골드 증가
                    Price = GetPrice.ToString() + "G"; //"판매완료"를 다시 원가로 표시
                    SoldOut = false;

                    //판매하면 reciptItem에서 삭제
                    reciptItem.Remove(this);
                }
                else if (SoldOut == true && Equipped == true)
                {
                    Console.WriteLine("상점 주인: 그 물건은 장착중이잖아? 정말 팔 건가?");
                    Console.WriteLine(" ");
                    Console.WriteLine(Name + "을(를) 장착 해제 하고 판매하시겠습니까? (Y/N)");
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "Y":
                            Console.WriteLine($"{Name}을(를) 장착 해제 하였습니다.");
                            Console.WriteLine($"{Name}을(를) 판매하였습니다.");

                            Equipped = false;
                            SoldOut = false;

                            EquipItem = "[ ]"; //장착 해제 후 아이템 리스트에서 [E] 제거
                            Price = GetPrice.ToString() + "G"; //판매 후 가격 초기화

                            player.attack -= Attack; //플레이어의 공격력 감소
                            player.defense -= Defense; //플레이어의 방어력 감소
                            player.gold += (int)(GetPrice * 0.8); //판매 후 골드 증가

                            //판매하면 reciptItem에서 삭제
                            reciptItem.Remove(this); //구매한 아이템 리스트에서 삭제

                            ItemSell(player); //판매 후 상점으로 이동
                            break;
                        case "N":
                            Console.WriteLine("상점 주인: 뭐야, 그만 둘건가?");
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
                    //정상적인 플레이에서는 이 조건이 발생하지 않음.
                    Console.WriteLine("상점 주인: 그 물건은 안 살 거야. 다른 데 알아보라고.");
                    Console.WriteLine($"{Name}은(는) 판매할 수 없습니다.");
                }
                ItemSell(player); //판매 후 상점-판매로 이동

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
                    EquipItem = "[E]";
                    //아이템의 장착 여부를 true로 설정
                    Equipped = true;
                }
                else if (Equipped)
                {
                    Console.WriteLine($"{Name}의 장착을 해제하였습니다.");

                    //플레이어의 공격력과 방어력 감소
                    player.attack -= Attack;
                    player.defense -= Defense;

                    //장착된 아이템의 리스트 넘버링을 [ ]로 출력
                    EquipItem = "[ ]";
                    Equipped = false;
                }
                Equipment(); //장착 후 장비관리로 이동
            }

            //아이템 리스트 생성
            public static List<Item> itemList = new List<Item>()
            {
                new Item("[ ]","수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, 0, 5),
                new Item("[ ]","무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800, 0, 9),
                new Item("[ ]","스파르타의 갑옷", "스파르타 전사들이 입던 갑옷입니다.", 3500, 0, 15),
                new Item("[ ]","낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 600, 2, 0),
                new Item("[ ]","청동 도끼", "청동으로 만들어진 도끼입니다.", 1500, 5, 0),
                new Item("[ ]","스파르타의 창", "스파르타 전사들이 사용하던 창입니다.", 2000, 7, 0)
            };

            //구매한 아이템 리스트(SoldOut이 true인 아이템만 출력)
            //아이템을 리스트화
            public static List<Item> reciptItem = new List<Item>()
            {
                //아이템을 구매하면 여기에 add됨
            };
            //장비한 아이템의 공격력과 방어력 총 증가를 계산함 (공격력 7 장비를 장비시, 기본값+장비보정치 (+장비보정치)로 표시)
            public static void reciptAttack()
            {
                int totalAttack = 0; //아이템으로 증가한 공격력
                int totalDefense = 0; //아이템으로 증가한 방어력
                foreach (var item in Item.itemList)
                {
                    if (item.SoldOut == true && item.Equipped == true)
                    {
                        totalAttack += item.Attack;
                        totalDefense += item.Defense;
                    }
                }
                Console.WriteLine($"총 공격력: {totalAttack}");
                Console.WriteLine($"총 방어력: {totalDefense}");
            }
        }

        static void Main(string[] args)
        {
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
            //플레이어 스탯 반영
            //장비 착용 시 공격력, 방어력 증가를 표시함 (공격력 7 장비를 장비시, 기본값+장비보정치 (+장비보정치)로 표시)

            Console.WriteLine($"{player.name}님의 상태창입니다.");
            Console.WriteLine("==Status==");
            Console.WriteLine($"이름: {player.name}");
            Console.WriteLine($"레벨: {player.level}");
            Console.WriteLine($"직업: {player.job}");
            Console.WriteLine($"공격력: {player.attack} (+{player.attack - 10})");
            Console.WriteLine($"방어력: {player.defense} (+{player.defense - 5})");
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
            Console.WriteLine("낡은 가죽 가방을 열어본다.");

            Console.WriteLine("==Inventory==");
            //구매한 아이템=소지한 아이템
            //그럼 여기도 itemList가 아니라 reciptItem을 사용해야 함

            Item.reciptItem.ForEach(item =>
            {
                Console.WriteLine($"-{item.EquipItem} {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.GetPrice * 0.8}G");
            });

            Console.WriteLine(" ");
            Console.WriteLine("1. 장비관리");
            Console.WriteLine("0. 나가기");
            Console.Write("입력: ");
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
            //장비 관리 시 넘버링을 함께 확인
            Console.WriteLine("장착할 아이템을 선택해주세요.");
            Console.WriteLine("==Equip==");
            int index = 1; //아이템 리스트 넘버링

            //reciptItem에서 장착할 아이템을 선택
            Item.reciptItem.ForEach(item =>
            {
                Console.WriteLine($"{index++}. {item.EquipItem} {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.GetPrice * 0.8}G");
            });

            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");
            Console.Write("입력: ");
            int Eselect = int.Parse(Console.ReadLine());

            if (Eselect == 0)
            {
                Inventory();
            }
            else if (Eselect > 0 && Eselect <= Item.reciptItem.Count)
            {
                Item.reciptItem[Eselect - 1].Equip(); //장착 메서드 호출
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Equipment();
            }

        }

        protected static void Store()
        {
            Console.WriteLine($"문에 달린 경첩이 끼익거리며 열린다. 소리를 들은 상인이 호탕하게 웃으며 {player.name}을 맞이한다.");
            Console.WriteLine("상점 주인: 오, 모험가 양반. \n무엇을 도와드릴까?");
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
            Console.WriteLine($"상점 주인: 자, 자, 원하는 걸 골라봐.");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");

            Console.WriteLine("==Buy==");
            Console.WriteLine("구매할 아이템을 선택해주세요.");

            int index = 1; //아이템 리스트 넘버링
            Item.itemList.ForEach(item =>
            {
                Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
            });

            //넘버링을 통해 구매할 아이템 선택, 구매처리

            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");

            Console.Write("입력:");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    Item.itemList[0].Buy();
                    break;
                case 2:
                    Item.itemList[1].Buy();
                    break;
                case 3:
                    Item.itemList[2].Buy();
                    break;
                case 4:
                    Item.itemList[3].Buy();
                    break;
                case 5:
                    Item.itemList[4].Buy();
                    break;
                case 6:
                    Item.itemList[5].Buy();
                    break;
                case 0:
                    Console.WriteLine("상점 주인: 하하, 소중히 쓰라고!");
                    Store();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    ItemPurchase(player);
                    break;
            }

        }
        static void ItemSell(Player player)
        {
            Console.WriteLine($"상점 주인: 뭔가 팔 물건이라도 있나?");
            Console.WriteLine("==Sell==");
            Console.WriteLine("판매할 아이템을 선택해주세요.");
            Console.WriteLine("0. 나가기");

            int index = 1; //아이템 리스트 넘버링

            //내가 가진 물건 중에서만 판매

            Item.reciptItem.ForEach(item =>
            {
                Console.WriteLine($"{index++}. {item.EquipItem} {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.GetPrice * 0.8}G");
            });

            int Sellselect = int.Parse(Console.ReadLine());
            if (Sellselect == 0)
            {
                Console.WriteLine("상점 주인: 다른 용건이 더 있나?");
                Store();
            }
            else if (Sellselect > 0 && Sellselect <= Item.reciptItem.Count)
            {
                Console.WriteLine($"상점주인: 그 물건은 {Item.reciptItem[Sellselect - 1].GetPrice * 0.8}G에 사지. 팔텐가?");
                Console.WriteLine("판매하시겠습니까? (Y/N)");
                Item.reciptItem[Sellselect - 1].Sell(); //판매 메서드 호출
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ItemSell(player);
            }

        }
    }
}