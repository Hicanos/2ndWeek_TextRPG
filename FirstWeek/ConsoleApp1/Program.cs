namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("원하는 프로그램을 선택하세요.");
            Console.WriteLine("1. 이름과 나이 입력하기");
            Console.WriteLine("2. 사칙연산하기");
            Console.WriteLine("3. 화씨로 변환하기");
            Console.WriteLine("4. BMI 계산하기");
            Console.WriteLine("5. 종료하기");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    InputNameAndAge();
                    break;
                case "2":
                    calculate();
                    break;
                case "3":
                    CelsiusToFahrenheit();
                    break;
                case "4":
                    BMI();
                    break;
                case "5":
                    Console.WriteLine("프로그램을 종료합니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다.");
                    Main(args); // 재귀 호출로 다시 시작
                    break;
            }
        }

        static void InputNameAndAge()
        {
            Console.WriteLine("이름을 입력하세요: ");
            string name = Console.ReadLine();
            Console.WriteLine("나이를 입력하세요: ");
            bool isValidAge;
            string age;

            do
            {
                age = Console.ReadLine();

                int ageInt;
                isValidAge = int.TryParse(age, out ageInt);

                if (!isValidAge)
                {
                    Console.WriteLine("유효하지 않은 나이입니다. 다시 입력하세요.");
                    continue;
                }
                else
                {
                    if (ageInt < 0 || ageInt > 120)
                    {
                        Console.WriteLine("유효하지 않은 나이입니다. 다시 입력하세요.");
                        isValidAge = false;
                    }
                    else
                    {
                        isValidAge = true;
                    }
                }
            } while (!isValidAge);

            Console.WriteLine("당신이 입력한 정보는 다음과 같습니다.");
            Console.WriteLine("이름: " + name);
            Console.WriteLine("나이: " + age);

            Console.WriteLine(" ");

            Select();
        }

        static void calculate()
        {
            Console.WriteLine("사칙연산을 시작합니다.");
            Console.WriteLine("두 개의 숫자를 입력하세요: ");
            Console.WriteLine("첫 번째 숫자: ");
            int num1 = int.Parse(Console.ReadLine());
            Console.WriteLine("두 번째 숫자: ");
            int num2 = int.Parse(Console.ReadLine());
            
            Console.WriteLine("덧셈 결과: " + (num1 + num2));
            Console.WriteLine("뺄셈 결과: " + (num1 - num2));           string resultMultiply = "곱셈 결과: " + (num1 * num2);
            Console.WriteLine("나눗셈 결과: " + (num1 / num2));
            Console.WriteLine("나머지 결과: " + (num1 % num2));

            Console.WriteLine(" ");

            Select();

        }

        static void CelsiusToFahrenheit()
        {
            Console.WriteLine("섭씨를 화씨로 변환합니다.");
            Console.WriteLine("섭씨 온도를 입력하세요: ");
            float celsius = float.Parse(Console.ReadLine());
            float fahrenheit = (celsius * 9 / 5f) + 32;
            Console.WriteLine("화씨 온도: " + fahrenheit);

            Console.WriteLine(" ");

            Select();
        }

        static void BMI()
        {
            Console.WriteLine("BMI 계산을 시작합니다.");
            Console.WriteLine("체중(kg)을 입력하세요: ");
            float weight = float.Parse(Console.ReadLine());
            Console.WriteLine("신장(cm)을 입력하세요: ");
            float height = float.Parse(Console.ReadLine());
            float bmi = weight / ((height* 0.01f) * (height*0.01f));
            Console.WriteLine("당신의 BMI는: " + bmi);
            if (bmi < 18.5)
            {
                Console.WriteLine("저체중입니다.");
            }
            else if (bmi >= 18.5 && bmi < 23)
            {
                Console.WriteLine("정상 체중입니다.");
            }
            else if (bmi >= 23 && bmi < 25)
            {
                Console.WriteLine("비만전단계입니다.");
            }
            else if (bmi >= 25 && bmi < 30)
            {
                Console.WriteLine("1단계 비만입니다.");
            }
            else if (bmi >= 30 && bmi < 35)
            {
                Console.WriteLine("2단계 비만입니다.");
            }
            else
            {
                Console.WriteLine("3단계 비만입니다.");
            }

            Console.WriteLine(" ");
            Console.WriteLine("계산 기준: 대한비만학회 제공 \"비만 치료지침(2022 8판)\"");
            Console.WriteLine(" ");

            Select();


        }

        static void Select()
        {
            Console.WriteLine("원하는 조작을 선택하세요");
            Console.WriteLine("1. 메인메뉴로 돌아가기");
            Console.WriteLine("2. 종료하기");

            string select = Console.ReadLine();


            switch (select)
            {
                case "1":
                    Main(new string[0]);
                    break;
                case "2":
                    Console.WriteLine("프로그램을 종료합니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다.");
                    Select();
                    break;
            }
        }
    }
}
