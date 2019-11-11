using bioTCache.Entitys;
using bioTCache.Firebase;
using bioTCache.Repositorys;
using bioTCache.Tests;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace bioTCache
{
    class EntityCache_Example
    {
        private readonly string              r_baseUrl = "https://biot-cec18.firebaseio.com/";
        private FirebaseDB                   m_Firebase;
        private FirebaseRepository           m_WeatherRepository;
        private FirebaseRepository           m_StudentRepository;
        private DbJsonSerializer             m_JsonSerializer;
        private EntityCache<WeatherForecast> m_WeatherCache;
        private EntityCache<Student>         m_StudentCache;

        public void Start()
        {
            Run();
            Console.ReadLine();
        }

        public Dictionary<string, WeatherForecast> makeWeather()
        {
            var w1 = new WeatherForecast()
            {
                Id = 101,
                Summary = "Saterday",
                TemperatureC = 30,
                Date = new DateTime(1992, 10, 14)
            };
            var w2 = new WeatherForecast()
            {
                Id = 102,
                Summary = "Sunday",
                TemperatureC = 33,
                Date = new DateTime(1992, 10, 15)
            };
            var w3 = new WeatherForecast()
            {
                Id = 103,
                Summary = "Monday",
                TemperatureC = 29,
                Date = new DateTime(1992, 10, 16)
            };
            var wd = new Dictionary<string, WeatherForecast>();
            wd.Add(w1.Id.ToString(), w1);
            wd.Add(w2.Id.ToString(), w2);
            wd.Add(w3.Id.ToString(), w3);

            return wd;
        }

        public Dictionary<string, Student> makeStudents()
        {
            var s1 = new Student()
            {
                Id = 101,
                FullName = "Ran Bardogo",
                AvarageGrades = 67,
                Birthdate = new DateTime(1994, 10, 16)
            };
            var s2 = new Student()
            {
                Id = 102,
                FullName = "Jane Gorila",
                AvarageGrades = 32,
                Birthdate = new DateTime(1997, 5, 16)
            };
            var s3 = new Student()
            {
                Id = 103,
                FullName = "Mitzdayen Dikan",
                AvarageGrades = 98,
                Birthdate = new DateTime(1995, 7, 20)
            };
            var sd = new Dictionary<string, Student>();

            sd.Add(s1.Id.ToString(), s1);
            sd.Add(s2.Id.ToString(), s2);
            sd.Add(s3.Id.ToString(), s3);
            return sd;

        }

        private void init()
        {
            m_Firebase = new FirebaseDB(r_baseUrl);
            m_WeatherRepository = new FirebaseRepository(m_Firebase.Node("Weather"));
            m_StudentRepository = new FirebaseRepository(m_Firebase.Node("Students"));
            m_JsonSerializer = new DbJsonSerializer();
            m_WeatherCache = new EntityCache<WeatherForecast>(m_WeatherRepository, m_JsonSerializer, eCacheLoadingMode.Eager);
            m_StudentCache = new EntityCache<Student>(m_StudentRepository, m_JsonSerializer, eCacheLoadingMode.Lazy);
        }

        private void addEventListiners()
        {
            m_WeatherCache.Event_AddCompleted += OnAddComplete_Weather;
            m_StudentCache.Event_AddCompleted += OnAddComplete_Student;
            m_StudentCache.Event_UpdateCompleted += OnUpdateComplete_Student;
            m_StudentCache.Event_RemoveCompleted += OnRemoveComplete_Student;
        }

        private void Run()
        {
            init();
            addEventListiners();
            var weatherDictionary = makeWeather();
            var studentDictonary = makeStudents();

            foreach (var x in weatherDictionary) { m_WeatherCache.Add(x.Value); };
            foreach (var x in studentDictonary) { m_StudentCache.Add(x.Value); };

            printAllStudents();
            printAllWeather();
            doGetUpdateAndRemove();

            runTest();
        }

        private  void runTest()
        {
            var tester = new TestCache<Student>(m_StudentCache, new Student()
            {
                Id = 999,
                Birthdate = DateTime.Now,
                AvarageGrades = 99,
                FullName = "Dummy Student"
            });

            tester.RunTests();
        }

        private void doGetUpdateAndRemove()
        {
            var ido = m_StudentCache.Get(203428453);
            ido.AvarageGrades = 60;
            m_StudentCache.Update(ido);
            m_StudentCache.Remove(103);
            Console.WriteLine();
            Console.WriteLine();
        }

        private void printAllStudents()
        {
            ImmutableDictionary<string, Student> students = m_StudentCache.GetAll();
            Console.WriteLine(students.ToString());
            Console.WriteLine(m_JsonSerializer.SerializePrettyPrint(students.Values.Where(val => val.AvarageGrades > 60)));
        }

        private void printAllWeather()
        {
            ImmutableDictionary<string, WeatherForecast> weatherForcasts = m_WeatherCache.GetAll();
            Console.WriteLine(weatherForcasts.ToString());
            Console.WriteLine(m_JsonSerializer.SerializePrettyPrint(weatherForcasts));
        }

        private void OnAddComplete_Weather(object sender, EventArgs e)
        {
            var args = e as CacheEventArgs;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Weather <" + args.Key + "> was added sucessfully!");
            Console.ForegroundColor = ConsoleColor.White;

        }

        private void OnAddComplete_Student(object sender, EventArgs e)
        {
            var args = e as CacheEventArgs;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Student <" + args.Key + "> was added sucessfully!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void OnUpdateComplete_Student(object sender, EventArgs e)
        {
            var args = e as CacheEventArgs;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Student <" + args.Key + "> was added Updated!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void OnRemoveComplete_Student(object sender, EventArgs e)
        {
            var args = e as CacheEventArgs;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Student <" + args.Key + "> was Removed!");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
