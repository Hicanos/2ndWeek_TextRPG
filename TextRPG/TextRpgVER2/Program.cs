using System;
using System.ComponentModel.Design;
using System.Data;
using System.Numerics;
using System.Xml.Linq;

namespace TextRpgVER2
{
    internal class Program
    {
        //플레이어 직업 enum
        enum Job
        {
            전사 = 1,
            마법사,
            궁수
        }


        //플레이어 클래스
        class Player
        {
            //레벨, 이름, 직업, 공격력, 아이템공격력, 방어력, 아이템 방어력, 체력, 골드
            public int level;
            public string name;
            public string job;

            public float attack;
            public int itemAttack;
            public float defense;
            public int itemDefense;

            public int hp;
            public int gold;

            public int storeCount; //상점 방문 횟수
            public int killCount; //몬스터 처치 횟수

            public int exp; //경험치
            public int needExp; //레벨업에 필요한 경험치

            //생성자
            //이름, 직업, 레벨, 공격력, 방어력, 체력, 골드, 아이템 공격력, 아이템 방어력, 상점 방문 횟수, 몬스터 처치 횟수, 경험치
            public Player(string name, string job, int level, float attack, float defense, int hp, int gold, int itemAttack, int itemDefense, int storecount, int killCount, int exp)
            {
                this.name = name;
                this.job = job;
                this.level = level;
                this.attack = attack;
                this.defense = defense;
                this.hp = hp;
                this.gold = gold;
                this.itemAttack = itemAttack;
                this.itemDefense = itemDefense;
                this.storeCount = storecount;
                this.killCount = killCount;
                this.exp = exp;
                needExp = 125 + (level * level * level); //레벨업에 필요한 경험치 계산
            }

            //레벨업 메서드-매 몬스터 처치 시 경험치 증가, 레벨업 조건 확인
            public void LevelUp()
            {
                if (exp >= needExp)
                {
                    //필요 경험치보다 경험치가 적어질 때까지 반복
                    while (exp >= needExp)
                    {
                        attack += attack * 0.5f;
                        defense += defense * 0.5f;
                        level++;
                        exp -= needExp; //레벨업 후 경험치 초기화
                        needExp = 125 + (level * level * level); //다음 레벨업에 필요한 경험치 계산
                        Console.WriteLine("============================================");
                        Console.WriteLine($"{name}의 레벨이 올랐습니다! \n 현재 레벨: {level}");
                        Console.WriteLine($"공격력: {attack + itemAttack} (+{itemAttack})");
                        Console.WriteLine($"방어력: {defense + itemAttack} (+{itemDefense})");
                        Console.WriteLine("============================================");
                    }

                }
            }

