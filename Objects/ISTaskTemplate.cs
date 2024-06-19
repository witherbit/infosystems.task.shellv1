using infosystems.task.shellv1.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.Statistic;
using wcheck.Statistic.Items;
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Styles;
using wcheck.Statistic.Templates;
using pwither.formatter;

namespace infosystems.task.shellv1.Objects
{
    [BitSerializable]
    public class ISTaskTemplate : IStatisticTemplate
    {
        public List<IStatisticNode> Nodes { get; }

        public TextNodeStyle HeaderStyle => new TextNodeStyle
        {
            SpacingBetweenLines = 2,
            FontSize = 14,
            IsBold = true,
            WpfFontSize = 16,
            Aligment = wcheck.Statistic.Enums.TextAligment.Center
        };

        public string? Header => "Отчет по тестированию ИС";

        public bool UseBreakAfterTemplate => true;

        public ISTaskTemplate(ISType type, string formsResult)
        {
            var iStype = string.Empty;
            switch (type)
            {
                case Enums.ISType.GIS:
                    iStype = "ГИС";
                    break;
                case Enums.ISType.ISPD:
                    iStype = "ИСПДн";
                    break;
                case Enums.ISType.AS:
                    iStype = "АС";
                    break;
            }
            Nodes = new List<IStatisticNode>
            {
                new TextStatisticNode(),
                new TextStatisticNode($"Тип тестируемой ИС: {iStype}", new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                    FontSize = 12,
                    WpfFontSize= 14,
                    SpacingBetweenLines = 1.5
                }),
                new TextStatisticNode(),
                new TextStatisticNode($"Дата и время отчета: {DateTime.Now.ToString()}", new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                    FontSize = 10,
                    WpfFontSize= 12,
                    IsItalic = true,
                }),
                new TextStatisticNode($"по UTC: {DateTime.UtcNow.ToString("HH:mm:ss")}", new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                    FontSize = 10,
                    WpfFontSize= 12,
                    IsItalic = true,
                    WpfMargin = new WpfThinkness(5, 0, 5 ,5)
                }),
                new TextStatisticNode(),
                new TextStatisticNode($"Оценочные результаты тестирования опросным методом", new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Center,
                    FontSize = 14,
                    WpfFontSize= 16,
                    IsBold = true,
                    WpfMargin = new WpfThinkness(5, 0, 5 ,5),
                    SpacingBetweenLines = 1.5
                }),
                new TextStatisticNode(),
            };

            foreach (var str in formsResult.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                Nodes.Add(new TextStatisticNode(str, new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Both,
                    FontSize = 12,
                    WpfFontSize = 14,
                    WpfMargin = new WpfThinkness(5, 0, 5, 5),
                    SpacingBetweenLines = 1.5
                }));
        }
    }
}
