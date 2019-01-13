using System.Windows.Forms;
//Брижак Андрей Домашнее задание по курсу C# уровень 2 урок 4

namespace AsteroidsGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            
            Game.Init(form);
            form.Show();
            Game.Load();
            Game.Draw();
            Application.Run(form);
        }
    }
}
