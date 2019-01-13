﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

//Брижак Андрей Домашнее задание к уроку 4 Продвинутый курс C#

namespace AsteroidsGame
{
    class Ship : BaseObject
    {
        /// <summary>
        /// Энергия корабля
        /// </summary>
        private int _energy = 100;
        public int Energy => _energy;

        /// <summary>
        /// init bonus 
        /// </summary>
        private int _bonus = 0;
        public int Bonus => _bonus;

        /// <summary>
        /// конструктор создания корабля
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }



        /// <summary>
        ///  event (делегат) Гибель корабля
        /// </summary>
        public static event Message MessageDie;  //+

        /// <summary>
        ///  событие потеря энергии кораблем
        /// </summary>
        public static event Message LooseEnergy;

        /// <summary>
        /// событие (делегат) добавление энергии корабля
        /// </summary>
        public static event Message AddEnergy;


        /// <summary>
        /// add bonus energy
        /// </summary>
        /// <param name="n"></param>
        public void BonusPlus(int n)
        {
            _bonus += n;
        }

        /// <summary>
        /// снижение энергии корабля
        /// </summary>
        /// <param name="n"></param>
        public void EnergyLow(int n)
        {
            _energy -= n;
        }


        /// <summary>
        /// add energy high
        /// </summary>
        /// <param name="n"></param>
        public void EnergyHigh(int n)
        {
            _energy += n;
        }

        /// <summary>
        /// Когда корабль теряет энергию вызываем это событие:
        /// </summary>
        public void LEnergy()
        {
            LooseEnergy?.Invoke();

        }

        /// <summary>
        /// Когда корабль добавляет энергию вызываем это событие:
        /// </summary>
        public void AEnergy()
        {
            AddEnergy?.Invoke();

        }

        /// <summary>
        /// рисуем корабль
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
            //Image newImage = Image.FromFile("space-shuttle.png");
            //Game.Buffer.Graphics.DrawImage(newImage, Pos.X, Pos.Y, (Size.Width*2), (Size.Height*2));
        }

        public override void Update()
        {
        }

        /// <summary>
        /// сдвинуть вверх позицию корабля
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        /// <summary>
        /// сдвинуть вниз позицию корабля
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        /// <summary>
        /// Когда корабль погибает вызываем событие Die starship 
        /// </summary>
        public void Die()
        {
            MessageDie?.Invoke();   //+
            Console.WriteLine("Starship died!"); // Вывод сообщения в консоль
                //Game.MessageToFile(" Starship died! ");

        }

        public static void ShowMessageShipLooseEnergy()
        {
            Console.WriteLine("Starship loosing energy!!!");
            Game.MessageToFile(" Starship loosing energy!!! ");
        }

        public static void ShowMessageShipAddEnergy()
        {
            Console.WriteLine("Starship add energy!!!");
            Game.MessageToFile(" Starship add energy!!! ");
        }

    }
}

