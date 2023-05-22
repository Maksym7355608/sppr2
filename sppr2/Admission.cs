using ClosedXML.Report;
using Newtonsoft.Json;

namespace sppr2
{
    internal class Admission
    {
        public static List<Abiturient> CreateAbbiturients(int count)
        {
            List<Abiturient> abbiturients = new(count);
            Random rnd = new();
            for (int i = 0; i < count; i++)
            {
                var benefit = rnd.Next(0, 4);
                abbiturients.Add(new Abiturient(i, "Вступник №" + (i + 1).ToString(), rnd.Next(100, 201), rnd.Next(100, 201), rnd.Next(100, 201), benefit > 2));
            }

            return abbiturients;
        }

        public static List<Abiturient> PassSelection(List<Abiturient> abiturients)
        {
            List<Abiturient> passedAbiturients = new();

            abiturients.Sort((a1, a2) => a2.Rating.CompareTo(a1.Rating));

            int privilegeCount = default;

            abiturients.ForEach(applicant =>
            {
                if (passedAbiturients.Count >= 350)
                    return;

                int maxBenefitiant = (int)(passedAbiturients.Count * 0.1);
                if (applicant.Benefit && maxBenefitiant >= passedAbiturients.Count(x => x.Benefit))
                {
                    if (applicant.Rating >= 144 && applicant.Math >= 120)
                    {
                        passedAbiturients.Add(applicant);
                        privilegeCount++;
                    }
                }
                else if (applicant.Rating >= 160 && applicant.Math >= 140)
                {
                    passedAbiturients.Add(applicant);
                }
            });

            return passedAbiturients;
        }

        public static void SaveToDb(string filePath, List<Abiturient> applicants)
        {
            string json = JsonConvert.SerializeObject(applicants, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        public static List<Abiturient> GetApplicantsFromDb(string filePath, List<Abiturient> applicants)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Abiturient>>(json ?? string.Empty) ?? new List<Abiturient>();

            } else
                return new List<Abiturient>();
        }

        public static void WriteModelToExcel<TModel>(TModel model, string templatePath, string filePath)
        {
            XLTemplate? template = new(templatePath);            
            template.AddVariable(model);            
            template.Generate();            
            template.SaveAs(filePath);
        }
    }

    public class Abiturient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Math { get; set; }
        public double English { get; set; }
        public double Ukrainian { get; set; }
        public bool Benefit { get; set; }
        public double Rating { get; set; }

        public Abiturient(int id, string name, double math, double english, double ukrainian, bool benefit)
        {
            Id = id;
            Name = name;
            Math = math;
            English = english;
            Ukrainian = ukrainian;
            Benefit = benefit;
            Rating = 0.4 * math + 0.3 * english + 0.3 * ukrainian;
        }
    }

    public class AbiturientDocumentModel
    {
        public List<Abiturient> Rows { get; set; }
    }
}
