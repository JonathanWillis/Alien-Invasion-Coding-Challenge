using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlienInvasion.Client;
using AlienInvasion.Client.AlienInvaders;
using AlienInvasion.Client.DefenceAssets;
using NUnit.Framework;
using Rhino.Mocks;

namespace AlienInvasion
{
    [TestFixture]
    public class EarthDefenderTests
    {
        [Test]
        public void WhenLondonIsBeingInvaded()
        {
            var random = new Random();
            var weapons = new List<IDefenceWeapon>
			              	{
			              		WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster),
			              		WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster),
			              		WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster),
			              		WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster),
			              		WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster)
			              	};

            var defender = new EarthDefender();

            for (var wave = 1; wave <= 20; wave++)
            {
                var numberOfInvaders = random.Next(5) + 1;
                var invasionWave = CreateInvasionWave(numberOfInvaders, weapons);
                var defenceStrategy = defender.DefendEarth(invasionWave);

                Assert.That(defenceStrategy.WeaponsToFireAtThisWave.Count(), Is.EqualTo(numberOfInvaders));
            }
        }

        private IAlienInvasionWave CreateInvasionWave(int numberOfInvaders, IEnumerable<IDefenceWeapon> weaponsAvailable)
        {
            var invaders = new List<IAlienInvader>();

            for (var i = 0; i < numberOfInvaders; i++)
            {
                var invader = MockRepository.GenerateStub<IAlienInvader>();
                invader.Stub(x => x.Size).Return(FlyingSaucerSize.Small);
                invaders.Add(invader);
            }

            var invasionWave = MockRepository.GenerateStub<IAlienInvasionWave>();
            invasionWave.Stub(x => x.WeaponsAvailableForDefence).Return(weaponsAvailable.ToArray());
            invasionWave.Stub(x => x.AlienInvaders).Return(invaders.ToArray());

            return invasionWave;
        }
    }

    public class WeaponGenerator
    {
        public static IDefenceWeapon CreateDefenceWeapon(DefenceWeaponType weaponType)
        {
            var weapon = MockRepository.GenerateStub<IDefenceWeapon>();
            weapon.Stub(x => x.DefenceWeaponType).Return(weaponType);
            return weapon;
        }
    }

    [TestFixture]
    public class ArmoryTests
    {
        [Test]
        public void NewArmoryHasNoWeapons()
        {
            var subject = new Armory();
            CollectionAssert.IsEmpty(subject.Weapons());
        }

        [Test]
        public void ArmoryCanContainWeapons()
        {
            var weapons = new List<IDefenceWeapon> { WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster) };
            var subject = new Armory(weapons);
            CollectionAssert.AreEquivalent(subject.Weapons(), weapons);
        }

        [Test]
        public void GetWeaponFromArmory()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var subject = new Armory(weapons);
            var result = subject.GetWeapon();
            Assert.That(result, Is.SameAs(defenceWeapon));
        }

        [Test]
        public void ThrowExceptionIfNoWeaponIsInArmory()
        {
            var subject = new Armory();
            Assert.Throws<NoWeaponAvailableException>(() => subject.GetWeapon());
        }

        [Test]
        public void CannotGetTheSameWeaponOutOfTheArmory()
        {
            var defenceWeapon1 = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var defenceWeapon2 = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);

            var weapons = new List<IDefenceWeapon> { defenceWeapon1, defenceWeapon2 };
            var subject = new Armory(weapons);
            var firstWeapon = subject.GetWeapon();
            var secondWeapon = subject.GetWeapon();
            Assert.That(firstWeapon,Is.Not.SameAs(secondWeapon));
        }

        [Test]
        public void AfterGettingAWeaponItIsStillInTheArmoryCollection()
        {
            var weapons = new List<IDefenceWeapon> { WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster) };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            CollectionAssert.AreEquivalent(subject.Weapons(), weapons);
        }

        [Test]
        public void ReloadUsedWeaponsMakesThemAvailable()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            subject.ReloadWeapons();
            var result = subject.GetWeapon();
            Assert.That(result, Is.SameAs(defenceWeapon));
        }

        [Test]
        public void AfterReloadingTheArmoryContainsTheSameWeapons()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            subject.ReloadWeapons();
            CollectionAssert.AreEquivalent(subject.Weapons(), weapons);
        }
    }

    public class NoWeaponAvailableException : Exception
    {
        public NoWeaponAvailableException() : base("No weapons available")
        {
        }
    }

    public class Armory
    {
        private List<IDefenceWeapon> AvailableWeapons { get; set; }
        private List<IDefenceWeapon> UsedWeapons { get; set; }

        public Armory() : this(new List<IDefenceWeapon>()) {}
        
        public Armory(IEnumerable<IDefenceWeapon> defenceWeapons)
        {
            AvailableWeapons = new List<IDefenceWeapon>(defenceWeapons);
            UsedWeapons = new List<IDefenceWeapon>();
        }

        public IEnumerable<IDefenceWeapon> Weapons()
        {
            var defenceWeapons = new List<IDefenceWeapon>();
            defenceWeapons.AddRange(AvailableWeapons);
            defenceWeapons.AddRange(UsedWeapons);
            return defenceWeapons;
        }

        public IDefenceWeapon GetWeapon()
        {
            if (AvailableWeapons.Any())
            {
                var weapon = AvailableWeapons.First();
                AvailableWeapons.Remove(weapon);
                UsedWeapons.Add(weapon);
                return weapon;
            }
            throw new NoWeaponAvailableException();
        }

        public void ReloadWeapons()
        {
            AvailableWeapons.AddRange(UsedWeapons);
            UsedWeapons.Clear();
        }
    }
}
