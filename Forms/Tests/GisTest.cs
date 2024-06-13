using infosystems.task.shellv1.Forms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infosystems.task.shellv1.Forms.Tests
{
    public class GisTest : ITest
    {
        public IEnumerable<TestElement> TestElements { get; }

        public GisTest() 
        {
            TestElements = new List<TestElement>
            {
                new TestElement("Выберите масштаб ГИС")
                .Insert("Федеральный")
                .Insert("Региональный")
                .Insert("Объектовый"),
                new TestElement("Какие негативные последствия в социальной, политической, международной, экономической, финансовой или иных областях деятельности возможны в результате нарушения конфиденциальности, целостности и (или) доступности защищаемой информации")
                .Insert("Существенные")
                .Insert("Умеренные")
                .Insert("Незначительные или отсутствуют"),
                new TestElement("Какие функции, в результате нарушения конфиденциальности, целостности и (или) доступности защищаемой информации, не может выполнять оператор и (или) информационная система")
                .Insert("Все функции")
                .Insert("Одну или несколько функций")
                .Insert("Оператор и (или) ИС может выполнить одну или несколько функций, включая выполнение функций с привлечением дополнительных сил и средств"),
            };
        }
        public string GetResult()
        {
            var arr = TestElements.ToArray();
            var significanceLevel0 = arr[1].SelectedAnswer + 1;
            var significanceLevel1 = arr[2].SelectedAnswer + 1;
            var scale = arr[0].SelectedAnswer;
            var significanceLevel = Math.Min(significanceLevel0, significanceLevel1);
            if(scale == 0)
            {
                return GetFederalSecClass(significanceLevel);
            }
            else if (scale == 1)
            {
                return GetRegionSecClass(significanceLevel);
            }
            else
            {
                return GetObjectSecClass(significanceLevel);
            }
        }

        private string GetFederalSecClass(int significanceLevel)
        {
            if(significanceLevel == 1)
            {
                return $"К1";
            }
            else if(significanceLevel == 2)
            {
                return $"К1";
            }
            else
            {
                return $"К2";
            }
        }
        private string GetRegionSecClass(int significanceLevel)
        {
            if (significanceLevel == 1)
            {
                return $"К1";
            }
            else if (significanceLevel == 2)
            {
                return $"К2";
            }
            else
            {
                return $"К3";
            }
        }
        private string GetObjectSecClass(int significanceLevel)
        {
            if (significanceLevel == 1)
            {
                return $"К1";
            }
            else if (significanceLevel == 2)
            {
                return $"К2";
            }
            else
            {
                return $"К3";
            }
        }
    }
}
