using Newtonsoft.Json;

public class ProfessionalStandard
{
    public string Content { get; set; }
}

public class ActivityDuration
{
    public string ActivityType { get; set; }
    public int Count { get; set; }
    public DateTime NextDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class Subrow
{
    public string Description { get; set; }
    public string UnitsCost { get; set; }
    public List<Competence> Competences { get; set; }
    public string Title { get; set; }
    public List<bool> Terms { get; set; }
    public string Index { get; set; }
}

public class EduPlan
{
    public Block1 Block1 { get; set; }
}
public class Block1
{
    public List<Subrow> Subrows { get; set; }
}

public class Competence
{
    public string Code { get; set; }
}

public class Category
{
    public string Title { get; set; }
}

public class Indicator
{
    public string Code { get; set; }
    public string Content { get; set; }
}

public class UniversalCompetencyRow
{
    public Competence Competence { get; set; }
    public Category Category { get; set; }
    public List<Indicator> Indicators { get; set; }
}

public class Section4
{
    public List<ProfessionalStandard> ProfessionalStandards { get; set; }
    public List<UniversalCompetencyRow> universalCompetencyRows { get; set; }
}

public class Section5
{
    public EduPlan EduPlan { get; set; }

    public calendarPlanTable calendarPlanTable { get; set; }
}

public class calendarPlanTable
{
    public List<Course> Courses { get; set; }
}

public class Course
{
    public List<string> WeekActivityIds { get; set; }
}


public class Content
{
    public Section4 Section4 { get; set; }

    public Section5 Section5 { get; set; }
}

public class EducationalProgram
{
    public Content Content { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "ProfessionalStandards.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        EducationalProgram program = JsonConvert.DeserializeObject<EducationalProgram>(jsonString);

        string targetCompetenceCode;
        string targetDisciplineIndex;
        int targetSemester;
        int targetCourse;

        string input;
        while (true)
        {
            Console.WriteLine("\nВыберите опцию:");
            Console.WriteLine("1. Информация о профессиональных стандартах");
            Console.WriteLine("2. Ввести код компетенции");
            Console.WriteLine("3. Ввести индекс дисциплины");
            Console.WriteLine("4. Ввести номер семестра");
            Console.WriteLine("5. Ввести номер курса");
            Console.WriteLine("6. Завершить программу");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    DisplayProfessionalStandards(program);
                    break;
                case "2":
                    Console.Write("Введите код компетенции: ");
                    targetCompetenceCode = Console.ReadLine();
                    DisplayCompetenceInfo(program, targetCompetenceCode);
                    break;

                case "3":
                    Console.Write("Введите индекс дисциплины: ");
                    targetDisciplineIndex = Console.ReadLine();
                    DisplayDisciplineInfo(program, targetDisciplineIndex);
                    break;

                case "4":
                    Console.Write("Введите номер семестра: ");
                    if (int.TryParse(Console.ReadLine(), out int semester))
                    {
                        DisplayDisciplinesInSemester(program, semester);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода. Введите целое число.");
                    }
                    break;

                case "5":
                    Console.Write("Введите номер курса: ");
                    if (int.TryParse(Console.ReadLine(), out int course))
                    {
                        DisplayWeekActivityInfo(program, course);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода. Введите целое число.");
                    }
                    break;

                case "6":
                    Console.WriteLine("Программа завершена.");
                    break;

                default:
                    Console.WriteLine("Некорректная опция. Пожалуйста, выберите существующую опцию.");
                    break;
            }

        } 
    }

    static void DisplayProfessionalStandards(EducationalProgram program)
    {
        Console.WriteLine("Код\tНазвание");
        foreach (var standard in program.Content.Section4.ProfessionalStandards)
        {
            string[] parts = standard.Content.Split(' ');
            if (parts.Length >= 2)
            {
                string code = parts[0];
                string content = string.Join(" ", parts.Skip(1));
                Console.WriteLine($"{code}\t{content}");
            }
            else
            {
                Console.WriteLine($"{standard.Content}");
            }
        }
    }

    static void DisplayCompetenceInfo(EducationalProgram program, string targetCompetenceCode)
    {
        Console.WriteLine($"\n\nИнформация для {targetCompetenceCode} компетенции:");
        Console.WriteLine($"Компетенция\t\tСодержание");
        if (program.Content.Section4.universalCompetencyRows != null)
        {
            foreach (var row in program.Content.Section4.universalCompetencyRows)
            {
                if (row.Competence != null && row.Competence.Code == targetCompetenceCode)
                {
                    Console.WriteLine($"{row.Competence.Code}\t\t{row.Category.Title}");
                    if (row.Indicators != null)
                    {
                        foreach (var indicator in row.Indicators)
                        {
                            Console.WriteLine($"{indicator.Code}\t\t{indicator.Content}");
                        }
                    }
                }
            }
        }
    }

