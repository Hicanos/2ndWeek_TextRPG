// 상점에서 판매할 아이템을 정의하는 클래스
class Item
{
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public Item(string name, int price, string description)
    {
        Name = name;
        Price = price;
        Description = description;
    }
}
// 플레이어의 스탯을 정의하는 클래스
class Player
{
    public int Level { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int HP { get; set; }
    public int EXP { get; set; }
    public int Gold { get; set; }
    public Player(int level, int attack, int defense, int hp, int exp, int gold)
    {
        Level = level;
        Attack = attack;
        Defense = defense;
        HP = hp;
        EXP = exp;
        Gold = gold;
    }
}

// 인벤토리에서 아이템을 관리하는 클래스
class Inventory
{
    public List<Item> Items { get; set; }
    public Inventory()
    {
        Items = new List<Item>();
    }
    public void AddItem(Item item)
    {
        Items.Add(item);
    }
    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}




        //몬스터 클래스
        class Monster 
        {
            public string name;
            public int level;
            public int attack;
            public int defense;
            public int hp;
            public int exp;
            public int gold;

            //생성자
            public Monster(string name, int level, int attack, int defense, int hp, int exp, int gold)
            {
                this.name = name;
                this.level = level;
                this.attack = attack;
                this.defense = defense;
                this.hp = hp;
                this.exp = exp;
                this.gold = gold;
            }
            //몬스터 처치 메서드
            public void Kill()
            {
                Console.WriteLine($"{name}을(를) 처치하였습니다.");
                player.killCount += 1; //몬스터 처치 횟수 증가
                player.gold += gold; //골드 증가
                player.exp += exp; //경험치 증가
                player.LevelUp(); //레벨업 메서드 호출
            }
        }


        private static readonly List<Monster> monsterList = new List<Monster>()
        {
			//이름,레벨,공격력,방어력,hp,경험치,골드
            new Monster("슬라임", 1, 2, 0, 10, 5, 10),
            new Monster("박쥐", 2, 3, 1, 15, 10, 20),
            new Monster("스켈레톤", 3, 4, 2, 20, 15, 30),
            new Monster("좀비", 4, 5, 3, 25, 20, 40),
            new Monster("오크", 5, 6, 4, 30, 25, 50)
        };
		
		
Console.WriteLine("4. 필드");



if (player.hp <= 0)
{
    Console.WriteLine("공략은 성공했다. 하지만, 체력이 바닥났다.");
    Console.WriteLine("눈 앞이 깜깜해지고, 어지럽다.");
    Console.WriteLine("이대로 끝나는 건가?");
    Console.WriteLine("(Y/N)");

    string answer2 = Console.ReadLine();
    if (answer2 == "Y")
    {
        Console.WriteLine("눈을 뜨자, 주변의 소음이 들려온다.");
        Console.WriteLine("몸은 푹신한 침대 위에 누워있다.");
        Console.WriteLine("여관 주인: 아, 드디어 일어났네. 몸은 좀 괜찮아?");
        Console.WriteLine("여관 주인: 던전에서 쓰러진 걸, 다른 모험가들이 발견하고 데려왔어.");
        Console.WriteLine("여관 주인: 이번은 무사해서 다행이지만, 다음부터는 조심해.");
        Console.WriteLine("여관 주인: 방에 치료비까지 다해서 1000G야.");
        Console.WriteLine("여관 주인: 나도 먹고 살아야지.");
        Console.WriteLine(" ");
        Console.WriteLine($"{player.name}의 체력이 모두 회복되었습니다.");
        Console.WriteLine("여관 주인: 다음부터는 무리하지 마.");
        Console.WriteLine("여관 주인: 아, 이건 네가 가지고 있던 물건이야.");
        player.hp = 100; //체력 회복

        player.gold -= 1000; //골드 차감
                             //보상은 그대로 획득할 것-공략을 완료했으므로, 마이너스가 되어도 다시 회복됨
    }
    else if (answer2 == "N")
    {
        Console.WriteLine("당신의 여정은 여기서 끝나고 말았다.");
        Console.WriteLine("당신의 물건은 또 다른 누군가에게 발견되었다.");
        Console.WriteLine("그 물건은, 다른 모험가의 여정으로 이어지겠지.");
        Console.WriteLine("===================================");
        Console.WriteLine("게임을 종료합니다.");
        Environment.Exit(0);
        break;
    }
    else
    {
        Console.WriteLine("잘못된 입력입니다.");
        Console.WriteLine("게임을 종료합니다.");
        Environment.Exit(0);
        break;
    }


}