            //게임 오버 메서드
            public void GameOver()
            {
                Console.WriteLine("눈 앞이 깜깜해지고, 어지럽다.");
                Console.WriteLine("이대로 끝나는 건가?");
                Console.WriteLine("=============================");
                Console.WriteLine("계속 하시겠습니까?");
                Console.WriteLine("(Y/N)");
                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "Y":
                        Console.WriteLine("눈을 뜨자, 주변의 소음이 들려온다.");
                        Console.WriteLine("몸은 푹신한 침대 위에 누워있다.");
                        Console.WriteLine("여관 주인: 아, 드디어 일어났네. 몸은 좀 괜찮아?");
                        Console.WriteLine("여관 주인: 던전에서 쓰러진 걸, 다른 모험가들이 발견하고 데려왔어.");
                        Console.WriteLine("여관 주인: 이번은 무사해서 다행이지만, 다음부터는 조심해.");
                        Console.WriteLine("여관 주인: 방에 치료비까지 다해서 1000G야.");
                        Console.WriteLine("여관 주인: 나도 먹고 살아야지.");

                        if (player.gold >= 1000)
                        {
                            player.gold -= 1000; //골드 차감
                            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
                            Console.WriteLine($"{player.name}의 체력이 모두 회복되었습니다.");
                            Console.WriteLine("여관 주인: 다음부터는 무리하지 마.");
                            player.hp = 100; //체력 회복                            
                        }
                        else if (player.gold < 1000 && player.gold >= 500)
                        {
                            Console.WriteLine("여관 주인: 돈이 부족한 거야?");
                            Console.WriteLine("여관 주인: 어쩔 수 없네. 지금 가진 돈이라도 줘.");

                            int plusHP = 100 - (1000 - player.gold) / 10; //남은 골드가 600이면 60 회복
                            player.hp += plusHP; //체력 회복
                            player.gold = 0; //골드 전액 차감
                        }
                        else
                        {
                            Console.WriteLine("여관 주인: 아이고, 방값도 없는 거야?");
                            Console.WriteLine("여관 주인: 그렇다고 치료한 것까지 다 뺏어갈 생각은 없어.");
                            Console.WriteLine("여관 주인: 이번에는 그냥 가. 다음부터는 조심하고.");
                            Console.WriteLine("여관 주인: 돈을 벌려면 모험을 가야겠지. 이건 얼마 안되지만, 모험 자금으로 써.");
                            Console.WriteLine("   ");
                            Console.WriteLine("여관 주인에게서 300G를 받았습니다.");
                            Console.WriteLine("여관 주인: 미안하면 다음에 와서 많이 사줘.");
                            player.hp = 45; //체력 회복
                            player.gold += 300; //골드 지급
                        }

                        Tarvern(); //여관으로 이동
                        break;
                    case "N":
                        Console.WriteLine("당신의 여정은 여기서 끝나고 말았다.");
                        Console.WriteLine("하지만, 당신의 꿈은.");
                        Console.WriteLine("언젠가 다른 모험가의 꿈으로, 발자취로 이어질 것이다.");
                        Console.WriteLine("===================================");

                        Console.WriteLine("게임을 종료합니다.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        GameOver();
                        break;
                }
            }

        }
        //직업 별로 다른 스탯 적용 (전사: 공격력 5, 방어력 10/ 마법사: 공격력 10, 방어력 5/ 궁수: 공격력 8, 방어력 7)
        //플레이어 생성
        static Player player = new Player("이름", "직업", 1, 0, 0, 100, 1500, 0, 0, 0, 0, 0); //플레이어 생성


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
                                player.itemAttack += Attack;
                                player.itemDefense += Defense;

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
                    Console.WriteLine("상점 주인: 그 물건은 이미 가지고 있잖아? 다른 물건은 어때?");
                    Console.WriteLine($"{Name}을(를) 이미 소유하고 있습니다.");
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

                            player.itemAttack -= Attack; //플레이어의 공격력 감소
                            player.itemDefense -= Defense; //플레이어의 방어력 감소
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
                    //플레이어의 아이템 공격력과 방어력 증가
                    player.itemAttack += Attack;
                    player.itemDefense += Defense;

