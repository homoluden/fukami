using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldControllers
{
    public class Will
    {


    #region Singleton
        private static volatile Will _instance;
        private static object syncRoot = new Object();

        private Will() { }

        public static Will Instance
        {
            get 
            {
                if (_instance == null) 
                {
                lock (syncRoot) 
                {
                    if (_instance == null)
                        _instance = new Will();
                }
                }

                return _instance;
            }
        }
	#endregion
        
    }
}
