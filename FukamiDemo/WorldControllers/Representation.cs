using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldControllers
{
    public class Representation
    {
        
    #region Singleton
        private static volatile Representation _instance;
        private static object syncRoot = new Object();

        private Representation() { }

        public static Representation Instance
        {
            get 
            {
                if (_instance == null) 
                {
                lock (syncRoot) 
                {
                    if (_instance == null)
                        _instance = new Representation();
                }
                }

                return _instance;
            }
        }
	#endregion

    }
}
