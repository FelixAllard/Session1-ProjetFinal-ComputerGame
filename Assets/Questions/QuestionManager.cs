using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Questions
{
    public class QuestionManager : MonoBehaviour
    {
        private void Start()
        {
            
            
            
            
        }
        
        public static List<IQuestion> GetAllQuestions()
        {
            throw new NotImplementedException();
            /*List<IQuestion> questions = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass &&
                            !t.IsAbstract &&
                            typeof(IQuestion).IsAssignableFrom(t))
                .ToList();

            int id = 0;
            foreach (var question in questions)
            {
                question. = id++;
            }*/
        }

        public IQuestion GetRandomQuestion()
        {
            throw new NotImplementedException();
        }
    }
}