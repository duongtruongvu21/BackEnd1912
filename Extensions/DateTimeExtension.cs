namespace DatingApp.API.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalAge(this DateTime dateOfBirth){
            var today = DateTime.Now;
            var age = today.Year - dateOfBirth.Year;
            if(dateOfBirth.Date > today.AddYears(-age)) {}
            return age;
        }
    }
}