using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Models;

namespace Tree
{
    public class OptionsParser
    {

        private List<Option> _options = new List<Option>();

        /// <summary>
        /// Возвращает значение опции как целое
        /// </summary>
        /// <param name="shortName">Короткое имя опции</param>
        /// <param name="name">Полное имя опции</param>
        /// <returns>Возвращает значение опции как целое</returns>
        public int GetOptionAsInt(string shortName, string name, int defaultValue = 0)
        {
            Option option = _options.FirstOrDefault(o => o.Name == name || o.Name == shortName);
            if (option == null)
                return defaultValue;
            try
            {
                int result = int.Parse(option.Value);
                return result;
            }
            catch
            {
                throw new FormatException($"Не верно задан аргумент ключа {shortName} или {name}");
            }
        }


        /// <summary>
        /// Возвращает значение опции как строку
        /// </summary>
        /// <param name="shortName">Короткое имя опции</param>
        /// <param name="name">Полное имя опции</param>
        /// <returns>Возвращает значение опции как строку</returns>
        public string GetOptionAsString(string shortName, string name)
        {
            Option option = _options.FirstOrDefault(o => o.Name == name || o.Name == shortName);
            return option == null ? string.Empty : option.Value;            
        }


        /// <summary>
        /// Возвращает значение опции как bool
        /// </summary>
        /// <param name="shortName">Короткое имя опции</param>
        /// <param name="name">Полное имя опции</param>
        /// <returns>Возвращает значение опции как bool</returns>
        public bool GetOptionAsBool(string shortName, string name)
        {
            Option option = _options.FirstOrDefault(o => o.Name == name || o.Name == shortName);
            return option == null ? false : true;                
        }

        /// <summary>
        /// Парсинг параметров
        /// </summary>
        /// <param name="args">Массив параметров парсинга</param>
        public void Parse(string[] args)
        {

            string keyName;
            foreach (string arg in args)
            {
                Option option = new Option();
                string[] splitData = arg.Split('=');
                // Ключ с аргументом
                if (splitData.Length == 2)
                {
                    keyName = ResolveKeyName(splitData[0]);
                    if (string.IsNullOrEmpty(keyName))                    
                        continue;
                    else
                    {
                        option.Name = keyName;
                        option.Value = splitData[1];
                    }                    
                }

                // Ключ без аргумента. Как флаг
                if (splitData.Length == 1)
                {
                    keyName = ResolveKeyName(splitData[0]);
                    if (string.IsNullOrEmpty(keyName))
                        continue;
                    option.Name = keyName;
                    option.Value = "True";
                }                
                _options.Add(option);
            }          
        }

        /// <summary>
        ///  Метод убирает "-" или "--" с начала строки
        /// </summary>
        /// <param name="key">Исходный ключ</param>
        /// <returns>Возвращает имя ключа, или пустую строку, если имя ключа не удалось разобрать.</returns>
        /// <remarks>
        /// Ключ должен начинаться с "-" или "--", иначе считаем ошибкой
        /// </remarks>
        private string ResolveKeyName(string key)
        {
            string result = string.Empty;
            if (key.IndexOf("--") == 0)
                result = key.Substring(2);
            else if (key.IndexOf("-") == 0)
                result = key.Substring(1);
            return result;
        }
    }
}
