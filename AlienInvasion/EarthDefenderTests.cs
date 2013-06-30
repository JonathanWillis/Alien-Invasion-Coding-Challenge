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

        [Test]
        public void WhenRomeIsBeingInvaded()
        {
            var random = new Random();
            var weapons = new List<IDefenceWeapon>
			{
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster),
			    WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster)
			};

            var defender = new EarthDefender();

            var weaponsUsedInPreviousWave = new List<IDefenceWeapon>();
            for (var wave = 1; wave <= 25; wave++)
            {
                var numberOfInvaders = random.Next(5) + 1;
                var invasionWave = CreateInvasionWave(numberOfInvaders, weapons);
                var defenceStrategy = defender.DefendEarth(invasionWave);

                Assert.That(defenceStrategy.WeaponsToFireAtThisWave.Count(), Is.EqualTo(numberOfInvaders));
                CollectionAssert.IsNotSubsetOf(defenceStrategy.WeaponsToFireAtThisWave, weaponsUsedInPreviousWave);
                weaponsUsedInPreviousWave = new List<IDefenceWeapon>(defenceStrategy.WeaponsToFireAtThisWave);
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
            var weapons = new List<IDefenceWeapon> { WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster) };
            var subject = new Armory(weapons);
            CollectionAssert.AreEquivalent(subject.Weapons(), weapons);
        }

        [Test]
        public void GetWeaponFromArmory()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster);
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
            var defenceWeapon1 = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster);
            var defenceWeapon2 = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster);

            var weapons = new List<IDefenceWeapon> { defenceWeapon1, defenceWeapon2 };
            var subject = new Armory(weapons);
            var firstWeapon = subject.GetWeapon();
            var secondWeapon = subject.GetWeapon();
            Assert.That(firstWeapon,Is.Not.SameAs(secondWeapon));
        }

        [Test]
        public void AfterGettingAWeaponItIsStillInTheArmoryCollection()
        {
            var weapons = new List<IDefenceWeapon> { WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster) };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            CollectionAssert.AreEquivalent(subject.Weapons(), weapons);
        }

        [Test]
        public void ReloadUsedWeaponsMakesThemAvailable()
        {
            var weapons = new List<IDefenceWeapon> { WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster) };
            var subject = new Armory(weapons);
            var result1 = subject.GetWeapon();
            subject.ReloadWeapons();
            var result2 = subject.GetWeapon();
            Assert.That(result1, Is.SameAs(result2));
        }

        [Test]
        public void AfterReloadingTheArmoryContainsTheSameWeapons()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            subject.ReloadWeapons();
            CollectionAssert.AreEquivalent(subject.Weapons(), weapons);
        }

        [Test]
        public void UsingAPeashooter500ItIsNotAvailableAfterOneReload()
        {
            var weapons = new List<IDefenceWeapon> { WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster) };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            subject.ReloadWeapons();
            Assert.Throws<NoWeaponAvailableException>(() => subject.GetWeapon());
        }

        [Test]
        public void UsingAPeashooter500ItIsAvailableAfterASecondReload()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var subject = new Armory(weapons);
            subject.GetWeapon();
            subject.ReloadWeapons();
            subject.ReloadWeapons();
            var result = subject.GetWeapon();
            Assert.That(result, Is.SameAs(defenceWeapon));
        }
    }

    [TestFixture]
    public class CommandCentreTests
    {
        [Test]
        public void GivenAnAlienInvasionWhenNoArmoryExistsANewArmoryIsReturned()
        {
            var alienInvasionWave = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWave.Stub(x => x.WeaponsAvailableForDefence).Return(new IDefenceWeapon[0]);
            
            var subject = new CommandCentre();
            var result = subject.GetArmory(alienInvasionWave);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAnAlienInvasionThenTheReturnedArmoryContainsWeaponsForThatInvasion()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var alienInvasionWave = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWave.Stub(x => x.WeaponsAvailableForDefence).Return(weapons.ToArray());
            
            var subject = new CommandCentre();
            var result = subject.GetArmory(alienInvasionWave);
            CollectionAssert.AreEquivalent(result.Weapons(), weapons);
        }

        [Test]
        public void GivenACommandCentreWithAnArmoryWhenGettingTheArmoryForAnInvasionThenTheArmoryForThatInvasionIsReturned()
        {
            var defenceWeapon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weapons = new List<IDefenceWeapon> { defenceWeapon };
            var alienInvasionWave1 = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWave1.Stub(x => x.WeaponsAvailableForDefence).Return(weapons.ToArray());

            var subject = new CommandCentre();
            
            var result1 = subject.GetArmory(alienInvasionWave1);
            var result2 = subject.GetArmory(alienInvasionWave1);

            Assert.That(result1, Is.SameAs(result2));
        }

        [Test]
        public void GivenACommandCentreWithAnArmoryForALondonInvasionWhenARomeInvasionArmoryIsRequestThenANewArmoryForRomeIsReturned()
        {
            var defenceWeaponLondon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster);
            var weaponsLondon = new List<IDefenceWeapon> { defenceWeaponLondon };
            var alienInvasionWaveLondon = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWaveLondon.Stub(x => x.WeaponsAvailableForDefence).Return(weaponsLondon.ToArray());

            var defenceWeaponRome = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weaponsRome = new List<IDefenceWeapon> { defenceWeaponRome };
            var alienInvasionWaveRome = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWaveRome.Stub(x => x.WeaponsAvailableForDefence).Return(weaponsRome.ToArray());

            var subject = new CommandCentre();
            var resultLondon = subject.GetArmory(alienInvasionWaveLondon);
            var resultRome = subject.GetArmory(alienInvasionWaveRome);
            Assert.That(resultLondon, Is.Not.SameAs(resultRome));
        }

        [Test]
        public void GivenACommandCentreWithAnArmoryForLondonAndRomeInvasionsWhenALondonInvasionArmoryIsRequestedThenTheExistingArmoryIsReturned()
        {
            var defenceWeaponLondon = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter1000Blaster);
            var weaponsLondon = new List<IDefenceWeapon> { defenceWeaponLondon };
            var alienInvasionWaveLondon = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWaveLondon.Stub(x => x.WeaponsAvailableForDefence).Return(weaponsLondon.ToArray());

            var defenceWeaponRome = WeaponGenerator.CreateDefenceWeapon(DefenceWeaponType.Peashooter500Blaster);
            var weaponsRome = new List<IDefenceWeapon> { defenceWeaponRome };
            var alienInvasionWaveRome = MockRepository.GenerateStub<IAlienInvasionWave>();
            alienInvasionWaveRome.Stub(x => x.WeaponsAvailableForDefence).Return(weaponsRome.ToArray());

            var subject = new CommandCentre();
            var resultLondon1 = subject.GetArmory(alienInvasionWaveLondon);
            subject.GetArmory(alienInvasionWaveRome);
            var resultLondon2 = subject.GetArmory(alienInvasionWaveLondon);
            Assert.That(resultLondon1, Is.SameAs(resultLondon2));
        }
    }

    public class CommandCentre
    {
        private IDictionary<IDefenceWeapon[], IArmory> _armories = new Dictionary<IDefenceWeapon[], IArmory>();  

        public IArmory GetArmory(IAlienInvasionWave invasionWave)
        {
            var weapons = invasionWave.WeaponsAvailableForDefence;

            if (_armories.ContainsKey(weapons))
                return _armories[weapons];

            var armory = new Armory(weapons);                
            _armories.Add(weapons, armory);
            return armory;
        }
    }

    public class NoWeaponAvailableException : Exception
    {
        public NoWeaponAvailableException() : base("No weapons available")
        {
        }
    }

    public interface IArmory
    {
        IEnumerable<IDefenceWeapon> Weapons();
        IDefenceWeapon GetWeapon();
        void ReloadWeapons();
    }

    public class Armory : IArmory
    {
        private List<IDefenceWeapon> AvailableWeapons { get; set; }
        private List<IDefenceWeapon> OneReloadRemainingWeapons { get; set; }
        private List<IDefenceWeapon> TwoReloadRemainingWeapons { get; set; }

        public Armory() : this(new List<IDefenceWeapon>()) {}
        
        public Armory(IEnumerable<IDefenceWeapon> defenceWeapons)
        {
            AvailableWeapons = new List<IDefenceWeapon>(defenceWeapons);
            OneReloadRemainingWeapons = new List<IDefenceWeapon>();
            TwoReloadRemainingWeapons = new List<IDefenceWeapon>();
        }

        public IEnumerable<IDefenceWeapon> Weapons()
        {
            var defenceWeapons = new List<IDefenceWeapon>();
            defenceWeapons.AddRange(AvailableWeapons);
            defenceWeapons.AddRange(OneReloadRemainingWeapons);
            return defenceWeapons;
        }

        public IDefenceWeapon GetWeapon()
        {
            if (AvailableWeapons.Any())
            {
                var weapon = AvailableWeapons.First();
                AvailableWeapons.Remove(weapon);
                if(weapon.DefenceWeaponType == DefenceWeaponType.Peashooter500Blaster)
                    TwoReloadRemainingWeapons.Add(weapon);
                else
                    OneReloadRemainingWeapons.Add(weapon);
                return weapon;
            }
            throw new NoWeaponAvailableException();
        }

        public void ReloadWeapons()
        {
            AvailableWeapons.AddRange(OneReloadRemainingWeapons);
            OneReloadRemainingWeapons.Clear();
            OneReloadRemainingWeapons.AddRange(TwoReloadRemainingWeapons);
            TwoReloadRemainingWeapons.Clear();
        }
    }
}