                    //장착된 아이템의 리스트 넘버링을 [E]로 출력
                    EquipItem = "[E]";
                    //아이템의 장착 여부를 true로 설정
                    Equipped = true;
                }
                else if (Equipped)
                {
                    Console.WriteLine($"{Name}의 장착을 해제하였습니다.");

                    //플레이어의 공격력과 방어력 감소
                    player.itemAttack -= Attack;
                    player.itemDefense -= Defense;

                    //장착된 아이템의 리스트 넘버링을 [ ]로 출력
                    EquipItem = "[ ]";
                    Equipped = false;
                }
                Equipment(); //장착 후 장비관리로 이동
            }

            //아이템 리스트 생성-직업군 별로 다른 아이템 출력

            public static List<Item> itemList_Warrior = new List<Item>()
            {
                new Item("[ ]","수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1200, 0, 6),
                new Item("[ ]","무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, 0, 10),
                new Item("[ ]","스파르타의 갑옷", "스파르타 전사들이 입던 갑옷입니다.", 4000, 0, 17),
                new Item("[ ]","낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 600, 2, 0),
                new Item("[ ]","청동 도끼", "청동으로 만들어진 도끼입니다.", 1500, 4, 1),
                new Item("[ ]","스파르타의 창", "스파르타 전사들이 사용하던 창입니다.", 2500, 7, 2)
            };

            public static List<Item> itemList_Wizard = new List<Item>()
            {
                new Item("[ ]","수련자의 로브", "견습 마법사들이 입는 로브입니다.", 1000, 2, 3),
                new Item("[ ]","마법사의 로브", "마법사들이 입는 로브입니다.", 1800, 4, 5),
                new Item("[ ]","드루이드의 로브", "드루이드들이 입던 로브입니다.", 3500, 6, 9),
                new Item("[ ]","낡은 완드", "쉽게 볼 수 있는 낡은 완드입니다.", 900, 3, 0),
                new Item("[ ]","의식용 완드", "청동으로 만들어진 지팡이입니다.", 1800, 6, 0),
                new Item("[ ]","드루이드의 스태프", "드루이드들이 사용하던 스태프입니다.", 3500, 11, 0)
            };

            public static List<Item> itemList_Archer = new List<Item>()
            {
                new Item("[ ]","수련자의 경갑", "견습 궁수들이 입는 경갑옷입니다.", 1000, 0, 5),
                new Item("[ ]","궁수의 경갑", "궁수들이 입는 경갑옷입니다.", 1800, 0, 9),
                new Item("[ ]","크레타의 경갑", "크레타 궁수들이 입던 경갑입니다.", 4000, 2, 15),
                new Item("[ ]","낡은 활", "쉽게 볼 수 있는 낡은 활입니다.", 600, 2, 0),
                new Item("[ ]","청동 활", "청동으로 만들어진 활입니다.", 1500, 5, 0),
                new Item("[ ]","크레타의 활", "크레타 궁수들이 사용하던 활입니다.", 3500, 7, 4)
            };

            //특별상점 아이템 리스트
            public static List<Item> specialItemList = new List<Item>()
            {
                new Item("[ ]","용맹의 전투 갑옷", "전투 중 집중력을 높여 방어력을 강화하는 갑옷입니다.", 4500, 0, 21),
                new Item("[ ]","기사 갑옷", "기사들이 입는 튼튼한 갑옷입니다.", 10000, 0, 40),
                new Item("[ ]","황금 갑옷", "황금으로 만들어진 갑옷입니다. 실전에서 사용하긴 힘듭니다.", 50000, 0, 5),
                new Item("[ ]","세계수의 도끼", "세계수로 만든 도끼로, 공격력도 뛰어납니다.", 5000, 10, 8),
                new Item("[ ]","광휘의 창", "강력한 마법이 깃든 창입니다.", 10000, 21, 15),                
                new Item("[ ]","기사 검방패", "기사들이 사용하는 검과 방패입니다.", 20000, 50, 22),
                new Item("[ ]","황금 검", "황금으로 만들어진 검입니다. 실전에서 사용하긴 힘듭니다.", 100000, 5, 0),
            };

            //구매한 아이템 리스트(SoldOut이 true인 아이템만 출력)
            //아이템을 리스트화
            public static List<Item> reciptItem = new List<Item>()
            {
                //아이템을 구매하면 여기에 add됨
            };

        }


        //던전 클래스
        class Dungeon
        {
            //던전 생성
            //던전 이름, 권장 방어력, 보상 기본 골드

            public string name;
            public string description;
            public int recommendDefense;
            public int rewardGold;

            //생성자
            public Dungeon(string name, string description, int recommendDefense, int rewardGold)
            {
                this.name = name;
                this.description = description;
                this.recommendDefense = recommendDefense;
                this.rewardGold = rewardGold;
            }

            //던전 리스트 생성 (평원, 바위산, 화산)
            public static List<Dungeon> dungeonList = new List<Dungeon>()
            {
                new Dungeon("평원 던전", "드넓은 풀밭과 나무들이 어우러진 평원입니다.", 5, 1000),
                new Dungeon("바위산 던전", "거대한 바위들이 우뚝 솟아있는 바위산입니다.", 11, 1700),
                new Dungeon("화산 던전", "활화산으로 불길이 치솟는 위험한 지역입니다.", 17, 2500)
            };

            //던전 진행 메서드 (권장 방어력보다 낮으면 일정 확률로 실패함)
            public void DungeonSelect()
            {
                Console.WriteLine($"{name}의 권장 방어력은 {recommendDefense}입니다.");
                Console.WriteLine("입장하시겠습니까?(Y/N)");
                Console.Write("입력: ");
                string answer = Console.ReadLine();
                Console.WriteLine("============================================");
                switch (answer)
                {
                    case "Y":

                        Console.WriteLine("던전 공략을 시작합니다.");
                        Console.WriteLine("============================================");
                        //실패는 방어력이 권장 방어력보다 낮을 때만 확률적으로 발생
                        if (player.defense + player.itemDefense <= recommendDefense)
                        {
                            int clear = new Random().Next(1, 101); //던전 공략 실패 확률 1~100
                            if (clear <= 40) //40% 확률로 실패
                            {
                                Console.WriteLine("던전 공략에 실패했습니다.");
                                Console.WriteLine($"체력이 {50 + recommendDefense - (player.defense + player.itemDefense)}감소합니다.");
                                player.hp -= (int)(50 + recommendDefense - (player.defense + player.itemDefense)); //체력 감소
                                if (player.hp <= 0)
                                {
                                    player.hp = 0; //마이너스가 되더라도 체력 0으로 설정
                                    Console.WriteLine("============================================");
                                    Console.WriteLine("공략은 실패했다. 하지만….");
                                    Console.WriteLine($"{player.name}님의 남은 체력: 0");
                                    player.GameOver(); //게임 오버 메서드 호출
                                    break;
                                }
                                Console.WriteLine($"{player.name}님의 체력: {player.hp}");

                                DungeonStart(); //던전으로 이동
                            }        
                            
                        }
                        //나머지는 성공                        
                        //체력 소모 값=20~35에 방어력에 따른 보정치 발생
                        int minusHP = new Random().Next(20, 36);
                        player.hp -= (int)(minusHP - ((player.defense + player.itemDefense) - 5));

                        //공략에 성공해도 체력이 0이 되면 행동불능->실패
                        if (player.hp <= 0)
                        {
                            player.hp = 0; //마이너스가 되더라도 체력 0으로 설정
                            Console.WriteLine("============================================");
                            Console.WriteLine("공략은 성공했다. 하지만….");
                            Console.WriteLine($"{player.name}님의 남은 체력: 0");
                            player.GameOver(); //게임 오버 메서드 호출
                            break;
                        }
                        Console.WriteLine("던전 공략에 성공했습니다.");
                        Console.WriteLine("============================================");
                        Console.WriteLine($"{player.name}님의 체력이 {minusHP} 감소했습니다.");
                        Console.WriteLine($"{player.name}님의 체력: {player.hp}");

                        //보상은 공격력~공격력*2 %까지 보정됨 (공격력 10이면 10%~20%까지 보정됨)
                        int rewardPercent = (int)(player.attack + player.itemAttack);
                        int reward = new Random().Next(rewardPercent, rewardPercent * 2 + 1);
                        player.gold += (int)(rewardGold * (1 + reward * 0.01f)); //던전 보상 골드 지급 (기본 보상+보정치) 즉, 리워드가 20%로 결정되면 1.2배 지급
                        player.exp += (int)(minusHP * 10); //감소한 체력의 10배 만큼 경험치 지급

                        Console.WriteLine("==던전 결과==");
                        Console.WriteLine($"획득 Gold:{(int)(rewardGold * (1 + reward * 0.01f))}");
                        Console.WriteLine($"획득 경험치:{(int)(minusHP * 10)}");
                        player.LevelUp(); //레벨업 체크
                        DungeonStart(); //던전 공략 후 던전으로 이동 
                        break;
                    case "N":
                        Console.WriteLine("던전에 들어가지 않고, 다시 발길을 돌렸다.");
                        Console.WriteLine("============================================");
                        DungeonStart(); //던전으로 이동
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        DungeonSelect();
                        break;
                }

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
            //선택한 직업에 따라 스탯 초기화
            if (job == Job.전사)
            {
                player.attack = 5;
                player.defense = 10;
            }
            else if (job == Job.마법사)
            {
                player.attack = 10;
                player.defense = 5;
            }
            else if (job == Job.궁수)
            {
                player.attack = 8;
                player.defense = 7;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Main(args);
            }


            Console.WriteLine($"{name}님, 게임을 시작합니다.");
            player.name = name; //플레이어 이름 설정
            player.job = job.ToString(); //직업 설정//플레이어 직업 설정

            GameStart(player); //게임 시작
        }

        static void GameStart(Player player)
        {
            Console.WriteLine($"그린 티 마을에 어서오세요, {player.name}님!");
            Console.WriteLine("============================================");
            Console.WriteLine("마을의 초입부터 푸른 녹음이 감싸는 조용하고 평화로운 마을. \n 부드러운 바람에 실려 오는 풀내음과 찻잎의 향기가 마을 전체를 감싼다. \n마을의 건물들은 대부분 따뜻한 색의 목재건물로 지어져, 자연과 어우러지고, 아늑함을 준다.\n마을에 모여든 모험가들은 각자의 여정을 시작하고, 이어간다.\n");
            Console.WriteLine("여기서 할 수 있는 행동은 다음과 같습니다.");
            Console.WriteLine(" ");
            Console.WriteLine("1. 스테이터스");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 여관");
            Console.WriteLine("5. 던전");
            Console.WriteLine("0. 게임종료");
            Console.WriteLine("============================================");
            Console.WriteLine(" ");
            Console.WriteLine("원하는 행동을 선택해주세요.");
            Console.Write("입력: ");
            int select = int.Parse(Console.ReadLine());
            switch (select)
            {
                case 1:
                    Status(player);
                    break;
                case 2:
                    Inventory();
                    Console.WriteLine("낡은 가죽 가방을 열어본다.");
                    break;
                case 3:
                    player.storeCount += 1;
                    Store();
                    Console.WriteLine($"문에 달린 경첩이 끼익거리며 열린다. 소리를 들은 상인이 호탕하게 웃으며 {player.name}을 맞이한다.");
                    Console.WriteLine(" ");
                    break;
                case 4:
                    Tarvern();
                    Console.WriteLine("맑은 종소리가 울린다. 분주하게 움직이던 직원이 큰 소리로 외친다.");
                    Console.WriteLine("직원: 어서오세요! '한 잔의 휴식'입니다!");
                    break;
                case 5:
                    Console.WriteLine("마을 밖으로 나가, 던전으로 향했다.");
                    DungeonStart();
                    break;
                case 0:
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

            Console.WriteLine($"{player.name}님의 현재 상태입니다.");
            Console.WriteLine("==Status==");
            Console.WriteLine($"이름: {player.name}");
            Console.WriteLine($"레벨: {player.level}");
            Console.WriteLine($"직업: {player.job}");
            Console.WriteLine($"공격력: {player.attack + player.itemAttack} (+{player.itemAttack})");
            Console.WriteLine($"방어력: {player.defense + player.itemDefense} (+{player.itemDefense})");
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
                    Console.WriteLine("낡은 가죽 가방을 열어본다.");
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
            Console.WriteLine("상점은 오래되었지만 깔끔하다. \n여러 사람이 오가며 맨질하게 닳은 바닥은, 이곳이 많은 모험가들을 맞이했음을 알려준다. \n진열된 물건들 중에서는 낡은 무기들도 있지만, \n상당히 좋은 품질의 무기와 방어구도 눈에 들어온다.");
            Console.WriteLine(" ");
            if (player.storeCount == 1)
            {
                Console.WriteLine("상점 주인: 오, 처음보는 얼굴이군. 새로운 모험가인가? 어서와, 어서와!");
                Console.WriteLine("상점 주인: 필요한 게 있으면 얼마든지 말하라고!");
            }
            else if (player.storeCount > 1 && player.storeCount < 10)
            {
                Console.WriteLine("상점 주인: 오, 또 왔군. 이번엔 무엇을 사러왔나?");
            }
            else if (player.storeCount == 10)
            {
                Console.WriteLine($"상점 주인: 이야, {player.name} 아닌가! 오늘은 무슨 일이야?");
                Console.WriteLine("상점 주인: 이렇게 자주 와주니 내가 다 기쁘군.");
                Console.WriteLine("단골에게는, 특별 상점을 열고 있다네. 조금 비싸긴 하지만 말이야! 하하!");
            }
            else
            {
                Console.WriteLine($"상점 주인: 어서오게, {player.name}!");
                Console.WriteLine("상점 주인: 오늘 모험은 어땠는가?");
            }
            Console.WriteLine("=======================================");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
            Console.WriteLine("=======================================");

            Console.WriteLine("==Store==");

            if (Job.전사.ToString() == player.job)
            {
                Item.itemList_Warrior.ForEach(item =>
                {
                    Console.WriteLine($"{item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
                });
            }
            else if (Job.마법사.ToString() == player.job)
            {
                Item.itemList_Wizard.ForEach(item =>
                {
                    Console.WriteLine($"{item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
                });
            }
            else if (Job.궁수.ToString() == player.job)
            {
                Item.itemList_Archer.ForEach(item =>
                {
                    Console.WriteLine($"{item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
                });
            }


            Console.WriteLine("=============================");
            Console.WriteLine("  ");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            if (player.storeCount >= 10)
            {
                Console.WriteLine("3. 특별 상점");
            }
            Console.WriteLine("0. 나가기\n ");
            string Sselect = Console.ReadLine();
            switch (Sselect)
            {
                case "1":
                    ItemPurchase(player);
                    break;
                case "2":
                    ItemSell(player);
                    break;
                case "3":
                    if (player.storeCount >= 10)
                    {
                        Console.WriteLine("상점 주인: 하핫, 단골에게는 얼마든지!");
                        Console.WriteLine("상점 주인: 특별 상품들은 기존에 파는 물건들 보다 훨씬 성능이 좋지.");
                        Console.WriteLine("상점 주인: 하지만 비싸니까, 돈은 두둑히 챙겨와!");
                        SpecialStore();
                    }
                    else
                    {
                        Console.WriteLine("상점 주인: 특별 상점이야기를 어디서 들었는진 모르겠지만,");
                        Console.WriteLine("상점 주인: 이건 단골에게만 보여주는 물건들이야.");
                        Store();
                    }
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
            Console.WriteLine("=======================================");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
            Console.WriteLine("=======================================");
            Console.WriteLine("  \n");
            Console.WriteLine($"상점 주인: 자, 자, 원하는 걸 골라봐.");


            Console.WriteLine("==Buy==");


            int index = 1; //아이템 리스트 넘버링
            if (Job.전사.ToString() == player.job)
            {
                Item.itemList_Warrior.ForEach(item =>
                {
                    Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
                });
            }
            else if (Job.마법사.ToString() == player.job)
            {
                Item.itemList_Wizard.ForEach(item =>
                {
                    Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
                });
            }
            else if (Job.궁수.ToString() == player.job)
            {
                Item.itemList_Archer.ForEach(item =>
                {
                    Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
                });
            }

            //넘버링을 통해 구매할 아이템 선택, 구매처리
            Console.WriteLine("구매할 아이템을 선택해주세요.");
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기\n ");

            Console.Write("입력:");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    if (Job.전사.ToString() == player.job)
                    {
                        Item.itemList_Warrior[0].Buy();
                    }
                    else if (Job.마법사.ToString() == player.job)
                    {
                        Item.itemList_Wizard[0].Buy();
                    }
                    else if (Job.궁수.ToString() == player.job)
                    {
                        Item.itemList_Archer[0].Buy();
                    }
                    break;
                case 2:
                    if (Job.전사.ToString() == player.job)
                    {
                        Item.itemList_Warrior[1].Buy();
                    }
                    else if (Job.마법사.ToString() == player.job)
                    {
                        Item.itemList_Wizard[1].Buy();
                    }
                    else if (Job.궁수.ToString() == player.job)
                    {
                        Item.itemList_Archer[1].Buy();
                    }
                    break;
                case 3:
                    if (Job.전사.ToString() == player.job)
                    {
                        Item.itemList_Warrior[2].Buy();
                    }
                    else if (Job.마법사.ToString() == player.job)
                    {
                        Item.itemList_Wizard[2].Buy();
                    }
                    else if (Job.궁수.ToString() == player.job)
                    {
                        Item.itemList_Archer[2].Buy();
                    }
                    break;
                case 4:
                    if (Job.전사.ToString() == player.job)
                    {
                        Item.itemList_Warrior[3].Buy();
                    }
                    else if (Job.마법사.ToString() == player.job)
                    {
                        Item.itemList_Wizard[3].Buy();
                    }
                    else if (Job.궁수.ToString() == player.job)
                    {
                        Item.itemList_Archer[3].Buy();
                    }
                    break;
                case 5:
                    if (Job.전사.ToString() == player.job)
                    {
                        Item.itemList_Warrior[4].Buy();
                    }
                    else if (Job.마법사.ToString() == player.job)
                    {
                        Item.itemList_Wizard[4].Buy();
                    }
                    else if (Job.궁수.ToString() == player.job)
                    {
                        Item.itemList_Archer[4].Buy();
                    }
                    break;
                case 6:
                    if (Job.전사.ToString() == player.job)
                    {
                        Item.itemList_Warrior[5].Buy();
                    }
                    else if (Job.마법사.ToString() == player.job)
                    {
                        Item.itemList_Wizard[5].Buy();
                    }
                    else if (Job.궁수.ToString() == player.job)
                    {
                        Item.itemList_Archer[5].Buy();
                    }
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
            Console.WriteLine("=======================================");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
            Console.WriteLine("=======================================");
            Console.WriteLine("  \n");
            Console.WriteLine($"상점 주인: 뭔가 팔 물건이라도 있나?");
            Console.WriteLine("==Sell==");

            int index = 1; //아이템 리스트 넘버링

            //내가 가진 물건 중에서만 판매

            Item.reciptItem.ForEach(item =>
            {
                Console.WriteLine($"{index++}. {item.EquipItem} {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.GetPrice * 0.8}G");
            });

            Console.WriteLine("판매할 아이템을 선택해주세요.");
            Console.WriteLine("0. 나가기\n ");

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

        static void SpecialStore()
        {
            Console.WriteLine("=======================================");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
            Console.WriteLine("=======================================");
            Console.WriteLine("  \n");
            Console.WriteLine($"상점 주인: 이거 아무한테나 보여주는 물건이 아니야.");
            Console.WriteLine("==Buy==");

            int index = 1; //아이템 리스트 넘버링
            Item.specialItemList.ForEach(item =>
            {
                Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
            });
            Console.WriteLine("구매할 아이템을 선택해주세요.");
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기\n ");
            Console.Write("입력:");
            int select = int.Parse(Console.ReadLine());
            if (select == 0)
            {
                Console.WriteLine("상점 주인: 다른 용건이 더 있나?");
                Store();
            }
            else if (select > 0 && select <= Item.specialItemList.Count)
            {
                Item.specialItemList[select - 1].Buy(); //구매 메서드 호출
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                SpecialStore();
            }
        }


        //여관
        static void Tarvern()
        {
            Console.WriteLine("여관설명");
            Console.WriteLine(" ");
            Console.WriteLine("==Tavern==");
            Console.WriteLine("1. 식사 주문");
            Console.WriteLine("2. 휴식/숙박");
            Console.WriteLine("3. 의뢰");
            Console.WriteLine("4. 대화");
            Console.WriteLine(" ");
            Console.WriteLine("원하는 행동을 선택해주세요.");
            Console.WriteLine("0. 나가기\n ");
            Console.Write("입력: ");
            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    Meal();
                    break;
                case 2:
                    Rest();
                    break;
                case 3:
                    Request();
                    break;
                case 4:
                    Console.WriteLine("여관 주인: 이야기? 좋지.");
                    Tarvern();
                    break;
                case 0:
                    GameStart(player);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Tarvern();
                    break;
            }


        }

        //식사 주문
        static void Meal()
        {
            Console.WriteLine("=======================================");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
            Console.WriteLine("=======================================");
            Console.WriteLine("여관 주인: 식사 한 명? 자, 여기 메뉴판이야.");
            Console.WriteLine("여관 주인: 메뉴가 정해지면 말해줘.");
            Console.WriteLine("==Menu==");
            //아이템 리스트

            Console.WriteLine(" ");
            Console.WriteLine("원하는 행동을 선택해주세요.");
            Console.WriteLine("0. 나가기\n ");
            Console.Write("입력: ");


            int select = int.Parse(Console.ReadLine());
        }

        //휴식/숙박
        static void Rest()
        {
            Console.WriteLine("여관 주인: 숙박? 마침 방이 비어있네.");
            Console.WriteLine("여관 주인: 방은 500G야.");
            Console.WriteLine("=======================================");
            Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
            Console.WriteLine("=======================================");
            Console.WriteLine("여관 주인: 방 하나 내어줄까?(Y/N)");
            string answer = Console.ReadLine();

            switch (answer)
            {
                case "Y":
                    if (player.gold >= 500)
                    {
                        Console.WriteLine("여관 주인: 방은 2층에 있어.");
                        Console.WriteLine("여관 주인: 편히 쉬어.");
                        player.gold -= 500; //골드 차감
                        player.hp = 100; //체력 회복
                        Console.WriteLine($"{player.name}의 체력이 회복되었습니다.");
                        Console.WriteLine("=======================================");
                        Console.WriteLine($"{player.name}님의 골드: {player.gold}G");
                        Console.WriteLine("=======================================");
                        Tarvern();
                    }
                    else
                    {
                        Console.WriteLine("여관 주인: 방을 빌리기엔 돈이 부족한 것 같은데?");
                        Console.WriteLine("여관 주인: 던전에 가서 돈을 벌어오거나, 의뢰를 받아보는 건 어때?");
                        Tarvern();
                    }
                    break;
                case "N":
                    Console.WriteLine("여관 주인: 뭐야, 그만 둘 거야?");
                    Tarvern();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Rest();
                    break;
            }
        }
        //의뢰
        static void Request()
        {
            Console.WriteLine("여관의 한쪽 벽에는 각종 의뢰서가 붙어있다.");
            Console.WriteLine("여관 주인: 의뢰를 받고 싶으면, 의뢰서를 나한테 가져와.");
            Console.WriteLine("여관 주인: 의뢰를 완료하면 보상을 줄게.");

            Console.WriteLine("==Request==");
            //의뢰 리스트 랜덤 생성


        }

        //던전 입장
        static void DungeonStart()
        {
            Console.WriteLine("여러 모험가들이 던전으로 향하고, 돌아오고 있다.");
            Console.WriteLine("그린티 마을의 던전은 세 곳으로 나뉘어져 있다.");
            Console.WriteLine("첫 모험가도 쉽게 공략할 수 있는 평원 던전, \n 자신을 어느정도 보호할 수 있어야 하는 바위산 던전, \n 마지막으로, 뜨거운 불길이 가득한 화산 던전이 있다.");
            Console.WriteLine(" ");
            Console.WriteLine("==Dungeon==");
            Console.WriteLine("1. 평원 던전: 권장 방어력 5");
            Console.WriteLine("2. 바위산 던전: 권장 방어력 11");
            Console.WriteLine("3. 화산 던전: 권장 방어력 17");


            Console.WriteLine(" ");
            Console.WriteLine("원하는 행동을 선택해주세요.");
            Console.WriteLine("0. 나가기\n ");
            Console.Write("입력: ");

            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    Console.WriteLine("==평원 던전==");
                    Console.WriteLine(" ");
                    Console.WriteLine("던전의 입구는 넓고, 바람이 시원하게 불어온다.");
                    Console.WriteLine("던전의 깊은 곳에서 괴물들의 울음소리가 들린다.");
                    Console.WriteLine(" ");
                    Console.WriteLine("평원 던전은 대부분 토끼, 사슴 등 초식 동물형 몬스터들이 많다.");
                    Console.WriteLine("그렇기에, 공격성이 낮아 초보자가 공략하기 쉽다.");


                    break;

                case 2:
                    Console.WriteLine("==바위산 던전==");
                    Console.WriteLine(" ");
                    Console.WriteLine("바위산은 좁고, 어두운 통로가 이어져 있다.");
                    Console.WriteLine("바위산의 깊은 곳에서 괴물들의 울음소리가 울려온다.");
                    break;
                case 3:
                    Console.WriteLine("화산 던전으로 입장합니다.");
                    Console.WriteLine(" ");
                    Console.WriteLine("화산 던전은 뜨거운 불길이 가득하다.");
                    Console.WriteLine("화산의 깊은 곳에서 들려오는 괴물들의 울음소리가 들린다.");
                    break;
                case 0:
                    Console.WriteLine("다시 마을로 돌아갑니다.");
                    GameStart(player);
                    break;
            }

        }

        /*일단 방어력을 비교한다.
        >총 방어력이 권장 방어력보다 낮으면 실패할 확률이 발생한다.
        >방어력이 높으면 성공-권장 방어력+-에 따라 종료시 체력 소모에 반영됨

        공격력을 비교한다.
        >공력력에 따라 +추가보상을 획득한다.
        
        경험치 획득
        >감소한 hp*10만큼 획득한다. (너무 쉬우면, 그만큼 경험치가 올라가지 않는다)*/

        //던전 공략 1. 평원 2. 바위산 3. 화산
        //던전 클래스-공략 메서드를 제작하는 게 나아보임       

    }
}
