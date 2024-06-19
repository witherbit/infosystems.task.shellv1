using DocumentFormat.OpenXml.Drawing;
using infosystems.task.shellv1.Forms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infosystems.task.shellv1.Forms.Tests
{
    public class IspdTest : ITest
    {
        public IEnumerable<TestElement> TestElements { get; }

        public IspdTest() 
        {
            TestElements = new List<TestElement>
            {
                new TestElement("Какое количество субъектов ПДн содержится в информационной системе?")
                .Insert("Более 100000")
                .Insert("Менее 100000")
                .Insert("Неограниченно (субъектами ПДн являются сотрудники оператора)"),
                new TestElement("Какой тип актуальных угроз характерен для Вашей организации?")
                .Insert("1 тип - Присутствие не декларированных возможностей в системном ПО")
                .Insert("2 тип - Присутствие не декларированных возможностей в прикладном ПО")
                .Insert("3 тип - Отсутствие не декларированных возможностей в ПО (угрозы в ПО, не связанные с первыми двумя типами)"),

                new TestElement("К каким последствиям может привести нарушение безопасности ПДн категории специальных данных, обрабатываемых в ИСПДн, для субъектов персональных данных?")
                .Insert("К значительным негативным последствиям")
                .Insert("К негативным последствиям")
                .Insert("К незначительным негавным последствиям")
                .Insert("Нарушение безопасности ПДн категории специальных данных не приводит к негавным последствиям"),
                new TestElement("К каким последствиям может привести нарушение безопасности ПДн категории биометрических данных, обрабатываемых в ИСПДн, для субъектов персональных данных?")
                .Insert("К значительным негативным последствиям")
                .Insert("К негативным последствиям")
                .Insert("К незначительным негавным последствиям")
                .Insert("Нарушение безопасности ПДн категории биометрических данных не приводит к негавным последствиям"),
                new TestElement("К каким последствиям может привести нарушение безопасности ПДн категории общедоступных данных, обрабатываемых в ИСПДн, для субъектов персональных данных?")
                .Insert("К значительным негативным последствиям")
                .Insert("К негативным последствиям")
                .Insert("К незначительным негавным последствиям")
                .Insert("Нарушение безопасности ПДн категории общедоступных данных не приводит к негавным последствиям"),
                new TestElement("К каким последствиям может привести нарушение безопасности ПДн категории иных данных, обрабатываемых в ИСПДн, для субъектов персональных данных?")
                .Insert("К значительным негативным последствиям")
                .Insert("К негативным последствиям")
                .Insert("К незначительным негавным последствиям")
                .Insert("Нарушение безопасности ПДн категории иных данных не приводит к негавным последствиям"),

                new TestElement("Соблюдается ли требование по соблюдению режима обеспечения безопасности помещений, в которых ведется обработка ПДн?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по сохранности носителей ПДн?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по наличию перечня лиц, допущенных к ПДн?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по использованию сертифицированных средств защиты?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по назначению должностного лица, ответственного за обеспечение безопсности ПДн в ИСПДн?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по ограничению доступа к электронным журналам только для сотрудников оператора?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по автоматической регистрации в электронном журнале безопасности изменения полномочий сотрудника оператора по доступу к ПДн?")
                .Insert("Да")
                .Insert("Нет"),

                new TestElement("Соблюдается ли требование по созданию либо возложению на одно из структурных подразделений функций по обеспечению безопасности ПДн?")
                .Insert("Да")
                .Insert("Нет"),
            };
        }

        public string GetResult()
        {
            var arr = TestElements.ToArray();
            var subjects = arr[0].SelectedAnswer + 1 == 1 ? 1 : 2;
            var type = arr[1].SelectedAnswer + 1;
            var warnSpecial = arr[2].SelectedAnswer + 1; //1 - самые тяжелые, 4 - нет
            var warnBiometric = arr[3].SelectedAnswer + 1; //1 - самые тяжелые, 4 - нет
            var warnSocial = arr[4].SelectedAnswer + 1; //1 - самые тяжелые, 4 - нет
            var warnOther = arr[5].SelectedAnswer + 1; //1 - самые тяжелые, 4 - нет
            var previewLevel = Math.Min(Math.Min(warnSocial, warnOther), Math.Min(warnSpecial, warnBiometric)); //УЗ предварительный

            var specialLevel = 1;
            var bioLevel = 1;
            var socialLevel = 1;
            var otherlLevel = 1;
            if (subjects == 1)
            {
                if (type == 1)
                {
                    specialLevel = 1;
                    bioLevel = 1;
                    socialLevel = 2;
                    otherlLevel = 1;
                }
                else if (type == 2)
                {
                    specialLevel = 1;
                    bioLevel = 2;
                    socialLevel = 2;
                    otherlLevel = 2;
                }
                else
                {
                    specialLevel = 2;
                    bioLevel = 3;
                    socialLevel = 4;
                    otherlLevel = 3;
                }
            }
            else
            {
                if (type == 1)
                {
                    specialLevel = 1;
                    bioLevel = 1;
                    socialLevel = 2;
                    otherlLevel = 1;
                }
                else if (type == 2)
                {
                    specialLevel = 2;
                    bioLevel = 2;
                    socialLevel = 3;
                    otherlLevel = 3;
                }
                else
                {
                    specialLevel = 3;
                    bioLevel = 3;
                    socialLevel = 4;
                    otherlLevel = 4;
                }
            }

            var levelGeneral = Math.Min(Math.Min(socialLevel, otherlLevel), Math.Min(specialLevel, bioLevel)); //УЗ общий

            bool s0 = arr[6].SelectedAnswer == 0 ? true : false;
            bool s1 = arr[7].SelectedAnswer == 0 ? true : false;
            bool s2 = arr[8].SelectedAnswer == 0 ? true : false;
            bool s3 = arr[9].SelectedAnswer == 0 ? true : false;
            bool s4 = arr[10].SelectedAnswer == 0 ? true : false;
            bool s5 = arr[11].SelectedAnswer == 0 ? true : false;
            bool s6 = arr[12].SelectedAnswer == 0 ? true : false;
            bool s7 = arr[13].SelectedAnswer == 0 ? true : false;

            var comparing = $"Текущий предполагаемый выявленный уровень защищенности ИСПДн не соответствует требуемому.";
            if (levelGeneral == 1)
            {
                if (s0 && s1 && s2 && s3 && s4 && s5 && s6 && s7)
                {
                    comparing = comparing.Replace("не соответствует", "соответствует");
                }
                else
                {
                    var reason = "Причины: ";
                    int c = 1;
                    for (int i = 6; i <= 13; i++)
                    {
                        if(arr[i].SelectedAnswer != 0)
                        {
                            reason += $"\r\n{c++} {arr[i].Question.Replace("Соблюдается ли", "Не соблюдается").Replace("?", "")};";
                        }
                    }
                    comparing += $"\r\n{reason}";
                }
            }
            else if (levelGeneral == 2)
            {
                if (s0 && s1 && s2 && s3 && s4 && s5)
                {
                    comparing = comparing.Replace("не соответствует", "соответствует");
                }
                else
                {
                    var reason = "Причины: ";
                    int c = 1;
                    for (int i = 6; i <= 11; i++)
                    {
                        if (arr[i].SelectedAnswer != 0)
                        {
                            reason += $"\r\n{c++} {arr[i].Question.Replace("Соблюдается ли", "Не соблюдается").Replace("?", "")};";
                        }
                    }
                    comparing += $"\r\n{reason}";
                }
            }
            else if (levelGeneral == 3)
            {
                if (s0 && s1 && s2 && s3 && s4)
                {
                    comparing = comparing.Replace("не соответствует", "соответствует");
                }
                else
                {
                    var reason = "Причины: ";
                    int c = 1;
                    for (int i = 6; i <= 10; i++)
                    {
                        if (arr[i].SelectedAnswer != 0)
                        {
                            reason += $"\r\n{c++} {arr[i].Question.Replace("Соблюдается ли", "Не соблюдается").Replace("?", "")};";
                        }
                    }
                    comparing += $"\r\n{reason}";
                }
            }
            else
            {
                if (s0 && s1 && s2 && s3)
                {
                    comparing = comparing.Replace("не соответствует", "соответствует");
                }
                else
                {
                    var reason = "Причины: ";
                    int c = 1;
                    for (int i = 6; i <= 9; i++)
                    {
                        if (arr[i].SelectedAnswer != 0)
                        {
                            reason += $"\r\n{c++} {arr[i].Question.Replace("Соблюдается ли", "Не соблюдается").Replace("?", "")};";
                        }
                    }
                    comparing += $"\r\n{reason}";
                }
            }

            return $"Определенный, в результате опросного метода, предполагаемый уровень защищенности ИСПДн: УЗ{levelGeneral}.\r\n" +
                $"{comparing}\r\n" +
                $"В зависимости от показателей зависимости от кол-ва субъектов, типа субъектов и актуальных угроз:\r\n" +
                $"Категория специальных данных: УЗ{specialLevel};\r\n" +
                $"Категория биометрических данных: УЗ{bioLevel};\r\n" +
                $"Категория общедоступных данных: УЗ{socialLevel};\r\n" +
                $"Категория иных данных: УЗ{otherlLevel}.\r\n" +
                $"Предварительный уровень защищенности (без дополнительных параметров): УЗ{previewLevel}.\r\n" +
                $"В зависимости от показателей зависимости категории от уровня значимости защищаемой информации:\r\n" +
                $"Категория специальных данных: УЗ{warnSpecial};\r\n" +
                $"Категория биометрических данных: УЗ{warnBiometric};\r\n" +
                $"Категория общедоступных данных: УЗ{warnSocial};\r\n" +
                $"Категория иных данных: УЗ{warnOther}.";
        }
    }
}
