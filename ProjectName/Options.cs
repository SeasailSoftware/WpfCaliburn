using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ProjectName
{
    class Options
    {
        public Options()
        {
            Culture = "zh-CN";
            Theme = "Light.Green";
        }
        public string Theme { get; set; }
        public string Culture { get; set; } = "zh-CN";

        internal static Options Load(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = "option.json";
            if (File.Exists(path))
            {
                var context = File.ReadAllText(path);
                var options = JsonConvert.DeserializeObject<Options>(context);
                return options;
            }
            return null;
        }

        internal void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = "option.json";
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json);
        }

    }

    public class SparkleStandard
    {
        public double[] CoarseDatas = new double[] { 1.031115, 10.44001, 7.937099, 5.777131, 2.276917 };
        public double[,] SIDatas = new double[,] { { 0.6543264, 2.2834695, 0.3543705, 1.394474, 1.394474, 0.3543705 }, { 27.15188, 28.770545, 30.38921, 17.87344, 17.87344, 30.38921 }, { 20.80567, 22.73347, 24.66127, 14.54644, 14.54644, 24.66127 }, { 15.66275, 17.5301, 19.39745, 8.98437, 8.98437, 19.39745 }, { 1.622577, 0.50434845, 4.814518, 13.15229, 13.15229, 4.814518 } };
        public double[,] SADatas = new double[,] { { 1.283938, 13.56558, 1.709936, 6.320302, 6.320302, 1.709936 }, { 11.09141, 17.06334, 23.03526, 37.57501, 37.57501, 23.03526 }, { 19.31447, 24.72829, 30.1421, 31.91361, 31.91361, 30.1421 }, { 24.18349, 27.54976, 30.91602, 21.63191, 21.63191, 30.91602 }, { 1.041667, 1.496937, 6.901041, 22.50792, 22.50792, 6.901041 } };
        public SparkleStandard()
        {
            var index = 0;
            foreach (var swatch in SwatchNames)
            {
                Coarse.Add(swatch, CoarseDatas[index]);
                SI.Add(swatch, new Dictionary<string, double>());
                SA.Add(swatch, new Dictionary<string, double>());
                var temp = 0;
                foreach (var angle in Angles)
                {
                    SI[swatch].Add(angle, SIDatas[index, temp]);
                    SA[swatch].Add(angle, SADatas[index, temp]);
                    temp++;
                }
                index++;
            }

        }

        public string[] SwatchNames => new string[]
        {
            "0#爱色丽蓝砖",
            "1#银浆板",
            "2#银浆板",
            "3#银浆板",
            "6#珍珠烤漆板"
        };

        public string[] Angles => new string[]
        {
            "15as-45",
            "15as-30",
             "15as-15",
            "15as15",
            "15as45",
            "15as80"
        };

        public Dictionary<string, Dictionary<string, double>> SI { get; set; } = new Dictionary<string, Dictionary<string, double>>();

        public Dictionary<string, Dictionary<string, double>> SA { get; set; } = new Dictionary<string, Dictionary<string, double>>();

        public Dictionary<string, double> Coarse { get; set; } = new Dictionary<string, double>();

    }
}
