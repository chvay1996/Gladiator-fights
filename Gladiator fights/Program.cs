using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main ( string [] args )
        {
            Arena arena = new Arena ();
            arena.Work ();
        }
    }

    class Arena
    {
        private List<Fighter> _fighters = new List<Fighter> ();
        private Fighter _leftFighter;
        private Fighter _rightFighter;

        public Arena ()
        {
            _fighters.Add ( new Warrior ( "Варвар", 300, 60, 0 ) );
            _fighters.Add ( new Paladin ( "Паладин", 150, 30, 30 ) );
            _fighters.Add ( new Hunter ( "Охотник", 130, 50, 15 ) );
            _fighters.Add ( new Berserck ( "Берсерк", 200, 60, 5 ) );
            _fighters.Add ( new Magician ( "Маг", 100, 50, 10 ) );
        }

        public void Work ()
        {
            bool isWork = true;

            while ( isWork == true)
            {
                Console.Write ( "Выяви какой боец сильнее!" +
    "\n1 - Выбрать бойцов" +
    "\n2 - Посмотреть всех бойцов" +
    "\n3 - Выход" +
    "\nВыберете пункт: \t" );

                switch ( int.Parse ( Console.ReadLine () ) )
                {
                    case 1:
                        WorkArena ();
                        break;
                    case 2:
                        ShowInfo ();
                        break;
                    case 3:
                        isWork = false;
                        break;
                    default:
                        Console.WriteLine ( "Неверно указан пункт!" );
                        break;
                }
                Console.ReadLine ();
                Console.Clear ();

            }
        }

        private void WorkArena ()
        {
            Winner ();
            ChooseAFighter ();
            Battle ();
            ShowBattleResult ();
        }

        private void Winner ( )
        {
            if ( _fighters.Count < 1 )
            {
                Console.WriteLine ( $"Победил боец \n{_fighters [ 0 ]}" );
                _fighters.RemoveAt ( 0 );
            }

        }

        private void ChooseAFighter ()
        {
            ShowInfo ();
            string message;
            message = "\nВведите индекс бойца левого угла:";
            SelectPlayer ( message, out _leftFighter );
            Console.Clear ();
            ShowInfo ();
            message = "\nВведите индекс бойца правого угла:";
            SelectPlayer ( message, out _rightFighter );
        }

        private void ShowBattleResult ()
        {
            if ( _leftFighter.Health <= 0 && _rightFighter.Health <= 0 )
            {
                Console.WriteLine ( "Ничья, оба бойца погибли!" );
            }
            else if ( _leftFighter.Health <= 0 )
            {
                Console.WriteLine ( $"Победил боец правых ворот - {_rightFighter.Name}. " );
                _fighters.Remove ( _leftFighter );
            }
            else if ( _rightFighter.Health <= 0 )
            {
                Console.WriteLine ( $"Победил боец левых ворот - {_leftFighter.Name}. " );
                _fighters.Remove ( _rightFighter );
            }
        }

        private void Battle ()
        {
            while ( _leftFighter.Health > 0 && _rightFighter.Health > 0 )
            {
                _leftFighter.ShowStats ();
                _rightFighter.ShowStats ();
                Console.WriteLine ();
                _leftFighter.TakeDamage ( _rightFighter.Damage );
                _rightFighter.TakeDamage ( _leftFighter.Damage );
                _leftFighter.UseAnAttack ();
                _rightFighter.UseAnAttack ();
            }
        }

            private void SelectPlayer ( string message, out Fighter fighter )
        {
            string userInput;
            bool isNumber;
            Console.Write ( message );
            userInput = Console.ReadLine ();
            isNumber = int.TryParse ( userInput, out int indexFighter );

            if ( isNumber == false )
            {
                Console.WriteLine ( "Ошибка! Не верный ввод!" );
                fighter = null;
            }
            else if ( indexFighter - 1 < _fighters.Count && indexFighter > 0 )
            {
                fighter = _fighters [ indexFighter - 1 ];
                Console.WriteLine ( "Боец выбран" );
            }
            else
            {
                Console.WriteLine ( "Такого бойца нету!" );
                fighter = null;
            }
        }

        private void ShowInfo ()
        {
            Console.WriteLine ( "Все бойцы:" );
            for ( int i = 0; i < _fighters.Count; i++ )
            {
                Console.Write ( i + 1 + "- " );
                _fighters [ i ].ShowStats ();
            }
        }
    }

    class Fighter
    {
        protected int Armor;

        public Fighter ( string name, int health, int damage, int armor )
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }

        public void TakeDamage ( int damage )
        {
            Health -= damage - Armor;
        }

        public void ShowStats ()
        {
            Console.WriteLine ( $"{Name} - {Health} хп, {Damage} урона, {Armor} брони." );
        }

        public void UseAnAttack ()
        {
            Random random = new Random ();
            int randomNumber;
            int number = 1;
            int maximumNumber;
            int minimumNumber;
            maximumNumber = 4;
            minimumNumber = 1;
            randomNumber = random.Next ( minimumNumber, maximumNumber );

            if ( number == randomNumber )
            {
                Skill ();
            }
        }

        public virtual void Skill () { }
    }
    class Warrior : Fighter
    {
        public Warrior ( string name, int health, int damage, int armor ) : base ( name, health, damage, armor ) { }

        public override void Skill ()
        {
            base.Skill ();
            int damageBuff = 10;
            int armorBuff = 10;
            Console.WriteLine ( $"{Name} Собралю всю свою ярость и увеличел урон на 10 едениц и увеличинает браню на 10" );
            Damage += damageBuff;
            Armor += armorBuff;
        }
    }

    class Paladin : Fighter
    {
        public Paladin ( string name, int health, int damage, int armor ) : base ( name, health, damage, armor ) { }

        public override void Skill ()
        {
            base.Skill ();
            int healthBuff = Health / 100 * 40;
            Console.WriteLine ( $"{Name} Скорбит над падшими и отхиливается на 40%" );
            Health += healthBuff;
        }
    }

    class Hunter : Fighter
    {
        public Hunter ( string name, int health, int damage, int armor ) : base ( name, health, damage, armor ) { }

        public override void Skill ()
        {
            base.Skill ();
            int damageBuff = 15;
            Console.WriteLine ( $"{Name} Призвал спутника и сливается с ним воедино урон увеличен на 15 " );
            Damage += damageBuff;
        }
    }

    class Berserck : Fighter
    {
        public Berserck ( string name, int health, int damage, int armor ) : base ( name, health, damage, armor ) { }

        public override void Skill ()
        {
            base.Skill ();
            int damageBuff = Damage * 2;
            int armorDebaff = 20;
            Console.WriteLine ( $"{Name} Впадает в ярость: уменьшает браню на 20 и увеличивает урон х2" );
            Damage += damageBuff;
            Armor -= armorDebaff;
        }
    }

    class Magician : Fighter
    {
        public Magician ( string name, int health, int damage, int armor ) : base ( name, health, damage, armor ) { }

        public override void Skill ()
        {
            base.Skill ();
            int damageBuff = 100;
            Console.WriteLine ( $"{Name} Кидает глыбу огня и наносит 100 урона" );
            TakeDamage ( damageBuff );
        }
    }
}
/*Задача:
Создать 5 бойцов, пользователь выбирает 2 бойцов и они сражаются друг с другом до смерти. 
У каждого бойца могут быть свои статы.
Каждый игрок должен иметь особую способность для атаки, которая свойственна только его классу!*/