 static void PlainDungeon()
 {
     Console.WriteLine("평원 던전의 권장 방어력은 5입니다.");
     Console.WriteLine("입장하시겠습니까?(Y/N)");
     string answer = Console.ReadLine();

     switch (answer)
     {
         case "Y":

             Console.WriteLine("던전 공략을 시작합니다.");
             if (player.defense + player.itemDefense >= 5)
             {
                 Console.WriteLine("던전 공략에 성공했습니다.");
                 //체력 소모 값=20~35에 방어력에 따른 보정치 발생
                 int minusHP = new Random().Next(20, 36);
                 player.hp -= (int)(minusHP - ((player.defense + player.itemDefense) - 5));
                 
                 //보상은 공격력~공격력*2 %까지 보정됨 (공격력 10이면 10%~20%까지 보정됨)
                 float rewardPercent = player.attack + player.itemAttack;
                 int reward = new Random().Next((int)rewardPercent, (int)rewardPercent * 2 + 1);
                 int rewardGold = (int)(1000 + 1000 * (reward * 0.01f));
                 player.gold += rewardGold;
                 player.exp += (int)(minusHP * 10); //경험치 획득



                 Console.WriteLine("==던전 결과==");
                 Console.WriteLine($"획득 Gold:{rewardGold}");
                 Console.WriteLine($"획득 경험치:{(int)(minusHP * 10)}");
                 player.LevelUp(); //레벨업 체크

                 DungeonStart();

             }
             else
             {
                 int clear = new Random().Next(1, 101);
                 if (clear <= 40)
                 {
                     Console.WriteLine("던전 공략에 실패했습니다.");
                     Console.WriteLine($"체력이 {50 + 5 - (player.defense + player.itemDefense)}감소합니다.");
                     player.hp -= (int)(50 + 5 - (player.defense + player.itemDefense));
                     Console.WriteLine($"현재 체력: {player.hp}");  
                 }
                 else
                 {
                     Console.WriteLine("던전 공략에 성공했습니다.");
                     //체력 소모 값=20~35에 방어력에 따른 보정치 발생
                     int minusHP = new Random().Next(20, 36);
                     player.hp -= (int)(minusHP - ((player.defense + player.itemDefense) - 5));

                     //보상은 공격력~공격력*2 %까지 보정됨 (공격력 10이면 10%~20%까지 보정됨)
                     float rewardPercent = player.attack + player.itemAttack;
                     int reward = new Random().Next((int)rewardPercent, (int)rewardPercent * 2 + 1);
                     int rewardGold = (int)(1000 + 1000 * (reward * 0.01f));
                     player.gold += rewardGold;
                     player.exp += (int)(minusHP * 10); //경험치 획득



                     Console.WriteLine("==던전 결과==");
                     Console.WriteLine($"획득 Gold:{rewardGold}");
                     Console.WriteLine($"획득 경험치:{(int)(minusHP * 10)}");
                     player.LevelUp(); //레벨업 체크

                     DungeonStart();
                 }
             }
             break;

         case "N":
             Console.WriteLine("던전 공략을 그만둡니다.");
             DungeonStart();
             break;

     }

 }
 
 
 
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
 
 
 
 
 
   private static void DisplayItemList(List<Item> items)
  {
      int index = 1;
      items.ForEach(item =>
      {
          Console.WriteLine($"{index++}. {item.Name} \t| 공격력 +{item.Attack} \t| 방어력 +{item.Defense} \t|{item.Description}\t|가격: {item.Price}");
      });
  }