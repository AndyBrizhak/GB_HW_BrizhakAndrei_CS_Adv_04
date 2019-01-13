﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO; // для работы с потоками ввода-вывода
using System.Collections.Generic;


//Брижак Андрей Домашнее задание по курсу C# уровень 2 урок 4

namespace AsteroidsGame
{
    /// <summary>
    /// класс, где будут происходить все действия игры
    /// </summary>
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        /// <summary>
        /// статический таймер для класса Game
        /// </summary>
        private static Timer _timer = new Timer(); //+

        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        public static Random Rnd = new Random(); //+

        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        /// <summary>
        /// Максимальная высота и ширина экрана
        /// </summary>
        const int MaxH = 1000, MaxW = 1000;

        // <summary>
        /// массив объектов BaseObject
        /// </summary>
        public static BaseObject[] _objs;

        
        /// <summary>
        /// Коллекция пуль
        /// </summary>
        private static List<Bullet> _bullets = new List<Bullet>();

        ///// <summary>
        ///// массив объектов Asteroid
        ///// </summary>
        //private static Asteroid[] _asteroids;

        /// <summary>
        /// коллекция Астероидов
        /// </summary>
        private static List<Asteroid> _listAsteroids = new  List<Asteroid>();

        /// <summary>
        /// статитческий обьект Корабль +
        /// </summary>
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));

        private static Healthpack[] _healthpacks;
        static StreamWriter fileOut = new StreamWriter("t.txt", true);

        /// <summary>
        /// Стартовый уровень сложности/количество астероидов
        /// </summary>
        private static int StartLev = 1;

        /// <summary>
        /// Текущий уровень сложности/количество астероидов
        /// </summary>
        private static int CurLev;


        static Game()
        {
        }

        /// <summary>
        /// Инициализация сцены и обьектов
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            Width = form.Width;
            Height = form.Height;
            CheckSizeScreen(Width, Height);
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
           // Load();
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
            Bullet.MessageBulletDestroyed += Bullet.ShowMessageBulletDestroyed;
            Ship.LooseEnergy += Ship.ShowMessageShipLooseEnergy;
            Ship.AddEnergy += Ship.ShowMessageShipAddEnergy;


        }

        /// <summary>
        /// обработка событий Ctrl создаем снаряд, Up -сдвиг корабля вверх, Down -сдвиг корабля вниз+
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                //_bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1));
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1)));

            }
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }


        /// <summary>
        /// Обработчик таймера в котором вызываются Draw () и Update();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// вывод графики
        /// </summary>
        public static void Draw() //+
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();


            //CurLev++;
            //// проверка размера коллекции и добавление элементов 
            //if (_listAsteroids.Count == 0)
            //{
            //    AddAsteroidsList(Rnd, CurLev);
            //}

            foreach (Asteroid a in _listAsteroids) a?.Draw();  //прорисовка каждого существующего астероида в коллекции


            foreach (Bullet b in _bullets) b.Draw();

            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Bonus:" + _ship.Bonus, SystemFonts.DefaultFont, Brushes.YellowGreen, 100, 0);
            foreach (Healthpack obj in _healthpacks)
                obj?.Draw();

            Buffer.Render();
        }

        /// <summary>
        /// инициализация  объектов
        /// </summary>
        public static void Load()
        {
            _objs = new BaseObject[30];
            //_asteroids = new Asteroid[3];

            _healthpacks = new Healthpack[3];
            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(800, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }

            //for (var i = 0; i < _asteroids.Length; i++)
            //{
            //    int r = rnd.Next(5, 50);
            //    _asteroids[i] = new Asteroid(new Point(800, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new
            //        Size(r, r));
            //}


            AddAsteroidsList(Rnd, StartLev);// добавление астероида в коллекцию
            CurLev = StartLev;

            for (var i = 0; i < _healthpacks.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _healthpacks[i] = new Healthpack(new Point(800, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new
                    Size(r, r));
            }


        }

        /// <summary>
        /// Добавление астероидов в коллекцию
        /// </summary>
        /// <param name="rnd">ссылка на генератор случайных чисел для указания размера и местоположения</param>
        /// <param name="c">количество элементов в коллекции астероидов</param>
        private static void AddAsteroidsList(Random rnd, int c)
        {
            for (var i = 0; i < c; i++)  
            {
                int r = rnd.Next(5, 50);
                _listAsteroids.Add(new Asteroid(new Point(800, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));

            }
        }

        //private static void CheckSizeC

        /// <summary>
        /// изменения состояния объектов
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs) //+
                obj.Update();      //+

            
            foreach (Bullet b in _bullets) b?.Update();

            //for (var i = 0; i < _asteroids.Length; i++)    //+
            //{
            //    if (_asteroids[i] == null) continue;    //+
            //    _asteroids[i].Update();                 //+

            //    for (int j = 0; j < _bullets.Count; j++)
            //        if (_asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
            //        {
            //            _ship.BonusPlus(Rnd.Next(1, 10));
            //            _bullets[j].MessageDestroyed();
            //            System.Media.SystemSounds.Hand.Play();
            //            _asteroids[i] = null;
            //            _bullets.RemoveAt(j);
            //            j--;
            //        }

            //    if (_asteroids[i] == null || !_ship.Collision(_asteroids[i])) continue;
            //    _ship.LEnergy();
            //   _ship?.EnergyLow(Rnd.Next(1, 10));                      //+
            //    System.Media.SystemSounds.Asterisk.Play();              //+    
            //    if (_ship.Energy <= 0) _ship?.Die();                    //+
            //}

            
            // проверка размера коллекции и добавление элементов 
            if (_listAsteroids.Count == 0)
            {
                CurLev++;
                AddAsteroidsList(Rnd, CurLev);
            }
            //обработка коллекции астероидов
            for (int i = 0; i < _listAsteroids.Count; i++)
            {
                var asteroid = _listAsteroids[i];
                if (asteroid == null) continue;
                asteroid.Update();
                for (int j = 0; j < _bullets.Count; j++)
                    if (asteroid != null && _bullets[j].Collision(asteroid))
                    {
                        _ship.BonusPlus(Rnd.Next(1, 10));
                        _bullets[j].MessageDestroyed();
                        System.Media.SystemSounds.Hand.Play();
                       // asteroid = null;
                        _listAsteroids.Remove(asteroid);
                       _bullets.RemoveAt(j);
                        j--;
                    }

                if (asteroid == null || !_ship.Collision(asteroid)) continue;
                _ship.LEnergy();
                _ship?.EnergyLow(Rnd.Next(1, 10));                      //+
                System.Media.SystemSounds.Asterisk.Play();              //+    
                if (_ship.Energy <= 0) _ship?.Die();

            }

            for (var i = 0; i < _healthpacks.Length; i++)
            {
                if (_healthpacks[i] == null) continue;
                { _healthpacks[i].Update(); }

                if (!_ship.Collision(_healthpacks[i])) continue;
                _ship.AEnergy();
                {
                    var rnd = new Random();
                    _ship?.EnergyHigh(rnd.Next(1, 10));
                    System.Media.SystemSounds.Asterisk.Play();
                    _healthpacks[i] = null;
                }
            }


        }


        /// <summary>
        /// Проверка на задание размера экрана
        /// </summary>
        /// <param name="width">фактическая ширина экрана</param>
        /// <param name="height">фактическая высота экрана</param>
        public static void CheckSizeScreen(int width, int height)
        {
            if (width <= 0 || height <= 0 || width > MaxW || height > MaxH)
            {
                throw new ArgumentOutOfRangeException();

            }
        }

        /// <summary>
        /// Метод завершение игры
        /// </summary>
        public static void Finish()  //+
        {
            Console.WriteLine("The End");
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60,
                FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
            fileOut?.Close();
        }

        /// <summary>
        /// Запись сообщения в файл t.txt
        /// </summary>
        /// <param name="msg"></param>
        public static void MessageToFile(string msg)
        {
            
                fileOut?.WriteLine(msg);
            
            
            
            //try
            //{
                

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
        }
    }
}
