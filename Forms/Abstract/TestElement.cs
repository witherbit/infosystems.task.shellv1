using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infosystems.task.shellv1.Forms.Abstract
{
    public class TestElement
    {
        public string Question { get; }
        public List<string> Answers { get; }
        public int SelectedAnswer { get; set; }
        public TestElement(string question) 
        {
            Question = question;
            Answers = new List<string>();
            SelectedAnswer = -1;
        }

        public TestElement Insert(string answer)
        {
            Answers.Add(answer);
            return this;
        }

        public TestElement Select(string answer)
        {
            if(Answers.Contains(answer))
                SelectedAnswer = Answers.IndexOf(answer);
            return this;
        }
    }
}
