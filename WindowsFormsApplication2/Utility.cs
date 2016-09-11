using System.Configuration;

namespace WindowsFormsApplication2
{
    class Utility
    {
        //public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        //{
        //    MemberExpression me = propertyLambda.Body as MemberExpression;
        //    if (me == null)
        //    {
        //        throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
        //    }

        //    string result = string.Empty;
        //    do
        //    {
        //        result = me.Member.Name + "." + result;
        //        me = me.Expression as MemberExpression;
        //    } while (me != null);

        //    result = result.Remove(result.Length - 1); // remove the trailing "."
        //    return result;
        //}

        public static string[] GetVideoExtensions()
        {
            return new string[] {".dat", ".webm",".mkv",".flv",".vob",".ogv, ",".ogg"
                ,".drc",".gif",".gifv",".mng",".avi",".mov, ",".qt",".wmv"
                ,".yuv",".rm",".rmvb",".asf",".amv",".mp4, ",".m4p",".m4v"
                ,".mpg, ",".mp2", ".mp4",".mpeg",".mpe",".mpv",".mpg"
                ,".mpeg, ",".m2v",".m4v",".svi",".3gp",".3g2",".mxf"
                ,".roq",".nsv",".flv ",".f4v ",".f4p ",".f4a ",".f4b" };
        }

        public static string[] GetNonVideoExtensions()
        {
            string[] fileExtExcluded = new string[] { ".txt", ".nfo", ".db", ".jpg"
                , ".png", ".srt", ".rar", ".gif", ".sub" };
            return fileExtExcluded;
        }

        internal static void UpdateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
