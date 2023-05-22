using sppr2;

bool exit = false;
var abiturients = new List<Abiturient>();
var admittedApplicants = new List<Abiturient>();
while (!exit)
{
    Console.Clear();
    Console.WriteLine("Меню:");
    Console.WriteLine("1. Додати абітурієнта");
    Console.WriteLine("2. Видалити абітурієнта");
    Console.WriteLine("3. Створити список абітурієнтів");
    Console.WriteLine("4. Відібрати абітурієнтів");
    Console.WriteLine("5. Зберегти список зарахованих абітурієнтів у файл Excel");
    Console.WriteLine("6. Зберегти список зарахованих абітурієнтів у Базу даних");
    Console.WriteLine("7. Зчитати список абітурієнтів з бази даних");
    Console.WriteLine("8. Вийти");
    Console.WriteLine();

    Console.Write("Виберіть пункт меню: ");
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddApplicant();
            break;
        case "2":
            RemoveApplicant();
            break;
        case "3":
            abiturients.AddRange(Admission.CreateAbbiturients(5000));
            break;
        case "4":
            admittedApplicants = Admission.PassSelection(abiturients);
            break;
        case "5":
            var model = new AbiturientDocumentModel
            {
                Rows = admittedApplicants
            };
            Admission.WriteModelToExcel(model, @"Шаблон файлика", @"назва вихідного файлу");

            break;
        case "6":
            Admission.SaveToDb("шлях до файла \"бд\"", abiturients);
            break;
        case "7":
            Admission.GetApplicantsFromDb("шлях до файла \"бд\"", abiturients);
            break;
        case "8":
            exit = true;
            break;
        default:
            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
            break;
    }

    Console.WriteLine();
}

void AddApplicant()
{
    Console.WriteLine("Додавання абітурієнта");
    Console.WriteLine();

    // Отримуємо дані абітурієнта від користувача
    Console.Write("Введіть ім'я абітурієнта: ");
    var name = Console.ReadLine();
    Console.Write("Математика: ");
    var math = Console.ReadLine();
    Console.Write("Англ: ");
    var eng = Console.ReadLine();
    Console.Write("Укр: ");
    var ukr = Console.ReadLine();
    Console.Write("Пільга: ");
    var previlegy = Console.ReadLine();

    // Створюємо нового абітурієнта
    var abiturient = new Abiturient(abiturients.Count > 0 ? abiturients.Max(x => x.Id) + 1 : 0, name, int.Parse(math), int.Parse(eng), int.Parse(ukr),
        int.Parse(previlegy) != 0);

    // Додаємо абітурієнта до списку
    abiturients.Add(abiturient);

    Console.WriteLine("Абітурієнта успішно додано до списку.");
}

void RemoveApplicant()
{
    Console.WriteLine("Введіть Id Абітурієнта: ");
    var input = int.Parse(Console.ReadLine());
    if (abiturients.Select(x => x.Id).Contains(input))
        Console.WriteLine("Абітурієнта успішно видалено зі списку.");
    else
        Console.WriteLine("Абітурієнта не видалено зі списку.");

}