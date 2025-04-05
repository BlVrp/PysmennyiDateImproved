using Pysmennyi02.Models.Exceptions;
using Pysmennyi02.Models.ZodiacSigns;

using System.Text.RegularExpressions;


namespace Pysmennyi02.Models
{
    public class Person
    {
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate;

        private readonly bool _isAdult;
        private readonly WesternZodiacSign _sunSign;
        private readonly ChineseZodiacSign _chineseSign;
        private readonly bool _isBirthday;

        public string Name { get { return _name; } set { _name = value; } }
        public string Surname { get { return _surname; } set { _surname = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public DateTime BirthDate { get { return _birthDate; } set { _birthDate = value; } }


        public bool? IsAdult { get { return _isAdult; } }
        public WesternZodiacSign? SunSign { get { return _sunSign; } }
        public ChineseZodiacSign? ChineseSign { get { return _chineseSign; } }
        public bool? IsBirthday { get { return _isBirthday; } }


        public Person(string name, string surname, string email, DateTime birthDate)
        {
            ValidateBirthDate(birthDate);
            ValidateEmail(email);
            ValidateName(name);
            ValidateSurname(surname);

            _name = name;
            _surname = surname;
            _email = email;
            _birthDate = birthDate;

            Thread.Sleep(1000);
            _isAdult = getIsAdult(birthDate);
            _sunSign = GetWesternZodiacSign(birthDate);
            _chineseSign = GetChineseZodiacSign(birthDate);
            _isBirthday = getIsBirthdayToday(birthDate);
        }

        public Person(string name, string surname, string email) : this(name, surname, email, DateTime.Now)
        {
        }

        public Person(string name, string surname, DateTime birthDate) : this(name, surname, "dummy@gmail.com", birthDate)
        {
        }

        private void  ValidateBirthDate(DateTime birthDate)
        {
            var now = DateTime.Now;
            if (birthDate > now)
            {
                throw new PersonNotBornException(birthDate, now);
            }

            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            if (age > 135)
            {
                var closestValidBirthdate = now.AddYears(-135);
                throw new PersonTooOldException(birthDate, closestValidBirthdate);
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException(email);
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            bool match = Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);

            if (!match)
            {
                throw new InvalidEmailException(email);
            }

        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name cannot be empty");
            }
        }

        private void ValidateSurname(string surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentException("surname cannot be empty");
            }
        }



        private WesternZodiacSign GetWesternZodiacSign(DateTime birthDate)
        {
            foreach (var sign in ZodiacRepository.WesternZodiacSigns)
            {
                if (sign.IsDateInRange(birthDate))
                {
                    return sign;
                }
            }
            throw new Exception("No western zodiac sign found");
        }

        private ChineseZodiacSign GetChineseZodiacSign(DateTime birthDate)
        {
            try
            {
                int year = birthDate.Year;
                var lunarNewYearForYear = ChineseNewYearsRepository.Dates.Where(x => x.Year == year).First();
                if (birthDate < lunarNewYearForYear)
                {
                    year--;
                }
                int remainder = (year - 4) % 12;
                return ZodiacRepository.ChineseZodiacSigns[remainder];
            }
            catch (Exception)
            {
                throw new Exception("No chinese zodiac sign found");
            }
        }

        private bool getIsAdult(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }
            return age >= 18;
        }

        private bool getIsBirthdayToday(DateTime birthDate)
        {
            return birthDate.Day == DateTime.Now.Day && birthDate.Month == DateTime.Now.Month;
        }
    }
}
