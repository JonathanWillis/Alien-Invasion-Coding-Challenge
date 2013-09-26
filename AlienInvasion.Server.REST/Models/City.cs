using System;
using System.Collections.Generic;

namespace AlienInvasion.Server.REST.Models
{
    public interface ICity
    {
        string Name { get; }
        string Briefing { get; }
        string Id { get; }
        IEnumerable<IWeapon> Weapons { get; }
        IEnumerable<InvasionWave> GenerateInvasionWaves();
    }

    public interface IWeapon
    {
        string Id { get; }
        string Type { get; }
    }

    public class PeaShooter1000 : IWeapon
    {
        public string Id { get; private set; }
        public string Type { get; private set; }

        public PeaShooter1000()
        {
            Id = Guid.NewGuid().ToString();
            Type = "Pea Shooter 1000";
        }
    }

    public class InvasionWave
    {
        private readonly ICity _city;
        private readonly IEnumerable<IAlienInvader> _alienInvaders;
        private readonly IEnumerable<IWeapon> _weapons;

        public InvasionWave(ICity city, IEnumerable<IAlienInvader> alienInvaders, IEnumerable<IWeapon> weapons)
        {
            _city = city;
            _alienInvaders = alienInvaders;
            _weapons = weapons;
        }
    }

    public interface IAlienInvader
    {
    }

    public class SmallFlyingSaucer : IAlienInvader
    {
    }

    public class London : ICity
    {
        private static int _cityId = 1;
        private const int MaxAliensPerWave = 5;  
        private const int NumberOfWaves = 5;  
        public string Name { get; private set; }
        public string Briefing { get; private set; }
        public string Id { get; private set; }
        public IEnumerable<IWeapon> Weapons { get; private set; }

        public London()
        {
            Id = NextCityId();
            Name = CityName.London;
            Briefing = "London Briefing";
            var weapons = new List<IWeapon> { new PeaShooter1000(), new PeaShooter1000(), new PeaShooter1000(), new PeaShooter1000(), new PeaShooter1000() };
            Weapons = weapons;
        }

        public IEnumerable<InvasionWave> GenerateInvasionWaves()
        {
            var waves = new List<InvasionWave>();

            var random = new Random();
            for (int wave = 0; wave < NumberOfWaves; wave++)
            {
                int numberOfInvaders = random.Next(MaxAliensPerWave) + 1;
                var invaders = new List<IAlienInvader>();
                for (int invader = 0; invader < numberOfInvaders; invader++)
                    invaders.Add(new SmallFlyingSaucer());

                waves.Add(new InvasionWave(this, invaders.ToArray(), Weapons));
            }
            return waves;
        }

        private static string NextCityId()
        {
            return _cityId++.ToString();
        }
    }

    public class City
    {
        public string Name { get; set; }
        public string Briefing { get; set; }

        private City(string name, string briefing)
        {
            Name = name;
            Briefing = briefing;
        }

        public static City London()
        {
            return new City(CityName.London,"The London Briefing");
        }

        public static City Rome()
        {
            return new City(CityName.Rome, "The Rome Briefing");
        }
    }
}