    static void DisplayDisciplineInfo(EducationalProgram program, string targetDisciplineIndex)
    {
        var targetDiscipline = program.Content.Section5.EduPlan.Block1.Subrows.FirstOrDefault(d => d.Index == targetDisciplineIndex);


        Console.WriteLine($"\n\nИнформация по {targetDisciplineIndex}:");
        if (targetDiscipline != null)
        {
            Console.WriteLine($"{targetDiscipline.Index} {targetDiscipline.Title}");
            int indexOfFirstDot = targetDiscipline.Description.IndexOf('.');
            if (indexOfFirstDot != -1)
            {
                string truncatedDescription = targetDiscipline.Description.Substring(0, indexOfFirstDot + 1);
                Console.WriteLine($"Цель: {truncatedDescription}");
            }
            else
            {
                Console.WriteLine($"Цель: {targetDiscipline.Description}");
            }
            Console.WriteLine("Компетенции: " + string.Join(", ", targetDiscipline.Competences.Select(competence => competence.Code)));
            Console.WriteLine($"Зачетные единицы: {targetDiscipline.UnitsCost}");
            Console.WriteLine($"Семестры: {string.Join(" ", targetDiscipline.Terms.Select((term, index) => term ? (index + 1).ToString() : ""))}");
        }
        else
        {
            Console.WriteLine($"Дисциплина с индексом {targetDisciplineIndex} не найдена.");
        }
    }

    static void DisplayDisciplinesInSemester(EducationalProgram program, int targetSemester)
    {
        if (program.Content.Section5 != null && program.Content.Section5.EduPlan != null && program.Content.Section5.EduPlan.Block1 != null && program.Content.Section5.EduPlan.Block1.Subrows != null)
        {
            var disciplinesInTargetSemester = program.Content.Section5.EduPlan.Block1.Subrows
                .Where(d => d.Terms != null && d.Terms.Count >= targetSemester && d.Terms[targetSemester - 1])
                .Select(d => new { Index = d.Index, Title = d.Title });
            Console.WriteLine($"\n\nСписок дисциплин для {targetSemester} семестра:");
            if (disciplinesInTargetSemester.Any())
            {
                Console.WriteLine("Шифр\t\tНазвание дисциплины");
                foreach (var discipline in disciplinesInTargetSemester)
                {
                    Console.WriteLine($"{discipline.Index}\t\t{discipline.Title}");
                }
            }
            else
            {
                Console.WriteLine($"Дисциплины для семестра {targetSemester} не найдены.");
            }
        }
    }

    static void DisplayWeekActivityInfo(EducationalProgram program, int targetCourse)
    {
        List<string> weekActivityIds = program.Content.Section5.calendarPlanTable.Courses[targetCourse - 1].WeekActivityIds;
        List<ActivityDuration> durations = CalculateDurations(weekActivityIds);
        Console.WriteLine($"\n\nГрафик учебного процесса для {targetCourse} курса:");
        foreach (var duration in durations)
        {
            Console.WriteLine($"{duration.ActivityType}: Продолжительность {duration.NextDate} - {duration.EndDate}, Количество {duration.Count}");
        }
    }

    static List<ActivityDuration> CalculateDurations(List<string> weekActivityIds)
    {
        List<ActivityDuration> durations = new List<ActivityDuration>();
        string currentActivity = "";
        int currentCount = 0;
        DateTime nextDate = new DateTime(2020, 9, 1);

        foreach (var activity in weekActivityIds)
        {
            if (activity == currentActivity)
            {
                currentCount++;
            }
            else
            {
                if (!string.IsNullOrEmpty(currentActivity))
                {
                    var duration = new ActivityDuration
                    {
                        ActivityType = currentActivity,
                        Count = currentCount,
                        NextDate = nextDate.Month == 9 ? nextDate : nextDate.AddDays(-1)
                    };

                    duration.EndDate = nextDate.AddDays(currentCount * 7).AddDays(-3);
                    durations.Add(duration);

                    nextDate = nextDate.AddDays(currentCount * 7);
                }

                currentActivity = activity;
                currentCount = 1;
            }
        }

        if (!string.IsNullOrEmpty(currentActivity))
        {
            var duration = new ActivityDuration
            {
                ActivityType = currentActivity,
                Count = currentCount,
                NextDate = nextDate.AddDays(-1)
            };

            duration.EndDate = nextDate.AddDays(currentCount * 7).AddDays(-3);
            durations.Add(duration);
        }

        return durations;
    }
}
