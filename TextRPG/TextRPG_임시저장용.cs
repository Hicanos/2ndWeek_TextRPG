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
			//
            new Monster("슬라임", 1, 2, 0, 10, 5, 10),
            new Monster("박쥐", 2, 3, 1, 15, 10, 20),
            new Monster("스켈레톤", 3, 4, 2, 20, 15, 30),
            new Monster("좀비", 4, 5, 3, 25, 20, 40),
            new Monster("오크", 5, 6, 4, 30, 25, 50)
        };
		
		
Console.WriteLine("4. 필드");